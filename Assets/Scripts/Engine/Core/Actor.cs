using System;
using System.Collections.Generic;
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
        
        public int PlayerId { get; set; } // An id is generated for each human player. AI-controlled Actors are 0.

        public float Speed => 6; // TODO: base land speed for now, will probably end up an array of movement speeds or a dictionary or something to include other movement type speeds. Base speeds are calculated from racial base speeds as well as magical effects, equipment, etc.
        
        public bool IsAlive => true; // TODO: When health and stats are added, return whether the Actor's health is
                                     // below negative their Constitution score. NOTE: what to do about undead/creatures
                                     // that are destroyed at 0? Perhaps compare it to a deathHealthThreshold defined in race?

        public bool NeedsInput
        {
            get
            {
                if (PlayerId == 0) return false;
                
                // TODO: Include a check here to confirm the actor can perform the behaviour. if not, null out the behaviour.
                
                return _behaviour == null;
            }
        }

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
        }

        /*public IEnumerable<Action> StartTurn()
        {
            yield return null;
        }*/

        public IEnumerable<Action> TakeTurn()
        {
            Debug.Log("TakeTurn() invoked.");
            // get the next action
            Action turnAction = _behaviour.NextAction();

            if (turnAction != null) 
                yield return turnAction;
            
            WaitForInput();
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

        private void WaitForInput()
        {
            // TODO: Ask the AI assigned to control this actor for input here, rename to GetNextAction().
            // For example, ActorAI<WaitForUserInputAI>.GetNextBehaviour() would just set behaviour to null.
            // ActorAI<WanderAimlesslyAI> would set it to OneShotBehaviour<StrideAction> targeting a random location repeatedly.
            _behaviour = null;
        }

        public void SetNextAction(NotNull<Action> action)
        {
            SetBehaviour(new OneShotBehaviour(action));
        }

        public void SetBehaviour(NotNull<Behaviour> behaviour)
        {
            _behaviour = behaviour;
        }
        
        private Behaviour _behaviour;
        private Vec2 _position;
    }
}