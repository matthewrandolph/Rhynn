using NUnit.Framework;
using NUnit.Framework.Constraints;
using Rhynn.Engine;
using Util;
using Util.Pathfinding;

namespace Tests
{
    public class WeightedEdgeTests
    {
        [Test]
        public void SetTravsersableFlag_IsTraversableLand_True()
        {
            IPathfindingNode start = new GridTile(TileType.Floor, new Vec2(0, 0));
            IPathfindingNode end = new GridTile(TileType.Floor, new Vec2(1, 1));
            WeightedEdge edge = new WeightedEdge(start, end, 1, Direction.E);
            
            Assert.IsFalse(edge.IsTraversable(Traversable.Land));
            
            edge.SetTraversableFlag(Traversable.Land);
            
            Assert.IsTrue(edge.IsTraversable(Traversable.Land));
        }
        
        [Test]
        public void SetTravsersableFlag_IsTraversableFly_True()
        {
            IPathfindingNode start = new GridTile(TileType.Floor, new Vec2(0, 0));
            IPathfindingNode end = new GridTile(TileType.Floor, new Vec2(1, 1));
            WeightedEdge edge = new WeightedEdge(start, end, 1, Direction.E);
            
            Assert.IsFalse(edge.IsTraversable(Traversable.Fly));
            
            edge.SetTraversableFlag(Traversable.Fly);
            
            Assert.IsTrue(edge.IsTraversable(Traversable.Fly));
        }

        [Test]
        public void SetTraversableFlag_OthersUnchanged_True()
        {
            IPathfindingNode start = new GridTile(TileType.Floor, new Vec2(0, 0));
            IPathfindingNode end = new GridTile(TileType.Floor, new Vec2(1, 1));
            WeightedEdge edge = new WeightedEdge(start, end, 1, Direction.E);
            
            Assert.IsFalse(edge.IsTraversable(Traversable.Land));
            Assert.IsFalse(edge.IsTraversable(Traversable.Fly));
            Assert.IsFalse(edge.IsTraversable(Traversable.Burrow));
            
            edge.SetTraversableFlag(Traversable.Land);
            
            Assert.IsFalse(edge.IsTraversable(Traversable.Fly));
            Assert.IsFalse(edge.IsTraversable(Traversable.Burrow));
        }
        
        [Test]
        public void IsTraversible_IsTraversability_True()
        {
            IPathfindingNode start = new GridTile(TileType.Floor, new Vec2(0, 0));
            IPathfindingNode end = new GridTile(TileType.Floor, new Vec2(1, 1));
            WeightedEdge edge = new WeightedEdge(start, end, 1, Direction.E);

            Assert.IsFalse(edge.IsTraversable(Traversable.Land));
            
            edge.SetTraversableFlag(Traversable.Land);
            
            Assert.IsTrue(edge.IsTraversable(Traversable.Land));
        }

        [Test]
        public void IsTraversible_ContainsTraversability_True()
        {
            IPathfindingNode start = new GridTile(TileType.Floor, new Vec2(0, 0));
            IPathfindingNode end = new GridTile(TileType.Floor, new Vec2(1, 1));
            WeightedEdge edge = new WeightedEdge(start, end, 1, Direction.E);
            Traversable traversable = Traversable.Land | Traversable.Fly;
            
            Assert.IsFalse(edge.IsTraversable(Traversable.Land));
            Assert.IsFalse(edge.IsTraversable(Traversable.Fly));
            Assert.IsFalse(edge.IsTraversable(Traversable.Burrow));
            
            edge.SetTraversableFlag(Traversable.Land);
            edge.SetTraversableFlag(Traversable.Fly);
            edge.SetTraversableFlag(Traversable.Burrow);
            
            Assert.IsTrue(edge.IsTraversable(traversable));
        }

        [Test]
        public void IsTraversible_DoesNotContainTraversability_False()
        {
            IPathfindingNode start = new GridTile(TileType.Floor, new Vec2(0, 0));
            IPathfindingNode end = new GridTile(TileType.Floor, new Vec2(1, 1));
            WeightedEdge edge = new WeightedEdge(start, end, 1, Direction.E);
            Traversable traversable = Traversable.Fly;
            
            Assert.IsFalse(edge.IsTraversable(Traversable.Land));
            Assert.IsFalse(edge.IsTraversable(Traversable.Fly));
            Assert.IsFalse(edge.IsTraversable(Traversable.Burrow));
            
            edge.SetTraversableFlag(Traversable.Land);
            edge.SetTraversableFlag(Traversable.Burrow);
            
            Assert.IsFalse(edge.IsTraversable(traversable));
        }

        [Test]
        public void UnsetTraversableFlag_IsTraversable_False()
        {            
            IPathfindingNode start = new GridTile(TileType.Floor, new Vec2(0, 0));
            IPathfindingNode end = new GridTile(TileType.Floor, new Vec2(1, 1));
            WeightedEdge edge = new WeightedEdge(start, end, 1, Direction.E);
            
            edge.SetTraversableFlag(Traversable.Everything);
            edge.UnsetTraversableFlag(Traversable.Land);

            Assert.IsFalse(edge.IsTraversable(Traversable.Land));
        }

        [Test]
        public void UnsetTraversableFlag_OthersUnchanged_True()
        {
            IPathfindingNode start = new GridTile(TileType.Floor, new Vec2(0, 0));
            IPathfindingNode end = new GridTile(TileType.Floor, new Vec2(1, 1));
            WeightedEdge edge = new WeightedEdge(start, end, 1, Direction.E);

            edge.SetTraversableFlag(Traversable.Everything);

            edge.UnsetTraversableFlag(Traversable.Land);

            Assert.IsTrue(edge.IsTraversable(Traversable.Fly));
            Assert.IsTrue(edge.IsTraversable(Traversable.Burrow));
        }
    }
}
