using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CloudDrive.Auth
{
    public static class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "MyAuthClient";
        const string KEY = "L8uL8usvxYyUX9GsvxL8usvxYyUL8usvxYyUX9GX9GYyUX9G";
        public const int LIFETIME = 5;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
