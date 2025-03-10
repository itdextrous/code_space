using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using SAASCLOUDAPP.BusinessLayer.Services;
using Workfacta.Common.Enums;

namespace SAASCLOUDAPP.API.Authorization
{
    internal class UserAccessor : IUserAccessor
    {
        public string GetUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }

        public bool IsInRole(RolesEnum role)
        {
            return HttpContext.Current.User.IsInRole(role.ToString());
        }
    }
}