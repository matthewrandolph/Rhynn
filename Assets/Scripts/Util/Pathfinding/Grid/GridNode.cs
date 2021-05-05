using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rhynn.Engine;

namespace Util.Pathfinding
{
    public class GridNode : IEquatable<GridNode>
    {
        /// <summary>
        /// The running cost of traversing to this <see cref="GridNode"/> of the most recent path found. If not part
        /// of the path, it will be <c>Mathf.Infinity</c>.
        /// </summary>
        public float PathCost { get; set; }

        /// <summary>
        /// The <see cref="GridNode"/>'s location in the <see cref="PathfindingGrid"/>
        /// </summary>
        public Vec2 Position { get; }
        
        public TileType Type { get; }

        /// <summary>
        /// The <see cref="Motility"/> of the <see cref="GridNode"/> based on its <see cref="TileType"/> and any
        /// object within the node restricting movement.
        /// </summary>
        public Motility Motility
        {
            get
            {
                if (_motility.Contains(Motility.Unconstrained)) return Type.Motility;
                if (Type.Motility.Contains(Motility.Unconstrained)) return _motility;
                return Type.Motility & _motility;
            }
        }

        /// <summary>
        /// The collection of this node's neighboring <see cref="GridNode"/>s.
        /// </summary>
        public ReadOnlyCollection<GridNode> Neighbors => _grid.Neighbors(this);

        public ReadOnlyCollection<GridEdge> Borders
        {
            get => throw new System.NotImplementedException();
            
            // return the edges that border this node
        }

        public ReadOnlyCollection<GridVertex> Corners
        {
            get => throw new System.NotImplementedException();
            
            // return the vertices that border this node
        }
        
        #region Operators

        public static bool operator ==(GridNode left, GridNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GridNode left, GridNode right)
        {
            return !Equals(left, right);
        }

        #endregion
        
        /// <summary>
        /// Initializes a new GridNode.
        /// </summary>
        /// <param name="type">The type of tile.</param>
        /// <param name="contentMotility">The motility of something residing in the node.</param>
        /// <param name="position">The position of the node.</param>
        public GridNode(TileType type, Motility contentMotility, Vec2 position, PathfindingGrid grid)
        {
            Type = type;
            _motility = contentMotility;
            Position = position;
            _grid = grid;
        }
        
        public GridNode(TileType type, Vec2 position, PathfindingGrid grid) : this(type, Motility.Unconstrained, position, grid) { }

        /// <summary>
        /// Returns <c>true</c> when this <see cref="GridNode"/> can be traversed to from the given neighbor using one
        /// (or more) of the passed in <see cref="Motility">motilities</see>.
        /// </summary>
        public bool CanEnter(GridNode neighbor, Motility motility)
        {
            // Check internal motility
            if (!motility.Contains(Motility)) { return false; }
            
            // Check motility of edge connecting this and neighbor
            if (!JoiningEdge(neighbor).IsTraversable(motility)) { return false; }
            
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returns <c>true</c> when this <see cref="GridNode"/> would not prevent traversal based on the passed in
        /// <see cref="Motility"/>. This does not check its <see cref="GridEdge"/>s.
        /// </summary>
        /// <param name="motility">The motility to check this node with.</param>
        public bool AllowsMotility(Motility motility)
        {
            return Motility.Contains(motility);
        }
        
        /// <summary>
        /// Returns the <see cref="IPathfindingEdge{T}"/> that joins this node and the passed in node.
        /// </summary>
        /// <param name="node">The neighbor <see cref="GridNode"/></param>
        /// <exception cref="InvalidOperationException">Thrown when a node not connected to this node is passed in as a
        /// parameter.</exception>
        public IPathfindingEdge JoiningEdge(GridNode node)
        {
            Direction direction = Direction.Towards(node.Position - this.Position);

            // Standard grid connection
            if (direction != Direction.None)
            {
                return _grid[this.Position, direction];
            }

            // Special connection
            if (direction == Direction.None)
            {
                throw new NotImplementedException();
            }

            throw new InvalidOperationException("The passed in node is not connected to this node.");
        }

        #region IEquatable<GridNode> Members

        public bool Equals(GridNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _motility.Equals(other._motility) && PathCost.Equals(other.PathCost) && Position.Equals(other.Position) && Type.Equals(other.Type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GridNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _motility.GetHashCode();
                hashCode = (hashCode * 397) ^ PathCost.GetHashCode();
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Type.GetHashCode();
                return hashCode;
            }
        }

        #endregion

        /// <summary>
        /// The motility based on the tile's contents
        /// </summary>
        private Motility _motility;

        private PathfindingGrid _grid;
    }
}