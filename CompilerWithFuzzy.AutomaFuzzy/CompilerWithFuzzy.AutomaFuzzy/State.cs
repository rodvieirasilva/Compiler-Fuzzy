using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CompilerWithFuzzy.AutomaFuzzy
{
    [DataContract]
    [Serializable]
    public class State<T>
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double PertinenceInitial { get; set; }

        [DataMember]
        public double PertinenceFinal { get; set; }

        [DataMember]
        public List<Transition<T>> Transitions { get; set; }

        [DataMember]
        public object Value { get; set; }


        public State()
        { }
        public State(int id, string name, double pertinenceInitial, double pertinenceFinal, object value = null)
        {
            this.Name = name;
            this.PertinenceInitial = pertinenceInitial;
            this.PertinenceFinal = pertinenceFinal;
            this.Transitions = new List<Transition<T>>();
            this.Value = value;

            if (id < 0)
            {
                ids++;
                Id = ids;
            }
        }

        public U GetValue<U>()
        {
            return (U)Value;
        }

        private static int ids;


        public override string ToString()
        {  
            if (Value != null)
            {
                string strVa = Value.ToString();

                int maior = strVa.Split('\r').Max(s => s.Length);
                int meio = maior - this.Name.Length;
                if (meio > 0)
                {
                    return string.Format("{0}{1}{0}\r\n{2}", new string(' ', meio), this.Name, Value.ToString());
                }                
                return string.Format("{0}\r\n{1}", this.Name, Value.ToString());
            }
            return this.Name;
        }


        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
