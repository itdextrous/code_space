using System;
using Workfacta.Entities;

namespace SAASCLOUDAPP.DataAccessLayer.Entities
{
    public static class NumbersExtensions
    {
        /// <summary>
        /// If a new number has been created set the global ID.
        /// </summary>
        public static Numbers SetGlobalId(this Numbers number) =>
            SetGlobalId(number, Guid.NewGuid().ToString());

        /// <summary>
        /// If a number has been copied set the global ID from the copied field.
        /// </summary>
        public static Numbers SetGlobalId(this Numbers number, string globalId)
        {
            if (String.IsNullOrEmpty(globalId))
                throw new ArgumentNullException(nameof(globalId));

            if (!string.IsNullOrEmpty(number.globalId))
                throw new InvalidOperationException($"Global ID should not be set for number {number.id} {number.description}");

            number.globalId = globalId;

            return number;
        }
    }
}
