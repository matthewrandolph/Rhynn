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
    public class GridTile
    {
        public GridTile(TileType type, Vec2 position)
        {
            _type = type;
            _position = position;
        }

        public TileType Type
        {
            get => _type;
            set => _type = value;
        }

        public Vec2 Position => _position;

        /// <summary>
        /// Whether its possible for an <see cref="Actor"/> with the given motility to enter this tile.
        /// </summary>
        /// <param name="motility"></param>
        /// <returns></returns>
        public bool CanEnter(Motility motility)
        {
            return _pathfindingNode.CanEnter(motility);
        }

        private TileType _type;
        private Vec2 _position;
        private readonly IGridNode<GridTile> _pathfindingNode;
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