using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIList2
{
    class Node<T>
    {
        public List<T> Domain { get; set; }
        public T Value { get; set; }
        public bool IsDefault { get; set; }
        public Node(T value, bool isDefault)
        {
            Value = value;
            IsDefault = isDefault;
        }
    }
}
