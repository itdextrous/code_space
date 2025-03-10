using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Core.IServices
{
    public interface IHttpContextService
    {
        /// <summary>
        /// Retrieves the user ID from the HttpContext.
        /// </summary>
        /// <returns>The user ID if available; otherwise, null.</returns>
        /// <remarks>
        /// This method attempts to retrieve the user ID from the HttpContext.
        /// If the HttpContext or User is null, or if the user ID claim is not found,
        /// null is returned. Any exceptions that occur during the process are logged.
        /// </remarks>
        string GetUserId();

        /// <summary>
        /// Retrieves the Product ID claim from the current HTTP context user.
        /// </summary>
        /// <returns>
        /// The Product ID if the claim is present; otherwise, null.
        /// </returns>
        string GetProductId();
    }
}
