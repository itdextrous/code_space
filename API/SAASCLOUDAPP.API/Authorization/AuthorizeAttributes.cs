using System.Collections.Generic;
using System.Web.Http;
using Workfacta.Common.Enums;

namespace SAASCLOUDAPP.API.Authorization
{
    internal class BaseAuthorizeAttribute : AuthorizeAttribute
    {
        public BaseAuthorizeAttribute(IEnumerable<RolesEnum> roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }

    internal class AuthorizeSuperAdminAttribute : BaseAuthorizeAttribute
    {
        public AuthorizeSuperAdminAttribute() : base(new[] { RolesEnum.SuperAdmin })
        {
        }
    }

    internal class AuthorizeEnterpriseAdminAttribute : BaseAuthorizeAttribute
    {
        public AuthorizeEnterpriseAdminAttribute() : base(new[] {
            RolesEnum.SuperAdmin,
            RolesEnum.EnterpriseAdmin })
        {
        }
    }

    internal class AuthorizeAdminAttribute : BaseAuthorizeAttribute
    {
        public AuthorizeAdminAttribute() : base(new[] {
            RolesEnum.SuperAdmin,
            RolesEnum.EnterpriseAdmin,
            RolesEnum.PartnerAdmin,
            RolesEnum.Partner,
            RolesEnum.Admin })
        {
        }
    }

    internal class AuthorizeFullUserAttribute : BaseAuthorizeAttribute
    {
        public AuthorizeFullUserAttribute() : base(new[] {
            RolesEnum.SuperAdmin,
            RolesEnum.EnterpriseAdmin,
            RolesEnum.PartnerAdmin,
            RolesEnum.Partner,
            RolesEnum.Admin,
            RolesEnum.User,
            RolesEnum.Advisor })
        {
        }
    }

    internal class AuthorizePartnerAdminAttribute : BaseAuthorizeAttribute
    {
        public AuthorizePartnerAdminAttribute() : base(new[]
        {
            RolesEnum.SuperAdmin,
            RolesEnum.PartnerAdmin
        })
        {
        }
    }

    internal class AuthorizePartnerManagerAttribute : BaseAuthorizeAttribute
    {
        public AuthorizePartnerManagerAttribute() : base(new[]
        {
            RolesEnum.SuperAdmin,
            RolesEnum.PartnerAdmin,
            RolesEnum.EnterpriseAdmin
        })
        {
        }
    }

    internal class AuthorizePartnerAttribute : BaseAuthorizeAttribute
    {
        public AuthorizePartnerAttribute() : base(new[]
        {
            RolesEnum.SuperAdmin,
            RolesEnum.PartnerAdmin,
            RolesEnum.Partner
        })
        {
        }
    }
}