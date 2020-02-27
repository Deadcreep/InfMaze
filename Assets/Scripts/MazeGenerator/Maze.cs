namespace MazeGenerators
{
    public class Maze
    {
        public int StartPoint;
        public int EndPoint;
        public Cell[,] Cells;

        public Maze(int height, int width)
        {
            Cells = new Cell[height, width];
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    Cells[x, y] = new Cell(x, y);
                }
            }

        }

    }
}
