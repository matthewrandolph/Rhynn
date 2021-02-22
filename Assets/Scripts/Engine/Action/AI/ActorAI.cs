using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Rhynn.Engine.AI
{
    /// <summary>
    /// Handles receiving and issuing commands for an <see cref="Actor"/>.
    /// </summary>
    public class ActorAI
    {
        /// <summary>
        /// Returns whether the <see cref="Actor"/> is ready to take a turn or not.
        /// <remarks>
        /// First checks if an <see cref="Activities">activity</see> is still pending, and if not then asks its
        /// <see cref="IDecisionMakingAlgorithm">DecisionMakingAlgorithm</see> if it requires user input before it can
        /// issue its next activity.
        /// </remarks>
        /// </summary>
        public bool NeedsUserInput => _activity == null && _decisionMakingAlgorithm.NeedsUserInput;

        /// <summary>
        /// Returns the next <see cref="Action"/> that the <see cref="Actor"/> will perform.
        /// </summary>
        /// <remarks>If the <see cref="Actor"/> has an in-progress <see cref="Activities">Activity</see>, it will
        /// request the next <see cref="Action"/> of that activity. Otherwise, it will request another activity from
        /// its AI.</remarks>
        /// <returns></returns>
        public IEnumerable<Action> NextAction()
        {
            if (_activity == null)
            {
                _activity = _decisionMakingAlgorithm.GetNextActivity();
            }
            
            for (int i = 0; i < _activity.Count; i++)
            {
                Action action = _activity[i];
                if (i == _activity.Count - 1)
                {
                    // This is the last action of this behaviour
                    _activity = null;
                }

                yield return action;
            }
        }

        /// <summary>
        /// Sets the <see cref="Actor"/>'s <see cref="Activities">Activity</see>.
        /// </summary>
        /// <remarks>
        /// An <see cref="Action"/> can be implicitly converted to the parameter of <see cref="SetActivity"/>.
        /// </remarks>
        /// <param name="activity">The <see cref="Activities">Activity</see> or <see cref="Action"/> to perform.</param>
        public void SetActivity(NotNull<List<Action>> activity)
        {
            _activity = activity;
        }

        /// <summary>
        /// Sets the <see cref="IDecisionMakingAlgorithm">DecisionMakingAlgorithm</see> that the <see cref="Actor"/>
        /// uses to decide its <see cref="Activities"/> and <see cref="Action"/>s.
        /// </summary>
        /// <param name="algorithm">The <see cref="IDecisionMakingAlgorithm">DecisionMakingAlgorithm</see> to request
        /// <see cref="Activities"/> from.</param>
        public void SetAlgorithm(IDecisionMakingAlgorithm algorithm)
        {
            _decisionMakingAlgorithm = algorithm;
        }

        public ActorAI(Actor actor)
        {
            _actor = actor;
        }

        private IDecisionMakingAlgorithm _decisionMakingAlgorithm;
        private List<Action> _activity;
        private Action _nextAction;
        private Actor _actor;
    }
}