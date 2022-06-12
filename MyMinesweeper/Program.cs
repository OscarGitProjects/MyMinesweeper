// See https://aka.ms/new-console-template for more information
using System;

namespace MyMinesweeper;

public class Program
{
    static void Main(string[] args)
    {
        Program program = new Program();

        //Console.WriteLine("Hello, World!");
        program.RunMineSweeper();        
    }

    private void RunMineSweeper()
    {
        var startup = new StartUp();
        startup.SetUp();
    }
}