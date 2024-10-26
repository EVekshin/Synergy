using System;
using System.Threading;

class Tetris
{
    private const int WIDTH = 12;
    private const int HEIGHT = 20;
    private static int[,] field = new int[HEIGHT, WIDTH];
    private static Figure currentFigure;
    private static bool gameOver = false;
    private static int score = 0;

    static void Main()
	{
	    bool playAgain = true;
	    
	    while (playAgain)
	    {
	        Console.CursorVisible = false;
	        InitField();
	        gameOver = false;
	        score = 0;
	        
	        while (!gameOver)
	        {
	            if (currentFigure == null)
	            {
	                currentFigure = new Figure(WIDTH / 2 - 1);
	                if (!CanPlace(currentFigure))
	                {
	                    gameOver = true;
	                    break;
	                }
	            }

	            DrawField();
	            ProcessInput();
	            
	            if (!CanMove(currentFigure, 0, 1))
	            {
	                PlaceFigure(currentFigure);
	                CheckLines();
	                currentFigure = null;
	            }
	            else
	            {
	                currentFigure.Y++;
	            }

	            Thread.Sleep(300);
	        }

	        Console.Clear();
	        Console.WriteLine($"Game Over! Score: {score}");
	        Console.WriteLine("\nWould you like to play again? (Y/N)");
	        
	        while (true)
	        {
	            var key = Console.ReadKey(true).Key;
	            if (key == ConsoleKey.Y)
	            {
	                Console.Clear();
	                break;
	            }
	            else if (key == ConsoleKey.N)
	            {
	                playAgain = false;
	                break;
	            }
	        }
	    }
	}

    class Figure
	{
	    public int X { get; set; }
	    public int Y { get; set; }
	    public int[,] Shape { get; set; }
	    public ConsoleColor Color { get; set; }

	    private static readonly int[][,] Tetrominoes = new int[][,]
	    {
	        // I
	        new int[,] {
	            {0, 0, 0, 0},
	            {1, 1, 1, 1},
	            {0, 0, 0, 0},
	            {0, 0, 0, 0}
	        },
	        // O
	        new int[,] {
	            {1, 1},
	            {1, 1}
	        },
	        // T
	        new int[,] {
	            {0, 1, 0},
	            {1, 1, 1},
	            {0, 0, 0}
	        },
	        // S
	        new int[,] {
	            {0, 1, 1},
	            {1, 1, 0},
	            {0, 0, 0}
	        },
	        // Z
	        new int[,] {
	            {1, 1, 0},
	            {0, 1, 1},
	            {0, 0, 0}
	        },
	        // J
	        new int[,] {
	            {1, 0, 0},
	            {1, 1, 1},
	            {0, 0, 0}
	        },
	        // L
	        new int[,] {
	            {0, 0, 1},
	            {1, 1, 1},
	            {0, 0, 0}
	        }
	    };

	    private static readonly ConsoleColor[] Colors = new ConsoleColor[]
	    {
	        ConsoleColor.Cyan,    // I
	        ConsoleColor.Yellow,  // O
	        ConsoleColor.Magenta, // T
	        ConsoleColor.Green,   // S
	        ConsoleColor.Red,     // Z
	        ConsoleColor.Blue,    // J
	        ConsoleColor.DarkYellow  // L
	    };

	    public Figure(int startX)
	    {
	        X = startX;
	        Y = 0;
	        
	        Random rand = new Random();
	        int figureIndex = rand.Next(Tetrominoes.Length);
	        
	        Shape = CloneShape(Tetrominoes[figureIndex]);
	        Color = Colors[figureIndex];
	    }

	    private int[,] CloneShape(int[,] original)
	    {
	        int rows = original.GetLength(0);
	        int cols = original.GetLength(1);
	        int[,] clone = new int[rows, cols];
	        
	        for (int i = 0; i < rows; i++)
	            for (int j = 0; j < cols; j++)
	                clone[i, j] = original[i, j];
	                
	        return clone;
	    }

	    public void Rotate()
	    {
	        int rows = Shape.GetLength(0);
	        int cols = Shape.GetLength(1);
	        int[,] newShape = new int[cols, rows];

	        for (int i = 0; i < rows; i++)
	            for (int j = 0; j < cols; j++)
	                newShape[j, rows - 1 - i] = Shape[i, j];

	        Shape = newShape;
	    }
	}

    static void InitField()
    {
        for (int i = 0; i < HEIGHT; i++)
            for (int j = 0; j < WIDTH; j++)
                field[i, j] = (j == 0 || j == WIDTH - 1 || i == HEIGHT - 1) ? 1 : 0;
    }

    static void DrawField()
	{
	    Console.SetCursorPosition(0, 0);
	    Console.WriteLine($"Score: {score}");

	    int[,] tempField = (int[,])field.Clone();
	    if (currentFigure != null)
	    {
	        for (int i = 0; i < currentFigure.Shape.GetLength(0); i++)
	            for (int j = 0; j < currentFigure.Shape.GetLength(1); j++)
	                if (currentFigure.Shape[i, j] == 1)
	                    tempField[currentFigure.Y + i, currentFigure.X + j] = 2;
	    }

	    for (int i = 0; i < HEIGHT; i++)
	    {
	        for (int j = 0; j < WIDTH; j++)
	        {
	            if (tempField[i, j] == 2 && currentFigure != null)
	            {
	                Console.ForegroundColor = currentFigure.Color;
	                Console.Write("▒");
	            }
	            else
	            {
	                Console.ForegroundColor = ConsoleColor.White;
	                Console.Write(tempField[i, j] switch
	                {
	                    0 => " ",
	                    1 => "█",
	                    _ => " "
	                });
	            }
	        }
	        Console.WriteLine();
	    }
	    Console.ForegroundColor = ConsoleColor.White;
	}

    static void ProcessInput()
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (CanMove(currentFigure, -1, 0))
                        currentFigure.X--;
                    break;
                case ConsoleKey.RightArrow:
                    if (CanMove(currentFigure, 1, 0))
                        currentFigure.X++;
                    break;
                case ConsoleKey.DownArrow:
                    if (CanMove(currentFigure, 0, 1))
                        currentFigure.Y++;
                    break;
                case ConsoleKey.UpArrow:
                    var tempFigure = new Figure(currentFigure.X) 
                    { 
                        Y = currentFigure.Y,
                        Shape = (int[,])currentFigure.Shape.Clone() 
                    };
                    tempFigure.Rotate();
                    if (CanPlace(tempFigure))
                        currentFigure.Rotate();
                    break;
            }
        }
    }

    static bool CanMove(Figure figure, int deltaX, int deltaY)
    {
        for (int i = 0; i < figure.Shape.GetLength(0); i++)
        {
            for (int j = 0; j < figure.Shape.GetLength(1); j++)
            {
                if (figure.Shape[i, j] == 1)
                {
                    int newX = figure.X + j + deltaX;
                    int newY = figure.Y + i + deltaY;
                    if (newX < 0 || newX >= WIDTH || newY >= HEIGHT || field[newY, newX] == 1)
                        return false;
                }
            }
        }
        return true;
    }

    static bool CanPlace(Figure figure)
    {
        return CanMove(figure, 0, 0);
    }

    static void PlaceFigure(Figure figure)
    {
        for (int i = 0; i < figure.Shape.GetLength(0); i++)
            for (int j = 0; j < figure.Shape.GetLength(1); j++)
                if (figure.Shape[i, j] == 1)
                    field[figure.Y + i, figure.X + j] = 1;
    }

    static void CheckLines()
    {
        for (int i = HEIGHT - 2; i >= 0; i--)
        {
            bool lineFull = true;
            for (int j = 1; j < WIDTH - 1; j++)
                if (field[i, j] == 0)
                {
                    lineFull = false;
                    break;
                }

            if (lineFull)
            {
                score += 100;
                for (int k = i; k > 0; k--)
                    for (int j = 1; j < WIDTH - 1; j++)
                        field[k, j] = field[k - 1, j];
            }
        }
    }
}