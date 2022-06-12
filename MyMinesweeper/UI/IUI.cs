namespace MyMinesweeper.UI;

public interface IUI
{
    void Clear();
    string ReadLine();
    void Write(string text);
    void WriteLine(string text);
}
