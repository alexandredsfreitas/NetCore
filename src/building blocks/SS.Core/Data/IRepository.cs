using SS.Core.DomainObjects;

namespace SS.Core.Data
{
	public interface IRepository<T> : IDisposable where T : IAggregateRoot
	{
	}
}

