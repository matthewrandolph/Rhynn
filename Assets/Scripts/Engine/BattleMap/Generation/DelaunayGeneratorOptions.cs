namespace Engine
{
    /// <summary>
    /// Parameters that tune how the <see cref="DelaunayGenerator"/> will generate dungeons.
    /// Changing these values will affect the overall look of the dungeon, sometimes drastically.
    /// </summary>
    public class DelaunayGeneratorOptions : IGeneratorOptions
    {
        public int MaxTries { get; set; }
        public int MinimumOpenPercent { get; set; }
        
        // room
        public int RoomSizeMin { get; set; }
        public int RoomSizeMax { get; set; }
        
        // hall
        public int ChanceOfExtraHallway { get; set; }
        
        // door
        public int ChanceOfOpenDoor { get; set; }
        public int ChanceOfClosedDoor { get; set; }
        
        public DelaunayGeneratorOptions()
        {
            MaxTries = 100;
            MinimumOpenPercent = 20;

            RoomSizeMin = 5;
            RoomSizeMax = 20;

            ChanceOfExtraHallway = 13;

            ChanceOfOpenDoor = 20;
            ChanceOfClosedDoor = 10;
        }
    }
}