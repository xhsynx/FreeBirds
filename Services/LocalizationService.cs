using System.Globalization;
using FreeBirds.Models;

namespace FreeBirds.Services
{
    /// <summary>
    /// Service for handling JSON-based localization
    /// </summary>
    public class LocalizationService
    {
        private readonly LocalizationSettings _settings;

        /// <summary>
        /// Initializes a new instance of the LocalizationService
        /// </summary>
        /// <param name="settings">Localization settings containing messages and supported cultures</param>
        public LocalizationService(LocalizationSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Gets a localized message for the specified key and culture
        /// </summary>
        /// <param name="key">The message key</param>
        /// <param name="culture">The target culture</param>
        /// <param name="args">Optional format arguments</param>
        /// <returns>Localized message string</returns>
        public string GetMessage(string key, string culture, params object[] args)
        {
            if (!_settings.Messages.TryGetValue(culture, out var messages))
            {
                culture = _settings.DefaultCulture;
                messages = _settings.Messages[culture];
            }

            if (!messages.TryGetValue(key, out var message))
            {
                return key;
            }

            return string.Format(message, args);
        }

        /// <summary>
        /// Gets the localized field name for the specified key and culture
        /// </summary>
        /// <param name="key">The field key</param>
        /// <param name="culture">The target culture</param>
        /// <returns>Localized field name</returns>
        public string GetFieldName(string key, string culture)
        {
            return GetMessage(key, culture);
        }

        /// <summary>
        /// Gets a localized validation message for the specified key and culture
        /// </summary>
        /// <param name="key">The validation message key</param>
        /// <param name="culture">The target culture</param>
        /// <param name="args">Optional format arguments</param>
        /// <returns>Localized validation message</returns>
        public string GetValidationMessage(string key, string culture, params object[] args)
        {
            return GetMessage(key, culture, args);
        }
    }
} 