using System.Linq;
using Content;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Rhynn.Engine;
using Util;
using Util.Pathfinding;
using Util.Pathfinding.SearchAlgorithms;

namespace Tests
{
    public class SearchAlgorithmBaseTests<T>
    {
        private IPathfindingEdge<T> edgeNorth;
        private IPathfindingEdge<T> edgeNorthWest;
        private IPathfindingEdge<T> edgePortal;

        [SetUp]
        public void Init()
        {
            /*IPathfindingNode<T> start = new GridTile(Tiles.Floor, new Vec2(0, 0));
            IPathfindingNode<T> endNorth = new GridTile(Tiles.Floor, start.Position + Direction.N);
            IPathfindingNode<T> endNorthWest = new GridTile(Tiles.Floor, start.Position + Direction.NW);
            IPathfindingNode<T> endPortal = new GridTile(Tiles.Floor, new Vec2(10, 10));
            
            start.AddNeighbor(endNorth, 1f, Direction.N);
            start.AddNeighbor(endNorthWest, 1f, Direction.NW);
            start.AddNeighbor(endPortal, 1f, Direction.None);

            edgeNorth = start.NeighborMap.Values.First(x => x.End == endNorth);
            edgeNorthWest = start.NeighborMap.Values.First(x => x.End == endNorthWest);
            edgePortal = start.NeighborMap.Values.First(x => x.End == endPortal);
            
            edgeNorth.SetMotilityFlag(start.Type.Motility & endNorth.Type.Motility);
            edgeNorthWest.SetMotilityFlag(start.Type.Motility & endNorthWest.Type.Motility);
            edgePortal.SetMotilityFlag(start.Type.Motility & endPortal.Type.Motility);*/
        }

        /*[Test]
        public void IsTraversable_Unconstrained_True()
        {
            Assert.IsTrue(SearchAlgorithmBase.IsTraversable(edgeNorth, Motility.Unconstrained));
        }
        
        [Test]
        public void IsTraversable_NeighborsDefault_True()
        {
            Assert.IsTrue(SearchAlgorithmBase.IsTraversable(edgeNorth, Motility.Land));
            Assert.IsTrue(SearchAlgorithmBase.IsTraversable(edgeNorthWest, Motility.Land));
            Assert.IsTrue(SearchAlgorithmBase.IsTraversable(edgePortal, Motility.Land));
        }
        
        [Test]
        public void IsTraversable_AllNeighbors_True()
        {
            Motility motility = Motility.Land | Motility.AllNeighbors;
            
            Assert.IsTrue(SearchAlgorithmBase.IsTraversable(edgeNorth, motility));
            Assert.IsTrue(SearchAlgorithmBase.IsTraversable(edgeNorthWest, motility));
            Assert.IsTrue(SearchAlgorithmBase.IsTraversable(edgePortal, motility));
        }

        [Test]
        public void IsTraversable_FourWayNeighbors_NorthTrue()
        {
            Motility motility = Motility.Land | Motility.FourWayNeighbors;
            
            Assert.IsTrue(SearchAlgorithmBase.IsTraversable(edgeNorth, motility));
            Assert.IsFalse(SearchAlgorithmBase.IsTraversable(edgeNorthWest, motility));
            Assert.IsFalse(SearchAlgorithmBase.IsTraversable(edgePortal, motility));
        }

        [Test]
        public void IsTraversable_EightWayNeighbors_PortalFalse()
        {
            Motility motility = Motility.Land | Motility.EightWayNeighbors;
            
            Assert.IsTrue(SearchAlgorithmBase.IsTraversable(edgeNorth, motility));
            Assert.IsTrue(SearchAlgorithmBase.IsTraversable(edgeNorthWest, motility));
            Assert.IsFalse(SearchAlgorithmBase.IsTraversable(edgePortal, motility));
        }

        [Test]
        public void IsTraversable_DoesNotContainMotility_False()
        {
            Motility motility = Motility.Swim;
            
            Assert.IsFalse(SearchAlgorithmBase.IsTraversable(edgeNorth, motility));
        }*/
    }
}
