using Application.Auth;
using Domain.Auth;

namespace Application.Validation
{
    public class AuthValidationRules
    {
        private readonly IUserRepository _userRepository;

        public AuthValidationRules(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> IsUniqueUsername(string username)
        {
            User user = await _userRepository.GetByUsername(username);

            return user is null;
        }
    }
}
