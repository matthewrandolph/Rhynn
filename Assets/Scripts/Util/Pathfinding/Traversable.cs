using System;

namespace Util
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
        LineOfSight  = 1 << 6, //  64
        LineOfEffect = 1 << 7, // 128
        AllNeighbors = 1 << 8, // 256
        FourWayNeighbors = 1 << 9,   // 512
        EightWayNeighbors = 1 << 10, // 1024
        Everything   = ~0      //  -1 in decimal
    }
}