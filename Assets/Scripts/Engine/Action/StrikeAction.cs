using UnityEngine;
using Util;

namespace Rhynn.Engine
{
    /// <summary>
    /// <see cref="Action"/> for when one <see cref="Actor"/> attacks another.
    /// </summary>
    public class StrikeAction : Action
    {
        public StrikeAction(NotNull<Actor> attacker, NotNull<Strike> strike, NotNull<Actor> defender) : base(attacker)
        {
            _defender = defender;
            _strike = strike;
        }

        protected override ActionResult OnPerform()
        {
            // send the hit to the defender
            Hit hit = new Hit(Actor, _strike);
            int damage = hit.Perform(this, Actor, _defender);
            Debug.Log($"{Actor} hit {_defender} for {damage} damage!");

            return ActionResult.Success;
        }

        private readonly Actor _defender;
        private readonly Strike _strike;
    }
}