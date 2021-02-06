namespace Engine
{
    public class Game
    {
        public BattleMap BattleMap => _battleMap;
        
        public Game()
        {
            _battleMap = new BattleMap(this);
        }

        private BattleMap _battleMap;
    }
}
