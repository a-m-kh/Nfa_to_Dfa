using System;
using System.Collections.Generic;
using System.Linq;
namespace ConvertNFA_TO_DFA
{

    class SuperState
    {
        public string name;
        public bool final = false;
        public List<State> states = new List<State>();
    }



    class State
    {
        public string name;
        public bool final = false;
        public List<Yal> yals = new List<Yal>();
        public State(string Name)
        {
            name = Name;
        }
    }

    class Yal
    {
        public State now;
        public string language;
        public State next;
    }
    class Program
    {
        static List<SuperState> Compare4(List<SuperState>All)
        {
            List<SuperState> between = new List<SuperState>();
            int j = 0;
            foreach(var Super in All)
            {
                if(j == 0)
                {
                    j++;
                    between.Add(Super);
                }
                else
                {
                    int z = 0;
                    foreach (var SuBe in between)
                    {                        
                        int k = 0;
                        foreach (var s in Super.states)
                        {
                            if (SuBe.states.Find(a => a.name == s.name) != null && Super.states.Count == SuBe.states.Count)
                            {
                                k++;
                            }
                        }
                        if (k == Super.states.Count )
                        {
                            z++;                            
                        }
                        k = 0;
                    }
                    if(z == 0)
                    {
                        between.Add(Super);
                        //break;
                    }
                    z = 0;
                }

            }
            return between;
        }
        static List<State> Compare3(List<State>n1)
        {
            List<State> L = new List<State>();
            var x = n1.Distinct<State>();            
            return x.ToList();
        }
        static void NextNode(List<SuperState> Now,ref List<SuperState> All, string[] Language, State Trap , int step)
        {
            List<SuperState> Find = new List<SuperState>();
            foreach (SuperState ss in Now)
            {           
                foreach (string L in Language)
                {
                int T = 0;
                List<State> NowState = new List<State>();
                
                    foreach (State s in ss.states)
                    {
                        foreach (Yal y in s.yals)
                        {
                            if (y.language == L)
                            {
                                T++;
                                List<State> example = new List<State> { y.next };
                                List<State> Landas = FindLanda(example);
                                /*Console.WriteLine("------------");
                                foreach (var x in Landas)
                                {
                                    Console.Write(x.name + "   ");
                                }
                                Console.WriteLine();*/
                                Landas.Add(y.next);
                                foreach (State e in Landas)
                                {
                                    NowState.Add(e);
                                }
                                NowState = Compare3(NowState);
                            }
                        }
                    }
                    SuperState NEW = new SuperState();  
                    NowState = Compare3(NowState);
                    if (T == 0)
                    {
                        NEW.states.Add(Trap);
                    }
                    else
                    {
                        NEW.states = NowState;
                    }
                    Find.Add(NEW);
                }
                
            }
            bool status = Compare2(ref All, Find);
            if (status)
            {
                //Console.WriteLine(status);
                return;
            }
            else
            {
                Compare(ref All, Find);
                /*Console.WriteLine("-----All");
                foreach(var x in All)
                {
                    Console.WriteLine("-----------------");
                    foreach(var s in x.states)
                    {
                        Console.Write(s.name + "  ");
                    }
                    Console.WriteLine();
                }*/
                NextNode(Find, ref All, Language, Trap , step+1);
            }
        }
        static List<State> FindLanda(List<State> FindPath)
        {
            List<State> now = new List<State>();
            int i = 0;
            foreach (var node in FindPath.ToList())
            {
                foreach (Yal yal in node.yals)
                {
                    if (yal.language == "$")
                    {
                        i++;
                        now.Add(yal.next);
                    }
                }
            }
            if (i != 0)
            {
                var fin = FindLanda(now);
                foreach (State node in fin)
                {
                    now.Add(node);
                }               
            }
            return now;
        }
        static bool Compare2(ref List<SuperState> All, List<SuperState> Now)
        {
            int k = 0;
            foreach (SuperState n in Now)
            {
                if (n.name == "Trap")
                {
                    if (All.Find(a => a.name == "Trap") == null)
                    {
                        return false;
                    }
                    else
                    {
                        k++;
                        continue;
                    }
                }
                foreach (SuperState A in All)
                {
                    int j = 0;

                    foreach (State s in n.states)
                    {
                        var node = A.states.Find(a => a.name == s.name);
                        if (node != null)
                        {
                            j++;
                        }
                    }
                    if (j == n.states.Count && j == A.states.Count)
                    {
                        k++;
                        break;
                    }
                }
            }
            if (k == Now.Count)
            {
                return true;
            }
            return false;
        }

        static void Compare(ref List<SuperState>All, List<SuperState> Now)
        {
            List<SuperState> NEW = new List<SuperState>();


            foreach (SuperState n in Now)
            {
                if (n.states.Find(a => a.name == "Trap") != null)
                {
                    int i = 0;
                    foreach(var x in All.ToList())
                    {
                        if(x.states.Find(a => a.name == "Trap") != null)
                        {
                            i++;
                        }
                    }
                    if(i == 0 )
                    {
                        All.Add(n);
                        continue;
                    }
                }
                int Check = 0;
                foreach (SuperState A in All)
                {
                    Check++;
                    int j = 0;
                    foreach (State s in n.states)
                    {
                        var node = A.states.Find(a => a.name == s.name);
                        if (node != null)
                        {
                            j++;
                        }
                    }
                    if (j == n.states.Count && j == A.states.Count)
                    {
                        break;
                    }                   
                }
                if (Check == All.Count)
                {
                    if(n.states.Find(a => a.name == "Trap") != null)
                    {
                        continue;
                    }
                    All.Add(n);                                       
                    NEW.Add(n);                                      
                }
            }
            Now = NEW;
        }
        static void Main(string[] args)
        {
            string[] states = Console.ReadLine().Split('{', '}')[1].Split(',');
            string[] language = Console.ReadLine().Split('{', '}')[1].Split(',');
            string[] FinalState = Console.ReadLine().Split('{', '}')[1].Split(',');
            int number = int.Parse(Console.ReadLine());
            string[,] yals = new string[number, 3];
            List<State> States = new List<State>();

            foreach (string state in states)
            {
                State s = new State(state);
                States.Add(s);
            }
            foreach (string state in FinalState)
            {
                States.Find(a => a.name == state).final = true;
            }
            for (int i = 0; i < number; i++)
            {
                string[] yal = Console.ReadLine().Split(',');
                Yal y = new Yal();
                y.now = States.Find(a => a.name == yal[0]);
                y.language = yal[1];
                y.next = States.Find(a => a.name == yal[2]);
                States.Find(a => a.name == yal[0]).yals.Add(y);
            }
            var Trap = new State("Trap");
            var first = States.Where(a => a.name == states[0]).FirstOrDefault();
            SuperState S = new SuperState();
            S.name = first.name;
            S.states.Add(first);
            List<SuperState> All = new List<SuperState>();
            State Start = States.Find(a => a.name == states[0]);
            List<State> st = new List<State> { Start };
            List<State> Landa = FindLanda(st); 
            SuperState ss = new SuperState();
            ss.states.Add(Start);
            foreach(var s in Landa)
            {
                ss.states.Add(s);
            }
            List<SuperState> now = new List<SuperState> { ss };
            NextNode(now,ref All, language, Trap , 0);
            var x = Compare4(All);
            int error = 0;
            foreach(var z in x.ToList())
            {
                foreach(var d in z.states)
                {
                    if(d.name == states[0] && z.states.Count == 1)
                    {
                        error++;
                        Console.WriteLine(x.ToList().Count);
                        break;
                    }
                }
            }
            if(error ==0)
            {

                Console.WriteLine(x.ToList().Count + 1);
            }            
        }
    }
}
