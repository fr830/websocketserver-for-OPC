using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketOpc
{
    class Leaf
    {
        private string opckey;
        private string opcvalue;
        private string itemclientHander;
        private List<Session> sessionlist=new List<Session>(); 
        public string Opckey
        {
            get { return opckey; }
            set { opckey = value; }
        }
        public string Opcvalue
        {
            get { return opcvalue; }
            set { opcvalue = value; }
        }
        public string ItemclientHander
        {
            get { return itemclientHander; }
            set { itemclientHander = value; }
        }
        public List<Session> Sessionlist
        {
            get { return sessionlist; }
            set { sessionlist = value; }
        }
        public void sessionlistadd(ref Session session)
        {
            sessionlist.Add(session);
        }
        public void setstream(string opcvalue)
        {
            var msg = opckey + "," + opcvalue;
            for (int i = 0; i < sessionlist.Count(); i++)
            {
                if (sessionlist[i].Websocketsession.Connected)
                {
                    sessionlist[i].Websocketsession.Send(msg);                   
                }
                else
                sessionlist.Remove(sessionlist[i]);
            }
        }
    }
}
