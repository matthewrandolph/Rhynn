using Content;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Rhynn.Engine;
using Util;
using Util.Pathfinding;

namespace Tests
{
    public class WeightedEdgeTests
    {
        private IPathfindingNode start;
        private IPathfindingNode end;
        private WeightedEdge edge;

        [SetUp]
        public void Init()
        {
            start = new GridTile(Tiles.Floor, new Vec2(0, 0));
            end = new GridTile(Tiles.Floor, new Vec2(1, 1));
            edge = new WeightedEdge(start, end, 1, Direction.E);
        }
        
        [Test]
        public void SetTravsersableFlag_IsTraversableLand_True()
        {
            Assert.IsFalse(edge.IsTraversable(Motility.Land));
            
            edge.SetMotilityFlag(Motility.Land);
            
            Assert.IsTrue(edge.IsTraversable(Motility.Land));
        }
        
        [Test]
        public void SetTravsersableFlag_IsTraversableFly_True()
        {
            Assert.IsFalse(edge.IsTraversable(Motility.Fly));
            
            edge.SetMotilityFlag(Motility.Fly);
            
            Assert.IsTrue(edge.IsTraversable(Motility.Fly));
        }

        [Test]
        public void SetTraversableFlag_OthersUnchanged_True()
        {
            Assert.IsFalse(edge.IsTraversable(Motility.Land));
            Assert.IsFalse(edge.IsTraversable(Motility.Fly));
            Assert.IsFalse(edge.IsTraversable(Motility.Burrow));
            
            edge.SetMotilityFlag(Motility.Land);
            
            Assert.IsFalse(edge.IsTraversable(Motility.Fly));
            Assert.IsFalse(edge.IsTraversable(Motility.Burrow));
        }
        
        [Test]
        public void IsTraversible_IsTraversability_True()
        {
            Assert.IsFalse(edge.IsTraversable(Motility.Land));
            
            edge.SetMotilityFlag(Motility.Land);
            
            Assert.IsTrue(edge.IsTraversable(Motility.Land));
        }

        [Test]
        public void IsTraversible_ContainsTraversability_True()
        {
            Motility traversable = Motility.Land | Motility.Fly;
            
            Assert.IsFalse(edge.IsTraversable(Motility.Land));
            Assert.IsFalse(edge.IsTraversable(Motility.Fly));
            Assert.IsFalse(edge.IsTraversable(Motility.Burrow));
            
            edge.SetMotilityFlag(Motility.Land);
            edge.SetMotilityFlag(Motility.Fly);
            edge.SetMotilityFlag(Motility.Burrow);
            
            Assert.IsTrue(edge.IsTraversable(traversable));
        }

        [Test]
        public void IsTraversible_DoesNotContainTraversability_False()
        {
            Motility traversable = Motility.Fly;
            
            Assert.IsFalse(edge.IsTraversable(Motility.Land));
            Assert.IsFalse(edge.IsTraversable(Motility.Fly));
            Assert.IsFalse(edge.IsTraversable(Motility.Burrow));
            
            edge.SetMotilityFlag(Motility.Land);
            edge.SetMotilityFlag(Motility.Burrow);
            
            Assert.IsFalse(edge.IsTraversable(traversable));
        }

        [Test]
        public void UnsetTraversableFlag_IsTraversable_False()
        {
            edge.SetMotilityFlag(Motility.Everything);
            
            edge.UnsetMotilityFlag(Motility.Land);

            Assert.IsFalse(edge.IsTraversable(Motility.Land));
        }

        [Test]
        public void UnsetTraversableFlag_OthersUnchanged_True()
        {
            edge.SetMotilityFlag(Motility.Everything);

            edge.UnsetMotilityFlag(Motility.Land);

            Assert.IsTrue(edge.IsTraversable(Motility.Fly));
            Assert.IsTrue(edge.IsTraversable(Motility.Burrow));
        }
    }
}
