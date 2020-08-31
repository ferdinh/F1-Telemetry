using System.ComponentModel.DataAnnotations;

namespace F1Telemetry.Core.Data
{
    public enum WeatherType : byte
    {
        Clear,

        [Display(Name = "Light Cloud")]
        LightCloud,

        Overcast,

        [Display(Name = "Light Rain")]
        LightRain,

        [Display(Name = "Heavy Rain")]
        HeavyRain,

        Storm
    }
}