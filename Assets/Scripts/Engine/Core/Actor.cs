using System;
using System.Collections.Generic;
using System.Linq;
using Rhynn.Engine.AI;
using UnityEngine;
using Util;
using Util.Pathfinding;
using Util.PostOffice;

namespace Rhynn.Engine
{
    /// <summary>
    /// An active entity in the game. Includes both player characters as well as NPCs.
    /// </summary>
    public class Actor
    {
        public event EventHandler<ValueChangeEventArgs<Vec2>> Moved; 
        
        public readonly Game Game;

        public Vec2 Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    Debug.Log("Position set on Actor object.");
                    OnSetPosition(value);
                }
            }
        }
        
        public int PlayerId { get; } // An id is generated for each human player. AI-controlled Actors are 0.

        public float Speed => 6; // TODO: base land speed for now, will probably end up an array of movement speeds or a dictionary or something to include other movement type speeds. Base speeds are calculated from racial base speeds as well as magical effects, equipment, etc.
        
        public bool IsAlive => true; // TODO: When health and stats are added, return whether the Actor's health is
                                     // below negative their Constitution score. NOTE: what to do about undead/creatures
                                     // that are destroyed at 0? Perhaps compare it to a deathHealthThreshold defined in race?
                                     // PF2e has a neat dying mechanic that prevents auto-death.
                                     
        public bool NeedsInput => _actorAI.NeedsUserInput;

        public int ActionsPerTurn => 1;
        public int RemainingActions { get; set; }

        public Motility Motility => Motility.Land; // TODO: Motility is calculated from racial base motility as well as magical effects, equipment, etc.

        public bool CanOccupy(Vec2 position)
        {
            GridTile tile = Game.BattleMap.Tiles[position];
            return tile.CanEnter(Motility);
        }

        public Actor(Game game, Vec2 position, int playerId) 
        {
            Game = game;
            Position = position;
            PlayerId = playerId;
            
            _actorAI = new ActorAI(this);
        }

        /*public IEnumerable<Action> StartTurn()
        {
            yield return null;
        }*/

        public void StartTurn()
        {
            RemainingActions = ActionsPerTurn;
        }

        public Action TakeTurn()
        {
            return _actorAI.NextAction().First();
        }

        /*public IEnumerable<Action> EndTurn()
        {
            yield return null;
        }*/

        private void OnSetPosition(Vec2 position)
        {
            Vec2 oldPosition = Position;
            
            _position = position;

            OnMoved(new ValueChangeEventArgs<Vec2>(oldPosition, position));
        }

        protected virtual void OnMoved(ValueChangeEventArgs<Vec2> args)
        {
            EventHandler<ValueChangeEventArgs<Vec2>> handler = Moved;
            handler?.Invoke(this, args);
        }

        public void SetAI<TDecisionMakingAlgorithm>() where TDecisionMakingAlgorithm : IDecisionMakingAlgorithm, new()
        {
            var algorithm = new TDecisionMakingAlgorithm();
            algorithm.SetActor(this);
            _actorAI.SetAlgorithm(algorithm);
        }

        /// <summary>
        /// Sets the <see cref="Actor"/>'s <see cref="Activities">Activity</see>.
        /// </summary>
        /// <remarks>
        /// An <see cref="Action"/> can be implicitly converted to the parameter of <see cref="SetActivity"/>.
        /// </remarks>
        /// <param name="activity">The <see cref="Activities">Activity</see> or <see cref="Action"/> to perform.</param>
        public void SetActivity(NotNull<List<Action>> activity)
        {
            _actorAI.SetActivity(activity);
        }
        
        private ActorAI _actorAI;
        private Vec2 _position;
    }
}