using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using Assets.Scripts.Managers;
using MazeGenerators;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public event Action OnEndPointReached;
        public event Action OnStartPointReached;
        public event Action OnCreateTriggerActivated;

        [SerializeField]
        List<Vector3> passableDirections;
        public MazeManager mazeManager;
        public Vector2Int coordInMaze;
        int speed;
        bool isMoving;


        void OnEnable()
        {
            isMoving = false;            
            speed = Settings.Speed;
            mazeManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>().mazeManager;
            coordInMaze = new Vector2Int(0, mazeManager.Maze.StartPoint);
        }

        void FixedUpdate()
        {
            

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                var moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                int x = (int)(Math.Sign(moveDirection.x) * Math.Ceiling(Math.Abs(moveDirection.x)));
                int z = (int)(Math.Sign(moveDirection.z) * Math.Ceiling(Math.Abs(moveDirection.z)));
                Vector3Int moveVector = new Vector3Int(x, 0, z);
                if(!isMoving)
                {
                    StartCoroutine(Move(moveVector));                    
                }
            }
        }


        public IEnumerator Move(Vector3 direction)
        {
            passableDirections = mazeManager.GetPassableDirections(coordInMaze);
            if (passableDirections.Contains(direction))
            {
                var pos = transform.position;
                isMoving = true;
                var step = direction / speed;
                var t = 0;
                while (t < speed)
                {
                    transform.position += step;
                    t++;
                    yield return new WaitForFixedUpdate();
                }
                transform.position = pos + direction;
                isMoving = false;
                coordInMaze += new Vector2Int((int)direction.x, (int)direction.z);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var go = other.gameObject;
            if (Regex.IsMatch(go.name, "Start trigger"))
            {
                coordInMaze = new Vector2Int(0, mazeManager.Maze.StartPoint);
                OnStartPointReached();
                go.SetActive(false);
            }
            if (Regex.IsMatch(go.name, "Create trigger"))
            {
                go.SetActive(false);
                OnCreateTriggerActivated();
            }
            if (Regex.IsMatch(go.name, "End trigger"))
            {
                Debug.Log("End trigger");
                StartCoroutine(Move(Vector3Int.right));
                go.SetActive(false);
                OnEndPointReached();
            }
        }
    }
}