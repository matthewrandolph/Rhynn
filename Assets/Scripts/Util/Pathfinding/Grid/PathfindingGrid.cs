using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Rhynn.Engine;
using UnityEngine;
using Util.Pathfinding.SearchAlgorithms;

namespace Util.Pathfinding
{
    public class PathfindingGrid
    {
        /// <summary>
        /// Gets the size of the grid.
        /// </summary>
        public Vec2 Size => new Vec2(Width, Height);
        
        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        public int Width { get; }
        
        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        public int Height => _nodes.Length / Width;
        
        public GridNode this[Vec2 position]
        {
            get => this[position.x, position.y];
            set => this[position.x, position.y] = value;
        }

        public GridNode this[int x, int y]
        {
            get => _nodes[GetIndex(x, y)];
            set => _nodes[GetIndex(x, y)] = value;
        }

        public IPathfindingEdge this[Vec2 position, Direction direction]
        {
            get => this[position.x, position.y, direction];
            set => this[position.x, position.y, direction] = value;
        }

        public IPathfindingEdge this[int x, int y, Direction direction]
        {
            get 
            { 
                if (Direction.CardinalDirections.Contains(direction))
                {
                    if (_edges.TryGetValue(GetEdgeLocation(x, y, direction), out GridEdge edge))
                    {
                        return edge;
                        //return _edges[new GridPoint(x, y, direction)];
                    }
                    //return _edges[GetEdgeOrVertIndex(x, y, direction)];
                }

                if (Direction.IntercardinalDirections.Contains(direction))
                {
                    if (_vertices.TryGetValue(GetEdgeLocation(x, y, direction), out GridVertex vertex))
                    {
                        return vertex;
                        //return _vertices[new GridPoint(x, y, direction)];
                    }
                    //return _vertices[GetEdgeOrVertIndex(x, y, direction)];
                }

                return null;
                
                //throw new NotImplementedException("Unrecognized direction, currently implements the Cardinal or Intercardinal directions only.");
            }
            set // TODO: Consider making this separate functions for GridEdges and GridVertices
            { 
                if (Direction.CardinalDirections.Contains(direction))
                {
                    GridEdge edge = value as GridEdge;
                    edge = new GridEdge(edge.Weight, edge.Motility, new Vec2(x, y), direction);
                    
                    SetEdge(x, y, direction, edge);
                    //_edges[GetEdgeOrVertIndex(x, y, direction)] = value as GridEdge;
                }
                else if (Direction.IntercardinalDirections.Contains(direction))
                {
                    GridVertex vertex = value as GridVertex;
                    vertex = new GridVertex(vertex.Weight, vertex.Motility, new Vec2(x, y));
                    
                    SetVertex(x, y, direction, value as GridVertex);
                    //_vertices[GetEdgeOrVertIndex(x, y, direction)] = value as GridVertex;
                }
                else
                {
                    throw new NotImplementedException("Unrecognized direction, currently implements the Cardinal or Intercardinal directions only.");
                }
            }
        }

        private void SetEdge(int x, int y, Direction direction, GridEdge edge)
        {
            if (!Direction.CardinalDirections.Contains(direction))
            {
                throw new InvalidOperationException("\"direction\" must be a CardinalDirection to be able to use SetEdge.");
            }
            
/*            // All edges are stored as either the North or West edge of a tile.
            if (direction == Direction.E)
            {
                x += 1;
                direction = Direction.W;
            } 
            else if (direction == Direction.S)
            {
                y += 1;
                direction = Direction.N;
            }*/

            GridPoint gridPoint = GetEdgeLocation(x, y, direction);
            edge = new GridEdge(edge.Weight, edge.Motility, gridPoint.Position, gridPoint.Annotation);
            _edges[gridPoint] = edge;
        }

        private void SetVertex(int x, int y, Direction direction, GridVertex vertex)
        {
            if (!Direction.IntercardinalDirections.Contains(direction))
            {
                throw new InvalidOperationException("\"direction\" must be an IntercardinalDirection to be able to use SetVertex.");
            }
            
/*            // All vertices are stored as the NorthWest vertex of a tile.
            if (direction == Direction.NE) { x -= 1; } 
            else if (direction == Direction.SW) { y += 1; }
            else if (direction == Direction.SE) { x -= 1; y += 1; }

            direction = Direction.NW;*/
            
            _vertices[GetEdgeLocation(x, y, direction)] = vertex;
        }

        private GridPoint GetEdgeLocation(int x, int y, Direction direction)
        {
            // All edges are stored as either the North or West edge of a tile.
            if (direction == Direction.E)
            {
                x += 1;
                direction = Direction.W;
            } 
            
            else if (direction == Direction.S)
            {
                y += 1;
                direction = Direction.N;
            }
            
            // All vertices are stored as the NorthWest vertex of a tile.
            else if (direction == Direction.NE)
            {
                x -= 1;
                direction = Direction.NW;
            } 
            
            else if (direction == Direction.SW)
            {
                y += 1;
                direction = Direction.NW;
            }
            
            else if (direction == Direction.SE)
            {
                x -= 1; 
                y += 1;
                direction = Direction.NW;
            }
            
            return new GridPoint(x, y, direction);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PathfindingGrid"/> with the given dimensions.
        /// </summary>
        public PathfindingGrid(int width, int height)
        {
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width),"Width must be greater than zero.");
            if (height < 0) throw new ArgumentOutOfRangeException(nameof(height),"Height must be greater than zero.");

            Width = width;
            
            _nodes = new GridNode[width * height];
            _edges = new Dictionary<GridPoint, GridEdge>();
            _vertices = new Dictionary<GridPoint, GridVertex>();
            
            //_edges = new GridEdge[2 * (width + 1) * (height + 1)];
            //_vertices = new GridVertex[(width + 1) * (height + 1)];
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PathfindingGrid"/> with the given size.
        /// </summary>
        public PathfindingGrid(Vec2 size) : this(size.x, size.y) { }
        
        /// <summary>
        /// Fills all of the <see cref="GridNode"/>s in the grid with values returned by the given callback.
        /// </summary>
        /// <param name="callback">The function to call for each node.</param>
        public void Fill(Func<Vec2, GridNode> callback)
        {
            foreach (Vec2 position in new Rect(Size))
            {
                this[position] = callback(position);
            }
        }
        
        /// <summary>
        /// Fills all of the <see cref="GridEdge"/>s in the grid with values returned by the given callback.
        /// </summary>
        /// <param name="callback">The function to call for each node.</param>
        public void FillEdges(Func<Vec2, Direction, GridEdge> callback)
        {
            foreach (Vec2 position in new Rect(Size))
            {
                foreach (Direction annotation in Direction.CardinalDirections)
                {
                    this[position, annotation] = callback(position, annotation);
                }
            }
        }
        
        /// <summary>
        /// Fills all of the <see cref="GridVertex">GridVertices</see> in the grid with values returned by the given callback.
        /// </summary>
        /// <param name="callback">The function to call for each node.</param>
        public void FillVertices(Func<Vec2, GridVertex> callback)
        {
            foreach (Vec2 position in new Rect(Size))
            {
                foreach (Direction annotation in Direction.IntercardinalDirections)
                {
                    this[position, annotation] = callback(position);
                }
            }
        }
        
        #region Pathfinding Functions
        
        public IList<GridNode> FindPath<TSearchAlgorithm>(GridNode start, GridNode goal, Motility agentMotility) 
            where TSearchAlgorithm : ISearchAlgorithm, new()
        {
            throw new NotImplementedException();
        }

        public IList<GridNode> FindPath<TSearchAlgorithm>(GridNode start, GridNode goal, Motility agentMotility, 
            Func<GridNode, GridNode, float> heuristic) where TSearchAlgorithm : ISearchAlgorithm, new()
        {
            throw new NotImplementedException();
        }

        public IDictionary<GridNode, GridNode> FloodFill<TFloodFillAlgorithm>(GridNode start, float searchDepth, 
            Motility agentMotility) where TFloodFillAlgorithm : IFloodFillAlgorithm, new()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Gets the <see cref="GridNode"/>s that the agent is able to reach traversing a single edge. Filters out
        /// neighbors based on the flags set in <see cref="agentMotility"/>.
        /// </summary>
        /// <param name="tile">The tile to get the neighbors of.</param>
        /// <param name="agentMotility">The agent's motility. If the agent does not have the appropriate motility to reach
        /// a node, it is not returned as a neighbor.</param>
        /// <returns>A list of GridNode neighbors that the agent can traverse to.</returns>
        public ReadOnlyCollection<GridNode> FilteredNeighbors(GridNode tile, Motility agentMotility)
        {
            IList<Direction> directions = new List<Direction>();

            // Identify the graph structure for this agent and reject extraneous neighbors.
            if (agentMotility.Contains(Motility.AllNeighbors))
            {
                // Most permissive option is true, so add all nodes
                directions = Direction.Clockwise;
                directions.Add(Direction.None);
            }
            else if (agentMotility.Contains(Motility.EightWayNeighbors))
            {
                // Reject only special nodes (such as portals)
                directions = Direction.Clockwise;
            }
            else if (agentMotility.Contains(Motility.FourWayNeighbors))
            {
                // Reject the intercardinal directions (diagonals) as well as any special nodes (such as portals)
                directions = Direction.CardinalDirections;
            }
            else
            {
                // This is an implicit "has no neighbors flag" flag, which is just a lack of any neighbor flag at all.
                // In this case we default to Motility.AllNeighbors and continue.
                directions = Direction.Clockwise;
                directions.Add(Direction.None);
            }

            // Agent ignores any other motility, so just return all nodes we found.
            if (agentMotility.Contains(Motility.Unconstrained)) // NOTE: This is currently NOT honoring a node marked as "Motility.Unconstrained" unless the agent is marked that way because that doesn't seem valid 
            {
                return Neighbors(tile, directions);
            }
            
            List<GridNode> neighbors = new List<GridNode>();
            
            foreach (GridNode neighbor in Neighbors(tile, directions))
            {
                // Check if we can traverse to that neighbor
                if (neighbor.CanEnter(tile, agentMotility))
                {
                    neighbors.Add(neighbor);
                }
                
                /*// Get the edge (or vertex) and ask if it is traversable
                IPathfindingEdge edge = tile.JoiningEdge(neighbor);
                
                // Check the agent's movement Motility compared to the edge Motility
                if (edge.IsTraversable(agentMotility))
                {
                    neighbors.Add(neighbor);
                }*/
            }

            return neighbors.AsReadOnly();
        }
        
        /// <summary>
        /// Resets all <see cref="GridNode"/>'s PathCosts to Mathf.Infinity except for the starting node.
        /// </summary>
        /// <param name="start">The starting node. This node's PathCost is set to 0.</param>
        private void InitializeCosts(GridNode start)
        {
            throw new NotImplementedException();
            foreach (GridNode node in _nodes)
            {
                node.PathCost = Mathf.Infinity;
            }

            start.PathCost = 0;
        }
        
        #endregion
        
        #region Grid Functions
        
        #region Face Relationships

        /// <summary>
        /// Convenience method for <see cref="Neighbors(GridNode, IEnumerable{Direction})"/> using
        /// <see cref="Direction.Clockwise"/> as 'directions'.
        /// </summary>
        /// <returns></returns>
        public ReadOnlyCollection<GridNode> Neighbors(GridNode tile)
        {
            return Neighbors(tile, Direction.Clockwise);
        }

        /// <summary>
        /// Returns the <see cref="GridNode"/>s that neighbor the GridNode along one of the <see cref="Direction"/>s
        /// given.
        /// </summary>
        /// <example>
        /// Passing in <see cref="Direction.CardinalDirections"/> will result in all NodeB's, assuming that tile is NodeA:
        ///   #   | NodeB |   #
        /// NodeB · NodeA · NodeB
        ///   #   | NodeB |   #
        /// </example>
        /// <example>
        /// Passing in <see cref="Direction.IntercardinalDirections"/> will result in these NodeB's, assuming that
        /// tile is NodeA:
        /// NodeB |   #   | NodeB
        ///   #   · NodeA ·   #
        /// NodeB |   #   | NodeB
        /// </example>
        /// <remarks>Neighbors that would fall outside of the map's bounds are ignored.
        /// TODO: Passing in 'Direction.None' will result in special nodes (such as portals) being returned.</remarks>
        /// <param name="tile">The node to find the neighbors of.</param>
        /// <param name="directions">The directions in which to look for neighbors.</param>
        public ReadOnlyCollection<GridNode> Neighbors(GridNode tile, IEnumerable<Direction> directions)
        {
            //throw new NotImplementedException();
            List<GridNode> neighbors = new List<GridNode>();

            foreach (Direction direction in directions)
            {
                if (direction == Direction.None)
                {
                    // TODO: Add all special neighbors here (this is a portal or something)
                    continue;
                }
                
                Vec2 neighborCoordinate = tile.Position + direction.Offset;
                neighbors.Add(this[neighborCoordinate]);
            }
            
            return neighbors.AsReadOnly();
        }

        public ReadOnlyCollection<GridEdge> Borders(GridNode tile)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<GridVertex> Corners(GridNode tile)
        {
            throw new NotImplementedException();
        }
        
        #endregion
        
        #region Edge Relationships

        /// <summary>
        /// The <see cref="GridNode"/>s an agent can reach through the <see cref="GridEdge"/>.
        /// <example>
        /// <see cref="Joins"/> will return both NodeB's, assuming the edge is EdgeA:
        /// # | NodeB | #
        /// - · EdgeA · -
        /// # | NodeB | #
        /// </example>
        /// </summary>
        public ReadOnlyCollection<GridNode> Joins(GridEdge edge)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The <see cref="GridEdge"/>s that share a <see cref="GridVertex"/> with this edge.
        /// <example>
        /// <see cref="Connects"/> will return both EdgeB's, assuming the edge is EdgeA:
        ///   #   |   #   |   #
        /// EdgeB . EdgeA . EdgeB
        ///   #   |   #   |   #
        /// </example>
        /// </summary>
        public ReadOnlyCollection<GridEdge> Connects(GridEdge edge)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The endpoint <see cref="GridVertex{T}"/>s of the <see cref="GridEdge{T}"/>.
        /// <example>
        /// <see cref="Endpoints"/> will return both VertB's, assuming the edge is EdgeA:
        ///   |     #    |
        /// VertB EdgeA VertB
        ///   |     #    |
        /// </example>
        /// </summary>
        public ReadOnlyCollection<GridVertex> Endpoints(GridEdge edge)
        {
            throw new NotImplementedException();
        }
        
        #endregion
        
        #region Vertex Relationships

        public ReadOnlyCollection<GridNode> Touches(GridVertex tile)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<GridEdge> Protrudes(GridVertex vertex)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<GridVertex> Adjacent(GridVertex vertex)
        {
            throw new NotImplementedException();
        }
        
        #endregion
        
        #endregion
        
        private int GetIndex(int x, int y)
        {
            return (y * Width) + x;
        }

        /*public void PrintIndexTest()
        {
            foreach (GridNode node in _nodes)
            {
                Debug.Log($"The Edge and Vertex indices for the tile at {node.Position} are:");
                foreach (Direction direction in Direction.Clockwise)
                {
                    Debug.Log($"Direction: {direction}");
                    int index = GetEdgeOrVertIndex(node.Position.x, node.Position.y, direction);
                    Debug.Log($"Index: {index}");
                }
            }
        }*/

        public int GetEdgeOrVertIndex(int x, int y, Direction annotation)
        {
            if (annotation == Direction.NW)
                return (y * Width) + x;
            if (annotation == Direction.NE)
                return (y * Width) + x + 1;
            if (annotation == Direction.SW)
                return (y * Width + 1) + x;
            if (annotation == Direction.SE)
                return (y * Width + 1) + x + 1;
            
            // x=0, y=0: N=0, W=6, E=7, S=13
            // x=1, y=0: N=1, W=7, E=8, S=14
            // x=0, y=1: N=13, W=19, E=20, S=26
            // x=1, y=1: N=14, W=20, E=21, S=27
            // x=0, y=2  N=26
            
            // (y * Width * 2) = 0, 12, 24, 36
            // y = 0, 1, 2, 3
            
            // (y * Width * 2) + (y * 2) + x
            // width * height * 2 + 1 * height

            if (annotation == Direction.N)
                return 2 * (Width + 1) * y + x;
            //return (y * Width * 2) + (y * 2) + x;
            if (annotation == Direction.W)
                return 2 * (Width + 1) * y + x + (Width + 1);
            //return (y * Width * 2) + Width + (y * 2) + x;
            if (annotation == Direction.E) // West with x+1
                return 2 * (Width + 1) * y + (x + 1) + (Width + 1);
            //return (y * Width * 2) + Width + (y * 2) + (x + 1);
            if (annotation == Direction.S) // North with y+1
                return 2 * (Width + 1) * (y + 1) + x;
            //return ((y + 1) * Width * 2) + (y * 2) + x;
            
            throw new InvalidOperationException("Direction \"annotation\" must be one of the cardinal or intercardinal directions.");
        }

        private GridNode[] _nodes;

        private Dictionary<GridPoint, GridEdge> _edges;
        private Dictionary<GridPoint, GridVertex> _vertices;
        //private GridEdge[] _edges;
        //private GridVertex[] _vertices;
    }
}