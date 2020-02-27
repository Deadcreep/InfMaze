using System;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public Camera Camera;
        [SerializeField]
        public GameObject Player;
        private Player playerScript;
        public MazeManager mazeManager;
        private UIManager UIManager;
        private Timer timer;
        private int lifeTime;
        private int passedStagesCount;
        private int totalScore_;
        public int TotalScore { get => totalScore_; private set => totalScore_ = value; }



        private void OnEnable()
        {
            TotalScore = 0;
            passedStagesCount = 0;
            mazeManager = ScriptableObject.CreateInstance<MazeManager>();
            Player = Instantiate(Resources.Load("Prefabs/Player") as GameObject, position: new Vector3(0, 1, mazeManager.Maze.StartPoint), rotation: Quaternion.identity);
            Player.name = "Player";
            this.Camera = Camera.main;
            playerScript = Player.GetComponent<Player>();
            playerScript.OnCreateTriggerActivated += OnCreateTriggerActivated;
            playerScript.OnStartPointReached += OnStartPointReached;
            playerScript.OnEndPointReached += UpdateGameStatus;
            
            timer = new Timer(1000);
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            lifeTime = Settings.LifeTime;
        }

        private void Start()
        {
            UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
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

        private void UpdateGameStatus()
        {
            lifeTime = Settings.LifeTime;
            mazeManager.UpdateStatus();
            Camera.transform.position += new Vector3(Settings.MazeSize.x, 0, 0);
            passedStagesCount++;
            CalcScore();
            UIManager.OnScoreChanged(TotalScore);
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
            TotalScore = Mathf.RoundToInt(lifeTime * Settings.TimeMultiplier + passedStagesCount * Settings.PassedStageMultiplier);
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
