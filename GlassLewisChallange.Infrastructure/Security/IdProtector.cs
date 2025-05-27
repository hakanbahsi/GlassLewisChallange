using GlassLewisChallange.Infrastructure.Encoder;
using System.Security.Cryptography;
using System.Text;

namespace GlassLewisChallange.Infrastructure.Security
{
    public class IdProtector : IIdProtector
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public IdProtector()
        {
            _key = SHA256.HashData(Encoding.UTF8.GetBytes("secret-key-glasslewis"));
            _iv = Encoding.UTF8.GetBytes("glass-lewis-iv123")[..16];
        }

        public string Protect(Guid id)
        {
            var plainBytes = id.ToByteArray();

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var encryptor = aes.CreateEncryptor();
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return Base64UrlEncoder.Encode(encryptedBytes);
        }

        public Guid Unprotect(string protectedId)
        {
            var encryptedBytes = Base64UrlEncoder.Decode(protectedId);

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var decryptor = aes.CreateDecryptor();
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return new Guid(decryptedBytes);
        }

        public bool CanUnprotect(string encrypted)
        {
            try
            {
                Unprotect(encrypted);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
