using System;
using System.Collections.Generic;
using Content;
using NUnit.Framework;
using Rhynn.Engine;
using UnityEngine.WSA;
using Util;
using Util.Pathfinding;

namespace Tests
{
    public class PathfindingGridTests
    {
        #region Properties
        
        [Test]
        public void Width_IsTen_Vec2()
        {
            Vec2 size = new Vec2(10, 10);
            PathfindingGrid grid = new PathfindingGrid(size);
            
            Assert.AreEqual(size.x, grid.Width);
        }
        
        [Test]
        public void Width_IsTen_int()
        {
            int dimension = 10;
            PathfindingGrid grid = new PathfindingGrid(dimension, dimension);

            Assert.AreEqual(dimension, grid.Width);
        }

        [Test]
        public void Height_IsTen_Vec2()
        {
            Vec2 size = new Vec2(10, 10);
            PathfindingGrid grid = new PathfindingGrid(size);
            
            Assert.AreEqual(size.y, grid.Height);
        }
        
        [Test]
        public void Height_IsTen_int()
        {
            int dimension = 10;
            PathfindingGrid grid = new PathfindingGrid(dimension, dimension);

            Assert.AreEqual(dimension, grid.Height);
        }
        
        [Test]
        public void Size_IsTenByTen_Vec2()
        {
            Vec2 size = new Vec2(10, 10);
            PathfindingGrid grid = new PathfindingGrid(size);
            
            Assert.AreEqual(size, grid.Size);
        }
        
        [Test]
        public void Size_IsTenByTen_int()
        {
            int dimension = 10;
            PathfindingGrid grid = new PathfindingGrid(dimension, dimension);
            Vec2 size = new Vec2(10, 10);
            
            Assert.AreEqual(size, grid.Size);
        }
        
        [Test]
        public void Size_Vec2sNegative_ThrowsArgumentOutOfRangeException()
        {
            Vec2 size = new Vec2(-1, 1);
            PathfindingGrid grid;
            
            Assert.Throws<ArgumentOutOfRangeException>(
                () => grid = new PathfindingGrid(size));
        }
        
        [Test]
        public void Size_IntIsNegative_ThrowsArgumentOutOfRangeException()
        {
            int dimension = -1;
            PathfindingGrid grid;
            
            Assert.Throws<ArgumentOutOfRangeException>(
                () => grid = new PathfindingGrid(dimension, dimension));
        }
        
        #endregion
        
        [Test]
        public void IndexingInt_ReturnsGridNode_IsType()
        {
            Vec2 vec2 = new Vec2(10, 10);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            int int1 = 1;
            
            Assert.AreEqual(typeof(GridNode),grid[1, 1].GetType());
        }
        
        [Test]
        public void IndexingVec2_ReturnsGridNode_IsType()
        {
            int int10 = 10;
            PathfindingGrid grid = new PathfindingGrid(new Vec2(int10, int10));
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            int int1 = 1;
            
            Assert.AreEqual(typeof(GridNode),grid[int1,int1].GetType());
        }
        
        [Test]
        public void IndexingInt_SetNode_Equals()
        {
            Vec2 size = new Vec2(2, 2);
            TileType type = Tiles.Floor;
            PathfindingGrid grid = new PathfindingGrid(size);
            GridNode node = new GridNode(Tiles.Floor, Vec2.One, grid);
            
            Assert.IsNull(grid[0, 0]);

            grid[0, 0] = node;

            Assert.AreEqual(node,grid[0, 0]);
        }
        
        [Test]
        public void IndexingVec2_SetNode_IsType()
        {
            int int10 = 10;
            PathfindingGrid grid = new PathfindingGrid(new Vec2(int10, int10));
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            int int1 = 1;
            
            Assert.AreEqual(typeof(GridNode),grid[int1,int1].GetType());
        }
        
        #region Pathfinding Methods
        
        [Test]
        public void Fill_SetNode_Equals()
        {
            Vec2 vec2 = new Vec2(2, 2);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            
            Assert.IsNull(grid[0, 0]);
            Assert.IsNull(grid[0, 1]);
            Assert.IsNull(grid[1, 0]);
            Assert.IsNull(grid[1, 1]);
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            
            Assert.AreEqual(grid[0,0], new GridNode(Tiles.Stone, Vec2.Zero, grid));
            Assert.AreEqual(grid[0,1], new GridNode(Tiles.Stone, new Vec2(0, 1), grid));
            Assert.AreEqual(grid[1,0], new GridNode(Tiles.Stone, new Vec2(1,0), grid));
            Assert.AreEqual(grid[1,1], new GridNode(Tiles.Stone, Vec2.One, grid));
        }
        
        [Test]
        public void Fill_SetEdge_Equals()
        {
            Vec2 vec2 = new Vec2(2, 2);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            
            Assert.IsNull(grid[0, 0, Direction.N]);
            Assert.IsNull(grid[0, 0, Direction.W]);
            Assert.IsNull(grid[0, 0, Direction.E]);
            Assert.IsNull(grid[0, 0, Direction.S]);
            
            Assert.IsNull(grid[0, 1, Direction.N]);
            Assert.IsNull(grid[0, 1, Direction.W]);
            Assert.IsNull(grid[0, 1, Direction.E]);
            Assert.IsNull(grid[0, 1, Direction.S]);
            
            Assert.IsNull(grid[1, 0, Direction.N]);
            Assert.IsNull(grid[1, 0, Direction.W]);
            Assert.IsNull(grid[1, 0, Direction.E]);
            Assert.IsNull(grid[1, 0, Direction.S]);
            
            Assert.IsNull(grid[1, 1, Direction.N]);
            Assert.IsNull(grid[1, 1, Direction.W]);
            Assert.IsNull(grid[1, 1, Direction.E]);
            Assert.IsNull(grid[1, 1, Direction.S]);
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            grid.FillEdges((position, annotation) => new GridEdge(position, annotation));
            //grid.FillVertices((position) => new GridVertex(position));
            
            Assert.IsNotNull(grid[0, 0, Direction.N]);
            Assert.IsNotNull(grid[0, 0, Direction.W]);
            Assert.IsNotNull(grid[0, 0, Direction.E]);
            Assert.IsNotNull(grid[0, 0, Direction.S]);
            
            Assert.IsNotNull(grid[0, 1, Direction.N]);
            Assert.IsNotNull(grid[0, 1, Direction.W]);
            Assert.IsNotNull(grid[0, 1, Direction.E]);
            Assert.IsNotNull(grid[0, 1, Direction.S]);
            
            Assert.IsNotNull(grid[1, 0, Direction.N]);
            Assert.IsNotNull(grid[1, 0, Direction.W]);
            Assert.IsNotNull(grid[1, 0, Direction.E]);
            Assert.IsNotNull(grid[1, 0, Direction.S]);
            
            Assert.IsNotNull(grid[1, 1, Direction.N]);
            Assert.IsNotNull(grid[1, 1, Direction.W]);
            Assert.IsNotNull(grid[1, 1, Direction.E]);
            Assert.IsNotNull(grid[1, 1, Direction.S]);
            
            Assert.AreEqual(new GridEdge(Vec2.Zero, Direction.N), grid[0,0, Direction.N]);
            Assert.AreEqual(new GridEdge(Vec2.Zero, Direction.W), grid[0,0, Direction.W]);
            Assert.AreEqual(new GridEdge(new Vec2(1, 0), Direction.W), grid[0,0, Direction.E]);
            Assert.AreEqual(new GridEdge(new Vec2(0, 1), Direction.N), grid[0,0, Direction.S]);

            Assert.AreEqual(new GridEdge(new Vec2(0, 1), Direction.N), grid[0,1, Direction.N]);
            Assert.AreEqual(new GridEdge(new Vec2(0, 1), Direction.W), grid[0,1, Direction.W]);
            Assert.AreEqual(new GridEdge(new Vec2(1, 1), Direction.W), grid[0,1, Direction.E]);

            var expected = new GridEdge(new Vec2(0, 2), Direction.N);
            var actual = grid[0, 1, Direction.S];
            Assert.AreEqual(new GridEdge(new Vec2(0, 2), Direction.N), grid[0,1, Direction.S]);

            Assert.AreEqual(new GridEdge(new Vec2(1,0), Direction.N), grid[1,0, Direction.N]);
            Assert.AreEqual(new GridEdge(new Vec2(1,0), Direction.W), grid[1,0, Direction.W]);
            Assert.AreEqual(new GridEdge(new Vec2(2,0), Direction.W), grid[1,0, Direction.E]);
            Assert.AreEqual(new GridEdge(new Vec2(1,1), Direction.N), grid[1,0, Direction.S]);

            Assert.AreEqual(new GridEdge(new Vec2(1,1), Direction.N), grid[1,1, Direction.N]);
            Assert.AreEqual(new GridEdge(new Vec2(1,1), Direction.W), grid[1,1, Direction.W]);
            Assert.AreEqual(new GridEdge(new Vec2(2,1), Direction.W), grid[1,1, Direction.E]);
            Assert.AreEqual(new GridEdge(new Vec2(1,2), Direction.N), grid[1,1, Direction.S]);
        }
        
        [Test]
        public void FilteredNeighbors_Unconstrained_FourWayNeighbors()
        {
            Vec2 vec2 = new Vec2(5, 5);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            Vec2 tileCoordinates = new Vec2(3,3);
            
            Motility agentMotility = Motility.Unconstrained | Motility.FourWayNeighbors;
            List<GridNode> nodes = new List<GridNode>();
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(3, 2), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(4, 3), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(2, 3), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(3, 4), grid));
            
            CollectionAssert.AreEquivalent(nodes.AsReadOnly(), grid.FilteredNeighbors(grid[tileCoordinates], agentMotility));
        }

        [Test]
        public void FilteredNeighbors_Unconstrained_EightWayNeighbors()
        {
            Vec2 vec2 = new Vec2(5, 5);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            Vec2 tileCoordinates = new Vec2(3,3);
            
            Motility agentMotility = Motility.Unconstrained | Motility.EightWayNeighbors;
            List<GridNode> nodes = new List<GridNode>();
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(2, 2), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(3, 2), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(4, 2), grid));
            
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(2, 3), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(4, 3), grid));

            nodes.Add(new GridNode(Tiles.Stone, new Vec2(2, 4), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(3, 4), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(4, 4), grid));

            IReadOnlyCollection<GridNode> filteredNeighbors = grid.FilteredNeighbors(grid[tileCoordinates], agentMotility);
            
            CollectionAssert.AreEquivalent(nodes.AsReadOnly(), filteredNeighbors);
        }

        [Test]
        public void FilteredNeighbors_LandMotility_FourWayNeighbors()
        {
            Vec2 vec2 = new Vec2(5, 5);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            Vec2 tileCoordinates = new Vec2(3,3);
            
            Motility agentMotility = Motility.Land | Motility.FourWayNeighbors;
            List<GridNode> nodes = new List<GridNode>();
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            grid[2,3] = new GridNode(Tiles.Floor, new Vec2(2, 3), grid);
            grid[3, 4] = new GridNode(Tiles.Floor, new Vec2(3, 4), grid);

            nodes.Add(new GridNode(Tiles.Floor, new Vec2(2, 3), grid));
            nodes.Add(new GridNode(Tiles.Floor, new Vec2(3, 4), grid));

            CollectionAssert.AreEquivalent(nodes.AsReadOnly(), grid.FilteredNeighbors(grid[tileCoordinates], agentMotility));
        }

        #region Face Relationships

        [Test]
        public void Neighbors_VariousDirections_CountMatches()
        {
            Vec2 vec2 = new Vec2(5, 5);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            Vec2 tileCoordinates = new Vec2(3,3);
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            
            Assert.AreEqual(4, grid.Neighbors(grid[tileCoordinates], Direction.CardinalDirections).Count);
            Assert.AreEqual(8, grid.Neighbors(grid[tileCoordinates], Direction.Clockwise).Count);
            Assert.AreEqual(0, grid.Neighbors(grid[tileCoordinates], new List<Direction>{Direction.None}).Count);
        }

        [Test]
        public void Neighbors_DefaultDirectionOverload_CountMatches()
        {
            Vec2 vec2 = new Vec2(5, 5);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            Vec2 tileCoordinates = new Vec2(3,3);
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            
            Assert.AreEqual(8, grid.Neighbors(grid[tileCoordinates]).Count);
        }

        [Test]
        public void Neighbors_DefaultDirections_IncludeAllClockwiseDirections()
        {
            Vec2 vec2 = new Vec2(5, 5);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            Vec2 tileCoordinates = new Vec2(3, 3);
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            
            List<GridNode> nodes = new List<GridNode>();
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(2, 2), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(3, 2), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(4, 2), grid));
            
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(2, 3), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(4, 3), grid));

            nodes.Add(new GridNode(Tiles.Stone, new Vec2(2, 4), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(3, 4), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(4, 4), grid));
            
            foreach (Direction direction in Direction.Clockwise)
            {
                GridNode neighbor = new GridNode(Tiles.Stone, tileCoordinates + direction, grid);
                Assert.Contains(neighbor, grid.Neighbors(grid[tileCoordinates]));
            }
        }
        
        [Test]
        public void Neighbors_DefaultDirections_IncludeAllClockwiseDirectionsHardcoded()
        {
            Vec2 vec2 = new Vec2(5, 5);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            Vec2 tileCoordinates = new Vec2(3, 3);
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            
            List<GridNode> nodes = new List<GridNode>();
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(2, 2), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(3, 2), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(4, 2), grid));
            
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(2, 3), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(4, 3), grid));

            nodes.Add(new GridNode(Tiles.Stone, new Vec2(2, 4), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(3, 4), grid));
            nodes.Add(new GridNode(Tiles.Stone, new Vec2(4, 4), grid));
            
            foreach (GridNode node in nodes)
            {
                Assert.Contains(node, grid.Neighbors(grid[tileCoordinates]));
            }
        }
        
        [Test]
        public void Neighbors_ClockwiseDirections_IncludeAllClockwiseDirections()
        {
            Vec2 vec2 = new Vec2(5, 5);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            Vec2 tileCoordinates = new Vec2(3, 3);
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            
            foreach (Direction direction in Direction.Clockwise)
            {
                GridNode neighbor = new GridNode(Tiles.Stone, tileCoordinates + direction, grid);
                Assert.Contains(neighbor, grid.Neighbors(grid[tileCoordinates], Direction.Clockwise));
            }
        }
        
        [Test]
        public void Neighbors_CardinalDirections_IncludeAllCardinalDirections()
        {
            Vec2 vec2 = new Vec2(5, 5);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            Vec2 tileCoordinates = new Vec2(3, 3);
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            
            foreach (Direction direction in Direction.CardinalDirections)
            {
                GridNode neighbor = new GridNode(Tiles.Stone, tileCoordinates + direction, grid);
                Assert.Contains(neighbor, grid.Neighbors(grid[tileCoordinates], Direction.CardinalDirections));
            }
        }
        
        [Test]
        public void Neighbors_IntercardinalDirections_IncludeAllIntercardinalDirections()
        {
            Vec2 vec2 = new Vec2(5, 5);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            Vec2 tileCoordinates = new Vec2(3, 3);
            
            grid.Fill(position => new GridNode(Tiles.Stone, position, grid));
            
            foreach (Direction direction in Direction.IntercardinalDirections)
            {
                GridNode neighbor = new GridNode(Tiles.Stone, tileCoordinates + direction, grid);
                Assert.Contains(neighbor, grid.Neighbors(grid[tileCoordinates], Direction.IntercardinalDirections));
            }
        }
        
        #endregion
        
        #endregion

        /*[Test]
        public void GetEdgeOrVertIndex_GetIndex()
        {
            Vec2 vec2 = new Vec2(5, 5);
            PathfindingGrid grid = new PathfindingGrid(vec2);
            
            // x=0, y=0: N=0, W=6, E=7, S=13
            // x=1, y=0: N=1, W=7, E=8, S=14
            // x=0, y=1: N=13, W=19, E=20, S=26
            // x=1, y=1: N=14, W=20, E=21, S=27
            // x=0, y=2  N=26

            // x=0, y=0
            int index = grid.GetEdgeOrVertIndex(0, 0, Direction.N);
            Assert.AreEqual(0, index);
            
            index = grid.GetEdgeOrVertIndex(0, 0, Direction.W);
            Assert.AreEqual(6, index);
            
            index = grid.GetEdgeOrVertIndex(0, 0, Direction.E);
            Assert.AreEqual(7, index);
            
            index = grid.GetEdgeOrVertIndex(0, 0, Direction.S);
            Assert.AreEqual(12, index);
            
            // x=1, y=0
            index = grid.GetEdgeOrVertIndex(1, 0, Direction.N);
            Assert.AreEqual(1, index);
            
            index = grid.GetEdgeOrVertIndex(1, 0, Direction.W);
            Assert.AreEqual(7, index);
            
            index = grid.GetEdgeOrVertIndex(1, 0, Direction.E);
            Assert.AreEqual(8, index);
            
            index = grid.GetEdgeOrVertIndex(1, 0, Direction.S);
            Assert.AreEqual(13, index);

            // x=1, y=1
            index = grid.GetEdgeOrVertIndex(1, 1, Direction.N);
            Assert.AreEqual(13, index);
            
            index = grid.GetEdgeOrVertIndex(1, 1, Direction.W);
            Assert.AreEqual(19, index);
            
            index = grid.GetEdgeOrVertIndex(1, 1, Direction.E);
            Assert.AreEqual(20, index);
            
            index = grid.GetEdgeOrVertIndex(1, 1, Direction.S);
            Assert.AreEqual(25, index);
            
        }*/

    }
}
