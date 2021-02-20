using System;
using System.Collections.Generic;
using Rhynn.Engine.Generation;
using Util;
using Util.Pathfinding;

namespace Rhynn.Engine
{
    [Serializable]
    public class BattleMap
    {
        public Grid2D<GridTile> Tiles => _tiles;

        public List<Actor> Actors { get; }

        public Rect Bounds => new Rect(_tiles.Size);

        public Game Game { get; }

        public BattleMap(Game game, int width = 100, int height = 80)
        {
            Game = game;
            
            _tiles = new Grid2D<GridTile>(width, height);
            
            // TODO: Instantiate items list
            
            Actors = new List<Actor>();
        }

        public void Generate()
        {
            // Items.Clear();
            Actors.Clear();

            // generate the dungeon
            DelaunayGeneratorOptions options = new DelaunayGeneratorOptions();
            IBattleMapGenerator generator = new DelaunayGenerator(this, options);

            generator.Create();
            
            // Add the player
            Actors.Add(Game.PlayerCharacter);
            Game.PlayerCharacter.Position = generator.StartPosition;
        }

        private readonly Grid2D<GridTile> _tiles;
    }
}