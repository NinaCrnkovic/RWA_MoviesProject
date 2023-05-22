
using IntegrationModule.Models;

namespace IntegrationModule.Services
{
    public interface IUserService
    {
        User Add(UserRegisterRequest request);
        void ValidateEmail(ValidateEmailRequest request);
        Tokens JwtTokens(JwtTokensRequest request);
        void ChangePassword(ChangePasswordRequest request);
    }
}
