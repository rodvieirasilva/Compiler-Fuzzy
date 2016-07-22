using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompilerWithFuzzy.GrammarFuzzy;
using CompilerWithFuzzy.AutomaFuzzy;
using CompilerWithFuzzy.AutomaFuzzy.Rules;
using System.Data;
using CompilerWithFuzzy.Compiler.DataStruct;
using CompilerWithFuzzy.Compiler.Lex;
using CompilerWithFuzzy.Core;
using System.Runtime.Serialization;
using System.Collections;

namespace CompilerWithFuzzy.Compiler.Syn
{
    [DataContract]
    [Serializable]
    public class SyntacticAnalysisLR1 : SyntacticAnalysisAbstract
    {
        [DataMember]
        public List<RuleProduction> Productions { get; set; }
        [DataMember]
        public Automa<Symbol> Automa { get; set; }

        [DataMember]
        public Grammar GrammarLine { get; set; }

        [DataMember]
        public List<RuleProductionState> Rules { get; set; }

        [DataMember]
        public Symbol SymbolInitialLine { get; set; }

        [DataMember]
        public Symbol SymbolInitial { get; set; }

        [DataMember]
        public Dictionary<State<Symbol>, Dictionary<Symbol, List<Operation>>> Table;

        [DataMember]
        public DataTable DataTable { get; set; }

        [DataMember]
        public State<Symbol> FirstState { get; set; }

        public SyntacticAnalysisLR1() : base(null, null, null)
        {

        }
        public SyntacticAnalysisLR1(GrammarFuzzy.Grammar grammar, NormAbstract norm, ConormAbstract conorm) : base(grammar, norm, conorm)
        {
            CacheFirst = new Hashtable();
            Table = new Dictionary<State<Symbol>, Dictionary<Symbol, List<Operation>>>();
            //Rules = new List<RuleProductionState>();
            List<Symbol> alphabet = new List<Symbol>();
            alphabet.AddRange(grammar.Variables);
            alphabet.AddRange(grammar.Terminals);
            this.Grammar = grammar;
            GrammarLine = new GrammarFuzzy.Grammar();
            GrammarLine.Rules.AddRange(grammar.Rules);
            GrammarLine.Terminals.AddRange(grammar.Terminals);
            GrammarLine.Variables.AddRange(grammar.Variables);
            GrammarLine.VariablesEmpty.AddRange(grammar.VariablesEmpty);
            SymbolInitialLine = new Symbol(100, "S'", false);
            GrammarLine.Variables.Add(SymbolInitialLine);
            GrammarLine.AddRule(SymbolInitialLine, grammar.VariableStart);
            GrammarLine.VariableStart = SymbolInitialLine;
            Automa = new Automa<Symbol>(alphabet, Symbol.EmptySymbol);
            SymbolInitial = grammar.VariableStart;
            CreateAutoma();
            CreateTable();
        }

        private void CreateAutoma()
        {
            ValueState value = new ValueState();
            RuleProductionState rule = new RuleProductionState();

            rule.Source = GrammarLine.VariableStart;
            rule.Destiny = GrammarLine.GetRules(rule.Source)[0].Destiny;
            rule.Id = GrammarLine.GetRules(rule.Source)[0].Id;
            rule.TypeName = GrammarLine.GetRules(rule.Source)[0].TypeName;

            rule.Pointer = 0;
            rule.Pertinence = 1;
            rule.Lookahead = Symbol.TapeFinal;
            rule.Parent = GrammarLine.GetRules(rule.Source)[0].Parent;
            value.Rules.Add(rule);
            State<Symbol> state = new State<Symbol>(0, "I000", 1, 0, Closure(value));
            FirstState = state;
            // Rules.AddRange(state.GetValue<ValueState>().Rules);

            Automa.States.Add(state);
            bool change = true;
            //  while (change)
            {
                change = false;

                for (int i = 0; i < Automa.States.Count; i++)
                {

                    var stateActualI = Automa.States[i];

                    SymbolList symbols = Grammar.Symbols;
                    for (int j = 0; j < symbols.Count; j++)
                    {

                        var symbolX = symbols[j];

                        var valStateI = stateActualI.GetValue<ValueState>();

                        var valueGoto = Goto(valStateI, symbolX);

                        if (valueGoto.Rules.Count > 0)
                        {
                            //var item = value.Rules 
                            // foreach (var item in value.Rules)
                            {
                                state = Automa.States.FirstOrDefault(s => s.GetValue<ValueState>().Equals(valueGoto));
                                if (state == null)
                                {
                                    state = new State<Symbol>(-1, "Temp", 1, 0, valueGoto);
                                    state.Name = string.Format("I{0:000}", state.Id);
                                    Automa.States.Add(state);
                                }

                                //    Rules.AddRange(value.Rules);

                                Transition<Symbol> transition = new Transition<Symbol>();
                                transition.From = stateActualI;
                                stateActualI.Transitions.Add(transition);


                                transition.To = state;
                                var sir = new SimpleIncludeRule<Symbol>(symbolX, 1);
                                //  sir.Pertinence = item.Pertinence * Automa.States[i].GetValue<ValueState>().Rules[0].Pertinence;

                                transition.Rule = sir;

                                Automa.Transitions.Add(transition);

                                change = true;

                            }
                        }


                    }
                }

            }
        }



        public ValueState Goto(ValueState i, Symbol x)
        {


            ValueState j = new ValueState();

            foreach (var ruleI in i.Rules)
            {
                if (ruleI.Destiny.Contains(x))
                {
                    if (ruleI.Pointer < ruleI.Destiny.Count)
                    {
                        var destiny = ruleI.Destiny[ruleI.Pointer];

                        if (destiny == x)
                        {
                            RuleProductionState rule = new RuleProductionState();
                            rule.Id = ruleI.Id;
                            rule.Source = ruleI.Source;
                            rule.Lookahead = ruleI.Lookahead;
                            rule.Destiny.AddRange(ruleI.Destiny);
                            rule.Parent = ruleI.Parent;
                            rule.Pertinence = ruleI.Pertinence;
                            rule.TypeName = ruleI.TypeName;
                            rule.Pointer = ruleI.Pointer + 1;
                            rule.CalculateHash();

                            if (!j.HashCodeRules.ContainsKey(rule.HashCode))
                            {
                                j.AddRule(rule);

                            }

                        }
                    }
                }
            }
            return Closure(j);
        }


        public ValueState Closure(ValueState state)
        {
            // ValueState returns = new ValueState();

            bool change = true;
            //while (change)
            {
                change = false;
                //
                for (int i = 0; i < state.Rules.Count; i++)
                {
                    //A->alphaBbeta, a
                    Symbol A = state.Rules[i].Source;
                    List<Symbol> alpha = state.Rules[i].Destiny.Take(state.Rules[i].Pointer).ToList();
                    if (alpha.Count == 0)
                    {
                        alpha.Add(Symbol.EmptySymbol);
                    }
                    Symbol B = Symbol.EmptySymbol;
                    if (state.Rules[i].Destiny.Count > state.Rules[i].Pointer)
                    {
                        B = state.Rules[i].Destiny[state.Rules[i].Pointer];
                    }
                    Symbol beta = Symbol.TapeFinal;
                    if (state.Rules[i].Pointer + 1 < state.Rules[i].Destiny.Count)
                        beta = state.Rules[i].Destiny[state.Rules[i].Pointer + 1];

                    Symbol a = state.Rules[i].Lookahead;

                    var rolesB = GrammarLine.GetRules(B);
                    for (int j = 0; j < rolesB.Count; j++)
                    {
                        var first = First(beta, a);

                        for (int k = 0; k < first.Count; k++)
                        {
                            if (first[k].Terminal || first[k] == Symbol.TapeFinal)
                            {
                                RuleProductionState rule = new RuleProductionState();
                                rule.Id = rolesB[j].Id;
                                rule.TypeName = rolesB[j].TypeName;
                                rule.Source = B;
                                rule.Destiny.AddRange(rolesB[j].Destiny);
                                rule.Pointer = 0;
                                rule.Lookahead = first[k];
                                rule.Parent = rolesB[j].Parent;
                                rule.CalculateHash();
                                // state.Rules[i].Pertinence;

                                if (!state.HashCodeRules.ContainsKey(rule.HashCode))
                                //if (!state.Rules.Any(r => r.Equals(rule)))
                                {
                                    rule.Pertinence = rolesB[j].Pertinence;// *state.Rules[i].Pertinence;// 0.8;
                                    state.AddRule(rule);
                                    change = true;
                                }
                            }
                        }
                    }
                }
            }

            return state;
        }


        public Hashtable CacheFirst { get; set; }
        public List<Symbol> First(Symbol symbol, Symbol lookAHead)
        {
            if (!CacheFirst.ContainsKey(symbol))
            {
                CacheFirst.Add(symbol, new Hashtable());
            }

            if (((Hashtable)CacheFirst[symbol]).ContainsKey(lookAHead))
            {
                return (List<Symbol>)((Hashtable)CacheFirst[symbol])[lookAHead];
            }
            else
            {
                ((Hashtable)CacheFirst[symbol]).Add(lookAHead, null);
            }

            List<Symbol> returns = new List<Symbol>();

            if (symbol.Terminal)
            {
                returns.Add(symbol);
            }
            else
            {
                Queue<Symbol> queue = new Queue<Symbol>();
                queue.Enqueue(symbol);

                Dictionary<Symbol, bool> visitados = new Dictionary<Symbol, bool>();
                while (queue.Count > 0)
                {
                    Symbol actual = queue.Dequeue();

                    var rules = GrammarLine.GetRules(actual);

                    foreach (var rule in rules)
                    {
                        List<Symbol> firsts = new List<Symbol>();

                        foreach (var destiny in rule.Destiny)
                        {
                            firsts.Add(destiny);
                            if (!destiny.GoToEmpty)
                            {
                                break;
                            }
                        }


                        foreach (var first in firsts)
                        {
                            if (first.Terminal)
                            {
                                var temp = first;
                                if (temp == Symbol.EmptySymbol)
                                {
                                    temp = Symbol.TapeFinal;
                                }

                                if (!returns.Contains(temp))
                                    returns.Add(temp);
                            }
                            else
                            {
                                if (!visitados.ContainsKey(first))
                                {
                                    visitados.Add(first, true);
                                    queue.Enqueue(first);
                                }
                            }

                        }

                    }
                }
            }

            if (returns.Count == 0)// [0] != Symbol.TapeFinal)
            {
                returns.Add(lookAHead);
            }

            ((Hashtable)CacheFirst[symbol])[lookAHead] = returns;

            return returns;
        }

        public void CreateTable()
        {
            //• Para toda aresta I X−→ J Na linha I coluna X 
            //insira: – shift J se X ´e terminal 
            //          – goto J se X ´e n˜ao-terminal •
            // S’ → S.$ – insira accept 
            //• A → α. – insira reduce para todo token
            //    A tabela LR(1) ´e constru´ıda da mesma forma que a LR(0) exceto na maneira de inserir os 
            //reduces (redu¸c˜oes). Quando h´a um ´ıtem [A → α., a] em um estado i,
            //ent˜ao deve-se inserir na linha i coluna a a redu¸c˜ao da regra de produ¸c˜ao A → α. 
            var symbols = GetOrdenedSymbols();
            foreach (var state in Automa.States)
            {
                Table.Add(state, new Dictionary<Symbol, List<Operation>>());
                foreach (var symbol in symbols)
                {
                    Table[state].Add(symbol, new List<Operation>());
                }

                ValueState val = state.GetValue<ValueState>();
                foreach (var rule in val.Rules)
                {
                    Operation operation = new Operation(state, rule);
                    operation.Pertinence = 1;
                    operation.Symbol = rule.Source;
                    if (rule.Pointer == rule.Destiny.Count)
                    {
                        if (rule.Source == SymbolInitialLine && rule.Destiny.Count == 1 && rule.Destiny[0] == SymbolInitial)
                        {
                            operation.Type = TypeOperation.Acept;
                        }
                        else
                        {
                            operation.Type = TypeOperation.Reduce;

                        }
                        Table[state][rule.Lookahead].Add(operation);
                    }
                }
            }

            foreach (var transition in Automa.Transitions)
            {
                ValueState valTo = transition.To.GetValue<ValueState>();
                ValueState valFrom = transition.To.GetValue<ValueState>();


                foreach (var rule in valTo.Rules)
                {

                    Operation op = new Operation(transition.To, rule);
                    op.Pertinence = 1;
                    op.Symbol = rule.Source;
                    if (transition.Rule.Symbol.Terminal)
                    {
                        op.Type = TypeOperation.Shift;
                    }
                    else
                    {
                        op.Type = TypeOperation.Goto;
                        op.State = transition.To;
                    }

                    if (!Table[transition.From][transition.Rule.Symbol].Contains(op))
                    {
                        Table[transition.From][transition.Rule.Symbol].Add(op);
                    }
                }

            }
        }

        public override DataTable GetDataTable()
        {
            DataTable = new DataTable("Data");

            var symbols = GetOrdenedSymbols();
            DataTable.Columns.Add("State");
            foreach (var symbol in symbols)
            {
                DataTable.Columns.Add(symbol.ToString());
            }

            foreach (var keyState in Table.Keys)
            {
                DataRow dr = DataTable.NewRow();
                dr.SetField("State", keyState.Name);
                foreach (var keySymbol in Table[keyState].Keys)
                {
                    string op = string.Empty;
                    foreach (Operation operation in Table[keyState][keySymbol])
                    {
                        op += operation.ToString() + ", ";
                    }
                    dr.SetField(keySymbol.ToString(), op.Trim().Trim(','));
                }
                DataTable.Rows.Add(dr);

            }

            return DataTable;
        }

        public double Validate(params Symbol[] tokens)
        {
            return Validate(tokens.ToList());
        }

        public override double Validate(List<Symbol> tokens)
        {
            CacheFirst = null;
            try
            {
                GraphsSyntactic = new List<Graph<Symbol, double>>();
                tokens.Add(Symbol.TapeFinal);
                Productions = new List<RuleProduction>();
                Graph<State<Symbol>, double> graph = new Graph<State<Symbol>, double>();
                FirstState.PertinenceInitial = 1;
                var firstNode = graph.AddNode("First", FirstState);
                firstNode.Parent = null;
                List<Node<State<Symbol>, double>> leafs = new List<Node<State<Symbol>, double>>();

                int idNode = 1;
                List<Node<State<Symbol>, double>> newLeafs = new List<Node<State<Symbol>, double>>();

                CustomValueStateNodeLR1 customValueStateNodeLR1 = new CustomValueStateNodeLR1();
                customValueStateNodeLR1.CoutPaths = 1;
                //customValueStateNodeLR1.Tokens.Add(tokens[0]);
                customValueStateNodeLR1.IToken = 1;
                customValueStateNodeLR1.ASymbol = tokens[0];
                customValueStateNodeLR1.ASymbolReal = tokens[0];

                firstNode.SetCustomOject("CustomValueStateNodeLR1", customValueStateNodeLR1);
                leafs.Add(firstNode);

                double maxAcept = 0;
                while (true)
                {
                    for (int iLeaf = 0; iLeaf < leafs.Count; iLeaf++)
                    {

                        var sLeaf = leafs[iLeaf];
                        customValueStateNodeLR1 = sLeaf.GetCustomOject<CustomValueStateNodeLR1>("CustomValueStateNodeLR1");

                        #region GETOP
                        double pertinenceS = sLeaf.Info.PertinenceInitial;
                        double newPertinence = 1;

                        var operations = Table[sLeaf.Info][customValueStateNodeLR1.ASymbol];
                        var emptyrule = false;

                        if (operations.Count == 0)
                        {
                            if (Table[sLeaf.Info].ContainsKey(Symbol.EmptySymbol))
                            {
                                operations = Table[sLeaf.Info][Symbol.EmptySymbol];
                                emptyrule = true;
                            }
                        }

                        if (operations.Count == 0)
                        {
                            operations = Table[sLeaf.Info][Symbol.TapeFinal];
                            emptyrule = true;
                        }

                        #endregion GETOP

                        if (operations.Count > 0)

                            for (int iOp = 0; iOp < operations.Count; iOp++)
                            {

                                customValueStateNodeLR1 = sLeaf.GetCustomOject<CustomValueStateNodeLR1>("CustomValueStateNodeLR1");

                                Operation op = operations[iOp];

                                #region FOREACH
                                if (op.Type == TypeOperation.Shift)
                                {
                                    #region SHIFT
                                    newPertinence = Norm.Calculate(pertinenceS, op.Pertinence);
                                    var newLeaf = graph.AddNode(idNode++, op.State, sLeaf, newPertinence);
                                    CustomValueStateNodeLR1 newCustomValueStateNodeLR1 = new CustomValueStateNodeLR1(customValueStateNodeLR1);
                                    newCustomValueStateNodeLR1.IndexInParent = iOp;

                                    List<Symbol> sNewTokens = new List<Symbol>();
                                    if (!emptyrule)
                                    {
                                        if (newCustomValueStateNodeLR1.IToken >= tokens.Count)
                                        {
                                            newCustomValueStateNodeLR1.ASymbol = Symbol.TapeFinal;
                                        }
                                        else
                                        {
                                            newCustomValueStateNodeLR1.ASymbol = tokens[newCustomValueStateNodeLR1.IToken];
                                        }
                                        newCustomValueStateNodeLR1.IToken++;
                                    }
                                    newLeaf.Info.PertinenceInitial = newPertinence;
                                    newLeaf.SetCustomOject("CustomValueStateNodeLR1", newCustomValueStateNodeLR1);
                                    newLeafs.Add(newLeaf);
                                    #endregion SHIFT
                                }
                                else if (op.Type == TypeOperation.Reduce)
                                {
                                    #region REDEUCE 1
                                    RuleProduction rule = op.Rule;
                                    var prox = sLeaf;
                                    int popStack = op.Rule.Destiny.Count;
                                    int countNodes = rule.Destiny.Count;
                                    for (int i = 0; i < popStack; i++)
                                    {
                                        newPertinence = prox.Info.PertinenceInitial;
                                        var temp = prox;
                                        //graph.RemoveNode(prox);
                                        prox = prox.Parent;
                                    }
                                    #endregion REDEUCE 1
                                    #region GOTO
                                    var operationsGoto = Table[prox.Info][rule.Source];
                                    if (operationsGoto.Count > 0)
                                    {
                                        var state = operationsGoto[0].State;

                                        newPertinence = Norm.Calculate(prox.Info.PertinenceInitial, op.Rule.Pertinence);

                                        var newLeaf = graph.AddNode(idNode++, state, prox, newPertinence);
                                        CustomValueStateNodeLR1 newCustomValueStateNodeLR1 = new CustomValueStateNodeLR1(customValueStateNodeLR1);
                                        newCustomValueStateNodeLR1.IndexInParent = iOp;
                                        newCustomValueStateNodeLR1.Productions.Add(rule);

                                        newLeaf.SetCustomOject("CustomValueStateNodeLR1", newCustomValueStateNodeLR1);

                                        newCustomValueStateNodeLR1.Pertinence = newPertinence;

                                        newLeaf.Info.PertinenceInitial = newPertinence;

                                        newLeafs.Add(newLeaf);
                                    }
                                    #endregion GOTO
                                }
                                else if (op.Type == TypeOperation.Acept)
                                {
                                    #region Acept

                                    Graph<Symbol, double> graphA =
                                        CreateTreeSyntax(customValueStateNodeLR1.Productions,
                                           tokens);

                                    GraphsSyntactic.Add(graphA);

                                    if (sLeaf.Info.PertinenceInitial > maxAcept)
                                    {
                                        Productions = customValueStateNodeLR1.Productions;
                                        maxAcept = sLeaf.Info.PertinenceInitial;
                                    }
                                    #endregion Acept
                                }
                                else
                                {
                                    //TODO: Recuperar de errros
                                    return 0;
                                }

                                #endregion FOREACH
                            }
                    }

                    leafs = newLeafs;

                    if (leafs.Count == 0)
                        return maxAcept;

                    newLeafs = new List<Node<State<Symbol>, double>>();
                }


            }
            catch (Exception ex)
            {

            }
            return 0;

        }

        private Graph<Symbol, double> CreateTreeSyntax(List<RuleProduction> rules, List<Symbol> tokens)
        {
            iToken = 0;

            Stack<Node<Symbol, double>> stack = new Stack<Node<Symbol, double>>();

            int iRule = rules.Count - 1;
            var nodeActual = new Node<Symbol, double>(rules[iRule].Source.Name + (iRule), rules[iRule].Source);
            var firstNode = nodeActual;

            stack.Push(nodeActual);

            Graph<Symbol, double> result = new Graph<Symbol, double>();

            while (stack.Count > 0)
            {
                nodeActual = stack.Pop();
                RuleProduction ruleActual = rules[iRule--];
                nodeActual.SetCustomOject("Rule", ruleActual);

                for (int j = 0; j < ruleActual.Destiny.Count; j++)
                {
                    Node<Symbol, double> nodeChild = new Node<Symbol, double>(ruleActual.Destiny[j].Name, ruleActual.Destiny[j]);
                    if (ruleActual.Destiny[j].Terminal)
                    {
                        if (ruleActual.Destiny[j].Id == tokens[iToken].Id)
                        {
                            nodeChild.Info = ruleActual.Destiny[j];
                        }
                    }
                    else
                    {
                        stack.Push(nodeChild);
                    }
                    nodeActual.AddEdge(ruleActual.Pertinence, nodeChild, ruleActual.Pertinence);
                }
            }
            iToken = 0;
            FillTerminals(firstNode, tokens);

            result = firstNode.ToGraph();

            return result;
        }

        private int iToken;
        public void FillTerminals(Node<Symbol, double> nodeActual, List<Symbol> tokens)
        {
            if (nodeActual.Info.Terminal)
            {
                if (nodeActual.Info.Id == tokens[iToken].Id)
                {
                    nodeActual.Info = tokens[iToken];
                    iToken++;
                }
            }
            for (int j = 0; j < nodeActual.Edges.Count; j++)
            {
                FillTerminals(nodeActual.Edges[j].Destiny, tokens);
            }
        }

        public int IdsNode { get; set; }


        private List<Symbol> GetOrdenedSymbols()
        {
            var symbols = Grammar.Symbols.OrderBy(s => !s.Terminal).ToList();
            symbols.Insert(symbols.FindIndex(s => !s.Terminal), Symbol.TapeFinal);

            return symbols;

        }

        private object CustomOrTerminal(object custom, Symbol syn)
        {
            if (custom == null)
            {
                if (syn.Terminal)
                    return syn;
            }
            return custom;
        }
    }
}


