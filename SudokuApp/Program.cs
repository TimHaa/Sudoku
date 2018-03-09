using System;

namespace SudokuApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SudokuSolver s = new SudokuSolver();
            s.Solve();
        }
    }
}
