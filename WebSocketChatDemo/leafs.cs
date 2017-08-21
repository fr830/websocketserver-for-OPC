using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketOpc
{
    class Leafs
    {
        private Dictionary<string,Leaf> dictionary=new Dictionary<string, Leaf>();
        public Dictionary<string, Leaf> Dictionary
        {
            get { return dictionary; }
            set { dictionary = value; }
        }
    }
}
