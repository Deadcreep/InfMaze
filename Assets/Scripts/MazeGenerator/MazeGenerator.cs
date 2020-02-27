using System;
using System.Collections.Generic;
using System.Text;

namespace MazeGenerators
{

    public static class MazeGenerator
    {
        public static Maze CreateMaze(int height, int width, int startPoint)
        {
            var visitedCells = new Stack<Cell>();
            Random random = new Random();
            Maze maze = new Maze(height, width);
            maze.Cells[0, startPoint].Type = CellType.Floor;
            maze.StartPoint = startPoint;
            Cell currentCell = maze.Cells[1, startPoint];
            do
            {               
                var neighbors = CellHelper.GetNeighbors(currentCell, maze);
                currentCell.Type = CellType.Floor;
                currentCell.isVisited = true;
                if (neighbors.Count != 0)
                {

                    visitedCells.Push(currentCell);
                    var chosenNeighbour = neighbors[random.Next(neighbors.Count)];
                    var intermediateCell = CellHelper.GetIntermediateCell(currentCell, chosenNeighbour);
                    maze.Cells[intermediateCell.x, intermediateCell.y].Type = CellType.Floor;
                    maze.Cells[intermediateCell.x, intermediateCell.y].isVisited = true;
                    currentCell = maze.Cells[chosenNeighbour.x, chosenNeighbour.y];
                }
                else if (visitedCells.Count > 0)
                {
                    currentCell = visitedCells.Pop();
                }
                else
                {
                    var unvisitedCells = MazeHelper.GetUnvisitedCells(maze.Cells);
                    currentCell = unvisitedCells[random.Next(unvisitedCells.Count)];
                }
            } while (visitedCells.Count != 0);

        randChoice:
            var endCellY = random.Next(1, width - 1);
            if (maze.Cells[width - 2, endCellY].Type == CellType.Floor)
            {

                maze.Cells[width - 1, endCellY].Type = CellType.Floor;

                maze.EndPoint = endCellY;
            }
            else
            {
                goto randChoice;
            }
            return maze;
        }
    }

}
