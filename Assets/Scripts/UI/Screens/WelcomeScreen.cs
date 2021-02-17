using Rhynn.Engine;
using UnityEngine;

namespace Rhynn.UI
{
    public class WelcomeScreen : Screen
    {
        [SerializeField] private PlayGameScreen playGameScreen;
        
        public void NewHero()
        {
            
        }

        /// <summary>
        /// Quits out of the main menu and starts regular gameplay.
        /// </summary>
        /// <remarks>This is only on this screen temporarily since there is no load character or character creation
        /// screens. One those are in, this should be moved to one of those. There will probably be a different version
        /// of this on each script (i.e. one just passes in the character created to a fresh new game or prompts to
        /// load a previous save from disk).</remarks>
        public void StartGame()
        {
            Game game = new Game();
            game.GenerateBattleMap();
            
            playGameScreen.AddGame(game);

            UI.SetScreen(playGameScreen);
        }
    }
}