using System.Text;

/*
 □□   □   □
□□    □  □□□
     □□
□□   
□□   □□□□
*/

Console.OutputEncoding = Encoding.UTF8;

Tetris game = new Tetris();
game.Start();

struct Element
{
    public int id;
    public string value;
    public int type;
    public Side rotation;
    public Side direction;

    public Element(int id, string value, int type, Side rotation)
    {
        this.id = id;
        this.value = value;
        this.type = type;
        this.rotation = rotation;
    }
    
    public Element(int id, string value, int type, Side rotation, Side direction)
    {
        this.id = id;
        this.value = value;
        this.type = type;
        this.rotation = rotation;
        this.direction = direction;
    }
}

enum Side
{
    Vertical, Horizontal, Left, Right, Up, Down, Null
}

class Tetris
{
    Element[,] field = new Element[20, 10];
    bool stopThreads = false;
    bool figureActive = false;
    int latency = 400;
    int id = 0;
    int currentFigure = -1;

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
                field[i, j] = new Element(-1, ".", -1, Side.Null);
            }
        }
    }

    private void RotateFigure()
    {

    }

    public void MoveFigure(Side direction)
    {
        if(direction == Side.Left)
        {
            for (int i = 0; i < field.GetLength(0); i++)
                if (field[i, 0].value != "." && field[i, 1].value != ".") break;
               // if (field[i, 0].id == id - 1 || field[i, 0].value != "." && field[i, 1].id == id -1) break;

            for (int j = 0; j < field.GetLength(1); j++)
            {
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    if (j + 1 != field.GetLength(1) && field[i, j].value == ".")
                    {
                        if (field[i, j + 1].value != "." && field[i, j + 1].id == id - 1)
                        {
                            field[i, j] = field[i, j + 1];
                            field[i, j + 1] = new Element(-1, ".", -1, Side.Null);
                        }
                    }
                }
            }
        }
    }

    private void CreateFigure()
    {
        if(!figureActive)
        {
            figureActive = true;
            Random random = new Random();
            //int r = random.Next(1, 6);
            int r = 1;

            if(r == 1)
            {
                //  ■ | ■ ■ ■ ■ 
                //  ■ |
                //  ■ | 
                //  ■ |

                for (int i = 0; i < 4; i++)
                    field[i, 4] = new Element(id, "■", 1, Side.Vertical);
                id++;
                currentFigure = 1;
            }
            else if(r == 2)
            {
                //   ■ | ■   |       |       | ■ ■ | ■ ■ |       |
                //   ■ | ■   |     ■ | ■     | ■   |   ■ | ■ ■ ■ | ■ ■ ■
                // ■ ■ | ■ ■ | ■ ■ ■ | ■ ■ ■ | ■   |   ■ | ■     |     ■
            }
            else if(r == 3)
            {
                //       |       |   ■ | ■
                //   ■   | ■ ■ ■ | ■ ■ | ■ ■ 
                // ■ ■ ■ |   ■   |   ■ | ■
            }
            else if(r == 4)
            {
                //       |       |   ■ | ■ 
                //   ■ ■ | ■ ■   | ■ ■ | ■ ■
                // ■ ■   |   ■ ■ | ■   |   ■
            }
            else if(r == 5)
            {
                // ■ ■
                // ■ ■
            }
        }
    }

    private bool FallFigure()
    {
        bool falled = false;
        for (int j = field.GetLength(1)-1; j > 0; j--)
        { 
            for (int i = field.GetLength(0)-1; i > 0; i--)
            {
                if (field[i, j].value == "." && field[i - 1, j].value != ".")
                {
                    field[i, j] = field[i - 1, j];
                    field[i - 1, j] = new Element(-1, ".", -1, Side.Null);
                    falled = true;
                }
            }
        }
        return falled;
    }

    private void PrintField()
    {
        while(!stopThreads)
        {
            CreateFigure();
            Console.Clear();
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                    Console.Write(field[i, j].value + " ");
                Console.WriteLine();
            }
            Thread.Sleep(latency);

            if (!FallFigure())
            {
                DeactivateFigures();
            }
        }        
    }

    private void DeactivateFigures()
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                field[i, j].type = -1;
                field[i, j].id = -1;
            }
        }
        figureActive = false;
        currentFigure = -1;
    }

    private void Input()
    {
        while(!stopThreads)
        {
            ConsoleKeyInfo ch = Console.ReadKey(true);
            int code = ch.GetHashCode();

            if (Key(code) == "up") RotateFigure();
            else if (Key(code) == "left") MoveFigure(Side.Left);
            else if (Key(code) == "right") MoveFigure(Side.Right);
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