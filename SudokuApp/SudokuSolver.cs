using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuApp
{
    class SudokuSolver
    {
        public SudokuSolver() { }

        public void SolveLogically(int[,,] sudokuInput)
        {
            bool containsI;
            for (int i = 1; i <= 9; i++)
            {
                Console.Write(i + ":");
                for (int j = 0; j < 9; j++)
                {
                    containsI = false;
                    for (int k = 0; k < 9; k++)
                    {
                        if (sudokuInput[j % 3 * 3 + k % 3, j / 3 * 3 + k / 3, 0] == i) { containsI = true; break; }
                    }
                    if (containsI)
                    {
                        Console.Write(j + 1 + ",");

                        //check zeilen + spalten -> wenn sie enthalten i * zehn in iter dim speichern in verlängerung der spalte
                    }

                }
                Console.WriteLine();
            }
        }
        public void SolveTest(int[,,] input)
        {

        }
    }
}
