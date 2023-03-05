using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Infrastructure.Configurations;
using SleekFlow.Todo.Infrastructure.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SleekFlow.Todo.Infrastructure.Identities;

internal sealed class TokenService : ITokenService
{
	private readonly IClock _clock;
	private readonly TokenConfiguration _tokenConfiguration;

	public TokenService(
		IClock clock, 
		IOptions<TokenConfiguration> tokenConfigurationOptions)
	{
		_clock = clock;
		_tokenConfiguration = tokenConfigurationOptions.Value;
	}

	public string CreateToken(IEnumerable<Claim> claims)
	{
		SecurityKey securityKey = KeyProvider.GetSecurityKey();

		JwtSecurityToken token = new(
			audience: _tokenConfiguration.ValidAudience,
			claims: claims,
			notBefore: _clock.Now.UtcDateTime,
			expires: _clock.Now.AddSeconds(_tokenConfiguration.ValidForSeconds).UtcDateTime,
			signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256));

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}