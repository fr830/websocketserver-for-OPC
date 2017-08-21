using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpcDa.Client;
using System.Windows.Forms;

namespace WebSocketOpc
{
    class OPC 
    {
        private OpcServer _opcServer;
        public  void ConnectOPCSerevr(string strProgID,string strServer)
        {
            if (_opcServer != null)
            {
                _opcServer.Disconnect();
                _opcServer = null;
            }
            Type tp = Type.GetTypeFromProgID(strProgID, strServer);
            _opcServer = new OpcServer(tp.GUID.ToString(), strServer);
       
            try
            {
                _opcServer.Connect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateGroup()
        {
            if (_opcServer == null) return;
            try
            {
                OpcGroup grp = _opcServer.AddGroup("Fast", int.Parse("1000"), true);
                if (grp != null)
                {
                    //                    grp.DataChanged += OnDataChange;
                    grp.DataChanged += OnDataChange;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            try
            {
                OpcGroup grp = _opcServer.AddGroup("Normal", int.Parse("5000"), true);
                if (grp != null)
                {
                    //                    grp.DataChanged += OnDataChange;
                    grp.DataChanged += OnDataChange;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            try
            {
                OpcGroup grp = _opcServer.AddGroup("Slow", int.Parse("20000"), true);
                if (grp != null)
                {
                    //                    grp.DataChanged += OnDataChange;
                    grp.DataChanged += OnDataChange;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            try
            {
                OpcGroup grp = _opcServer.AddGroup("InActive", int.Parse("60000"), true);
                if (grp != null)
                {
                    //                    grp.DataChanged += OnDataChange;
                    grp.DataChanged += OnDataChange;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OnDataChange(object subscriptionHandle, object requestHandle, ItemValueResult[] values)
        {
//            DataChangedEventHandler _Handler = new DataChangedEventHandler(OnDataChange);
//            _Handler(subscriptionHandle, requestHandle, values);
            foreach (ItemValueResult IR in values)
            {
     //           Global.PointDataDepot.Item(IR.ClientHandle.ToString()).OPCItemValue = IR;
            }
            return;
        }

        public void AddItem(PointData _PointData)
        {
            List<string> items = new List<string>();
            items.Add(_PointData.AddrTag);
            OpcGroup grp = _opcServer.FindGroupByName(_PointData.OPCGrpName);
            if (grp != null)
            {
                try
                {
                    ItemResult[] results = grp.AddItems(items.ToArray());
                    foreach (ItemResult result in results)
                    {
                        if (result.ResultID.Failed())
                        {
                            string message = "Failed to add item \'" + result.ItemName + "\'" + " Error: " + result.ResultID.Name;
                            MessageBox.Show(message);
                        }
                        else
                        {
                            //                            AddItemToList(result);
//                            string message = "Success to add item \'" + result.ItemName ;
                            _PointData.ClientID = result.ClientHandle;
   //                         Global.PointDataDepot.AddClientID(_PointData.NameKey, _PointData.ClientID); 
//                            MessageBox.Show(message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;

                } // end catch
            }
        }
       
        public void DisconnectOPCSerevr()
        {
            if (_opcServer != null)
            {
                _opcServer.Disconnect();
                _opcServer = null;
            }
        }

        public OpcGroup FindGroupByName(string GroupName)
        {
            return (OpcGroup)_opcServer.FindGroupByName(GroupName);
        }

    }
}
