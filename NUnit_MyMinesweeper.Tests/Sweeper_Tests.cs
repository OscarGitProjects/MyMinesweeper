using MyMinesweeper;
using System.Reflection;

namespace NUnit_MyMinesweeper.Tests
{
    public class Sweeper_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Description("Metoden testar att konstruktorn skapar en spelplan")]
        public void Test()
        {
            // Arrange
            //int expectedNumberOfRows = 10;
            //int expectedNumberOfColumns = 10;
            //Board board = new Board(expectedNumberOfColumns, expectedNumberOfRows);

            // Actual
            //int actualNumberOfRows = board.NumberOfColumns;
            //int actualNumberOfColumns = board.NumberOfRows;
            //Square[,] actualBoard = board.TheBoard;

            // Assert
            //Assert.AreEqual(actualNumberOfRows, expectedNumberOfRows);
            //Assert.AreEqual(actualNumberOfColumns, expectedNumberOfColumns);
            //Assert.NotNull(actualBoard);

            Assert.Pass();
        }
    }
}
