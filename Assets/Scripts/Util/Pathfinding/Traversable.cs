using System;

namespace Util.Pathfinding
{
    [Flags]
    public enum Traversable // Uses an Int32, so up to 32 values
    {
        None   = 0 << 0,
        Land   = 1 << 0,       //   1 in decimal
        Swim   = 1 << 1,       //   2 in decimal
        Climb  = 1 << 2,       //   4 in decimal
        Fly    = 1 << 3,       //   8 in decimal
        Burrow = 1 << 4,       //  16 in decimal
        Incorporeal  = 1 << 5, //  32 in decimal
        Unconstrained = 1 << 6,//  64 - special case where the agent can ignore movement constrains (used in map generation, etc)
        LineOfSight  = 1 << 7, // 128 - agent is constrained by edges that block Line of Sight (ranged attacks, etc)
        LineOfEffect = 1 << 8, // 256 - agent is constrained by edges that block Line of Effect (magic effects, mostly)
        AllNeighbors = 1 << 9, // 512 - agent can move along any connected edge. This is the default movement.
        FourWayNeighbors = 1 << 10,  // 1024 - agent only moves in the cardinal direction (useful for map generation)
        EightWayNeighbors = 1 << 11, // 2048 - agent moves in cardinal and intercardinal directions (but not specialty directions like portals)
        Everything   = ~0      //  -1 in decimal
    }
}