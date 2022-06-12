using MyMinesweeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit_MyMinesweeper.Tests
{
    public class Base_Tests
    {
        #region Hjälp metoder

        /// <summary>
        /// Metoden sätter värdena på rutorna på spelplanen till önskat värde med enum Cell
        /// </summary>
        /// <param name="board">Spelplanen</param>
        /// <param name="cell">enum Cell med önskat värde</param>
        /// <returns>Uppdaterad spelplan</returns>
        protected Square[,] SetSquaresToCellValue(Square[,] board, Cell cell)
        {
            int numberOfRows = board.GetLength(0);
            int numberOfColumns = board.GetLength(1);
            Square square = null;

            for (int row = 0; row < numberOfRows; row++)
            {
                for (int col = 0; col < numberOfColumns; col++)
                {
                    square = board[row, col];
                    if (square != null)
                    {
                        square.SquareCell = cell;
                        square.IsOpenSquare = false;
                        square.MineAsNeighbourCount = 0;
                    }
                }
            }

            return board;
        }

        #endregion
    }
}
