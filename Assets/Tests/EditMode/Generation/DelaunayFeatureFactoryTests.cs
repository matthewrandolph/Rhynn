using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rhynn.Engine;
using Rhynn.Engine.Generation;
using UnityEditor.StyleSheets;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DelaunayFeatureFactoryTests
    {
        public Game Game;
        public BattleMap BattleMap;
        public DelaunayGenerator Generator;
        public DelaunayGeneratorOptions Options;
        
        [SetUp]
        public void Init()
        {
            Game = new Game();
            BattleMap = Game.BattleMap;
            Generator = new DelaunayGenerator(BattleMap, Options);
        }

/*        [Test]
        public void MakeStartingRoom_IsRect()
        {
            var factory = new DelaunayFeatureFactory(Generator);

            var result = factory.MakeStartingRoom();
            
            Assert.IsInstanceOf(result.GetType(), typeof(Util.Rect));
        }
        
        [Test]
        public void MakeStartingRoom_1Attempt_RectWithinBounds()
        {
            var factory = new DelaunayFeatureFactory(Generator);

            var result = factory.MakeStartingRoom();
            
            Assert.IsTrue(result.Left > BattleMap.Bounds.Left);
            Assert.IsTrue(result.Right < BattleMap.Bounds.Right);
            Assert.IsTrue(result.Top < BattleMap.Bounds.Top);
            Assert.IsTrue(result.Bottom > BattleMap.Bounds.Bottom);
        }*/
    }
}
