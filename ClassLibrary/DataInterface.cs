using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace NLDBLibrary
{
    public class DataInterface
    {
        public DBToolbox DBToolbox { get; set; }

        public DataInterface(string connectionString)
        {
            this.DBToolbox = new DBToolbox(DbTypes.SqlServer, connectionString);
        }

        public Dictionary<string, object> GetSpaceUsedSummary()
        {
            Dictionary<string, object> summary = new Dictionary<string, object>();

            string query = "EXEC sp_spaceused";
            List<List<Dictionary<string, object>>> tables = this.DBToolbox.Query(query, null);
            if (tables != null && tables.Count == 2)
            {
                foreach (string key in tables[0][0].Keys)
                {
                    summary.Add(key, tables[0][0][key]);
                }
                foreach (string key in tables[1][0].Keys)
                {
                    summary.Add(key, tables[1][0][key]);
                }
            }
            return summary;
        }

        public List<Dictionary<string, object>> GetRunningQueries()
        {
            //from:
            //http://stackoverflow.com/questions/941763/list-the-queries-running-on-sql-server
            //and
            //http://blog.sqlauthority.com/2009/01/07/sql-server-find-currently-running-query-t-sql/
            string query = "SELECT ( SELECT SUBSTRING(text,statement_start_offset/2,(CASE WHEN statement_end_offset = -1 then LEN(CONVERT(nvarchar(max), text)) * 2 ELSE statement_end_offset end -statement_start_offset)/2 ) FROM sys.dm_exec_sql_text(req.sql_handle) ) AS query_text, " +
                           " req.session_id, " +
                           " req.status, " +
                           " req.command, " +
                           " req.cpu_time, " +
                           " req.total_elapsed_time " +
                           " ,   right(convert(varchar,  " +
                           "             dateadd(ms, datediff(ms, P.last_batch, getdate()), '1900-01-01'),  " +
                           "             121), 12) as 'batch_duration' " +
                           " ,   P.program_name " +
                           " ,   P.hostname " +
                           " ,   P.loginame " +
                           " FROM sys.dm_exec_requests req " +
                           " INNER JOIN master.dbo.sysprocesses P on req.session_id=P.spid " +
                           " WHERE P.spid > 50 " +
                           "         and      P.status not in ('background', 'sleeping') " +
                           "         and      P.cmd not in ('AWAITING COMMAND' " +
                           "                            ,'MIRROR HANDLER' " +
                           "                            ,'LAZY WRITER' " +
                           "                            ,'CHECKPOINT SLEEP' " +
                           "                            ,'RA MANAGER') " +
                           " AND p.spid != @@SPID " + //exclude this query
                           " AND loginame != '' " +
                           " order by batch_duration desc";
            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public void KillRunningQuery(int sessionId)
        {
            string query = "KILL " + sessionId;
            this.DBToolbox.ExecuteNonQuery(query);
        }

        public void DeleteExceptionLog(int eid)
        {
            string query = "DELETE FROM dbo.ExceptionLog WHERE eid = " + eid;
            this.DBToolbox.ExecuteNonQuery(query);
        }

        public void ResetExceptionLog(int eid)
        {
            string query = "EXEC dbo.ResetException " + eid + ";";
            this.DBToolbox.ExecuteNonQuery(query);
        }

        public List<Dictionary<string, object>> GetQueryHistory()
        {
            //from:
            //http://sqlblog.com/blogs/elisabeth_redei/archive/2009/03/01/how-to-get-high-quality-information-about-query-performance.aspx
            string query = "SELECT " +
                           " ( SELECT SUBSTRING(text,statement_start_offset/2,(CASE WHEN statement_end_offset = -1 then LEN(CONVERT(nvarchar(max), text)) * 2 ELSE statement_end_offset end -statement_start_offset)/2 ) FROM sys.dm_exec_sql_text(sql_handle) ) AS query_text " +
                           " , total_elapsed_time " +
                           " , qs.execution_count AS NumberOfExecs " +
                           " , (total_elapsed_time/execution_count)/1000 AS [Avg Exec Time in ms] " +
                           " , max_elapsed_time/1000 AS [MaxExecTime in ms] " +
                           " , min_elapsed_time/1000 AS [MinExecTime in ms] " +
                           " , (total_worker_time/execution_count)/1000 AS [Avg CPU Time in ms] " +
                           " , (total_logical_writes+total_logical_Reads)/execution_count AS [Avg Logical IOs] " +
                           " , max_logical_reads AS MaxLogicalReads " +
                           " , min_logical_reads AS MinLogicalReads " +
                           " , max_logical_writes AS MaxLogicalWrites " +
                           " , min_logical_writes AS MinLogicalWrites " +
                           " FROM sys.dm_exec_query_stats qs " +
                           " where max_elapsed_time/1000 > 0 " +
                           " ORDER BY total_elapsed_time desc,[Avg Exec Time in ms] DESC;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetIndexFragmentation()
        {
            string query = "SELECT a.object_id, " +
                           " b.name AS IndexName, " +
                           " a.avg_fragmentation_in_percent AS PercentFragment, " +
                           " a.fragment_count AS TotalFrags, " +
                           " a.avg_fragment_size_in_pages AS PagesPerFrag, " +
                           " a.page_count AS NumPages, " +
                           " (a.fragment_count * (a.avg_fragmentation_in_percent /100.0)) as Severity " +
                           " FROM sys.dm_db_index_physical_stats(DB_ID(), " +
                           " NULL, NULL, NULL , NULL) AS a " +
                           " INNER JOIN sys.indexes AS b ON a.object_id = b.object_id AND a.index_id = b.index_id " +
                           " WHERE a.avg_fragmentation_in_percent > 0 " +
                           " AND page_count > 10 " +
                           " ORDER BY Severity desc;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetExceptionLogs()
        {
            string query = "SELECT eid AS [标识],CONVERT(varchar,time,120) AS [时间],number AS [错误号],serverity AS [级别],state AS [状态],prodcedure AS [过程名],line AS [行数],message AS [消息]" +
                           " FROM dbo.ExceptionLog ORDER BY eid DESC;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetStatisticInfo()
        {
            string query = "SELECT [统计对象], [统计类别], [统计数量] FROM (SELECT 'ExceptionLog' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.ExceptionLog " +
                           " UNION " +
                           "SELECT 'Dictionary' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.Dictionary" +
                           " UNION " +
                           "SELECT 'TextPool' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.TextPool" +
                           " UNION " +
                           "SELECT 'FilterRule' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.FilterRule" +
                           " UNION " +
                           "SELECT 'ParseRule' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.ParseRule" +
                           " UNION " +
                           "SELECT 'PhraseRule' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.PhraseRule" +
                           " UNION " +
                           "SELECT 'WordAttribute' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.WordAttribute" +
                           " UNION " +
                           "SELECT 'InnerContent' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.InnerContent" +
                           " UNION " +
                           "SELECT 'OuterContent' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.OuterContent" +
                           " UNION " +
                           "SELECT 'ExternalContent' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.ExternalContent) AS T ORDER BY [统计对象]";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetExceptionLogCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.ExceptionLog;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectExceptionLog(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT eid AS [标识],CONVERT(varchar,time,120) AS [时间],number AS [错误号],serverity AS [级别],state AS [状态],prodcedure AS [过程名],line AS [行数],message AS [消息]" +
                " FROM dbo.ExceptionLog ORDER BY eid DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectTextPool(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT tid AS [标识], length AS [长度], (CASE WHEN length > 64 THEN (LEFT(content,64) + '......') ELSE content END) AS [内容], parsed AS [状态], result AS [结果值]" +
                " FROM dbo.TextPool ORDER BY tid OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetTextPoolCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.TextPool;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetTextPoolByID(int tid)
        {
            string query = String.Format("SELECT tid,length,content,parsed,result FROM dbo.TextPool WHERE tid={0}", tid);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> ClearTextPoolByID(int tid)
        {
            string query = String.Format("EXEC dbo.ClearTextContent {0};" +
                "SELECT tid,length,content,parsed,result FROM dbo.TextPool WHERE tid={0};", tid);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetTextPoolRandomly()
        {
            string query = "DECLARE @SqlTID INT; " +
                        "SELECT @SqlTID = MAX(TID) FROM dbo.TextPool; " +
                        "SET @SqlTID = ROUND(@SqlTID * RAND(), 0); " +
                        "SELECT tid,length,content,parsed,result FROM dbo.TextPool WHERE tid=@SqlTID;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectDictionary(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT did AS [标识], dbo.ContentGetCID(content) AS [内容标识], length AS [长度], content AS [内容], enable AS [是否可用], count AS [统计词频], (CASE WHEN dbo.IsTerminator(content) > 0 THEN 1 ELSE 0 END) AS [终结符],classification AS [分类], ISNULL(remark,'') AS [备注]" +
                " FROM dbo.Dictionary ORDER BY did OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetDictionaryCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.Dictionary;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetDictionaryByID(int did)
        {
            string query = String.Format("SELECT did,dbo.ContentGetCID(content) AS cid,length,content,enable,count,classification,ISNULL(remark,'') as [remark] FROM dbo.Dictionary WHERE did={0}", did);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SwitchDictionaryByID(int did)
        {
            string query = String.Format("DECLARE @SqlContent UString;" +
                "SELECT TOP 1 @SqlContent = content FROM dbo.Dictionary WHERE did = {0};" +
                "UPDATE dbo.Dictionary SET enable = ~enable WHERE content = @SqlContent;" +
                "SELECT did,dbo.ContentGetCID(content) AS cid,length,content,enable,count,classification,ISNULL(remark,'') as [remark] FROM dbo.Dictionary WHERE did={0};", did);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> InsertDictionaryByID(int did)
        {
            string query = String.Format("DECLARE @SqlContent UString;" +
                "SELECT TOP 1 @SqlContent = content FROM dbo.Dictionary WHERE did = {0};" +
                "DECLARE @SqlCID INT = 0;" +
                "EXEC @SqlCID = dbo.ContentInsertValue @SqlContent;" +
                "SELECT did,dbo.ContentGetCID(content) AS cid,length,content,enable,count,classification,ISNULL(remark,'') as [remark] FROM dbo.Dictionary WHERE did={0};", did);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SwitchDictionaryByContent(string content)
        {
            string query = "UPDATE dbo.Dictionary SET enable = ~enable WHERE content = @SqlContent;" +
                "SELECT TOP 1 did,dbo.ContentGetCID(content) AS cid,length,content,enable,count,classification,ISNULL(remark,'') as [remark] FROM dbo.Dictionary WHERE content=@SqlContent;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetDictionaryRandomly()
        {
            string query = "DECLARE @SqlDID INT; " +
                        "SELECT @SqlDID = MAX(DID) FROM dbo.Dictionary; " +
                        "SET @SqlDID = ROUND(@SqlDID * RAND(), 0); " +
                        "SELECT did,dbo.ContentGetCID(content) AS cid,length,content,enable,count,classification,ISNULL(remark,'') as [remark] FROM dbo.Dictionary WHERE did=@SqlDID;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetFilterRuleCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.FilterRule;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectFilterRule(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT rid AS [标识], classification AS [分类], [rule] AS [规则项], replace AS [替代项], ISNULL(requirements,'') AS [其他要求]" +
                " FROM dbo.FilterRule ORDER BY rid DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetInnerContentCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.InnerContent;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectInnerContent(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT cid AS [内容标识], length AS [长度], [content] AS [内容], classification AS [分类], count AS [统计词频], type AS [状态标志], ISNULL(attribute,'') AS [词性标注]" +
                " FROM dbo.InnerContent ORDER BY cid OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetInnerContentByID(int cid)
        {
            string query = String.Format("SELECT cid,length,content,type,count,classification,ISNULL(attribute,'') as [attribute] FROM dbo.InnerContent WHERE cid={0}", cid);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetInnerContentRandomly()
        {
            string query = "DECLARE @SqlCID INT; " +
                        "SELECT @SqlCID = MAX(CID) FROM dbo.InnerContent; " +
                        "SET @SqlCID = ROUND(@SqlCID * RAND(), 0); " +
                        "SELECT cid,length,content,type,count,classification,ISNULL(attribute,'') as [attribute] FROM dbo.InnerContent WHERE cid=@SqlCID;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetOuterContentCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.OuterContent;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectOuterContent(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT oid AS [标识], cid AS [内容标识], length AS [长度], [content] AS [内容], classification AS [分类], count AS [统计词频], type AS [状态标志], ISNULL([rule],'') AS [解析规则]" +
                //",a_id AS [Aid],b_id AS [Bid],c_id AS [Cid],d_id AS [Did],e_id AS [Eid],f_id AS [Fid],g_id AS [Gid],h_id AS [Hid],i_id AS [Iid],j_id AS [Jid],k_id AS [Kid],l_id AS [Lid],m_id AS [Mid],n_id AS [Nid],o_id AS [Oid],p_id AS [Pid],q_id AS [Qid],r_id AS [Rid],s_id AS [Sid],t_id AS [Tid],u_id AS [Uid],v_id AS [Vid],w_id AS [Wid],x_id AS [Xid],y_id AS [Yid],z_id AS [Zid]" +
                " FROM dbo.OuterContent ORDER BY oid DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetOuterContentByID(int oid)
        {
            string query = String.Format("SELECT oid,cid,length,content,type,count,classification,ISNULL([rule],'') as [rule] ,a_id,b_id,c_id,d_id,e_id,f_id,g_id,h_id,i_id,j_id,k_id,l_id,m_id,n_id,o_id,p_id,q_id,r_id,s_id,t_id,u_id,v_id,w_id,x_id,y_id,z_id FROM dbo.OuterContent WHERE oid={0}", oid);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetOuterContentRandomly()
        {
            string query = "DECLARE @SqlOID INT; " +
                        "SELECT @SqlOID = MAX(OID) FROM dbo.OuterContent; " +
                        "SET @SqlOID = ROUND(@SqlOID * RAND(), 0); " +
                        "SELECT oid,cid,length,content,type,count,classification,ISNULL([rule],'') as [rule],a_id,b_id,c_id,d_id,e_id,f_id,g_id,h_id,i_id,j_id,k_id,l_id,m_id,n_id,o_id,p_id,q_id,r_id,s_id,t_id,u_id,v_id,w_id,x_id,y_id,z_id FROM dbo.OuterContent WHERE oid=@SqlOID;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetExternalContentCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.ExternalContent;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectExternalContent(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT eid AS [标识], length AS [长度], (CASE WHEN length > 64 THEN (LEFT(content,64) + '......') ELSE content END) AS [内容], classification AS [分类], tid AS [语料标识], type AS [状态标志], ISNULL([rule],'') AS [解析规则] " + 
                " FROM dbo.ExternalContent ORDER BY eid DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetExternalContentByID(int eid)
        {
            string query = String.Format("SELECT eid,length,content,type,tid,classification,ISNULL([rule],'') as [rule] FROM dbo.ExternalContent WHERE eid={0}", eid);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetExternalContentRandomly()
        {
            string query = "DECLARE @SqlEID INT; " +
                        "SELECT @SqlEID = MAX(EID) FROM dbo.ExternalContent; " +
                        "SET @SqlEID = ROUND(@SqlEID * RAND(), 0); " +
                        "SELECT eid,length,content,type,tid,classification,ISNULL([rule],'') as [rule] FROM dbo.ExternalContent WHERE eid=@SqlEID;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetRegularRuleCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.PhraseRule WHERE classification='正则';";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectRegularRule(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT rid AS [标识], classification AS [分类], dbo.XMLEscape([rule]) AS [规则项], attribute AS [属性], ISNULL(requirements,'') AS [其他要求]" +
                " FROM dbo.PhraseRule WHERE classification='正则' ORDER BY rid DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> ParseContentByRegularRule(string content)
        {
            string query = String.Format("DECLARE @SqlRules XML;" +
                "SET @SqlRules = dbo.LoadRegularRules();" +
                "DECLARE @SqlResult XML;" + 
                "SET @SqlResult = dbo.ExtractExpressions(@SqlRules,@SqlContent);" +
                "SELECT " +
                    "Nodes.value('(@id)[1]', 'int') AS [标识]," +
                    "Nodes.value('(@type)[1]', 'nvarchar(max)') AS [类型]," +
                    "Nodes.value('(@pos)[1]', 'int') AS [位置]," +
                    "Nodes.value('(@len)[1]', 'int') AS [长度]," +
                    "Nodes.value('(text())[1]', 'nvarchar(max)') AS [内容] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [位置];", content);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetNumericalRuleCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.PhraseRule WHERE classification IN ('正则','数词','数量词');";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectNumericalRule(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT rid AS [标识], classification AS [分类], dbo.XMLEscape([rule]) AS [规则项], attribute AS [属性], ISNULL(requirements,'') AS [其他要求]" +
                " FROM dbo.PhraseRule WHERE classification IN ('正则','数词','数量词') ORDER BY rid DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> ParseContentByNumericalRule(string content)
        {
            string query = String.Format("DECLARE @SqlRules XML;" +
                "SET @SqlRules = dbo.LoadNumericalRules();" +
                "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.ExtractExpressions(@SqlRules,@SqlContent);" +
                "SELECT " +
                    "Nodes.value('(@id)[1]', 'int') AS [标识]," +
                    "Nodes.value('(@type)[1]', 'nvarchar(max)') AS [类型]," +
                    "Nodes.value('(@pos)[1]', 'int') AS [位置]," +
                    "Nodes.value('(@len)[1]', 'int') AS [长度]," +
                    "Nodes.value('(text())[1]', 'nvarchar(max)') AS [内容] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [位置];", content);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetPhraseRuleCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.PhraseRule WHERE classification IN ('短语','句子');";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectPhraseRule(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT rid AS [标识], classification AS [分类], dbo.XMLEscape([rule]) AS [规则项], attribute AS [属性], ISNULL(requirements,'') AS [其他要求]" +
                " FROM dbo.PhraseRule WHERE classification IN ('短语','句子') ORDER BY rid DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetPhraseRuleByID(int rid)
        {
            string query = String.Format("SELECT rid,classification,dbo.XMLEscape([rule]) AS [rule],attribute,requirements" +
                " FROM dbo.PhraseRule WHERE rid = {0};", rid);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> ParseContentByPhraseRule(string content)
        {
            string query = String.Format("DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.ExtractPhrases(@SqlContent);" +
                "SELECT " +
                    "Nodes.value('(@id)[1]', 'int') AS [标识]," +
                    "Nodes.value('(@type)[1]', 'nvarchar(max)') AS [类型]," +
                    "Nodes.value('(@pos)[1]', 'int') AS [位置]," +
                    "Nodes.value('(@len)[1]', 'int') AS [长度]," +
                    "Nodes.value('(text())[1]', 'nvarchar(max)') AS [内容] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [位置];", content);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetAttributeRuleCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.WordAttribute;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectAttributeRule(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT wid AS [标识], classification AS [分类], length AS [长度], content AS [内容], attribute AS [属性], ISNULL(collections,'') AS [可匹配项]" +
                " FROM dbo.WordAttribute ORDER BY wid DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetAttributeRuleByID(int wid)
        {
            string query = String.Format("SELECT wid,classification,content AS [rule],attribute,collections" +
                                            " FROM dbo.WordAttribute WHERE wid = {0};", wid);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> ParseContentByAttributeRule(string content)
        {
            string query = String.Format("DECLARE @SqlRules XML;" +
                "SET @SqlRules = dbo.LoadAttributeRules();" +
                "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.ExtractExpressions(@SqlRules,@SqlContent);" +
                "SELECT " +
                    "Nodes.value('(@id)[1]', 'int') AS [标识]," +
                    "Nodes.value('(@type)[1]', 'nvarchar(max)') AS [类型]," +
                    "Nodes.value('(@pos)[1]', 'int') AS [位置]," +
                    "Nodes.value('(@len)[1]', 'int') AS [长度]," +
                    "Nodes.value('(text())[1]', 'nvarchar(max)') AS [内容] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [位置];", content);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetParseRuleCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.ParseRule WHERE classification IN ('拼接','配对','通用','单句','复句');";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectParseRule(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT rid AS [标识], classification AS [分类], dbo.XMLEscape([rule]) AS [规则项], minimum_length AS [最小长度], parameter_count AS [参数个数], static_prefix AS [前缀], static_suffix AS [后缀], controllable_priority AS [优先级] " +
                " FROM dbo.ParseRule WHERE classification IN ('拼接','配对','通用','单句','复句') ORDER BY rid DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> CutContentIntoSentences(string content)
        {
            string query = String.Format("DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.CutIntoSentences(@SqlContent);" +
                "SELECT " +
                    "Nodes.value('(@id)[1]', 'int') AS [序号]," +
                    "Nodes.value('(@eid)[1]', 'int') AS [标识]," +
                    "Nodes.value('(@len)[1]', 'int') AS [长度]," +
                    "Nodes.value('(text())[1]', 'nvarchar(max)') AS [内容] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [序号];", content);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> FilterContentByRule(string content)
        {
            string query = String.Format("DECLARE @SqlRules XML;" +
                "SET @SqlRules = dbo.LoadFilterRules();" + 
                "SELECT dbo.FilterContent(@SqlRules,@SqlContent) AS [内容];", content);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetTextPoolInfo()
        {
            string query = "SELECT [统计对象], [统计类别], [统计数量] FROM (" +
                           "SELECT 'TextPool' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.TextPool" +
                           " UNION " +
                           "SELECT 'TextPool' AS [统计对象], '最大长度' AS [统计类别], MAX(length) AS [统计数量] FROM dbo.TextPool" +
                           " UNION " +
                           "SELECT 'TextPool' AS [统计对象], '最小长度' AS [统计类别], MIN(length) AS [统计数量] FROM dbo.TextPool" +
                           ") AS T ORDER BY [统计类别]";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetTextPoolParsedInfo()
        {
            string query = "SELECT [统计对象], [统计类别], [统计数量] FROM (" +
                           "SELECT 'TextPool.parsed' AS[统计对象], parsed AS[统计类别], count(*) AS[统计数量] FROM dbo.TextPool GROUP BY parsed" +
                           " UNION " +
                           "SELECT 'TextPool.(parsed > 0).result' AS[统计对象], result AS[统计类别], count(*) AS[统计数量] FROM dbo.TextPool WHERE parsed > 0 GROUP BY result" +
                           ") AS T ORDER BY[统计对象] DESC, [统计类别]";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetTextPoolLengthInfo()
        {
            string query = "SELECT 'TextPool.length' AS [统计对象]," +
                        "(CASE WHEN length <= 2 THEN '1-2' WHEN length <= 4 THEN '2-4' WHEN length <= 8 THEN '4-8' WHEN length <= 16 THEN '8-16' WHEN length <= 32 THEN '16-32' WHEN length <= 64 THEN '32-64' " +
                        " WHEN length <= 128 THEN '64-128' WHEN length <= 256 THEN '128-256' WHEN length <= 512 THEN '256-512' WHEN length <= 1024 THEN '512-1024' WHEN length <= 2048 THEN '1024-2048' WHEN length <= 4096 THEN '2048-4096' " +
                        "  WHEN length <= 8192 THEN '4096-8192' ELSE '8192-' END) AS [统计类别] , COUNT(*) AS [统计数量] FROM dbo.TextPool GROUP BY (CASE WHEN length <= 2 THEN '1-2' WHEN length <= 4 THEN '2-4' WHEN length <= 8 THEN '4-8' " +
                        " WHEN length <= 16 THEN '8-16' WHEN length <= 32 THEN '16-32' WHEN length <= 64 THEN '32-64' WHEN length <= 128 THEN '64-128' WHEN length <= 256 THEN '128-256' WHEN length <= 512 THEN '256-512' " +
                        " WHEN length <= 1024 THEN '512-1024' WHEN length <= 2048 THEN '1024-2048' WHEN length <= 4096 THEN '2048-4096' WHEN length <= 8192 THEN '4096-8192' ELSE '8192-' END) ORDER BY[统计数量] DESC";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetDictionaryInfo()
        {
            string query = "SELECT [统计对象], [统计类别], [统计数量] FROM (" +
                           "SELECT 'Dictionary' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.Dictionary" +
                           " UNION " +
                           "SELECT 'Dictionary' AS [统计对象], '最大长度' AS [统计类别], MAX(length) AS [统计数量] FROM dbo.Dictionary" +
                           " UNION " +
                           "SELECT 'Dictionary' AS [统计对象], '最小长度' AS [统计类别], MIN(length) AS [统计数量] FROM dbo.Dictionary" +
                           " UNION " +
                           "SELECT 'Dictionary' AS [统计对象], '单词数量' AS [统计类别], count(*) AS [统计数量] FROM (SELECT DISTINCT(content) FROM dbo.Dictionary) AS K" +
                           ") AS T ORDER BY [统计类别]";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetDictionaryEnabledInfo()
        {
            string query = "SELECT [统计对象], [统计类别], [统计数量] FROM (" +
                            "SELECT 'Dictionary.enable' AS [统计对象], enable AS [统计类别], count(*) AS[统计数量] FROM dbo.Dictionary GROUP BY enable" + 
                            ") AS T ORDER BY [统计对象] , [统计类别]";
            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetDictionaryCountInfo()
        {
            string query = "SELECT 'Dictionary.count' AS [统计对象], [词频] AS [统计类别], [数量] AS [统计数量] FROM ( SELECT '零次' AS [词频], count(*) AS [数量] FROM dbo.Dictionary WHERE enable = 1 AND count = 0 " +
                " UNION " +
                " SELECT (CASE[指数] WHEN 0 THEN '单次' WHEN 1 THEN '十余次' WHEN 2 THEN '百余次' WHEN 3 THEN '千余次' WHEN 4 THEN '万余次' WHEN 5 THEN '十万余次' WHEN 6 THEN '百万余次' WHEN 7 THEN '千万余次' WHEN 8 THEN '亿余次' ELSE '其他' END) AS [词频],SUM([总数]) AS [数量] " +
                " FROM (SELECT CEILING(LOG10(count)) AS[指数], count(*) as [总数] FROM dbo.Dictionary WHERE enable = 1 AND count > 0 GROUP BY count) AS T GROUP BY[指数]) AS T2 ORDER BY[统计数量] DESC";
            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetDictionaryLengthInfo()
        {
            string query = "SELECT 'Dictionary.length' AS [统计对象]," +
                        "(CASE WHEN length <= 2 THEN '1-2' WHEN length <= 4 THEN '2-4' WHEN length <= 8 THEN '4-8' WHEN length <= 16 THEN '8-16' WHEN length <= 32 THEN '16-32' WHEN length <= 64 THEN '32-64' " +
                        " ELSE '64-' END) AS [统计类别] , COUNT(*) AS [统计数量] FROM dbo.Dictionary GROUP BY (CASE WHEN length <= 2 THEN '1-2' WHEN length <= 4 THEN '2-4' WHEN length <= 8 THEN '4-8' " +
                        " WHEN length <= 16 THEN '8-16' WHEN length <= 32 THEN '16-32' WHEN length <= 64 THEN '32-64' ELSE '64-' END) ORDER BY [统计数量] DESC";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetDictionaryClassificationInfo()
        {
            string query = "SELECT 'Dictionary.classification' AS [统计对象]," +
                        "classification AS [统计类别] , COUNT(*) AS [统计数量] FROM dbo.Dictionary GROUP BY classification ORDER BY [统计类别]";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetFilterRuleInfo()
        {
            string query = "SELECT 'FilterRule' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.FilterRule";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetRegularRuleInfo()
        {
            string query = "SELECT 'RegularRule' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.PhraseRule WHERE classification = '正则' " +
                " UNION " +
                "SELECT 'RegularRule.attribute' AS [统计对象], attribute AS [统计类别], count(*) AS [统计数量] FROM dbo.PhraseRule WHERE classification = '正则' GROUP BY attribute";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetNumericalRuleInfo()
        {
            string query = "SELECT 'NumericalRule' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.PhraseRule WHERE classification IN ('数词','数量词') " +
                " UNION " +
                "SELECT 'NumericalRule.attribute' AS [统计对象], attribute AS [统计类别], count(*) AS [统计数量] FROM dbo.PhraseRule WHERE classification IN ('数词','数量词') GROUP BY attribute";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetAttributeRuleInfo()
        {
            string query = "SELECT 'AttributeRule' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.WordAttribute " +
                " UNION " +
                "SELECT 'AttributeRule.attribute' AS [统计对象], attribute AS [统计类别], count(*) AS [统计数量] FROM dbo.WordAttribute GROUP BY attribute";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetPhraseRuleInfo()
        {
            string query = "SELECT 'PhraseRule' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.PhraseRule WHERE classification IN ('短语','句子') " +
                " UNION " +
                "SELECT 'PhraseRule.attribute' AS [统计对象], attribute AS [统计类别], count(*) AS [统计数量] FROM dbo.PhraseRule WHERE classification IN ('短语','句子') GROUP BY attribute";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetParseRuleInfo()
        {
            string query = "SELECT 'ParseRule' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.ParseRule " +
                " UNION " +
                "SELECT 'ParseRule.classification' AS [统计对象], classification AS [统计类别], count(*) AS [统计数量] FROM dbo.ParseRule GROUP BY classification";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetInnerContentInfo()
        {
            string query = "SELECT 'InnerContent' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.InnerContent " +
                " UNION " +
                "SELECT 'InnerContent.classification' AS [统计对象], classification AS [统计类别], count(*) AS [统计数量] FROM dbo.InnerContent GROUP BY classification";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetOuterContentInfo()
        {
            string query = "SELECT 'OuterContent' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.OuterContent " +
                " UNION " +
                "SELECT 'OuterContent.classification' AS [统计对象], classification AS [统计类别], count(*) AS [统计数量] FROM dbo.OuterContent GROUP BY classification";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetExternalContentInfo()
        {
            string query = "SELECT 'ExternalContent' AS [统计对象], '总数' AS [统计类别], count(*) AS [统计数量] FROM dbo.ExternalContent " +
                " UNION " +
                "SELECT 'ExternalContent.classification' AS [统计对象], classification AS [统计类别], count(*) AS [统计数量] FROM dbo.ExternalContent GROUP BY classification" +
                " UNION " +
                "SELECT 'ExternalContent.(classification=\"单句\").type' AS [统计对象], CONVERT(NVARCHAR(MAX),[type]) AS [统计类别], count(*) AS [统计数量] FROM dbo.ExternalContent WHERE classification = '单句' GROUP BY [type]";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> FMMAllDictionaryByID(int did)
        {
            string query = "DECLARE @SqlContent UString;" +
                "SELECT TOP 1 @SqlContent = content FROM dbo.Dictionary WHERE did = @SqlDID;" + 
                "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.FMMSplitAll(1,@SqlContent);" +
                "SELECT " +
                    "Nodes.value('(@id)[1]', 'int') AS [序号]," +
                    "Nodes.value('(@pos)[1]', 'int') AS [位置]," +
                    "Nodes.value('(@cid)[1]', 'int') AS [内容标识]," +
                    "Nodes.value('(@term)[1]', 'int') AS [终结符]," + 
                    "Nodes.value('(@freq)[1]', 'int') AS [统计词频]," +
                    "Nodes.value('(text())[1]', 'nvarchar(max)') AS [内容] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [序号];";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlDID", did);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> BMMAllDictionaryByID(int did)
        {
            string query = "DECLARE @SqlContent UString;" +
                "SELECT TOP 1 @SqlContent = content FROM dbo.Dictionary WHERE did = @SqlDID;" +
                "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.BMMSplitAll(1,@SqlContent);" +
                "SELECT " +
                    "Nodes.value('(@id)[1]', 'int') AS [序号]," +
                    "Nodes.value('(@pos)[1]', 'int') AS [位置]," +
                    "Nodes.value('(@cid)[1]', 'int') AS [内容标识]," +
                    "Nodes.value('(@term)[1]', 'int') AS [终结符]," +
                    "Nodes.value('(@freq)[1]', 'int') AS [统计词频]," +
                    "Nodes.value('(text())[1]', 'nvarchar(max)') AS [内容] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [序号];";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlDID", did);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SplitExternalContentByID(int eid)
        {
            string query = "DECLARE @SqlContent UString;" +
                "SELECT TOP 1 @SqlContent = content FROM dbo.ExternalContent WHERE eid = @SqlEID;" +
                "DECLARE @SqlRules XML;" +
                "SET @SqlRules = dbo.LoadNumericalRules();" +
                "DECLARE @SqlExpressions XML;" +
                "SET @SqlExpressions = dbo.ExtractExpressions(@SqlRules,@SqlContent);" +
                "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.SplitContent(@SqlContent,@SqlExpressions);" +
                "SELECT " +
                    "ISNULL(Nodes.value('(@id)[1]', 'int'),0) AS [序号]," +
                    "ISNULL(Nodes.value('local-name(.)', 'nvarchar(max)'),'') AS [节点类]," +
                    "ISNULL(Nodes.value('(@name)[1]', 'nvarchar(max)'),'') AS [参数名]," +
                    "ISNULL(Nodes.value('(@pos)[1]', 'int'),0) AS [位置]," +
                    "ISNULL(Nodes.value('(@type)[1]', 'nvarchar(max)'),'') AS [类型]," +
                    "ISNULL(Nodes.value('(text())[1]', 'nvarchar(max)'),'') AS [内容] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [序号];";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlEID", eid);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> FMMSplitExternalContentByID(int eid)
        {
            string query = "DECLARE @SqlContent UString;" +
                "SELECT TOP 1 @SqlContent = content FROM dbo.ExternalContent WHERE eid = @SqlEID;" +
                "DECLARE @SqlRules XML;" +
                "SET @SqlRules = dbo.LoadNumericalRules();" +
                "DECLARE @SqlExpressions XML;" +
                "SET @SqlExpressions = dbo.ExtractExpressions(@SqlRules,@SqlContent);" +
                "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.FMMSplitContent(1,@SqlContent,@SqlExpressions);" +
                "SELECT " +
                    "ISNULL(Nodes.value('(@id)[1]', 'int'),0) AS [序号]," +
                    "ISNULL(Nodes.value('local-name(.)', 'nvarchar(max)'),'') AS [节点类]," +
                    "ISNULL(Nodes.value('(@pos)[1]', 'int'),0) AS [位置]," +
                    "ISNULL(Nodes.value('(@type)[1]', 'nvarchar(max)'),'') AS [类型]," +
                    "ISNULL(Nodes.value('(@cid)[1]', 'int'),0) AS [内容标识]," +
                    "ISNULL(Nodes.value('(text())[1]', 'nvarchar(max)'),'') AS [内容], " +
                    "ISNULL(Nodes.value('(@term)[1]', 'int'),0) AS [终结符], " + 
                    "ISNULL(Nodes.value('(@freq)[1]', 'int'),0) AS [词频统计] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [序号];";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlEID", eid);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> BMMSplitExternalContentByID(int eid)
        {
            string query = "DECLARE @SqlContent UString;" +
                "SELECT TOP 1 @SqlContent = content FROM dbo.ExternalContent WHERE eid = @SqlEID;" +
                "DECLARE @SqlRules XML;" +
                "SET @SqlRules = dbo.LoadNumericalRules();" +
                "DECLARE @SqlExpressions XML;" +
                "SET @SqlExpressions = dbo.ExtractExpressions(@SqlRules,@SqlContent);" +
                "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.BMMSplitContent(1,@SqlContent,@SqlExpressions);" +
                "SELECT " +
                    "ISNULL(Nodes.value('(@id)[1]', 'int'),0) AS [序号]," +
                    "ISNULL(Nodes.value('local-name(.)', 'nvarchar(max)'),'') AS [节点类]," +
                    "ISNULL(Nodes.value('(@pos)[1]', 'int'),0) AS [位置]," +
                    "ISNULL(Nodes.value('(@type)[1]', 'nvarchar(max)'),'') AS [类型]," +
                    "ISNULL(Nodes.value('(@cid)[1]', 'int'),0) AS [内容标识]," +
                    "ISNULL(Nodes.value('(text())[1]', 'nvarchar(max)'),'') AS [内容], " +
                    "ISNULL(Nodes.value('(@term)[1]', 'int'),0) AS [终结符], " +
                    "ISNULL(Nodes.value('(@freq)[1]', 'int'),0) AS [词频统计] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [序号];";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlEID", eid);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public void ContentDeleteValue(string content)
        {
            string query = "DECLARE @SqlInput UString;" +
                "SET @SqlInput = TRIM(@SqlContent);" +
                "EXEC dbo.ContentDeleteValue @SqlInput;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            this.DBToolbox.ExecuteNonQuery(query, parameters);
        }

        public void ContentInsertValue(string content)
        {
            string query = "DECLARE @SqlInput UString;" +
                "SET @SqlInput = TRIM(@SqlContent);" +
                "EXEC dbo.ContentInsertValue @SqlInput;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            this.DBToolbox.ExecuteNonQuery(query, parameters);
        }

        public void ContentSetTerminator(string content)
        {
            string query = "DECLARE @SqlInput UString;" +
                "SET @SqlInput = TRIM(@SqlContent);" +
                "EXEC dbo.SetTerminator @SqlInput;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            this.DBToolbox.ExecuteNonQuery(query, parameters);
        }

        public List<Dictionary<string, object>> GetPrefixFreqCount(string content)
        {
            string query = "SELECT COUNT(*) AS [总数] FROM " +
                "(SELECT DISTINCT length, content, count FROM dbo.Dictionary " +
                " WHERE enable = 1 AND content like @SqlLike AND content <> @SqlContent AND count > 0) AS T;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlLike", content + "%");
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectPrefixFreq(int pageIndex, int pageSize,string content)
        {
            string query = String.Format("SELECT length AS [长度],content AS [内容],count AS [统计词频],dbo.DictionaryGetClassificationInfo(content)AS [类别] FROM " +
                            "(" +
                            " SELECT DISTINCT length, content, count FROM dbo.Dictionary " +
                            " WHERE enable = 1 AND content like @SqlLike AND content <> @SqlContent AND count > 0" +
                            ") AS T " +
                            " ORDER BY count DESC, content OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY; ", pageIndex, pageSize);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlLike", content + "%");
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetSuffixFreqCount(string content)
        {
            string query = "SELECT COUNT(*) AS [总数] FROM " +
                "(SELECT DISTINCT length,content,count FROM dbo.Dictionary" +
                " WHERE enable = 1 AND content like @SqlLike AND content <> @SqlContent AND count > 0) AS T;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlLike", "%" + content);
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectSuffixFreq(int pageIndex, int pageSize, string content)
        {
            string query = String.Format("SELECT length AS [长度],content AS [内容],count AS [统计词频],dbo.DictionaryGetClassificationInfo(content)AS [类别] FROM " +
                            "(" +
                            " SELECT DISTINCT length, content, count FROM dbo.Dictionary " +
                            " WHERE enable = 1 AND content like @SqlLike AND content <> @SqlContent AND count > 0 " +
                            ") AS T " +
                            " ORDER BY count DESC, content OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY; ", pageIndex, pageSize);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlLike", "%" + content);
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> IsTerminator(string content)
        {
            string query = "SELECT dbo.IsTerminator(@SqlContent) AS cid;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetFreqCountByContent(string content)
        {
            string query = "SELECT did AS [标识], length AS [长度],content AS [内容],count AS [统计词频],classification AS [类别], enable AS [是否可用], ISNULL(remark,'') AS [备注] FROM dbo.Dictionary WHERE content = @SqlContent";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetRelatedFreqCount(string content)
        {
            string query = "SELECT COUNT(*) AS [总数] FROM " +
                "(SELECT DISTINCT length,content,count FROM dbo.Dictionary" +
                " WHERE enable = 1 AND content like @SqlLike AND content <> @SqlContent AND count > 0) AS T;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlLike", "%" + content + "%");
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectRelatedFreq(int pageIndex, int pageSize, string content)
        {
            string query = String.Format("SELECT dbo.ContentGetCID(content) AS [内容标识], length AS [长度],content AS [内容],count AS [统计词频],dbo.DictionaryGetClassificationInfo(content)AS [类别] FROM " +
            //string query = String.Format("SELECT dbo.ContentGetCID(content) AS [内容标识], length AS [长度],content AS [内容],count AS [统计词频] FROM " +
                            "(" +
                            " SELECT DISTINCT length, content, count FROM dbo.Dictionary " +
                            " WHERE enable = 1 AND content like @SqlLike AND content <> @SqlContent AND count > 0 " +
                            ") AS T " +
                            " ORDER BY count DESC, content OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY; ", pageIndex, pageSize);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlLike", "%" + content + "%");
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetEditablePhraseRuleCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.PhraseRule WHERE classification IN ('数词','数量词','短语');";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectEditablePhraseRule(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT rid AS [标识], classification AS [分类], dbo.XMLEscape([rule]) AS [规则项], attribute AS [属性], ISNULL(requirements,'') AS [其他要求]" +
                " FROM dbo.PhraseRule WHERE classification IN ('数词','数量词','短语','句子') ORDER BY rid DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> InsertPhraseRule(string rule, string classification, string attribute)
        {
            string query = "DECLARE @SqlRID INT;" +
                "EXEC @SqlRID = dbo.AddPhraseRule @SqlClassification, @SqlRule, @SqlAttribute;" +
                "IF @SqlRID > 0 UPDATE dbo.PhraseRule SET requirements = 'User' WHERE rid = @SqlRID;" +
                "SELECT TOP 1 rid,classification,[rule],attribute,requirements FROM dbo.PhraseRule WHERE rid = @SqlRID;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlClassification", classification);
            parameters.Add("SqlRule", rule);
            parameters.Add("SqlAttribute", attribute);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> UpdatePhraseRule(int rid, string rule, string classification, string attribute)
        {
            string query = "IF NOT EXISTS (SELECT * FROM dbo.PhraseRule WHERE rid <> @SqlRID AND [rule] = @SqlRule) " +
                "UPDATE dbo.PhraseRule SET [rule] = @SqlRule, classification = @SqlClassification, attribute = @SqlAttribute WHERE rid = @SqlRID;" +
                "SELECT TOP 1 rid,classification,[rule],attribute,requirements FROM dbo.PhraseRule WHERE rid = @SqlRID;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlRID", rid);
            parameters.Add("SqlClassification", classification);
            parameters.Add("SqlRule", rule);
            parameters.Add("SqlAttribute", attribute);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public void DeletePhraseRule(int rid, string rule)
        {
            string query = "DELETE FROM PhraseRule WHERE rid = @SqlRID AND [rule] = @SqlRule;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlRID", rid);
            parameters.Add("SqlRule", rule);

            this.DBToolbox.ExecuteNonQuery(query, parameters);
        }

        public void DeletePhraseRuleByID(int rid)
        {
            string query = "DELETE FROM PhraseRule WHERE rid = @SqlRID;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlRID", rid);

            this.DBToolbox.ExecuteNonQuery(query, parameters);
        }

        public List<Dictionary<string, object>> SelectPhraseRuleByRule(string rule)
        {
            string query = "SELECT TOP 1 rid,classification,[rule],attribute,requirements FROM dbo.PhraseRule WHERE [rule] = @SqlRule;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlRule", rule);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetSubsentenceRandomly()
        {
            string query = "SELECT TOP 1 * FROM dbo.ExternalContent WHERE classification = '分句' ORDER BY NEWID();  ";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> MatchPhraseRule(string rule,string content)
        {
            string query = "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.MatchExpressions(dbo.RecoverPhraseRule(@SqlRule),@SqlContent);" +
                "SELECT " +
                    "ISNULL(Nodes.value('(@id)[1]', 'int'),0) AS [序号]," +
                    "ISNULL(Nodes.value('local-name(.)', 'nvarchar(max)'),'') AS [节点类]," +
                    "ISNULL(Nodes.value('(@pos)[1]', 'int'),0) AS [位置]," +
                    "ISNULL(Nodes.value('(@len)[1]', 'int'),0) AS [长度]," +
                    "ISNULL(Nodes.value('(text())[1]', 'nvarchar(max)'),'') AS [内容] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [位置];";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlRule", rule);
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> ExtractPhrases(string content)
        {
            string query = "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.ExtractPhrases(@SqlContent);" +
                "SELECT " +
                    "ISNULL(Nodes.value('(@id)[1]', 'int'),0) AS [序号]," +
                    "ISNULL(Nodes.value('local-name(.)', 'nvarchar(max)'),'') AS [节点类]," +
                    "ISNULL(Nodes.value('(@type)[1]', 'nvarchar(max)'),0) AS [类型]," +
                    "ISNULL(Nodes.value('(@pos)[1]', 'int'),0) AS [位置]," +
                    "ISNULL(Nodes.value('(@len)[1]', 'int'),0) AS [长度]," +
                    "ISNULL(Nodes.value('(text())[1]', 'nvarchar(max)'),'') AS [内容] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [位置];";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetEditableParseRuleCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.ParseRule WHERE classification = '结构';";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectEditableParseRule(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT rid AS [标识], classification AS [分类], dbo.XMLEscape([rule]) AS [规则项], static_suffix AS [后缀], static_prefix AS [前缀], parameter_count AS [参数个数], minimum_length AS [最小长度], controllable_priority AS [优先级] " +
                " FROM dbo.ParseRule WHERE classification = '结构' ORDER BY rid DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }


        public List<Dictionary<string, object>> InsertParseRule(string rule)
        {
            string query = "DECLARE @SqlRID INT;" +
                "EXEC @SqlRID = dbo.AddParseRule '结构', @SqlRule;" +
                "SELECT TOP 1 rid,classification,[rule],static_suffix,static_prefix,parameter_count,minimum_length,controllable_priority FROM dbo.ParseRule WHERE rid = @SqlRID;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlRule", rule);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> UpdateParseRule(int rid, string rule)
        {
            string query = "IF NOT EXISTS (SELECT * FROM dbo.ParseRule WHERE rid <> @SqlRID AND [rule] = @SqlRule) " +
                "UPDATE dbo.ParseRule SET [rule] = @SqlRule WHERE rid = @SqlRID;" +
                "SELECT TOP 1 rid,classification,[rule],static_suffix,static_prefix,parameter_count,minimum_length,controllable_priority FROM dbo.ParseRule WHERE rid = @SqlRID;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlRID", rid);
            parameters.Add("SqlRule", rule);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public void DeleteParseRule(int rid, string rule)
        {
            string query = "DELETE FROM dbo.ParseRule WHERE rid = @SqlRID AND [rule] = @SqlRule;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlRID", rid);
            parameters.Add("SqlRule", rule);

            this.DBToolbox.ExecuteNonQuery(query, parameters);
        }

        public void DeleteParseRuleByID(int rid)
        {
            string query = "DELETE FROM dbo.ParseRule WHERE rid = @SqlRID;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlRID", rid);

            this.DBToolbox.ExecuteNonQuery(query, parameters);
        }

        public List<Dictionary<string, object>> SelectParseRuleByRule(string rule)
        {
            string query = "SELECT TOP 1 rid,classification,[rule],static_suffix,static_prefix,parameter_count,minimum_length,controllable_priority FROM dbo.ParseRule WHERE [rule] = @SqlRule;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlRule", rule);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetParseRuleByID(int rid)
        {
            string query = String.Format("SELECT rid,classification,[rule],static_suffix,static_prefix,parameter_count,minimum_length,controllable_priority" +
                " FROM dbo.ParseRule WHERE rid = {0};", rid);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> ParseContentByRule(string rule, string content)
        {
            string query = "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.MatchStructs(@SqlRule,@SqlContent);" +
                "SELECT " +
                    "ISNULL(Nodes.value('(@id)[1]', 'int'),0) AS [序号]," +
                    "ISNULL(Nodes.value('local-name(.)', 'nvarchar(max)'),'') AS [节点类]," +
                    "ISNULL(Nodes.value('(@name)[1]', 'nvarchar'),'') AS [参数名]," +
                    "ISNULL(Nodes.value('(@pos)[1]', 'int'),0) AS [位置]," +
                    "ISNULL(Nodes.value('(@len)[1]', 'int'),0) AS [长度]," +
                    "ISNULL(Nodes.value('(text())[1]', 'nvarchar(max)'),'') AS [内容] " +
                    " FROM @SqlResult.nodes('//result/row/*') AS N(Nodes) ORDER BY [位置];";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlRule", rule);
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> ExtractStructs(string content)
        {
            string query = "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.ExtractStructs(@SqlContent);" +
                "SELECT " +
                    "ISNULL(Nodes.value('(@id)[1]', 'int'),0) AS [序号]," +
                    "ISNULL(Nodes.value('local-name(.)', 'nvarchar(max)'),'') AS [节点类]," +
                    "ISNULL(Nodes.value('(@type)[1]', 'nvarchar(max)'),0) AS [类型]," +
                    "ISNULL(Nodes.value('(@pos)[1]', 'int'),0) AS [位置]," +
                    "ISNULL(Nodes.value('(@len)[1]', 'int'),0) AS [长度]," +
                    "ISNULL(Nodes.value('(text())[1]', 'nvarchar(max)'),'') AS [内容] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes) ORDER BY [位置];";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetStructRuleCount()
        {
            string query = "SELECT COUNT(*) AS [总数] FROM dbo.ParseRule WHERE classification IN ('结构');";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectStructRule(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT rid AS [标识], classification AS [分类], dbo.XMLEscape([rule]) AS [规则项], minimum_length AS [最小长度], parameter_count AS [参数个数], static_prefix AS [前缀], static_suffix AS [后缀], controllable_priority AS [优先级] " +
                " FROM dbo.ParseRule WHERE classification IN ('结构') ORDER BY rid DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> FMMSplitAllByStructRule(string content)
        {
            string query = "DECLARE @SqlExpressions XML;" +
                "SET @SqlExpressions = dbo.ExtractStructs(@SqlContent);" +
                "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.FMMSplitContent(1,@SqlContent,@SqlExpressions);" +
                "SELECT [序号],[节点类],[位置],[类型],[内容标识],[内容],[终结符],[词频统计] FROM " +
                "(SELECT " +
                    "ISNULL(Nodes.value('(@id)[1]', 'int'),0) AS [序号]," +
                    "ISNULL(Nodes.value('local-name(.)', 'nvarchar(max)'),'') AS [节点类]," +
                    "ISNULL(Nodes.value('(@pos)[1]', 'int'),0) AS [位置]," +
                    "ISNULL(Nodes.value('(@type)[1]', 'nvarchar(max)'),'') AS [类型]," +
                    "ISNULL(Nodes.value('(@cid)[1]', 'int'),0) AS [内容标识]," +
                    "ISNULL(Nodes.value('(text())[1]', 'nvarchar(max)'),'') AS [内容], " +
                    "ISNULL(Nodes.value('(@term)[1]', 'int'),0) AS [终结符], " +
                    "ISNULL(Nodes.value('(@freq)[1]', 'int'),0) AS [词频统计] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes)) AS T WHERE [节点类] <> 'rule' ORDER BY [序号];";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> BMMSplitAllByStructRule(string content)
        {
            string query = "DECLARE @SqlExpressions XML;" +
                "SET @SqlExpressions = dbo.ExtractStructs(@SqlContent);" +
                "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.BMMSplitContent(1,@SqlContent,@SqlExpressions);" +
                "SELECT [序号],[节点类],[位置],[类型],[内容标识],[内容],[终结符],[词频统计] FROM " +
                "(SELECT " +
                    "ISNULL(Nodes.value('(@id)[1]', 'int'),0) AS [序号]," +
                    "ISNULL(Nodes.value('local-name(.)', 'nvarchar(max)'),'') AS [节点类]," +
                    "ISNULL(Nodes.value('(@pos)[1]', 'int'),0) AS [位置]," +
                    "ISNULL(Nodes.value('(@type)[1]', 'nvarchar(max)'),'') AS [类型]," +
                    "ISNULL(Nodes.value('(@cid)[1]', 'int'),0) AS [内容标识]," +
                    "ISNULL(Nodes.value('(text())[1]', 'nvarchar(max)'),'') AS [内容], " +
                    "ISNULL(Nodes.value('(@term)[1]', 'int'),0) AS [终结符], " +
                    "ISNULL(Nodes.value('(@freq)[1]', 'int'),0) AS [词频统计] " +
                    " FROM @SqlResult.nodes('//result/*') AS N(Nodes)) AS T WHERE [节点类] <> 'rule' ORDER BY [序号];";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> MultilayerSplitAll(string content)
        {
            string query = "DECLARE @SqlResult XML;" +
                "SET @SqlResult = dbo.MultilayerSplitAll(@SqlContent);" +
                "SELECT " +
                "   nodeID AS [序号], " +
                "   nodePos AS [位置], " +
                "   nodeType AS [类型], " +
                "   dbo.ContentGetCID(nodeValue) AS [内容标识], " +
                "   nodeValue AS [内容], " +
                "   (CASE WHEN dbo.IsTerminator(nodeValue) > 0 THEN 1 ELSE 0 END) AS [终结符], " +
                "   dbo.GetFreqCount(nodeValue) AS [词频统计] FROM " +
                "(SELECT" +
                "   ISNULL(Nodes.value('(@id)[1]', 'int'), 0) AS nodeID, " +
                "   ISNULL(Nodes.value('(@pos)[1]', 'int'), 0) AS nodePos, " +
                "   Nodes.value('(@type)[1]', 'nvarchar(max)') AS nodeType, " +
                "   Nodes.value('(text())[1]', 'nvarchar(max)') AS nodeValue, " +
                "   ISNULL(Nodes.value('(@len)[1]', 'int'), 0) AS nodeLen " +
                "   FROM @SqlResult.nodes('//result/*') AS N(Nodes)) AS T " +
                "ORDER BY [位置]";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SqlContent", content);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, parameters);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetClearableDictionaryCount()
        {
            string query = "SELECT count(*) AS [总数] " +
                " FROM " +
                "(" +
                " SELECT DISTINCT length, content, count FROM dbo.Dictionary WHERE enable = 1 AND length > 1" +
                ") AS T " +
                " WHERE content NOT IN" +
                "(" +
                "SELECT DISTINCT content FROM dbo.Dictionary WHERE classification IN ('公司','公司缩写','名人','姓名','姓氏','日文名','新华字典','现代汉语词典','组织结构','成语')" +
                "); ";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectClearableDictionary(int pageIndex, int pageSize)
        {
            string query = String.Format("SELECT dbo.ContentGetCID(content) AS [内容标识], LEN(content) AS [长度], content AS [内容], count AS [统计词频], (CASE WHEN dbo.IsTerminator(content) > 0 THEN 1 ELSE 0 END) AS [终结符], dbo.DictionaryGetClassificationInfo(content) AS [类别] " +
                " FROM (" +
                "SELECT content, count " +
                " FROM " +
                "(" +
                " SELECT DISTINCT length, content, count FROM dbo.Dictionary WHERE enable = 1 AND length > 1" +
                ") AS T " +
                " WHERE content NOT IN" +
                "(" +
                    "SELECT DISTINCT content FROM dbo.Dictionary WHERE classification IN ('公司','公司缩写','名人','姓名','姓氏','日文名','新华字典','现代汉语词典','组织结构','成语')" +
                ") ) AS K " +
                " ORDER BY count DESC OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> GetAmbiguityCount()
        {
            string query = "SELECT count(*) AS [总数] FROM dbo.Ambiguity;";

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public List<Dictionary<string, object>> SelectAmbiguity(int pageIndex, int pageSize)
        {
            /*
            string query = String.Format("SELECT aid AS [标识], " +
                "content AS [内容]," +
                "(CASE WHEN dbo.ContentGetCID(content) > 0 THEN freq ELSE -1 END) AS [统计词频], " +
                "fmm_content AS [FMM内容], " +
                "(CASE WHEN dbo.ContentGetCID(fmm_content) > 0 THEN fmm_freq ELSE -1 END) AS [FMM词频], " +
                "bmm_content AS [BMM内容], " +
                "(CASE WHEN dbo.ContentGetCID(bmm_content) > 0 THEN bmm_freq ELSE -1 END) AS [BMM词频], " +
                "eid AS [语料标识], position AS [位置], length AS [长度], count AS [次数]," +
                "(CASE operation WHEN 0 THEN '待定' WHEN 1 THEN '合并解析' WHEN 2 THEN 'FMM解析' WHEN 3 THEN 'BMM解析' ELSE '禁用' END) AS [处理选择] " +
                "FROM dbo.Ambiguity ORDER BY content,fmm_content,bmm_content OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);
            */
            string query = String.Format("SELECT aid AS [标识], " +
                "content AS [内容]," +
                "fmm_content AS [FMM内容], " +
                "bmm_content AS [BMM内容], " +
                "dbo.CutAbbreviation(external_content,position,length,32) AS [语料原文], " +
                "eid AS [语料标识], " +
                //"(CASE operation WHEN 0 THEN '待定' WHEN 1 THEN '合并解析' WHEN 2 THEN 'FMM解析' WHEN 3 THEN 'BMM解析' ELSE '禁用' END) AS [处理选择] " +
                "operation AS [操作] " +
                "FROM" +
                "(" +
                    "SELECT t1.aid AS aid, t1.eid as eid, t1.position as position, t1.length as length, t1.content AS content,t1.fmm_content as fmm_content,t1.bmm_content AS bmm_content,t2.content AS external_content,t1.operation AS operation FROM dbo.Ambiguity AS T1 " +
                    "INNER JOIN dbo.ExternalContent AS T2 ON T1.eid = T2.eid" +
                ") AS T3 " +
                "ORDER BY operation,content,fmm_content,bmm_content OFFSET(({0} - 1) * {1}) ROWS FETCH NEXT {1} ROWS ONLY;", pageIndex, pageSize);

            List<List<Dictionary<string, object>>> results = this.DBToolbox.Query(query, null);
            if (results.Count > 0)
                return results[0];
            else
                return null;
        }

        public void DeleteAmbiguityByID(int id)
        {
            string query = String.Format("DELETE FROM dbo.Ambiguity WHERE aid IN (SELECT aid FROM dbo.Ambiguity AS T1 " +
                " INNER JOIN ( " +
                " SELECT content, fmm_content, bmm_content FROM dbo.Ambiguity WHERE aid = {0} " +
                " ) AS T2 ON t1.content = t2.content AND t1.fmm_content = t2.fmm_content AND t1.bmm_content = t2.bmm_content); ", id);

            this.DBToolbox.ExecuteNonQuery(query);
        }

        public void ClearAmbiguity()
        {
            string query = "DELETE FROM dbo.Ambiguity WHERE freq = 0 AND fmm_freq = 0 AND bmm_freq = 0;" +
                "DELETE FROM dbo.Ambiguity WHERE dbo.ContentGetCID(content) <= 0 AND dbo.ContentGetCID(fmm_content) <= 0 AND dbo.ContentGetCID(bmm_content) <= 0;";

            this.DBToolbox.ExecuteNonQuery(query);
        }

        public void UpdateAmbiguityOperationByID(int id,int operation)
        {
            string query = String.Format("UPDATE dbo.Ambiguity SET operation = {1} WHERE aid = {0} ", id, operation);

            this.DBToolbox.ExecuteNonQuery(query);
        }
    }
}