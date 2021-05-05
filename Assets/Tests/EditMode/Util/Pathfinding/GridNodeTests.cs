using Content;
using NUnit.Framework;
using Rhynn.Engine;
using Util;
using Util.Pathfinding;

namespace Tests
{
    public class GridNodeTests
    {
        private PathfindingGrid _grid;
        
        [SetUp]
        public void Init()
        {
            _grid = new PathfindingGrid(5, 5);
            _grid.Fill(position => new GridNode(Tiles.Stone, position, _grid));
            _grid.FillEdges((position, annotation) => new GridEdge(position, annotation));
            _grid.FillVertices((position) => new GridVertex(position));
        }
        
        [Test]
        public void Motility_BothUnconstrained_Unconstrained()
        {
            GridNode gridNode = new GridNode(
                new TileType("unconstrained", Motility.Unconstrained, 0), Vec2.Zero, _grid);
            
            Assert.AreEqual(gridNode.Motility, Motility.Unconstrained);
        }
        
        [Test]
        public void Motility_NodeUnconstrained_TileTypeMotility()
        {
            GridNode gridNode = new GridNode(Tiles.Stone, Vec2.Zero, _grid);
            
            Assert.AreEqual(gridNode.Motility, Motility.Incorporeal);
        }
        
        [Test]
        public void Motility_TileUnconstrained_NodeMotility()
        {
            GridNode gridNode = new GridNode(new TileType("unconstrained", 
                Motility.Unconstrained, 0), Motility.Incorporeal, Vec2.Zero, _grid);
            
            Assert.AreEqual(gridNode.Motility, Motility.Incorporeal);
        }
        
        [Test]
        public void Motility_BothConstrained_BooleanANDMotility()
        {
            GridNode gridNode = new GridNode(Tiles.Floor, Motility.Incorporeal, Vec2.Zero, _grid);
            
            Assert.AreEqual(gridNode.Motility, Motility.Incorporeal);
        }

        [Test]
        public void Neighbors_Get()
        {
            Vec2 tileCoordinates = new Vec2(3,3);
            GridNode tile = _grid[tileCoordinates];
            
            CollectionAssert.AreEqual(_grid.Neighbors(tile), tile.Neighbors);
        }

        [Test]
        public void AllowsMotility_Unconstrained_True()
        {
            GridNode gridNode = new GridNode(new TileType("unconstrained", 
                Motility.Unconstrained, 0), Vec2.Zero, _grid);
            
            Assert.IsTrue(gridNode.AllowsMotility(Motility.Unconstrained));
        }
        
        [Test]
        public void AllowsMotility_NodeUnconstrainedTileConstrained_True()
        {
            GridNode gridNode = new GridNode(Tiles.Floor, Vec2.Zero, _grid);
            
            Assert.IsTrue(gridNode.AllowsMotility(Motility.Land));
        }
        
        [Test]
        public void AllowsMotility_NoOverlap_False()
        {
            GridNode gridNode = new GridNode(Tiles.Floor, Motility.Burrow, Vec2.Zero, _grid);
            
            Assert.IsFalse(gridNode.AllowsMotility(Motility.Swim));
        }

        [Test]
        public void JoiningEdge_NeighborNodes_Edge()
        {
            Vec2 position = new Vec2(3, 3);
            GridNode node = _grid[position];

            IPathfindingEdge joiningEdge = node.JoiningEdge(_grid[position + Direction.N]);
            IPathfindingEdge edge = new GridEdge(position, Direction.N);
            Assert.AreEqual(edge, joiningEdge);
            
            joiningEdge = node.JoiningEdge(_grid[position + Direction.W]);
            edge = new GridEdge(position, Direction.W);
            Assert.AreEqual(edge, joiningEdge);
            
            joiningEdge = node.JoiningEdge(_grid[position + Direction.E]);
            edge = new GridEdge(new Vec2(position.x + 1, position.y), Direction.W);
            Assert.AreEqual(edge, joiningEdge);
            
            joiningEdge = node.JoiningEdge(_grid[position + Direction.S]);
            edge = new GridEdge(new Vec2(position.x, position.y + 1), Direction.N);
            Assert.AreEqual(edge, joiningEdge);
            
            joiningEdge = node.JoiningEdge(_grid[position + Direction.NW]);
            edge = new GridVertex(position);
            Assert.AreEqual(edge, joiningEdge);
            
            joiningEdge = node.JoiningEdge(_grid[position + Direction.NE]);
            edge = new GridVertex(new Vec2(position.x + 1, position.y));
            Assert.AreEqual(edge, joiningEdge);
            
            joiningEdge = node.JoiningEdge(_grid[position + Direction.SW]);
            edge = new GridVertex(new Vec2(position.x, position.y + 1));
            Assert.AreEqual(edge, joiningEdge);
            
            joiningEdge = node.JoiningEdge(_grid[position + Direction.SE]);
            edge = new GridVertex(new Vec2(position.x + 1, position.y + 1));
            Assert.AreEqual(edge, joiningEdge);
        }
        
        [Test]
        public void CanEnter_MotilityMatches_True()
        {
            Vec2 position = new Vec2(3, 3);
            GridNode node = _grid[position];
            GridNode neighborNode = _grid[position + Direction.N];

            Assert.IsTrue(node.CanEnter(neighborNode, Motility.Incorporeal));
        }
        
        [Test]
        public void CanEnter_MotilityDoesNotMatch_False()
        {
            Vec2 position = new Vec2(3, 3);
            GridNode node = _grid[position];
            GridNode neighborNode = _grid[position + Direction.N];

            Assert.IsFalse(node.CanEnter(neighborNode, Motility.Land));
        }
        
    }
}
