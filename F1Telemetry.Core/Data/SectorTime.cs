namespace F1Telemetry.Core.Data
{
    public class SectorTime
    {
        public float Sector1 { get; }
        public float Sector2 { get; }
        public float Sector3 { get; }

        public SectorTime(float sector1, float sector2, float sector3)
        {
            Sector1 = sector1;
            Sector2 = sector2;
            Sector3 = sector3;
        }
    }
}