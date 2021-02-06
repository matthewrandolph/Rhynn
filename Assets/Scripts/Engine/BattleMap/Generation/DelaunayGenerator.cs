using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Engine
{
    public class DelaunayGenerator : IBattleMapGenerator, IFeatureWriter<DelaunayGeneratorOptions>
    {
        #region IBattleMapGenerator Members

        public void Create(BattleMap map, object optionsObj)
        {
            _options = (DelaunayGeneratorOptions) optionsObj;

            _map = map;

            do
            {
                _try++;

                //_map.Entities.Clear();
                //_map.Items.Clear();

                MakeBattleMap(map.Bounds.Size);
            } while (100 * _openCount / _map.Bounds.Area < _options.MinimumOpenPercent);
        }

        #endregion

        private void MakeBattleMap(Vec2 size)
        {
            // Clear the grid
            _openCount = 0;
            _map.Tiles.Fill(position => new GridTile(TileType.Stone));
            
            _factory = new DelaunayFeatureFactory(this);
            
            // Initialize the pathfinding edges to default value
            InitializePathfindingGraph();
            
            // Create a starting room
            _startPosition = _factory.MakeStartingRoom().Center;

            for (int i = 0; i < _options.MaxTries; i++)
            {
                // Make a bunch of rooms
                var feature = "room";
                _factory.CreateFeature(feature);
            }

            _factory.MakeHalls();
            
            foreach (Vec2 tileCoordinates in _map.Bounds)
            {
                IPathfindingNode tile = _map.Tiles.GetNodeAt(tileCoordinates);
                
                // Assign pathfinding graph traversabilities
                foreach (IPathfindingEdge edge in tile.NeighborMap.Values)
                {
                    GridTile start = (GridTile) edge.Start;
                    GridTile end = (GridTile) edge.End;
                    
                    // Sets edges based on end node, irrespective of starting node. So this is valid:
                    // TileType.Wall--Traversable.Land-->TileType.Floor
                    // TODO: Restrict traversability based on neighboring nodes for diagonals
                    // Ghosts can walk through all tiles (TODO: change this so that it must remain adjacent to any object’s exterior, and so cannot pass entirely through an object whose space is larger than its own)
                    edge.SetTraversableFlag(Traversable.Incorporeal);

                    if (start.Type == TileType.Floor && end.Type == TileType.Floor)
                    {
                        edge.SetTraversableFlag(Traversable.Land);
                    }
                }
                
                // Do a final pass to see how much battlemap we've carved
                if (tile.NeighborMap.Any(entry => entry.Key.IsTraversableTo(Traversable.Land, entry.Value.End)))
                {
                    _openCount++;
                }
            }
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
        /// <remarks>It tests this by simply seeing if the outer edge of the rect touches a non-wall square.</remarks>
        public bool IsOpen(Rect rect, Vec2? exception)
        {
            // must be totally in bounds
            if (!_map.Bounds.Contains(rect)) return false;
            
            // and not cover an existing feature
            foreach (Vec2 edge in rect.Trace())
            {
                // allow the exception
                if (exception != null && (exception == edge)) continue;

                if (_map.Tiles[edge].Type != TileType.Stone) return false;
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

        #endregion

        private DelaunayGeneratorOptions _options;
        private int _try = 0;
        private int _openCount;
        private BattleMap _map;
        private DelaunayFeatureFactory _factory;
        private Vec2 _startPosition;
    }
}