namespace MyMinesweeper.Utils
{
    public interface IBoardHelper
    {
        string GetSymbol(Square square);
        void ShowAllSquaresOnBoard(Board board);
        void ShowOnlyOpenSquaresOnBoard(Board board);
        void ShowOnlySquaresWithMinesOnBoard(Board board);
    }
}