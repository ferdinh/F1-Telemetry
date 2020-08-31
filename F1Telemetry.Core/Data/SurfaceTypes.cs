using System.ComponentModel.DataAnnotations;

namespace F1Telemetry.Core.Data
{
    public enum SurfaceTypes : byte
    {
        [Display(Name = "Tarmac")]
        Tarmac,

        [Display(Name = "Rumble Strip")]
        RumbleStrip,

        [Display(Name = "Concrete")]
        Concrete,

        [Display(Name = "Rock")]
        Rock,

        [Display(Name = "Gravel")]
        Gravel,

        [Display(Name = "Mud")]
        Mud,

        [Display(Name = "Sand")]
        Sand,

        [Display(Name = "Grass")]
        Grass,

        [Display(Name = "Water")]
        Water,

        [Display(Name = "Cobblestone")]
        Cobblestone,

        [Display(Name = "Metal")]
        Metal,

        [Display(Name = "Ridged")]
        Ridged,
    }
}