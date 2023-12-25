using System.Security.Claims;

namespace CloudDrive.Utilities
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            Claim claim = user.FindAll(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

            if (claim is null)
            {
                throw new Exception("Id пользователя не найден");
            }

            return claim.Value;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            Claim claim = user.FindAll(x => x.Type == ClaimTypes.Name).FirstOrDefault();

            if (claim is null)
            {
                throw new Exception("Username пользователя не найден");
            }

            return claim.Value;
        }
    }
}
