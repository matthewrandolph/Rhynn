using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rhynn.Engine;
using UnityEngine;
using UnityEngine.TestTools;
using Util;
using Util.Pathfinding;

namespace Tests
{
    public class GridTileTests
    {
        [Test]
        public void IsTraversableTo_DefaultIsTraversable_False()
        {
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);

            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Land));
        }
        
        [Test]
        public void IsTraversableTo_IsTraversable_True()
        {
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);
            
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Land));
            
            end.SetIncomingTraversableFlag(Traversable.Land);
            
            Assert.IsTrue(start.IsTraversableTo(end, Traversable.Land));
        }
        
        [Test]
        public void IsTraversableTo_ContainsTraversable_True()
        {
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);
            Traversable traversable = Traversable.Land | Traversable.Burrow | Traversable.Climb;
            
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Land));
            
            end.SetIncomingTraversableFlag(traversable);
            
            Assert.IsTrue(start.IsTraversableTo(end, Traversable.Land));
        }
        
        [Test]
        public void IsTraversibleTo_DoesNotContainTraversability_False()
        {
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);
            Traversable traversable = Traversable.Fly;
            
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Land));
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Fly));
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Burrow));
            
            end.SetIncomingTraversableFlag(Traversable.Land);
            end.SetIncomingTraversableFlag(Traversable.Burrow);
            
            Assert.IsFalse(start.IsTraversableTo(end, traversable));
        }

        [Test]
        public void SetIncomingTravsersableFlag_SetSingleTraversableFlag_True()
        {
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);
            
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Land));
            
            end.SetIncomingTraversableFlag(Traversable.Land);
            
            Assert.IsTrue(start.IsTraversableTo(end, Traversable.Land));
        }
        
        [Test]
        public void SetIncomingTravsersableFlag_SetMultipleTraversableFlags_True()
        {
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);
            Traversable traversable = Traversable.Land | Traversable.Fly;
            
            Assert.IsFalse(start.IsTraversableTo(end, traversable));
            
            end.SetIncomingTraversableFlag(traversable);

            Assert.IsTrue(start.IsTraversableTo(end, Traversable.Land));
            Assert.IsTrue(start.IsTraversableTo(end, Traversable.Fly));
        }
        
        [Test]
        public void SetIncomingTravsersableFlag_OthersUnchanged_True()
        {
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);

            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Land));
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Fly));
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Burrow));
            
            end.SetIncomingTraversableFlag(Traversable.Land);
            
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Fly));
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Burrow));
        }

        [Test]
        public void UnsetIncomingTraversableFlag_IsTraversable_False()
        {            
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);
            
            end.SetIncomingTraversableFlag(Traversable.Everything);
            end.UnsetIncomingTraversableFlag(Traversable.Land);

            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Land));
        }

        [Test]
        public void UnsetIncomingTraversableFlag_OthersUnchanged_True()
        {
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);

            end.SetIncomingTraversableFlag(Traversable.Everything);

            end.UnsetIncomingTraversableFlag(Traversable.Land);

            Assert.IsTrue(start.IsTraversableTo(end, Traversable.Fly));
            Assert.IsTrue(start.IsTraversableTo(end, Traversable.Burrow));
        }
        
        [Test]
        public void SetOutgoingTravsersableFlag_SetSingleTraversableFlag_True()
        {
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);
            
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Land));
            
            start.SetOutgoingTraversableFlag(Traversable.Land);
            
            Assert.IsTrue(start.IsTraversableTo(end, Traversable.Land));
        }
        
        [Test]
        public void SetOutgoingTravsersableFlag_SetMultipleTraversableFlags_True()
        {
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);
            Traversable traversable = Traversable.Land | Traversable.Fly;
            
            Assert.IsFalse(start.IsTraversableTo(end, traversable));
            
            start.SetOutgoingTraversableFlag(traversable);

            Assert.IsTrue(start.IsTraversableTo(end, Traversable.Land));
            Assert.IsTrue(start.IsTraversableTo(end, Traversable.Fly));
        }
        
        [Test]
        public void SetOutgoingTravsersableFlag_OthersUnchanged_True()
        {
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);

            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Land));
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Fly));
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Burrow));
            
            start.SetOutgoingTraversableFlag(Traversable.Land);
            
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Fly));
            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Burrow));
        }

        [Test]
        public void UnsetOutgoingTraversableFlag_IsTraversable_False()
        {            
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);
            
            start.SetOutgoingTraversableFlag(Traversable.Everything);
            start.UnsetOutgoingTraversableFlag(Traversable.Land);

            Assert.IsFalse(start.IsTraversableTo(end, Traversable.Land));
        }

        [Test]
        public void UnsetOutgoingTraversableFlag_OthersUnchanged_True()
        {
            GridTile start = new GridTile(TileType.Floor, new Vec2(0, 0));
            GridTile end = new GridTile(TileType.Floor, new Vec2(1, 1));
            start.AddNeighbor(end, 1f, Direction.E);
            end.AddNeighbor(start, 1f, Direction.E);

            start.SetOutgoingTraversableFlag(Traversable.Everything);

            start.UnsetOutgoingTraversableFlag(Traversable.Land);

            Assert.IsTrue(start.IsTraversableTo(end, Traversable.Fly));
            Assert.IsTrue(start.IsTraversableTo(end, Traversable.Burrow));
        }
    }
}
