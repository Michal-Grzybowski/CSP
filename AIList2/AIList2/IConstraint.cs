using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIList2
{
    interface IConstraint<T>
    {
        public List<Node<T>> Nodes { get; set; }
        public Boolean isCorrect { get; set; }
        public int Step {get; set;}
        public int Iter { get; set; }
        public bool checkConditions();
        public bool checkConditionsCutDomain(int iter);
        public List<T> ValueQueue(int iter);
    }
}
