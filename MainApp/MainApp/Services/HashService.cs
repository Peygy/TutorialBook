using System.Security.Cryptography;

namespace MainApp.Services
{
    public class HashService
    {
        // Took from here ↓↓↓
        // https://stackoverflow.com/questions/20621950/asp-net-identitys-default-password-hasher-how-does-it-work-and-is-it-secure/20622428#20622428

        // Service for generating a hash of a password
        // and verifying the entered password for correctness
        public static string HashPassword(string userPassword)
        {
            byte[] salt;
            byte[] buffer1;

            if (userPassword == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(userPassword, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer1 = bytes.GetBytes(0x20);
            }

            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer1, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string userPassword)
        {
            byte[] buffer1;

            if (hashedPassword == null)
            {
                return false;
            }
            if (userPassword == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] src = Convert.FromBase64String(hashedPassword);

            if (src.Length != 0x31 || src[0] != 0)
            {
                return false;
            }

            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer2 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer2, 0, 0x20);

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(userPassword, dst, 0x3e8))
            {
                buffer1 = bytes.GetBytes(0x20);
            }
            return buffer1.SequenceEqual(buffer2);
        }
    }
}
