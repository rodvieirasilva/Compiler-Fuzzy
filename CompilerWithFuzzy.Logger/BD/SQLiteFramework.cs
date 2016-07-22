using Logger.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Logger.BD
{
    [ComVisible(false)]
    public class SQLiteFramework
    {
        #region Attributes
        private static SQLiteConnection conn;
        #endregion Attributes

        public SQLiteFramework()
        {
            string connectionString = @"Data Source=" + Params.PathDataBase + ";Version=3;";
            bool create = false;
            if (!File.Exists(Params.PathDataBase))
            {
                SQLiteConnection.CreateFile(Params.PathDataBase);
                create = true;
            }
            conn = new SQLiteConnection(connectionString);
            if (create)
                CreateDataBase();
        }


        #region Métodos SQL

        public SQLiteDataReader ReturnReader(string command, SQLiteParameter[] parameters = null)
        {
            SQLiteDataReader reader;

            using (SQLiteCommand comma = new SQLiteCommand(conn))
            {
                conn.Open();

                if (parameters != null)
                    comma.Parameters.AddRange(parameters);

                comma.CommandText = command;
                reader = comma.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }

            return reader;
        }

        public object ReturnaSingleValue(string command, SQLiteParameter[] parameters = null)
        {
            object retorno;

            using (SQLiteCommand comma = new SQLiteCommand(conn))
            {
                conn.Open();

                if (parameters != null)
                    comma.Parameters.AddRange(parameters);

                comma.CommandText = command;
                retorno = comma.ExecuteScalar(System.Data.CommandBehavior.CloseConnection);
                conn.Close();
            }

            return retorno;
        }

        public int Execute(string command, SQLiteParameter[] parameters = null)
        {
            int returns = 0;

            using (SQLiteCommand comma = new SQLiteCommand(conn))
            {
                conn.Open();

                if (parameters != null)
                    comma.Parameters.AddRange(parameters);

                comma.CommandText = command;
                returns = comma.ExecuteNonQuery(System.Data.CommandBehavior.CloseConnection);
                conn.Close();
            }

            return returns;
        }

        public void Connect()
        {
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
        }

        public void Close()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }

        #endregion


        #region CreateDataBase
        public void CreateDataBase()
        {
            Execute(
                    @"-- Table: TypeLog
                    CREATE TABLE TypeLog ( 
                        id   INTEGER PRIMARY KEY AUTOINCREMENT
                                     NOT NULL
                                     UNIQUE,
                        name TEXT    NOT NULL
                                     UNIQUE 
                    );

                    INSERT INTO [TypeLog] ([id], [name]) VALUES (1, 'Error');
                    INSERT INTO [TypeLog] ([id], [name]) VALUES (2, 'Information');
                    INSERT INTO [TypeLog] ([id], [name]) VALUES (3, 'Warning');

                    -- Table: ApplicationMode
                    CREATE TABLE ApplicationMode ( 
                        id   INTEGER PRIMARY KEY AUTOINCREMENT
                                     NOT NULL
                                     UNIQUE,
                        name TEXT    NOT NULL
                                     UNIQUE 
                    );

                    INSERT INTO [ApplicationMode] ([id], [name]) VALUES (1, 'Debug');
                    INSERT INTO [ApplicationMode] ([id], [name]) VALUES (2, 'Release');

                    -- Table: TypeObject
                    CREATE TABLE TypeObject ( 
                        id   INTEGER PRIMARY KEY AUTOINCREMENT
                                     UNIQUE,
                        name TEXT    UNIQUE 
                    );


                    -- Table: Application
                    CREATE TABLE Application ( 
                        id         INTEGER  PRIMARY KEY AUTOINCREMENT
                                            NOT NULL
                                            UNIQUE,
                        name       TEXT     NOT NULL
                                            UNIQUE,
                        pathExe    TEXT,
                        pathConfig TEXT,
                        createDt   DATETIME NOT NULL
                                            DEFAULT ( date( 'now' )  ) 
                    );


                    -- Table: Message
                    CREATE TABLE Message ( 
                        id                INTEGER  PRIMARY KEY AUTOINCREMENT
                                                   NOT NULL
                                                   UNIQUE,
                        idApplication     INTEGER  REFERENCES Application ( id ),
                        createDt          DATETIME NOT NULL
                                                   DEFAULT ( date( 'now' )  ),
                        idTypeLog         INTEGER  NOT NULL
                                                   DEFAULT ( 1 ) 
                                                   REFERENCES TypeLog ( id ),                        
                        msg               TEXT,
                        exception         TEXT,
                        stacktrace        TEXT,                        
                        domain            TEXT,
                        username          TEXT,
                        hostname          TEXT,
                        ips               TEXT,
                        class             TEXT,
                        method            TEXT,
                        line              INTEGER,
                        
                        idTypeObject      INTEGER  REFERENCES TypeObject ( id ),
                        idObject          INTEGER,
                        idApplicationMode INTEGER  NOT NULL
                                                   DEFAULT ( 2 ) 
                                                   REFERENCES ApplicationMode ( id ),
                        thread            TEXT
                    );

                    ");
        }
        #endregion CreateDataBase
    }
}
