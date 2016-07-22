using Logger.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;

namespace Logger.Model
{
    public class Message
    {
        public int Id { get; set; }
        public int IdApplication { get; set; }
        public DateTime CreateDt { get; set; }
        public TypeLog TypeLog { get; set; }
        public string Thread { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Hostname { get; set; }
        public string Ips { get; set; }
        public string Exception { get; set; }
        public string Msg { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public int Line { get; set; }
        public string Stacktrace { get; set; }
        public int IdTypeObject { get; set; }
        public int IdObject { get; set; }
        public ApplicationMode ApplicationMode { get; set; }

        public Message()
        {
            TypeLog = Enums.TypeLog.Error;
            Thread = string.Format(", CurrentThreadName:{0}, CurrentThreadManagedThreadId:{1}, CurrentThreadCurrentCulture:{2}, CurrentThreadPriority:{3}",
                System.Threading.Thread.CurrentContext,
                System.Threading.Thread.CurrentThread.Name,
                System.Threading.Thread.CurrentThread.ManagedThreadId,
                System.Threading.Thread.CurrentThread.CurrentCulture,
                System.Threading.Thread.CurrentThread.Priority);
            Domain = Environment.UserDomainName;
            Username = Environment.UserName;
            Hostname = Environment.MachineName;
            try
            {
                Ips = string.Join(", ", Dns.GetHostEntry(Dns.GetHostName()).AddressList.Select(ip => ip.ToString()));
            }
            catch (Exception ex)
            { }

            System.Diagnostics.StackTrace stack = new System.Diagnostics.StackTrace();
            ApplicationMode = Params.ApplicationMode;

            Stacktrace = string.Empty;
            foreach (var item in stack.GetFrames())
            {
                Stacktrace += string.Format("{0}: Class: {1}; Method: {2}; Line: {3}; Collumn: {4}; \r\n", item.GetFileName(), item.GetMethod().DeclaringType.FullName,
                        item.GetMethod().Name, item.GetFileLineNumber(), item.GetFileColumnNumber());
            }
        }

        public override string ToString()
        {
            return string.Format(@"
{0}|{1}:{2}                        
Exception: {3},
Stacktrace: {4},
Domain: {5},
Username: {6},
Hostname: {7},
Ips: {8},
Class: {9},
Method: {10},
Line: {11},
IdTypeObject: {12},
IdObject: {13},
ApplicationMode: {14},
Thread: {15}
{16}
",
                        DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                        Enum.GetName(TypeLog.GetType(), TypeLog),
                        Msg,
                        Exception,
                        Stacktrace,
                        Domain,
                        Username,
                        Hostname,
                        Ips,
                        Class,
                        Method,
                        Line,
                        IdTypeObject,
                        IdObject,
                        Enum.GetName(ApplicationMode.GetType(), ApplicationMode),
                        Thread,
                        new String('-', 5));
        }
    }
}
