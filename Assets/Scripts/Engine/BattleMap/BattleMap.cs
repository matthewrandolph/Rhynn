using System;
using System.Collections.Generic;
using Util;

namespace Engine
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
        }

        public void Generate()
        {
            // Clear items
            // Clear actors
            
            // fill battlemap with default tiles
            _tiles.Fill((position) => new GridTile(TileType.Stone));
            
            // generate the dungeon
            IBattleMapGenerator generator;
            object options = null;

            generator = new DelaunayGenerator();
            options = new DelaunayGeneratorOptions();
        }

        private readonly Grid2D<GridTile> _tiles;
        
        
        private Game _game;
        
    }
}