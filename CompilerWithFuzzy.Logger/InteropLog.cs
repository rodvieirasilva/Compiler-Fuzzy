using System;
using System.Runtime.InteropServices;
using Logger.Enums;

namespace Logger
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("6E51004A-7FC8-45F4-AE09-5661F4E32136")]
    [ProgId("Logger.Log")]
    public class InteropLog
    {

        #region Métodos
        #region BancoSqLite

        public void Debug(string msg)
        {
            Log.Debug(msg);
        }

        public void Debug(string msg, string jsonXml)
        {
            Log.Debug(msg, jsonXml);
        }
        public void Debug(string msg, string ex, string jsonXml, int typeLog)
        {
            Log.Debug(msg, jsonXml, new Exception(ex), (TypeLog)typeLog);
        }

        public void Release(string msg)
        {
            Log.Release(msg);
        }

        public void Release(string msg, string jsonXml)
        {
            Log.Release(msg, jsonXml);
        }

        public void Release(string msg, string ex, string jsonXml, int typeLog)
        {
            if (typeLog <= 0 || typeLog > (int)TypeLog.Warning)
            {
                typeLog = (int)TypeLog.Warning;
            }
            Log.Release(msg, jsonXml, new Exception(ex), (TypeLog)typeLog);
        }

        public void Info(string msg)
        {
            Log.Info(msg);
        }

        public void Info(string msg, string jsonXml)
        {
            Log.Info(msg, jsonXml);
        }

        public void Info(string msg, string jsonXml, string ex)
        {
            Log.Info(msg, jsonXml, new Exception(ex));
        }

        public void Error(string ex)
        {
            Log.Error(new Exception(ex));
        }

        public void Error(string msg, string ex)
        {
            Log.Error(msg, new Exception(ex));
        }

        public void Error(string msg, string ex, string jsonXml)
        {
            Log.Error(msg, jsonXml, new Exception(ex));
        }

        public void Warn(string msg)
        {
            Log.Warn(msg);
        }

        public void Warn(string msg, string jsonXml)
        {
            Log.Warn(msg, jsonXml);
        }

        public void Warn(string msg, string jsonXml, string ex)
        {
            Log.Warn(msg, jsonXml, new Exception(ex));
        }
        #endregion BancoSqLite

        #endregion Métodos
    }
}
