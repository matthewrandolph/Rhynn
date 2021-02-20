using System;
using Rhynn.Engine;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Rhynn.UI
{
    /// <summary>
    /// The UI has been told that the player wishes to issue a move action to their character.
    /// </summary>
    public class MoveState : GameUIState
    {
        public override void OnEnter()
        {
            // Listen for relevant player inputs
            _onSelectAction = delegate { OnSelect(); };
            Screen.Controls.Gameplay.Select.performed += _onSelectAction;
            
            // Turn on MoveState-related menu buttons
            Screen.UI.PushScreen(Screen.MoveMenu);
            
            // TODO: Display the maximum movement the character can do in one action
            
            // TODO: Activate the selection indicator - it should only appear when hovering over a valid tile
        }

        public override void OnExit()
        {
            Screen.Controls.Gameplay.Select.performed -= _onSelectAction;
            
            // Turn off MoveState-related menu buttons
            Screen.UI.PopScreen();
        }
        
        public MoveState(GameScreen screen) : base(screen) { }

        private void OnSelect()
        {
            // Get the selected tile from the selection indicator/mouse
            Vector2 pointerPosition = Pointer.current.position.ReadValue();
            Debug.Log($"Clicked on point {pointerPosition}");
            Ray ray = Camera.main.ScreenPointToRay(pointerPosition);

            if (Screen.BattleMapGfx.GetComponent<Collider>().Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
            {
                Debug.Log($"RaycastHit hit the battlemap collider at world point {hitInfo.point}");
                Vector3 pointerCoordinates = Screen.BattleMapGfx.transform.InverseTransformPoint(hitInfo.point);
                Debug.Log($"RaycastHit hit the battlemap collider at local point {pointerCoordinates}");

                int x = Mathf.FloorToInt(pointerCoordinates.x / Screen.BattleMapGfx.TileSize);
                int z = Mathf.FloorToInt(pointerCoordinates.z / Screen.BattleMapGfx.TileSize);
                Vec2 tileSelected = new Vec2(x, z);

                Screen.Game.CurrentActor.SetNextAction(new StrideAction(Screen.Game.CurrentActor, tileSelected));
                
                // Send the StrideAction to the engine for processing
            }
        }
        
        private Action<InputAction.CallbackContext> _onSelectAction;
    }
}