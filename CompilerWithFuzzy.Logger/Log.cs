using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Logger.Business;
using System.Xml.Linq;
using System.Collections;
using Newtonsoft.Json;
using System.Diagnostics;
using Logger.Enums;
using Logger.Model;

namespace Logger
{
    public class Log
    {
        #region Atributos
        private static BLog business;

        private static object mutex = new object();
        #endregion Atributos

        #region Construtor
        static Log()
        {
            try
            {
                WriteResourceInteropToFile();
                business = new BLog();
            }
            catch (Exception ex)
            { }
        }
        #endregion Construtor

        #region Métodos


        #region BancoSqLite


        public static void Time(string msg, DateTime date)
        {

            if (Params.ApplicationMode == ApplicationMode.Debug)
                SaveMessage(msg, null, new
                {
                    DateInitial = date,
                    DateFim = DateTime.Now,
                    TotalMili = (DateTime.Now - date).TotalMilliseconds,
                    TotalSeconds = (DateTime.Now - date).TotalSeconds
                }, TypeLog.Information);
        }


        public static void Debug(string msg, Exception ex = null, object obj = null, TypeLog typeLog = TypeLog.Information)
        {
            if (ex != null)
            {
                typeLog = TypeLog.Error;
            }
            if (Params.ApplicationMode == ApplicationMode.Debug)
                SaveMessage(msg, ex, obj, typeLog);
        }

        public static void Release(string msg, Exception ex = null, object obj = null, TypeLog typeLog = TypeLog.Information)
        {
            if (ex != null)
            {
                typeLog = TypeLog.Error;
            }
            if (Params.ApplicationMode == ApplicationMode.Release)
                SaveMessage(msg, ex, obj, typeLog);
        }

        public static void Info(string msg, Exception ex = null, object obj = null)
        {
            SaveMessage(msg, ex, obj, TypeLog.Information);
        }

        public static void Error(Exception ex, object obj)
        {
            SaveMessage(string.Empty, ex, obj, TypeLog.Error);
        }

        public static void Error(string msg, Exception ex = null, object obj = null)
        {
            SaveMessage(msg, ex, obj, TypeLog.Error);
        }

        public static void Warn(string msg, Exception ex = null, object obj = null)
        {
            SaveMessage(msg, ex, obj, TypeLog.Warning);

        }

        public static void Debug(string msg, string jsonXml, Exception ex = null, TypeLog typeLog = TypeLog.Information)
        {
            Debug(msg, ex, GetObject(jsonXml), typeLog);
        }

        public static void Release(string msg, string jsonXml, Exception ex = null, TypeLog typeLog = TypeLog.Information)
        {
            Release(msg, ex, GetObject(jsonXml), typeLog);
        }

        public static void Info(string msg, string jsonXml, Exception ex = null)
        {
            Info(msg, ex, GetObject(jsonXml));
        }

        public static void Error(Exception ex, string jsonXml = null)
        {
            Error(null, ex, GetObject(jsonXml));
        }

        public static void Error(string msg, string jsonXml, Exception ex = null)
        {
            Error(msg, ex, GetObject(jsonXml));
        }

        public static void Warn(string msg, string jsonXml, Exception ex = null)
        {
            Warn(msg, ex, GetObject(jsonXml));
        }

        public static void SaveMessageText(Message message)
        {
            lock (mutex)
            {
                string filename = Path.Combine(Params.DirectoryText, string.Format("{0}-{1}-{2}.log", Params.ApplicationName, DateTime.Now.Year, DateTime.Now.Month));
                File.AppendAllText(filename, message.ToString());
            }
        }
        public static void SaveMessage(string msg, Exception ex, object obj, TypeLog type)
        {
            try
            {
                Message message = new Message();
                message.Msg = msg;

                if (ex != null)
                {
                    message.Exception = ex.ToString();
                }
                message.TypeLog = type;

                StackTrace stack = new StackTrace();
                var frames = stack.GetFrames();
                foreach (var item in frames)
                {
                    if (!(item.GetMethod().DeclaringType.Name == "Log" && item.GetMethod().DeclaringType.Namespace == "Logger"))
                    {
                        message.Method = item.GetMethod().ToString();
                        message.Class = item.GetMethod().DeclaringType.Name;
                        message.Line = item.GetFileLineNumber();
                        break;
                    }
                }
                if ((Params.OutputLog & OutputLog.Text) == OutputLog.Text)
                {
                    SaveMessageText(message);
                }
                if ((Params.OutputLog & OutputLog.SQLite) == OutputLog.SQLite)
                {
                    int idtTypeObj = 0;
                    int idObj = 0;
                    if (obj != null)
                    {
                        Type typeObj = obj.GetType();
                        Hashtable colluns;
                        if (obj.GetType() == typeof(Hashtable))
                        {
                            colluns = (Hashtable)obj;
                        }
                        else
                        {
                            colluns = SerializeObject(obj);
                        }

                        idObj = business.InsertObject(typeObj.Name, colluns, out idtTypeObj);
                    }

                    int idAppliction = business.InsertApplication();
                    message.IdApplication = idAppliction;

                    message.IdObject = idObj;
                    message.IdTypeObject = idtTypeObj;
                    business.InsertMessage(message);
                }
            }
            catch (Exception exception)
            { }
        }

        private static Hashtable SerializeObject(object obj)
        {

            Hashtable hash = new Hashtable();
            try
            {
                if (obj != null)
                {
                    Type tipo = obj.GetType();
                    foreach (PropertyInfo inf in tipo.GetProperties())
                    {
                        if (inf.GetIndexParameters().Length == 0)
                        {
                            string valor = inf.GetValue(obj, null) != null ? inf.GetValue(obj, null).ToString() : string.Empty;
                            hash.Add(inf.Name, valor);
                        }
                    }
                    if (obj != null)
                    {
                        hash.Add("ToString", obj.ToString());
                    }
                }
            }
            catch (Exception ex)
            { }
            return hash;
        }

        private static Hashtable GetObject(string jsonXml)
        {
            Hashtable retorno = null;
            XElement element;
            Dictionary<object, object> dic;
            try
            {
                element = XElement.Parse(jsonXml);
                dic = element.Descendants()
                                  .ToDictionary(d => (object)d.Name.LocalName,
                                  d => (object)d.Value);
                dic.Add("ToString", jsonXml);
                return new Hashtable(dic);
            }
            catch (Exception ex)
            {
                try
                {
                    dic = JsonConvert.DeserializeObject<Dictionary<object, object>>(jsonXml);
                    dic.Add("ToString", jsonXml);
                    return new Hashtable(dic);
                }
                catch (Exception ex2)
                { }
            }
            return retorno;
        }
        #endregion  Banco SQLite


        private static void WriteResourceInteropToFile()
        {
            try
            {
                string filename = Params.PathSQLiteInterop;
                if (!File.Exists(filename))
                {
                    File.WriteAllBytes(filename, Logger.Properties.Resources.SQLite_Interop);
                }
                filename = Params.PathSQLiteInteropx86;
                if (!File.Exists(filename))
                {
                    File.WriteAllBytes(filename, Logger.Properties.Resources.SQLite_Interopx86);
                }

                int wsize = IntPtr.Size;
                string libdir = (wsize == 4) ? Params.PathSQLiteInteropx86 : Params.PathSQLiteInterop;
                SetDllDirectory(Path.GetDirectoryName(libdir));
            }
            catch (Exception ex)
            { }
        }


        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool SetDllDirectory(string lpPathName);
        #endregion Métodos
    }
}
