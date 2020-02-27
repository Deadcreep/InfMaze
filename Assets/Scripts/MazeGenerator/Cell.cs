namespace MazeGenerators
{
    public class Cell
    {
        public CellType Type;
        public bool isVisited;
        public int x;
        public int y;

        public Cell(int x, int y)
        {
            Type = CellType.Wall;
            this.x = x;
            this.y = y;
            this.isVisited = false;
        }

        public Cell() { }
    }


    public enum CellType
    {
        Wall,
        Floor
    }
}
