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
        //hello
        public bool getMine()
        {
            return mine;
        }
        public void setMine()
        {
            mine = true;
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
            grid = new int[width, height];
            revealed = new bool[width, height];
            neighbours = new int[width, height];
            rnd = new Random(1234);
        }
        public void placeMines()
        {
            int numberOfMines = (width * height) / 10;
            for (int i = 0; i < numberOfMines; i++)
            {
                int mineX = rnd.Next(0, width);
                int mineY = rnd.Next(1, height);
                
                
                while (grid[mineX, mineY] != 0)
                {
                    mineX = rnd.Next(0, width);
                    mineY = rnd.Next(1, height);
                }
                grid[mineX, mineY] = 1;


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
                    output += grid[x,y] == 1 ? "@" : neighbours[x, y].ToString();
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
                                num += grid[nx, ny];
                            }
                        }
                    }
                    neighbours[x, y] = num;
                }
            }
        }
    }
}
