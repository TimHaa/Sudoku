using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuApp
{
    class SudokuSolver
    {
        public SudokuSolver() { }
        
        public void Solve()
        {
            //TODO for every zero check which nrs are possible and add them to a list as 1 element. If the nr of these elements == element.count delete the possibilities from every other layer.
            int[,,] sudoku = Generate();
            int attempts = 6;
            //int missingNrs = 0;
            //for (int row = 0; row < 9; row++)
            //{
            //    for (int col = 0; col < 9; col++)
            //    {
            //        if (sudoku[col,row, 0] == 0) { missingNrs++; }
            //    }
            //}
            while (attempts > 0)
            {
                Console.WriteLine("whileLoop starts");
                //foreach number 1 to 9 iterate through the 9 quadrants and check if they contain the number
                for (int currentNr = 1; currentNr <= 9; currentNr++)
                {
                    bool containsCurrNr;
                    for (int quadrant = 0; quadrant <= 8; quadrant++)
                    {
                        containsCurrNr = false;
                        int quadStartCol = quadrant % 3 * 3;
                        int quadStartRow = quadrant / 3 * 3;
                        for (int iterateQuad = 0; iterateQuad < 9; iterateQuad++)
                        {
                            if (sudoku[quadStartCol + iterateQuad % 3, quadStartRow + iterateQuad / 3, 0] == currentNr) { containsCurrNr = true; break; }
                        }
                        //If not containing save information about possible position in the layer of said number
                        if (!containsCurrNr)
                        {
                            for (int threeIndex = 0; threeIndex < 3; threeIndex++)
                            {
                                for (int nineIndex = 0; nineIndex < 9; nineIndex++)
                                {
                                    if (sudoku[nineIndex, quadStartRow + threeIndex, 0] == currentNr)//TODO maybe optimize so it doesnt consider the current quadrant
                                    {
                                        for (int m = 0; m < 3; m++) { sudoku[quadStartCol + m, quadStartRow + threeIndex, currentNr] = currentNr; }
                                    }
                                    if (sudoku[quadStartCol + threeIndex, nineIndex, 0] == currentNr)
                                    {
                                        for (int n = 0; n < 3; n++) { sudoku[quadStartCol + threeIndex, quadStartRow + n, currentNr] = currentNr; }
                                    }
                                }
                            }
                            //check this numbers layer for single zeros in quadrants, these represent solutions
                            //Also store info using rows/cols of zeros to find solutions in next run
                            int zeroCount = 0;
                            int colOfZero = 0;
                            int rowOfZero = 0;
                            List<int> checkSameRow = new List<int>();
                            List<int> checkSameCol = new List<int>();
                            for (int l = 0; l < 9; l++)
                            {
                                if (sudoku[quadStartCol + l % 3, quadStartRow + l / 3, currentNr] == 0)
                                {
                                    zeroCount++;
                                    colOfZero = quadStartCol + l % 3;
                                    rowOfZero = quadStartRow + l / 3;
                                    checkSameCol.Add(colOfZero);
                                    checkSameRow.Add(rowOfZero);
                                }
                            }
                            if (zeroCount == 1)
                            {
                                sudoku[colOfZero, rowOfZero, 0] = currentNr;
                                for (int layer = 1; layer < 10; layer++)
                                {
                                    sudoku[colOfZero, rowOfZero, layer] = layer;
                                }
                                attempts = 6;
                                Console.WriteLine("col: " + (colOfZero + 1) + " / row: " + (rowOfZero + 1));
                            }
                            else if (zeroCount > 1)//TODO not tested yet
                            {
                                bool sameCol = true;
                                foreach (int possibleCol in checkSameCol) { if (possibleCol != colOfZero) { sameCol = false; } }
                                bool sameRow = true;
                                foreach (int possibleRow in checkSameRow) { if (possibleRow != colOfZero) { sameRow = false; } }
                                if (sameCol)
                                {
                                    for (int n = 0; n < 9; n++)
                                    {
                                        if (n < quadStartRow || n > (quadStartRow + 2)) { sudoku[colOfZero, n, currentNr] = currentNr; }
                                    }
                                }
                                if (sameRow)
                                {
                                    for (int n = 0; n < 9; n++)
                                    {
                                        if (n < quadStartCol || n > (quadStartCol + 2)) { sudoku[n, rowOfZero, currentNr] = currentNr; }
                                    }
                                }
                            }
                        }
                    }
                }
                attempts--;
            }
            for (int k = 0; k < 10; k++)
            {
                Print(sudoku, k);
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

        public int[,,] Generate()
        {
            int[,,] sud = new int[9, 9, 10];
            string lineInput;
            Console.WriteLine("Input the Sudoku, line by line. No punctuation or spaces allowed, 0's for empty!");
            for (int row = 0; row < 9; row++)
            {
                lineInput = Console.ReadLine();
                for (int col = 0; col < 9; col++)
                {
                    sud[col, row, 0] = Convert.ToInt16(lineInput[col]-'0');
                    if (sud[col, row, 0] != 0)
                    {
                        for(int layer = 1; layer < 10; layer++)
                        {
                            sud[col, row, layer] = layer;
                        }
                    }
                }
            }
            return sud;
        }

        public void SolveTest(int[,,] input)
        {
            
            
        }
    }
}
