using MainApp.Services;

namespace MainApp.Tests.Services
{
    public class PasswordHashingTests
    {
        [Theory]
        [InlineData("ABCDEFG")]
        [InlineData("TestPassword_12343454356")]
        [InlineData("{}{'.,./'Psadf23424")]
        public void PasswordHashingTests_ReturnsTrue(string password)
        {
            var hashedPassword = HashService.HashPassword(password);
            Assert.True(HashService.VerifyHashedPassword(hashedPassword, password));
        }
    }
}
