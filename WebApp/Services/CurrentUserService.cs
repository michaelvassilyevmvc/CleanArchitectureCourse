
using Infrastructure.Interefaces.WebApp;

namespace WebApp.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string Email => "test@test.test";
    }
}