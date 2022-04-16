using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIList2
{
    class BinaryDataReader : IDataReader<int>
    {
        public string FileName { get; set; }
        public IConstraint<int> Constraint { get; set; }
        public BinaryDataReader(string fileName)
        {
            FileName = fileName;
        }

        public List<Node<int>> ReadData()
        {
            List<Node<int>> nodes = new List<Node<int>>();
            string[] lines = System.IO.File.ReadAllLines(FileName);
            foreach (string line in lines)
            {
                foreach (char letter in line)
                {
                    if(letter == 'x')
                    {
                        nodes.Add(new Node<int>(-1, false));
                        nodes.Last().Domain = new List<int> { 0, 1 };
                    }
                    else if (letter == '0')
                    {
                        nodes.Add(new Node<int>(0, true));
                        nodes.Last().Domain = new List<int> { 0 };
                    }
                    else if (letter == '1')
                    {
                        nodes.Add(new Node<int>(1, true));
                        nodes.Last().Domain = new List<int> { 1 };
                    }
                }
            }
            Constraint = new BinaryConstraint<int>(nodes, (int)Math.Sqrt(nodes.Count), 0);
            return nodes;
        }


    }
}
