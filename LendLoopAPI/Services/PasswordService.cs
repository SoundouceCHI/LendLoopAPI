using LendLoopAPI.ModelDto;
using LendLoopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LendLoopAPI.Services
{
    public class PasswordService
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool CheckAuth(UserAppLogin user, UserApp userDB)
        {
            if (user == null || userDB == null)
            {
                return false;
            }
            return BCrypt.Net.BCrypt.Verify(user.PasswordHash, userDB.PasswordHash);

        }
    }
}
