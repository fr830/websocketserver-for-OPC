using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperWebSocket;

namespace WebSocketOpc
{
    class Session
    {
        private WebSocketSession websocketsession;
        private bool heart=true;
        public WebSocketSession Websocketsession
        {
            get { return websocketsession; }
            set { websocketsession = value; }
        }
        public bool Heart
        {
            get { return heart; }
            set { heart = value; }
        }

    }
}
