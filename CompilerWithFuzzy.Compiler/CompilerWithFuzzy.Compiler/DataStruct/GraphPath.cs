using CompilerWithFuzzy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CompilerWithFuzzy.Compiler.DataStruct
{
    [DataContract]
    [Serializable]
    public class GraphPath<T, U> : IEquatable<GraphPath<T, U>>
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public List<Node<T, U>> Nodes { get; set; }
        [DataMember]
        public NormAbstract Norm { get; set; }
        [DataMember]
        public double Cost { get; set; }

        [DataMember]
        public double MinEquality { get; set; }

        public GraphPath()
        {
        }
        public GraphPath(NormAbstract norm)
        {
            this.MinEquality = 0.001;
            this.Norm = norm;
            Nodes = new List<Node<T, U>>();
            Cost = 1;
            Id = id++;
        }

        public void AddNode(Node<T, U> node, double cost)
        {
            Nodes.Add(node);
            if (cost == 1)
            {

            }
            Cost = Norm.Calculate(Cost, cost);
        }

        public GraphPath<T, U> Copy()
        {
            GraphPath<T, U> returns = new GraphPath<T, U>(this.Norm);

            returns.Cost = this.Cost;
            returns.Nodes.AddRange(this.Nodes);
            return returns;
        }

        public override string ToString()
        {
            return string.Format("{0}({1:0.000}) - {2}", Id, Cost, string.Join(",", Nodes.Select(n => n.Id)));
        }


        public static int id = 1;

        public bool Equals(GraphPath<T, U> other)
        {
            return Math.Abs(other.Cost - this.Cost) <= 0.001;
        }
    }
}
