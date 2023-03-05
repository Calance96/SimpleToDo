namespace SleekFlow.Todo.Application.Common.Extensions;

public static class EnumExtensions
{
	public static string GetCommaSeparatedEnumValues(this Type enumType)
		=> string.Join(", ", Enum.GetNames(enumType));
}