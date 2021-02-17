using System;
using Rhynn.Engine.Generation;
using Util;
using Util.Pathfinding;

namespace Rhynn.Engine
{
    [Serializable]
    public class BattleMap
    {
        public Grid2D<GridTile> Tiles => _tiles;
        
        public Rect Bounds => new Rect(_tiles.Size);

        public Game Game => _game;
        
        public BattleMap(Game game, int width = 100, int height = 80)
        {
            _game = game;
            
            _tiles = new Grid2D<GridTile>(width, height);
            
            // Instantiate items
            // Instantiate actors
            
            // fill battlemap with default tiles
            _tiles.Fill(position => new GridTile(TileType.Stone, position));
        }

        public void Generate()
        {
            // Clear items
            // Clear actors

            // generate the dungeon
            DelaunayGeneratorOptions options = new DelaunayGeneratorOptions();
            IBattleMapGenerator generator = new DelaunayGenerator(this, options);

            generator.Create();
        }

        private readonly Grid2D<GridTile> _tiles;
        
        private Game _game;
        
    }
}