using System;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            bool coordinates = false;

            //Check for Switches
            if (args.Length == 0)
            {
                Console.WriteLine("Please Enter 1 for enabling Y coordinates or 0 for disabling Y coordinates using --show-coordinates");
            }
            else
            {

                var command = args[0];

                switch (command)
                {
                    case "--show-coordinates" when args.Length == 2 && args[1] == "1":
                        coordinates = true;
                        break;
                    case "--show-coordinates" when args.Length == 2 && args[1] == "0":
                        coordinates = false;
                        break;
                    default:
                        Console.WriteLine("Invalid Arguments");
                        break;
                }

                //Obtain Console Height and Width
                int height = Console.WindowHeight;
                int width = Console.WindowWidth;

                //Initialize GameOfLife instance with Console window Height and Width
                GameOfLife game = new GameOfLife(height, width, coordinates);

                //Infinite Loop to be switched out with a Proper Clock if refresh rate is to be altered via a switch
                while (true)
                {
                    game.Refresh();

                    System.Threading.Thread.Sleep(50);
                }
            }
        }
    }

    internal class GameOfLife
    {
        //Declare Width and Height of Game
        private int height;
        private int width;

        //Declare 2D Array for Cells
        private bool[,] cells;

        //Declare Coordinates Switch (Currently set to only show Y coordinate however the code to show X is commented below)
        private bool coordinates;

        //GameOfLife Constructor which takes in a height and width as parameters for the cell matrix
        public GameOfLife(int height, int width, bool coordinates = false)
        {
            this.height = Console.WindowHeight;
            this.width = Console.WindowWidth;
            this.coordinates = coordinates;

            cells = new bool[height, width];
            InitializeMatrix();
        }

        /// <summary>
        /// Initializes Cell Matrix with Random Values by default
        /// </summary>
        private void InitializeMatrix()
        {
            Random random = new Random();
            int num;
            
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    num = random.Next(2);
                    cells[i, j] = (num == 0) ? false : true;
                }
            }
        }

        /// <summary>
        /// Stores cells of current growth state in a string buffer and writes it to screen.
        /// </summary>
        private void Render()
        {
            string buffer = "";
            for (int i = 0; i <= height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i != height)
                    {
                        if (coordinates && j == 0)
                        {
                            buffer += i;
                        }
                        buffer += cells[i, j] ? "X" : " ";
                    }
                    else
                    {
                        //X Axis Coordinates
                        //if (coordinates)
                        //{
                        //    if (j == 0)
                        //        buffer += " ";
                        //    buffer += j;
                        //}
                    }
                }
                buffer += "\n";
            }
            Console.SetCursorPosition(0, Console.WindowTop);
            Console.Write(buffer.TrimEnd('\n'));
        }

        /// <summary>
        /// Gets a count of an individual cell's neighboring cells.
        /// </summary>
        /// <param name="x">x coordinate of cell</param>
        /// <param name="y">y coordinate of cell</param>
        /// <returns>Number of neighbors</returns>
        private int GetNeighbors(int x, int y)
        {
            int neighbors = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (!((i < 0 || j < 0) || (i >= height || j >= width)))
                    {
                        if (!((i == x) && (j == y)))
                        {
                            if (cells[i, j] == true) neighbors++;
                        }
                    }
                }
            }
            return neighbors;
        }

        /// <summary>
        /// Gets a count of an individual cell's neighboring cells.
        /// </summary>
        private void Grow()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int neighbors = GetNeighbors(i, j);

                    if (cells[i, j])
                    {
                        if (neighbors < 2 || neighbors > 3)
                        {
                            cells[i, j] = false;
                        }
                    }
                    else
                    {
                        if (neighbors == 3)
                        {
                            cells[i, j] = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Advances screen with new growth changes.
        /// </summary>
        public void Refresh()
        {
            Render();
            Grow();
        }
    }
}