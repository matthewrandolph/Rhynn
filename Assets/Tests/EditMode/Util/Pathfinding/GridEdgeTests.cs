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
    public class GridEdgeTests
    {
        [Test]
        public void IsTraversable_UnconstrainedEdge_True()
        {
            GridEdge edge = new GridEdge(1f, Motility.Unconstrained,Vec2.One, Direction.N);
            
            Assert.IsTrue(edge.IsTraversable(Motility.Land));
        }
        
        [Test]
        public void IsTraversable_ContainsMotility_True()
        {
            GridEdge edge = new GridEdge(1f, Motility.Land,Vec2.One, Direction.N);
            
            Assert.IsTrue(edge.IsTraversable(Motility.Land));
        }
        
        [Test]
        public void IsTraversable_DoesNotContainMotility_False()
        {
            GridEdge edge = new GridEdge(1f, Motility.Land,Vec2.One, Direction.N);
            
            Assert.IsFalse(edge.IsTraversable(Motility.Burrow));
        }
    }
}
