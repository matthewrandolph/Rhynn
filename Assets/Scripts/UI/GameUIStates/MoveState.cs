using System;
using System.Collections.Generic;
using Rhynn.Engine;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;
using Util.Pathfinding;
using Util.Pathfinding.SearchAlgorithms;

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
            Ray ray = Camera.main.ScreenPointToRay(pointerPosition);

            if (!Screen.BattleMapGfx.GetComponent<Collider>()
                .Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity)) { return; }
            
            Vector3 pointerCoordinates = Screen.BattleMapGfx.transform.InverseTransformPoint(hitInfo.point);

            int x = Mathf.FloorToInt(pointerCoordinates.x / Screen.BattleMapGfx.TileSize);
            int z = Mathf.FloorToInt(pointerCoordinates.z / Screen.BattleMapGfx.TileSize);
            Vec2 tileSelected = new Vec2(x, z);
                
            // Do some basic verification that the tile wont be rejected by the engine. No point in telling the
            // engine if it is just going to throw the action away.
            Actor actor = Screen.Game.CurrentActor;
            PathfindingGrid tiles = Screen.Game.BattleMap.Tiles;
                
            GridNode position = tiles[actor.Position];
            IDictionary<GridNode, GridNode> moveableTiles = 
                tiles.FloodFill<DijkstraFloodFill>(position, actor.Speed, actor.Motility);
            GridNode destination = tiles[tileSelected];

            if (!moveableTiles.ContainsKey(destination)) return;
            
            // Send the StrideAction to the engine for processing
            Screen.Game.CurrentActor.SetActivity(new StrideAction(Screen.Game.CurrentActor, tileSelected));
            Screen.TransitionState(new AnimatingState(Screen));
        }
        
        private Action<InputAction.CallbackContext> _onSelectAction;
    }
}