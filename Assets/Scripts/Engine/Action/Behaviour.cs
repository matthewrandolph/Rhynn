namespace Rhynn.Engine
{
    /// <summary>
    /// What an <see cref="Actor"/> is doing. If the actor has no behaviour, it is because it is waiting on user input.
    /// Otherwise, the behaviour will determine which <see cref="Action"/>s the actor performs.
    ///
    /// Behaviour is coarser-grained than actions. A single behaviour may produce a series of actions. Primarily used
    /// to start multi-round actions as well as full-round actions that are started at the tail end of one round and
    /// finished on the next.
    /// </summary>
    public abstract class Behaviour
    {
        public abstract Action NextAction();
    }
}