using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace ExampleAPI.Repository.Abstractions {
    public interface IUnitOfWork {

		IOrderItemRepository OrderItemRepository { get; }


		IRepository<TEntity> BaseRepository<TEntity>() where TEntity : class, new();

		Task<IDbContextTransaction> BeginTransactionAsync();

		Task CommitAsync(IDbContextTransaction transaction);

		Task SaveAsync();


	}
}
