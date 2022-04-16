using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIList2
{
    interface IDataReader<T>
    {
        public string FileName { get; set; }
        public IConstraint<T> Constraint { get; set; }
        public List<Node<T>> ReadData();
    }
}
