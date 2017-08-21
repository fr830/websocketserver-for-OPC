using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using OpcDa.Client;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Web;
using SuperWebSocket;
namespace WebSocketOpc
{
    public class WebSocketOpc
    {
        public const float DBL_EPSILON = 0.000001f;
        public const float Zero = 0.0f;
        public const float Max = 999999990000f;
        public const float Min = -99999990000f;
        // private int[] ID;
        //  private int a;
        //  private string[] name;
        //  private Hashtable pointhash = new Hashtable();
        private Dictionary<string, string> pointhash = new Dictionary<string, string>();
        WebSocket boardcastWebsocket = new WebSocket();
        private OpcServer _opcServer;
        private ItemValue itemvalue = new ItemValue();
        //  private Type _type;
        private int _readhandle = 0;
        //  private int _writehandle = 0;
        private IRequest _readrequest = null;
        //     private IRequest _writerequest = null;
        // private Timer timer = null;
        System.Timers.Timer t0 = new System.Timers.Timer(60000);
        System.Timers.Timer t = new System.Timers.Timer(60000); //设置时间间隔为60秒
        Sessions sessions = new Sessions();
        Leafs leafs = new Leafs();
        public WebSocketOpc()
        {

        }

        public void Start()
        {
            boardcastWebsocket.Ws.NewSessionConnected += InitializeConnected;
            boardcastWebsocket.Ws.NewMessageReceived += ws_NewMessageReceived;
            boardcastWebsocket.Ws.SessionClosed += ws_SessionClosed;//有会话被关闭 可能是服务端关闭 也可能是客户端关闭
            boardcastWebsocket.Start();
            if (Connect())
            {
                Console.WriteLine("yyspeak: 连接opc服务成功");
                AddGroup();
                //timer = new Timer((data) =>
                //{
                //    Hashtable hash = new Hashtable();
                //    hash.Add(11, 1232);
                //    hash.Add(12, 12324);
                //    //  var msg = string.Format("服务器当前时间：{0:HH:MM:ss}", DateTime.Now);
                //  //  string msg = null;
                //    //对当前已连接的所有会话进行广播
                //    List<WebSocketSession> sessionlist = sessions.Dictionary.Keys.ToList();
                //    for (int i=0;i<sessionlist.Count;i++)
                //    {
                //        Session session = sessions.Dictionary[sessionlist[i]];
                //        if (sessions.Dictionary[sessionlist[i]].Heart == false)
                //        {       
                //            sessions.Dictionary.Remove(sessionlist[i]);
                //            session = null;
                //        }
                //        else
                //        sessions.Dictionary[sessionlist[i]].Heart = false;
                //    }

                //}, null, 1000000, 1000);

            }
            else
            {
                Console.WriteLine("yyspeak: 连接opc服务不成功，opc服务器设置");


            }
            Console.WriteLine("yyspeak: Press 'q' To Exit");
            while ('q' == Console.ReadKey().KeyChar)
            {
                //     simpleWebsocket.Stop();
                boardcastWebsocket.Stop();
                //    chatWebsocket.Stop();
            }
        }
        void InitializeConnected(WebSocketSession websocketsession)
        {
            Session session = new Session();
            session.Websocketsession = websocketsession;
            sessions.Dictionary.Add(websocketsession, session);
            Console.WriteLine("yyspeak: {0:HH:MM:ss}  与客户端:{1}创建新会话", DateTime.Now, websocketsession.RemoteEndPoint);
            //lock (pointhash)
            //{
            //    List<string> test = new List<string>(pointhash.Keys);
            //    for (int i = 0; i < test.Count; i++)
            //    {
            //        var msg = test[i] + "," + pointhash[test[i]];
            //        websocketsession.Send(msg);

            //    }
            //}
        }
        void ws_SessionClosed(WebSocketSession websocketsession, SuperSocket.SocketBase.CloseReason value)
        {
            sessions.Dictionary.Remove(websocketsession);
            Console.WriteLine("{0:HH:MM:ss}  与客户端:{1}的会话被关闭 原因：{2}", DateTime.Now, websocketsession.RemoteEndPoint, value);
        }
        void ws_NewMessageReceived(WebSocketSession websocketsession, string value)
        {
            Session session = sessions.Dictionary[websocketsession];
            string[] request = value.Split(':');
            string[] requestleaf = request[1].Split(',');
            if (sessions.Dictionary.ContainsKey(websocketsession) == false)
            {
                //Session session1 = new Session();
                session.Websocketsession = websocketsession;
                sessions.Dictionary.Add(websocketsession, session);
                Console.WriteLine("yyspeak: {0:HH:MM:ss}  与客户端:{1}创建新会话", DateTime.Now, websocketsession.RemoteEndPoint);
            }

            if (request[0] == "read")
            {

                for (int i = 0; i < requestleaf.Count(); i++)
                {
                    if (leafs.Dictionary.ContainsKey(requestleaf[i]))
                    {
                        if (leafs.Dictionary[requestleaf[i]].Sessionlist.Contains(sessions.Dictionary[websocketsession]) == false)
                        {
                            leafs.Dictionary[requestleaf[i]].sessionlistadd(ref session);
                            leafs.Dictionary[requestleaf[i]].setstream(leafs.Dictionary[requestleaf[i]].Opcvalue);
                        }
                        else
                            leafs.Dictionary[requestleaf[i]].setstream(leafs.Dictionary[requestleaf[i]].Opcvalue);
                    }
                    else
                    {
                        OpcGroup grp = _opcServer.FindGroupByName("Grp_0");
                        ItemResult[] itemresults = grp.AddItems(new string[] { requestleaf[i] });
                        if (itemresults[0].ResultID.Succeeded())
                        {
                            Leaf leaf = new Leaf();
                            leaf.Opckey = requestleaf[i];
                            leafs.Dictionary.Add(requestleaf[i], leaf);
                            leaf.sessionlistadd(ref session);
                            grp.AsyncRead(new object[] { itemresults[0].ClientHandle }, ++_readhandle, new ReadCompleteEventHandler(this.OnReadComplete), out _readrequest);
                        }
                    }
                }
            }
            else
                if (request[0] == "write")
            {
                for (int i = 0; i < requestleaf.Count(); i++)
                {
                    //string[] item = requestleaf[i].Split('-');
                    //_type = typeof(bool);
                    //itemvalue.QualitySpecified = false;
                    //itemvalue.TimestampSpecified = false;
                    //itemvalue.Value = OpcDa.Client.Com.Convert.ChangeType(item[1], _type);
                    //Console.WriteLine(itemvalue.ClientHandle.ToString()); ;
                    //OpcGroup grp = _opcServer.FindGroupByName("Grp_0");
                    //grp.AsyncWrite(new ItemValue[] { itemvalue }, ++_writehandle, new WriteCompleteEventHandler(this.OnWriteComplete), out _writerequest);
                    ////var msg = string.Format("{0:HH:MM:ss} {1}说: {2}", DateTime.Now, GetSessionName(session), value);
                    ////SendToAll(session, msg);
                    //Console.WriteLine("yyspeak: {0:HH:MM:ss} {1}: {2}", DateTime.Now, websocketsession.RemoteEndPoint, value);
                }
            }

            sessions.Dictionary[websocketsession].Heart = true;

        }
        private void OnReadComplete(object clientHandle, ItemValueResult[] results)
        {
            foreach (ItemValueResult value in results)
            {
                // item value should never have a null client handle.
                if (value.ClientHandle == null)
                {
                    continue;
                }

                if (value.ResultID.Failed())
                {
                    string message = "yyspeak: Failed to write item \'" + value.ItemName + "\' error: " + value.ResultID.ToString();
                    Console.WriteLine(message);
                }
                leafs.Dictionary[value.ItemName.ToString()].Opcvalue = value.Value.ToString();
                leafs.Dictionary[value.ItemName.ToString()].setstream(value.Value.ToString());
            }
        }
        private void OnWriteComplete(object clientHandle, IdentifiedResult[] results)
        {
            foreach (IdentifiedResult result in results)
            {
                // item value should never have a null client handle.
                if (result.ClientHandle == null)
                {
                    continue;
                }

                if (result.ResultID.Failed())
                {
                    string message = "yyspeak: Failed to write item \'" + result.ItemName + "\' error: " + result.ResultID.ToString();
                    Console.WriteLine(message);
                }
            }
        }

        private Boolean Connect()
        {


            if (this._opcServer != null)
            {
                this._opcServer.Disconnect();
                this._opcServer = null;
            }
            Type tp = Type.GetTypeFromProgID("KEPware.KEPServerEx.V4", "192.168.0.62");
            //   this._opcServer = new OpcServer("3c5702a2-eb8e-11d4-83a4-00105a984cbd", "10.25.61.188");
            this._opcServer = new OpcServer(tp.GUID.ToString(), "192.168.0.62");
            try
            {
                if (this._opcServer.Connect())
                    return true;
                else
                    return false;
            }
            catch
            {

                return false;
            }

        }

        private void AddGroup()
        {
            if (this._opcServer == null)
                return;

            try
            {
                OpcGroup grp = this._opcServer.AddGroup("Grp_0", int.Parse("1000"), true);
                if (grp != null)
                {
                    grp.DataChanged += new DataChangedEventHandler(subscription_DataChanged);
                    // grp.DataChanged += OnDataChange;

                }
            }
            catch
            {
                Console.WriteLine("yyspeak： 错误，创建opc服务对象不成功");
            }
        }


        void subscription_DataChanged(object subscriptionHandle, object requestHandle, ItemValueResult[] values)
        {

            foreach (ItemValueResult value in values)
            {
                // item value should never have a null client handle.
                if (value.ClientHandle == null)
                {
                    //    MessageBox.Show("ondatachange value null");
                    continue;
                }
                UpdateListViewItem(value);
            }
            // MessageBox.Show("sub begin");
        }

        private void UpdateListViewItem(ItemValueResult value)
        {

            {
                string us = value.Value.ToString();
                string pa = value.ItemName.ToString();
                if (leafs.Dictionary.ContainsKey(pa))
                {
                    Leaf leaf = new Leaf();
                    leaf = leafs.Dictionary[pa];
                    leaf.Opcvalue = us;
                    leaf.setstream(us);
                }

            }



        }
    }
}