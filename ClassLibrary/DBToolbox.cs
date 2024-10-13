using System;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Data.Odbc;//for SqlServer and MySQL ODBC drivers
using System.Data.SqlClient;//for stored Native SqlServer


namespace NLDBLibrary
{
    public enum DbTypes { SqlServer, SqlServerODBC, MySQL, Oracle, Unknown };

    /// <summary>
    /// Summary description for DBToolbox.
    /// </summary>
    public class DBToolbox
    {

        //TODO: Externalize This
        /*private string connectionInfo = "DRIVER={MySQL ODBC 3.51 Driver};\r\n" +
            "SERVER=localhost;DATABASE=capfive;\r\n" +
            "UID=cap5user;PASSWORD=cap5K3yz;\r\n" +
            "OPTION=3\r\n";*/

        //For Local testing
        /*private string connectionInfo = "DRIVER={MySQL ODBC 3.51 Driver};\r\n" +
            "SERVER=dozer.conversive.com;DATABASE=capfive;\r\n" +
            "UID=remoteuser;PASSWORD=remo55;\r\n" +
            "OPTION=3\r\n";*/

        //For MS Sql Server
        /*private string connectionInfo = "Driver={SQL Server};" +
            "Server=dozer.conversive.com;" +
            "Database=capfive;" +
            "Uid=cap5user;" +
            "Pwd=cap5K3yz";*/

        //this is most recent connection string for sql server
        //user=username; password=password;Initial Catalog=databasename;Data Source=servername;Connect Timeout=30;
        //could add: Network Library=DBMSSOCN;

        public int? ConnectionTimeout
        {
            get { return this.connectionTimeout; }
            set { this.connectionTimeout = value; }
        }
        private int? connectionTimeout = null;//null means it will just use the default

        private string connectionString = null;

        private DbTypes dbType = DbTypes.Unknown;

        public delegate void DataBaseError(Exception e);
        public event DataBaseError OnDataBaseError;

        //NOTE: This is a dangerous constructor to use since if it's connecting remotely the username is sent in (more or less) clear text
        public DBToolbox(DbTypes dbType, string connectionString)
        {
            this.dbType = dbType;
            this.connectionString = connectionString;
        }

        private void SendDBError(string errorMessage, Exception e)
        {
            Exception sqlException = new Exception(errorMessage, e);
            if (this.OnDataBaseError != null)
                this.OnDataBaseError(sqlException);
            throw (e);
        }

        public bool TestConnection()
        {
            try
            {
                IDbConnection dbconQuery = this.GetDbConnection(true, false);
                bool bRet = dbconQuery.State == ConnectionState.Open;

                dbconQuery.Close();
                return bRet;
            }
            catch
            {
                return false;
            }
        }

        private bool ensureDBClosed(IDbConnection dbcon)
        {
            bool bRetVal = true;
            try
            {
                if (dbcon.State != System.Data.ConnectionState.Closed)
                    dbcon.Close();

                /*
                while(this.dbcon.State != System.Data.ConnectionState.Closed)
                {
                    this.dbcon.Close();
                    System.Threading.Thread.Sleep(100);
                }
                */
            }
            catch (Exception e)
            {
                bRetVal = false;
                this.SendDBError("Error in ensureDBClosed.", e);
            }
            finally
            {
                if (dbcon != null)
                    dbcon.Dispose();
            }
            return bRetVal;
        }

        //TODO: remove this and make our Query functions able to take parameters
        //this is giving us a bit of a false sense of security
        public string EscapeSingleQuotes(string text)
        {
            if (text == null)
                return null;
            switch (this.dbType)
            {
                case DbTypes.SqlServer:
                case DbTypes.SqlServerODBC:
                    return this.escapeSingleQuotesSQLServer(text);
                default:
                    return this.escapeSingleQuotesDefault(text);
            }
        }

        private string escapeSingleQuotesSQLServer(string text)
        {
            return text.Replace(@"'", @"''");
        }

        private string escapeSingleQuotesDefault(string text)
        {
            text = text.Replace(@"\", @"\\");
            return text.Replace(@"'", @"\'");
        }

        public IDbConnection GetDbConnection()
        {
            return this.GetDbConnection(true, false);
        }

        public IDbConnection GetDbConnection(bool bOpen, bool bAsync)
        {
            IDbConnection dbconQuery = null;

            string stAsync = bAsync ? "async=true;" : "";

            if (this.dbType == DbTypes.MySQL || this.dbType == DbTypes.SqlServerODBC)
                dbconQuery = new OdbcConnection(this.connectionString + stAsync);
            else if (this.dbType == DbTypes.SqlServer)
                dbconQuery = new SqlConnection(this.connectionString + stAsync);

            try
            {
                //if(bAlwaysReconnect && this.dbcon.State != System.Data.ConnectionState.Closed)
                //dbconQuery.Close();
                if (bOpen && dbconQuery.State != System.Data.ConnectionState.Open)
                    dbconQuery.Open();
            }
            catch (Exception e)
            {
                this.SendDBError("Error in GetDbConnection.", e);
            }

            return dbconQuery;
            //return this.dbcon;
        }

        /*
        public List<Dictionary<string, object>> Query(string query)
        {
            return Query(query, new Dictionary<string, object>());
        }

        public List<Dictionary<string, object>> Query(string query, Dictionary<string, object> parameters)
        {
            List<List<Dictionary<string, object>>> results = QueryForMultiResults(query, parameters);
            if (results != null && results.Count > 0)
            {
                return results[0];
            }
            return null;
        }

        public List<List<Dictionary<string, object>>> QueryForMultiResults(string query)
        {
            return QueryForMultiResults(query, new Dictionary<string, object>());
        }

        public List<List<Dictionary<string, object>>> QueryForMultiResults(string query, Dictionary<string, object> parameters)
        {
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rows;
            List<List<Dictionary<string, object>>> results = new List<List<Dictionary<string, object>>>();

            //lock(this.dbcon)
            {
                IDbConnection dbconQuery = this.GetDbConnection();

                IDbCommand command = null;
                IDataReader reader = null;

                try
                {

                    if (this.dbType == DbTypes.MySQL || this.dbType == DbTypes.SqlServerODBC)
                        command = new OdbcCommand(query, (OdbcConnection)dbconQuery);
                    else if (this.dbType == DbTypes.SqlServer)
                    {
                        command = new SqlCommand(query, (SqlConnection)dbconQuery);
                        updateCommandWithParameters((SqlCommand)command, parameters);//NOTE: this makes the parameters ones only compatible with SqlServer
                    }

                    if (this.connectionTimeout.HasValue)
                        command.CommandTimeout = this.connectionTimeout.Value;

                    if (this.dbType == DbTypes.MySQL || this.dbType == DbTypes.SqlServerODBC)
                        reader = (OdbcDataReader)command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    else if (this.dbType == DbTypes.SqlServer)
                        reader = (SqlDataReader)command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    do
                    {
                        rows = new List<Dictionary<string, object>>();
                        results.Add(rows);

                        while (reader.Read())
                        {
                            row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                object rowCol = reader.GetValue(i);
                                row.Add(reader.GetName(i), rowCol);
                            }
                            rows.Add(row);
                        }
                    } while (reader.NextResult());
                }
                catch (Exception e)
                {
                    this.SendDBError("Error in QueryForMultiResults: " + query, e);
                }
                finally
                {
                    if (reader != null)//I added this because sometimes I was getting NPE's on the reader.Close(); line
                        reader.Close(); //Does <strike>Doesn't</strike> Implicitly closes the connection because CommandBehavior.CloseConnection was not specified.
                    this.ensureDBClosed(dbconQuery);
                }
            }

            return results;
        }*/

        public int ExecuteNonQuery(string stNonQuery)
        {
            int rows = -1;
            if (this.dbType == DbTypes.MySQL || this.dbType == DbTypes.SqlServerODBC)
                rows = this.executeNonQueryOdbc(stNonQuery);
            else if (this.dbType == DbTypes.SqlServer)
                rows = this.executeNonQuerySqlServer(stNonQuery);
            return rows;
        }

        public int ExecuteNonQuery(string stNonQuery,Dictionary<string, object> parameters)
        {
            int rows = -1;
            if (this.dbType == DbTypes.MySQL || this.dbType == DbTypes.SqlServerODBC)
                rows = this.executeNonQueryOdbc(stNonQuery, parameters);
            else if (this.dbType == DbTypes.SqlServer)
                rows = this.executeNonQuerySqlServer(stNonQuery, parameters);
            return rows;
        }

        private int executeNonQuerySqlServer(string stNonQuery)
        {
            int rows = -1;
            try
            {
                SqlConnection connection = (SqlConnection)this.GetDbConnection();
                try
                {
                    SqlCommand command = new SqlCommand(stNonQuery, connection);
                    if (this.connectionTimeout.HasValue)
                        command.CommandTimeout = this.connectionTimeout.Value;
                    rows = command.ExecuteNonQuery();
                }
                catch (Exception innerE)
                {
                    this.SendDBError("InnerError in executeNonQuerySqlServer: " + stNonQuery, innerE);
                }
                finally
                {
                    this.ensureDBClosed(connection);
                }
            }
            catch (Exception e)
            {
                this.SendDBError("Error Getting Connection in executeNonQuerySqlServer: " + stNonQuery, e);
            }
            return rows;
        }

        private int executeNonQuerySqlServer(string stNonQuery, Dictionary<string, object> parameters)
        {
            int rows = -1;
            try
            {
                SqlConnection connection = (SqlConnection)this.GetDbConnection();
                try
                {
                    SqlCommand command = new SqlCommand(stNonQuery, connection);
                    if (this.connectionTimeout.HasValue)
                        command.CommandTimeout = this.connectionTimeout.Value;
                    updateCommandWithParameters(command, parameters);
                    rows = command.ExecuteNonQuery();
                }
                catch (Exception innerE)
                {
                    this.SendDBError("InnerError in executeNonQuerySqlServer: " + stNonQuery, innerE);
                }
                finally
                {
                    this.ensureDBClosed(connection);
                }
            }
            catch (Exception e)
            {
                this.SendDBError("Error Getting Connection in executeNonQuerySqlServer: " + stNonQuery, e);
            }
            return rows;
        }

        private int executeNonQueryOdbc(string stNonQuery)
        {
            int rows = -1;
            try
            {
                OdbcConnection connection = (OdbcConnection)this.GetDbConnection();
                try
                {
                    OdbcCommand command = new OdbcCommand(stNonQuery, connection);
                    if (this.connectionTimeout.HasValue)
                        command.CommandTimeout = this.connectionTimeout.Value;
                    rows = command.ExecuteNonQuery();
                }
                catch (Exception innerE)
                {
                    this.SendDBError("InnerError in executeNonQueryOdbc: " + stNonQuery, innerE);
                }
                finally
                {
                    this.ensureDBClosed(connection);
                }
            }
            catch (Exception e)
            {
                this.SendDBError("Error getting connection in executeNonQueryOdbc: " + stNonQuery, e);
            }
            return rows;
        }

        private int executeNonQueryOdbc(string stNonQuery, Dictionary<string, object> parameters)
        {
            int rows = -1;
            try
            {
                OdbcConnection connection = (OdbcConnection)this.GetDbConnection();
                try
                {
                    OdbcCommand command = new OdbcCommand(stNonQuery, connection);
                    if (this.connectionTimeout.HasValue)
                        command.CommandTimeout = this.connectionTimeout.Value;
                    updateCommandWithParameters(command, parameters);
                    rows = command.ExecuteNonQuery();
                }
                catch (Exception innerE)
                {
                    this.SendDBError("InnerError in executeNonQueryOdbc: " + stNonQuery, innerE);
                }
                finally
                {
                    this.ensureDBClosed(connection);
                }
            }
            catch (Exception e)
            {
                this.SendDBError("Error getting connection in executeNonQueryOdbc: " + stNonQuery, e);
            }
            return rows;
        }

        private List<List<Dictionary<string, object>>> dataSetToTableList(System.Data.DataSet ds)
        {
            List<List<Dictionary<string, object>>> tables = new List<List<Dictionary<string, object>>>();

            foreach (System.Data.DataTable dt in ds.Tables)
            {
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    foreach (System.Data.DataColumn dc in dt.Columns)
                    {
                        row.Add(dc.ColumnName, dr[dc.ColumnName]);
                    }
                    rows.Add(row);
                }
                tables.Add(rows);
            }
            return tables;
        }

        public DataSet ReturnDataTableUsingStoredProc(string procName, SqlCommand command)
        {
            SqlConnection connection = (SqlConnection)this.GetDbConnection();
            DataSet dt = new DataSet();

            try
            {
                if (this.connectionTimeout.HasValue)
                    command.CommandTimeout = this.connectionTimeout.Value;
                command.CommandText = procName;
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
            }
            catch (Exception e)
            {
                this.SendDBError("Error in ExecuteScalarProc: " + procName, e);
            }
            finally
            {
                this.ensureDBClosed(connection);
            }
            return dt;

        }

        public DataTable ReturnDataTable(string Query)
        {
            SqlConnection connection = (SqlConnection)this.GetDbConnection();
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(Query, connection);
                da.Fill(dt);
            }
            catch (Exception e)
            {
                this.SendDBError("Error in ReturnDataTable: " + Query, e);
            }
            finally
            {
                this.ensureDBClosed(connection);
            }
            return dt;
        }

        public List<List<Dictionary<string, object>>> Query(string cmdText, Dictionary<string, object> parameters)
        {
            Dictionary<string, object> row;
            List<Dictionary<string, object>> rows;
            List<List<Dictionary<string, object>>> results = new List<List<Dictionary<string, object>>>();
            SqlDataReader reader = null;
            //lock(this.dbcon)
            {
                SqlConnection connection = (SqlConnection)this.GetDbConnection();
                try
                {
                    SqlCommand command = new SqlCommand(cmdText, connection);
                    if (this.connectionTimeout.HasValue)
                        command.CommandTimeout = this.connectionTimeout.Value;
                    updateCommandWithParameters(command, parameters);

                    reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    do
                    {
                        rows = new List<Dictionary<string, object>>();
                        results.Add(rows);

                        while (reader.Read())
                        {
                            row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                object rowCol = reader.GetValue(i);
                                row.Add(reader.GetName(i), rowCol);
                            }
                            rows.Add(row);
                        }
                    }
                    while (reader.NextResult());
                }
                catch (Exception e)
                {
                    this.SendDBError("Error in Query: " + cmdText, e);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    this.ensureDBClosed(connection);
                }
                return results;
            }
        }

        private void updateCommandWithParameters(SqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (string key in parameters.Keys)
                {
                    command.Parameters.AddWithValue(key, parameters[key]);
                }
            }
        }

        private void updateCommandWithParameters(OdbcCommand command, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (string key in parameters.Keys)
                {
                    command.Parameters.AddWithValue(key, parameters[key]);
                }
            }
        }

    }//class DBToolbox


}
