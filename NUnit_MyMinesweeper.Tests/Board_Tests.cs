using MyMinesweeper;
using System.Reflection;

namespace NUnit_MyMinesweeper.Tests
{
    public class Board_Tests : Base_Tests
    {
        #region Hjälp metoder

        /// <summary>
        /// Metoden räknar hur många rutor det finns med sökt enum Cell
        /// </summary>
        /// <param name="board">Spelplanen</param>
        /// <param name="cell">enum Cell med önskat värde</param>
        /// <returns>Antalet rutor med sökt värde i cell</returns>
        private int CalculateNumberOfSquaresWithValue(Square[,] board, Cell cell)
        {
            int numberOfSquaresWithValue = 0;
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
                        if (square.SquareCell == cell)
                            numberOfSquaresWithValue++;
                    }
                }
            }

            return numberOfSquaresWithValue;
        }


        /// <summary>
        /// Metoden räknar hur många rutor det finns där variabeln MineAsNeighbourCount är sökt antal
        /// </summary>
        /// <param name="board">Spelplanen</param>
        /// <param name="cell">Sökt värde på MineAsNeighbourCount</param>
        /// <returns>Antalet rutor med sökt värde i variabeln MineAsNeighbourCount</returns>
        private int CalculateNumberOfSquaresWithValue(Square[,] board, int mineAsNeighbourCount)
        {
            int numberOfSquaresWithValue = 0;
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
                        if (square.MineAsNeighbourCount == mineAsNeighbourCount)
                            numberOfSquaresWithValue++;
                    }
                }
            }

            return numberOfSquaresWithValue;
        }


        #endregion // End of Hjälp metoder

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Description("Metoden testar att konstruktorn skapar en spelplan")]
        public void Constructor_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Actual
            int actualNumberOfRows = board.NumberOfColumns;
            int actualNumberOfColumns = board.NumberOfRows;
            Square[,] actualBoard = board.TheBoard;

            // Assert
            Assert.AreEqual(actualNumberOfRows, expectedNumberOfRows);
            Assert.AreEqual(actualNumberOfColumns, expectedNumberOfColumns);
            Assert.NotNull(actualBoard);
        }

        #region Testar metoden UpdateNeighbourCount

        /// <summary>
        /// Metoden testar att variabeln MineAsNeighbourCount uppdateras korrekt när man anropar methoden UpdateNeighbourCount
        /// Om rutan på spelplanen innehåller en mina. Ska inte värdet uppdateras
        /// </summary>
        [Test]
        [Description("Metoden testar att variabeln MineAsNeighbourCount uppdateras korrekt när man anropar methoden UpdateNeighbourCount")]
        public void UpdateNeighbourCount_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);
            Square[,] theBoard = board.TheBoard;

            // Clear board
            theBoard = SetSquaresToCellValue(theBoard, Cell.CELL_0);

            // Det ska finnas en dold mina
            int expectedRowPositionOfMine = expectedNumberOfRows / 5;
            int expectedColPositionOfMine = expectedNumberOfColumns / 5;
            theBoard[expectedRowPositionOfMine, expectedColPositionOfMine].SquareCell = Cell.CELL_UNREVEALED_MINE;

            // Anropa private metoden UpdateNeighbourCount med hjälp av reflection
            MethodInfo method = board.GetType().GetMethod("UpdateNeighbourCount", BindingFlags.NonPublic | BindingFlags.Instance);

            int expectedRow = 1;
            int expectedColumn = 1;

            object[] parms = new object[2] { expectedRow, expectedColumn };
            var updatedValue = method.Invoke(board, parms);

            object[] parms2 = new object[2] { expectedRowPositionOfMine, expectedColPositionOfMine };
            var updatedValue2 = method.Invoke(board, parms2);


            // Actual
            Square[,] actualTheBoard = board.TheBoard;
            Square actualSquare = actualTheBoard[expectedRow, expectedColumn];
            int actualNumberOfMinesAsNeighbourCount = actualSquare.MineAsNeighbourCount;

            Square actualSquare2 = actualTheBoard[expectedRowPositionOfMine, expectedColPositionOfMine];
            int actualNumberOfMinesAsNeighbourCount2 = actualSquare2.MineAsNeighbourCount;

            // Assert
            Assert.AreEqual(1, actualNumberOfMinesAsNeighbourCount);

            Assert.AreEqual(0, actualNumberOfMinesAsNeighbourCount2);
        }

        #endregion

        #region Testar metoden GetSquare

        [Test]
        [Description("Metoden testar att metoden GetSquare kastar NullReferenceException när spelplanen inte är skapad")]
        public void GetSquare_Kastar_NullReferenceException_Spelplanen_Inte_Skapad_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            board.TheBoard = null;

            // Actual

            // Assert
            Assert.Throws<NullReferenceException>(() => board.GetSquare(1, 1));
        }


        [Test]
        [Description("Metoden testar att metoden GetSquare kastar IndexOutOfRangeException när man försöker att hämta ruta utanför spelplanen")]
        public void GetSquare_Kastar_IndexOutOfRangeException_Nar_Hamtar_Data_Utanfor_Spelplanen_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Actual

            // Assert
            Assert.Throws<IndexOutOfRangeException>(() => board.GetSquare(expectedNumberOfColumns + 1, 0));
        }


        [Test]
        [Description("Metoden testar att metoden GetSquare returnerar önskad ruta från spelplanen")]
        public void GetSquare_Returnerar_Onskad_Ruta_Fran_Spelplanen_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Ändra värdet i en ruta på spelplanen
            Square[,] theBoard = board.TheBoard;
            theBoard[0, 0].SquareCell = Cell.CELL_BLOWN_MINE;

            // Actual
            Square actualSquare = board.GetSquare(0, 0);

            // Assert
            Assert.AreEqual(Cell.CELL_BLOWN_MINE, actualSquare.SquareCell);
        }

        #endregion

        #region Testar metoden HasSelectedSquareAValue

        [Test]
        [Description("Metoden testar att metoden HasSelectedSquareAValue kastar NullReferenceException när spelplanen inte är skapad")]
        public void HasSelectedSquareAValue_Kastar_NullReferenceException_Spelplanen_Inte_Skapad_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            board.TheBoard = null;


            // Actual

            // Assert
            Assert.Throws<NullReferenceException>(() => board.HasSelectedSquareAValue(1, 1, Cell.CELL_2));
        }


        [Test]
        [Description("Metoden testar att metoden HasSelectedSquareAValue kastar IndexOutOfRangeException när man försöker att hämta ruta utanför spelplanen")]
        public void HasSelectedSquareAValue_Kastar_IndexOutOfRangeException_Nar_Hamtar_Data_Utanfor_Spelplanen_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Actual

            // Assert
            Assert.Throws<IndexOutOfRangeException>(() => board.HasSelectedSquareAValue(expectedNumberOfColumns + 1, 0, Cell.CELL_INVALID));
        }


        [Test]
        [Description("Metoden testar att metoden HasSelectedSquareAValue fungerar korrekt")]
        public void HasSelectedSquareAValue_Fungerar_Korrekt_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Ändra värdet i en ruta på spelplanen
            Square[,] theBoard = board.TheBoard;
            theBoard[0, 0].SquareCell = Cell.CELL_BLOWN_MINE;

            // Actual
            bool actualSquarehasSelectedValue1 = board.HasSelectedSquareAValue(0, 0, Cell.CELL_BLOWN_MINE);
            bool actualSquarehasSelectedValue2 = board.HasSelectedSquareAValue(1, 1, Cell.CELL_BLOWN_MINE);

            // Assert
            Assert.IsTrue(actualSquarehasSelectedValue1);
            Assert.IsFalse(actualSquarehasSelectedValue2);
        }

        #endregion

        #region Testar metoden IsSelectedSquareAUnrevealedMine

        [Test]
        [Description("Metoden testar att metoden IsSelectedSquareAUnrevealedMine kastar NullReferenceException när spelplanen inte är skapad")]
        public void IsSelectedSquareAUnrevealedMine_Kastar_NullReferenceException_Spelplanen_Inte_Skapad_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            board.TheBoard = null;

            // Actual

            // Assert
            Assert.Throws<NullReferenceException>(() => board.IsSelectedSquareAUnrevealedMine(1, 1));
        }


        [Test]
        [Description("Metoden testar att metoden IsSelectedSquareAUnrevealedMine kastar IndexOutOfRangeException när man försöker att hämta ruta utanför spelplanen")]
        public void IsSelectedSquareAUnrevealedMine_Kastar_IndexOutOfRangeException_Nar_Hamtar_Data_Utanfor_Spelplanen_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Actual

            // Assert
            Assert.Throws<IndexOutOfRangeException>(() => board.IsSelectedSquareAUnrevealedMine(expectedNumberOfColumns + 1, 0));
        }


        [Test]
        [Description("Metoden testar att metoden IsSelectedSquareAUnrevealedMine fungerar korrekt")]
        public void IsSelectedSquareAUnrevealedMine_Fungerar_Korrekt_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Ändra värdet i en ruta på spelplanen
            Square[,] theBoard = board.TheBoard;
            theBoard[0, 0].SquareCell = Cell.CELL_UNREVEALED_MINE;

            // Actual
            bool actualSquareIsUnrevealedMine1 = board.IsSelectedSquareAUnrevealedMine(0, 0);
            bool actualSquareIsUnrevealedMine2 = board.IsSelectedSquareAUnrevealedMine(1, 1);

            // Assert
            Assert.IsTrue(actualSquareIsUnrevealedMine1);
            Assert.IsFalse(actualSquareIsUnrevealedMine2);
        }

        #endregion

        #region Testar metoden IsSelectedSquareOpen

        [Test]
        [Description("Metoden testar att metoden IsSelectedSquareOpen kastar NullReferenceException när spelplanen inte är skapad")]
        public void IsSelectedSquareOpen_Kastar_NullReferenceException_Spelplanen_Inte_Skapad_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            board.TheBoard = null;

            // Actual

            // Assert
            Assert.Throws<NullReferenceException>(() => board.IsSelectedSquareOpen(1, 1));
        }


        [Test]
        [Description("Metoden testar att metoden IsSelectedSquareOpen kastar IndexOutOfRangeException när man försöker att hämta ruta utanför spelplanen")]
        public void IsSelectedSquareOpen_Kastar_IndexOutOfRangeException_Nar_Hamtar_Data_Utanfor_Spelplanen_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Actual

            // Assert
            Assert.Throws<IndexOutOfRangeException>(() => board.IsSelectedSquareOpen(expectedNumberOfColumns + 1, 0));
        }


        [Test]
        [Description("Metoden testar att metoden IsSelectedSquareOpen fungerar korrekt")]
        public void IsSelectedSquareOpen_Fungerar_Korrekt_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Ändra värdet i en ruta på spelplanen
            Square[,] theBoard = board.TheBoard;
            theBoard[0, 0].IsOpenSquare = true;

            // Actual
            bool actualSquareIsUnrevealedMine1 = board.IsSelectedSquareOpen(0, 0);
            bool actualSquareIsUnrevealedMine2 = board.IsSelectedSquareOpen(1, 1);

            // Assert
            Assert.IsTrue(actualSquareIsUnrevealedMine1);
            Assert.IsFalse(actualSquareIsUnrevealedMine2);
        }

        #endregion

        #region Testar metoden CreateMinesOnBoard

        [Test]
        [Description("Metoden testar att metoden CreateMinesOnBoard skapar korrekt antal minor på spelplanen")]
        public void CreateMinesOnBoard_Skapar_Korrekt_Antal_Minor_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            int expectedNumberOfMines = (expectedNumberOfRows * expectedNumberOfColumns) / 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);
            Square[,] theBoard = board.TheBoard;

            // Clear board
            theBoard = SetSquaresToCellValue(theBoard, Cell.CELL_0);

            // Anropa private metoden CreateMinesOnBoard med hjälp av reflection
            MethodInfo method = board.GetType().GetMethod("CreateMinesOnBoard", BindingFlags.NonPublic | BindingFlags.Instance);

            object[] parms = null;
            method.Invoke(board, parms);

            // Actual
            int actualNumberOfMines = CalculateNumberOfSquaresWithValue(theBoard, Cell.CELL_UNREVEALED_MINE);

            // Assert
            Assert.AreEqual(expectedNumberOfMines, actualNumberOfMines);
        }

        #endregion

        #region Testar metoden CalculateNumberOfMinesAsNeigbours

        [Test]
        [Description("Metoden testar att metoden CalculateNumberOfMinesAsNeigbours gör korrekta beräkningar när vi har en mina på spelplanen")]
        public void CalculateNumberOfMinesAsNeigbours_Med_1_Mina_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);
            Square[,] theBoard = board.TheBoard;

            // Clear board
            theBoard = SetSquaresToCellValue(theBoard, Cell.CELL_0);

            // Skapa en mina
            theBoard[0, 0].SquareCell = Cell.CELL_UNREVEALED_MINE;
            int expectedNumberOfMines = 1;

            // Anropa private metoden CalculateNumberOfMinesAsNeigbours med hjälp av reflection
            MethodInfo method = board.GetType().GetMethod("CalculateNumberOfMinesAsNeigbours", BindingFlags.NonPublic | BindingFlags.Instance);

            object[] parms = null;
            method.Invoke(board, parms);

            // Actual
            int actualNumberOfMines = CalculateNumberOfSquaresWithValue(theBoard, Cell.CELL_UNREVEALED_MINE);
            int actualNumberOfNeigboursToOneMine = CalculateNumberOfSquaresWithValue(theBoard, 1);

            // Assert
            Assert.AreEqual(expectedNumberOfMines, actualNumberOfMines);
            Assert.AreEqual(3, actualNumberOfNeigboursToOneMine);
        }

        [Test]
        [Description("Metoden testar att metoden CalculateNumberOfMinesAsNeigbours gör korrekta beräkningar när vi har två minor på spelplanen. Dessa minor är inte placerade nära varandra")]
        public void CalculateNumberOfMinesAsNeigbours_Med_2_Minor_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);
            Square[,] theBoard = board.TheBoard;

            // Clear board
            theBoard = SetSquaresToCellValue(theBoard, Cell.CELL_0);

            // Skapa minor
            theBoard[0, 0].SquareCell = Cell.CELL_UNREVEALED_MINE;
            theBoard[5, 5].SquareCell = Cell.CELL_UNREVEALED_MINE;
            int expectedNumberOfMines = 2;

            // Anropa private metoden CalculateNumberOfMinesAsNeigbours med hjälp av reflection
            MethodInfo method = board.GetType().GetMethod("CalculateNumberOfMinesAsNeigbours", BindingFlags.NonPublic | BindingFlags.Instance);

            object[] parms = null;
            method.Invoke(board, parms);

            // Actual
            int actualNumberOfMines = CalculateNumberOfSquaresWithValue(theBoard, Cell.CELL_UNREVEALED_MINE);
            int actualNumberOfNeigboursToOneMine = CalculateNumberOfSquaresWithValue(theBoard, 1);

            // Assert
            Assert.AreEqual(expectedNumberOfMines, actualNumberOfMines);
            Assert.AreEqual(11, actualNumberOfNeigboursToOneMine);
        }


        [Test]
        [Description("Metoden testar att metoden CalculateNumberOfMinesAsNeigbours gör korrekta beräkningar när vi har två minor på spelplanen. Dessa minor är placerade bredvid varandra")]
        public void CalculateNumberOfMinesAsNeigbours_Med_2_Minor_Bredvid_Varandra_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);
            Square[,] theBoard = board.TheBoard;

            // Clear board
            theBoard = SetSquaresToCellValue(theBoard, Cell.CELL_0);

            // Skapa minor
            theBoard[1, 1].SquareCell = Cell.CELL_UNREVEALED_MINE;
            theBoard[1, 2].SquareCell = Cell.CELL_UNREVEALED_MINE;
            int expectedNumberOfMines = 2;

            // Anropa private metoden CalculateNumberOfMinesAsNeigbours med hjälp av reflection
            MethodInfo method = board.GetType().GetMethod("CalculateNumberOfMinesAsNeigbours", BindingFlags.NonPublic | BindingFlags.Instance);

            object[] parms = null;
            method.Invoke(board, parms);

            // Actual
            int actualNumberOfMines = CalculateNumberOfSquaresWithValue(theBoard, Cell.CELL_UNREVEALED_MINE);
            int actualNumberOfNeigboursToOneMine = CalculateNumberOfSquaresWithValue(theBoard, 1);
            int actualNumberOfNeigboursToTwoMine = CalculateNumberOfSquaresWithValue(theBoard, 2);

            /* 
            M = Mina, 1 = Granne med 1 mina, 2 = Granne med 2 minor
            |1|2|2|1|0|
            |1|M|M|1|0|
            |1|2|2|1|0|
            */

            // Assert
            Assert.AreEqual(expectedNumberOfMines, actualNumberOfMines);
            Assert.AreEqual(6, actualNumberOfNeigboursToOneMine);
            Assert.AreEqual(4, actualNumberOfNeigboursToTwoMine);
        }


        #endregion

        #region Testar metoden ConvertNumberOfMinesAsNeigboursToEnum

        [Test]
        [Description("Metoden testar att metoden ConvertNumberOfMinesAsNeigboursToEnum gör korrekta konverteringar till enum på spelplanen")]
        public void ConvertNumberOfMinesAsNeigboursToEnum_Gor_Korrekta_Konverteringar_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            int expectedNumberOfSquares = expectedNumberOfRows * expectedNumberOfColumns;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);
            Square[,] theBoard = board.TheBoard;

            // Clear board
            theBoard = SetSquaresToCellValue(theBoard, Cell.CELL_0);

            int actualNumberOfCELL_O = CalculateNumberOfSquaresWithValue(theBoard, Cell.CELL_0);

            // Skapa en mina
            theBoard[0, 0].SquareCell = Cell.CELL_UNREVEALED_MINE;
            int expectedNumberOfMines = 1;

            // Anropa private metoden CalculateNumberOfMinesAsNeigbours med hjälp av reflection
            MethodInfo method1 = board.GetType().GetMethod("CalculateNumberOfMinesAsNeigbours", BindingFlags.NonPublic | BindingFlags.Instance);

            object[] parms1 = null;
            method1.Invoke(board, parms1);

            // Anropa private metoden ConvertNumberOfMinesAsNeigboursToEnum med hjälp av reflection
            MethodInfo method2 = board.GetType().GetMethod("ConvertNumberOfMinesAsNeigboursToEnum", BindingFlags.NonPublic | BindingFlags.Instance);

            object[] parms2 = null;
            method2.Invoke(board, parms2);

            // Actual
            int actualNumberOfMines = CalculateNumberOfSquaresWithValue(theBoard, Cell.CELL_UNREVEALED_MINE);
            int actualNumberOfNeigboursToOneMine = CalculateNumberOfSquaresWithValue(theBoard, 1);
            int actualNumberOfCELL_1 = CalculateNumberOfSquaresWithValue(theBoard, Cell.CELL_1);

            // Assert
            Assert.AreEqual(expectedNumberOfSquares, actualNumberOfCELL_O);
            Assert.AreEqual(expectedNumberOfMines, actualNumberOfMines);
            Assert.AreEqual(3, actualNumberOfNeigboursToOneMine);
            Assert.AreEqual(3, actualNumberOfCELL_1);
        }

        #endregion
    }
}