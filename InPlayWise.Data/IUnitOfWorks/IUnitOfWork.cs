using InPlayWiseData.IRepositories;

namespace Chat.Data.IUOW
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository Events { get; }
        int Save();

    }
}
