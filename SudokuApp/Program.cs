using System;

namespace SudokuApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SudokuSolver s = new SudokuSolver();
            s.Solve();
            
            //int[,,] sud = new int[9, 9, 10];

            ////generate random numbers
            //Random rnd = new Random();
            //for (int m = 0; m <= 8; m++)
            //{
            //    for (int n = 0; n <= 8; n++)
            //    {
            //        sud[n, m, 0] = rnd.Next(1, 10);
            //    }
            //}
            //s.Print(sud, 0);
            //s.SolveLogically(sud);
            Console.ReadLine();
        }
    }
}
