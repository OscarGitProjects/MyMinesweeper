using MyMinesweeper.Interface;
using MyMinesweeper.UI;
using System.Text;

namespace MyMinesweeper.Utils;

/// <summary>
/// Klass med olika hjälpmetoder för hantering av Board object
/// </summary>
public class BoardHelper : IBoardHelper
{
    /// <summary>
    /// Referens till objekt som hanterar utskrift av information
    /// </summary>
    protected IUI Ui { get; }


    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="ui">Referense till objekt för utskrift</param>
    public BoardHelper(IUI ui)
    {
        Ui = ui;
    }


    /// <summary>
    /// Metoden visar öppna rutor på spelplanen
    /// </summary>
    /// <param name="board">Referens till spelplanen</param>
    /// <param name="showAllSquares">Visa alla rutor på spelplanen. Tar inte hänsyn till de andra parametrarar för vad som skall visas</param>
    /// <param name="showOnlyMines">Visar alla rutor med minor. Tar inte hänsyn till de andra parametrarar för vad som skall visas</param>
    /// <param name="showOnlyOpenSquares">Visar bara rutor som har öppnats. Tar inte hänsyn till de andra parametrarar för vad som skall visas</param>
    /// <exception cref="ArgumentNullException">Kastas om referensen till Board är null</exception>
    /// <exception cref="NullReferenceException">Kastas om spelplanen inte är skapad</exception>
    private void ShowBoard(Board board, bool showAllSquares = false, bool showOnlyMines = false, bool showOnlyOpenSquares = true)
    {
        if (board == null)
            throw new ArgumentNullException($"{nameof(BoardHelper)}->ShowBoard(). Referensen till Board är null");

        if (board.TheBoard == null)
            throw new NullReferenceException($"{nameof(BoardHelper)}->ShowBoard(). Spelplanen är inte skapad");

        int numberOfColumns = board.NumberOfColumns;
        int numberOfRows = board.NumberOfRows;

        Square? square = null;
        string symbol = string.Empty;
        StringBuilder strBuild = new StringBuilder();

        // Jag skriver ut en rad av spelplanen i taget
        for (int row = 0; row < numberOfRows; row++)
        {
            strBuild.Clear();

            for (int col = 0; col < numberOfColumns; col++)
            {
                try
                {
                    square = board.TheBoard[row, col];
                    if (square != null)
                    {
                        symbol = " ";

                        if (showAllSquares)
                        {// Vi ska visa alla rutor
                            symbol = GetSymbol(square);
                        }
                        else if (showOnlyOpenSquares)
                        {// Vi ska bara visa öppna rutor
                            if (square.IsOpenSquare)
                                symbol = GetSymbol(square);
                            else
                                symbol = " ";
                        }
                        else if (showOnlyMines)
                        {// Vi ska bara visa rutor med minor
                            if (square.SquareCell == Cell.CELL_BLOWN_MINE || square.SquareCell == Cell.CELL_UNREVEALED_MINE)
                                symbol = GetSymbol(square);
                            else
                                symbol = " ";
                        }

                        strBuild.Append($"|{symbol}|");
                    }
                }
                catch (Exception)
                { }
            }

            // Skriv ut en rad
            if (strBuild.Length > 0)
                Ui.WriteLine(strBuild.ToString());

            // Skriv ut en tom rad i underkant av rutorna
            Ui.WriteLine(" ");
        }
    }


    /// <summary>
    /// Metoden visar alla öppnade rutor på spelplanen
    /// </summary>
    /// <param name="board">Referens till spelplanen</param>
    /// <exception cref="ArgumentNullException">Kastas om referensen till Board är null</exception>
    /// <exception cref="NullReferenceException">Kastas om spelplanen inte är skapad</exception>
    public void ShowOnlyOpenSquaresOnBoard(Board board)
    {
        if (board == null)
            throw new ArgumentNullException($"{nameof(BoardHelper)}->ShowOnlyOpenSquaresOnBoard(). Referensen till Board är null");

        if (board.TheBoard == null)
            throw new NullReferenceException($"{nameof(BoardHelper)}->ShowOnlyOpenSquaresOnBoard(). Spelplanen är inte skapad");

        this.ShowBoard(board, showAllSquares: false, showOnlyMines: false, showOnlyOpenSquares: true);
    }


    /// <summary>
    /// Metoden visar alla rutor på spelplanen
    /// </summary>
    /// <param name="board">Referens till spelplanen</param>
    /// <exception cref="ArgumentNullException">Kastas om referensen till Board är null</exception>
    /// <exception cref="NullReferenceException">Kastas om spelplanen inte är skapad</exception>
    public void ShowAllSquaresOnBoard(Board board)
    {
        if (board == null)
            throw new ArgumentNullException($"{nameof(BoardHelper)}->ShowAllSquaresOnBoard(). Referensen till Board är null");

        if (board.TheBoard == null)
            throw new NullReferenceException($"{nameof(BoardHelper)}->ShowAllSquaresOnBoard(). Spelplanen är inte skapad");

        this.ShowBoard(board, showAllSquares: true, showOnlyMines: false, showOnlyOpenSquares: false);
    }


    /// <summary>
    /// Metoden visar bara rutor med minor på spelplanen
    /// </summary>
    /// <param name="board">Referens till spelplanen</param>
    /// <exception cref="ArgumentNullException">Kastas om referensen till Board är null</exception>
    /// <exception cref=NullReferenceException">Kastas om spelplanen inte är skapad</exception>
    public void ShowOnlySquaresWithMinesOnBoard(Board board)
    {
        if (board == null)
            throw new ArgumentNullException($"{nameof(Sweeper)}->ShowOnlySquaresWithMinesOnBoard(). Referensen till Board är null");

        if (board.TheBoard == null)
            throw new NullReferenceException($"{nameof(BoardHelper)}->ShowOnlySquaresWithMinesOnBoard(). Spelplanen är inte skapad");

        this.ShowBoard(board, showAllSquares: false, showOnlyMines: true, showOnlyOpenSquares: false);
    }


    /// <summary>
    /// Metoden konverterar innehållet i en ruta till lämpligt tecken
    /// Vi kolla på enum Cell som finns i rutan
    /// </summary>
    /// <param name="square">Ritan på spelplanen</param>
    /// <returns>Innehållet i rutan som lämpligt tecken</returns>
    /// <exception cref="ArgumentNullException">Kastas om referensen till Square är null</exception>
    public string GetSymbol(Square square)
    {
        if (square == null)
            throw new ArgumentNullException($"{nameof(BoardHelper)}->GetSymbol(). Referensen till Square är null");

        string symbol = string.Empty;

        switch (square.SquareCell)
        {
            case Cell.CELL_0:
                symbol = "0";
                break;
            case Cell.CELL_1:
                symbol = "1";
                break;
            case Cell.CELL_2:
                symbol = "2";
                break;
            case Cell.CELL_3:
                symbol = "3";
                break;
            case Cell.CELL_4:
                symbol = "4";
                break;
            case Cell.CELL_5:
                symbol = "5";
                break;
            case Cell.CELL_6:
                symbol = "6";
                break;
            case Cell.CELL_7:
                symbol = "7";
                break;
            case Cell.CELL_8:
                symbol = "8";
                break;
            case Cell.CELL_BLOWN_MINE:
                symbol = "*";
                break;
            case Cell.CELL_UNREVEALED_EMPTY:
                symbol = "E";
                break;
            case Cell.CELL_UNREVEALED_MINE:
                symbol = "M";
                break;
            case Cell.CELL_INVALID:
                symbol = "?";
                break;
            default:
                symbol = " ";
                break;
        }

        return symbol;
    }
}
