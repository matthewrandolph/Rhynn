using System.Collections;
using System.Collections.Generic;
using Content;
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
        private GridTile start;
        private GridTile end;

        [SetUp]
        public void Init()
        {
            start = new GridTile(Tiles.Floor, new Vec2(0, 0));
            end = new GridTile(Tiles.Floor, new Vec2(1, 1));
            
            //start.AddNeighbor(end, 1f, Direction.E);
            //end.AddNeighbor(start, 1f, Direction.E);
        }
        
        [Test]
        public void IsTraversableTo_DefaultIsTraversable_False()
        {
            //Assert.IsFalse(start.IsTraversableTo(end, Motility.Land));
        }
        
        [Test]
        public void IsTraversableTo_IsTraversable_True()
        {
            //Assert.IsFalse(start.IsTraversableTo(end, Motility.Land));
            
            //end.SetIncomingTraversableFlag(Motility.Land);
            
            //Assert.IsTrue(start.IsTraversableTo(end, Motility.Land));
        }
        
        [Test]
        public void IsTraversableTo_ContainsTraversable_True()
        {
            Motility traversable = Motility.Land | Motility.Burrow | Motility.Climb;
            
            //Assert.IsFalse(start.IsTraversableTo(end, Motility.Land));
            
            //end.SetIncomingTraversableFlag(traversable);
            
            //Assert.IsTrue(start.IsTraversableTo(end, Motility.Land));
        }
        
        [Test]
        public void IsTraversibleTo_DoesNotContainTraversability_False()
        {
            Motility traversable = Motility.Fly;
            
            /*Assert.IsFalse(start.IsTraversableTo(end, Motility.Land));
            Assert.IsFalse(start.IsTraversableTo(end, Motility.Fly));
            Assert.IsFalse(start.IsTraversableTo(end, Motility.Burrow));
            
            end.SetIncomingTraversableFlag(Motility.Land);
            end.SetIncomingTraversableFlag(Motility.Burrow);
            
            Assert.IsFalse(start.IsTraversableTo(end, traversable));*/
        }

        [Test]
        public void SetIncomingTravsersableFlag_SetSingleTraversableFlag_True()
        {
            /*Assert.IsFalse(start.IsTraversableTo(end, Motility.Land));
            
            end.SetIncomingTraversableFlag(Motility.Land);
            
            Assert.IsTrue(start.IsTraversableTo(end, Motility.Land));*/
        }
        
        [Test]
        public void SetIncomingTravsersableFlag_SetMultipleTraversableFlags_True()
        {
            Motility traversable = Motility.Land | Motility.Fly;
            
            /*Assert.IsFalse(start.IsTraversableTo(end, traversable));
            
            end.SetIncomingTraversableFlag(traversable);

            Assert.IsTrue(start.IsTraversableTo(end, Motility.Land));
            Assert.IsTrue(start.IsTraversableTo(end, Motility.Fly));*/
        }
        
        [Test]
        public void SetIncomingTravsersableFlag_OthersUnchanged_True()
        {
            /*Assert.IsFalse(start.IsTraversableTo(end, Motility.Land));
            Assert.IsFalse(start.IsTraversableTo(end, Motility.Fly));
            Assert.IsFalse(start.IsTraversableTo(end, Motility.Burrow));
            
            end.SetIncomingTraversableFlag(Motility.Land);
            
            Assert.IsFalse(start.IsTraversableTo(end, Motility.Fly));
            Assert.IsFalse(start.IsTraversableTo(end, Motility.Burrow));*/
        }

        [Test]
        public void UnsetIncomingTraversableFlag_IsTraversable_False()
        {
            /*end.SetIncomingTraversableFlag(Motility.Everything);
            end.UnsetIncomingTraversableFlag(Motility.Land);

            Assert.IsFalse(start.IsTraversableTo(end, Motility.Land));*/
        }

        [Test]
        public void UnsetIncomingTraversableFlag_OthersUnchanged_True()
        {
            /*end.SetIncomingTraversableFlag(Motility.Everything);

            end.UnsetIncomingTraversableFlag(Motility.Land);

            Assert.IsTrue(start.IsTraversableTo(end, Motility.Fly));
            Assert.IsTrue(start.IsTraversableTo(end, Motility.Burrow));*/
        }
        
        [Test]
        public void SetOutgoingTravsersableFlag_SetSingleTraversableFlag_True()
        {
            /*Assert.IsFalse(start.IsTraversableTo(end, Motility.Land));
            
            start.SetOutgoingTraversableFlag(Motility.Land);
            
            Assert.IsTrue(start.IsTraversableTo(end, Motility.Land));*/
        }
        
        [Test]
        public void SetOutgoingTravsersableFlag_SetMultipleTraversableFlags_True()
        {
            Motility traversable = Motility.Land | Motility.Fly;
            
            /*Assert.IsFalse(start.IsTraversableTo(end, traversable));
            
            start.SetOutgoingTraversableFlag(traversable);

            Assert.IsTrue(start.IsTraversableTo(end, Motility.Land));
            Assert.IsTrue(start.IsTraversableTo(end, Motility.Fly));*/
        }
        
        [Test]
        public void SetOutgoingTravsersableFlag_OthersUnchanged_True()
        {
            /*Assert.IsFalse(start.IsTraversableTo(end, Motility.Land));
            Assert.IsFalse(start.IsTraversableTo(end, Motility.Fly));
            Assert.IsFalse(start.IsTraversableTo(end, Motility.Burrow));
            
            start.SetOutgoingTraversableFlag(Motility.Land);
            
            Assert.IsFalse(start.IsTraversableTo(end, Motility.Fly));
            Assert.IsFalse(start.IsTraversableTo(end, Motility.Burrow));*/
        }

        [Test]
        public void UnsetOutgoingTraversableFlag_IsTraversable_False()
        {
            /*start.SetOutgoingTraversableFlag(Motility.Everything);
            start.UnsetOutgoingTraversableFlag(Motility.Land);

            Assert.IsFalse(start.IsTraversableTo(end, Motility.Land));*/
        }

        [Test]
        public void UnsetOutgoingTraversableFlag_OthersUnchanged_True()
        {
            /*start.SetOutgoingTraversableFlag(Motility.Everything);

            start.UnsetOutgoingTraversableFlag(Motility.Land);

            Assert.IsTrue(start.IsTraversableTo(end, Motility.Fly));
            Assert.IsTrue(start.IsTraversableTo(end, Motility.Burrow));*/
        }

        [Test]
        public void CanEnter_FlagSet_True()
        {
            //start.SetIncomingTraversableFlag(Motility.Land);
            
            Assert.IsTrue(start.CanEnter(Motility.Land));
        }
        
        [Test]
        public void CanEnter_FlagNotSet_False()
        {
            Assert.IsFalse(start.CanEnter(Motility.Swim));
        }
    }
}
