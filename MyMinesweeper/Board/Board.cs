namespace MyMinesweeper;

public enum Cell
{
    CELL_INVALID = -1,
    CELL_0 = 0,
    CELL_1 = 1,
    CELL_2 = 2,
    CELL_3 = 3,
    CELL_4 = 4,
    CELL_5 = 5,
    CELL_6 = 6,
    CELL_7 = 7,
    CELL_8 = 8,

    CELL_UNREVEALED_EMPTY = 10,
    CELL_UNREVEALED_MINE = 11,
    CELL_BLOWN_MINE = 12,
}


/// <summary>
/// Klass som representerar spelplanen
/// Rutorna på spelplanen representeras av klassen Square. Data i varje ruta representeras av enum Cell
/// Square[rader, kolumner]
/// </summary>
public class Board
{
    /// <summary>
    /// Spelplanen
    /// </summary>
    public Square[,] TheBoard = null;

    /// <summary>
    /// Antal kolumner på spelplanen
    /// </summary>
    public int NumberOfColumns { get; protected set; } = 0;

    /// <summary>
    /// Antal rader på spelplanen
    /// </summary>
    public int NumberOfRows { get; protected set; } = 0;


    /// <summary>
    /// Konstruktor
    /// </summary>
    public Board()
    {
    }

    /// <summary>
    /// Konstruktor
    /// Skapar en ny spelplan
    /// </summary>
    /// <param name="numberOfColumns">Antal kolumner som spelplanen skall ha</param>
    /// <param name="numberOfRows">Antal rader som spelplanen skall ha</param>
    public Board(int numberOfColumns, int numberOfRows)
    {
        CreateBoard(numberOfColumns, numberOfRows);

        // Skapa data i spelplanens rutor
        // Skapa minor på spelplanen
        CreateMinesOnBoard();

        // Räkna antalet minor som en ruta har som granne
        CalculateNumberOfMinesAsNeigbours();

        ConvertNumberOfMinesAsNeigboursToEnum();
    }


    /// <summary>
    /// Metoden skapare en ny spelplan med önskat antal kolumner och rader
    /// Finns det en spelplan, kommer den raderas
    /// </summary>
    /// <param name="numberOfColumns">Antal kolumner som spelplanen skall ha</param>
    /// <param name="numberOfRows">Antal rader som spelplanen skall ha</param>
    public void CreateBoard(int numberOfColumns, int numberOfRows)
    {
        NumberOfColumns = numberOfColumns;
        NumberOfRows = numberOfRows;

        TheBoard = new Square[numberOfRows, numberOfColumns];

        // Allokera spelplanen
        for (int rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
        {
            for (int colIndex = 0; colIndex < numberOfColumns; colIndex++)
            {
                TheBoard[rowIndex, colIndex] = new Square(Cell.CELL_0, $"{rowIndex}, {colIndex}");
            }
        }
    }


    /// <summary>
    /// Metoden kommer att konvertera antalet minor som grannar till rätt enum Cell
    /// </summary>
    private void ConvertNumberOfMinesAsNeigboursToEnum()
    {
        for (int rowIndex = 0; rowIndex < NumberOfRows; rowIndex++)
        {
            for (int colIndex = 0; colIndex < NumberOfColumns; colIndex++)
            {
                try
                {
                    Square square = TheBoard[rowIndex, colIndex];
                    if(square != null && square.SquareCell != Cell.CELL_UNREVEALED_MINE)
                    {
                        square.SquareCell = (Cell)square.MineAsNeighbourCount;
                    }
                }
                catch (Exception) { }
            }
        }
    }


    /// <summary>
    /// Metoden kontrollerar rutor på spelplanen och hur många minor som rutan har som grannar
    /// </summary>
    private void CalculateNumberOfMinesAsNeigbours()
    {
        // Nu vill jag gå igenom alla rutor i spelplanen som inte är minor.
        // Dessa rutor skall ange hur många minor det finns runt denna ruta
        Square square = null;

        for (int rowIndex = 0; rowIndex < NumberOfRows; rowIndex++)
        {
            for (int colIndex = 0; colIndex < NumberOfColumns; colIndex++)
            {
                try
                {
                    square = TheBoard[rowIndex, colIndex];
                    if (square != null)
                    {
                        if (square.SquareCell == Cell.CELL_UNREVEALED_MINE)
                        {// Rutan är en mina. Uppdatera grannarna runt minan

                            // Jag gör inga kontroller av om index är ok. Det hanteras i metoden UpdateNeighbourCount

                            // Uppdatera grannarna på samma rad
                            // Uppdatera rutan till vänster
                            UpdateNeighbourCount(rowIndex, colIndex - 1);
                            // Uppdatera rutan till höger
                            UpdateNeighbourCount(rowIndex, colIndex + 1);


                            // Uppdatera grannarna på raden ovan
                            // Uppdatera ovanför
                            UpdateNeighbourCount(rowIndex - 1, colIndex);
                            // Uppdatera rutan till vänster
                            UpdateNeighbourCount(rowIndex - 1, colIndex - 1);
                            // Uppdatera rutan till höger
                            UpdateNeighbourCount(rowIndex - 1, colIndex + 1);


                            // Uppdatera grannar på raden under
                            // Uppdatera ovanför
                            UpdateNeighbourCount(rowIndex + 1, colIndex);
                            // Uppdatera rutan till vänster
                            UpdateNeighbourCount(rowIndex + 1, colIndex - 1);
                            // Uppdatera rutan till höger
                            UpdateNeighbourCount(rowIndex + 1, colIndex + 1);
                        }
                    }
                }
                catch (Exception)
                { }
            }
        }
    }


    /// <summary>
    /// Metoden kontrollerar att rutan på spelplanen inte är en mina. Då uppdateras värdet på antal grannar som är minor med ett
    /// </summary>
    /// <param name="row">Vilken rad sökt ruta har på spelplanen</param>
    /// <param name="column">Vilken column sökt ruta har på spelplanen</param>
    /// <returns>true om värdet på antal grannar som är minor har uppdaterats. Annars returneras false</returns>
    private bool UpdateNeighbourCount(int row, int column)
    {
        bool updatedValue = false;
       
        try
        {
            Square square = TheBoard[row, column];
            if (square != null)
            {
                if (square.SquareCell != Cell.CELL_UNREVEALED_MINE)
                {
                    updatedValue = true;
                    square.MineAsNeighbourCount++;
                }
            }
        }
        catch (Exception) { }

        return updatedValue;
    }


    /// <summary>
    /// Metoden skapar slumpmässigt utplacerade minor
    /// </summary>
    private void CreateMinesOnBoard()
    {        
        // Skapa minor
        Random rnd = new Random();
        int numberOfMines = 0;
        int maxNumberOfMines = ((NumberOfRows * NumberOfColumns) / 10);
        int randRow = 0;
        int randCol = 0;
        Square square = null;

        while (numberOfMines < maxNumberOfMines)
        {
            try
            {
                do
                {
                    randRow = rnd.Next(NumberOfRows);
                    randCol = rnd.Next(NumberOfColumns);
                    square = TheBoard[randRow, randCol];
                    if (square == null)
                        break;
                }
                while (square.SquareCell == Cell.CELL_UNREVEALED_MINE);// Kontrollera att vi inte redan har en mina i rutan

                if (square != null)
                {// lägg till en mina på spelplanen
                    square.SquareCell = Cell.CELL_UNREVEALED_MINE;
                    numberOfMines++;
                }
            }
            catch (Exception)
            { }
        }
    }


    /// <summary>
    /// Metoden returnerar en vald ruta på spelplanen
    /// </summary>
    /// <param name="column">Kolumn för vald ruta</param>
    /// <param name="row">Rad för vald ruta</param>
    /// <returns>Vald ruta på spelplanen eller null</returns>
    /// <exception cref="NullReferenceException">Kastas om spelplanen inte är skapad dvs är null</exception>
    /// <exception cref="IndexOutOfRangeException">Kastas om man försöker att läsa en ruta utanför spelplanen</exception>
    public Square GetSquare(int column, int row)
    {
        if (TheBoard == null)
            throw new NullReferenceException($"{nameof(Board)}->GetSquare(). Spelplanen är null");

        Square? selectedSquare = null;

        try
        {
            selectedSquare = TheBoard[row, column];
        }
        catch (IndexOutOfRangeException)
        {
            throw;
        }

        return selectedSquare;
    }


    /// <summary>
    /// Metoden kontrollerar om vald ruta på spelplanen är öppnad eller inte
    /// </summary>
    /// <param name="selectedColumn">Vald kolumn på spelplanen</param>
    /// <param name="selectedRow">Vald rad på spelplanen</param>
    /// <returns>true om vald ruta är öppnad. Annars returneras false</returns>
    /// <exception cref="NullReferenceException">Kastas om spelplanen inte är skapad</exception>
    /// <exception cref="IndexOutOfRangeException">Kastas om vi försöker att läsa en ruta utanför spelplanen</exception>
    public bool IsSelectedSquareOpen(int selectedColumn, int selectedRow)
    {
        if (TheBoard == null)
            throw new NullReferenceException($"{nameof(Board)}->IsSelectedSquareOpen(). Spelplanen är inte skapad");

        Square? selectedSquare = GetSquare(selectedColumn, selectedRow);
        if (selectedSquare != null)
        {
            if (selectedSquare.IsOpenSquare)
                return true;
        }

        return false;
    }


    /// <summary>
    /// Metoden kontrollerar om vald ruta på spelplanen är en gömd mina eller inte
    /// </summary>
    /// <param name="selectedColumn">Vald kolumn på spelplanen</param>
    /// <param name="selectedRow">Vald rad på spelplanen</param>
    /// <returns>true om vald ruta är en dold minaöppnad. Annars returneras false</returns>
    /// <exception cref="NullReferenceException">Kastas om spelplanen inte är skapad</exception>
    /// <exception cref="IndexOutOfRangeException">Kastas om vi försöker att läsa en ruta utanför spelplanen</exception>
    public bool IsSelectedSquareAUnrevealedMine(int selectedColumn, int selectedRow)
    {
        if (TheBoard == null)
            throw new NullReferenceException($"{nameof(Board)}->IsSelectedSquareAUnrevealedMine(). Spelplanen är inte skapad");

        Square? selectedSquare = GetSquare(selectedColumn, selectedRow);
        if (selectedSquare != null)
        {
            if (selectedSquare.SquareCell == Cell.CELL_UNREVEALED_MINE)
                return true;
        }

        return false;
    }


    /// <summary>
    /// Metoden kontrollerar om vald ruta på spelplanen har samma value som enum Cell
    /// </summary>
    /// <param name="selectedColumn">Vald kolumn på spelplanen</param>
    /// <param name="selectedRow">Vald rad på spelplanen</param>
    /// <param name="cell">enum Cell med sökt värde</param>
    /// <returns>true om vald ruta har samma value som enum Cell. Annars returneras false</returns>
    /// <exception cref="NullReferenceException">Kastas om spelplanen inte är skapad</exception>
    /// <exception cref="IndexOutOfRangeException">Kastas om vi försöker att läsa en ruta utanför spelplanen</exception>
    public bool HasSelectedSquareAValue(int selectedColumn, int selectedRow, Cell cell)
    {
        if (TheBoard == null)
            throw new NullReferenceException($"{nameof(Board)}->HasSelectedSquareAValue(). Spelplanen är inte skapad");

        Square? selectedSquare = GetSquare(selectedColumn, selectedRow);
        if (selectedSquare != null)
        {
            if (selectedSquare.SquareCell == cell)
                return true;
        }

        return false;
    }

}
