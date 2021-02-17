using JetBrains.Annotations;
using Util;
using Util.Pathfinding;

namespace Rhynn.Engine.Generation
{
    /// <summary>
    /// Interface used by a feature to actually apply the feature to a battlemap. The battlemap generator
    /// is expected to give an instance of this to a feature object so that it can place itself in the battlemap.
    /// </summary>
    public interface IFeatureWriter<TGeneratorOptions> where TGeneratorOptions : IGeneratorOptions
    {
        /// <summary>
        /// Gets the bounds of the battlemap. Features must stay within this.
        /// </summary>
        Rect Bounds { get; }

        IPathfindingGraph Graph { get; }

        bool IsOpen(Rect rect, [CanBeNull] Vec2? exception);
        
        void SetTile(Vec2 position, TileType type);

        IPathfindingNode GetTile(Vec2 position);
        
        TGeneratorOptions Options { get; }
    }
}