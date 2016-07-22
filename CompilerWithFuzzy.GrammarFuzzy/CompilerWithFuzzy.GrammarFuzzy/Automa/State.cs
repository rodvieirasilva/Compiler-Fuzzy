using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace CompilerWithFuzzy.GrammarFuzzy.Automa
{

    /// <summary>
    /// Classe que representa um estado do Autômato.
    /// </summary>
    [DataContract]
    [Serializable]
    public class State
    {

        #region Atributos

        /// <summary>
        /// Indica se o estado é final ou não.
        /// </summary>
        [DataMember]
        public bool Final { get; set; }
        /// <summary>
        /// O nome do estado.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// A lista de transiçõoes que partem deste estado.
        /// </summary>
        [DataMember]
        public List<Transition> Transitions { get; set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Cria nova instância da classe estado.
        /// </summary>

        public State()
            : this("q0", false)
        {
        }

        /// <summary>
        /// Cria nova estado.
        /// </summary>
        /// <param name="nome">O nome do estado.</param>

        public State(string nome, bool final)
        {
            Transitions = new List<Transition>();
            Name = nome;
            Final = final;
        }

        #endregion

        #region Métodos


        public void AdicionarTransicao(State destino, Symbol symbol, Symbol consumirPilha, SymbolList gravarPilha)
        {
            Transitions.Add(new Transition(this, symbol, destino, consumirPilha, gravarPilha));
        }

        /// <summary>
        /// Adiciona transição para o estado destino com consumo de símbolo.
        /// </summary>
        /// <param name="destino">O estado destino da transição.</param>
        /// <param name="simbolos">Símbolo a ser consumido.</param>

        public void AdicionarTransicao(State destino, Symbol symbol, SymbolList consumirPilha, SymbolList gravarPilha)
        {
            Transitions.Add(new Transition(this, symbol, destino, consumirPilha, gravarPilha));
        }

        /// <summary>
        /// Busca por uma transição válida.
        /// </summary>
        /// <param name="estado">O estado origem.</param>
        /// <param name="simbolo">O símbolo a ser consumido.</param>
        /// <returns>A transição encontrada. Caso não encontre retorna null.</returns>

        public Transition BuscaTransicao(Symbol simbolo)
        {
            // Retorna todas transições que consomem o símbolo..
            var transicao = from t in Transitions
                            where t.Symbol == simbolo
                            select t;
            // Busca a transição e retorna se encontrar..
            if (transicao != null && transicao.Count() > 0)
            {
                return transicao.First();
            }
            return null;
        }

        /// <summary>
        /// Encontra todas as transições para um símbolo (AFN / AFE).
        /// </summary>
        /// <param name="simbolo"></param>
        /// <returns></returns>

        public Transition[] BuscaTransicoes(Symbol simbolo)
        {
            // Retorna todas transições que consomem o símbolo..
            var transicao = from t in Transitions
                            where t.Symbol == simbolo
                            select t;
            // Busca a transição e retorna se encontrar..
            if (transicao != null && transicao.Count() > 0)
            {
                return transicao.ToArray();
            }
            return null;

        }

        #endregion

    }
}
