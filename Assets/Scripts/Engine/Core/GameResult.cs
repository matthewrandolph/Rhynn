using System.Collections.Generic;
using Util;

namespace Rhynn.Engine
{
    /// <summary>
    /// Each call to [Game.ProcessGameLogic()] will return a [GameResult] object that tells the UI what happened during that
    /// update and what it needs to do.
    /// </summary>
    public class GameResult
    {
        /// <summary>
        /// The "interesting" events that occurred in this update.
        /// </summary>
        public readonly List<Event> Events = new List<Event>();

        /// <summary>
        /// Whether or not any game state has changed. If this is 'false', then no game processing has occurred (i.e.
        /// the game is stuck waiting on user input).
        /// </summary>
        public readonly bool MadeProgress;

        public GameResult(bool madeProgress)
        {
            MadeProgress = madeProgress;
        }

        public override string ToString()
        {
            return $"MadeProgress: {MadeProgress}, Events: {Events}";
        }
    }

    public class Event
    {
        public readonly Actor Actor;
        public readonly Vec2 Position;
        public readonly Direction Direction;

        public Event(Actor actor, Vec2 position, Direction direction)
        {
            Actor = actor;
            Position = position;
            Direction = direction;
        }
    }
}