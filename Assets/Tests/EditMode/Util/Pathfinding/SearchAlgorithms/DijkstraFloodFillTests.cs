using System;
using System.Collections;
using System.Collections.Generic;
using Content;
using NUnit.Framework;
using Rhynn.Engine;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.TestTools;
using Util.Pathfinding;
using Util.Pathfinding.SearchAlgorithms;

namespace Tests
{
    public class DijkstraFloodFillTests
    {
        private Game game;
        private BattleMap map;

        [OneTimeSetUp]
        public void Init()
        {
            game = new Game();
            map = game.BattleMap;
            game.GenerateBattleMap();
        }

        [Test]
        public void Fill_NodeWithinSearchDepth_True()
        {
            var tiles = map.Tiles.Pathfinder<DijkstraFloodFill>(map.Tiles[10, 10], 5, Motility.Unconstrained);
            
            Assert.IsTrue(tiles.ContainsKey(map.Tiles[11, 11]));
        }
        
        [Test]
        public void Fill_SameNode_True()
        {
            var tiles = map.Tiles.Pathfinder<DijkstraFloodFill>(map.Tiles[10, 10], 5, Motility.Unconstrained);
            
            Assert.IsTrue(tiles.ContainsKey(map.Tiles[10, 10]));
        }
        
        [Test]
        public void Fill_NodeOutsideSearchDepth_False()
        {
            var tiles = map.Tiles.Pathfinder<DijkstraFloodFill>(map.Tiles[10, 10], 5, Motility.Unconstrained);
            
            Assert.IsFalse(tiles.ContainsKey(map.Tiles[20, 20]));
        }
        
        [Test]
        public void Fill_NodeNotTraversable_False()
        {
            GridTile tile = null;
            
            for (int x = 0; x < map.Tiles.Width; x++)
            {
                for (int y = 0; y < map.Tiles.Height; y++)
                {
                    tile = map.Tiles[x, y];
                    if (tile.Type == Tiles.Stone)
                        break;
                }
            }
            
            Assert.IsNotNull(tile);

            var tiles = map.Tiles.Pathfinder<DijkstraFloodFill>(
                map.Tiles.GetNodeAt(game.PlayerCharacter.Position), Int32.MaxValue, Motility.Land);
            
            Assert.IsFalse(tiles.ContainsKey(tile));
        }
    }
}
