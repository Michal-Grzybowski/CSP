using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIList2
{
    class BinaryConstraint<T> : IConstraint<T>
    {
        public List<Node<T>> Nodes { get; set; }
        public Boolean isCorrect { get; set; }
        public int Step {get; set;}
        public int A { get; set; }
        public int Iter { get; set; }
        public BinaryConstraint(List<Node<T>> nodes, int a, int iter)
        {
            Nodes = nodes;
            A = a;
            Iter = iter;
            Step = 0;
        }

        public bool checkConditions()
        {
            //Sequence condition
            Step++;
            List<Node<T>> Neighbours = FindNeighbours();
            Node<T> MainNode = Nodes[Iter];
            for (int i = 0; i < Neighbours.Count; i += 2)
            {
                if (MainNode.Value.Equals(Neighbours[i].Value) && MainNode.Value.Equals(Neighbours[i + 1].Value)) return false;
            }
            if (!checkIsItTooLateRows()) return false;
            if (!checkIsItTooLateColumns()) return false;
            if (Iter % A == A - 1)
            {

                //Unique condition
                List<int> uniqueList = new List<int>();
                int number = 0;
                //sprawdzam wiersze
                int tmp = 0;
                for (int i = 0; i <= Iter; i += A)
                {
                    for (int j = i; j < i + A; j++)
                    {
                        if (Nodes[j].Value.Equals(1)) tmp = 1;
                        else tmp = 0;
                        number += tmp * (int)Math.Pow(2, (j - i));
                    }
                    if (uniqueList.Contains(number)) return false;
                    uniqueList.Add(number);
                    number = 0;
                }
                uniqueList.Clear();
                //sprawdzam kolumny
                if (Iter / A == A - 1)
                {
                    for (int i = 0; i + (A * (A - 1)) <= Iter; i++)
                    {
                        int power = 0;
                        for (int j = i; j <= Iter; j += A)
                        {
                            if (Nodes[j].Value.Equals(1)) tmp = 1;
                            else tmp = 0;
                            number += tmp * (int)Math.Pow(2, power);
                            power++;
                        }
                        if (uniqueList.Contains(number)) return false;
                        uniqueList.Add(number);
                        number = 0;
                    }
                }
            }

            return true;
        }
        public bool checkConditionsCutDomain(int Iter)
        {
            throw new NotImplementedException();
        }
        //----------------------------------PRYWATNE FUNKCJE POMOCNICZE--------------------------------------

        private bool checkIsItTooLateRows()
        {
            int counter0 = 0;
            int counter1 = 0;
            for (int i = (Iter / A) * A; i<= (Iter / A) * A + Iter % A; i++)
            {
                if (Nodes[i].Value.Equals(1))
                {
                    counter1++;
                }
                else
                {
                    counter0++;
                }
            }
            if (counter0 > A / 2) return false;
            else if (counter1 > A / 2) return false;
            return true;
        }
        private bool checkIsItTooLateColumns()
        {
            //Same zero and ones
            int counter0 = 0;
            int counter1 = 0;


            for (int i = Iter%A; i < Nodes.Count; i+=A)
            {
                if (Nodes[i].Value.Equals(1))
                {
                    counter1++;
                }
                else if (Nodes[i].Value.Equals(0))
                {
                    counter0++;
                }
            }

            if (counter0 > A / 2) return false;
            else if (counter1 > A / 2) return false;
            return true;
        }



        private List<Node<T>> FindNeighbours()
        {
            List<Node<T>> result = new List<Node<T>>();
            //pierwszy lub drugi wiersz
            if (Iter / A <= 1)
            {
                //sprawdzanie w lewo
                //pierwsza lub druga kolumna
                if (Iter % A >= 2)
                {
                    result.Add(Nodes[Iter - 1]);
                    result.Add(Nodes[Iter - 2]);
                }

            }
            //trzeci lub dalszy wiersz
            else if (Iter / A > 1)
            {
                result.Add(Nodes[Iter - A]);
                result.Add(Nodes[Iter - (2 * A)]);
                if (Iter % A >= 2)
                {
                    result.Add(Nodes[Iter - 1]);
                    result.Add(Nodes[Iter - 2]);
                }
            }
            //sprawdzanie czy Iter jest 2 od brzegu
            if (Iter % A == 1 || Iter % A == A - 2)
            {
                result.Add(Nodes[Iter - 1]);
                result.Add(Nodes[Iter + 1]);
            }
            //sprawdzanie czy ma dwoch sasiadow z prawej
            if (Iter % A < A - 2)
            {
                result.Add(Nodes[Iter + 1]);
                result.Add(Nodes[Iter + 2]);
            }
            //sprawdzanie czy ma dwoch sasiadow z dołu
            if(Iter/A < A - 2)
            {
                result.Add(Nodes[Iter + A]);
                result.Add(Nodes[Iter + 2*A]);
            }
            if(Iter/A <= A - 2 && Iter / A >= 1)
            {
                result.Add(Nodes[Iter - A]);
                result.Add(Nodes[Iter + A]);
            }

            return result;
        }

        public List<T> ValueQueue(int iter)
        {
            return Nodes[iter].Domain;
        }
    }
}
