using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rhynn.Engine;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameTests
    {
        private Game game;

        [Test]
        public void PlayerCharacter_AfterBattleMapGenerated_IsNotNull()
        {
            game = new Game();
            game.GenerateBattleMap();
            Assert.IsNotNull(game.PlayerCharacter);
        }
    }
}
