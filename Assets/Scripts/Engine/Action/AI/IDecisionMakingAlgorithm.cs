using System.Collections.Generic;
using Util;

namespace Rhynn.Engine.AI
{
    public interface IDecisionMakingAlgorithm
    {
        /// <summary>
        /// Returns whether or not the algorithm requires input from the user in order to issue a command to the
        /// Actor's AI. This allows the AI to choose whether to take the player's input into account when it issues a
        /// command. For instance, it allows a unit to take commands but sometimes misbehave, such in the case
        /// of directing animals under your control (due to failed handle animal checks, etc).
        /// </summary>
        bool NeedsUserInput { get; }

        /// <summary>
        /// Sets the internal <see cref="Actor"/> variable that many algorithms use to access game state.
        /// </summary>
        /// <param name="actor">The <see cref="Actor"/> this is making decisions for.</param>
        void SetActor(NotNull<Actor> actor);
        
        List<Action> GetNextActivity();
    }
}