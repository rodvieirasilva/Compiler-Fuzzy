using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace CompilerWithFuzzy.Core
{
    [DataContract]
    [Serializable]
    public class MaxConorm : ConormAbstract
    {
        
        public override double Calculate(double a, double b)
        {
            return Math.Max(a, b);
        }


    }
}
