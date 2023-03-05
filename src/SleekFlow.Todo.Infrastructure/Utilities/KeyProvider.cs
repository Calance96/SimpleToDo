using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace SleekFlow.Todo.Infrastructure.Utilities;

internal static class KeyProvider
{
	public static SecurityKey GetSecurityKey()
	{
		RSA rsa = RSA.Create();
		rsa.ImportRSAPublicKey(File.ReadAllBytes(Path.Combine("Securities", "public.key")), out _);
		rsa.ImportRSAPrivateKey(File.ReadAllBytes(Path.Combine("Securities", "private.key")), out _);

		RsaSecurityKey rsaSecurityKey = new(rsa);

		return rsaSecurityKey;
	}
}