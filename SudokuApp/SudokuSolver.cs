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
            int nrsFound = 1;
            while (nrsFound > 0)//TODO change whilecondition to not solved
            {
                Console.WriteLine("whileLoop starts");
                nrsFound = 0;
                bool containsI;
                for (int currentNr = 1; currentNr <= 9; currentNr++)
                {

                    for (int j = 0; j < 9; j++)
                    {
                        containsI = false;
                        for (int k = 0; k < 9; k++)
                        {
                            if (sudokuInput[j % 3 * 3 + k % 3, j / 3 * 3 + k / 3, 0] == currentNr) { containsI = true; break; }
                        }
                        if (!containsI)
                        {
                            //Containfunction:
                            //Console.WriteLine("quadrant " + (j + 1) + " does not contain " + i + ".");

                            for (int threeIndex = 0; threeIndex < 3; threeIndex++)
                            {
                                for (int nineIndex = 0; nineIndex < 9; nineIndex++)
                                {
                                    if (sudokuInput[nineIndex, j / 3 * 3 + threeIndex, 0] == currentNr)
                                    {
                                        for (int m = 0; m < 3; m++) { sudokuInput[j % 3 * 3 + m, j / 3 * 3 + threeIndex, currentNr] = currentNr; }
                                    }
                                    if (sudokuInput[j % 3 * 3 + threeIndex, nineIndex, 0] == currentNr)
                                    {
                                        for (int n = 0; n < 3; n++) { sudokuInput[j % 3 * 3 + threeIndex, j / 3 * 3 + n, currentNr] = currentNr; }
                                    }
                                }

                            }
                            int zeroCount = 0;
                            int colOfZero = 0;
                            int rowOfZero = 0;
                            List<int> checkSameRow = new List<int>();
                            List<int> checkSameCol = new List<int>();
                            for (int l = 0; l < 9; l++)
                            {
                                if (sudokuInput[j % 3 * 3 + l % 3, j / 3 * 3 + l / 3, currentNr] == 0)
                                {
                                    zeroCount++;
                                    colOfZero = j % 3 * 3 + l % 3;
                                    rowOfZero = j / 3 * 3 + l / 3;
                                    checkSameCol.Add(colOfZero);
                                    checkSameRow.Add(rowOfZero);
                                }
                            }
                            if (zeroCount == 1)
                            {
                                sudokuInput[colOfZero, rowOfZero, 0] = currentNr; sudokuInput[colOfZero, rowOfZero, currentNr] = currentNr; nrsFound++;
                                Console.WriteLine("col: " + (colOfZero + 1) + " / row: " + (rowOfZero + 1));
                            }
                            else if (zeroCount > 1)
                            {
                                bool sameCol = true;
                                foreach (int possibleCol in checkSameCol) { if (possibleCol != colOfZero) { sameCol = false; } }
                                bool sameRow = true;
                                foreach (int possibleCol in checkSameCol) { if (possibleCol != colOfZero) { sameRow = false; } }
                                if (sameCol)
                                {
                                    for (int n = 0; n < 9; n++)
                                    {
                                        if (n < (j / 3 * 3) || n > (j / 3 * 3 + 2)) { sudokuInput[colOfZero, n, currentNr] = currentNr; }
                                    }
                                }
                                if (sameRow)
                                {
                                    for (int n = 0; n < 9; n++)
                                    {
                                        if (n < (j % 3 * 3) || n > (j % 3 * 3 + 2)) { sudokuInput[n, rowOfZero, currentNr] = currentNr; }
                                    }
                                }
                            }
                        }

                    }
                    
                }
                Print(sudokuInput, 0);
            }
        }

        public void Print(int[,,] sud, int layer)
        {
            for (int i = 0; i < 9; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(sud[j, i, layer] + " ");
                    if (j % 3 == 2) { Console.Write("| "); }
                }
                Console.Write("\n");
                if (i % 3 == 2)
                {
                    Console.WriteLine("_________________________");
                    Console.WriteLine();
                }
            }
        }
        public void SolveTest(int[,,] input)
        {

        }
    }
}
