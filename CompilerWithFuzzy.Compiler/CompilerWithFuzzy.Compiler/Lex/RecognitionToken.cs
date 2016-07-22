using CompilerWithFuzzy.AutomaFuzzy;
using CompilerWithFuzzy.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CompilerWithFuzzy.Compiler.Lex
{
    [DataContract]
    [Serializable]
    public class RecognitionToken : IEquatable<RecognitionToken>, IComparable<RecognitionToken>
    {
        [DataMember]
        public RecognitionFuzzy RegexFuzzy { get; set; }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool Unitary { get; set; }

        [DataMember]
        public double LastMatchValue { get; set; }

        [DataMember]
        public System.Drawing.Color Color { get; set; }

        [DataMember]
        public NormAbstract Norm
        {
            get; set;
        }

        [DataMember]
        public ConormAbstract Conorm
        {
            get; set;
        }

        
        public RecognitionToken()
        {
        }
        
        public RecognitionToken(int id, string name, string fregex, string hexColor, NormAbstract norm, ConormAbstract conorm)
        {
            this.Norm = norm;
            this.Conorm = conorm;
            Id = id;
            Name = name;
            RegexFuzzy = new RecognitionFuzzy(fregex, norm, conorm, 0.8);
            Unitary = (!String.IsNullOrEmpty(fregex)) && (fregex.Length == 1);
            if (string.IsNullOrWhiteSpace(hexColor))
            {
                System.Array colorsArray = Enum.GetValues(typeof(KnownColor));
                this.Color = Color.FromKnownColor((KnownColor)colorsArray.GetValue((id * 10) % colorsArray.Length));
            }
            else
            {
                this.Color = System.Drawing.ColorTranslator.FromHtml(hexColor);
            }
        }

      
        public bool Equals(RecognitionToken other)
        {
            return this.Id == other.Id;
        }

        public int CompareTo(RecognitionToken other)
        {
            return this.LastMatchValue.CompareTo(other.LastMatchValue);
        }

     
        public override string ToString()
        {
            return Name;
        }
    }
}
