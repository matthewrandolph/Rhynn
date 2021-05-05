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
                if (_position != value) OnSetPosition(value);
            }
        }
        
        public int PlayerId { get; } // An id is generated for each human player. AI-controlled Actors are 0.

        public float Speed => 6; // TODO: base land speed for now, will probably end up an array of movement speeds or a dictionary or something to include other movement type speeds. Base speeds are calculated from racial base speeds as well as magical effects, equipment, etc.

        public int Health // TODO: Move to own class so that it can be expanded to "overall vitality" i.e. how many hits before unconsciousness and "limb health" i.e. how much damage individual body parts have suffered.
        {
            get => _health;
            set => Mathf.Clamp(value, 0, MaxHealth);
        }
        
        // TODO: below negative their Constitution score. NOTE: what to do about undead/creatures
        // that are destroyed at 0? Perhaps compare it to a deathHealthThreshold defined in race?
        // PF2e has a neat dying mechanic that prevents auto-death, health stays at 0 but gain a Dying status effect and if it gets too high, the Actor dies.
        public bool IsAlive { get; private set; }
        
                                     
        public bool NeedsInput => _actorAI.NeedsUserInput;

        public int ActionsPerTurn => IsAlive ? _actionsPerTurn : 0;


        public int RemainingActions
        {
            get => IsAlive ? _remainingActions : 0;
            set => _remainingActions = value;
        }

        public Motility Motility => Motility.Land; // TODO: Motility is calculated from racial base motility as well as magical effects, equipment, etc.

        public int MaxHealth => 10; // TODO: Calculate this number from class levels, Constitution score, equipment, and magical effects.
        
        public bool CanOccupy(Vec2 position) // TODO: This method seems like it needs to change with the way the new pathfinding system works
        {
            GridNode tile = Game.BattleMap.Tiles[position];
            return tile.AllowsMotility(Motility);
        }

        public Actor(Game game, Vec2 position, int playerId) 
        {
            Game = game;
            Position = position;
            PlayerId = playerId;
            
            _actorAI = new ActorAI(this);
            _actionsPerTurn = 1;
            IsAlive = true;
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

        /// <summary>
        /// Reduces the actor's health by <see cref="damage"/>, and handles its death.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="damage"></param>
        /// <param name="attacker"></param>
        /// <returns><c>true</c> if the actor died.</returns>
        public bool TakeDamage(Action action, int damage, Actor attacker)
        {
            Health -= damage;
            OnTakeDamage(action, attacker, damage);

            if (IsAlive) return false;
            
            // Add an event to the action that the Actor has died.

            OnDied();

            return true;
        }

        /// <summary>
        /// Called when this actor has successfully hit this <see cref="defender"/>
        /// </summary>
        public void OnGiveDamage(Action action, Actor defender, int damage)
        {
            // Do nothing
        }

        /// <summary>
        /// Called when <see cref="attacker"/> has successfully hit this actor.
        /// </summary>
        /// <remarks>The <see cref="attacker"/> may be <c>null</c> if the damage done is not the direct result of an
        /// attack (for instance, poison).</remarks>
        private void OnTakeDamage(Action action, Actor attacker, int damage)
        {
            // Do nothing
        }

        /// <summary>
        /// Called when this Actor has been killed.
        /// </summary>
        private void OnDied()
        {
            Despawn();

            // TODO: Actors turn to corpses when they die

            // TODO: If the Actor was the party's final player character, then they lose the game
        }

        /// <summary>
        /// Call this to remove this Actor from the current encounter, without explicitly killing it. Useful for
        /// dismissing summons, fleeing, etc.
        /// </summary>
        public void Despawn()
        {
            // Game will automatically handle adjusting its own actor indexing as long as the actor is marked !IsAlive.
            Game.BattleMap.RemoveActor(this);
            IsAlive = false;
        }

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

        public override string ToString()
        {
            return $"PlayerId: {PlayerId}, Position: {Position}";
        }

        private ActorAI _actorAI;
        private Vec2 _position;
        private int _health;
        private readonly int _actionsPerTurn;
        private int _remainingActions;
    }
}