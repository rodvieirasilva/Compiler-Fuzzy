using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace CompilerWithFuzzy.Compiler.DataStruct
{
    [DataContract]
    [Serializable]
    public class Node<T, U>
    {
        private T info;
        private string name;
        private List<Edge<T, U>> edges;
        private bool visited;
        private Node<T, U> parent;

        [DataMember]
        public static int Ids
        {
            get; set;
        }

        [DataMember]
        public Node<T, U> Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        [DataMember]
        public bool Visited
        {
            get { return visited; }
            set { visited = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public T Info
        {
            get { return info; }
            set { info = value; }
        }

        [DataMember]
        public List<Edge<T, U>> Edges
        {
            get { return edges; }
            set
            {
                edges = value;
            }
        }

        [DataMember]
        public Dictionary<string, object> CustomOject { get; set; }

        [DataMember]
        public int Nivel { get; set; }

        [DataMember]
        public int Id { get; set; }


        public Node(string name, T info)
        {
            Ids++;
            CustomOject = new Dictionary<string, object>();

            int id;

            if (string.IsNullOrWhiteSpace(name))
            {
                id = Ids;
                name = Ids.ToString();
            }

            this.name = name;
            this.info = info;
            edges = new List<Edge<T, U>>();

            if (int.TryParse(name, out id))
            {
                this.Id = id;
            }

        }

  
        public Edge<T, U> AddEdge(Edge<T, U> a)
        {
            edges.Add(a);
            return a;
        }

    
        public Edge<T, U> AddEdge(U info, Node<T, U> destiny, double cost = 0.0)
        {
            Edge<T, U> a = new Edge<T, U>(info, destiny, cost);
            edges.Add(a);
            return a;
        }


        public V GetCustomOject<V>(string name)
        {
            if (CustomOject.ContainsKey(name))
                return (V)CustomOject[name];

            return default(V);
        }

      
        public object SetCustomOject(string name, object value)
        {
            if (CustomOject.ContainsKey(name))
            {
                CustomOject[name] = value;
            }
            else
            {
                CustomOject.Add(name, value);
            }

            return value;
        }

        public override string ToString()
        {
            return this.Name;
        }


        public Graph<T, U> ToGraph()
        {
            Graph<T, U> graph = new Graph<T, U>();
            graph.Root = this;
            Queue<Node<T, U>> queue = new Queue<Node<T, U>>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var actual = queue.Dequeue();
                //double pertinence = actual.GetCustomOject<double>("Pertinence");
                if (actual != null)
                {
                    graph.Nodes.Add(actual);
                    for (int i = 0; i < actual.Edges.Count; i++)
                    {
                        if (!graph.Edges.Any(e => e.Destiny == actual.Edges[i].Destiny))
                        {
                            //actual.Edges[i].Cost = pertinence;
                            graph.Edges.Add(actual.Edges[i]);
                        }

                        if (!graph.Nodes.Contains(actual.Edges[i].Destiny))
                        {
                            queue.Enqueue(actual.Edges[i].Destiny);
                        }
                    }
                }

            }

            return graph;
        }


    }
}
