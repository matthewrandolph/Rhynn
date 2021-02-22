using System;
using System.Collections.Generic;
using Content;
using UnityEngine;
using Util;
using Util.Pathfinding;
using Util.Pathfinding.SearchAlgorithms;
using Rect = Util.Rect;

namespace Rhynn.Engine.Generation
{
    public class DelaunayFeatureFactory
    {
        public DelaunayFeatureFactory(IFeatureWriter<DelaunayGeneratorOptions> writer)
        {
            _rooms = new List<Rect>();
            _writer = writer;
        }

        public Rect MakeStartingRoom()
        {
            return CreateRoom(true);
        }

        public bool CreateFeature(string name, out Rect bounds)
        {
            switch (name)
            {
                case "room":
                    return MakeRoom(out bounds);
                default: throw new ArgumentException($"Unknown feature \"{name}\"");
            }
        }

        private bool MakeRoom(out Rect bounds)
        {
            bounds = CreateRoom();
            return bounds != Rect.Empty;
        }

        private Rect CreateRoom(bool startingRoom = false)
        {
            int width = Rng.Int(_writer.Options.RoomSizeMin, _writer.Options.RoomSizeMax);
            int height = Rng.Int(_writer.Options.RoomSizeMin, _writer.Options.RoomSizeMax);

            Rect bounds = CreateRectRoom(width, height, startingRoom);
            
            // Bail if we failed
            if (bounds == Rect.Empty) return bounds;
            
            // Place the room
            foreach (Vec2 position in bounds)
            {
                _writer.SetTile(position, Tiles.Floor);
            }
            
            // And the walls surrounding the room
            foreach (Vec2 position in bounds.Trace())
            {
                _writer.SetTile(position, Tiles.Wall);
            }
            
            // === Room decoration code here ===
            //TileType decoration = ChooseInnerWall();

            //RoomDecoration.Decorate(bounds, new RoomDecorator(this, position => _writer.Populate(position, 60, 200)));

            //_writer.LightRect(bounds);
            
            // place the connectors
            //AddRoomConnectors(bounds);

            Populate(bounds, 20, 20);
            // === End room decoration code === 
            
            _rooms.Add(bounds);
            return bounds;
        }

        private Rect CreateRectRoom(int width, int height, bool firstRoom = false)
        {
            int x, y;
            
            // Position the room
            if (firstRoom)
            {
                // Initial room, so start near center
                x = Rng.TriangleInt((_writer.Bounds.Width - width) / 2, (_writer.Bounds.Width - width) / 2 - 4);
                y = Rng.TriangleInt((_writer.Bounds.Height - height) / 2, (_writer.Bounds.Height - height) / 2 - 4);
            }
            else
            {
                // Place it wherever
                x = Rng.Int(_writer.Bounds.Width);
                y = Rng.Int(_writer.Bounds.Height);
            }
            
            Rect bounds = new Rect(x, y, width, height);
            
            // Check to see if the room can be positioned
            if (!_writer.IsOpen(bounds.Inflate(1), null)) return Rect.Empty;

            return bounds;
        }

        public bool MakeHalls()
        {
            List<Vertex> vertices = new List<Vertex>();
            
            // add a vertex at the center of each room
            foreach (Rect room in _rooms)
            {
                vertices.Add(new Vertex<Rect>(new Vector2(room.Center.x, room.Center.y), room));
            }
            
            // Create a Delaunay triangulation of the rooms
            _delaunay = Delaunay2D.Triangulate(vertices);
            
            // Convert the Delaunay edges to Prim edges
            List<Prim.Edge> edges = new List<Prim.Edge>();

            foreach (Delaunay2D.Edge edge in _delaunay.Edges)
            {
                edges.Add(new Prim.Edge(edge.U, edge.V));
            }
            
            // Create a minimum spanning tree of the triangulation
            List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);
            
            // Get the edges not part of the MST
            HashSet<Prim.Edge> selectedEdges = new HashSet<Prim.Edge>(mst);
            HashSet<Prim.Edge> remainingEdges = new HashSet<Prim.Edge>(edges);
            remainingEdges.ExceptWith(selectedEdges);

            // Create some loops, since MSTs make for boring maps
            foreach (Prim.Edge edge in remainingEdges)
            {
                if (Rng.Int(100) < _writer.Options.ChanceOfExtraHallway)
                {
                    selectedEdges.Add(edge);
                }
            }
            
            // Drive a pathfinder to carve out the halls
            Motility motility = Motility.FourWayNeighbors | Motility.Unconstrained;
            foreach (Prim.Edge edge in selectedEdges)
            {
                Rect startRoom = (edge.U as Vertex<Rect>).Item;
                Rect endRoom = (edge.V as Vertex<Rect>).Item;

                GridTile start = (GridTile) _writer.GetTile(new Vec2(startRoom.Center.x, startRoom.Center.y));
                GridTile goal = (GridTile) _writer.GetTile(new Vec2(endRoom.Center.x, endRoom.Center.y));

                // Heuristic is Manhattan distance on a square grid
                IList<IPathfindingNode> path = _writer.Graph.Pathfinder<AStarSearchAlgorithm>(start, goal,
                    motility, (a, b) => 
                        Math.Abs(a.Position.x - b.Position.x) + Math.Abs(a.Position.y - b.Position.y));

                // Bail if the pathfinder failed to find a path (NOTE: if it does, it seems like it should be a problem)
                if (path == null) continue;
                foreach (IPathfindingNode step in path)
                {
                    _writer.SetTile(step.Position, Tiles.Floor);
                    foreach (IPathfindingNode neighbor in step.NeighborMap.Keys)
                    {
                        if (((GridTile) _writer.GetTile(neighbor.Position)).Type == Tiles.Stone)
                        {
                            _writer.SetTile(neighbor.Position, Tiles.Wall);
                        }
                    }
                }
            }

            return true;
        }

        private void Populate(Rect bounds, int monsterDensity, int itemDensity)
        {
            // TODO: For testing, place one NPC in each room. For real this should probably be more random!
            //foreach (Rect room in _rooms)
            {
                int xOffset = Rng.Int(bounds.Width);
                int yOffset = Rng.Int(bounds.Height);

                Vec2 location = bounds.Position + new Vec2(xOffset, yOffset);
                
                _writer.Populate(location);
            }
            
            
        }

        private IList<Rect> _rooms;
        private Delaunay2D _delaunay;
        private IFeatureWriter<DelaunayGeneratorOptions> _writer;
    }
}