namespace MyMinesweeper.Interface;

public interface ISweeper
{
    bool IsSelectedSquareAMine(Board board, int selectedColumn, int selectedRow);
    (Board updatedBoard, int status) PerformMove(Board board, int selectedColumn, int selectedRow);
}