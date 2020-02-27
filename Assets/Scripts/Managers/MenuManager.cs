using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class MenuManager : MonoBehaviour
    {

        Scene currentScene;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadGame()
        {
            SceneManager.LoadScene("Game");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
