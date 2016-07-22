using CompilerWithFuzzy.AutomaFuzzy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CompilerWithFuzzy.Compiler.Lex
{
    [DataContract]
    [Serializable]
    public class Token
    {
        [DataMember]
        public RecognitionToken RecToken;

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Collumn { get; set; }

        [DataMember]
        public int Line { get; set; }

        [DataMember]
        public string Word { get; set; }


        public Token()
        {
        }
        public Token(int id, int line, int collumn, RecognitionToken recToken, string word)
        {
            this.Id = id;
            this.Line = line;
            this.Collumn = collumn;
            this.RecToken = recToken;
            this.Word = word;
        }
        public Token(int id, int line, int collumn, RecognitionToken recToken, char word)
            : this(id, line, collumn, recToken, "" + word)
        {

        }

        public override string ToString()
        {
            return Word;
        }
    }
}
