using System;
using System.Collections.ObjectModel;
using Rhynn.Engine;

namespace Util.Pathfinding
{
    public class GridEdge : ITraversable, IEquatable<GridEdge>, IPathfindingEdge
    {
        public float Weight { get; }
        
        public GridPoint Location { get; }
        //public Vec2 Position { get; }
        //public Direction Annotation { get; }
        
        /// <summary>
        /// The <see cref="Motility"/> of an object positioned over this <see cref="GridEdge"/>.
        /// </summary>
        public Motility Motility { get; }

        public GridEdge(float weight, Motility motility, Vec2 position, Direction annotation)
        {
            Weight = weight;
            Motility = motility;
            //Position = position;
            //Annotation = annotation;
            Location = new GridPoint(position, annotation);
        }

        public GridEdge(Vec2 position, Direction annotation) : 
            this(1f, Motility.Unconstrained, position, annotation) { }

        #region Operators

        public static bool operator ==(GridEdge left, GridEdge right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GridEdge left, GridEdge right)
        {
            return !Equals(left, right);
        }

        #endregion

        public ReadOnlyCollection<GridEdge> Connects()
        {
            throw new System.NotImplementedException();
            
            // returns the edges that this edge connects
        }

        public ReadOnlyCollection<GridVertex> Endpoints()
        {
            throw new System.NotImplementedException();
            
            // returns the vertices that are endpoints to this line segment
        }

        public ReadOnlyCollection<GridNode> Joins()
        {
            throw new System.NotImplementedException();
            
            // returns the faces that this edge connects
        }

        #region ITraversable Members

        /// <summary>
        /// Returns whether this <see cref="GridEdge"/> is traversable with the given motility.
        /// </summary>
        /// <param name="motility">The agent's motility</param>
        /// <returns><c>true</c> if the agent is capable of traversing this edge with one (or more) of its motilities.</returns>
        public bool IsTraversable(Motility motility)
        {
            // NOTE: Checks internal motility only
            
            // if the edge has 'Motility.Unconstrained' set then it does not explicitly block any movement
            if (Motility == Motility.Unconstrained) { return true; }
            
            return motility.Contains(Motility);
        }

        #endregion

        /*#region IPathfindingNode Members

        public IPathfindingEdge JoiningEdge(GridTile node)
        {
            throw new NotImplementedException();
        }

        #endregion*/

        #region IEquatable<GridEdge> Members

        public bool Equals(GridEdge other)
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
            return Equals((GridEdge) obj);
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