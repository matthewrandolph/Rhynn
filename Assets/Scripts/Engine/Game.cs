namespace Rhynn.Engine
{
    public class Game
    {
        public BattleMap BattleMap => _battleMap;
        
        public Game()
        {
            _battleMap = new BattleMap(this);
        }

        public void GenerateBattleMap()
        {
            _battleMap.Generate();
        }

        private BattleMap _battleMap;
    }
}
