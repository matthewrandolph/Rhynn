using System;
using Rhynn.Engine;
using UnityEngine;
using BattleMap = Rhynn.App.BattleMap;

namespace Rhynn.UI
{
    public class PlayGameScreen : Screen
    {
        [SerializeField] private GameObject battleMapPrefab;
        
        public Game Game => _game;

        public void AddGame(Game game)
        {
            if (Game != null) throw new InvalidOperationException("Cannot add a second Game object to PlayGameScreen.");

            _game = game;
        }

        public override void Init()
        {
            base.Init();

            BattleMap battleMap = Instantiate(battleMapPrefab).GetComponent<BattleMap>();
            battleMap.Init(Game);

            ProcessGame();
        }

        public void ProcessGame()
        {
            // ... continue here
        }

        private Game _game;
    }
}