using MyMinesweeper;
using MyMinesweeper.Interface;
using System.Reflection;

namespace NUnit_MyMinesweeper.Tests
{
    public class Sweeper_Tests : Base_Tests
    {
        private Sweeper sweeper = null;

        [OneTimeSetUp]
        public void TestSetup()
        {
            sweeper = new Sweeper();
        }

        [SetUp]
        public void Setup()
        {
        }

        #region Testar metoden IsSelectedSquareAMine

        [Test]
        [Description("Metoden testar att metoden IsSelectedSquareAMine kastar ArgumentNullException när referensen till Board är null")]
        public void IsSelectedSquareAMine_Kastar_ArgumentNullException_Nar_Referensen_Till_Board_Null_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = null;//new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Actual

            // Assert
            Assert.Throws<ArgumentNullException>(() => sweeper.IsSelectedSquareAMine(board, 1, 1));
        }


        [Test]
        [Description("Metoden testar att metoden IsSelectedSquareAMine kastar NullReferenceException när spelplanen inte är skapad")]
        public void IsSelectedSquareAMine_Kastar_NullReferenceException_Spelplanen_Inte_Skapad_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            board.TheBoard = null;

            // Actual

            // Assert
            Assert.Throws<NullReferenceException>(() => sweeper.IsSelectedSquareAMine(board, 1, 1));
        }


        [Test]
        [Description("Metoden testar att metoden IsSelectedSquareAMine kastar IndexOutOfRangeException när vi försöker att läsa en ruta utanför spelplanen")]
        public void IsSelectedSquareAMine_Kastar_IndexOutOfRangeException_Nar_Vi_Laser_Ruta_Utanfor_Spelplanen_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Actual

            // Assert
            Assert.Throws<IndexOutOfRangeException>(() => sweeper.IsSelectedSquareAMine(board, expectedNumberOfColumns + 1, 1));
        }


        [Test]
        [Description("Metoden testar att metoden IsSelectedSquareAMine returnerar rätt resultat")]
        public void IsSelectedSquareAMine_Fungerar_Korrekt_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Rensa spelplanen           
            board.TheBoard = SetSquaresToCellValue(board.TheBoard, Cell.CELL_0);

            // Ändra värdet i en ruta på spelplanen
            Square[,] theBoard = board.TheBoard;
            theBoard[0, 0].SquareCell = Cell.CELL_UNREVEALED_MINE;
            theBoard[1, 1].SquareCell = Cell.CELL_BLOWN_MINE;

            // Actual
            bool actualMine1 = sweeper.IsSelectedSquareAMine(board, 0, 0);
            bool actualMine2 = sweeper.IsSelectedSquareAMine(board, 1, 1);
            bool actualMine3 = sweeper.IsSelectedSquareAMine(board, 2, 2);


            // Assert
            Assert.IsTrue(actualMine1);
            Assert.IsTrue(actualMine2);
            Assert.IsFalse(actualMine3);
        }

        #endregion

        #region Testar metoden PerformMove

        [Test]
        [Description("Metoden testar att metoden PerformMove kastar ArgumentNullException när referensen till Board är null")]
        public void PerformMove_Kastar_ArgumentNullException_Spelplanen_Inte_Skapad_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = null;//new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Actual

            // Assert
            Assert.Throws<ArgumentNullException>(() => sweeper.PerformMove(board, 1, 1));
        }


        [Test]
        [Description("Metoden testar att metoden PerformMove kastar NullReferenceException när spelplanen inte är skapad")]
        public void PerformMove_Kastar_NullReferenceException_Spelplanen_Inte_Skapad_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            board.TheBoard = null;
            // Actual

            // Assert
            Assert.Throws<NullReferenceException>(() => sweeper.PerformMove(board, 1, 1));
        }


        [Test]
        [Description("Metoden testar att metoden PerformMove kastar IndexOutOfRangeException när vi föröker att läsa en ruta utanför spelplanen")]
        public void PerformMove_Kastar_IndexOutOfRangeException_Nar_Vi_Laser_Ruta_Utanfor_Spelplanen_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Actual

            // Assert
            Assert.Throws<IndexOutOfRangeException>(() => sweeper.PerformMove(board, expectedNumberOfColumns + 1, 1));
        }


        [Test]
        [Description("Metoden testar att metoden PerformMove returnerar rätt resultat")]
        public void PerformMove_Fungerar_Korrekt_Test()
        {
            // Arrange
            int expectedNumberOfRows = 10;
            int expectedNumberOfColumns = 10;
            Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Rensa spelplanen           
            board.TheBoard = SetSquaresToCellValue(board.TheBoard, Cell.CELL_0);

            // Ändra värdet i en ruta på spelplanen
            Square[,] theBoard = board.TheBoard;
            theBoard[0, 0].SquareCell = Cell.CELL_1;
            theBoard[1, 1].SquareCell = Cell.CELL_UNREVEALED_MINE;

            int expectedStatus1 = 2;
            int expectedStatus2 = 1;
            int expectedStatus3 = 0;

            // Actual
            // Status ska vara 2
            (Board actualBoard1, int actualStatus1) = sweeper.PerformMove(board, 0, 0);
            // Status ska vara 1 dvs. det är en mina
            (Board actualBoard2, int actualStatus2) = sweeper.PerformMove(board, 1, 1);
            // Status ska vara 0
            (Board actualBoard3, int actualStatus3) = sweeper.PerformMove(board, 2, 2);


            // Assert
            Assert.AreEqual(expectedStatus1, actualStatus1);
            Assert.AreEqual(expectedStatus2, actualStatus2);
            Assert.AreEqual(expectedStatus3, actualStatus3);
        }


        #endregion
    }
}
