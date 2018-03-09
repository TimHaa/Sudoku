using System;

namespace SudokuApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            SudokuSolver s = new SudokuSolver();
            s.Solve();


            //string[,] sarr = new string[9, 9];
            //sarr[3, 2] = "13479";
            //for (int i = 1; i < 10; i++)
            //{
            //    if (!sarr[3,2].Contains(i.ToString())) { Console.WriteLine(i); }
            //}

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
