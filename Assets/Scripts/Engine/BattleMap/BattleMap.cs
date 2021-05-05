using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rhynn.Engine.Generation;
using Util;
using Util.Pathfinding;
using Util.PostOffice;

namespace Rhynn.Engine
{
    [Serializable]
    public class BattleMap
    {
        public PathfindingGrid Tiles { get; }

        public ReadOnlyCollection<Actor> Actors => _actors.AsReadOnly();

        public Rect Bounds => new Rect(Tiles.Size);

        public Game Game { get; }

        /// <summary>
        /// A spacial partition to let us quickly locate an actor by tile. This is a performance bottleneck since
        /// pathfinding needs to ensure it doesn't set on other actors.
        /// </summary>
        public Grid2D<Actor> ActorsByTile { get; }

        public BattleMap(Game game, int width = 100, int height = 80)
        {
            Game = game;
            
            Tiles = new PathfindingGrid(width, height);
            ActorsByTile = new Grid2D<Actor>(width, height);
            
            // TODO: Instantiate items list
            
            _actors = new List<Actor>();
        }

        public void AddActor(Actor actor)
        {
            if (ActorsByTile[actor.Position] != null)
                throw new InvalidOperationException("Cannot add an Actor to a tile that already contains an Actor.");
            
            _actors.Add(actor);
            actor.Moved += MoveActor;
            ActorsByTile[actor.Position] = actor;
        }

        /// <summary>
        /// Called when an <see cref="Actor"/>'s position has changed so the stage can track it.
        /// </summary>
        private void MoveActor(object sender, ValueChangeEventArgs<Vec2> valueChangeEventArgs)
        {
            Actor actor = ActorsByTile[valueChangeEventArgs.Old];
            ActorsByTile[valueChangeEventArgs.Old] = null;
            ActorsByTile[valueChangeEventArgs.New] = actor;
        }

        public void RemoveActor(Actor actor)
        {
            if (ActorsByTile[actor.Position] == null)
                throw new InvalidOperationException("Cannot remove an Actor from a tile that does not contain an Actor.");
            
            // Game handles removing it from its internal list if we mark it as !IsAlive.
            ActorsByTile[actor.Position] = null;
        }

        public void ClearActors()
        {
            foreach (Actor actor in Actors)
            {
                RemoveActor(actor);
            }
            _actors.Clear();
        }

        public Actor ActorAt(Vec2 position) => ActorsByTile[position];

        public void Generate()
        {
            // Items.Clear();
            ClearActors();

            // generate the dungeon
            DelaunayGeneratorOptions options = new DelaunayGeneratorOptions();
            IBattleMapGenerator generator = new DelaunayGenerator(this, options);

            generator.Create();
            
            // Add the player
            AddActor(Game.PlayerCharacter);
            Game.PlayerCharacter.Position = generator.StartPosition;
        }
        
        private readonly List<Actor> _actors;
    }
}