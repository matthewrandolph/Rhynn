using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rhynn.Engine;
using UnityEngine;
using UnityEngine.TestTools;
using Util;

namespace Tests
{
    public class DirectionTests
    {
        [Test]
        public void Towards_PointsTowardDirection_VariousDirections()
        {
            Assert.AreEqual(Direction.None, Direction.Towards(Vec2.Zero));
           
            Assert.AreEqual(Direction.N, Direction.Towards(new Vec2(0, -1)));
            Assert.AreEqual(Direction.W, Direction.Towards(new Vec2(-1, 0)));
            Assert.AreEqual(Direction.E, Direction.Towards(new Vec2(1, 0)));
            Assert.AreEqual(Direction.S, Direction.Towards(new Vec2(0, 1)));
            
            Assert.AreEqual(Direction.NE, Direction.Towards(new Vec2(1, -1)));
            Assert.AreEqual(Direction.NW, Direction.Towards(new Vec2(-1, -1)));
            Assert.AreEqual(Direction.SE, Direction.Towards(new Vec2(1, 1)));
            Assert.AreEqual(Direction.SW, Direction.Towards(new Vec2(-1, 1)));
        }
    }
}
