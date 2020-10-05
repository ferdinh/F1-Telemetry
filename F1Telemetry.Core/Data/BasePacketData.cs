namespace F1Telemetry.Core.Data
{
    /// <summary>
    /// Data that derives or retrieved from packet data.
    /// </summary>
    public abstract class BasePacketData
    {
        public ulong SessionUID { get; }
        public float SessionTime { get; }

        public BasePacketData(ulong sessionUID, float sessionTime)
        {
            SessionUID = sessionUID;
            SessionTime = sessionTime;
        }
    }
}