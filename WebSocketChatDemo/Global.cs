using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using OpcDa.Client;

namespace WebSocketOpc
{
    public class Global:PointDepot
    {
        private static PointDepot PointDataDepot= new PointDepot();
        private static OPC MyOPC = new OPC();
        private Hashtable MyOPCServers = new Hashtable();
        // IsNumeric Function
        static bool IsNumeric(object Expression)
        {
            // Variable to collect the Return value of the TryParse method.
            bool isNum;

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public Global()
        {
            DB MyDB = new DB();
            IList ll = MyDB.ReaderToList("SELECT * FROM OPC_Servers ");
            foreach (Hashtable tmp in ll)
            {
                OPC tmpOPC = new OPC();
                tmpOPC.ConnectOPCSerevr(tmp["OPCServerName"].ToString(), tmp["OPCServerNode"].ToString());
                MyOPCServers.Add(tmp["OPCServerID"].ToString(), tmpOPC);
            }
            //PointDataDepot = new PointDepot();
            PointDataDepot.CreateDepot();
        }

   //     public void  GetOPCServer( )
    //    {
          //  return (OPC)MyOPC[ServerID];
 //       }

        public long DisconnectOPCServer()
        {
            foreach (OPC oo in MyOPCServers)
            {
                oo.DisconnectOPCSerevr();
            }
            return 1;
        }

        public OpcGroup FindGroupByName(string GroupName)
        {
            return (OpcGroup)MyOPC.FindGroupByName(GroupName);
        }
    }
}
