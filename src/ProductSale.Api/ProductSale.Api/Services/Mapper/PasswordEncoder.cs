using System.Security.Cryptography;
using System.Text;

namespace ProductSale.Api.Services.Mapper
{
    public class PasswordEncoder
    {
        public static string EncodeBase64(string password)
        {
            // Chuyển mật khẩu thành byte array
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Mã hóa byte array thành Base64
            return Convert.ToBase64String(passwordBytes);
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Chuyển mật khẩu thành byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Chuyển byte array thành chuỗi hex để lưu trữ
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool isValidPassword(string inputPassword, string hashedPassword)
        {
            // Băm mật khẩu người dùng nhập vào
            string hashedInput = HashPassword(inputPassword);

            // So sánh với mật khẩu đã lưu trong DB
            return hashedInput == hashedPassword;
        }
    }
}
