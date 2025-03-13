namespace minesweeper
{
    internal class Program
    {
        static bool gameOver = false;
        static Grid myGrid;
        static bool firstTurn = true;
        static int width = 25;
        static int height = 25;
        static void Main(string[] args)
        {
            myGrid = new Grid(width, height);
            myGrid.placeMines();
            
            loop();

        }
        static void draw(Grid grid)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(grid.drawGrid());
        }
        static (int x, int y) getInputs()
        {
            Console.WriteLine("enter x coord:");
            int x = int.Parse(Console.ReadLine());
            Console.WriteLine("enter y coord:");
            int y = int.Parse(Console.ReadLine());
            return (x, y);
        }
        static void loop()
        {
            //first turn

            while (!gameOver)
            {
                draw(myGrid);
                Console.WriteLine("your turn:");
                Console.WriteLine("Options: R, Reveal; F, Flag; A, Reveal All; H, Hide All");
                string choice = Console.ReadLine().ToUpper();

                if (choice == "F")
                {
                    (int x, int y) = getInputs(); 
                    myGrid.getSquare(x, y).addFlag();
                    myGrid.getSquare(x, y).reveal();
                }
                else if (choice == "R")
                {
                    (int x, int y) = getInputs();
                    if (firstTurn)
                    {
                        for (int i = x - 5; i < x + 5; i++)
                        {
                            for (int j = y - 5; j < y + 5; j++)
                            {
                                if (i >= 0 && i < 25 && j >= 0 && j < 25)
                                {
                                    myGrid.getSquare(i, j).clearMine();
                                }
                            }
                        }

                        myGrid.getSquare(x, y).reveal();
                        
                        firstTurn = false;
                    }
                    else
                    {
                        myGrid.getSquare(x, y).reveal();
                    }
                    myGrid.calculateNeighbours();
                }
                else if (choice == "A")
                {
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            myGrid.getSquare(i, j).reveal();
                        }
                    }
                }
                else if (choice == "H")
                {
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            myGrid.getSquare(i, j).hide();
                        }
                    }
                }
            }
        }
    }
    class Square
    {
        private bool mine;
        private bool revealed;
        private int neighbours;
        private bool flag;
        public Square(bool mine, bool revealed, int neighbours, bool flag)
        {
            this.mine = mine;
            this.revealed = revealed;
            this.neighbours = neighbours;
            this.flag = flag;
        }
        //hello
        public bool getMine()
        {
            return mine;
        }
        public void setMine()
        {
            mine = true;
        }
        public void clearMine()
        {
            mine = false;
        }
        public bool getFlag()
        {
            return flag;
        }
        public void addFlag()
        {
            flag = true;
        }
        public void removeFlag()
        {
            flag = false;
        }
        public int getNeighbours()
        {
            return neighbours;
        }
        public void setNeighbours(int _neighbours)
        {
            neighbours = _neighbours;
        }
        public void reveal()
        {
            revealed = true;
        }
        public void hide()
        {
            revealed = false;
        }
        public bool getRevealed()
        {
            return revealed;
        }
    }
    class Grid
    {
        private int width;
        private int height;
        private int[,] grid;
        private Square[,] squares;
        private bool[,] revealed;
        private int[,] neighbours;
        private Random rnd;
        public Grid(int width, int height)
        {
            this.width = width;
            this.height = height;
            //grid = new int[width, height];
            squares = new Square[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    squares[i, j] = new Square(false, false, 0, false);
                }
            }
            //revealed = new bool[width, height];
            //neighbours = new int[width, height];
            rnd = new Random(1234);
        }
        public Square getSquare(int x, int y)
        {
            return squares[x, y];
        }
        public void placeMines()
        {
            int numberOfMines = (width * height) / 10;
            for (int i = 0; i < numberOfMines; i++)
            {
                int mineX = rnd.Next(0, width);
                int mineY = rnd.Next(1, height);
                
                
                while (squares[mineX,mineY].getMine())//(grid[mineX, mineY] != 0)
                {
                    mineX = rnd.Next(0, width);
                    mineY = rnd.Next(1, height);
                }
                //grid[mineX, mineY] = 1;
                squares[mineX, mineY].setMine();


            }
            calculateNeighbours();
        }
        public void revealAll()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    squares[i, j].reveal();
                }
            }
        }
        public string drawGrid()
        {
            string output = "\n   ";
            for (int y = 0; y < height; y++)
            {
                output += y.ToString("00");
                output += " ";
            }
            output += "\n";
            for (int x = 0; x < width; x++)
            {
                output += x.ToString("00");
                output += ": ";
                for (int y = 0; y < height; y++)
                {
                    //output += !revealed[x, y] ? "." : grid[x, y];
                    //output += grid[x,y] == 1 ? "@" : neighbours[x, y].ToString();
                    //output += squares[x, y].getMine() ? "@" : squares[x, y].getNeighbours().ToString();
                    if (squares[x, y].getRevealed())
                    {
                        if ((squares[x, y].getMine() || squares[x, y].getFlag()))
                        {
                            output += squares[x, y].getMine() ? "@" : "F";
                        }
                        else
                        {
                            output += squares[x, y].getNeighbours().ToString();
                        }
                        
                    }
                    else
                    {
                        output += ".";
                    }
                    output += "| ";
                }
                output += "\n";
            }
            return output;
        }
        public void calculateNeighbours()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int num = 0;
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx >= 0 && nx < width &&
                                ny >= 0 && ny < height &&
                                !(x == nx && y == ny))
                            {
                                num += squares[nx,ny].getMine() ? 1 : 0;
                            }
                        }
                    }
                    //neighbours[x, y] = num;
                    squares[x, y].setNeighbours(num);
                }
            }
        }
    }
}
