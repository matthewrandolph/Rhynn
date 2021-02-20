using System.Collections.Generic;
using Util;

namespace Rhynn.Engine
{
    public abstract class Action
    {
        public static implicit operator ActionResult(Action action)
        {
            return new ActionResult(action);
        }
        
        public Actor Actor { get; }

        /// <summary>
        /// Initializes a new instance of Action.
        /// </summary>
        /// <param name="actor">The <see cref="Actor"/> performing this Action.</param>
        public Action(NotNull<Actor> actor)
        {
            Actor = actor;
        }

        public Action(NotNull<Game> game)
        {
            _game = game;
        }

        public ActionResult Perform(Queue<Action> actions)
        {
            _actions = actions;
            
            ActionResult result = OnPerform();

            _actions = null;

            return result;
        }
        
        public void AddEvent(Event theEvent)
        {
            _game.AddEvent(theEvent.Actor, theEvent.Position, theEvent.Direction);
        }

        public void AfterSuccess()
        {
            // TODO: Reduce the number of 'acts' the actor has appropriately.
        }
        
        #region Game accessors

        public Game Game => _game ?? Actor.Game;
        public BattleMap BattleMap => Game.BattleMap;
        
        #endregion

        protected abstract ActionResult OnPerform();

        private Queue<Action> _actions;
        private Game _game;
    }
}