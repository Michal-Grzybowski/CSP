using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIList2
{
    class FutoshikiDataReader : IDataReader<int>
    {
        public string FileName { get; set; }
        public IConstraint<int> Constraint { get; set; }
        public List<(int,int)> OperatorList { get; set; }

        public FutoshikiDataReader(string fileName)
        {
            FileName = fileName;
            OperatorList = new List<(int, int)>();
        }

        public List<Node<int>> ReadData()
        {
            List<Node<int>> nodes = new List<Node<int>>();
            string[] lines = System.IO.File.ReadAllLines(FileName);
            int A = lines[0].Count()/2+1;
            for(int i=0; i<lines.Count(); i++)
            {
                for (int j = 0; j < lines[i].Count(); j++)
                {
                    if (i % 2 == 0) // value line
                    {
                        if (lines[i][j] != '-')
                        {
                            if (lines[i][j] == 'x')
                            {
                                Node<int> newNode = new Node<int>(-1, false);
                                List<int> dom = new List<int>();
                                for (int k = 1; k <= A; k++)
                                {
                                    dom.Add(k);
                                }
                                newNode.Domain = dom;
                                nodes.Add(newNode);
                            }
                            else if (lines[i][j] == '<')
                            {
                                OperatorList.Add((nodes.Count - 1, nodes.Count));
                            }
                            else if (lines[i][j] == '>')
                            {
                                OperatorList.Add((nodes.Count, nodes.Count - 1));
                            }
                            else
                            {
                                int value = lines[i][j] - 48;
                                Node<int> newNode = new Node<int>(value, true);
                                newNode.Domain = new List<int> { value };
                                nodes.Add(newNode);
                            }
                        }
                    }
                    else //constraint line
                    {
                        if (lines[i][j] != '-')
                        {
                            if (lines[i][j] == '<')
                            {
                                OperatorList.Add((nodes.Count - A + j, nodes.Count + j));
                            }
                            else if (lines[i][j] == '>')
                            {
                                OperatorList.Add((nodes.Count + j, nodes.Count + j - A));
                            }
                        }
                    }
                }
            }
            foreach ((int, int) elem in OperatorList)
            {
                (int smaller,int larger) = elem;
                nodes[smaller].Domain.Remove(A);
                nodes[larger].Domain.Remove(1);
            }
            Constraint = new FutoshikiConstraint(nodes, (int)Math.Sqrt(nodes.Count), 0, OperatorList);
            return nodes;
        }
    }
}
