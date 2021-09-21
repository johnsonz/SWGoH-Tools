namespace SWGOH.Models.Options
{
    public  class GameClientOptions
    {
        public const string Name = "GameClient";

        public string ExternalVersion { get; set; }
        public string InternalVersion { get; set; }
        public string UA { get; set; }
    }
}
