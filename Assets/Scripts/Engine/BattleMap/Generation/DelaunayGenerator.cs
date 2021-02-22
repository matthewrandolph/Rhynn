using System;
using System.Linq;
using Content;
using Rhynn.Engine.AI;
using UnityEngine;
using Util;
using Util.Pathfinding;
using Rect = Util.Rect;

namespace Rhynn.Engine.Generation
{
    public class DelaunayGenerator : IBattleMapGenerator, IFeatureWriter<DelaunayGeneratorOptions>
    {
        public DelaunayGenerator(BattleMap map, object optionsObj)
        {
            _options = (DelaunayGeneratorOptions) optionsObj;
            _map = map;

            Reset();
        }

        #region IBattleMapGenerator Members
        
        public Vec2 StartPosition { get; private set; }

        /// <summary>
        /// Generates a battle map using the settings in the DelaunayGeneratorOptions.
        /// </summary>
        public void Create()
        {
            // Avoids dud maps with only like one or two rooms. If that happens, throw it out and keep trying from
            // scratch until we get one with at least a certain amount of carved open area.
            do
            {
                _try++;
                Reset();
                MakeBattleMap();
                
            } while (100f * _openCount / _map.Bounds.Area < _options.MinimumOpenPercent);
        }

        #endregion
        
        /// <summary>
        /// Clear the battlemap and start with a fresh, empty map.
        /// </summary>
        private void Reset()
        {
            _map.Actors.Clear();
            //_map.Items.Clear();
            
            _openCount = 0;
            _map.Tiles.Fill(position => new GridTile(Tiles.Stone, position));
            
            _factory = new DelaunayFeatureFactory(this);
        }

        private void MakeBattleMap()
        {
            // Initialize the pathfinding edges to default values
            InitializePathfindingGraph();
            
            // Create a starting room
            StartPosition = _factory.MakeStartingRoom().Center;

            // Make a bunch of rooms
            for (int i = 0; i < _options.MaxTries; i++)
            {
                CreateRoom();
            }

            _factory.MakeHalls();

            CalculateTraversability();
            _openCount = CalculateOpenCount();
        }

        /// <summary>
        /// Generates a single valid room.
        /// </summary>
        private void CreateRoom()
        {
            var feature = "room";
            _factory.CreateFeature(feature, out Rect bounds);
        }


        /// <summary>
        /// Creates a fresh pathfinding graph from the map bounds by generating empty edges connecting each tile.
        /// </summary>
        private void InitializePathfindingGraph()
        {
            // Iterate over each of the tiles within the battlemap boundaries
            foreach (Vec2 tileCoordinates in _map.Bounds)
            {
                IPathfindingNode tile = _map.Tiles.GetNodeAt(tileCoordinates);

                // Look at each of the eight compass directions
                foreach (Direction direction in Direction.Clockwise)
                {
                    // Check that the neighbor is in bounds
                    Vec2 neighborCoordinates = tileCoordinates + direction.Offset;
                    if (!_map.Bounds.Contains(neighborCoordinates)) continue;

                    // Calculate the weight. Diagonals are more expensive than the cardinal directions.
                    float weight;
                    if (Math.Abs(direction.Offset.x) > 0 && Math.Abs(direction.Offset.y) > 0)
                        weight = 1.41422f;
                    else
                        weight = 1f;
                    
                    // Add the neighbor
                    IPathfindingNode neighbor = _map.Tiles.GetNodeAt(neighborCoordinates);
                    tile.AddNeighbor(neighbor, weight, direction);
                }
            }
        }

        /// <summary>
        /// Finalizes the traversability of the nodes within the battlemap.
        /// </summary>
        private void CalculateTraversability()
        {
            foreach (Vec2 tileCoordinates in _map.Bounds)
            {
                IPathfindingNode tile = _map.Tiles.GetNodeAt(tileCoordinates);
                
                // Ghosts can walk through all tiles (TODO: change this so that it must remain adjacent to any object’s exterior, and so cannot pass entirely through an object whose space is larger than its own)
                tile.SetIncomingTraversableFlag(Motility.Incorporeal);

                // Sets edges based on end node, irrespective of starting node. So this is valid:
                // TileType.Wall--Traversable.Land-->TileType.Floor
                if (tile.Type == Tiles.Floor)
                {
                    tile.SetIncomingTraversableFlag(Motility.Land);
                }
                
                // TODO: Restrict traversability based on neighboring nodes for diagonals
            }
        }

        /// <summary>
        /// A final pass to see how much battlemap has been carved.
        /// <returns>The number of tiles that is traversable to by land movement.</returns>
        /// </summary>
        private int CalculateOpenCount()
        {
            int openCount = 0;
            foreach (Vec2 tileCoordinates in _map.Bounds)
            {
                IPathfindingNode tile = _map.Tiles.GetNodeAt(tileCoordinates);

                // Do a final pass to see how much battlemap we've carved
                if (tile.NeighborMap.Keys.Any(neighbor => neighbor.IsTraversableTo(tile, Motility.Land)))
                {
                    openCount++;
                }
            }

            return openCount;
        }

        #region IFeatureWriter Members

        public Rect Bounds => _map.Bounds;

        public DelaunayGeneratorOptions Options => _options;

        public IPathfindingGraph Graph => _map.Tiles;

        /// <summary>
        /// Gets whether the given rectangle is empty (i.e. solid stone) and can have a feature placed in it.
        /// </summary>
        /// <param name="rect">The rectangle to test.</param>
        /// <param name="exception">An optional exception position. If this position is not stone, it is still possible
        /// to use the rect. Used for the connector to a new feature.</param>
        /// <remarks>It tests this by verifying each tile within the rect is a stone square. Originally only traced
        /// the perimeter, but that was allowing rooms to fully enclose other rooms.</remarks>
        public bool IsOpen(Rect rect, Vec2? exception)
        {
            // must be totally in bounds
            if (!_map.Bounds.Contains(rect)) return false;
            
            // and not cover an existing feature
            foreach (Vec2 tile in rect)
            {
                // allow the exception
                if (exception != null && (exception == tile)) continue;

                if (_map.Tiles[tile].Type != Tiles.Stone) return false;
            }

            return true;
        }

        public IPathfindingNode GetTile(Vec2 position)
        {
            return _map.Tiles[position];
        }

        public void SetTile(Vec2 position, TileType type)
        {
            _map.Tiles[position].Type = type;
        }

        public void Populate(Vec2 position)
        {
            // TODO: Expand this to populate for NPCs with non-typical motilities (flying creatures, sea monsters, etc).
            if (_map.Tiles[position].CanEnter(Motility.Land))
            {
                // Place an NPC TODO: Check that tile is not occupied
                Actor actor = new Actor(_map.Game, position, 0);
                actor.SetAI<WanderAimlesslyAI>();
                AddActor(actor);
            }
        }

        public void AddActor(Actor actor)
        {
            _map.Actors.Add(actor);
        }

        #endregion

        private readonly DelaunayGeneratorOptions _options;
        private readonly BattleMap _map;

        private int _try = 0;
        private int _openCount;
        private DelaunayFeatureFactory _factory;
    }
}