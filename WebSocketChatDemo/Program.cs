using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketOpc { 
    class Program
    {
        static void Main(string[] args)
        {
            // SimpleWebsocket simpleWebsocket = new SimpleWebsocket();
            WebSocketOpc websocketopc = new WebSocketOpc();
            // ChatWebSocket chatWebsocket = new ChatWebSocket();

            //  simpleWebsocket.Start();
            websocketopc.Start();
         //   chatWebsocket.Start();
         //   Console.WriteLine("Press 'q' To Exit");

           // while ('q' == Console.ReadKey().KeyChar)
           // {
           ////     simpleWebsocket.Stop();
           //     boardcastWebsocket.Stop();
           // //    chatWebsocket.Stop();
           // }
        }
    }
}
