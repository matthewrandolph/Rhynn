using UnityEngine;

namespace Rhynn.UI
{
    public class MoveMenu : Screen
    {
        [SerializeField] private GameScreen gameScreen;
        
        /// <summary>
        /// Quit from the Move menu and return to the AwaitingInput menu.
        /// </summary>
        public void Cancel()
        {
            Debug.Log("Cancel() invoked.");
            
            gameScreen.TransitionState(new AwaitingInputState(gameScreen));
        }
    }
}
