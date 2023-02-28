using System;
using System.Diagnostics;

namespace Solution
{
    public class Solution
    {
        static readonly int BOARD_SIZE = 4;
        public static void main(string[] args)
        {
            // you can write to stdout for debugging purposes, e.g.
            char[,] tester = new char[,]{{'a','b','c','d'},
                                         {'b','o','a','t'},
                                         {'d','m','r','w'},
                                         {'d','m','t','g'}};
            solveWordament(tester);
            Debug.WriteLine("hello");
        }

        public static void solveWordament(char[,] board)
        {
            bool[,] visited = new bool[BOARD_SIZE, BOARD_SIZE];
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    solveWordament(board, visited, i, j, "");
                }
            }
        }

        public static void solveWordament(char[,] board, bool[,] visited, int row, int col, string prev)
        {
            if (!validCall(visited, row, col))
            {
                return;
            }
            prev = prev + board[row, col];
            visited[row, col] = true;
            if (isValidWord(prev))
            {
                Debug.WriteLine(prev);
            }
            if (isBegWord(prev))
            {
                solveWordament(board, visited, row, col + 1, prev);
                solveWordament(board, visited, row, col - 1, prev);
                solveWordament(board, visited, row + 1, col, prev);
                solveWordament(board, visited, row - 1, col, prev);
            }
            visited[row, col] = false;
        }

        public static bool validCall(bool[,] visited, int row, int col)
        {
            if (row < 0 || row > visited.GetLength(0) - 1)
            {
                return false;
            }
            if (col < 0 || col > visited.GetLength(1) - 1)
            {
                return false;
            }
            return !visited[row, col];
        }

        public static bool isValidWord(string input)
        {
            if (input.Equals("boat"))
            {
                return true;
            }
            if (input.Equals("moat"))
            {
                return true;
            }
            if (input.Equals("cart"))
            {
                return true;
            }
            return false;
        }

        public static bool isBegWord(string input)
        {
            if (input.Equals("b"))
            {
                return true;
            }
            if (input.Equals("bo"))
            {
                return true;
            }
            if (input.Equals("boa"))
            {
                return true;
            }
            if (input.Equals("m"))
            {
                return true;
            }
            if (input.Equals("mo"))
            {
                return true;
            }
            if (input.Equals("moa"))
            {
                return true;
            }
            if (input.Equals("c"))
            {
                return true;
            }
            if (input.Equals("ca"))
            {
                return true;

            }
            if (input.Equals("car"))
            {
                return true;
            }
            return false;
        }
    }
}
