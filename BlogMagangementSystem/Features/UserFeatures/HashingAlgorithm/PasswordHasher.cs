using System.Security.Cryptography;

namespace BlogMagangementSystem.Features.UserFeatures.HashingAlgorithm
{
    public class PasswordHasher
    {
        private readonly int SaltSize = 16;
        private readonly int HashSize = 20;
        private readonly int Iterations = 10000;
        private static readonly HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA256;

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, hashAlgorithmName, HashSize);
            return $"{Convert.ToHexString(hash)}:{Convert.ToHexString(salt)}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            string [] parts = hashedPassword.Split(':');
            byte   [] salt = Convert.FromHexString(parts[1]);
            byte   [] hash = Convert.FromHexString(parts[0]);

            byte[] testHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, hashAlgorithmName, HashSize);

            return CryptographicOperations.FixedTimeEquals(hash, testHash);
        }
    }
}