using System;
using System.Collections.Generic;
using Util;

namespace Engine
{
    /// <summary>
    /// One square of a 2D BattleMap
    /// </summary>
    [Serializable]
    public class GridTile : IPathfindingNode
    {
        public TileType Type
        {
            get => _type;
            set => _type = value;
        }

        public GridTile(TileType type)
        {
            _type = type;
            _neighbors = new Dictionary<IPathfindingNode, IPathfindingEdge>();
        }

        #region IPathfindingNode Members

        public float PathCost { get; set; }

        public Vec2 Position { get; set; }

        public IDictionary<IPathfindingNode, IPathfindingEdge> NeighborMap => _neighbors;

        public void AddNeighbor(IPathfindingNode neighbor, float weight, Direction direction)
        {
            _neighbors.Add(neighbor, new WeightedEdge(this, neighbor, weight, direction));
        }

        /// <summary>
        /// Identifies if a given node is a neighbor of this node and is traversable.
        /// </summary>
        /// <remarks>This does not perform pathfinding. It only looks at its direct neighbors.</remarks>
        /// <param name="traversable">The agent's traversability.</param>
        /// <param name="end">The neighbor node to traverse to.</param>
        /// <returns>Whether or not the agent can traverse along an edge to the given node given its
        ///     current traversability.</returns>
        public bool IsTraversableTo(Traversable traversable, IPathfindingNode end)
        {
            if (!_neighbors.TryGetValue(end, out IPathfindingEdge edge)) return false;

            return edge.IsTraversable(traversable);
        }

        /// <summary>
        /// Adds the Traversable flags to the traversability of its neighbors. This requires all edges be two way,
        /// although its okay if one of them is set to Traversable.None.
        /// </summary>
        /// <remarks>This modifies "incoming" edges only. Outgoing edges are unaffected.</remarks>
        /// <param name="traversable">The traversability flags to add.</param>
        public void SetTraversableFlag(Traversable traversable)
        {
            // Update neighbor's edges (i.e incoming edges) that this tile has new traversability
            foreach (KeyValuePair<IPathfindingNode, IPathfindingEdge> entry in _neighbors)
            {
                IPathfindingEdge neighborEdge = entry.Value;
                neighborEdge.SetTraversableFlag(traversable);
            }
        }

        /// <summary>
        /// Removes the Traversable flags from the traversability of its neighbors. This requires all edges be two
        /// way, although its okay if one of them is set to Traversable.None.
        /// </summary>
        /// <remarks>This modifies "incoming" edges only. Outgoing edges are unaffected.</remarks>
        /// <param name="traversable"></param>
        public void UnsetTraversableFlag(Traversable traversable)
        {
            // Update neighbor's edges (i.e incoming edges) that this tile has new traversability
            foreach (KeyValuePair<IPathfindingNode, IPathfindingEdge> entry in _neighbors)
            {
                IPathfindingEdge neighborEdge = entry.Value;
                neighborEdge.UnsetTraversableFlag(traversable);
            }
        }

        #endregion
        
        private TileType _type;
        private IDictionary<IPathfindingNode, IPathfindingEdge> _neighbors;
        private Vec2 _position;
    }

    public enum TileType
    {
        Unknown,
        Floor,
        Wall,
        Stone
    }
}