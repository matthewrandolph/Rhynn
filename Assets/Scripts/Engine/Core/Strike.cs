using System;
using System.Collections;
using System.Collections.Generic;

namespace Rhynn.Engine
{
    /// <summary>
    /// Represents the power and damage types of a given Strike. Specific Strike objects belong in Content. For
    /// example: each Item will have a strike for each type of attack they can make, a zombie will have a Strike that
    /// represents their slam attack, everyone will have an Unarmed Strike, etc.
    /// </summary>
    public class Strike
    {
        public IReadOnlyCollection<DamageFormula> DamageFormulas => _damageFormulas.AsReadOnly();
        
        public Strike(List<DamageFormula> damageFormulas)
        {
            _damageFormulas = damageFormulas;
        }

        private List<DamageFormula> _damageFormulas;
    }

    public struct DamageFormula
    {
        public static implicit operator List<DamageFormula>(DamageFormula damageFormula)
        {
            return new List<DamageFormula> {damageFormula};
        }
        
        private int DiceSize { get; }
        private int NumDice { get; }
        // TODO: Add DamageTypes (i.e. piercing, bludgeoning, etc.)
        // TODO: Add crit-only bool

        public DamageFormula(int diceSize, int numDice)
        {
            DiceSize = diceSize;
            NumDice = numDice;
        }
        
        public int RollDamage()
        {
            return Rng.Roll(NumDice, DiceSize);
        }
    }
}