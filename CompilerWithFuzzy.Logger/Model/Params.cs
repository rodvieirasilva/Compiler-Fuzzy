using Logger.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Logger.Model
{
    public static class Params
    {
        static Configuration config;
        static Params()
        {
            try
            {
                if (System.Web.HttpContext.Current != null)
                {
                    config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                }
                else
                {
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }

                if (config == null)
                {
                    string path = Environment.GetCommandLineArgs()[0];
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        if (!path.ToUpper().Contains("iis"))
                        {
                            config = ConfigurationManager.OpenExeConfiguration(Environment.GetCommandLineArgs()[0]);
                        }
                    }

                }
            }
            catch (Exception ex)
            { }
        }



        public static ApplicationMode ApplicationMode
        {
            get
            {
                try
                {
                    if (GetValueConfig<bool>("DebugMode", true))
                    {
                        return Enums.ApplicationMode.Debug;
                    }
                    return Enums.ApplicationMode.Release;
                }
                catch (Exception ex)
                {
                    return Enums.ApplicationMode.Release;
                }
            }
        }

        public static string ApplicationName
        {
            get
            {
                try
                {
                    return GetValueConfig<string>("ApplicationName", Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().ProcessName));
                }
                catch (Exception ex)
                {
                    return "ApplicationGreen";
                }

            }
        }

        public static OutputLog OutputLog
        {
            get
            {
                try
                {
                    return (OutputLog)GetValueConfig<int>("OutputLog", (int)OutputLog.SQLite);
                }
                catch (Exception ex)
                {
                    return OutputLog.SQLite;
                }
            }
        }


        public static string PathConfig
        {
            get
            {
                return config.FilePath;
            }
        }

        public static string CurrentFolder
        {
            get
            {
                try
                {
                    string temp = string.Empty;
                    if (System.Web.HttpContext.Current != null)
                    {
                        return System.Web.HttpContext.Current.Server.MapPath("~") + "bin\\";
                    }
                    else
                    {
                        temp = Process.GetCurrentProcess().MainModule.FileName;

                        if (string.IsNullOrWhiteSpace(temp))
                        {
                            return Path.GetDirectoryName(temp);
                        }

                        if (Environment.GetCommandLineArgs().Length > 0)
                        {
                            temp = Environment.GetCommandLineArgs()[0];

                            if (!string.IsNullOrWhiteSpace(temp))
                            {
                                if (!temp.ToUpper().Contains("iis"))
                                {
                                    return Path.GetDirectoryName(temp);
                                }
                            }
                        }
                    }
                    return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }
                catch (Exception ex)
                {
                    return ".\\" + ApplicationName + ".exe";
                }

            }
        }

        public static string PathExe
        {
            get
            {
                return Environment.GetCommandLineArgs()[0];
            }
        }

        public static string DirectoryText
        {
            get
            {
                try
                {
                    return GetValueConfig<string>("DirectoryText", CurrentFolder);
                }
                catch (Exception ex)
                {
                    return ".\\";
                }
            }
        }

        public static string PathDataBase
        {
            get
            {
                try
                {
                    return GetValueConfig<string>("PathDataBase", Path.Combine(CurrentFolder, "Log.sdb"));
                }
                catch (Exception ex)
                {
                    return "Log.sdb";
                }
            }
        }

        public static string PathSQLiteInteropx86
        {
            get
            {
                string pathSqlLite = Path.Combine(CurrentFolder, "x86\\");
                if (!Directory.Exists(pathSqlLite))
                {
                    Directory.CreateDirectory(pathSqlLite);
                }
                return pathSqlLite + "SQLite.Interop.dll";
            }
        }

        public static string PathSQLiteInterop
        {
            get
            {
                string pathSqlLite = Path.Combine(CurrentFolder, "x64\\");
                if (!Directory.Exists(pathSqlLite))
                {
                    Directory.CreateDirectory(pathSqlLite);
                }
                return pathSqlLite + "SQLite.Interop.dll";
            }
        }

        private static T GetValueConfig<T>(string name, T defaultValue)
        {
            if (config.AppSettings.Settings.AllKeys.Contains(name) && !string.IsNullOrWhiteSpace(config.AppSettings.Settings[name].Value))
            {
                object value = Convert.ChangeType(config.AppSettings.Settings[name].Value, typeof(T));

                if (typeof(T) == typeof(string))
                {
                    if (value != null)
                        value = (value as string).Replace("{SERVERPATH}", System.Web.HttpContext.Current.Server.MapPath("~"));
                }
                return (T)value;
            }
            return defaultValue;
        }
    }
}
