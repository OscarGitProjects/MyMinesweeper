namespace MyMinesweeper.Interface;

public interface ISweeper
{
    bool IsSelectedSquareAMine(Board board, int selectedColumn, int selectedRow);
    Board PerformMove(Board board, int selectedColumn, int selectedRow);
}