namespace Rhynn.Engine
{
    public class ActionResult
    {
        public static ActionResult Success => new ActionResult(true, true);
        public static ActionResult Failure => new ActionResult(false, true);
        public static ActionResult NotDone => new ActionResult(true, false);

        /// <summary>
        /// An alternate <see cref="Action"/> that should be performed instead of the one that failed to perform and
        /// returned this.
        /// </summary>
        public readonly Action Alternative;
        
        /// <summary>
        /// <c>True</c> if the <see cref="Action"/> was successful.
        /// </summary>
        public readonly bool Succeeded;
        
        /// <summary>
        /// <c>True</c> if the <see cref="Action"/> does not need any further processing.
        /// </summary>
        public readonly bool Done;

        public ActionResult(bool succeeded, bool done)
        {
            Succeeded = succeeded;
            Done = done;
        }

        public ActionResult(Action alternative)
        {
            Alternative = alternative;
        }
    }
}