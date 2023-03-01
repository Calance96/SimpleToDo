using SleekFlow.Todo.Application.Common.Interfaces;

namespace SleekFlow.Todo.Infrastructure.Services;

internal sealed class Clock : IClock
{
    public DateTimeOffset CurrentTime => DateTimeOffset.UtcNow;
}