﻿using System.Text;

/*

□

*/

Console.OutputEncoding = Encoding.UTF8;

Tetris game = new Tetris();
game.Start();

struct Element
{
    public int id;
    public string value;

    public Element(int id, string value)
    {
        this.id = id;
        this.value = value;
    }
}

class Tetris
{
    Element[,] field = new Element[20, 10];
    bool stopThreads = false;
    bool figureActive = false;
    int latency = 400;
    int id = 0;

    string Key(int code)
    {
        if (code == 2424832) return "left";
        else if (code == 2555904) return "right";
        else if (code == 2490368) return "up";
        else if (code == 2621440) return "down";
        else if (code == 2097184) return "space";
        return "";
    }

    public Tetris() 
    {
        ClearField();
    }

    private void ClearField()
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for(int j = 0; j < field.GetLength(1); j++)
            {
                field[i, j] = new Element(-1, ".");
            }
        }
    }

    private void CreateFigure()
    {
        if(!figureActive)
        {
            figureActive = true;

        }
    }

    private void FallFigure()
    {

    }

    private void PrintField()
    {
        while(!stopThreads)
        {
            Console.Clear();
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                    Console.Write(field[i, j].value + " ");
                Console.WriteLine();
            }
            Thread.Sleep(latency);
        }        
    }

    private void Input()
    {
        while(!stopThreads)
        {
            ConsoleKeyInfo ch = Console.ReadKey(true);
            int code = ch.GetHashCode();
            Console.WriteLine(code);
        }        
    }    

    public void Start()
    {
        Thread print = new Thread(PrintField);
        Thread input = new Thread(Input);

        print.Start();
        input.Start();
    }
}