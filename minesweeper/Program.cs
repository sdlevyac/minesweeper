namespace minesweeper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Grid myGrid = new Grid(25, 25);
            myGrid.placeMines();
            draw(myGrid);
        }
        static void draw(Grid grid)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(grid.drawGrid());

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
        public bool getMine()
        {
            return mine;
        }
        public void setMine()
        {
            mine = true;
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
        public void placeMines()
        {
            int numberOfMines = (width * height) / 20;
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
        public string drawGrid()
        {
            string output = "";
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //output += !revealed[x, y] ? "." : grid[x, y];
                    //output += grid[x,y] == 1 ? "@" : neighbours[x, y].ToString();
                    //output += squares[x, y].getMine() ? "@" : squares[x, y].getNeighbours().ToString();
                    output += squares[x, y].getNeighbours().ToString();
                }
                output += "\n";
            }
            return output;
        }
        private void calculateNeighbours()
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
