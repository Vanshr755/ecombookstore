using NationalParkAPI_1033.Models;

namespace NationalParkAPI_1033.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        User Authenticate(string username,string password);
        User Register(string  username,string password);
    }
}
