using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerWithFuzzy.GrammarFuzzy
{
    public static class Extension
    {

     public static bool IsTerminal(this char t)
        {
            return Char.IsLower(t) || Char.IsDigit(t);
        }

      public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

       
    }
}
