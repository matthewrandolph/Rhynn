using System.Collections.Generic;
using Util;

namespace Rhynn.Engine.AI
{
    /// <summary>
    /// This <see cref="IDecisionMakingAlgorithm">DecisionMakingAlgorithm</see> never returns any
    /// <see cref="Activities"/>, so the <see cref="Actor"/> is always waiting for an external source of
    /// <see cref="Activities"/> to issue <see cref="Action"/>s (so the player can make it dance like the puppet it is!)
    /// </summary>
    public class WaitForUserInputAI : IDecisionMakingAlgorithm
    {
        public bool NeedsUserInput => true;

        public void SetActor(NotNull<Actor> actor)
        {
            // Doesn't need a reference to the actor, as the ActorAI handles player input directly.
        }

        public List<Action> GetNextActivity()
        {
            return null;
        }
    }
}