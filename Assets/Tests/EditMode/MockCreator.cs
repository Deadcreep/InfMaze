using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Managers;
using Assets.Scripts;
using Moq;
using UnityEngine;

namespace Assets.Tests.EditMode
{
    public static class MockCreator
    {
        public static MazeManager ReturnMazeManagerMock()
        {
            return Mock.Of<MazeManager>(mm
                => mm.GetPassableDirections(It.IsAny<Vector2Int>()) ==
                new List<Vector3>() { new Vector3(1, 0, 0) });
        }

    }

}
