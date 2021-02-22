using System.Collections.Generic;
using Util;

namespace Rhynn.Engine
{
    /// <summary>
    /// An atomic action an <see cref="Actor"/> can perform. All Actions use either 0 or 1 of the Actor's per-turn
    /// allotment of actions. If something requires more than 1 action, then it is instead an
    /// <see cref="Activities">Activity</see>.
    /// </summary>
    /// <remarks>An Action can be implicitly converted to an <see cref="ActionResult"/> or as an
    /// <see cref="Activities">Activity</see> (which is implemented as a List{<see cref="Action"/>}.</remarks>
    public abstract class Action
    {
        public static implicit operator ActionResult(Action action)
        {
            return new ActionResult(action);
        }

        public static implicit operator List<Action>(Action action)
        {
            return new List<Action> { action };
        }

        public static implicit operator NotNull<List<Action>>(Action action)
        {
            var actions = new List<Action> { action };
            return new NotNull<List<Action>>(actions);
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
            if (RequiresAction) { Actor.RemainingActions--; }
        }
        
        #region Game accessors

        public Game Game => _game ?? Actor.Game;
        public BattleMap BattleMap => Game.BattleMap;
        
        #endregion

        protected abstract ActionResult OnPerform();
        
        /// <summary>
        /// Whether or not this action counts against the <see cref="Actor"/>'s per-turn allotment of actions. If
        /// <c>false</c>, then this is considered a "free action."
        /// </summary>
        protected bool RequiresAction = true;

        private Queue<Action> _actions;
        private Game _game;
    }
}