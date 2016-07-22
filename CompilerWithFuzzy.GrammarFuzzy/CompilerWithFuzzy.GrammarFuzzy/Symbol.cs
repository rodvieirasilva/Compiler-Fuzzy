using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CompilerWithFuzzy.GrammarFuzzy
{
    [DataContract]
    [Serializable]
    public class Symbol : IComparable<Symbol>, IEquatable<Symbol>, ICloneable
    {

        [DataMember]
        public static Symbol EmptySymbol = new Symbol(0, "EmptySymbol", true, 'ε');

        [DataMember]
        public static Symbol EmptyTerminal = new Symbol(-1, "EmptyTerminal", true);

        [DataMember]
        public static Symbol EmptyVariable = new Symbol(-2, "EmptyVariable", false, ' ');

        [DataMember]
        public static Symbol StackFinal = new Symbol(-3, "StackFinal", false, '?');

        [DataMember]
        public static Symbol TapeFinal = new Symbol(-4, "TapeFinal", false, '?');

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool Terminal { get; set; }
        [DataMember]
        public char Value { get; set; }

        [DataMember]
        public bool GoToEmpty { get; set; }

        [DataMember]
        public Dictionary<string, object> CustomValue { get; set; }

        public Symbol()
        {

        }

        public Symbol(int id, char name, bool terminal = false, char value = '\0')
            : this(id, name + "", terminal, value)
        {
        }
        public Symbol(int id, string name, bool terminal = false, char value = '\0')
        {
            if (id < 0)
            {
                id = newId++;
            }

            Id = id;
            Name = name;
            name = "Var";
            if (terminal)
            {
                name = "Term";
                Terminal = true;
                Value = value;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = string.Format("{0}{1:000}", name, id);
            }
            CustomValue = new Dictionary<string, object>();
        }

        public static bool operator ==(Symbol x, Symbol y)
        {
            if (Object.ReferenceEquals(x, null) && Object.ReferenceEquals(y, null))
                return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            if (x.Name != y.Name || x.Terminal != y.Terminal || x.Id != y.Id)
                return false;
            //if (x.CustomValue != null && y.CustomValue != null)
            //    return x.CustomValue.Equals(y.CustomValue);
            return x.Value == y.Value;
        }

        public static bool operator !=(Symbol x, Symbol y)
        {
            return !(x == y);
        }

        public int CompareTo(Symbol other)
        {
            return this.Name.CompareTo(other.Name);
        }


        public bool Equals(Symbol obj)
        {
            return this == obj;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            return obj == this;
        }

        public object Clone()
        {
            var sym = new Symbol(this.Id, this.Name, this.Terminal, this.Value);
            sym.GoToEmpty = this.GoToEmpty;

            foreach (var key in this.CustomValue.Keys)
            {
                sym.CustomValue.Add(key, this.CustomValue[key]);
            }

            return sym;
        }

        public Symbol Copy()
        {
            return (Symbol)Clone();
        }

        [DataMember]
        public static int newId { get; set; }

        public override string ToString()
        {
            if (Terminal)
            {
                if (Value == '\0')
                {
                    if (this.GetCustomValue<object>("RecToken") != null)
                    {
                        return this.GetCustomValue<object>("RecToken").ToString();
                    }
                    return Id.ToString();
                }

                return Name;
            }
            return Name;
        }


        public T GetCustomValue<T>(string name)
        {
            if (CustomValue.ContainsKey(name))
                return (T)CustomValue[name];
            return default(T);
        }

        public void SetCustomValue(string name, object value)
        {
            if (name == "Token")
            {

            }
            if (CustomValue.ContainsKey(name))
                CustomValue[name] = value;
            else
                CustomValue.Add(name, value);
        }
    }
}
