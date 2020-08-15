using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using Entity;

namespace DataLayer
{
    internal delegate void SQLExecution(MySqlDataReader ResultSet);
    internal sealed class DataManager : IDisposable
    {
        public event SQLExecution SQLExecutionEvent = null;
        private MySqlCommand SqlCmd;
        private string strConnectionString;

        #region Constructor

        public DataManager()
        {
            SqlCmd = new MySqlCommand();

            //Decrypting connection string
            //UTF8Encoding encodepwd = new UTF8Encoding();
            //Decoder Decode = encodepwd.GetDecoder();
            //byte[] todecode_byte = Convert.FromBase64String(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
            //int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            //char[] decoded_char = new char[charCount];
            //Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            //strConnectionString = new String(decoded_char);

            strConnectionString = ConfigurationManager.ConnectionStrings["constr"].ToString();
        }
        ~DataManager()
        {
            Dispose(false);
        }

        #endregion

        #region Add Method
        /// <summary>
        /// Adds parameter name and value for the stored procedures. Parameter name will be same as found in stored procedures
        /// </summary>
        /// <param name="parameterName">Name of the parameter used in stored procedures</param>
        /// <param name="value">Value to be passed in stored procedures</param>
        /// <returns>Initializes a new instance of the SqlParameter class that uses the parameter name and a value of the new SqlParameter.</returns>
        public MySqlParameter Add(string parameterName, object value)
        {
            return this.Add(new MySqlParameter(parameterName, value));

        }
        /// <summary>
        /// Adds parameter name ,value and direction for the stored procedures. Parameter name will be same as found in stored procedures
        /// </summary>
        /// <param name="parameterName">Name of the parameter used in stored procedures</param>
        /// <param name="value">Value to be passed in stored procedures</param>
        /// <param name="direction">One of the value of ParameterDirection for the parameter</param>
        /// <returns>Initializes a new instance of the SqlParameter class that uses the parameter name and a value of the new SqlParameter.</returns>
        public MySqlParameter Add(string parameterName, object value, System.Data.ParameterDirection direction)
        {
            return this.Add(new MySqlParameter(parameterName, value), direction);
        }
        /// <summary>
        /// Adds parameter name, datatype and direction for the stored procedures. Parameter name will be same as found in stored procedures
        /// </summary>
        /// <param name="parameterName">Name of the parameter used in stored procedures</param>
        /// <param name="sqlDbType">One of the SqlDbType values used in stored procedures</param>
        /// <param name="direction">One of the ParameterDirection values</param>
        /// <returns>Initializes a new instance of the SqlParameter class that uses the parameter name and a value of the new SqlParameter.</returns>
        public MySqlParameter Add(string parameterName, System.Data.SqlDbType sqlDbType, System.Data.ParameterDirection direction)
        {
            return this.Add(new MySqlParameter(parameterName, sqlDbType), direction);
        }
        /// <summary>
        /// Adds parameter name, datatype and direction for the stored procedures. Parameter name will be same as found in stored procedures
        /// </summary>
        /// <param name="parameterName">Name of the parameter used in stored procedures</param>
        /// <param name="sqlDbType">One of the SqlDbType values used in stored procedures</param>
        /// <param name="direction">One of the ParameterDirection values</param>
        /// <param name="value">Value to be passed in stored procedures</param>
        /// <returns>Initializes a new instance of the SqlParameter class that uses the parameter name and a value of the new SqlParameter.</returns>
        public MySqlParameter Add(string parameterName, MySql.Data.MySqlClient.MySqlDbType sqlDbType, object value)
        {
            return this.Add(new MySqlParameter(parameterName, sqlDbType), ParameterDirection.Input, value);
        }
        /// <summary>
        /// Adds parameter name, datatype and direction for the stored procedures. Parameter name will be same as found in stored procedures
        /// </summary>
        /// <param name="parameterName">Name of the parameter used in stored procedures</param>
        /// <param name="sqlDbType">One of the SqlDbType values used in stored procedures</param>
        /// <param name="direction">One of the ParameterDirection values</param>
        /// <param name="value">Value to be passed in stored procedures</param>
        /// <returns>Initializes a new instance of the SqlParameter class that uses the parameter name and a value of the new SqlParameter.</returns>
        public MySqlParameter Add(string parameterName, MySql.Data.MySqlClient.MySqlDbType sqlDbType, System.Data.ParameterDirection direction, object value)
        {
            return this.Add(new MySqlParameter(parameterName, sqlDbType), direction, value);
        }
        /// <summary>
        /// Adds parameter name, datatype, size and direction for the stored procedures. Parameter name will be same as found in stored procedures
        /// </summary>
        /// <param name="parameterName">Name of the parameter used in stored procedures</param>
        /// <param name="sqlDbType">One of the SqlDbType values used in stored procedures<</param>
        /// <param name="size">The length of the column. </param>
        /// <param name="direction">One of the ParameterDirection values</param>
        /// <returns>Initializes a new instance of the SqlParameter class that uses the parameter name and a value of the new SqlParameter.</returns>
        public MySqlParameter Add(string parameterName, MySql.Data.MySqlClient.MySqlDbType sqlDbType, int size, System.Data.ParameterDirection direction)
        {
            return this.Add(new MySqlParameter(parameterName, sqlDbType, size), direction);
        }
        /// <summary>
        /// Adds parameter name, datatype, size and direction for the stored procedures. Parameter name will be same as found in stored procedures
        /// </summary>
        /// <param name="parameterName">Name of the parameter used in stored procedures</param>
        /// <param name="sqlDbType">One of the SqlDbType values used in stored procedures<</param>
        /// <param name="size">The length of the column. </param>
        /// <param name="direction">One of the ParameterDirection values</param>
        /// <param name="value">Value to be passed in stored procedures</param>
        /// <returns>Initializes a new instance of the SqlParameter class that uses the parameter name and a value of the new SqlParameter.</returns>
        public MySqlParameter Add(string parameterName, MySql.Data.MySqlClient.MySqlDbType sqlDbType, int size, object value)
        {
            return this.Add(new MySqlParameter(parameterName, sqlDbType, size), ParameterDirection.Input, value);
        }
        /// <summary>
        /// Adds parameter name, datatype, size and direction for the stored procedures. Parameter name will be same as found in stored procedures
        /// </summary>
        /// <param name="parameterName">Name of the parameter used in stored procedures</param>
        /// <param name="sqlDbType">One of the SqlDbType values used in stored procedures<</param>
        /// <param name="size">The length of the column. </param>
        /// <param name="direction">One of the ParameterDirection values</param>
        /// <param name="value">Value to be passed in stored procedures</param>
        /// <returns>Initializes a new instance of the SqlParameter class that uses the parameter name and a value of the new SqlParameter.</returns>
        public MySqlParameter Add(string parameterName, MySql.Data.MySqlClient.MySqlDbType sqlDbType, int size, System.Data.ParameterDirection direction, object value)
        {
            return this.Add(new MySqlParameter(parameterName, sqlDbType, size), direction, value);
        }
        /// <summary>
        ///  Adds SqlParameter object 
        /// </summary>
        /// <param name="SqlPar">The SqlParameter to add to the collection.</param>
        /// <returns>Initializes a new instance of the SqlParameter class that uses the parameter name and a value of the new SqlParameter.</returns>
        public MySqlParameter Add(MySqlParameter SqlPar)
        {
            SqlCmd.CommandType = CommandType.StoredProcedure;
            return SqlCmd.Parameters.Add(SqlPar);
        }
        public DataManager InsertPaging(DataManager sqlCommand, BaseEntity entity, long primaryId)
        {
            //int pageIndex = (entity.PageIndex < 0) ? 0 : entity.PageIndex; //Coming to next page row number

            //pageIndex = (primaryId > 0) ? 1 : pageIndex + 1; //For single record coming blank

            if (entity.PageIndex == 0)
                sqlCommand.Add("p_PageIndex", DBNull.Value);
            else
                sqlCommand.Add("p_PageIndex", entity.PageIndex);
            if (entity.PageSize == 0)
                sqlCommand.Add("p_PageSize", DBNull.Value);
            else
                sqlCommand.Add("p_PageSize", entity.PageSize);
            return sqlCommand;
        }

        /// <summary>
        /// Adds SqlParameter object 
        /// </summary>
        /// <param name="SqlPar">The SqlParameter to add to the collection.</param>
        /// <param name="direction">One of the value of ParameterDirection for the parameter</param>
        /// <returns>Initializes a new instance of the SqlParameter class that uses the parameter name and a value of the new SqlParameter.</returns>
        private MySqlParameter Add(MySqlParameter SqlPar, System.Data.ParameterDirection direction)
        {
            SqlCmd.CommandType = CommandType.StoredProcedure;
            SqlPar.Direction = direction;
            return SqlCmd.Parameters.Add(SqlPar);
        }
        /// <summary>
        /// Adds SqlParameter object 
        /// </summary>
        /// <param name="value">The SqlParameter to add to the collection.</param>
        /// <param name="direction">One of the value of ParameterDirection for the parameter</param>
        /// <param name="value">Value to be passed in stored procedures</param>
        /// <returns>Initializes a new instance of the SqlParameter class that uses the parameter name and a value of the new SqlParameter.</returns>
        private MySqlParameter Add(MySqlParameter SqlPar, System.Data.ParameterDirection direction, object value)
        {
            SqlCmd.CommandType = CommandType.StoredProcedure;
            SqlPar.Value = value;
            SqlPar.Direction = direction;
            return SqlCmd.Parameters.Add(SqlPar);
        }
        #endregion

        #region Clear Method
        /// <summary>
        /// Removes all Parameter added.
        /// </summary>
        public void Clear()
        {
            SqlCmd.Parameters.Clear();
            //mCommandType = CommandType.Text;
        }
        #endregion

        #region Contains
        /// <summary>
        /// Gets a value indicating whether a SqlParameter exists in the collection
        /// </summary>
        /// <param name="value">The value of the SqlParameter object to find</param>
        /// <returns>true if the collection contains the parameter; otherwise, false.</returns>
        public bool Contains(object value)
        {
            return SqlCmd.Parameters.Contains(value);
        }
        /// <summary>
        /// Gets a value indicating whether a SqlParameter with the specified parameter name exists in the collection.
        /// </summary>
        /// <param name="value">The name of the SqlParameter object to find. </param>
        /// <returns>true if the collection contains the parameter; otherwise, false.</returns>
        public bool Contains(string value)
        {
            return SqlCmd.Parameters.Contains(value);
        }
        #endregion

        #region GetEnumerator
        public System.Collections.IEnumerator GetEnumerator()
        {
            return SqlCmd.Parameters.GetEnumerator();
        }
        #endregion

        #region IndexOf Method
        /// <summary>
        /// Gets the location of a SqlParameter in the collection.
        /// </summary>
        /// <param name="value">The SqlParameter object to locate. </param>
        /// <returns>The zero-based location of the SqlParameter in the collection.</returns>
        public int IndexOf(object value)
        {
            return SqlCmd.Parameters.IndexOf(value);
        }
        /// <summary>
        /// Gets the location of the SqlParameter in the collection with a specific parameter name.
        /// </summary>
        /// <param name="parameterName">The name of the SqlParameter object to retrieve. </param>
        /// <returns>The zero-based location of the SqlParameter in the collection.</returns>
        public int IndexOf(string parameterName)
        {
            return SqlCmd.Parameters.IndexOf(parameterName);
        }
        #endregion

        #region Insert Method
        /// <summary>
        /// Inserts a SqlParameter into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index where the parameter is to be inserted within the collection.</param>
        /// <param name="value">The SqlParameter to add to the collection. </param>
        public void Insert(int index, object value)
        {
            SqlCmd.Parameters.Insert(index, value);
        }
        #endregion

        #region Remove Method
        /// <summary>
        /// Removes the specified SqlParameter from the collection.
        /// </summary>
        /// <param name="value">A SqlParameter object to remove from the collection. </param>
        public void Remove(object value)
        {
            //if (SqlCmd.Parameters.Count == 0)
            //    mCommandType = CommandType.Text;
            SqlCmd.Parameters.Remove(value);
        }
        #endregion

        #region RemoveAt Method
        /// <summary>
        /// Removes the specified SqlParameter from the collection using the parameter name.
        /// </summary>
        /// <param name="parameterName">The name of the SqlParameter object to retrieve.</param>
        public void RemoveAt(string parameterName)
        {
            //if (SqlCmd.Parameters.Count == 0)
            //    mCommandType = CommandType.Text;
            SqlCmd.Parameters.RemoveAt(parameterName);
        }
        /// <summary>
        /// Removes the specified SqlParameter from the collection using a specific index.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter.</param>
        public void RemoveAt(int index)
        {
            SqlCmd.Parameters.RemoveAt(index);
        }
        #endregion

        #region Count Property
        /// <summary>
        /// Gets the number of SqlParameter objects in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return SqlCmd.Parameters.Count;
            }
        }
        #endregion

        #region Indexers Property
        /// <summary>
        /// The parameters of the Transact-SQL statement or stored procedure. The default is an empty collection.
        /// </summary>
        public MySqlParameter this[int index]
        {
            get
            {
                return SqlCmd.Parameters[index];
            }
            set
            {
                SqlCmd.Parameters[index] = value;
            }

        }
        /// <summary>
        /// The parameters of the Transact-SQL statement or stored procedure. The default is an empty collection.
        /// </summary>
        public MySqlParameter this[string parameterName]
        {
            get
            {
                return SqlCmd.Parameters[parameterName];
            }
            set
            {
                SqlCmd.Parameters[parameterName] = value;
            }
        }
        #endregion

        #region Property public CommandType CommandType
        private CommandType mCommandType = CommandType.Text;
        public CommandType CommandType
        {
            get { return mCommandType; }
            set { mCommandType = value; }
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Releases the resources used by the Component.
        /// </summary>
        /// <param name="disposing">Send true value when Dispose Method is called by the program</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
            SqlCmd.Parameters.Clear();
            if (SqlCmd.Connection != null)
            {
                if (SqlCmd.Connection.State == System.Data.ConnectionState.Open)
                {
                    //try{
                    SqlCmd.Connection.Close();
                    //}catch{}
                }
            }
            //try{
            SqlCmd.Dispose();
            //}catch{}
        }
        /// <summary>
        /// Releases the resources used by the Component.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        /// <summary>
        /// Releases the resources used by the Component.
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion

        #region SQL Execution By Return Object
        private MySql.Data.MySqlClient.MySqlConnection PrepareExecution(string ExecuteString)
        {
            if (SqlCmd == null)
            {
                SqlCmd = new MySql.Data.MySqlClient.MySqlCommand();
            }
            if (SqlCmd.Connection == null)
            {
                SqlCmd.Connection = new MySql.Data.MySqlClient.MySqlConnection(strConnectionString);
            }
            else if (SqlCmd.Connection.ConnectionString.Length == 0)
            {
                SqlCmd.Connection.ConnectionString = strConnectionString;
            }
            if (SqlCmd.Connection.State != System.Data.ConnectionState.Open)
            {
                SqlCmd.Connection.Open();
            }
            if (SqlCmd.Parameters.Count > 0)
            {
                SqlCmd.CommandType = mCommandType;
            }
            else
            {
                SqlCmd.CommandType = mCommandType;
            }
            SqlCmd.CommandText = ExecuteString;
            //SqlCmd.Prepare();
            return SqlCmd.Connection;
        }
        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="ExecuteString">Executes the Transact-SQL statement or stored procedure against the connection.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(string ExecuteString)
        {
            using (PrepareExecution(ExecuteString))
            {
                return SqlCmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns a stream of (reading & forward-only) rows from a SQL Server database.
        /// </summary>
        /// <param name="ExecuteString">Executes the Transact-SQL statement or stored procedure against the connection.</param>
        /// <returns>A SqlDataReader object.</returns>
        public MySql.Data.MySqlClient.MySqlDataReader ExecuteReader(string ExecuteString)
        {
            PrepareExecution(ExecuteString);
            return SqlCmd.ExecuteReader();
        }
        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns a stream of (reading & forward-only) rows from a SQL Server database.
        /// </summary>
        /// <param name="ExecuteString">Executes the Transact-SQL statement or stored procedure against the connection.</param>
        /// <param name="behavior">One of the CommandBehavior values.</param>
        /// <returns>A SqlDataReader object.</returns>
        //public System.Data.SqlClient.SqlDataReader ExecuteReader(string ExecuteString, System.Data.CommandBehavior behavior)
        //{
        //    PrepareExecution(ExecuteString);
        //    return SqlCmd.ExecuteReader(behavior);

        //}
        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="ExecuteString">Executes the Transact-SQL statement or stored procedure against the connection.</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public object ExecuteScalar(string ExecuteString)
        {
            using (PrepareExecution(ExecuteString))
            {
                return SqlCmd.ExecuteScalar();
            }

        }
        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns provides a stream of XML data which is forward-only, read-only access.
        /// </summary>
        /// <param name="ExecuteString">Executes the Transact-SQL statement or stored procedure against the connection.</param>
        /// <returns>An XmlReader object.</returns>
        //public System.Xml.XmlReader ExecuteXmlReader(string ExecuteString)
        //{
        //    PrepareExecution(ExecuteString);
        //    return SqlCmd.ExecuteXmlReader();
        //}
        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns represents one table of in-memory data
        /// </summary>
        /// <param name="ExecuteString">Executes the Transact-SQL statement or stored procedure against the connection.</param>
        /// <returns>An DataTable object</returns>
        public System.Data.DataTable ExecuteDataTable(string ExecuteString)
        {
            using (PrepareExecution(ExecuteString))
            {
                using (MySql.Data.MySqlClient.MySqlDataAdapter SqlAdp = new MySql.Data.MySqlClient.MySqlDataAdapter(SqlCmd))
                {
                    System.Data.DataSet dataSet = new System.Data.DataSet();
                    SqlAdp.Fill(dataSet);
                    return dataSet.Tables[0];
                }
            }
        }

        public System.Data.DataSet GetDataSet(string ExecuteString, ref DataSet dataSet, string tblname)
        {
            using (PrepareExecution(ExecuteString))
            {
                using (MySql.Data.MySqlClient.MySqlDataAdapter SqlAdp = new MySql.Data.MySqlClient.MySqlDataAdapter(SqlCmd))
                {
                    //System.Data.DataSet dataSet = new System.Data.DataSet();
                    SqlAdp.Fill(dataSet, tblname);
                    return dataSet;
                }
            }
        }
        /// <summary>
        /// Executes a Transact-SQL statement against the connection and generate SQLExecutionEvent Event
        /// </summary>
        /// <param name="ExecuteString">Executes the Transact-SQL statement or stored procedure against the connection.</param>
        //public void ExecuteforEvent(string ExecuteString)
        //{
        //    using (PrepareExecution(ExecuteString))
        //    {
        //        if (SQLExecutionEvent != null)
        //        {
        //            using (MySql.Data.MySqlClient.MySqlDataReader SqlRea = SqlCmd.ExecuteReader())
        //            {
        //                SQLExecutionEvent(SqlRea);
        //            }
        //        }
        //    }
        //}
        /// <summary>
        /// Executes a Transact-SQL statement against the connection and generate SQLExecutionEvent Event
        /// </summary>
        /// <param name="ExecuteString">Executes the Transact-SQL statement or stored procedure against the connection.</param>
        /// <param name="behavior">One of the CommandBehavior values.</param>
        //public void ExecuteforEvent(string ExecuteString, System.Data.CommandBehavior behavior)
        //{
        //    PrepareExecution(ExecuteString);
        //    if (SQLExecutionEvent != null)
        //    {
        //        using (System.Data.SqlClient.SqlDataReader SqlRea = SqlCmd.ExecuteReader(behavior))
        //        {

        //            SQLExecutionEvent(SqlRea);
        //        }
        //    }
        //}

        #endregion
    }

}