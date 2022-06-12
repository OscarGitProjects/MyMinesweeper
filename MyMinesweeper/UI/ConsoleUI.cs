namespace MyMinesweeper.UI;

public class ConsoleUI : IUI
{
    public void Clear()
    {
        Console.Clear();

        //Console.CursorVisible = false;
        //Console.SetCursorPosition(0, 0);
    }

    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }

    public void Write(string text)
    {
        Console.Write(text);
    }

    public string ReadLine()
    {
        return Console.ReadLine();
    }
}
