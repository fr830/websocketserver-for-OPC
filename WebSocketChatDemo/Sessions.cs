using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperWebSocket;

namespace WebSocketOpc
{
    class Sessions
    {
        private Dictionary<WebSocketSession,Session> dictionary=new Dictionary<WebSocketSession, Session>();
        public Dictionary<WebSocketSession, Session> Dictionary
        {
            get { return dictionary; }
            set { dictionary = value; }
        }
        public void dictionaryadd(WebSocketSession websocketsession, Session session)
        {
            dictionary.Add(websocketsession, session);
        }

    }
}
