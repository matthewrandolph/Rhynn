using System;
using System.Collections.Generic;
using System.Linq;
using Util;
using Util.Pathfinding;

namespace Rhynn.Engine
{
    /// <summary>
    /// One square of a 2D BattleMap
    /// </summary>
    [Serializable]
    public class GridTile : IPathfindingNode
    {

        public GridTile(TileType type, Vec2 position)
        {
            _type = type;
            _neighbors = new Dictionary<IPathfindingNode, IPathfindingEdge>();
            _position = position;
        }

        #region IPathfindingNode Members
        
        public TileType Type
        {
            get => _type;
            set => _type = value;
        }

        public float PathCost { get; set; }

        public Vec2 Position => _position;

        public IDictionary<IPathfindingNode, IPathfindingEdge> NeighborMap => _neighbors;

        public void AddNeighbor(IPathfindingNode neighbor, float weight, Direction direction)
        {
            _neighbors.Add(neighbor, new WeightedEdge(this, neighbor, weight, direction));
        }

        /// <summary>
        /// Whether its possible for an <see cref="Actor"/> with the given motility to enter this tile.
        /// </summary>
        /// <param name="motility"></param>
        /// <returns></returns>
        public bool CanEnter(Motility motility)
        {
            return Type.CanEnter(motility);
        }

        /// <summary>
        /// Identifies if a given node is a neighbor of this node and is traversable.
        /// </summary>
        /// <remarks>This does not perform pathfinding. It only looks at its direct neighbors.</remarks>
        /// <param name="end">The neighbor node to traverse to.</param>
        /// <param name="motility">The agent's motility.</param>
        /// <returns>Whether or not the agent can traverse along an edge to the given node given its
        ///     current motility.</returns>
        public bool IsTraversableTo(IPathfindingNode end, Motility motility)
        {
            if (!_neighbors.TryGetValue(end, out IPathfindingEdge edge)) return false;

            return edge.IsTraversable(motility);
        }

        /// <summary>
        /// Adds the Traversable flags to the edge going to its neighbors. This requires all edges be two way,
        /// although its okay if one of them is set to Traversable.None.
        /// </summary>
        /// <remarks>This modifies "incoming" edges only. Outgoing edges are unaffected.</remarks>
        /// <param name="motility">The motility flags to add.</param>
        public void SetOutgoingTraversableFlag(Motility motility)
        {
            // Update edges pointing to neighbors (i.e outgoing edges) that the end tile has new motility
            foreach (KeyValuePair<IPathfindingNode, IPathfindingEdge> entry in _neighbors)
            {
                IPathfindingEdge neighborEdge = entry.Value;
                neighborEdge.SetMotilityFlag(motility);
            }
        }

        /// <summary>
        /// Removes the Traversable flags from the edge going to its neighbors. This requires all edges be two
        /// way, although its okay if one of them is set to Traversable.None.
        /// </summary>
        /// <remarks>This modifies "incoming" edges only. Outgoing edges are unaffected.</remarks>
        /// <param name="motility"></param>
        public void UnsetOutgoingTraversableFlag(Motility motility)
        {
            // Update edges pointing to neighbors (i.e outgoing edges) that the end tile has new traversability
            foreach (KeyValuePair<IPathfindingNode, IPathfindingEdge> entry in _neighbors)
            {
                IPathfindingEdge neighborEdge = entry.Value;
                neighborEdge.UnsetMotilityFlag(motility);
            }
        }
        
        /// <summary>
        /// Adds the Traversable flags to the edge going to its neighbors. This requires all edges be two way,
        /// although its okay if one of them is set to Traversable.None.
        /// </summary>
        /// <remarks>This modifies "incoming" edges only. Outgoing edges are unaffected.</remarks>
        /// <param name="motility">The motility flags to add.</param>
        public void SetIncomingTraversableFlag(Motility motility)
        {
            // Update neighbor's edges (i.e incoming edges) that this tile has new traversability
            foreach (KeyValuePair<IPathfindingNode, IPathfindingEdge> entry in _neighbors)
            {
                IPathfindingNode neighborNode = entry.Key;
                neighborNode.SetOutgoingTraversableFlag(motility);
            }
        }

        /// <summary>
        /// Removes the Traversable flags from the edge going to its neighbors. This requires all edges be two
        /// way, although its okay if one of them is set to Traversable.None.
        /// </summary>
        /// <remarks>This modifies "incoming" edges only. Outgoing edges are unaffected.</remarks>
        /// <param name="motility"></param>
        public void UnsetIncomingTraversableFlag(Motility motility)
        {
            // Update neighbor's edges (i.e incoming edges) that this tile has new traversability
            foreach (KeyValuePair<IPathfindingNode, IPathfindingEdge> entry in _neighbors)
            {
                IPathfindingNode neighborNode = entry.Key;
                neighborNode.UnsetOutgoingTraversableFlag(motility);
            }
        }

        #endregion
        
        private TileType _type;
        private IDictionary<IPathfindingNode, IPathfindingEdge> _neighbors;
        private Vec2 _position;
    }

    public struct TileType
    {
        public readonly String Name;
        public readonly Motility Motility;
        public readonly int SpriteIndex;

        public TileType(string name, Motility motility, int spriteIndex)
        {
            Name = name;
            Motility = motility;
            SpriteIndex = spriteIndex;
        }

        public bool IsWalkable => Motility.Contains(Motility.Land);
        public bool CanEnter(Motility motility) => motility.Contains(Motility);

        #region Operators

        public static bool operator ==(TileType type1, TileType type2)
        {
            //if (type1 is null) return type2 is null;
            return type1.Equals(type2);
        }
        
        public static bool operator !=(TileType type1, TileType type2)
        {
            //if (type1 is null) return !(type2 is null);
            return !type1.Equals(type2);
        }
        
        #endregion

        #region IEquatable<Motility> Members

        public bool Equals(TileType other)
        {
            //if (ReferenceEquals(null, other)) return false;
            //if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Motility == other.Motility;
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            //if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TileType) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Motility.GetHashCode();
        }
        
        #endregion
    }
}