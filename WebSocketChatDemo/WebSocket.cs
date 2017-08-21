using System;
using System.Threading;
using SuperWebSocket;
using System.Collections;
namespace WebSocketOpc
{
    /// <summary>
    /// 服务端向客户端进行广播
    /// </summary>
    public class WebSocket
    {
        //private const string ip = "127.0.0.1";
        private const string ip = "192.168.0.62";
        private const int port = 2656;
        private WebSocketServer ws = null;//SuperWebSocket中的WebSocketServer对象
        private Timer timer = null;


        public WebSocket()
        {
            ws = new WebSocketServer();//实例化WebSocketServer

            //添加事件侦听
          //  ws.NewSessionConnected += ws_NewSessionConnected;//有新会话握手并连接成功
            //ws.SessionClosed += ws_SessionClosed;//有会话被关闭 可能是服务端关闭 也可能是客户端关闭
          //  ws.NewMessageReceived += ws_NewMessageReceived;//有客户端发送新的消息
          
        }

        //void ws_NewSessionConnected(WebSocketSession session)
        //{
        //    Console.WriteLine("{0:HH:MM:ss}  与客户端:{1}创建新会话", DateTime.Now, session.RemoteEndPoint);

        //}

        //void ws_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        //{
        //    Console.WriteLine("{0:HH:MM:ss}  与客户端:{1}的会话被关闭 原因：{2}", DateTime.Now, session.RemoteEndPoint, value);
        //}
        //void ws_NewMessageReceived(WebSocketSession session, string value)
        //{

        //    //var msg = string.Format("{0:HH:MM:ss} {1}说: {2}", DateTime.Now, GetSessionName(session), value);

        //    //SendToAll(session, msg);
        //    Console.WriteLine("{0:HH:MM:ss} {1}说: {2}", DateTime.Now, session.RemoteEndPoint, value);

        //}
        public WebSocketServer Ws
        {
            get { return ws; }
            set { ws = value; }
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            if (!ws.Setup(ip, port))
            {
                Console.WriteLine("设置WebSocket服务侦听地址失败");
                return;
            }

            if (!ws.Start())
            {
                Console.WriteLine("启动WebSocket服务侦听失败");
                return;
            }

            Console.WriteLine("启动WebSocket服务成功");

            //timer = new Timer((data) =>
            //{
            //    Hashtable hash = new Hashtable();
            //    hash.Add(11,1232);
            //    hash.Add(12,12324);
            //  //  var msg = string.Format("服务器当前时间：{0:HH:MM:ss}", DateTime.Now);
            //    string msg = null;
            //    //对当前已连接的所有会话进行广播
            //    foreach (var session in ws.GetAllSessions())
            //    {
            //        session.Send(msg);
            //    }

            //}, null, 1000, 1000);


         
        }

        /// <summary>
        /// 停止侦听服务
        /// </summary>
        public void Stop()
        {
            if (timer != null)
            {
                timer.Dispose();
            }
            if (ws != null)
            {
                ws.Stop();
            }
        }
    }
}
