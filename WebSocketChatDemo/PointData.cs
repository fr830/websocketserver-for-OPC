using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpcDa.Client;

namespace WebSocketOpc
{    //数据块定义
    public class PointData : ICloneable 
    {
        private string _PointName;
        private string _PointAlias;
        private object _Value;
        private object _OutValue;
        private string _StationName;
        private string _Tag;
        private string _OPCGrpName;
        private string _OPCServerID;
        private string _AddrTag;
        private object _ClientID;

        private ItemValueResult _OPCItemValue;

        public string PointName
        {
            get 
            { 
                return _PointName; 
            }
            set 
            { 
                _PointName=value; 
            }
        }

        public string PointAlias
        {
            get 
            { 
                return _PointAlias; 
            }
            set 
            { 
                _PointAlias=value; 
            }
        }

        public object Value
        {
            get
            {
                if (_OPCItemValue != null)
                {
                    if (_OPCItemValue.Quality.ToString()=="good" )
                    {
                        if (_OPCItemValue.Value != null)
                        { _Value = _OPCItemValue.Value; }
                    }
                    else
                    {
                        _Value = "---";
                    }
                }
                else
                {
                    _Value = "null";
                }
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }

        public object OutValue
        {
            get
            {
                return _OutValue;
            }
            set
            {
                _OutValue = value;
            }
        }

        public string StationName
        {
            get
            {
                return _StationName;
            }
            set
            {
                _StationName = value;
            }
        }

        public string NameKey
        {
            get
            {
                return _StationName + "." + _PointName;
            }
        }
        public string AliasKey
        {
            get
            {
                return _StationName + "." + _PointAlias;
            }
        }
        public string Tag
        {
            get
            {
                return _Tag;
            }
            set
            {
                _Tag = value;
            }
        }

        public string OPCServerID
        {
            get
            {
                return _OPCServerID;
            }
            set
            {
                _OPCServerID = value;
            }
        }

        public string OPCGrpName
        {
            get
            {
                return _OPCGrpName;
            }
            set
            {
                _OPCGrpName = value;
            }
        }

        public string AddrTag
        {
            get
            {
                return _AddrTag;
            }
            set
            {
                _AddrTag = value;
            }
        }

        public object ClientID
        {
            get
            {
                return _ClientID;
            }
            set
            {
                _ClientID = value;
            }
        }
        public ItemValueResult OPCItemValue
        {
            get
            {
                return _OPCItemValue;
            }
            set
            {
                _OPCItemValue = value;
            }
        }

        #region ICloneable Members
        /// <summary>
        /// Creates a shallow copy of the object.
        /// </summary>
        public virtual object Clone() { return MemberwiseClone(); }
        #endregion
        
    }
}
