using System.ComponentModel.DataAnnotations;

namespace F12020Telemetry.Data
{
    public class MarshalZone
    {
        /// <summary>
        /// Fraction (0..1) of way through the lap the marshal zone starts
        /// </summary>
        public float ZoneStart { get; set; }

        /// <summary>
        /// -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red
        /// </summary>
        public ZoneFlag ZoneFlag { get; set; }
    }

    public enum ZoneFlag : sbyte
    {
        [Display(Name = "Invalid/Unknown")]
        InvalidUnknown = -1,

        None,
        Green,
        Blue,
        Yellow,
        Red
    }
}