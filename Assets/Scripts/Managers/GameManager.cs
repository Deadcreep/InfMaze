using System;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public GameObject Camera;
        [SerializeField]
        private GameObject PlayerPrefab;
        private GameObject Player;
        private Player playerScript;
        private MazeManager mazeManager;
        private UIManager UIManager;
        private Timer timer;
        private int lifeTime;
        private int passedStagesCount;
        private int totalScore;
                

        private void Start()
        {
            totalScore = 0;
            passedStagesCount = 0;
            mazeManager = GetComponent<MazeManager>();
            Player = Instantiate(PlayerPrefab.gameObject, position: new Vector3(0, 1, mazeManager.Maze.StartPoint), rotation: Quaternion.identity);
            Player.name = "Player";
            UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
            playerScript = Player.GetComponent<Player>();
            playerScript.OnCreateTriggerActivated += OnCreateTriggerActivated;
            playerScript.OnStartPointReached += OnStartPointReached;
            playerScript.OnEndPointReached += OnEndPointReached;

            timer = new Timer(1000);
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            lifeTime = Settings.LifeTime;
        }


        private void OnApplicationQuit()
        {
            timer.Dispose();
        }

        private void Update()
        {
            if (lifeTime == 0)
            {
                OnTimeEnd();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lifeTime -= 1;
            UIManager.OnTimerChanged(lifeTime);
        }

        private void OnEndPointReached()
        {
            lifeTime = Settings.LifeTime;
            mazeManager.UpdateStatus();
            Camera.transform.position += new Vector3(Settings.MazeSize.x, 0, 0);
            passedStagesCount++;
            CalcScore();
        }

        private void OnStartPointReached()
        {
            timer.Enabled = true;
        }

        private void OnCreateTriggerActivated()
        {
            mazeManager.ContinueMaze();
        }

        private void OnTimeEnd()
        {        
            mazeManager.DestroyAll();
            UIManager.AlertEndGame();
            Player.SetActive(false);
            timer.Enabled = false;
        }

        private void CalcScore()
        {
            totalScore = Mathf.RoundToInt(lifeTime * Settings.TimeMultiplier + passedStagesCount * Settings.PassedStageMultiplier);
        }

        public void StartNewGame()
        {
            lifeTime = Settings.LifeTime;
            mazeManager.StartNewSequence();
            Player.transform.position = new Vector3(0, 1, mazeManager.Maze.StartPoint);
            playerScript.coordInMaze = new Vector2Int(0, mazeManager.Maze.StartPoint);
            if (!Player.activeInHierarchy)
                Player.SetActive(true);
            UIManager.StartNewGame();
            Camera.transform.position = new Vector3(-4, 8, -4);
        }

        public void ExitGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}
