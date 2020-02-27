using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MazeGenerators
{
    static class CellHelper
    {
        static public List<Cell> GetNeighbors(Cell cell, Maze maze)
        {
            List<Cell> neighbors = new List<Cell>();
            Cell up = new Cell(cell.x - 2, cell.y);
            Cell right = new Cell(cell.x, cell.y + 2);
            Cell down = new Cell(cell.x + 2, cell.y);
            Cell left = new Cell(cell.x, cell.y - 2);

            Cell[] n = { up, right, down, left };
            for (int i = 0; i < 4; i++)
            {
                if (n[i].x > 0 && n[i].x < maze.Cells.GetUpperBound(0) + 1 && n[i].y > 0 
                    && n[i].y < maze.Cells.GetUpperBound(1) + 1
                    && maze.Cells[n[i].x, n[i].y].isVisited == false)
                {
                    neighbors.Add(new Cell(n[i].x, n[i].y));
                }
            }

            return neighbors; 
        }

        static public Cell GetIntermediateCell(Cell first, Cell second)
        {
            var diffX = second.x - first.x;
            var diffY = second.y - first.y;

            var target = new Cell();
            target.x = first.x + ((diffX != 0) ? (diffX / Math.Abs(diffX)) : 0);
            target.y = first.y + ((diffY != 0) ? (diffY / Math.Abs(diffY)) : 0);

            return target;
        }
    }

    static class MazeHelper
    {
        static public List<Cell> GetUnvisitedCells(Cell[,] maze)
        {
            var cells = maze.OfType<Cell>().ToList().Where(item => item.isVisited == false).ToList();
            return cells;
        }
    }   
}
