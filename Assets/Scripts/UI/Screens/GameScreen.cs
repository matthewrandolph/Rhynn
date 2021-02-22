using System;
using System.Collections;
using Rhynn.Engine;
using Rhynn.Input;
using UI.Graphics;
using UnityEngine;

namespace Rhynn.UI
{
    /// <summary>
    /// Class that handles the visual elements of a tactical encounter. Basically the "Game" of the graphical system.
    /// </summary>
    public class GameScreen : Screen
    {
        [SerializeField] private GameObject battleMapPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject enemyPrefab;
        
        [Header("Action Menus")]
        [SerializeField] private Screen awaitingUserInputMenu;
        [SerializeField] private Screen moveMenu;

        public Game Game { get; private set; }
        public Controls Controls { get; private set; }

        public Screen AwaitingUserInputMenu => awaitingUserInputMenu;
        public Screen MoveMenu => moveMenu;
        
        public BattleMapGfx BattleMapGfx { get; private set; }

        protected override void Awake()
        {
            Controls = new Controls();
            _state = new NullState(this);
            base.Awake();
        }

        protected void OnEnable() =>Controls.Gameplay.Enable();
        protected void OnDisable() => Controls.Gameplay.Disable();

        private void Update() => ProcessGame(); // TODO: Eventually needs to handle animations that take longer than 1 frame. Maybe use DOTween's OnComplete() method.

        public void AddGame(Game game)
        {
            if (Game != null) throw new InvalidOperationException("Cannot add a second Game object to GameScreen.");

            Game = game;
        }

        public override void Init()
        {
            base.Init();
            
            // Create BattleMap visuals
            BattleMapGfx = Instantiate(battleMapPrefab).GetComponent<BattleMapGfx>();
            BattleMapGfx.Init(Game);

            // Create Actor visuals TODO: Temp for testing
            // TODO: Foreach actor, create new GameObjects and AddComponent<> the required components
            // TODO: Load actor meshes/textures/sprites from Content instead of referenced in the UnityEditor
            foreach (Actor actor in Game.BattleMap.Actors)
            {
                ActorGfx actorGfx;
                if (actor.PlayerId == 0)
                {
                    GameObject actorGameObject = Instantiate(enemyPrefab,
                        new Vector3(actor.Position.x, 0, actor.Position.y), Quaternion.identity);
                    actorGfx = actorGameObject.GetComponent<ActorGfx>();
                }
                else
                {
                    GameObject actorGameObject = Instantiate(playerPrefab,
                        new Vector3(actor.Position.x, 0, actor.Position.y), Quaternion.identity);
                    actorGfx = actorGameObject.GetComponent<ActorGfx>();
                }
                
                actorGfx.Init(actor);
            }

            // Set the starting camera position
            Camera.main.transform.position = 
                new Vector3(Game.PlayerCharacter.Position.x, 10, Game.PlayerCharacter.Position.y);
            
            // TODO: Move this to when the UI determines that it is the player's turn.
            TransitionState(new AwaitingInputState(this));

            //StartCoroutine(ProcessGame());
        }

        public void TransitionState(GameUIState uiState)
        {
            if (_state == uiState) return;

            _state.OnExit();
            _state = uiState;
            _state.OnEnter();
        }


/*        private IEnumerator ProcessGame()
        {
            while (true)
            {
                yield return Game.ProcessGameLogic();
                yield return new WaitForSeconds(1f);
            }
            
            // ... continue here
        }*/
        
        public void ProcessGame()
        {
            Game.ProcessGameLogic();

            // ... continue here
        }
        
        private GameUIState _state;
    }
}