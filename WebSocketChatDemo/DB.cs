using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace WebSocketOpc
{     //读数据
    public class DB
    {
        //private OleDbConnection connection;

        private string _strConn;
        //public string Db_Server = "Data Source=.;Initial Catalog=PointSource;Integrated Security=True";
        public string Db_Server = "server=.;database=engadmin;uid=sa;pwd=th1313TH";

      //  public string Db_Server = "server=192.168.0.41;Initial Catalog=engadmin;User ID=sa;Password=th1313TH";
        //private OleDbDataAdapter adapter;

        public Hashtable Attributes()
        {
            Hashtable Res = new Hashtable();
            return Res;
        }

        public SqlConnection DbConn()
        {
            _strConn = Db_Server;
            SqlConnection connection = new SqlConnection(_strConn);
            return connection;
        }

        public DataTable OpenTableBySQL(string sqlString)
        {

            SqlConnection conn = DbConn();
            if (conn.State == ConnectionState.Connecting)
            {
                MessageBox.Show("chenggong");
            }
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sqlString;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;

            DataSet DS = new DataSet();
            adapter.SelectCommand = cmd;
            adapter.Fill(DS);
            conn.Close();
            cmd.Dispose();
            return DS.Tables[0];
        }

        public DataTable UpDateTableBySQL(string sqlString)
        {
            SqlConnection conn = DbConn();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sqlString;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;

            DataSet DS = new DataSet();
            adapter.SelectCommand = cmd;
            adapter.Fill(DS);
            conn.Close();
            cmd.Dispose();

            conn.Open();
            return DS.Tables[0];
        }
        private IList ReaderToList(SqlCommand cmd)
        {
            IList list = new ArrayList();
            SqlDataReader reader = null;

            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Hashtable rdata = new Hashtable();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        rdata.Add(reader.GetName(i), reader.GetValue(i));
                    }

                    list.Add(rdata);
                }

                reader.Close();
            }
            catch
            {
                //throw new Exception(ee.Message);
            }

            return list;
        }

        public IList ReaderToList(string strSQL)
        {
            SqlConnection conn = DbConn();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = strSQL;

            IList list = new ArrayList();
            SqlDataReader reader = null;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Hashtable rdata = new Hashtable();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        rdata.Add(reader.GetName(i), reader.GetValue(i));
                    }

                    list.Add(rdata);
                }

                reader.Close();
            }
            catch
            {
                //throw new Exception(ee.Message);
            }

            return list;
        }

        //通过SQL语句取得数据
        protected IList sqlQueryWithRes(string sql)
        {
            SqlConnection conn = DbConn();
            conn.Open();

            SqlCommand cmd = new SqlCommand(sql, conn);
            IList list = ReaderToList(cmd);

            cmd.Dispose();
            conn.Close();

            return list;
        }
        //哈希表读数据 
        public Hashtable findAllBySql(string sql)
        {
            Hashtable DataRes = new Hashtable();

            DataRes.Add("Attributes", Attributes());
            DataRes.Add("Data", sqlQueryWithRes(sql));

            return DataRes;
        }

    }
}
