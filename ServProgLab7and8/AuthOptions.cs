using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ServProgLab7and8
{

    public class AuthOptions
    {
        public const string ISSUER = "GamesServer";
        public const string AUDIENCE = "Games";
        const string KEY = "mysupersecret_secretsecretsecretkey!123";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
