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
            int attempts = 4;
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
                        //If not containing then save information about possible position in the layer of said number
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
                                Fill(sudoku, currentNr, colOfZero, rowOfZero);
                                attempts = 4;
                                Console.WriteLine("col: " + (colOfZero + 1) + " / row: " + (rowOfZero + 1));
                            }
                            else if (zeroCount > 1)//TODO not tested yet
                            {
                                bool sameCol = true;
                                foreach (int possibleCol in checkSameCol) { if (possibleCol != colOfZero) { sameCol = false; } }
                                if (sameCol)
                                {
                                    for (int n = 0; n < 9; n++)
                                    {
                                        if (n < quadStartRow || n > (quadStartRow + 2)) { sudoku[colOfZero, n, currentNr] = currentNr; }
                                    }
                                }
                                bool sameRow = true;
                                foreach (int possibleRow in checkSameRow) { if (possibleRow != colOfZero) { sameRow = false; } }
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
                if (attempts <= 2)
                {
                    CombinationByNr(sudoku);
                    CombinationByCell(sudoku);//unintuitive attemptscounter
                    ExclusionPrinciple(sudoku);
                }
            }
            
            for (int k = 0; k < 10; k++)
            {
                Print(sudoku, k);
            }
        }

        public void ExclusionPrinciple(int[,,] sudoku)//check every cell if only 1 number is possible. If yes fill in.
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (sudoku[col, row, 0] == 0)
                    {
                        int candidate = 0;
                        int candidateNr = 0;
                        for (int nineIndex = 1; nineIndex < 10; nineIndex++)
                        {
                            if (sudoku[col, row, nineIndex] == 0) { candidateNr++; candidate = nineIndex; }
                        }
                        if(candidateNr == 1)
                        {
                            sudoku[col, row, 0] = candidate;
                            Fill(sudoku, candidate, col, row);
                        }
                    }
                }
            }
        }

        public void CombinationByNr(int[,,] sudoku)//TODO get working as intended!
        {//check for every number which possible coordinates it has (per row, col, quad). If it shares its coordinate these numbers belong to the coordinates
            for (int k = 0; k < 10; k++)
            {
                Print(sudoku, k);
            }
            for (int i = 0; i < 9; i++) 
            {
                string[] cellsPerRow = new string[9];
                
                string[] cellsPerCol = new string[9];
                
                string[] cellsPerQuad = new string[9];
                
                int quadStartCol = i % 3 * 3;
                int quadStartRow = i / 3 * 3;
                for (int nr = 0; nr < 9; nr++)
                {
                    string nrRow = "";
                    string nrCol = "";
                    string nrQuad = "";
                    for (int j = 0; j < 9; j++)
                    {
                        if (sudoku[j, i, nr + 1] == 0) { nrRow += j.ToString(); }//rows
                        if (sudoku[i, j, nr + 1] == 0) { nrCol += j.ToString(); }//cols
                        
                        if (sudoku[quadStartCol + j % 3, quadStartRow + j / 3, nr + 1] == 0) { nrQuad += j.ToString(); }//quadrants
                    }
                    cellsPerRow[nr] = nrRow;
                    cellsPerCol[nr] = nrCol;
                    cellsPerQuad[nr] = nrQuad;
                }
                Console.Write("Row " + i + ": ");
                for (int k = 0; k < 9; k++)//test
                {
                    Console.Write(cellsPerRow[k] + ",");
                }

                Console.Write("Col " + i + ": ");
                for (int k = 0; k < 9; k++)//test
                {
                    Console.Write(cellsPerCol[k] + ",");
                }

                Console.Write("Quad " + i + ": ");
                for (int k = 0; k < 9; k++)//test
                {
                    Console.Write(cellsPerQuad[k] + ",");
                }

                Console.WriteLine();
                for (int currNr = 0; currNr < 9; currNr++)
                {
                    int rowCount = 0;
                    int colCount = 0;
                    int quadCount = 0;
                    int zeroCountRow = 0;
                    int zeroCountCol = 0;
                    int zeroCountQuad = 0;
                    for (int pos = 0; pos < 9; pos++)
                    {
                        bool containsRow = cellsPerRow[pos] != String.Empty;
                        if (!containsRow) { zeroCountRow++; }
                        for (int x = 0; x < cellsPerRow[pos].Length; x++)
                        {
                            if (!(cellsPerRow[currNr].Contains(cellsPerRow[pos][x].ToString()))) { containsRow = false; }
                        }
                        if (containsRow) { rowCount++; }
                        

                        bool containsCol = cellsPerCol[pos] != String.Empty;
                        if (!containsCol) { zeroCountCol++;}
                        for (int x = 0; x < cellsPerCol[pos].Length; x++)
                        {
                            if (!(cellsPerCol[currNr].Contains(cellsPerCol[pos][x].ToString()))) { containsCol = false; }
                        }
                        if (containsCol) { colCount++; }
                        

                        bool containsQuad = cellsPerQuad[pos] != String.Empty;
                        if (!containsQuad) { zeroCountQuad++; }
                        for (int x = 0; x < cellsPerQuad[pos].Length; x++)
                        {
                            if (!(cellsPerQuad[currNr].Contains(cellsPerQuad[pos][x].ToString()))) { containsQuad = false; }
                        }
                        if (containsQuad) { quadCount++; }
                        
                        //if(cellsPerCol[currNr] != null && cellsPerCol[currNr] == cellsPerCol[pos]) { colCount++; }
                        //if(cellsPerQuad[currNr] != null && cellsPerQuad[currNr] == cellsPerQuad[pos]) { quadCount++; }
                    }
                    if (cellsPerRow[currNr] != String.Empty && rowCount == cellsPerRow[currNr].Length && rowCount != (9 - zeroCountRow))//rows
                    {
                        Console.WriteLine(cellsPerRow[currNr] + " row");
                        for (int nr = 1; nr < 10; nr++)
                        {
                            //unnessecary because its the condition

                            //if (cellsPerRow[nr - 1] == cellsPerRow[currNr])
                            //{
                            //    for (int col = 0; col < 9; col++)
                            //    {
                            //        if (!(cellsPerRow[currNr].Contains(col.ToString()))) { sudoku[col, i, nr] = nr; }
                            //    }
                            //}
                            //else if


                            //if (cellsPerRow[nr - 1] != cellsPerRow[currNr])
                            //{
                            //    for (int col = 0; col < 9; col++)
                            //    {
                            //        if (cellsPerRow[currNr].Contains(col.ToString())) { sudoku[col, i, nr] = nr; }
                            //    }
                            //}

                            bool containsRow = true;
                            for (int x = 0; x < cellsPerRow[nr - 1].Length; x++)
                            {
                                if (!(cellsPerRow[currNr].Contains(cellsPerRow[nr - 1][x].ToString()))) { containsRow = false; }
                            }
                            if (!containsRow)
                            {
                                for (int j = 0; j < 9; j++)
                                {
                                    if (cellsPerRow[currNr].Contains(j.ToString())) { sudoku[j, i, nr] = nr; Console.WriteLine(i +" " + j + " " + nr); }
                                }
                            }
                        }
                        for (int k = 0; k < 10; k++)
                        {
                            Print(sudoku, k);
                        }
                    }//if changed
                    if (cellsPerCol[currNr] != String.Empty && colCount == cellsPerCol[currNr].Length && colCount != (9 - zeroCountCol))//cols
                    {
                        Console.WriteLine(cellsPerCol[currNr] + " col");
                        for (int nr = 1; nr < 10; nr++)
                        {
                            //if (cellsPerCol[nr - 1] == cellsPerCol[currNr])
                            //{
                            //    for (int row = 0; row < 9; row++)
                            //    {
                            //        if (!(cellsPerCol[currNr].Contains(row.ToString()))) { sudoku[i, row, nr] = nr; }
                            //    }
                            //}
                            //else 


                            //if (cellsPerCol[nr - 1] != cellsPerCol[currNr])
                            //{
                            //    for (int row = 0; row < 9; row++)
                            //    {
                            //        if (cellsPerCol[currNr].Contains(row.ToString())) { sudoku[i, row, nr] = nr; }
                            //    }
                            //}

                            bool containsCol = true;
                            for (int x = 0; x < cellsPerCol[nr - 1].Length; x++)
                            {
                                if (!(cellsPerCol[currNr].Contains(cellsPerCol[nr - 1][x].ToString()))) { containsCol = false; }
                            }
                            if (!containsCol)
                            {
                                for (int j = 0; j < 9; j++)
                                {
                                    if (cellsPerCol[currNr].Contains(j.ToString())) { sudoku[i, j, nr] = nr; Console.WriteLine(i + " " + j + " " + nr); }
                                }
                            }
                        }
                        for (int k = 0; k < 10; k++)
                        {
                            Print(sudoku, k);
                        }
                    }
                    if (cellsPerQuad[currNr] != String.Empty && quadCount == cellsPerQuad[currNr].Length && quadCount != (9- zeroCountQuad))//quadrants
                    {
                        Console.WriteLine(cellsPerQuad[currNr] + " quad");
                        for (int nr = 1; nr < 10; nr++)
                        {

                            //if (cellsPerQuad[nr - 1] == cellsPerQuad[currNr])
                            //{
                            //    for (int j = 0; j < 9; j++)
                            //    {
                            //        if (!(cellsPerQuad[currNr].Contains(j.ToString()))) { sudoku[quadStartCol + j % 3, quadStartRow + j / 3, nr] = nr; }
                            //    }
                            //}
                            //else 


                            //if (cellsPerQuad[nr - 1] != cellsPerQuad[currNr])
                            //{
                            //    for (int j = 0; j < 9; j++)
                            //    {
                            //        if (cellsPerQuad[currNr].Contains(j.ToString())) { sudoku[quadStartCol + j % 3, quadStartRow + j / 3, nr] = nr; }
                            //    }
                            //}

                            bool containsQuad = true;
                            for (int x = 0; x < cellsPerQuad[nr-1].Length; x++)
                            {
                                if (!(cellsPerQuad[currNr].Contains(cellsPerQuad[nr-1][x].ToString()))) { containsQuad = false; }
                            }
                            if (!containsQuad)
                            {
                                for (int j = 0; j < 9; j++)
                                {
                                    if (cellsPerQuad[currNr].Contains(j.ToString())) { sudoku[quadStartCol + j % 3, quadStartRow + j / 3, nr] = nr; Console.WriteLine((quadStartCol + j % 3) + " " + (quadStartRow + j / 3) + " " + nr); }
                                }
                            }
                        }
                        for (int k = 0; k < 10; k++)
                        {
                        Print(sudoku, k);
                        }
                    }
                }
            }
            
        }

        public void CombinationByCell(int[,,] sudoku)
        {
            string[,] nrsPerCell = new string[9,9];
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if(sudoku[col, row, 0] == 0)
                    {
                        for (int nineIndex = 1; nineIndex < 10; nineIndex++)
                        {
                            if(sudoku[col, row, nineIndex] == 0) { nrsPerCell[col, row] += nineIndex; }
                        }
                    }
                }
            }
            //check rows, columns and quadrants if they contain multiple cells with same possible numbers
            for (int row = 0; row < 9; row++)//rows
            {
                for (int cell = 0; cell < 9; cell++)
                {
                    if (nrsPerCell[cell, row] == null) { continue; }
                    int combCount = 0;
                    List<int> combCoordsCol = new List<int>();
                    for (int col = cell; col < 9; col++)
                    {
                        if ( nrsPerCell[cell, row] == nrsPerCell[col, row] ) { combCount++; combCoordsCol.Add(col); }
                    }
                    if ( combCount == nrsPerCell[cell, row].Length)
                    {
                        //make nrsPerCell only and solely possible for combCoord's
                        for (int nr = 1; nr < 10; nr++)
                        {
                            if (!nrsPerCell[cell, row].Contains(nr.ToString())) {  foreach (int coord in combCoordsCol) { sudoku[coord, row, nr] = nr; }}
                            else if(nrsPerCell[cell, row].Contains(nr.ToString())){ for (int c = 0; c < 9; c++) { if (!combCoordsCol.Contains(c)) { sudoku[c, row, nr] = nr; } } }
                        }
                    }
                }
            }
            for (int col = 0; col < 9; col++)//cols
            {
                for (int cell = 0; cell < 9; cell++)
                {
                    if (nrsPerCell[col, cell] == null) { continue; }
                    int combCount = 0;
                    List<int> combCoordsRow = new List<int>();
                    for (int row = cell; row < 9; row++)
                    {
                        if (nrsPerCell[col, cell] == nrsPerCell[col, row]) { combCount++; combCoordsRow.Add(row); }
                    }
                    if (combCount == nrsPerCell[col, cell].Length)
                    {
                        for (int nr = 1; nr < 10; nr++)
                        {
                            if (!(nrsPerCell[col, cell].Contains(nr.ToString()))) { foreach (int coord in combCoordsRow) { sudoku[col, coord, nr] = nr; } }
                            else if (nrsPerCell[col, cell].Contains(nr.ToString())) { for (int r = 0; r < 9; r++) { if (!combCoordsRow.Contains(r)) { sudoku[col, r, nr] = nr; } } }
                        }
                    }
                }
            }
            for (int quad = 0; quad < 9; quad++)//quadrants
            {
                int quadStartCol = quad % 3 * 3;
                int quadStartRow = quad / 3 * 3;
                for (int cell = 0; cell < 9; cell++)
                {
                    if (nrsPerCell[quadStartCol + cell % 3, quadStartRow + cell / 3] == null) { continue; }
                    int combCount = 0;
                    List<int> combCoordsQuad = new List<int>();
                    for (int j = cell; j < 9; j++)
                    {
                        if (nrsPerCell[quadStartCol + j % 3, quadStartRow + j / 3] == nrsPerCell[quadStartCol + cell % 3, quadStartRow + cell / 3])
                        {
                            combCount++;
                            combCoordsQuad.Add(j);
                        }
                    }
                    if (combCount == nrsPerCell[quadStartCol + cell % 3, quadStartRow + cell / 3].Length)
                    {
                        for (int nr = 1; nr < 10; nr++)
                        {
                            if (!(nrsPerCell[quadStartCol + cell % 3, quadStartRow + cell / 3].Contains(nr.ToString())))
                            {
                                foreach (int coord in combCoordsQuad) { sudoku[quadStartCol + coord % 3, quadStartRow + coord / 3, nr] = nr; }
                            }
                            else if (nrsPerCell[quadStartCol + cell % 3, quadStartRow + cell / 3].Contains(nr.ToString()))
                            {
                                for (int coord = 0; coord < 9; coord++) { if (!combCoordsQuad.Contains(coord)) { sudoku[quadStartCol + coord % 3, quadStartRow + coord / 3, nr] = nr; } }
                            }
                        }
                    }
                }
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

        public void Print2d(string[,] sud)//just for testing
        {
            for (int i = 0; i < 9; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(sud[j, i] + " ");
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
            int[,,] sudoku = new int[9, 9, 10];
            string lineInput;
            Console.WriteLine("Input the Sudoku, line by line. No punctuation or spaces allowed, 0's for empty!");
            
            for (int row = 0; row < 9; row++)
            {
                lineInput = Console.ReadLine();
                for (int col = 0; col < 9; col++)
                {
                    int inputNr = Convert.ToInt16(lineInput[col] - '0');
                    sudoku[col, row, 0] = inputNr;
                    if (inputNr != 0)
                    {
                        Fill(sudoku, inputNr, col, row);
                    }
                }
            }
            return sudoku;
        }

        private void Fill(int[,,] sudoku, int nrFound, int col, int row)
        {
            for (int pos = 0; pos < 9; pos++)
            {
                sudoku[col, row, pos + 1] = pos + 1;
                sudoku[(col / 3) * 3 + (pos % 3), (row / 3) * 3 + (pos / 3), nrFound] = nrFound;
                sudoku[pos, row, nrFound] = nrFound;
                sudoku[col, pos, nrFound] = nrFound;
            }
        }

        public void SolveTest(int[,,] input)
        {
            
            
        }
    }
}
