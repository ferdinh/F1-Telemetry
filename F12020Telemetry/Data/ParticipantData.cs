namespace F12020Telemetry.Data
{
    public class ParticipantData
    {
        /// <summary>
        /// Whether the vehicle is AI (1) or Human (0) controlled
        /// </summary>
        public bool AiControlled;

        /// <summary>
        /// Driver id - see appendix
        /// </summary>
        public byte DriverId;

        /// <summary>
        /// Team id - see appendix
        /// </summary>
        public byte TeamId;

        /// <summary>
        /// Race number of the car
        /// </summary>
        public byte RaceNumber;

        /// <summary>
        /// Nationality of the driver
        /// </summary>
        public byte Nationality;

        /// <summary>
        /// Name of participant in UTF-8 format – null terminated
        /// Will be truncated with … (U+2026) if too long
        /// </summary>
        public char[] Name;

        /// <summary>
        /// The player's UDP setting.
        /// </summary>
        public TelemetrySetting YourTelemetry;

        public static string[] NationalityNames = new string[]
        {
            "Unknown",
            "American",
            "Argentinean",
            "Australian",
            "Austrian",
            "Azerbaijani",
            "Bahraini",
            "Belgian",
            "Bolivian",
            "Brazilian",
            "British",
            "Bulgarian",
            "Cameroonian",
            "Canadian",
            "Chilean",
            "Chinese",
            "Colombian",
            "Costa Rican",
            "Croatian",
            "Cypriot",
            "Czech",
            "Danish",
            "Dutch",
            "Ecuadorian",
            "English",
            "Emirian",
            "Estonian",
            "Finnish",
            "French",
            "German",
            "Ghanaian",
            "Greek",
            "Guatemalan",
            "Honduran",
            "Hong Konger",
            "Hungarian",
            "Icelander",
            "Indian",
            "Indonesian",
            "Irish",
            "Israeli",
            "Italian",
            "Jamaican",
            "Japanese",
            "Jordanian",
            "Kuwaiti",
            "Latvian",
            "Lebanese",
            "Lithuanian",
            "Luxembourger",
            "Malaysian",
            "Maltese",
            "Mexican",
            "Monegasque",
            "New Zealander",
            "Nicaraguan",
            "North Korean",
            "Northern Irish",
            "Norwegian",
            "Omani",
            "Pakistani",
            "Panamanian",
            "Paraguayan",
            "Peruvian",
            "Polish",
            "Portuguese",
            "Qatari",
            "Romanian",
            "Russian",
            "Salvadoran",
            "Saudi",
            "Scottish",
            "Serbian",
            "Singaporean",
            "Slovakian",
            "Slovenian",
            "South Korean",
            "South African",
            "Spanish",
            "Swedish",
            "Swiss",
            "Thai",
            "Turkish",
            "Uruguayan",
            "Ukrainian",
            "Venezuelan",
            "Welsh",
            "Barbadian",
            "Vietnamese",
        };
    }

    public enum TelemetrySetting
    {
        Restricted,
        Public
    }
}