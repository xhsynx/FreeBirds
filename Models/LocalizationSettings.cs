namespace FreeBirds.Models
{
    /// <summary>
    /// Settings for JSON-based localization
    /// </summary>
    public class LocalizationSettings
    {
        /// <summary>
        /// Default culture to use when no specific culture is requested
        /// </summary>
        public string DefaultCulture { get; set; } = "tr-TR";

        /// <summary>
        /// List of supported cultures in the application
        /// </summary>
        public string[] SupportedCultures { get; set; } = new[] { "tr-TR", "en-US" };

        /// <summary>
        /// Dictionary containing localized messages for each culture
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Messages { get; set; } = new();
    }
} 