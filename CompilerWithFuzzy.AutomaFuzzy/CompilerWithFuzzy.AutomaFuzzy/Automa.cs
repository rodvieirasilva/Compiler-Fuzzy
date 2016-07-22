using CompilerWithFuzzy.AutomaFuzzy.Rules;
using CompilerWithFuzzy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace CompilerWithFuzzy.AutomaFuzzy
{
    [DataContract]
    [Serializable]
    /// <summary>
    /// Classe responsável por representar um Automato Fuzzy
    /// </summary>
    /// <typeparam name="T">Tipo a ser armazenado no estado do automato</typeparam>

    public class Automa<T>
    {
        /// <summary>
        /// Sílbolo que representa a transição vazia
        /// </summary>
        [DataMember]
        public T SymbolEmpty { get; set; }

        [DataMember]
        public List<State<T>> InitialStates { get; set; }

        [DataMember]
        public List<State<T>> FinalStates { get; set; }

        [DataMember]
        public List<T> Alphabet { get; set; }

        [DataMember]
        public List<State<T>> States { get; set; }

        [DataMember]
        public List<Transition<T>> Transitions { get; set; }

        public Automa()
        { }
        public Automa(List<T> alphabet, T symbolEmpty)
        {
            SymbolEmpty = symbolEmpty;
            Alphabet = alphabet.ToList();
            States = new List<State<T>>();
            FinalStates = new List<State<T>>();
            InitialStates = new List<State<T>>();
            Transitions = new List<Transition<T>>();
        }
        public State<T> AddState(string name, double pertinenceInitial, double pertinenceFinal)
        {
            State<T> state = new State<T>(States.Count, name, pertinenceInitial, pertinenceFinal);
            States.Add(state);

            if (pertinenceInitial > 0)
                FinalStates.Add(state);

            if (pertinenceFinal > 0)
                FinalStates.Add(state);

            return state;
        }
        public void AddTransition(string from, string to, AbstractRule<T> rule)
        {
            State<T> stateFrom = States.FirstOrDefault(s => s.Name == from);
            if (stateFrom != null)
            {
                State<T> stateTo = States.FirstOrDefault(s => s.Name == to);
                if (stateTo != null)
                {
                    var tran = new Transition<T>(stateFrom, stateTo, rule);
                    Transitions.Add(tran);
                    stateFrom.Transitions.Add(tran);
                }
            }
        }
        
        public double SearchPertinence(State<T> from, State<T> to, T c, NormAbstract norm, ConormAbstract conorm)
        {
            double pertinenceMax = 0.0;
            Queue<Tuple<State<T>, double>> queue = new Queue<Tuple<State<T>, double>>();

            queue.Enqueue(Tuple.Create<State<T>, double>(from, 1));
            List<State<T>> visits = new List<State<T>>();

            while (queue.Count > 0)
            {
                Tuple<State<T>, double> tupleActual = queue.Dequeue();
                State<T> actual = tupleActual.Item1;
                visits.Add(actual);
                var trans = actual.Transitions.Where(t => t.To == to).ToList();
                if (trans.Count() > 0)
                {
                    pertinenceMax = conorm.Calculate(
                            conorm.Calculate(trans,
                                t => norm.Calculate(tupleActual.Item2, t.Rule.Math(c))),
                            pertinenceMax);
                }

                for (int i = 0; i < actual.Transitions.Count; i++)
                {
                    if (!visits.Contains(actual.Transitions[i].To) && actual.Transitions[i].Rule.Math(SymbolEmpty) > 0)
                    {
                        queue.Enqueue(Tuple.Create<State<T>, double>(actual.Transitions[i].To, norm.Calculate(actual.Transitions[i].Rule.Pertinence, tupleActual.Item2)));
                    }
                }
            }
            return pertinenceMax;
        }
    }
}

