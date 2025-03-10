using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAASCLOUDAPP.CommonLayer.Enums
{
    public enum CheckoutSessionStatus
    {
        /// <summary>
        /// Indicates the session is still in progress
        /// </summary>
        Pending = 0,
        /// <summary>
        /// Indicates the session was fully successful.
        /// </summary>
        Success = 1,
        /// <summary>
        /// Indicates the session has a permanent failure, and cannot be recovered.
        /// </summary>
        Failed = 2
    }
}
