using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CompilerWithFuzzy.GrammarFuzzy.Automa
{
    /// <summary>
    /// Classe que representa um autômato.
    /// </summary>
    [DataContract]
    [Serializable]
    public class StackAutoma
    {

        #region Atributos

        /// <summary>
        /// 
        /// O estado inicial do autômato.
        /// </summary>
        [DataMember]
        public State startState
        {
            get; set;
        }
        [DataMember]
        public Grammar gramma
        {
            get; set;
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Alfabeto do autômato.
        /// </summary>
        [DataMember]
        public SymbolList Alphabet { get; private set; }

        /// <summary>
        /// Alfabeto do autômato.
        /// </summary>
        [DataMember]
        public SymbolList StackAlphabet { get; private set; }
        /// <summary>
        /// O estado inicial do autômato.
        /// </summary>
        [DataMember]
        public State StartState
        {
            get { return startState; }
        }
        /// <summary>
        /// Lista de estados.
        /// </summary>
        [DataMember]
        public Dictionary<string, State> States { get; private set; }


        #endregion

        #region Construtores
        public StackAutoma()
        { }

        /// <summary>
        /// Cria nova instância do automato.
        /// </summary>
        /// <param name="estadoInicial">O nome do estado inicial.</param>
        /// <param name="alfabeto">O alfabeto do autômato.</param>
        /// <param name="final">Indica se estado inicial é final.</param>
        public StackAutoma(string estadoInicial, bool final)
        {
            this.States = new Dictionary<string, State>();
            this.Alphabet = new SymbolList();
            // Cria estado e adiciona..
            this.startState = new State(estadoInicial, final);
            this.States.Add(estadoInicial, this.startState);

            StackAlphabet = new SymbolList();

        }

   
        public StackAutoma(Grammar grammar)
            : this("q0", grammar.VariablesEmpty.Contains(grammar.VariableStart))
        {
            this.gramma = grammar;
            AdicionarEstado("q1", false);
            AdicionarEstado("q2", true);
            AdicionarTransicao("q0", "q1", Symbol.EmptySymbol, Symbol.EmptySymbol, grammar.VariableStart);

            AdicionarTransicao("q1", "q2", Symbol.StackFinal, Symbol.TapeFinal, Symbol.EmptySymbol);

            SymbolList visitados = new SymbolList();
            Queue<Symbol> variaveisAVisitar = new Queue<Symbol>();
            variaveisAVisitar.Enqueue(grammar.VariableStart);
            visitados.Add(grammar.VariableStart);
            while (variaveisAVisitar.Count > 0)
            {
                Symbol variavelAtual = variaveisAVisitar.Dequeue();

                List<RuleProduction> regrasAtuais = grammar.GetRules(variavelAtual.Name);

                foreach (var item in regrasAtuais)
                {
                    AdicionarTransicao("q1", "q1", item.Source, item.FirstDestiny(), item.SkipFirstDestiny().ToArray());
                    foreach (var itemDestino in item.Destiny)
                    {
                        if (!itemDestino.Terminal && !visitados.Contains(itemDestino))
                        {
                            variaveisAVisitar.Enqueue(itemDestino);
                            visitados.Add(itemDestino);
                        }
                    }
                }
            }

        }

        #endregion

        #region Métodos

        /// <summary>
        /// Adiciona novo estado.
        /// </summary>
        /// <param name="nomeEstado">O nome do estado.</param>
        /// <param name="final">Indica se o estado é final.</param>
     
        public void AdicionarEstado(string nomeEstado, bool final)
        {
            if (!States.ContainsKey(nomeEstado))
            {
                States.Add(nomeEstado, new State(nomeEstado, final));
            }
        }

        /// <summary>
        /// Definir se o estado é final ou não.
        /// </summary>
        /// <param name="nomeEstado">O nome do estado.</param>
        /// <param name="final">Indicação se é final ou inicial.</param>
    
        public void DefinirEstadoFinal(string nomeEstado, bool final)
        {
            if (States.ContainsKey(nomeEstado))
            {
                States[nomeEstado].Final = final;
            }
        }

        /// <summary>
        /// Adiciona transição entre 2 estados.
        /// </summary>
        /// <param name="estadoOrigem">O estado origem do autômato.</param>
        /// <param name="consumirFita">O símbolo a ser avaliado.</param>
        /// <param name="estadoDestino">O estado destino.</param>
    
        public void AdicionarTransicao(string estadoOrigem, string estadoDestino, Symbol consumirPilha,
                Symbol consumirFita, params Symbol[] gravarPilha)
        {
            if (consumirPilha == null)
            {
                consumirPilha = Symbol.EmptySymbol;
            }
            SymbolList vars = new SymbolList();
            if (gravarPilha == null || gravarPilha.Length == 0)
            {
                vars.Add(Symbol.EmptySymbol);
            }
            else
            {
                vars.AddRange(gravarPilha);
            }


            foreach (var symbol in vars)
            {
                if (StackAlphabet.IndexOf(symbol) < 0 && symbol != Symbol.EmptySymbol && symbol != Symbol.StackFinal && symbol != Symbol.TapeFinal)
                    StackAlphabet.Add(symbol);
            }

            // Adiciona transições..
            if (States.ContainsKey(estadoOrigem) && States.ContainsKey(estadoDestino))
            {
                if (!Alphabet.Contains(consumirFita))
                    Alphabet.Add(consumirFita);

                States[estadoOrigem].AdicionarTransicao(States[estadoDestino], consumirFita, consumirPilha, vars);
            }
        }

        /// <summary>
        /// Busca por uma transição válida.
        /// </summary>
        /// <param name="estado">O estado origem.</param>
        /// <param name="simbolo">O símbolo a ser consumido.</param>
        /// <returns>A transição encontrada. Caso não encontre retorna null.</returns>
        private Transition BuscaTransicao(State estado, Symbol simbolo)
        {
            return estado.BuscaTransicao(simbolo);
        }

        /// <summary>
        /// Operação edge: retorna todos estados alcançáveis a partir de "estado" consumindo "simbolo".
        /// </summary>
        /// <param name="nomeEstado"></param>
        /// <param name="simbolo"></param>
        /// <returns></returns>
    
        public State[] Edge(string nomeEstado, Symbol simbolo)
        {
            List<State> destinos = new List<State>();
            State estado = States[nomeEstado];
            Transition[] transicoes = estado.BuscaTransicoes(simbolo);
            if (transicoes != null)
            {
                // Percorrendo as transições e adicionando a lista destino..
                foreach (Transition t in transicoes)
                {
                    if (!destinos.Contains(t.Destiny))
                    {
                        destinos.Add(t.Destiny);
                    }
                }
            }
            return destinos.ToArray();
        }

        /// <summary>
        /// Operação DFAEdge: closure(Edge(estado, simbolo))
        /// </summary>
        /// <param name="nomeEstados"></param>
        /// <param name="simbolo"></param>
        /// <returns></returns>
     
        public State[] DFAEdge(string[] nomeEstados, Symbol simbolo)
        {
            if (nomeEstados == null || nomeEstados.Length == 0)
            {
                return null;
            }
            List<State> dfaEdge = new List<State>();
            State[] de = DFAEdge(nomeEstados[0], simbolo);
            // Adiciona elementos do primeiro estado..
            if (de != null)
            {
                dfaEdge.AddRange(de);
            }
            // Adiciona o DFAEdge dos demais estados..
            for (int i = 1; i < nomeEstados.Length; i++)
            {
                de = DFAEdge(nomeEstados[i], simbolo);
                if (de != null)
                {
                    foreach (State d in de)
                    {
                        if (!dfaEdge.Contains(d))
                        {
                            dfaEdge.Add(d);
                        }
                    }
                }
            }
            return dfaEdge.OrderBy(e => e.Name).ToArray();
        }

        /// <summary>
        /// Operação DFAEdge: closure(Edge(estado, simbolo))
        /// </summary>
        /// <param name="nomeEstado"></param>
        /// <param name="simbolo"></param>
        /// <returns></returns>
     
        public State[] DFAEdge(string nomeEstado, Symbol simbolo)
        {
            if (!States.ContainsKey(nomeEstado))
            {
                return null;
            }
            // Cria lista de resultado..
            List<State> estados = new List<State>();
            // Lista de estados atingidos..
            State[] edge = Edge(nomeEstado, simbolo);
            foreach (State e in edge)
            {
                State[] closure = Closure(e.Name);
                if (closure != null && closure.Length > 0)
                {
                    foreach (State c in closure)
                    {
                        if (!estados.Contains(c))
                        {
                            estados.Add(c);
                        }
                    }
                }
            }
            return estados.OrderBy(e => e.Name).ToArray();
        }

        /// <summary>
        /// Operação Closure: todos estados alcançados a partir de um estado sem consumir simbolos.
        /// </summary>
        /// <param name="nomeEstado"></param>
        /// <returns></returns>
   
        public State[] Closure(string nomeEstado)
        {
            if (!States.ContainsKey(nomeEstado))
            {
                return null;
            }
            // Obtem estado..
            State estado = States[nomeEstado];
            // Adiciona a lista de closure..
            List<State> closure = new List<State>();
            closure.Add(estado);
            // Percorre a lista..
            for (int i = 0; i < closure.Count; i++)
            {
                State[] edge = Edge(closure[i].Name, Symbol.EmptySymbol);
                foreach (State e in edge)
                {
                    if (!closure.Contains(e))
                    {
                        closure.Add(e);
                    }
                }
            }
            return closure.OrderBy(e => e.Name).ToArray();
        }

        /// <summary>
        /// Obtem o nome dos estados.
        /// </summary>
        /// <returns></returns>
   
        public string[] ObterEstados()
        {
            var nomeEstados = from e in States
                              orderby e.Key
                              select e.Key;
            return nomeEstados.ToArray();
        }

        /// <summary>
        /// Valida o texto através da estrutura do autômato.
        /// </summary>
        /// <param name="texto">O texto a ser validado.</param>
        /// <returns>Um valor indicando se o texto é válido ou não.</returns>
      
        public bool Validar(string texto)
        {
            //State estado = startState;
            //int posicao = 0;
            //int tamanho = texto.Length;
            //// Percorre a cadeia de caracteres..
            //while (posicao < tamanho)
            //{
            //    Transition transicao = BuscaTransicao(estado, texto[posicao]);
            //    if (transicao != null)
            //    {
            //        estado = transicao.Destiny;
            //        posicao++;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //return estado.Final;
            return false;
        }

 
        public void RemoveEstadosInacessiveis()
        {
            Dictionary<string, bool> visitado = new Dictionary<string, bool>();
            Queue<State> fila = new Queue<State>();

            foreach (var item in States.Keys)
            {
                visitado.Add(item, false);
            }

            fila.Enqueue(startState);
            while (fila.Count > 0)
            {
                State est = fila.Dequeue();
                visitado[est.Name] = true;
                foreach (var item in est.Transitions)
                {
                    if (!visitado[item.Destiny.Name])
                    {
                        fila.Enqueue(item.Destiny);
                    }
                }
            }

            foreach (var item in visitado)
            {
                if (!item.Value)
                {
                    RemoverEstado(item.Key);
                }
            }
        }

        
        public void RemoverEstado(string nome)
        {
            States[nome].Transitions = new List<Transition>();
            States.Remove(nome);
            var nomes = States.Keys;
            for (int j = nomes.Count - 1; j > -1; j--)
            {
                string n2 = nomes.ElementAt(j);

                States[n2].Transitions.RemoveAll(t => t.Destiny.Name == nome);
            }
        }

        #endregion


       
        public string[] ObterEstadosFinais()
        {
            var nomeEstados = from e in States
                              orderby e.Key
                              where e.Value.Final
                              select e.Key;
            return nomeEstados.ToArray();
        }
    }
}
