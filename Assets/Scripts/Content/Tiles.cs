using Rhynn.Engine;
using Util.Pathfinding;

namespace Content
{
    /// <summary>
    /// Static class containing all of the <see cref="Rhynn.Engine.TileType"/>s.
    /// </summary>
    public static class Tiles
    {
        //public static readonly TileType Unknown = Tile("unknown");
        public static readonly TileType Floor = Tile("floor", 1).Open();
        public static readonly TileType Wall = Tile("wall", 2).Solid();
        public static readonly TileType Stone = Tile("stone", 3).Solid();

        private static TileBuilder Tile(string name, int spriteIndex) => new TileBuilder(name, spriteIndex);
        
        private class TileBuilder
        {
            public TileBuilder(string name, int spriteIndex)
            {
                _name = name;
                _spriteIndex = spriteIndex;
            }

            public TileType Open()
            {
                return _Motility(Motility.Land | Motility.Incorporeal | Motility.Fly);
            }
            
            public TileType Solid()
            {
                return _Motility(Motility.Incorporeal);
            }

            public TileType Water()
            {
                return _Motility(Motility.Incorporeal | Motility.Fly | Motility.Swim);
            }

            public TileType _Motility(Motility motility)
            {
                return new TileType(_name, motility, _spriteIndex);
            }

            private readonly string _name;
            private readonly int _spriteIndex; // TODO: Read the sprite image directly from file
        }
    }

    
}