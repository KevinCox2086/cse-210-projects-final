namespace AdventureGame.Core
{
    public class Exit
    {
        public Location Destination { get; private set; }
        public bool IsLocked { get; set; }
        public int RequiredKeyId { get; private set; }

        public Exit(Location destination)
        {
            Destination = destination;
            IsLocked = false;
            RequiredKeyId = 0;
        }

        public Exit(Location destination, bool isLocked, int requiredKeyId)
        {
            Destination = destination;
            IsLocked = isLocked;
            RequiredKeyId = requiredKeyId;
        }
    }
}