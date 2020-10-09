namespace F1Telemetry.Core
{
    public static class CarInfo
    {
        public static F1 F1 { get; } = new F1();
    }

    public class F1
    {
        public float MaxDeployableERS { get; } = 4000000.0f;
    }
}