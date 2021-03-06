using System;
using Content;
using NUnit.Framework;
using Rhynn.Engine;
using Rhynn.Engine.Generation;
using Util;
using Util.Pathfinding;

namespace Tests
{
    [TestFixture]
    public class DelaunayGeneratorTests
    {
        public Game Game;
        public BattleMap BattleMap;

        [SetUp]
        public void Init()
        {
            Game = new Game();
            BattleMap = Game.BattleMap;
        }

        [Test]
        public void IsOpen_RectOutOfBoundsX_False()
        {
            var generator = new DelaunayGenerator(BattleMap, new DelaunayGeneratorOptions());

            Assert.IsFalse(generator.IsOpen(new Rect(-1, 1, 10, 10), null));
        }
        
        [Test]
        public void IsOpen_RectOutOfBoundsY_False()
        {
            var generator = new DelaunayGenerator(BattleMap, new DelaunayGeneratorOptions());

            Assert.IsFalse(generator.IsOpen(new Rect(1, -1, 10, 10), null));
        }
        
        [Test]
        public void IsOpen_OverlapsExisting_False()
        {
            var generator = new DelaunayGenerator(BattleMap, new DelaunayGeneratorOptions());
            BattleMap.Tiles[2, 2] = new GridNode(Tiles.Wall, new Vec2(2, 2), BattleMap.Tiles);

            Assert.IsFalse(generator.IsOpen(new Rect(3,3), null));
        }

        [Test]
        public void IsOpen_NewContainsExisting_False()
        {
            var generator = new DelaunayGenerator(BattleMap, new DelaunayGeneratorOptions());
            BattleMap.Tiles[4, 4] = new GridNode(Tiles.Wall, new Vec2(4, 4), BattleMap.Tiles);
            
            Assert.IsFalse(generator.IsOpen(new Rect(0, 0, 10,10), null));
        }
        
        [Test]
        public void IsOpen_ExistingContainsNew_False()
        {
            var generator = new DelaunayGenerator(BattleMap, new DelaunayGeneratorOptions());
            foreach (var tile in new Rect(10, 10))
            {
                BattleMap.Tiles[tile.x, tile.y] = new GridNode(Tiles.Wall, new Vec2(tile.x, tile.y), BattleMap.Tiles);
            }
            
            Assert.IsFalse(generator.IsOpen(new Rect(3, 3, 2, 2), null));
        }
        
        [Test]
        public void IsOpen_CoversExistingButIsInException_True()
        {
            var generator = new DelaunayGenerator(BattleMap, new DelaunayGeneratorOptions());
            BattleMap.Tiles[2, 2] = new GridNode(Tiles.Wall, new Vec2(2, 2), BattleMap.Tiles);

            Assert.IsTrue(generator.IsOpen(new Rect(3,3), new Vec2(2, 2)));
        }

        [Test]
        public void IsOpen_IsOpen_True()
        {
            var generator = new DelaunayGenerator(BattleMap, new DelaunayGeneratorOptions());
            
            Assert.IsTrue(generator.IsOpen(new Rect(3,3), null));
        }

        [Test]
        public void GetTile_OutOfBoundsX_IndexOutOfRangeException()
        {
            var generator = new DelaunayGenerator(BattleMap, new DelaunayGeneratorOptions());

            Assert.Throws<IndexOutOfRangeException>(() => generator.GetTile(new Vec2(-1, 0)));
        }
        
        [Test]
        public void GetTile_OutOfBoundsY_IndexOutOfRangeException()
        {
            var generator = new DelaunayGenerator(BattleMap, new DelaunayGeneratorOptions());

            Assert.Throws<IndexOutOfRangeException>(() => generator.GetTile(new Vec2(0, -1)));
        }

        [Test]
        public void GetTile_IsValidTile_True()
        {
            var generator = new DelaunayGenerator(BattleMap, new DelaunayGeneratorOptions());

            var tile = generator.GetTile(new Vec2(1, 1));

            Assert.AreNotEqual(tile,null);
        }
        
        [Test]
        public void SetTile_TileChanged_True()
        {
            var generator = new DelaunayGenerator(BattleMap, new DelaunayGeneratorOptions());
            Vec2 tileCoordinates = Vec2.One;

            Assert.AreEqual(BattleMap.Tiles[tileCoordinates].Type, Tiles.Stone);
            
            generator.SetTile(tileCoordinates, Tiles.Wall);
            
            Assert.AreEqual(BattleMap.Tiles[tileCoordinates].Type, Tiles.Wall);
        }
    }
}
