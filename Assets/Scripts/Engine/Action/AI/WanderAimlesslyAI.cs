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
    public class WanderAimlesslyAI : DecisionMakingAlgorithmBase
    {
        #region IDecisionMakingAlgorithm

        /// <summary>
        /// Returns a <see cref="StrideAction"/> to a random tile accessible by the actor.
        /// </summary>
        public override List<Action> GetNextActivity()
        {
            if (Actor == null) throw new NullReferenceException("You must call SetActor() before you can call GetNextActivity() on WanderAimlesslyAI.");
            
            var tiles = Actor.Game.BattleMap.Tiles;
            var viableTiles = tiles.FloodFill<DijkstraFloodFill>(
                tiles[Actor.Position], Actor.Speed, Actor.Motility);
            Vec2 newPosition = viableTiles.ElementAt(Rng.Int(0, viableTiles.Count)).Value.Position;
            return new List<Action> { new StrideAction(Actor, newPosition) };
        }
        
        #endregion
    }
}