namespace PlayerClient.AI
{
    //ChatGPT Promt: 
    // You are a programmer writing a "connect four" video game.
    // You need to write a function that produces the best possible turn.
    // The output of the function is a number between 0 and 6.
    // Implement the function with a Minimax algorithm.
    // The input of the function is the game state represented in a 2 dimensional array: 
    // 0 no play stone
    // 1 player 1 play stone
    // 2 player 2 play stone

    internal static class Connect4Ai
    {
        private const int rows = 6;
        private const int cols = 7;

        private static readonly Random random = new();

        public static int? GetBestMove(int[,] board, int searchDepth)
        {
            var result = Minimax(board, searchDepth, long.MinValue, long.MaxValue, true);
            return result.Item1;
        }

        private static bool IsTerminalNode(int[,] board)
        {
            return IsWon(board, 1) || IsWon(board, 2) || GetValidLocations(board).Count == 0;
        }

        private static bool IsWon(int[,] board, int piece)
        {
            // Check horizontal
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col <= cols - 4; col++)
                {
                    if (board[row, col] == piece &&
                        board[row, col + 1] == piece &&
                        board[row, col + 2] == piece &&
                        board[row, col + 3] == piece)
                    {
                        return true;
                    }
                }
            }

            // Check vertical
            for (var col = 0; col < cols; col++)
            {
                for (int row = 0; row <= rows - 4; row++)
                {
                    if (board[row, col] == piece &&
                        board[row + 1, col] == piece &&
                        board[row + 2, col] == piece &&
                        board[row + 3, col] == piece)
                    {
                        return true;
                    }
                }
            }

            // Check diagonal (bottom-left to top-right)
            for (int row = 0; row <= rows - 4; row++)
            {
                for (int col = 0; col <= cols - 4; col++)
                {
                    if (board[row, col] == piece &&
                        board[row + 1, col + 1] == piece &&
                        board[row + 2, col + 2] == piece &&
                        board[row + 3, col + 3] == piece)
                    {
                        return true;
                    }
                }
            }

            // Check diagonal (top-left to bottom-right)
            for (int row = 3; row < rows; row++)
            {
                for (int col = 0; col <= cols - 4; col++)
                {
                    if (board[row, col] == piece &&
                        board[row - 1, col + 1] == piece &&
                        board[row - 2, col + 2] == piece &&
                        board[row - 3, col + 3] == piece)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static int EvaluateWindow(int[] window, int piece)
        {
            var score = 0;
            var opp_piece = piece == 1 ? 2 : 1;

            if (window.Count(x => x == piece) == 4)
            {
                score += 100;
            }
            else if (window.Count(x => x == piece) == 3 && window.Count(x => x == 0) == 1)
            {
                score += 5;
            }
            else if (window.Count(x => x == piece) == 2 && window.Count(x => x == 0) == 2)
            {
                score += 2;
            }

            if (window.Count(x => x == opp_piece) == 3 && window.Count(x => x == 0) == 1)
            {
                score -= 4;
            }

            return score;
        }

        private static int ScorePosition(int[,] board, int piece)
        {
            var score = 0;

            // Score center column
            var centerArray = new List<int>();
            for (var i = 0; i < rows; i++)
            {
                centerArray.Add(board[i, cols / 2]);
            }
            var centerCount = centerArray.Count(x => x == piece);
            score += centerCount * 3;

            // Score Horizontal
            for (var r = 0; r < rows; r++)
            {
                var rowArray = new List<int>();
                for (var i = 0; i < cols; i++)
                {
                    rowArray.Add(board[r, i]);
                }
                for (var c = 0; c < cols - 3; c++)
                {
                    var window = rowArray.GetRange(c, 4).ToArray();
                    score += EvaluateWindow(window, piece);
                }
            }

            // Score Vertical
            for (var c = 0; c < cols; c++)
            {
                var colArray = new List<int>();
                for (var i = 0; i < rows; i++)
                {
                    colArray.Add(board[i, c]);
                }
                for (var r = 0; r < rows - 3; r++)
                {
                    var window = colArray.GetRange(r, 4).ToArray();
                    score += EvaluateWindow(window, piece);
                }
            }

            // Score positively sloped diagonals
            for (var r = 0; r < rows - 3; r++)
            {
                for (var c = 0; c < cols - 3; c++)
                {
                    var window = new int[4];
                    for (var i = 0; i < 4; i++)
                    {
                        window[i] = board[r + i, c + i];
                    }
                    score += EvaluateWindow(window, piece);
                }
            }

            // Score negatively sloped diagonals
            for (var r = 0; r < rows - 3; r++)
            {
                for (var c = 0; c < cols - 3; c++)
                {
                    var window = new int[4];
                    for (var i = 0; i < 4; i++)
                    {
                        window[i] = board[r + 3 - i, c + i];
                    }
                    score += EvaluateWindow(window, piece);
                }
            }

            return score;
        }

        private static List<int> GetValidLocations(int[,] board)
        {
            var validLocations = new List<int>();
            for (var col = 0; col < cols; col++)
            {
                if (board[0, col] == 0)
                {
                    validLocations.Add(col);
                }
            }
            return validLocations;
        }

        private static int GetNextOpenRow(int[,] board, int col)
        {
            for (var r = rows - 1; r >= 0; r--)
            {
                if (board[r, col] == 0)
                {
                    return r;
                }
            }
            return -1;
        }

        private static (int?, long) Minimax(int[,] board, int depth, long alpha, long beta, bool maximizingPlayer)
        {
            var validLocations = GetValidLocations(board);
            var isTerminal = IsTerminalNode(board);
            if (depth == 0 || isTerminal)
            {
                if (isTerminal)
                {
                    if (IsWon(board, 2))
                        return (null, 100_000_000_000_000 + depth);

                    if (IsWon(board, 1))
                        return (null, -10_000_000_000_000 - depth);

                    return (null, 0);
                }

                return (null, ScorePosition(board, 2));
            }

            if (maximizingPlayer)
            {
                var value = long.MinValue;
                var column = validLocations[random.Next(validLocations.Count)];
                foreach (var col in validLocations)
                {
                    var row = GetNextOpenRow(board, col);
                    var boardCopy = (int[,])board.Clone();
                    boardCopy[row, col] = 2;
                    var newScore = Minimax(boardCopy, depth - 1, alpha, beta, false).Item2;
                    if (newScore > value)
                    {
                        value = newScore;
                        column = col;
                    }
                    alpha = Math.Max(alpha, value);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                return (column, value);
            }
            else
            {
                var value = long.MaxValue;
                var column = validLocations[random.Next(validLocations.Count)];
                foreach (var col in validLocations)
                {
                    var row = GetNextOpenRow(board, col);
                    var boardCopy = (int[,])board.Clone();
                    boardCopy[row, col] = 1;
                    var newScore = Minimax(boardCopy, depth - 1, alpha, beta, true).Item2;
                    if (newScore < value)
                    {
                        value = newScore;
                        column = col;
                    }
                    beta = Math.Min(beta, value);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                return (column, value);
            }
        }

        
    }
}
