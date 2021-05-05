using System;
using System.Collections.Generic;
using Util;
using Util.Pathfinding;
using Util.Pathfinding.SearchAlgorithms;

namespace Rhynn.Engine.AI
{
    public class MeleeMookAI : DecisionMakingAlgorithmBase
    {
        public override List<Action> GetNextActivity()
        {
            if (Actor == null) throw new NullReferenceException("You must call SetActor() before you can call GetNextActivity() on WanderAimlesslyAI.");
            
            IList<GridNode> path = Actor.Game.BattleMap.Tiles.FindPath<AStarSearchAlgorithm>(
                Actor.Game.BattleMap.Tiles[Actor.Position],
                Actor.Game.BattleMap.Tiles[Actor.Game.PlayerCharacter.Position],
                Actor.Motility, (a, b) => 
                    Math.Abs(a.Position.x - b.Position.x) + Math.Abs(a.Position.y - b.Position.y));

            if (path.Count == 1)
            {
                var damageFormula = new DamageFormula(1, 3); // TODO: Read this from the Strike dictionary for the Actor
                var strike = new Strike(damageFormula);
                return new StrikeAction(Actor, strike, Actor.Game.PlayerCharacter);
            }

            GridNode targetTile = null;

            foreach (GridNode tile in path)
            {
                if (tile.PathCost > Actor.Speed)
                    break;
                
                if (Actor.Game.BattleMap.ActorAt(tile.Position) != null || !Actor.CanOccupy(tile.Position))
                    continue;

                targetTile = tile;
            }

            if (targetTile != null)
            {
                return new StrideAction(Actor, targetTile.Position);
            }

            throw new ArgumentNullException(nameof(targetTile), "MeleeMookAI \"targetTile\" is null.");
        }
    }
}