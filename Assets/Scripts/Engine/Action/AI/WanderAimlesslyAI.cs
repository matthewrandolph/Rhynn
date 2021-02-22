using System;
using System.Collections.Generic;
using System.Linq;
using Util;
using Util.Pathfinding.SearchAlgorithms;

namespace Rhynn.Engine.AI
{
    /// <summary>
    /// Causes the <see cref="Actor"/> to repeatedly take <see cref="StrideAction"/>s to a random tile accessible by
    /// the actor. The actor will appear to be wandering aimlessly around.
    /// </summary>
    public class WanderAimlesslyAI : IDecisionMakingAlgorithm
    {
        #region IDecisionMakingAlgorithm

        public bool NeedsUserInput => false;

        public void SetActor(NotNull<Actor> actor)
        {
            _actor = actor;
        }

        /// <summary>
        /// Returns a <see cref="StrideAction"/> to a random tile accessible by the actor.
        /// </summary>
        public List<Action> GetNextActivity()
        {
            if (_actor == null) throw new NullReferenceException("You must call SetActor() before you can call GetNextActivity() on WanderAimlesslyAI.");
            
            var tiles = _actor.Game.BattleMap.Tiles;
            var viableTiles = tiles.Pathfinder<DijkstraFloodFill>(
                tiles.GetNodeAt(_actor.Position), _actor.Speed, _actor.Motility);
            var newPosition = viableTiles.ElementAt(Rng.Int(0, viableTiles.Count)).Value.Position;
            return new List<Action> { new StrideAction(_actor, newPosition) };
        }
        
        #endregion

        private Actor _actor;
    }
}