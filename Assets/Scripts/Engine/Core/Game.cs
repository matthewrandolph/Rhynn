﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

namespace Rhynn.Engine
{
    public class Game
    {
        public BattleMap BattleMap { get; }
        
        public Actor PlayerCharacter { get; private set; }
        
        /// <summary>
        /// Gets the <see cref="Actor"/> who created the root <see cref="Action"/> that is currently being processed.
        /// Will be <c>null</c> outside of the main Action processing loop.
        /// </summary>
        public Actor ActingActor { get; private set; }
        
        /// <summary>
        /// Gets the <see cref="Actor"/> who's turn it is, regardless of whether they are currently taking an action or
        /// if they are waiting for input.
        /// </summary>
        public Actor CurrentActor { get; private set; }

        public Game()
        {
            BattleMap = new BattleMap(this);
        }

        public void GenerateBattleMap()
        {
            PlayerCharacter = new Actor(this, Vec2.Zero, 1);

            BattleMap.Generate();
        }

        public GameResult ProcessGameLogic()
        {
            if (_iterator == null)
            {
                _iterator = CreateProcessEnumerable().GetEnumerator();
            }

            _iterator.MoveNext();
            return _iterator.Current;
        }

        /// <summary>
        /// Creates an enumerable collection of the entire series of GameResults for the game.
        /// </summary>
        private IEnumerable<GameResult> CreateProcessEnumerable()
        {
            while (true)
            {
                // Not using a foreach loop here so that we don't get a concurrent modification exception if an actor
                // is added or removed during the action.
                for (int actorIndex = 0; actorIndex < BattleMap.Actors.Count; actorIndex++)
                {
                    CurrentActor = BattleMap.Actors[actorIndex];

                    // actor.StartTurn(); - perhaps make this a "foreach GameResult in" loop as well. This is used for any "start of turn" effects, like buffs running out, etc.
                    // while (actor.RemainingActions > 0)
                    
                    // bail if we need to wait for the UI or AI to provide an action
                    while (CurrentActor.NeedsInput)
                        yield return new GameResult(false);

                    // get the actor's actions
                    foreach (Action action in CurrentActor.TakeTurn())
                    {
                        // process it and everything it leads to
                        foreach (GameResult result in ProcessAction(action))
                        {
                            yield return result;
                        }
                    }
                    
                    // end while
                    // actor.EndTurn(); - perhaps make this a "foreach GameResult in" loop as well. This is used for any "end of turn" effects, such as enemy debuffs running out, etc.
                    
                    // actor was killed, so it will be removed from the collection and the next one shifted up.
                    if (!CurrentActor.IsAlive)
                    {
                        actorIndex--;
                    }
                }
                
                // Process all of the items and other things that need to have something happen each turn
                
                // Turn/time increment (advance 1 turn/6 seconds, etc).
            }
        }

        private IEnumerable<GameResult> ProcessAction(Action theAction)
        {
            Debug.Log($"ProcessAction() invoked with action {theAction}");
            Queue<Action> actions = new Queue<Action>();
            actions.Enqueue(theAction);

            while (actions.Count > 0)
            {
                Action action = actions.Peek();
                
                // track who owns this sequence of actions
                ActingActor = action.Actor;

                ActionResult result = action.Perform(actions);
                
                // cascade through all the alternates until we hit the "real" action to process
                while (result.Alternative != null)
                {
                    result = result.Alternative.Perform(actions);
                }

                // remove it if complete
                if (result.Done)
                {
                    actions.Dequeue();
                }
                
                // Run the post step
                if (result.Succeeded)
                {
                    action.AfterSuccess();
                }

                ActingActor = null;

                yield return MakeResult(true);
            }
        }

        public void AddEvent(Actor actor, Vec2 position, Direction direction)
        {
            _events.Add(new Event(actor, position, direction));
        }

        private GameResult MakeResult(bool madeProgress)
        {
            GameResult result = new GameResult(madeProgress);
            result.Events.AddRange(_events);

            return result;
        }

        private IEnumerator<GameResult> _iterator;
        
        /// <summary>
        /// The events that have occurred since the last call to Game.ProcessGameLogic().
        /// </summary>
        private List<Event> _events = new List<Event>();
    }
}
