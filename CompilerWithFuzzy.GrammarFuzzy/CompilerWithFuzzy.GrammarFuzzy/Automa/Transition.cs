using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CompilerWithFuzzy.GrammarFuzzy.Automa
{
    /// <summary>
    /// Classe que representa uma transição entre dois estados.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Transition
    {

        #region Atributos

        /// <summary>
        /// O estado origem da transição.
        /// </summary>
        [DataMember]
        public State Source { get; set; }
        /// <summary>
        /// O estado destino da transição.
        /// </summary>
        [DataMember]
        public State Destiny { get; set; }
        /// <summary>
        /// Os símbolos que podem ser consumidos por esta transição.
        /// </summary>
        [DataMember]
        public Symbol Symbol { get; set; }

        [DataMember]
        public SymbolList ConsumingStack { get; set; }

        [DataMember]
        public SymbolList PushStack { get; set; }


        #endregion

        #region Construtores

        public Transition()
        {
        }
        /// <summary>
        /// Cria nova transição.
        /// </summary>
        /// <param name="source">Estado origem da transição.</param>
        /// <param name="symbol">Símbolos que podem ser consumidos nesta transição.</param>
        /// <param name="destiny">Estado destino da transição.</param>

        public Transition(State source, Symbol symbol, State destiny, SymbolList consumirPilha, SymbolList gravarPilha)
        {

            Source = source;
            Symbol = symbol;
            Destiny = destiny;
            ConsumingStack = new SymbolList();
            PushStack = new SymbolList();
            ConsumingStack.AddRange(consumirPilha);
            PushStack.AddRange(gravarPilha);
        }

        public Transition(State source, Symbol symbol, State destiny, Symbol consumirPilha, SymbolList gravarPilha)
        {

            Source = source;
            Symbol = symbol;
            Destiny = destiny;
            ConsumingStack = new SymbolList();
            PushStack = new SymbolList();
            ConsumingStack.Add(consumirPilha);
            PushStack.AddRange(gravarPilha);
        }

        #endregion

    }
}
