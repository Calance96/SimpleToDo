namespace SleekFlow.Todo.Application.Common.Interfaces;

public interface IGenericRepository<TEntity> 
	where TEntity : class
{
	Task AddAsync(TEntity entity, CancellationToken cancellationToken);

	Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

	Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
}