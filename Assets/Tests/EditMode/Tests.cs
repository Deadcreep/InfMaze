using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;
using Assets.Scripts;
using Assets.Scripts.Managers;

namespace Tests
{
    public class Tests
    {
        Player player;
        GameManager gameManager;
        MazeManager mazeManager;


        [SetUp]
        public void SetUp()
        {
            GameObject managerGO = GameObject.Instantiate(Resources.Load("Prefabs/GameManager")) as GameObject;
            mazeManager = managerGO.GetComponent<MazeManager>();
            GameObject playerGO = GameObject.Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
            player = playerGO.GetComponent<Player>();
            player.mazeManager = Mock.Of<MazeManager>(
                mm => mm.GetPassableDirections(It.IsAny<Vector2Int>()) == new List<Vector3>()
                {
                    new Vector3(1,0,0),
                    new Vector3(-1,0,0)
                });

            gameManager = managerGO.GetComponent<GameManager>();
            

        }

        [Test]
        public void TestMoveOnWrongSide()
        {
            var pos = player.transform.position;
            player.StartCoroutine(player.Move(new Vector3(0, 0, -1)));
            
            var posAfterMove = player.transform.position;
            Assert.AreEqual(pos, posAfterMove);
        }



        // A Test behaves as an ordinary method
        [Test]
        public void TestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(player);
            GameObject.DestroyImmediate(mazeManager);
            GameObject.DestroyImmediate(gameManager);
        }
    }
}
