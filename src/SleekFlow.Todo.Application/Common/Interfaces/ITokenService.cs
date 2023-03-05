using System.Security.Claims;

namespace SleekFlow.Todo.Application.Common.Interfaces;

public interface ITokenService
{
	string CreateToken(IEnumerable<Claim> claims);
}