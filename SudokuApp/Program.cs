using System;

namespace SudokuApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SudokuSolver s = new SudokuSolver();

            int[,,] sud = new int[9, 9, 9];

            //generate random numbers
            Random rnd = new Random();
            for (int m = 0; m <= 8; m++)
            {
                for (int n = 0; n <= 8; n++)
                {
                    sud[n, m, 0] = rnd.Next(1, 10);
                }
            }

            //print sudoku
            for (int i = 0; i < 9; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(sud[j, i, 0] + " ");

                    if (j % 3 == 2) { Console.Write("| "); }

                }
                Console.Write("\n");
                if (i % 3 == 2)
                {
                    Console.WriteLine("_________________________");
                    Console.WriteLine();
                }


            }

            s.SolveLogically(sud);  //function: shows which number appears in which quadrant(1 - 9)

            //Console.Write("\n");
            //for (int i = 0; i < 9; i++)
            //{
            //    for (int j = 0; j < 9; j++)
            //    {
            //        Console.Write(sud[i, j, 8]+ " ");
            //    }
            //    Console.Write("\n");

            //}

            Console.ReadLine();
        }
    }
}
