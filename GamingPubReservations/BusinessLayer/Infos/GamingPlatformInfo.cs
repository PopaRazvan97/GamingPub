namespace BusinessLayer.Infos
{
    public class GamingPlatformInfo
    {
        public string Name { get; set; }

        public int Number { get; set; }

        public GamingPlatformInfo(string name, int number)
        {
            Name = name;
            Number = number;
        }

        public GamingPlatformInfo() { }
    }
}