using System;
using Rhynn.Input;

namespace Rhynn.UI
{
    public abstract class GameUIState : IEquatable<GameUIState>
    {
        public virtual void OnEnter() { }
        public virtual void OnExit() { }

        protected GameUIState(GameScreen screen)
        {
            Screen = screen;
        }
        
        #region Operators

        public static bool operator ==(GameUIState state1, GameUIState state2)
        {
            if (state1 is null) return state2 is null;
            return state1.Equals(state2);
        }
        
        public static bool operator !=(GameUIState state1, GameUIState state2)
        {
            if (state1 is null) return !(state2 is null);
            return !state1.Equals(state2);
        }
        
        #endregion
        
        #region IEquatable<PlayGameState> Members
        
        /// <summary>
        /// Returns whether the two states are the same type.
        /// </summary>
        /// <returns>Bool indicating whether the two states are the same type.</returns>
        public bool Equals(GameUIState other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ReferenceEquals(this.GetType(), other.GetType());
        }

        /// <summary>
        /// Returns whether an object is a PlayGameState of the same type.
        /// </summary>
        /// <returns>Bool indicating whether the two objects are the same PlayGameState type.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return ReferenceEquals(this.GetType(), obj.GetType());
        }

        #endregion

        protected GameScreen Screen;
    }
}