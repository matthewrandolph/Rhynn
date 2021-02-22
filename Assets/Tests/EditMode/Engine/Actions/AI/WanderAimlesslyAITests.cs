using NUnit.Framework;
using Rhynn.Engine;
using Rhynn.Engine.AI;

namespace Tests
{
    public class WanderAimlesslyAITests
    {
        [Test]
        public void NeedsUserInput_False()
        {
            var ai = new WanderAimlesslyAI();
            
            Assert.IsFalse(ai.NeedsUserInput);
        }

        [Test]
        public void GetBehaviour_IsNotNull()
        {
            var game = new Game();
            game.GenerateBattleMap();
            game.PlayerCharacter.SetAI<WanderAimlesslyAI>();
            
            Assert.IsNotNull(game.PlayerCharacter.TakeTurn());
        }
    }
}
