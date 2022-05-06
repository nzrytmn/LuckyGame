using LuckyGame.Api.Entities;

namespace LuckyGame.Api.Contracts
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
