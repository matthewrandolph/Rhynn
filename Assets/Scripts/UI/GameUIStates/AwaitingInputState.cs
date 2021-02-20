using System;
using Rhynn.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rhynn.UI
{
    /// <summary>
    /// The UI hasn't been given any information related to what the player wants their character to do next. The
    /// "default" state when it is the player's turn.
    /// </summary>
    public class AwaitingInputState : GameUIState
    {
        public override void OnEnter()
        {
            // Listen for relevant player inputs
            _onMoveAction = delegate { OnMove(); };
            Screen.Controls.Gameplay.Move.performed += _onMoveAction;
            
            // Turn on the AwaitingUserInput menu buttons
            Screen.UI.PushScreen(Screen.AwaitingUserInputMenu);
        }

        public override void OnExit()
        {
            Screen.Controls.Gameplay.Move.performed -= _onMoveAction;
            
            // Turn off the AwaitingUserInput menu buttons
            Screen.UI.PopScreen();
        }
        
        public AwaitingInputState(GameScreen screen) : base(screen) { }

        public void OnMove()
        {
            Debug.Log("OnMove fired.");
        }
        
        private Action<InputAction.CallbackContext> _onMoveAction;
    }
}