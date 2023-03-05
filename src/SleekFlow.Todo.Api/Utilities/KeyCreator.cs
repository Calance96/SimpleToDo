using System.Security.Cryptography;

namespace SleekFlow.Todo.Api.Utilities;

internal static class KeyCreator
{
    public static void CreateKeyPairs()
    {
        RSA rsa = RSA.Create(4096);
        byte[] publicKey = rsa.ExportRSAPublicKey();
        byte[] privateKey = rsa.ExportRSAPrivateKey();

        File.WriteAllBytes(Path.Combine("Securities", "public.key"), publicKey);
        File.WriteAllBytes(Path.Combine("Securities", "private.key"), privateKey);
    }
}
