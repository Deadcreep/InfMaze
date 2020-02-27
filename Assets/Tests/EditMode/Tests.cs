using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;
using Assets.Scripts;
using Assets.Scripts.Managers;
using UnityEngine.UI;

namespace Assets.Tests.EditMode
{
    public class Tests
    {
        Player player;
        GameManager gameManager;
        MazeManager mazeManager;
        UIManager uIManager;


        [SetUp]
        public void SetUp()
        {
            GameObject playerGO = GameObject.Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
            player = playerGO.GetComponent<Player>();
            uIManager = (GameObject.Instantiate(Resources.Load("Prefabs/Canvas")) as GameObject).GetComponent<UIManager>();
            gameManager = GameObject.Instantiate(Resources.Load("Prefabs/GameManager") as GameObject).GetComponent<GameManager>();
            mazeManager = gameManager.mazeManager;            
        }

        [Test]
        [MaxTime(1000)]
        public void TestMoveOnWrongSide()
        {
            player.mazeManager = MockCreator.ReturnMazeManagerMock();
            var pos = player.transform.position;            
            player.StartCoroutine(player.Move(new Vector3(0, 0, 1)));            
            var posAfterMove = player.transform.position;
            Assert.AreEqual(pos, posAfterMove);
        }

        [Test]
        [MaxTime(1000)]
        public void TestMoveOnRightSide()
        {
            player.mazeManager = MockCreator.ReturnMazeManagerMock();
            var pos = player.transform.position;
            player.StartCoroutine(player.Move(new Vector3(1, 0, 0)));
            var posAfterMove = player.transform.position;
            Assert.AreEqual(posAfterMove, pos + new Vector3(1, 0, 0));
        }


        [Test]
       public void TestGameEnd()
        {
            //gameManager.player
            
        }

        


        [TearDown]
        public void TearDown()
        {
            GameObject.DestroyImmediate(player);
            GameObject.DestroyImmediate(mazeManager);
            GameObject.DestroyImmediate(gameManager);
            GameObject.DestroyImmediate(uIManager);
        }

    }
}
