using UnityEngine;

namespace Rhynn.UI
{
    public class AwaitingUserInputMenu : Screen
    {
        [SerializeField] private GameScreen gameScreen;
        
        /// <summary>
        /// The player indicates they wish to send a StrideAction to their character.
        /// </summary>
        public void Stride()
        {
            gameScreen.TransitionState(new MoveState(gameScreen));
        }
    }
}