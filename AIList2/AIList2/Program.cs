using System;
using System.Collections.Generic;

namespace AIList2
{
    class Program
    {
        static void Main(string[] args)
        {
            string binary6x6 = "C:/Michal/STUDIA/6sem/SI/L2Etap1/binary-futoshiki_dane_v1.0/binary_6x6";
            string binary8x8 = "C:/Michal/STUDIA/6sem/SI/L2Etap1/binary-futoshiki_dane_v1.0/binary_8x8";
            string binary10x10 = "C:/Michal/STUDIA/6sem/SI/L2Etap1/binary-futoshiki_dane_v1.0/binary_10x10";
            string futoshiki4x4 = "C:/Michal/STUDIA/6sem/SI/L2Etap1/binary-futoshiki_dane_v1.0/futoshiki_4x4";
            string futoshiki5x5 = "C:/Michal/STUDIA/6sem/SI/L2Etap1/binary-futoshiki_dane_v1.0/futoshiki_5x5";
            string futoshiki6x6 = "C:/Michal/STUDIA/6sem/SI/L2Etap1/binary-futoshiki_dane_v1.0/futoshiki_6x6";
            string testFileName = "C:/Michal/STUDIA/6sem/SI/L2Etap1/binary-futoshiki_dane_v1.0/test.txt";

            TimeSpan srednia = TimeSpan.Zero;
            BinaryDataReader BDR1 = new BinaryDataReader(binary8x8);
            FutoshikiDataReader FDR1 = new FutoshikiDataReader(futoshiki6x6);
            int n = 1;
            for (int i = 0; i < n; i++)
            {
                Graph<int> g1 = new Graph<int>(FDR1, -1);
                g1.ReadData();
                DateTime startTime = DateTime.Now;
                //g1.RunBackTracking();
                g1.RunForwardChecking(false, true);
                DateTime stopTime = DateTime.Now;
                TimeSpan roznica = stopTime - startTime;
                srednia += roznica;
            }
            Console.WriteLine("Time: " + srednia / n + "s");

        }
    }
}
