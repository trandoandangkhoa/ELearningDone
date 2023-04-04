using Microsoft.AspNetCore.Authorization;


namespace WebLearning.Application.Helper
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class SecurityRole : AuthorizeAttribute
    {

        public Contract.Dtos.AuthorizeRole[] AuthorizedRoles { get; set; }

        public SecurityRole(params Contract.Dtos.AuthorizeRole[] roles)
        {

            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("roles");

            this.Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));

        }
    }
}
