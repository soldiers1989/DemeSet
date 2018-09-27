using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;

namespace ZF.Infrastructure.VFP
{
    public class BaseVFP : IDisposable
    {
        /// <summary>
        /// It's store the error message
        /// </summary>
        string _errorMessage;
        /// <summary>
        ///It's store the Exception 
        /// </summary>
        System.Exception _exception;
        /// <summary>
        /// It is marked run state.
        /// </summary>
        int result;


        private OdbcConnection m_Connection_Odbc = null;
        private OdbcCommand m_Command_Odbc = null;
        private OdbcDataAdapter m_Adapter_Odbc = null;

        private OleDbConnection m_Connection = null;
        private OleDbCommand m_Command = null;
        private OleDbDataAdapter m_Adapter = null;

        private string _ConnectionString = null;

        private string ODBC_Type = "0";

        /// <summary>
        /// 初始化连接参数
        /// </summary>
        /// <param name="dataPath"></param>
        public BaseVFP(string dataPath)
        {
            try
            {
                //vfp 6.0
                this._ConnectionString = @"Provider=MSDASQL;DRIVER=Microsoft Visual FoxPro Driver;UID=;Deleted=yes;Null=no;Collate=Machine;BackgroundFetch=no;Exclusive=No;SourceType=DBF;SourceDB=" + dataPath;
                m_Connection_Odbc = new OdbcConnection(_ConnectionString);
                this.m_Connection_Odbc.Open();
                ODBC_Type = "0";
            }
            catch
            {
                try
                {
                    //vfp 9.0
                    this.m_Connection_Odbc.Dispose();
                    this._ConnectionString = @"Provider=VFPOLEDB.1;Collating Sequence=MACHINE;Data Source=" + dataPath;
                    this.m_Connection = new OleDbConnection();
                    this.m_Connection.ConnectionString = this._ConnectionString;
                    this.m_Connection.Open();
                    ODBC_Type = "1";
                }
                catch
                {
                    this.m_Connection.Dispose();
                    throw new Exception("未安装VFP！");
                }
            }
            finally
            {
                if (this.m_Connection != null)
                {
                    if (this.m_Connection.State == ConnectionState.Open)
                    {
                        this.m_Connection.Close();
                    }
                }
                if (m_Connection_Odbc != null)
                {
                    if (this.m_Connection_Odbc.State == ConnectionState.Open)
                    {
                        this.m_Connection_Odbc.Close();
                    }
                }
            }

        }
        /// <summary>
        /// get or set ErrorMessage
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        /// <summary>
        /// get Result
        /// </summary>
        public int Result
        {
            get { return result; }
        }

        /// <summary>
        /// get or set Exception.
        /// </summary>
        public Exception Exception
        {
            get { return _exception; }
            set { _exception = value; }

        }
        /// <summary>
        /// get or set ConnectionString
        /// </summary>
        public string ConnectionString
        {
            get { return this._ConnectionString; }
            set { this._ConnectionString = value; }
        }

        /// <summary>
        /// 从VFP表中导入数据到Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dataSet"></param>
        public void SelectInfo(string sql, ref DataSet dataSet)
        {
            if (ODBC_Type.Equals("1"))
            {
                using (m_Command = new OleDbCommand(sql, m_Connection))
                {
                    try
                    {
                        m_Command.CommandType = CommandType.Text;
                        m_Connection.Open();
                        m_Adapter = new OleDbDataAdapter(m_Command);
                        m_Adapter.Fill(dataSet);
                    }
                    catch (Exception myEx)
                    {
                        this.ErrorMessage = myEx.Message;
                        this.Exception = myEx;
                        result = -1;
                        throw this.Exception;
                    }
                    finally
                    {
                        m_Connection.Close();
                    }
                }
            }
            else
            {
                using (m_Command_Odbc = new OdbcCommand(sql, m_Connection_Odbc))
                {
                    try
                    {
                        m_Command_Odbc.CommandType = CommandType.Text;
                        m_Connection_Odbc.Open();
                        m_Adapter_Odbc = new OdbcDataAdapter(m_Command_Odbc);
                        m_Adapter_Odbc.Fill(dataSet);
                    }
                    catch (Exception myEx)
                    {
                        this.ErrorMessage = myEx.Message;
                        this.Exception = myEx;
                        result = -1;
                        throw this.Exception;
                    }
                    finally
                    {
                        m_Connection_Odbc.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 从VFP表中导入数据到Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dataSet"></param>
        public DataSet SelectInfo(string sql)
        {
            if (ODBC_Type.Equals("1"))
            {
                using (m_Command = new OleDbCommand(sql, m_Connection))
                {
                    try
                    {
                        DataSet ds = new DataSet();
                        m_Command.CommandType = CommandType.Text;
                        m_Connection.Open();
                        m_Adapter = new OleDbDataAdapter(m_Command);
                        m_Adapter.Fill(ds);
                        return ds;
                    }
                    catch (Exception myEx)
                    {
                        this.ErrorMessage = myEx.Message;
                        this.Exception = myEx;
                        result = -1;
                        throw this.Exception;
                    }
                    finally
                    {
                        m_Connection.Close();
                    }
                }
            }
            else
            {
                using (m_Command_Odbc = new OdbcCommand(sql, m_Connection_Odbc))
                {
                    try
                    {
                        DataSet ds = new DataSet();
                        m_Command_Odbc.CommandType = CommandType.Text;
                        m_Connection_Odbc.Open();
                        m_Adapter_Odbc = new OdbcDataAdapter(m_Command_Odbc);
                        m_Adapter_Odbc.Fill(ds);
                        return ds;
                    }
                    catch (Exception myEx)
                    {
                        this.ErrorMessage = myEx.Message;
                        this.Exception = myEx;
                        result = -1;
                        throw this.Exception;
                    }
                    finally
                    {
                        m_Connection_Odbc.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 从Sql中导出数据到VFP
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool InsertInfo(string sql)
        {
            if (ODBC_Type.Equals("1"))
            {
                using (m_Command = new OleDbCommand(sql, m_Connection))
                {
                    try
                    {
                        m_Command.CommandType = CommandType.Text;
                        m_Connection.Open();
                        int n = m_Command.ExecuteNonQuery();
                        if (n > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception myEx)
                    {
                        this.ErrorMessage = myEx.Message;
                        this.Exception = myEx;
                        result = -1;
                        throw this.Exception;
                    }
                    finally
                    {
                        m_Connection.Close();
                    }
                }
            }
            else
            {
                using (m_Command_Odbc = new OdbcCommand(sql, m_Connection_Odbc))
                {
                    try
                    {
                        m_Command_Odbc.CommandType = CommandType.Text;
                        m_Connection_Odbc.Open();
                        int n = m_Command_Odbc.ExecuteNonQuery();
                        if (n > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception myEx)
                    {
                        this.ErrorMessage = myEx.Message;
                        this.Exception = myEx;
                        result = -1;
                        throw this.Exception;
                    }
                    finally
                    {
                        m_Connection_Odbc.Close();
                    }
                }
            }
        }


        #region 关闭连接
        public void CloseConnection()
        {
            if (this.m_Connection.State == System.Data.ConnectionState.Open)
            {
                this.m_Connection.Close();
            }
            if (this.m_Connection_Odbc.State == System.Data.ConnectionState.Open)
            {
                m_Connection_Odbc.Close();
            }
        }
        #endregion
        #region IDisposable 成员

        public void Dispose()
        {
            if (this.m_Connection == null)
                m_Connection.Dispose();
            else
                m_Connection = null;
            if (this.m_Connection_Odbc == null)
                m_Connection_Odbc.Dispose();
            else
                m_Connection_Odbc = null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        void IDisposable.Dispose()
        {
            //   确认连接是否已经关闭
            if (m_Connection != null)
            {
                if (m_Connection.State == ConnectionState.Open)
                {
                    m_Connection.Close();
                }
                m_Connection.Dispose();
                m_Connection = null;
            }
            if (m_Connection_Odbc != null)
            {
                if (m_Connection_Odbc.State == ConnectionState.Open)
                {
                    m_Connection_Odbc.Close();
                }
                m_Connection_Odbc.Dispose();
                m_Connection_Odbc = null;
            }
        }
        #endregion
    }
}