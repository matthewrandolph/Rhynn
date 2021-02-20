using Util;

namespace Rhynn.Engine
{
    public interface IBattleMapGenerator
    {
        Vec2 StartPosition { get; }

        void Create();
    }
}