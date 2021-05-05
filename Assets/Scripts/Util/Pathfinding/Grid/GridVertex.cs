using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rhynn.Engine;
using UnityEngine;

namespace Util.Pathfinding
{
    /// <summary>
    /// A <see cref="GridPoint"/> that lies on a vertex of a <see cref="PathfindingGrid"/>. Always stored internally as
    /// the Northwest vertex of a <see cref="GridTile"/>.
    /// </summary>
    public class GridVertex : ITraversable, IEquatable<GridVertex>, IPathfindingEdge
    {
        public float Weight { get; }
        public GridPoint Location { get; }
        
        /// <summary>
        /// The <see cref="Motility"/> of an object positioned over this <see cref="GridVertex"/>.
        /// </summary>
        public Motility Motility { get; }

        #region Operators

        public static bool operator ==(GridVertex left, GridVertex right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GridVertex left, GridVertex right)
        {
            return !Equals(left, right);
        }

        #endregion

        public GridVertex(float weight, Motility motility, Vec2 position)
        {
            Weight = weight;
            Motility = motility;
            Location = new GridPoint(position, Direction.NW);
        }

        public GridVertex(Vec2 position) : this(1.41422f, Motility.Unconstrained, position) { }

        public ReadOnlyCollection<GridNode> Touches()
        {
            throw new System.NotImplementedException();
            
            // returns the list of nodes that this vertex is a vertex of
            
        }

        public ReadOnlyCollection<GridEdge> Protrudes()
        {
            throw new System.NotImplementedException();
            
            // returns the list of edges that protrude from this vertex
        }

        public ReadOnlyCollection<GridVertex> Adjacent()
        {
            throw new System.NotImplementedException();
            
            // returns the list of vertices that share an edge with this vertex
        }

        #region ITraversable

        /// <summary>
        /// Returns whether this <see cref="GridVertex"/> is traversable with the given motility.
        /// </summary>
        /// <remarks>
        /// A <see cref="GridVertex"/> also checks with its protruding edges and touching nodes, as any of them can also
        /// prohibit movement through a corner. If any of them return false, then this method returns false.
        /// </remarks>
        /// <param name="motility">The agent's motility</param>
        /// <returns><c>true</c> if the agent is capable of traversing this edge with one (or more) of its motilities.</returns>
        public bool IsTraversable(Motility motility)
        {
            throw new System.NotImplementedException();
            
            // checks internal motility and the motility of its protruding edges and touching nodes
            // if the vertex has 'Motility.Unconstrained' set then it does not explicitly block any movement
        }

        #endregion

        #region IEquatable<GridVertex> Members

        public bool Equals(GridVertex other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Motility.Equals(other.Motility) && Weight.Equals(other.Weight) && Location.Equals(other.Location);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GridVertex) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Motility.GetHashCode();
                hashCode = (hashCode * 397) ^ Weight.GetHashCode();
                hashCode = (hashCode * 397) ^ Location.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}