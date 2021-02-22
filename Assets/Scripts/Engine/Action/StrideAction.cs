using System.Collections.Generic;
using Util;
using Util.Pathfinding;
using Util.Pathfinding.SearchAlgorithms;

namespace Rhynn.Engine
{
    /// <summary>
    /// Basic action for walking (or flying or whatever) up to your movement speed.
    /// </summary>
    public class StrideAction : Action
    {
        public readonly Vec2 Destination;

        public StrideAction(Actor actor, Vec2 destination) : base(actor)
        {
            Destination = destination;
        }

        protected override ActionResult OnPerform()
        {
            // See if we can stand there
            if (!Actor.CanOccupy(Destination))
            {
                return ActionResult.Failure;
            }
            
            // TODO: See if the tile is occupied already
            
            // See if we can get there
            IPathfindingNode position = BattleMap.Tiles.GetNodeAt(Actor.Position);
            IDictionary<IPathfindingNode, IPathfindingNode> moveableTiles = 
                BattleMap.Tiles.Pathfinder<DijkstraFloodFill>(position, Actor.Speed, Actor.Motility);
            IPathfindingNode destination = BattleMap.Tiles.GetNodeAt(Destination);
            
            bool contains = moveableTiles.ContainsKey(destination);
            
            if (!contains)
            {
                return ActionResult.Failure;
            }

            Actor.Position = Destination;
            
            return ActionResult.Success;
        }
    }
}