using System;

namespace SAASCLOUDAPP.API
{
    public class Utilities
    {
        /// <summary>
        /// Returns a "Unique" file name based off the current time and the name of the file
        /// </summary>
        /// <param name="fileName">The unmodified name of the file</param>
        /// <returns>The name that should be used when saving the file.</returns>
        // This deliberately does not use the injected time, as the file name generation should not be dependent of the user's time.
        public static string GetUniqueFileName(string fileName) => DateTime.UtcNow.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + "_" + fileName.Replace(" ", "");
    }
}