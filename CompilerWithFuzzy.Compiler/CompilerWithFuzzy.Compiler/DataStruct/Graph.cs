using CompilerWithFuzzy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CompilerWithFuzzy.Compiler.DataStruct
{
    [DataContract]
    [Serializable]
    public class Graph<T, U>
    {

        private List<Node<T, U>> nodes;

        [DataMember]
        public List<Node<T, U>> Nodes
        {
            get { return nodes; }

        }

        [DataMember]
        public List<Edge<T, U>> Edges
        {
            get;
            set;

        }

        [DataMember]
        public Node<T, U> Root { get; set; }
        public Graph()
        {
            Edges = new List<Edge<T, U>>();
            nodes = new List<Node<T, U>>();
        }

        public Node<T, U> AddNode(string name, T info)
        {
            var node = new Node<T, U>(name, info);
            if (Search(name) == null)
                nodes.Add(node);
            else
                throw new Exception("Nó já existente!");

            return node;

        }

        public List<Edge<T, U>> GetEdges()
        {
            List<Edge<T, U>> returns = new List<Edge<T, U>>();
            foreach (var item in Nodes)
            {
                returns.AddRange(item.Edges);
            }
            return returns;
        }
        public Node<T, U> AddNode(int id)
        {
            return AddNode(id.ToString(), default(T));
        }

        public Node<T, U> AddNode(string name)
        {
            return AddNode(name, default(T));
        }
        public Node<T, U> AddNode(int id, T value, Node<T, U> parent, double cost)
        {
            var newNode = this.AddNode(id.ToString(), value);
            newNode.Parent = parent;
            var edge = AddEdge(parent.Name, newNode.Name);
            edge.Cost = cost;
            return newNode;
        }

        private Node<T, U> Search(string name)
        {
            foreach (Node<T, U> n in nodes)
                if (n.Name == name) return n;
            return null;
        }

        public T SearchInfo(string name)
        {
            Node<T, U> n = Search(name);
            return (n == null ? default(T) : n.Info); // se sim retorna null, se não retorna a info

        }

        public Edge<T, U> LastEdge()
        {
            return Edges.Last();
        }
        public Edge<T, U> AddEdge(string source, string destiny, U info)
        {
            Node<T, U> no = Search(source);
            Node<T, U> nd = Search(destiny);

            if (no != null && nd != null)
            {
                var edge = new Edge<T, U>(info, nd);
                Edges.Add(edge);
                return no.AddEdge(edge);
            }
            else
                throw new Exception("Nó inexistente!");

        }

        public Edge<T, U> AddEdge(string source, string destiny)
        {
            return AddEdge(source, destiny, default(U));
        }
        public bool IsAdjacent(string origem, string destino)
        {
            Node<T, U> no = Search(origem);
            if (no == null) return false;
            foreach (Edge<T, U> a in no.Edges)
            {
                if (a.Destiny.Name == destino)
                    return true;
            }
            return false;
        }

        public string DFS(string root)
        {
            Node<T, U> no = Search(root);
            SetNotVisited();
            if (no == null) return "";

            string resultado = "";
            Stack<Node<T, U>> P = new Stack<Node<T, U>>();
            no.Visited = true;
            P.Push(no);

            while (P.Count > 0)
            {
                Node<T, U> n = P.Pop();
                resultado += n.Name + ";";
                foreach (Edge<T, U> a in n.Edges)
                {
                    if (!a.Destiny.Visited)//! = não(não visitado)
                    {
                        a.Destiny.Visited = true;
                        P.Push(a.Destiny);

                    }
                }
            }
            return resultado;
        }

        public string BFS(string root)
        {
            SetNotVisited();
            Node<T, U> no = Search(root);
            if (no == null) return "";

            string resultado = "";
            Queue<Node<T, U>> F = new Queue<Node<T, U>>();
            no.Visited = true;
            F.Enqueue(no);

            while (F.Count > 0)
            {
                Node<T, U> n = F.Dequeue();
                resultado += n.Name + ";";
                foreach (var a in n.Edges)
                {
                    if (!a.Destiny.Visited)
                    {
                        a.Destiny.Visited = true;
                        F.Enqueue(a.Destiny);
                    }
                }

            }
            return resultado;
        }


        //Escreva o método abaixo, que recebe um vetor de nomes de nós indicando um caminho
        //verifica se é possivel executar o caminho no grafo
        //Para testa-la no form use:

        public bool VerifyPath(string[] path)
        {

            for (int i = 0; i < path.Length - 1; i++)
            {
                Node<T, U> n = Search(path[i]);
                if (n == null) return false;

                if (IsAdjacent(path[i], path[i + 1]) == false)
                    return false;
            }
            return true;
        }


        public void SetNotVisited()
        {
            foreach (Node<T, U> n in nodes)
                n.Visited = false;

        }

        //11. Crie um método que recebe um nó do grafo e um número N, o nível. O método deve retornar
        //todos os nós com distância N do nó inicial informado. Observação: cada arco percorrido corresponde a uma unidade de distância.

        public string RetornaNoNivelN(string escolha, int num)
        {
            SetNotVisited();
            Node<T, U> no = Search(escolha);

            if (no == null) return "";
            string resultado = "";
            Queue<Node<T, U>> F = new Queue<Node<T, U>>();
            no.Visited = true;
            no.Nivel = 0;
            F.Enqueue(no);

            while (F.Count > 0)
            {
                Node<T, U> n = F.Dequeue();

                foreach (var a in n.Edges)
                {
                    if (!a.Destiny.Visited)
                    {
                        a.Destiny.Visited = true;
                        a.Destiny.Nivel = n.Nivel + 1;
                        F.Enqueue(a.Destiny);

                    }
                }
                if (Convert.ToInt32(n.Info) == num)
                {
                    resultado += n.Name + ";";
                }

            }
            return resultado;
        }

        //12. Crie um método capaz de retornar a conectividade entre dois nós, ou seja,
        //dados dois nós este método verifica se eles estão conectados entre si.

        public bool Connected(string source, string destiny)
        {
            string passeioOrigem = BFS(source);
            string passeioDestino = BFS(destiny);
            if (passeioOrigem.IndexOf(destiny + ";") == -1 || passeioDestino.IndexOf(source + ";") == -1)
                return false;
            return true;

        }

        public bool IsDirected()
        {
            foreach (Node<T, U> n in nodes)
                foreach (var a in n.Edges)
                    if (IsAdjacent(a.Destiny.Name, n.Name) == false)
                        return true;
            return false;
        }

        public string Degree(string name)
        {
            Node<T, U> n = Search(name);
            int[] result = Incidence(n);
            return "O no " + name + " possui grau " + (result[0] + result[1]) + ": \r\n   Grau de Entrada: " + result[0] + "\r\n   Grau de Saida: " + result[1];
        }

        public int[] Incidence(Node<T, U> n)
        {
            int[] result = new int[2];

            foreach (var a in n.Edges)
                result[0]++;

            foreach (var no in nodes)
                foreach (var a in no.Edges)
                    if (a.Destiny.Name == n.Name)
                        result[1]++;

            return result;
        }
        public bool HasCycle(string root)
        {
            Node<T, U> no = Search(root);
            foreach (var n in nodes)
            {
                if (Connected(root, n.Name) == true)
                {
                    if (IsAdjacent(root, n.Name) == true)
                        return true;
                }
            }
            return false;

        }

        public string Adjacent(string origem, string destino)
        {
            if (!IsAdjacent(origem, destino))
                return "O no " + origem + " é nao adjacente ao nó " + destino;
            else
                return "O no " + origem + " é adjacente ao nó " + destino;
        }

        public List<Node<T, U>>[] AdjacencyList()
        {
            int count = 0;
            List<Node<T, U>>[] Adjacencia = new List<Node<T, U>>[nodes.Count];
            foreach (var n in nodes)
            {
                List<Node<T, U>> lista = new List<Node<T, U>>();
                lista.Add(n);
                foreach (var a in n.Edges)
                    lista.Add(a.Destiny);
                Adjacencia[count] = lista;
                count++;
            }

            return Adjacencia;
        }

        public string AdjacencyMatrixString()
        {
            int[,] matriz = new int[nodes.Count + 1, nodes.Count + 1];
            string result = "\t";

            for (int i = 1; i < nodes.Count + 1; i++)
                result += i.ToString() + "\t";
            result += "\r\n";

            for (int i = 1; i <= nodes.Count; i++)
            {
                result += i.ToString() + "\t";
                for (int j = 1; j <= nodes.Count; j++)
                {
                    if (IsAdjacent(i.ToString(), j.ToString()))
                    {
                        result += "1\t";
                        matriz[i - 1, j - 1] = 1;
                    }
                    else
                    {
                        result += "0 \t";
                        matriz[i - 1, j - 1] = 0;
                    }
                }
                result += "\r\n";
            }
            return result;
        }

        public int[,] AdjacencyMatrix()
        {
            int[,] matriz = new int[nodes.Count + 1, nodes.Count + 1];

            for (int i = 1; i <= nodes.Count; i++)
                for (int j = 1; j <= nodes.Count; j++)
                    if (IsAdjacent(i.ToString(), j.ToString()))
                        matriz[i - 1, j - 1] = 1;
                    else
                        matriz[i - 1, j - 1] = 0;
            return matriz;
        }

        public string Path(string source, string destiny)
        {
            string resultado = "";
            resultado += DFS(source) + ";";
            if (resultado.IndexOf(destiny) != -1)
                return resultado;
            return null;

        }

        public List<Node<T, U>> DijkstraInverse(string source, string destiny)
        {
            //Inicializando  grafo solucao
            Graph<T, U> solution = new Graph<T, U>();
            var node = solution.AddNode(source, default(T));
            node.SetCustomOject("Cost", 0);
            //enquanto nao achou nó alvo na solucao
            while (solution.Search(destiny) == null)
            {
                double min = -1;
                Edge<T, U> arcoMin = null;
                Node<T, U> paiMin = null;
                foreach (Node<T, U> N in solution.nodes)
                {
                    Node<T, U> N1 = this.Search(N.Name);
                    var arcos = N1.Edges;
                    if (arcos != null)
                    {
                        foreach (var A in arcos)
                        {
                            if (solution.Search(A.Destiny.Name) != null)
                                continue;

                            double custo = N.GetCustomOject<double>("Cost") + A.Cost;

                            if (min == -1 || custo < min)
                            {
                                min = custo;
                                arcoMin = A;
                                paiMin = N;
                            }
                        }
                    }
                }
                //Aqui teremos o arco de menor custo para adicionar no grafo
                var newNode = solution.AddNode(arcoMin.Destiny.Name, arcoMin.Destiny.Info);
                newNode.SetCustomOject("Cost", min);
                var edge = solution.AddEdge(arcoMin.Destiny.Name, paiMin.Name, arcoMin.Info);
                edge.Cost = arcoMin.Cost;
            }
            return null;
            //solution.PasseioLargura(Destino);
        }

        public void RemoveNode(Node<T, U> prox)
        {
            Nodes.ForEach(n => n.Edges.RemoveAll(e => e.Destiny == prox));
            Nodes.RemoveAll(n => n == prox);
            this.Edges.RemoveAll(e => e.Destiny == prox);
        }

        public List<GraphPath<T, U>> AllPaths(NormAbstract norm)
        {
            var paths = new List<GraphPath<T, U>>();
            AllPaths(paths, this.Root, null, 1, norm);

            return paths;
        }

        public void AllPaths(List<GraphPath<T, U>> returns, Node<T, U> node, GraphPath<T, U> path, double cost, NormAbstract norm)
        {
            if (path == null)
            {
                path = new GraphPath<T, U>(norm);
            }
            path.AddNode(node, cost);

            if (node.Edges.Count > 0)
            {
                var original = path.Copy();
                for (int i = 0; i < node.Edges.Count; i++)
                {
                    if (i == 0)
                    {
                        AllPaths(returns, node.Edges[i].Destiny, path, node.Edges[i].Cost, norm);
                    }
                    else
                    {
                        AllPaths(returns, node.Edges[i].Destiny, original.Copy(), node.Edges[i].Cost, norm);
                    }
                }
            }
            else
            {
                if (!returns.Contains(path))
                    returns.Add(path);
            }
        }

    }
}

