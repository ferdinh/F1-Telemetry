using System.ComponentModel.DataAnnotations;

namespace F1Telemetry.Core.Data
{
    public enum SessionType : byte
    {
        Unknown,

        [Display(Name = "Practice 1")]
        P1,

        [Display(Name = "Practice 2")]
        P2,

        [Display(Name = "Practice 3")]
        P3,

        [Display(Name = "Short Practice")]
        ShortP,

        [Display(Name = "Qualifying 1")]
        Q1,

        [Display(Name = "Qualifying 2")]
        Q2,

        [Display(Name = "Qualifying 3")]
        Q3,

        [Display(Name = "Short Qualifying")]
        ShortQ,

        [Display(Name = "One Shot Qualifying")]
        OSQ,

        Race,

        [Display(Name = "Race 2")]
        Race2,

        [Display(Name = "Time Trial")]
        TimeTrial
    }
}