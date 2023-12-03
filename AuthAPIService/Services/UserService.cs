using AuthAPIService.Entities;

namespace AuthAPIService.Services
{
    public class UserService
    {
        public async Task<bool> FindByEmailAsync(string email) => false;

        public async Task<User> FindByEmailAndPasswordAsync(string email, string password)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Name = "Bill Kennedy",
                Email = email,
                Password = password,
                CreatedAt = DateTime.Now,
            };
        }

        public async Task<bool> CreateAsync(User user) => true;
    }
}
