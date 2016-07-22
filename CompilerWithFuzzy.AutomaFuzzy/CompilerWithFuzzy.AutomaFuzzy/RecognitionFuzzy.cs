using CompilerWithFuzzy.AutomaFuzzy.Rules;
using CompilerWithFuzzy.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace CompilerWithFuzzy.AutomaFuzzy
{

    [DataContract]
    [Serializable]
    public class RecognitionFuzzy
    {
        [DataMember]
        public static readonly char SYMBOLEMPTY = '\0';

        [DataMember]
        public string RegexFuzzy { get; set; }

        [DataMember]
        public Automa<char> Automa { get; set; }

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

        [DataMember]
        public double ValueDefaultTransitions
        {
            get; set;
        }

        public RecognitionFuzzy()
        { }
        public RecognitionFuzzy(string regexFuzzy) : this(regexFuzzy, new MultiplyNorm(), new MaxConorm(), 0.8)
        {

        }
        
        public RecognitionFuzzy(string regexFuzzy, NormAbstract norm, ConormAbstract conorm, double valueDefaultTransitions)
        {
            ValueDefaultTransitions = valueDefaultTransitions;
            this.Norm = norm;
            this.Conorm = conorm;
            this.RegexFuzzy = regexFuzzy;
            RegexToAutoma();
        }
        
        public virtual Automa<char> RegexToAutoma()
        {
            Automa = new Automa<char>(GetAlphabet().ToList(), SYMBOLEMPTY);
            var stateInitial = Automa.AddState(NextNameState(), 1, 0);

            if (string.IsNullOrEmpty(RegexFuzzy))
                return Automa;

            string temp = string.Empty;
            bool escape = false;

            for (int i = 0; i < RegexFuzzy.Length; i++)
            {

                if (RegexFuzzy[i] == '\\')
                {
                    //NOTHING
                }
                else if (escape)
                {
                    switch (RegexFuzzy[i])
                    {
                        case 't':
                            temp += '\t';
                            break;
                        case 'n':
                            temp += '\n';
                            break;
                        case 'r':
                            temp += '\r';
                            break;
                        default:
                            temp += RegexFuzzy[i];
                            break;
                    }

                }
                else
                {
                    switch (RegexFuzzy[i])
                    {
                        case '?':
                            temp += "{0,1}";
                            break;
                        case '+':
                            temp += "{1,}";
                            break;
                        case '*':
                            temp += "{0,}";
                            break;
                        default:
                            temp += RegexFuzzy[i];
                            break;
                    }
                }

                if (RegexFuzzy[i] == '\\')
                {
                    escape = true;
                }
                else
                {
                    escape = false;
                }
            }

            RegexFuzzy = temp;//;RegexFuzzy.Replace("?", "{0,1}").Replace("+", "{1,}").Replace("*", "{0,}");

            List<State<char>> lastStates = new List<State<char>>();
            lastStates.Add(stateInitial);

            for (int i = 0; i < RegexFuzzy.Length; i++)
            {
                bool final = (i == RegexFuzzy.Length - 1);

                TypeCharEnum typeActual = GetTypeChar(RegexFuzzy[i]);

                TypeCharEnum typeNext = TypeCharEnum.Final;

                if (!final)
                {
                    typeNext = GetTypeChar(RegexFuzzy[i + 1]);
                }
                if (typeActual == TypeCharEnum.Interval || typeActual == TypeCharEnum.Symbol || (typeActual == TypeCharEnum.Loop && typeNext == TypeCharEnum.Final))
                {
                    var nextState = Automa.AddState(NextNameState(), 0, Convert.ToInt16(final));
                    List<char> interval = null;
                    if (ValidateInterval(i))
                    {
                        interval = AddInterval(ref i, nextState, lastStates);

                        typeNext = TypeCharEnum.Final;
                        final = (i == RegexFuzzy.Length - 1);
                        if (!final)
                            typeNext = GetTypeChar(RegexFuzzy[i + 1]);
                    }
                    else //if ()
                    {
                        foreach (var item in lastStates)
                        {
                            AddSimpleNext(item.Name, nextState.Name, RegexFuzzy[i]);
                        }
                    }

                    if (typeNext == TypeCharEnum.Loop)
                    {
                        AddLoop(ref i, lastStates, nextState, final, interval);
                        final = (i == RegexFuzzy.Length - 1);
                    }
                    else
                    {
                        lastStates.Clear();
                        lastStates.Add(nextState);
                    }

                    if (final)
                    {
                        List<char> inclusion = alphabet.ToList();
                        inclusion.Add(SYMBOLEMPTY);
                        AbstractRule<char> roleAlphabet = new SimpleListIncludeRule<char>(ValueDefaultTransitions, inclusion);
                        foreach (var item in lastStates)
                        {
                            item.PertinenceFinal = 1;
                            Automa.FinalStates.Add(item);
                            Automa.AddTransition(item.Name, item.Name, roleAlphabet);
                        }
                    }
                }
            }

            return Automa;
        }

        private bool ValidateInterval(int i)
        {
            var type = GetTypeChar(RegexFuzzy[i]);
            return (type == TypeCharEnum.Interval && RegexFuzzy.IndexOf(']', i) > -1);
        }

        private void AddSimpleNext(string actual, string next, char c)
        {
            AbstractRule<char> roleChar = new SimpleIncludeRule<char>(c, 1);
            AbstractRule<char> roleEmpty = new SimpleIncludeRule<char>(SYMBOLEMPTY, ValueDefaultTransitions);
            AbstractRule<char> roleExclusion = new SimpleExclusionRule<char>(c, alphabet.ToList(), ValueDefaultTransitions);
            Automa.AddTransition(actual, next, roleChar);
            Automa.AddTransition(actual, next, roleEmpty);
            Automa.AddTransition(actual, actual, roleExclusion);
            //Automa.AddTransition(actual, actual, roleEmpty);
            Automa.AddTransition(actual, next, roleExclusion);
        }

        private void AddSimpleNext(string actual, string next, List<char> interval)
        {
            AbstractRule<char> roleChar = new SimpleListIncludeRule<char>(1, interval);
            AbstractRule<char> roleEmpty = new SimpleIncludeRule<char>(SYMBOLEMPTY, ValueDefaultTransitions);
            AbstractRule<char> roleExclusion = new SimpleListExclusionRule<char>(interval, alphabet.ToList(), ValueDefaultTransitions);
            Automa.AddTransition(actual, next, roleChar);
            Automa.AddTransition(actual, next, roleEmpty);
            Automa.AddTransition(actual, actual, roleExclusion);
            //Automa.AddTransition(actual, actual, roleEmpty);
            Automa.AddTransition(actual, next, roleExclusion);
        }

        private List<char> AddInterval(ref int i, State<char> next, List<State<char>> lastStates)
        {
            List<char> interval;
            i = GetInterval(i, out interval);
            foreach (var item in lastStates)
            {
                AddSimpleNext(item.Name, next.Name, interval);
            }
            return interval;
        }
        private void AddLoop(ref int i, List<State<char>> lastStates, State<char> state, bool final, List<char> interval)
        {

            int ini = 0, fim = 0;
            char current = RegexFuzzy[i];
            i = GetLoop(ref ini, ref fim, i);
            if (ini != 0)
            {
                lastStates.Clear();
            }

            lastStates.Add(state);

            int j;
            State<char> ant = lastStates[0];
            State<char> prox = null;
            for (j = 0; j < ini - 1; j++)
            {
                prox = Automa.AddState(NextNameState(), 0, Convert.ToInt16(final && ini == j - 1));
                if (interval == null)
                {
                    AddSimpleNext(ant.Name, prox.Name, current);
                }
                else
                {
                    AddSimpleNext(ant.Name, prox.Name, interval);
                }
                ant = prox;
            }

            if (ini != 0)
            {
                lastStates.Clear();
                lastStates.Add(ant);
            }

            if (fim == 0)
            {
                var last = lastStates.Last();

                if (interval == null)
                {
                    AddSimpleNext(last.Name, last.Name, current);
                }
                else
                {
                    AddSimpleNext(last.Name, last.Name, interval);
                }
            }
            else
            {
                for (j = ini; j < fim; j++)
                {
                    var nextState = Automa.AddState(NextNameState(), 0, Convert.ToInt16(final));

                    foreach (var item in lastStates)
                    {

                        if (interval == null)
                        {
                            AddSimpleNext(item.Name, nextState.Name, current);
                        }
                        else
                        {
                            AddSimpleNext(item.Name, nextState.Name, interval);
                        }

                    }
                    lastStates.Add(nextState);
                }
            }
        }

        public TypeCharEnum GetTypeChar(char p)
        {
            switch (p)
            {
                case '[':
                    return TypeCharEnum.Interval;
                case '{':
                    return TypeCharEnum.Loop;
                default:
                    return TypeCharEnum.Symbol;
            }
        }

        [DataMember]
        public List<List<double>> TableAutomaProcessing
        {
            get; set;
        }
        
        public virtual double Match(char source)
        {
            return Match(source.ToString());
        }
        public virtual double Match(string source)
        {
            TableAutomaProcessing = new List<List<double>>();
            int iSource = 0;
            char cActual = SYMBOLEMPTY;
            TableAutomaProcessing.Add(new List<double>());
            for (int i = 0; i < Automa.States.Count; i++)
            {
                TableAutomaProcessing[0].Add(Automa.States[i].PertinenceInitial);
            }

            int steps = (source.Length * 2) + 1;
            for (int step = 1; step <= steps; step++)
            {

                if (iSource == source.Length)
                {
                    iSource++;
                    cActual = SYMBOLEMPTY;
                }
                else
                {
                    if (step % 2 == 1)
                    {
                        cActual = SYMBOLEMPTY;
                    }
                    else
                    {
                        cActual = source[iSource++];
                    }
                }

                TableAutomaProcessing.Add(new List<double>());
                for (int i = 0; i < Automa.States.Count; i++)
                {
                    int stepOld = step - 1;
                    double pertinence = double.MinValue;
                    int indexMinor = 0;
                    for (int j = 0; j < TableAutomaProcessing[stepOld].Count; j++)
                    {
                        double maxState = TableAutomaProcessing[stepOld][j];
                        double pertinenceNew = 0;
                        if (maxState > 0)
                        {
                            pertinenceNew =
                                   Automa.SearchPertinence(Automa.States[j], Automa.States[i], cActual, Norm, Conorm);
                            pertinenceNew = Norm.Calculate(pertinenceNew, maxState);
                        }
                        if (cActual == SYMBOLEMPTY)
                        {
                            pertinenceNew = Conorm.Calculate(pertinenceNew, TableAutomaProcessing[stepOld][i]);
                        }

                        if (pertinence < pertinenceNew)
                        {
                            pertinence = pertinenceNew;
                            indexMinor = j;
                        }

                    }
                    TableAutomaProcessing[step].Add(pertinence);
                }
            }


            double maxFinal = 0;
            for (int i = 0; i < Automa.States.Count; i++)
            {
                if (Automa.States[i].PertinenceFinal > 0)
                    maxFinal = Conorm.Calculate(Norm.Calculate(TableAutomaProcessing[steps][i], Automa.States[i].PertinenceFinal), maxFinal);
            }
            return maxFinal;
        }

        private double MaxState(int j, List<List<double>> TableAutomaProcessing)
        {
            double max = 0;
            for (int i = 0; i < TableAutomaProcessing.Count; i++)
            {
                if (TableAutomaProcessing[i].Count > j && max < TableAutomaProcessing[i][j])
                {
                    max = TableAutomaProcessing[i][j];
                }
            }
            return max;
        }

        private int idState = 0;
        private string NextNameState()
        {
            return string.Format("q{0}", idState++);
        }

        private static string alphabet;
        public static string GetAlphabet()
        {
            if (string.IsNullOrWhiteSpace(alphabet))
            {
                alphabet = "";
                for (int i = 1; i < 256; i++)
                {
                    alphabet += (char)i;
                }
            }

            return alphabet;
        }
        
        public static List<char> GetInterval(char a, char b)
        {
            List<char> returns = new List<char>();
            for (int i = (char)(((int)a) + 1); i <= (int)b; i++)
            {
                returns.Add((char)i);
            }


            return returns;
        }

        private int GetInterval(int iActual, out List<char> interval)
        {
            interval = new List<char>();
            string temp = RegexFuzzy.Substring(iActual + 1, RegexFuzzy.IndexOf(']', iActual) - iActual);

            for (int i = 0; i < temp.Length; i++)
            {
                if (i > 0 && temp[i] == '-')
                {
                    interval.AddRange(GetInterval(temp[i - 1], temp[i + 1]));
                    i++;
                }
                else if (temp[i] != '[' && temp[i] != ']')
                {
                    interval.Add(temp[i]);
                }
            }
            interval = interval.Distinct().ToList();
            return RegexFuzzy.IndexOf(']', iActual);
        }
        private int GetLoop(ref int start, ref int end, int iActual)
        {
            string temp = RegexFuzzy.Substring(iActual, RegexFuzzy.IndexOf('}', iActual) - iActual);

            string[] res = temp.Split(',');

            start = Convert.ToInt32(Utils.GetNumbers(res[0]));

            temp = Utils.GetNumbers(res[1]);

            if (string.IsNullOrWhiteSpace(temp))
            {
                end = 0;
            }
            else
            {
                end = Convert.ToInt32(temp);
            }

            return RegexFuzzy.IndexOf('}', iActual);
        }


    }
}
