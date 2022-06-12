using MyMinesweeper.Interface;
using MyMinesweeper.UI;
using MyMinesweeper.Utils;
using System.Text;

namespace MyMinesweeper;

/// <summary>
/// Console versionen av Minesweeper
/// </summary>
public class ConsoleMineSweeper
{        
    /// <summary>
    /// Referense till objekt som hanterar utskrift
    /// </summary>
    protected IUI Ui { get; }

    /// <summary>
    /// Referens till objekt som hanterar min röjningen
    /// </summary>
    protected ISweeper Sweeper { get; }

    /// <summary>
    /// Referens till objekt med metoder för att hantera spelplanen
    /// </summary>
    protected IBoardHelper BoardHelper { get; }

    /// <summary>
    /// Namn på spelet
    /// </summary>
    public string GameName { get; set; }

    /// <summary>
    /// Spelplanen
    /// </summary>
    public Board GameBoard { get; set; }


    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="ui">Referens till object för utskrift</param>
    /// <param name="sweeper">Referens till objekt för att uppdatera spelplanen efter att användaren har valt en ruta</param>
    /// <param name="boardHelper">Referens till objekt med olika hjälp metoder för att hantera Board objekt</param>
    public ConsoleMineSweeper(IUI ui, ISweeper sweeper, IBoardHelper boardHelper)
    {
        this.GameName = String.Empty;
        this.GameBoard = null;
        this.Ui = ui;
        this.Sweeper = sweeper;
        this.BoardHelper = boardHelper;
    }


    /// <summary>
    /// Metoden kör spelet
    /// </summary>
    public void Run(string gameName, Board board)
    {
        StringBuilder strBuild = new StringBuilder();

        GameName = gameName;
        GameBoard = board;
        bool runGame = true;
        string input = String.Empty;
        int selectedColumn = 0;
        int selectedRow = 0;
        bool isSelectedSquareOpen = false;
        bool invalidInput = false;
        bool loser = false;
        // Status från ett anrop till PerformMove. Talar om vad det var för någon ruta på spelplanen som valdes
        // Status = 1 om det var en mina.Status = 2 om det var en ruta med 1 - 8 minor som grannar. Annars är Status = 0
        int status = 0; 

        do
        {
            Ui.Clear();

            Ui.WriteLine("Minesweeper is running");
            Ui.WriteLine(GameName);

            // TODO Nu visas allt på spelplanen
            BoardHelper.ShowAllSquaresOnBoard(GameBoard);

            Ui.WriteLine(Environment.NewLine);

            // Visa spelplanen med bara öppnade rutor
            BoardHelper.ShowOnlyOpenSquaresOnBoard(GameBoard);

            if (invalidInput)
                Ui.WriteLine("Ni skrev in felaktiga siffror. Skriv bara in siffror med mellanslag mellan talen");

            if (isSelectedSquareOpen)
                Ui.WriteLine("Ni valde en redan öppnad ruta. Välj en ny ruta");

            Ui.WriteLine("Välj vilken ruta ni vill öppna genom att ange rad och kolumn");
            Ui.WriteLine($"Ange rad från 0 till {board.NumberOfRows - 1} och kolumn från 0 till {board.NumberOfColumns - 1} med mellanslag mellan talen ex (7 12)");

            // Rensa några värden
            invalidInput = false;
            isSelectedSquareOpen = false;
            input = String.Empty;
            input = Ui.ReadLine();

            if (!String.IsNullOrWhiteSpace(input))
            {
                input = input.Trim();

                if (input.StartsWith('q') || input.StartsWith('Q'))
                {// Användare vill avsluta spelet
                    runGame = false;
                }
                else
                {
                    var inputValues = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    if(inputValues != null && inputValues.Length >= 2)
                    { // Vi borde ha två värden. Har vi flera kommer jag att läsa de två första
                        if (!Int32.TryParse(inputValues[0], out selectedRow))
                            invalidInput = true;

                        if (!Int32.TryParse(inputValues[1], out selectedColumn))
                            invalidInput = true;
                    }
                    else
                    {
                        invalidInput = true;
                    }


                    if (!invalidInput)
                    {
                        try
                        {
                            isSelectedSquareOpen = GameBoard.IsSelectedSquareOpen(selectedColumn, selectedRow);

                            if (!isSelectedSquareOpen)
                            {
                                // Kontrollera om användare har valt en ruta med en mina. Om det är så avslutas spelet och alla minor visas
                                if (Sweeper.IsSelectedSquareAMine(GameBoard, selectedColumn, selectedRow))
                                {// Rutan var en mina. Avsluta spelet
                                    loser = true;
                                    runGame = false;
                                }

                                // Uppdatera spelplanen
                                (GameBoard, status) = Sweeper.PerformMove(GameBoard, selectedColumn, selectedRow);
                            }
                        }
                        catch (Exception) { }
                    }                  
                }
            }

        } while (runGame);


        Ui.Clear();
        if (loser)
            Ui.WriteLine("Du lyckades hitta en mina!");

        BoardHelper.ShowOnlySquaresWithMinesOnBoard(GameBoard);

        Ui.WriteLine("Tryck på en tangent för att avsluta MineSweeper");
        Ui.ReadLine();
    }
}