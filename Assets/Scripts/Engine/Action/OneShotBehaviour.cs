namespace Rhynn.Engine
{
    /// <summary>
    /// Basic <see cref="Behaviour"/> that returns one selected <see cref="Action"/> once and then waits for user input.
    /// </summary>
    public class OneShotBehaviour : Behaviour
    {
        public OneShotBehaviour() { }

        public OneShotBehaviour(Action action)
        {
            _action = action;
        }

        public override Action NextAction()
        {
            // clear the action so it is only used once
            Action action = _action;
            _action = null;

            return action;
        }

        private Action _action;
    }
}