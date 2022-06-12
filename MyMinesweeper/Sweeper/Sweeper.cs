namespace MyMinesweeper.Interface;

/// <summary>
/// Klassen har en metod som anropas när en avnvändare hhar valt en ruta på spelplanen. returnerar en uppdaterad spelplan
/// Har en metod som testar om vald ruta på spelplanen är en mina eller ej
/// </summary>
public class Sweeper : ISweeper
{
    /// <summary>
    /// Metoden testar om en vald ruta på spelplanen var en mina eller ej
    /// </summary>
    /// <param name="board">Spelplanen</param>
    /// <param name="selectedColumn">Vald kolumn på spelplanen</param>
    /// <param name="selectedRow">Vald rad på spelplanen</param>
    /// <returns>true om vald ruta var en mina.Annars returneras false</returns>
    /// <exception cref="ArgumentNullException">Kastas om referensen till Board är null</exception>
    /// <exception cref="NullReferenceException">Kastas om spelplanen inte är skapad. Kastas också om vald ruta saknas på spelplanen</exception>
    /// <exception cref="IndexOutOfRangeException">Kastas om vi försöker att hämta ruta utanför spelplanen</exception>
    public bool IsSelectedSquareAMine(Board board, int selectedColumn, int selectedRow)
    {
        if (board == null)
            throw new ArgumentNullException($"{nameof(Sweeper)}->IsSelectedSquareAMine(). Referensen till Board är null");

        if (board.TheBoard == null)
            throw new NullReferenceException($"{nameof(Sweeper)}->IsSelectedSquareAMine(). Spelplanen är inte skapad");

        Square? selectedSquare = null;
        try
        {
            selectedSquare = board.TheBoard[selectedRow, selectedColumn];
            if (selectedSquare == null)
                throw new NullReferenceException($"{nameof(Sweeper)}->IsSelectedSquareAMine(). Vald ruta på spelplanen saknas");
        }
        catch (NullReferenceException)
        {
            throw;
        }
        catch (IndexOutOfRangeException)
        {
            throw;
        }

        bool selectedPositionIsAMine = false;
        if (selectedSquare.SquareCell == Cell.CELL_UNREVEALED_MINE || selectedSquare.SquareCell == Cell.CELL_BLOWN_MINE)
            selectedPositionIsAMine = true;

        return selectedPositionIsAMine;
    }


    /// <summary>
    /// Metoden anropas när användaren har valt en ruta på spelplanen
    /// Ska returnera ett uppdaterat bräde
    /// </summary>
    /// <param name="board">Spelplanen</param>
    /// <param name="selectedColumn">Vald kolumn på spelplanen</param>
    /// <param name="selectedRow">Vald rad på spelplanen</param>
    /// <returns>Uppdaterade spelplanen</returns>
    /// <exception cref="ArgumentNullException">Kastas om referensen till Board är null</exception>
    /// <exception cref="NullReferenceException">Kastas om spelplanen inte är skapad</exception>
    public Board PerformMove(Board board, int selectedColumn, int selectedRow)
    {
        if (board == null)
            throw new ArgumentNullException($"{nameof(Sweeper)}->PerformMove(). Referensen till Board är null");

        if (board.TheBoard == null)
            throw new NullReferenceException($"{nameof(Sweeper)}->PerformMove(). Spelplanen är inte skapad");

        try
        {
            bool isSquareAMine = IsSelectedSquareAMine(board, selectedColumn, selectedRow);
            if (isSquareAMine)
            {// Rutan är en mina. Uppdatera värdet för enum Cell och avsluta spelet
                Square square = board.GetSquare(selectedColumn, selectedRow);
                square.IsOpenSquare = true;
                square.SquareCell = Cell.CELL_BLOWN_MINE;
            }
            else
            {// Rutan var inte en mina. Eventuellt öppna upp flera rutor

                // Vald ruta ska ha värdet 0 om jag ska öppna upp flera rutor
                if (board.HasSelectedSquareAValue(selectedColumn, selectedRow, Cell.CELL_0))
                {
                    // TODO Kolla igenom detta
                    for (int row = selectedRow; row >= 0; row--)
                    {
                        if (board.HasSelectedSquareAValue(selectedColumn, row, Cell.CELL_0))
                        {
                            OpenColumns(board, selectedColumn, row, directionPositive: true);
                            OpenColumns(board, selectedColumn, row, directionPositive: false);
                        }
                    }

                    for (int row = selectedRow; row < board.NumberOfRows; row++)
                    {
                        if (board.HasSelectedSquareAValue(selectedColumn, row, Cell.CELL_0))
                        {
                            OpenColumns(board, selectedColumn, row, directionPositive: true);
                            OpenColumns(board, selectedColumn, row, directionPositive: false);
                        }
                    }
                }
                else
                {// Kontrollera om rutan som valdes hade ett värde mellan 1 och 8. Då skall rutan visas
                    Square square = board.GetSquare(selectedColumn, selectedRow);
                    if (square != null)
                    {
                        int squareCellvalue = (int)square.SquareCell;
                        if(squareCellvalue >= 1 && squareCellvalue <= 8)
                            square.IsOpenSquare = true;
                    }
                }
            }
        }
        catch (Exception){ }

        return board;
    }


    /// <summary>
    /// Metoden kommer att öppna rutor på spelplanen som antingen har värdet 0 eller 1-8
    /// Om värdet är 0 kommer vi rekursivt anropa metoden OpenColumns igen för nästa kolumn
    /// Om värdet är 1-8 kommer vi öppna rutan men rekursionen slutar
    /// </summary>
    /// <param name="board">Spelplanen</param>    
    /// <param name="column">Kolumn på på spelplanen</param>
    /// <param name="row">Rad på spelplanen</param>
    /// <param name="directionPositive">Anger om nästa kolumn skall vara nuvarande kolumn + 1 eller nuvarande kolumn - 1</param>
    /// <returns>Variabel som är tänkt att användas för att indikera status</returns>
    /// <exception cref="ArgumentNullException">Kastas om referensen till Board är null</exception>
    /// <exception cref="NullReferenceException">Kastas om spelplanen inte är skapad</exception>
    private int OpenColumns(Board board, int column, int row, bool directionPositive)
    {
        if (board == null)
            throw new ArgumentNullException($"{nameof(Sweeper)}->OpenColumns(). Referensen till Board är null");

        if (board.TheBoard == null)
            throw new NullReferenceException($"{nameof(Sweeper)}->OpenColumns(). Spelplanen är inte skapad");


        int step = 0;
        try
        {
            // Hämta önskad ruta på spelplanen
            Square square = board.TheBoard[row, column];
            if (square != null)
            {
                int squareCellvalue = (int)square.SquareCell;

                if (squareCellvalue == 0)
                {// Vi har en ruta som inte är granne med en mina
                    square.IsOpenSquare = true;

                    step = directionPositive == true ? 1 : -1;

                    OpenColumns(board, column + step, row, directionPositive);
                }
                else if (squareCellvalue >= 1 && squareCellvalue <= 8)
                {// Vi har en ruta som är granne med 1-8 minor
                    square.IsOpenSquare = true;
                }
            }
        }
        catch(Exception)
        {
            step = 0;
        }

        return step;
    }
}