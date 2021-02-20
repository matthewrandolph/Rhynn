using Rhynn.Engine;
using UnityEngine;

namespace Rhynn.UI
{
    public class WelcomeScreen : Screen
    {
        [SerializeField] private GameScreen gameScreen;
        
        public void NewCharacter()
        {
            
        }

        /// <summary>
        /// Starts a tactical encounter.
        /// </summary>
        /// <remarks>This is only on this screen temporarily since there is no load character, character creation, or
        /// "non-tactical encounter gameplay" screens. Once those are implemented, this should be moved to one of those.
        /// There will probably be a different version of this on each script (i.e. one just passes in the character
        /// created to a fresh new game or prompts to load a previous save from disk).</remarks>
        public void StartGame()
        {
            // Save the character's information to a CharacterSave, then pass that in to Game.

            Game game = new Game();
            game.GenerateBattleMap();
            
            gameScreen.AddGame(game);

            UI.SetScreen(gameScreen);
        }
    }
}