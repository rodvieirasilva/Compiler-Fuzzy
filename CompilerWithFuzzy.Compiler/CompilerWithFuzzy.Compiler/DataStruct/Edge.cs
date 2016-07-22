using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CompilerWithFuzzy.Compiler.DataStruct
{
    [DataContract]
    [Serializable]
    public class Edge<T, U>
    {
        private Node<T, U> destiny;
        private U info;


        [DataMember]
        public U Info
        {
            get { return info; }
            set { info = value; }
        }

        [DataMember]
        public double Cost { get; set; }

        [DataMember]
        public Node<T, U> Destiny
        {
            get { return destiny; }
            set { destiny = value; }
        }
        
        public Edge(U info, Node<T, U> destiny, double cost = 0.0)
        {
            this.info = info;
            this.destiny = destiny;
            this.Cost = cost;
        }

    }
}
