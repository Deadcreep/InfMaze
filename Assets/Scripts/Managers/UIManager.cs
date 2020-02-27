using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Managers
{
    public class UIManager: MonoBehaviour
    {
        private Text timerComponent;
        private Text scoreComponent;
        private Text totalScoreComponent;
        private GameObject endGamePanel;
        private int timeLeft;
        private int score;
        private GameObject controlPanel;
        private Button up;
        private Button right;
        private Button down;
        private Button left;
        public Player player;

        public bool GameEnd { get; set; }

        private void OnEnable()
        {
            timerComponent = transform.Find("TimeLeft").GetComponent<Text>();
            scoreComponent = transform.Find("Score").GetComponent<Text>();
            controlPanel = transform.Find("ControlPanel").gameObject;
            up = controlPanel.transform.Find("Up").GetComponent<Button>();
            right = controlPanel.transform.Find("Right").GetComponent<Button>();
            down = controlPanel.transform.Find("Down").GetComponent<Button>();
            left = controlPanel.transform.Find("Left").GetComponent<Button>();
            endGamePanel = transform.Find("EndGamePanel").gameObject;
            totalScoreComponent = endGamePanel.transform.Find("TotalScore").GetComponent<Text>();            
            timerComponent.text = Settings.LifeTime.ToString();
        }

        private void Start()
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            StartNewGame();
            up.onClick.AddListener(() =>
            {
                player.StartCoroutine(player.Move(Vector3.right));
            });
            right.onClick.AddListener(() =>
            {
                player.StartCoroutine(player.Move(Vector3.back));
            });
            down.onClick.AddListener(() =>
            {
                player.StartCoroutine(player.Move(Vector3.left));
            });
            left.onClick.AddListener(() =>
            {
                player.StartCoroutine(player.Move(Vector3.forward));
            });
        }


        private void OnGUI()
        {
            timerComponent.text = "Time: " + timeLeft.ToString();
            scoreComponent.text = "Score: " + score.ToString();      
        }


        public void AlertEndGame()
        {
            timerComponent.gameObject.SetActive(false);
            scoreComponent.gameObject.SetActive(false);
            endGamePanel.gameObject.SetActive(true);
            totalScoreComponent.text = "Total score: " + score.ToString();
            controlPanel.SetActive(false);
        }

        public void StartNewGame()
        {
            timerComponent.gameObject.SetActive(true);
            timerComponent.text = Settings.LifeTime.ToString();
            scoreComponent.gameObject.SetActive(true);
            scoreComponent.text = "0";
            endGamePanel.gameObject.SetActive(false);
            timeLeft = Settings.LifeTime;
            if (!controlPanel.activeInHierarchy)
                controlPanel.SetActive(true);
        }

        public void OnTimerChanged(int time)
        {
            timeLeft = time;
        }

        public void OnScoreChanged(int score)
        {
            this.score = score;
        }

        
    }
}
