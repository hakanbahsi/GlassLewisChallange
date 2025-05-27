namespace GlassLewisChallange.Infrastructure.Encoder
{
    public static class Base64UrlEncoder
    {
        public static string Encode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        public static byte[] Decode(string input)
        {
            input = input.Replace("-", "+").Replace("_", "/");

            switch (input.Length % 4)
            {
                case 2: input += "=="; break;
                case 3: input += "="; break;
                case 1: throw new FormatException("Invalid base64 string.");
            }

            return Convert.FromBase64String(input);
        }
    }
}
