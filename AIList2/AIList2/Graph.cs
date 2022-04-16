using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIList2
{
    class Graph<T>
    {
        public List<Node<T>> Nodes { get; set; }
        public IDataReader<T> DataReader { get; set; }
        public T Empty { get; set; }
        public List<List<Node<T>>> ResultList { get; set; }
        public int ResultCounter { get; set; }
        public IConstraint<T> Constraint { get; set; }
        //public dynamic Constraint { get; set; }
        public Graph(IDataReader<T> dataReader, T empty)
        {
            DataReader = dataReader;
            Empty = empty;
            ResultCounter = 0;
            ResultList = new List<List<Node<T>>>();
        }
        public void ReadData()
        {
            Nodes = DataReader.ReadData();
            Constraint = DataReader.Constraint;
        }

        public void RunBackTracking()
        {
            BackTracking(0, Nodes);

            Console.WriteLine("Liczba znalezionych rozwiazan: " + ResultCounter);
            Console.WriteLine("Liczba przeszukanych wezlow: " + Constraint.Step);
        }

        public void RunForwardChecking(bool valueHeuristic = false, bool variableHeuristic = false)
        {
            ForwardCheckingVar(0, Nodes, valueHeuristic, variableHeuristic);

            Console.WriteLine("Liczba znalezionych rozwiazan: " + ResultCounter);

            Console.WriteLine("Liczba przeszukanych wezlow: " + Constraint.Step);
        }

        private void printResult(List<Node<T>> resultList)
        {
            int a = (int)Math.Sqrt(Nodes.Count);
            if (resultList[0].Value.Equals(-1)) Console.Write('x' + " ");
            else Console.Write(resultList[0].Value + " ");
            for (int i=1; i<resultList.Count; i++)
            {
                if (i % a == a-1)
                {
                    if (resultList[i].Value.Equals(-1)) Console.WriteLine('x' + " ");
                    else Console.WriteLine(resultList[i].Value + " ");
                }
                else
                {
                    if (resultList[i].Value.Equals(-1)) Console.Write('x' + " ");
                    else Console.Write(resultList[i].Value + " ");
                }      
            }
            Console.WriteLine("\n");
        }

        private List<Node<T>> listCopy(List<Node<T>> list)
        {
            List<Node<T>> resultList = new List<Node<T>>();
            foreach(Node<T> node in list)
            {
                Node<T> newNode = new Node<T>(node.Value, node.IsDefault);
                List<T> newDomain = new List<T>();
                foreach(T val in node.Domain)
                {
                    newDomain.Add(val);
                }
                newNode.Domain = newDomain;
                resultList.Add(newNode);
            }

            return resultList;
        }

        public void BackTracking(int iter, List<Node<T>> combination)
        {
            //petla po wszystkich watosciach dziedziny
            foreach (T val in combination[iter].Domain)
            {
                if (!combination[iter].IsDefault)
                    combination[iter].Value = val;
                Constraint.Nodes = combination;
                Constraint.Iter = iter;
                //sprawdzam czy poprawnie
                if (Constraint.checkConditions())
                {
                    //sprawdzam czy koniec
                    if (iter == combination.Count - 1)
                    {
                        printResult(combination);
                        ResultCounter++;
                    }
                    //jak nie to lece dalej
                    else
                    {
                        List<Node<T>> clonedList = listCopy(combination);
                        BackTracking(iter+1, clonedList);
                    }
                }
            }
        }


        private int ChooseNextVariable(List<Node<T>> combination, int iter)
        {
            int result = 0;
            int lowestDomainSize = (int)Math.Sqrt(combination.Count);
            for(int i=0; i< combination.Count; i++)
            {
                if (i!= iter && combination[i].Value.Equals(Empty) && combination[i].Domain.Count < lowestDomainSize)
                {
                    lowestDomainSize = combination[i].Domain.Count;
                    if (lowestDomainSize == 2) return i;
                    else
                    {
                        result = i;
                    }
                }
            }
            return result;
        }
        private bool isSolution(List<Node<T>> combination)
        {
            foreach (Node<T> elem in combination)
            {
                if (elem.Value.Equals(Empty)) return false;
            }
            return true;
        }

        //czy do liczby przeszukanych stanow wliczac nody z 1 wartoscia ktore automatycznie przypisuje i sprawdzam
        //czy to jest po prostu ilosc blednych wynikow algorytmu

        public void ForwardCheckingVar(int iter, List<Node<T>> combination, bool valueHeuristic, bool variableHeuristic)
        {
            if (isSolution(combination))
            {
                printResult(combination);
                ResultCounter++;
            }
            else if (combination[iter].Value.Equals(Empty) || combination[iter].IsDefault)
            {
                List<T> domain = new List<T>();
                if (valueHeuristic)
                {
                    domain = Constraint.ValueQueue(iter);
                }
                else
                {
                    domain = combination[iter].Domain;
                }
                foreach (T val in domain)
                {
                    combination[iter].Value = val;
                    //printResult(combination);
                    List<Node<T>> newCombination = listCopy(combination);
                    Constraint.Nodes = newCombination;
                    //Constraint.Iter=iter;
                    if (Constraint.checkConditionsCutDomain(iter))
                    {
                        if (isSolution(newCombination))
                        {
                            printResult(newCombination);
                            ResultCounter++;
                        }
                        //jak nie to lece dalej
                        else
                        {
                            List<Node<T>> clonedList = listCopy(newCombination);
                            int nextVar;
                            if (variableHeuristic)
                                nextVar = ChooseNextVariable(clonedList, iter);
                            else
                                nextVar = iter + 1;
                            ForwardCheckingVar(nextVar, clonedList, valueHeuristic, variableHeuristic);
                        }

                    }
                    combination[iter].Value = Empty;
                }
            }
            else
            {
                List<Node<T>> clonedList = listCopy(combination);
                int nextVar;
                if (variableHeuristic)
                    nextVar = ChooseNextVariable(clonedList, iter);
                else
                    nextVar = iter + 1;
                ForwardCheckingVar(nextVar, clonedList, valueHeuristic, variableHeuristic);
            }
        }
    }
}
