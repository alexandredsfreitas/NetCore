namespace SS.Core.Data
{
	public interface IUnitOfWork
	{
		Task<bool> Commit();
	}
}

