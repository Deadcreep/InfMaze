using UnityEngine;
using MazeGenerators;
using System;
using System.Collections.Generic;
using Assets.Scripts;

namespace Assets.Scripts.Managers
{
    public class MazeManager : MonoBehaviour
    {
        public GameObject Platform;
        public GameObject Wall;
        public GameObject Floor;
        public GameObject StartTriggerPrefab;
        public GameObject EndTriggerPrefab;
        [SerializeField]
        private GameObject currMazeInstance;
        [SerializeField]
        private GameObject nextMazeInstance;
        public bool MustDestroy;
        public bool mustDestroyNextInstance;

        public Maze Maze;
        private Maze NextMaze;
        private Vector2Int mazeSize;
        private Vector3 currMazeCoords;
        private Vector3 nextMazeCoords;
        private int instancesCount;

        void Start()
        {
            instancesCount = 0;
            mazeSize = Settings.MazeSize;
            StartNewSequence();
        }

        private void DestroyInstance()
        {
            Destroy(currMazeInstance);
        }

        public void DestroyAll()
        {
            Destroy(currMazeInstance);
            Destroy(nextMazeInstance);
        }


        public void StartNewSequence()
        {
            MustDestroy = false;
            currMazeCoords = new Vector3(0, 0, 0);
            nextMazeCoords = new Vector3(15, 0, 0);
            var FirstStartIndex = UnityEngine.Random.Range(1, 14);
            Maze = CreateMazeInstance(currMazeCoords, FirstStartIndex);
            currMazeInstance = nextMazeInstance;
        }

        public void UpdateStatus()
        {
            DestroyInstance();
            currMazeInstance = nextMazeInstance;
            Maze = NextMaze;
        }

        public void ContinueMaze()
        {
            NextMaze = CreateMazeInstance(nextMazeCoords, Maze.EndPoint);
            currMazeCoords = nextMazeCoords;
            nextMazeCoords.x += mazeSize.x;
        }
                       
        private Maze CreateMazeInstance(Vector3 mazeNullCoords, int mazeStart)
        {
            var platformCoords = new Vector3(mazeNullCoords.x + ((int)Math.Floor(mazeSize.x / 2f)), 0,
                                             mazeNullCoords.z + ((int)Math.Floor(mazeSize.y / 2f)));
            nextMazeInstance = new GameObject();
            nextMazeInstance.name = "Instance №" + instancesCount;
            
            nextMazeInstance.transform.position = mazeNullCoords;

            var platform = Instantiate(Platform, position: platformCoords, rotation: Quaternion.identity, parent: nextMazeInstance.transform);
            platform.name = "Platform";
            var maze = MazeGenerator.CreateMaze((int)mazeSize.x, (int)mazeSize.y, mazeStart);

            Instantiate(StartTriggerPrefab, parent: nextMazeInstance.transform,
                                            position: new Vector3(mazeNullCoords.x, 1f, mazeStart),
                                            rotation: Quaternion.identity);
            Instantiate(EndTriggerPrefab, parent: nextMazeInstance.transform,
                                          position: new Vector3(mazeNullCoords.x + mazeSize.x - 1, 1f, maze.EndPoint),
                                          rotation: Quaternion.identity);


            var createTrigger = new GameObject(name: "Create trigger");
            createTrigger.transform.localScale = new Vector3(1, 1, mazeSize.y);
            createTrigger.transform.position = new Vector3(mazeNullCoords.x + mazeSize.x / 2, 1, mazeNullCoords.z + mazeSize.y / 2);
            createTrigger.transform.parent = nextMazeInstance.transform;
            createTrigger.AddComponent<BoxCollider>();
            createTrigger.GetComponent<BoxCollider>().isTrigger = true;
            
            for (int x = 0; x < mazeSize.x; x++)
            {
                for (int y = 0; y < mazeSize.y; y++)
                {
                    GameObject currElement;
                    if (maze.Cells[x, y].Type == CellType.Wall)
                    {
                        currElement = Instantiate(Wall, parent: nextMazeInstance.transform,
                                                        position: new Vector3(x + mazeNullCoords.x, 0.85f, y + mazeNullCoords.z),
                                                        rotation: Quaternion.identity);
                    }
                }
            }
            instancesCount++;
            return maze;
        }

        public virtual List<Vector3> GetPassableDirections(Vector2Int cellCoords)
        {
            
            List<Vector3> passableDirections = new List<Vector3>();
            try
            {
                if (cellCoords.x > 0 && Maze.Cells[cellCoords.x - 1, cellCoords.y].Type == CellType.Floor)
                    passableDirections.Add(new Vector3(-1, 0, 0));

                if (cellCoords.y > 0 && Maze.Cells[cellCoords.x, cellCoords.y - 1].Type == CellType.Floor)
                    passableDirections.Add(new Vector3(0, 0, -1));

                if (cellCoords.x < mazeSize.x - 1 && Maze.Cells[cellCoords.x + 1, cellCoords.y].Type == CellType.Floor)
                    passableDirections.Add(new Vector3(1, 0, 0));

                if (cellCoords.y < mazeSize.y - 1 && Maze.Cells[cellCoords.x, cellCoords.y + 1].Type == CellType.Floor)
                    passableDirections.Add(new Vector3(0, 0, 1));
            }
            catch 
            {
                Debug.Log("Out of range in: " + cellCoords);
            }

            return passableDirections;
        }
    }
}