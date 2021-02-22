using Rhynn.Engine.AI;

namespace Rhynn.Engine
{
    /// <summary>
    /// A collection of what an <see cref="Actor"/> can do. If the actor has no activity, it is because it is waiting
    /// on user input. Otherwise, the activity will determine which <see cref="Action"/>s the actor performs.
    ///
    /// Activities are coarser-grained than actions. A single activity produces a series of actions. Primarily used
    /// to start multi-round actions as well as full-round actions that are started at the tail end of one round and
    /// finished on the next.
    /// <remarks>
    /// If a prospective activity cannot be explicitly defined as a list of actions, then it probably belongs as a
    /// <see cref="IDecisionMakingAlgorithm">DecisionMakingAlgorithm</see> instead.
    /// </remarks>
    /// </summary>
    public class Activities
    {
        
    }
}