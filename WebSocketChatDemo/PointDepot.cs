using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace WebSocketOpc
{     //数据站定义
    public class PointDepot 
    {
        private Dictionary<string, int> _PointDepotIndex = new Dictionary<string, int>();
        private Collection<PointData> _PointDepot = new Collection<PointData>();
        private PointData _PointData;

        public Collection<PointData> GetDepot()
        {
            return _PointDepot;
        }
        #region 创建数据站
        public void CreateDepot()
        {
            
            PointData GetFromDepot;

            DB MyDB = new DB();
            DataTable DT = MyDB.OpenTableBySQL("SELECT * FROM PointDef Order By PointName");

            GetFromDepot = new PointData();
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                CreatePoint();
                GetFromDepot = ThisPointData;
                GetFromDepot.PointName = DT.Rows[i]["PointName"].ToString();
                GetFromDepot.PointAlias = DT.Rows[i]["PointAlias"].ToString();
                GetFromDepot.StationName = DT.Rows[i]["StationName"].ToString();
                GetFromDepot.OPCServerID = DT.Rows[i]["OPCServerID"].ToString();
                GetFromDepot.OPCGrpName = DT.Rows[i]["OPCGrpName"].ToString();
                GetFromDepot.AddrTag = DT.Rows[i]["OPCAddr"].ToString();
                GetFromDepot.Tag = DT.Rows[i]["Tag"].ToString();
                GetFromDepot.Value = (i + 1).ToString();
                Add();
            }
              
        }

        private void CreatePoint()
        {
            _PointData = new PointData(); 
        }

        private void Add()
        {
            try
            {
                _PointDepot.Add(_PointData);
                lock (_PointData)
                {
                    try
                    {
                        _PointDepotIndex.Add(_PointData.NameKey, _PointDepot.Count - 1);
                    }
                    catch (ArgumentException)
                    { }
                    try
                    {
                        _PointDepotIndex.Add(_PointData.AliasKey, _PointDepot.Count - 1);
                    }
                    catch (ArgumentException) { }
                }
            }
            finally
            {
            }
        }
        # endregion
        public void AddClientID(string key,object ClientID)
        {
            try
            {
                //PointData MyPoint = new PointData();
                PointData MyPoint = this.Item(key);
                lock (MyPoint)
                {
                    try
                    {
                        int i = _PointDepotIndex[MyPoint.NameKey];
                        _PointDepotIndex.Add(ClientID.ToString() ,i);
                    }
                    catch (ArgumentException)
                    { }
                }
            }
            finally
            {
            }
        }

        private void Add(ref PointData thisData)
        {
            _PointDepot.Add(thisData);
            _PointDepotIndex.Add(thisData.NameKey, _PointDepot.Count);
            _PointDepotIndex.Add(thisData.AliasKey, _PointDepot.Count);
        }

        public int Count
        {
            get { return _PointDepot.Count; }
        }

        public PointData ThisPointData
        {
            get { return _PointData; }
            set { _PointData = value; }
        }
        #region 数据站中的中数据块调用
        public PointData Item(string Key)
        {
            try
            {
               return _PointDepot[_PointDepotIndex[Key]];
            }
            catch (IndexOutOfRangeException)
            {
               throw new InvalidOperationException();
            }
        }
        #endregion
    }
}
