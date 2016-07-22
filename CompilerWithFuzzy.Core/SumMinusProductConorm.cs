using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CompilerWithFuzzy.Core
{
    [DataContract]
    [Serializable]
    public class SumMinusProductConorm : ConormAbstract
    {
        public override double Calculate(double a, double b)
        {
            return ((a + b) - a * b);
        }


    }
}
