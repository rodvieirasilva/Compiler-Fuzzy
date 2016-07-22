using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompilerWithFuzzy.Core
{
    public static class Utils
    {

        public static string Serialize(object obj)
        {

          

            return JsonConvert.SerializeObject(obj, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                        TypeNameHandling = TypeNameHandling.All
                    });
        }

        public static T DeSerialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                        TypeNameHandling = TypeNameHandling.All
                    });
        }

        public static void SaveTime(string description, DateTime date)
        {
           // File.AppendAllText(".\\times.txt",
           //     string.Format("{0}: {1}\r\n", description, ((DateTime.Now - date).TotalSeconds)));

        }

        public static System.Drawing.Color GetColor(double pertinence)
        {

            var r = 255;
            var g = 255;

            if (pertinence > 0.95)
            {
                r = 0;
                g = 153;
            }
            else if (pertinence > 0.85)
            {
                r = 0;
                g = 255;
            }
            else if (pertinence > 0.75)
            {
                r = 128;
                g = 255;
            }
            else if (pertinence >= 0.5)
            {
                r = 255;
                g = 255;
            }
            else if (pertinence > 0.3)
            {
                r = 255;
                g = 128;
            }
            else// if (pertinence > 0.3)
            {
                r = 255;
                g = 0;
            }
            return System.Drawing.Color.FromArgb(r, g, 0);
        }

        public static string GetNumbers(string st)
        {
            string result = string.Empty;
            for (int i = 0; i < st.Length; i++)
            {
                if (Char.IsDigit(st[i]))
                {
                    result += st[i];
                }
            }
            return result;
        }
    }
}
