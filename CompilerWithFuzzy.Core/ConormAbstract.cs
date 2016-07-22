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
    public abstract class ConormAbstract
    {
        public abstract double Calculate(double a, double b);

    
        public double Calculate<TSource>(IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            double ret = 0;
            if (source.Count() > 0)
            {

                foreach (var item in source)
                {
                    ret = Calculate(ret, selector(item));
                }
            }
            return ret;
        }
    }
}
