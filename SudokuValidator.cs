//============================================================================
// Name        : SudokuValidator.cs
// Author      : Jeff Hancock
//               https://www.linkedin.com/in/jeffreythancock/
// Version     :
// Copyright   : Carte blanche.  Plagiarize at will.
// Description : A solution to a programming problem asked in a phone interview.
//               In the interview, I was asked to verbally describe how I would
//               solve the problem of validating a potential solution to a
//               Sudoku puzzle.  As an exercise, I later implemented it in C++,
//               then translated to C#.
//============================================================================

using System;
using System.Collections.Generic;

namespace SudokuValidator
{
    class SudokuValidator
    {
        // Check the rows of the puzzle to make sure each entry in the row is both
        // unique and a valid value (int 1 - 9 only).
        // Return value:
        //    true - The rows are OK
        //    false - The rows are NOT OK.  There is either an invalid value in one
        //            or more rows, or a duplicate within one or more rows.
        static bool AreRowsOk(int [,] puzzle)
        {
            // Check rows for valid and non-repeating values.  The non-repeating
            // aspect will be done by using a set and adding the values to the
            // set, checking for a return code indicating the value to be added
            // already exists.
            HashSet<int> values = new HashSet<int>();
            bool success;
            for (int row = 0; row < 9; row++)
            {
                // Check the row to make sure it doesn't have any duplicates
                values.Clear();
                for (int col = 0; col < 9; col++) {
                    int value = puzzle[row, col];
                    if (value < 1 || value > 9) {
                        Console.WriteLine("Row " + row + " has an invalid value: " + value);
                        return false;
                    }

                    success = values.Add(value);
                    if (!success) { // If insert not successful due to value already being in set
                        Console.WriteLine("Row " + row + " has a repeat value: " + value);
                        return false;
                    }
                }
                Console.WriteLine("Row " + row + " is OK.");
            }
            return true;
        }

        // Check the columns of the puzzle to make sure each entry in the row is unique.
        // Return value:
        //    true - The columns are OK
        //    false - The columns are NOT OK.  There is a duplicate within one or more columns.
        static bool AreColumnsOk(int [,] puzzle) {
            // Check columns for valid and non-repeating values
            HashSet<int> values = new HashSet<int>();
            bool success;
            for (int col = 0; col< 9; col++) {
                // Check the col to make sure it doesn't have any duplicates
                values.Clear();
                for (int row = 0; row< 9; row++) {
                    int value = puzzle[row, col];
                    // Don't need to look for invalid values, since we have checked them all when checking rows,
                    // and  all values are in rows!
                    success = values.Add(value);
                        if (!success) { // If insert not successful due to value already being in set
                        Console.WriteLine("Col " + col + " has a repeat value: " + value);
                        return false;
                    }
                }
                Console.WriteLine("Col " + col + " is OK.");
            }
            return true;
        }

        // Check the specified submatrix of the puzzle to make sure each entry in the submatrix is unique.
        // Return value:
        //    true - The submatrix is OK
        //    false - The submatrix are NOT OK.  There is a duplicate within the submatrix.
        static bool IsSubmatrixOk(int [,] puzzle, int startRowIdx, int stopRowIdx, int startColIdx, int stopColIdx) {
            HashSet<int> values = new HashSet<int>();
            bool success;
            values.Clear(); // Probably isn't necessary
            for (int row = startRowIdx; row <= stopRowIdx; row++) {
                for (int col = startColIdx; col <= stopColIdx; col++) {
                    int value = puzzle[row, col];
                    success = values.Add(value);
                    if (!success) {
                        Console.WriteLine("Submatrix with starting row " + startRowIdx + 
                            " and starting column " + startColIdx +  " has a repeat value: " + value);
                       return false;
                    }
                }
            }
            Console.WriteLine("Submatrix with starting row " + startRowIdx + " and starting column " + startColIdx + " is OK.");
            return true;
        }

        // Check the submatrices of the puzzle to make sure each entry in each submatrix is unique.
        // Return value:
        //    true - The submatrices are OK
        //    false - The submatrices are NOT OK.  There is a duplicate within one or more submatrix.
        static bool AreSubmatricesOk(int [,] puzzle) {
            // Check sub-matrices (the 9 3x3 squares)
            for (int startRow = 0; startRow< 7; startRow += 3) {
                for (int startCol = 0; startCol< 7; startCol += 3) {
                    if (!IsSubmatrixOk(puzzle, startRow, startRow + 2, startCol, startCol + 2))
                        return false;
                }
            }
            return true;
        }

        // Check the given puzzle to see if it is a valid solution.
        // Return value:
        //    true - The puzzle contains a valid solution.
        //    false - The puzzle does NOT contain a valid solution.
        static bool IsSolutionOk(int [,] puzzle) {
            if (!AreRowsOk(puzzle))
                return false;
            if (!AreColumnsOk(puzzle))
                return false;
            return AreSubmatricesOk(puzzle);
        }

        static void Main(string[] args) {
            // A solution to be tested that passes the row and column
            // tests but not the submatrices test.
            int [,] puzzle = { { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                               { 2, 3, 4, 5, 6, 7, 8, 9, 1 },
                               { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
                               { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
                               { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
                               { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
                               { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
                               { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
                               { 9, 1, 2, 3, 4, 5, 6, 7, 8 } };

            // A supposedly solved one from google images, but not.
            int [,] puzzle2 = { { 2, 9, 5, 7, 4, 3, 8, 6, 1 },
                                { 4, 3, 1, 8, 6, 5, 9, 2, 7 },
                                { 8, 7, 6, 1, 9, 2, 5, 4, 3 },
                                { 3, 8, 7, 4, 5, 9, 2, 1, 6 },
                                { 6, 1, 2, 3, 8, 7, 4, 9, 5 },
                                { 5, 4, 9, 2, 1, 6, 7, 3, 8 },
                                { 7, 6, 3, 5, 3, 4, 1, 8, 9 },
                                { 9, 2, 8, 6, 7, 1, 3, 5, 4 },
                                { 1, 5, 4, 9, 3, 8, 6, 7, 2 } };

            // A truly solved one, also from google images.
            int [,] puzzle3 = { { 8, 2, 7, 1, 5, 4, 3, 9, 6 },
                                { 9, 6, 5, 3, 2, 7, 1, 4, 8 },
                                { 3, 4, 1, 6, 8, 9, 7, 5, 2 },
                                { 5, 9, 3, 4, 6, 8, 2, 7, 1 },
                                { 4, 7, 2, 5, 1, 3, 6, 8, 9 },
                                { 6, 1, 8, 9, 7, 2, 4, 3, 5 },
                                { 7, 8, 6, 2, 3, 5, 9, 1, 4 },
                                { 1, 5, 4, 7, 9, 6, 8, 2, 3 },
                                { 2, 3, 9, 8, 4, 1, 5, 6, 7 } };

            Console.WriteLine("Are rows OK for puzzle? " + AreRowsOk(puzzle));

            Console.WriteLine("Solution to puzzle is " + (IsSolutionOk(puzzle) ? "" : "NOT ") + "OK.");

            Console.WriteLine("Solution to puzzle2 is " + (IsSolutionOk(puzzle2) ? "" : "NOT ") + "OK.");

            Console.WriteLine("Solution to puzzle3 is " + (IsSolutionOk(puzzle3) ? "" : "NOT ") + "OK.");

            Console.Read(); // Keep console up until the Enter key is pressed (so we can see it).
        }
    }
}
