using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Managers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class Tests
    {

        Player player;
        GameManager gameManager;
        MazeManager mazeManager;
        UIManager uIManager;
        Text scoreComponent;
        GameObject endTrigger;
        GameObject createTrigger;

        [SetUp]
        public void SetUp()
        {
            GameObject gameManagerGO = GameObject.Instantiate(Resources.Load("Prefabs/GameManager") as GameObject); 
            
            gameManagerGO.name = "GameManager";
            gameManager = gameManagerGO.GetComponent<GameManager>();
            var camera = new GameObject();

            camera.AddComponent<Camera>();
            gameManager.Camera = camera.GetComponent<Camera>();
            camera.GetComponent<Camera>().enabled = true;
            camera.transform.position = new Vector3(-4, 8, -4);
            camera.transform.eulerAngles = new Vector3(30, 45, 0);
            player = gameManager.Player.GetComponent<Player>();
            GameObject uIManagerGO = GameObject.Instantiate(Resources.Load("Prefabs/Canvas")) as GameObject;
            uIManagerGO.name = "Canvas";
            uIManager = uIManagerGO.GetComponent<UIManager>();
            mazeManager = gameManager.mazeManager;
            scoreComponent = uIManager.transform.Find("Score").GetComponent<Text>();

            endTrigger = new GameObject(name: "End trigger");
            endTrigger.transform.localScale = new Vector3(1, 1, 1);
            endTrigger.transform.position = player.transform.position + new Vector3(1, 5, 0);          
            endTrigger.AddComponent<BoxCollider>();
            endTrigger.GetComponent<BoxCollider>().isTrigger = true;

            createTrigger = new GameObject(name: "Create trigger");
            createTrigger.transform.localScale = new Vector3(1, 1, 1);
            createTrigger.transform.position = player.transform.position + new Vector3(1, 5, 0);
            createTrigger.AddComponent<BoxCollider>();
            createTrigger.GetComponent<BoxCollider>().isTrigger = true;
        }

        [UnityTest]
        public IEnumerator TestUI()
        {
            var score = scoreComponent.text;
            uIManager.OnScoreChanged(2);
            yield return new WaitForSeconds(1);
            var newScore = scoreComponent.text;
            Assert.AreNotEqual(score, newScore);
        }

        [UnityTest]
        public IEnumerator TestDestroyMaze()
        {
            endTrigger.transform.position = player.transform.position;
            yield return new WaitForSeconds(1);
            GameObject instance = GameObject.Find("Instance №0");
            Assert.IsNull(instance);
        }

        [UnityTest]
        public IEnumerator TestCreateMaze()
        {
            createTrigger.transform.position = player.transform.position;
            yield return new WaitForSeconds(1);
            GameObject instance = GameObject.Find("Instance №1");
            Assert.IsNotNull(instance);
        }

        [UnityTest]
        public IEnumerator TestScoreIncrease()
        {
            endTrigger.transform.position = player.transform.position;
            var score = gameManager.TotalScore;
            yield return new WaitForSeconds(1);
            var newScore = gameManager.TotalScore;
            Assert.AreNotEqual(score, newScore);
        }


        [UnityTearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(player);
            GameObject.DestroyImmediate(mazeManager);
            GameObject.DestroyImmediate(gameManager);
            GameObject.DestroyImmediate(uIManager);
            GameObject.DestroyImmediate(endTrigger);
            GameObject.DestroyImmediate(createTrigger);
        }
    }
}
