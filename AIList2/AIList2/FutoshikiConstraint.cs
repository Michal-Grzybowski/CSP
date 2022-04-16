using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIList2
{
    class FutoshikiConstraint : IConstraint<int>
    {
        public List<Node<int>> Nodes { get; set; }
        public bool isCorrect { get; set; }
        public int A { get; set; }
        public int Iter { get; set; }
        public int Step { get; set; }
        public List<(int, int)> OperatorList { get; set; }
        public FutoshikiConstraint(List<Node<int>> nodes, int a, int iter, List<(int, int)> operators)
        {
            Nodes = nodes;
            A = a;
            Iter = iter;
            OperatorList = operators;
            Step = 0;
        }
        public bool checkConditions()
        {
            Step++;
            if (!checkRepetition()) return false;
            if (!checkInequality()) return false;
            return true;
        }

        public bool checkConditionsCutDomain(int iter)
        {
            Step++;
            if (!CheckForwardIneaqualityAndCutDomain(iter))
            {
                return false;
            }
            if (!CheckForwardRepetitionCutDomain(iter))
            {
                return false;
            }
            return true;
        }


        //----------------------------------PRYWATNE FUNKCJE POMOCNICZE--------------------------------------

        private bool CheckForwardIneaqualityAndCutDomain(int iter)
        {
            foreach ((int, int) elem in OperatorList)
            {
                (int smaller, int larger) = elem;
                if (iter == smaller)
                {
                    Nodes[larger].Domain.RemoveAll(x => x <= Nodes[smaller].Value);
                    if (Nodes[larger].Domain.Count == 0) return false;
                    else if (Nodes[larger].Value == -1 && Nodes[larger].Domain.Count == 1) {
                        Nodes[larger].Value = Nodes[larger].Domain[0];
                        if(!checkConditionsCutDomain(larger)) return false;
                    }
                }
                if (iter == larger)
                {

                    Nodes[smaller].Domain.RemoveAll(x => x >= Nodes[larger].Value);
                    if (Nodes[smaller].Domain.Count == 0) return false;
                    else if (Nodes[smaller].Value == -1 && Nodes[smaller].Domain.Count == 1) {
                        Nodes[smaller].Value = Nodes[smaller].Domain[0];
                        if(!checkConditionsCutDomain(smaller)) return false;
                    }
                }
            }
            return true;
        }
        private bool CheckForwardRepetitionCutDomain(int iter)
        {   //sprawdzam poziom
            for (int i = iter-iter%A; i < iter - iter % A + A; i++)
            {
                if(iter!= i)
                {
                    if (Nodes[i].Value.Equals(Nodes[iter].Value))
                    {
                        return false;
                    }
                    else
                    {
                        Nodes[i].Domain.Remove(Nodes[iter].Value);
                        if (Nodes[i].Domain.Count == 0) return false;
                        else if (Nodes[i].Value == -1 && Nodes[i].Domain.Count == 1)
                        {
                            Nodes[i].Value = Nodes[i].Domain[0];
                            if (!checkConditionsCutDomain(i)) return false;
                        }
                    }
                }
            }
            //sprawdzam pion
            for (int i = iter % A ; i < Nodes.Count - iter % A; i += A)
            {

                if (iter != i)
                {
                    Nodes[i].Domain.Remove(Nodes[iter].Value);
                    if (Nodes[i].Domain.Count == 0) return false;
                    else if (Nodes[i].Value == -1 && Nodes[i].Domain.Count == 1)
                    {
                        Nodes[i].Value = Nodes[i].Domain[0];
                        if (!checkConditionsCutDomain(i)) return false;
                    }
                   // }
                }
            }
            return true;
        }

        //------------------------------
        private bool checkInequality()
        {
            foreach ((int,int) elem in OperatorList)
            {
                (int smaller, int larger) = elem;
                if (Nodes[larger].Value != -1 && Nodes[smaller].Value > Nodes[larger].Value) return false;
            }
            return true;
        }




        private bool checkRepetition()
        {
            //sprawdzam poziom
            for(int i=Iter - Iter%A; i< Iter - Iter % A + A; i++)
            {
                if (i == Iter) i++;
                if (i<Nodes.Count && Nodes[i].Value.Equals(Nodes[Iter].Value))
                {
                    return false;
                }
            }
            //pion
            for (int i = Iter % A; i < Nodes.Count - Iter%A; i+=A)
            {
                if (i == Iter) i+=A;
                if (i< Nodes.Count && Nodes[i].Value.Equals(Nodes[Iter].Value))
                {
                    return false;
                }
            }
            return true;
        }

        public List<int> ValueQueue(int iter)
        {
            foreach ((int, int) elem in OperatorList)
            {
                (int smaller, int larger) = elem;
                if (iter == smaller)
                {
                    //Nodes[iter].Domain.Remove(A);
                    return Nodes[iter].Domain;
                }
                else if (iter == larger)
                {
                    //Nodes[iter].Domain.Remove(1);
                    Nodes[iter].Domain.Reverse();
                    return Nodes[iter].Domain;
                }
            }
            return Nodes[iter].Domain;
        }
    }
}
