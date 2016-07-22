using Logger.BD;
using Logger.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Logger.Business
{
    public class BLog
    {
        #region Attributes
        private SQLiteFramework bd;
        private static bool criaEstrutura = true;
        #endregion Attributes

        #region Constructor
        public BLog()
        {
            bd = new SQLiteFramework();
        }
        #endregion Constructor

        #region Messages
        public void InsertMessage(Message message)
        {
            string insert = @"INSERT INTO MESSAGE(   idApplication,
                                                    idTypeLog       ,
                                                    thread          ,
                                                    domain          ,
                                                    username        ,
                                                    hostname        ,
                                                    ips,
                                                    exception       ,
                                                    msg             ,
                                                    class           ,
                                                    method          ,
                                                    line            ,
                                                    stacktrace      ,
                                                    idTypeObject    ,
                                                    idObject        ,
                                                    idApplicationMode) values (
                                                                @idApplication,
                                                                @idTypeLog       ,
                                                                @thread          ,
                                                                @domain          ,
                                                                @username        ,
                                                                @hostname        ,
                                                                @ips,
                                                                @exception       ,
                                                                @msg             ,
                                                                @class           ,
                                                                @method          ,
                                                                @line            ,
                                                                @stacktrace      ,
                                                                @idTypeObject    ,
                                                                @idObject        ,
                                                                @idApplicationMode)";
            List<SQLiteParameter> lstParametros = new List<SQLiteParameter>();

            object tempValue;
            if (message.IdApplication <= 0)
            {
                tempValue = DBNull.Value;
            }
            else
            {
                tempValue = message.IdApplication;
            }
            lstParametros.Add(new SQLiteParameter("@idApplication", tempValue));
            if (((int)message.TypeLog) <= 0)
            {
                tempValue = DBNull.Value;
            }
            else
            {
                tempValue = message.TypeLog;
            }
            lstParametros.Add(new SQLiteParameter("@idTypeLog", tempValue));

            lstParametros.Add(new SQLiteParameter("@thread", message.Thread));
            lstParametros.Add(new SQLiteParameter("@domain", message.Domain));
            lstParametros.Add(new SQLiteParameter("@username", message.Username));
            lstParametros.Add(new SQLiteParameter("@hostname", message.Hostname));
            lstParametros.Add(new SQLiteParameter("@ips", message.Ips));
            lstParametros.Add(new SQLiteParameter("@exception", message.Exception));
            lstParametros.Add(new SQLiteParameter("@msg", message.Msg));
            lstParametros.Add(new SQLiteParameter("@class", message.Class));
            lstParametros.Add(new SQLiteParameter("@method", message.Method));
            lstParametros.Add(new SQLiteParameter("@line", message.Line));
            lstParametros.Add(new SQLiteParameter("@stacktrace", message.Stacktrace));
            if (message.IdTypeObject <= 0)
            {
                tempValue = DBNull.Value;
            }
            else
            {
                tempValue = message.IdTypeObject;
            }
            lstParametros.Add(new SQLiteParameter("@idTypeObject", tempValue));
            if (message.IdObject <= 0)
            {
                tempValue = DBNull.Value;
            }
            else
            {
                tempValue = message.IdObject;
            }
            lstParametros.Add(new SQLiteParameter("@idObject", tempValue));
            if ((int)message.ApplicationMode <= 0)
            {
                tempValue = DBNull.Value;
            }
            else
            {
                tempValue = (int)message.ApplicationMode;
            }
            lstParametros.Add(new SQLiteParameter("@idApplicationMode", tempValue));

            bd.Execute(insert, lstParametros.ToArray());
        }

        public void AddCollumns(List<string> collumns, string table)
        {
            string command = string.Empty;
            string union = string.Empty;
            foreach (var item in collumns)
            {
                command += string.Format(@"{0} SELECT '{1}'
                                            FROM sqlite_master where name = '{2}' and sql not like '%idCustomObject{2}  Integer%{1}%' ", union, item, table);
                union = "UNION";
            }
            if (!string.IsNullOrEmpty(command))
            {
                using (var reader = bd.ReturnReader(command))
                {
                    command = string.Empty;

                    while (reader.Read())
                    {
                        command += string.Format("ALTER TABLE {0} ADD {1} TEXT;\r\n", table, reader[0]);
                    }
                    reader.Close();
                    if (!string.IsNullOrWhiteSpace(command))
                        bd.Execute(command);
                }
            }
        }

        private void AddTable(string table)
        {
            string command = string.Format(@"create table if not exists {0} 
                    (idCustomObject{0}  Integer primary key AUTOINCREMENT, 
                     createDtCustomObject{0} DateTime DEFAULT ( date( 'now' )  )                 
                     )", table);

            bd.Execute(command);
        }


        public int InsertTypeObject(string type)
        {

            string command = string.Format(@"SELECT id FROM TypeObject where name = '{0}'", type);


            object id = bd.ReturnaSingleValue(command);
            if (id != null && id != DBNull.Value)
                return Convert.ToInt32(id);

            command = string.Format(@"INSERT INTO TypeObject ( name ) VALUES ('{0}'); SELECT last_insert_rowid();", type);
            id = bd.ReturnaSingleValue(command);
            if (id != null && id != DBNull.Value)
                return Convert.ToInt32(id);

            return 0;
        }


        public int InsertObject(string type, Hashtable colunas, out  int typeObject)
        {
            string fields = string.Empty;
            List<string> collumns = new List<string>();
            foreach (DictionaryEntry item in colunas)
            {
                string key = Convert.ToString(item.Key);
                collumns.Add(key);
                fields += key + ",";
            }
            AddTable(type);
            AddCollumns(collumns, type);
            typeObject = InsertTypeObject(type);


            fields = fields.Trim(',');
            string command = string.Format(@"insert into {0} ({1}) values (@{2}); 
                SELECT last_insert_rowid();", type, fields, fields.Replace(",", ",@"));

            var lstParametros = new List<SQLiteParameter>();
            foreach (DictionaryEntry item in colunas)
            {
                lstParametros.Add(new SQLiteParameter("@" + item.Key, Convert.ToString(item.Value)));
            }

            object id = bd.ReturnaSingleValue(command, lstParametros.ToArray());
            if (id != null && id != DBNull.Value)
                return Convert.ToInt32(id);
            return 0;
        }

        public int InsertApplication()
        {
            string command = string.Format(@"SELECT id FROM Application where name = '{0}'", Params.ApplicationName);


            object id = bd.ReturnaSingleValue(command);
            if (id != null && id != DBNull.Value)
                return Convert.ToInt32(id);

            command = string.Format(@"INSERT INTO Application ( name, pathExe, pathConfig) 
                VALUES ('{0}','{1}', '{2}' ); SELECT last_insert_rowid();",
                    Params.ApplicationName, Params.PathExe, Params.PathConfig);
            id = bd.ReturnaSingleValue(command);
            if (id != null && id != DBNull.Value)
                return Convert.ToInt32(id);

            return 0;
        }

        #endregion Messages
    }
}
