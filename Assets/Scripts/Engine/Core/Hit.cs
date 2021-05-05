using System;
using UnityEngine;

namespace Rhynn.Engine
{
    /// <summary>
    /// Contains the data required for one <see cref="Actor"/> to hit another.
    /// </summary>
    public class Hit
    {
        public Actor Attacker { get; }
        public Strike Strike { get; }
        
        public int Damage
        {
            get
            {
                if (!_damage.HasValue) throw new InvalidOperationException("Cannot access the Hit's Damage before it has been set by the defender.");

                return _damage.Value;
            }
            set => _damage = value;
        }

        public int Perform(Action action, Actor attacker, Actor defender)
        {
            // TODO: Should a Hit also represent non-strike type attacks, such as spells that don't make an attack roll?
            
            // Handle attack roll - did the attack hit the target?
            int strikeRoll = Rng.RollCheck(0); // TODO: include range penalties, penalties from cover, and attack bonus (Str mod for melee, Dex for ranged by default)

            strikeRoll -= 11; // TODO: Set this to subtract evasion rate (which is tied to the Actor's Dexterity and dodge bonuses)
            
            // TODO: Consider if other defenses add to the defender's dodge-related AC, and enumerate through them.

            if (strikeRoll < 0)
                return 0;
                                                   
            // TODO: Roll against cover and concealment
            
            // Roll for damage
            int damage = 0;

            foreach (DamageFormula damageFormula in Strike.DamageFormulas)
            {
                int amount = damageFormula.RollDamage();
                
                // TODO: Handle defender resistances, weaknesses, and DR (based on damage type)
                // Shuffle them so that any message shown isn't biased by their order (just their relative amounts).

                // Round up so that percent reduction abilities don't always cancel out 1 damage
                damage += Mathf.CeilToInt(amount);
            }

            if (damage == 0) // Resistances and DR cancelled out all damage.
                return 0;

            attacker?.OnGiveDamage(action, defender, damage);

            if (defender.TakeDamage(action, damage, attacker))
                return damage;
            
            // TODO: Should resistances and DR cancel all side effects if there is any, or only if it blocks all damage?
            // TODO: Add an action to the current action with the side-effect and defender
            
            // TODO: Add an event to the action
            return damage;
        }

        /// <summary>
        /// Initializes a new Hit
        /// </summary>
        public Hit(Actor attacker, Strike strike)
        {
            Attacker = attacker;
            Strike = strike;
        }

        private int? _damage;
    }
}