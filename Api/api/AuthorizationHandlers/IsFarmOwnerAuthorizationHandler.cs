using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using api.Entities;

namespace api.AuthorizationHandlers
{
    public class IsFarmOwnerAuthorizationHandler : AuthorizationHandler<IsFarmOwnerRequirement, Farm>  
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            IsFarmOwnerRequirement requirement,
            Farm farm)
        {
            // if (context.User.HasClaim(claim => claim.Type == "EmployeeNumber" && claim.Issuer == lounge.AirlineName))
            
            // if(farm.Farmer.Id == context.User.Clone())
            string name = context.User.Identity?.Name;
            // if(true)
            // {
                context.Succeed(requirement);
            // }

            return Task.CompletedTask;
        }
    }
        public class IsFarmOwnerRequirement: IAuthorizationRequirement
    {

    }
}