using Chat.Data.IUOW;
using InPlayWiseData.Data;
using InPlayWiseData.IRepositories;
using InPlayWiseData.Repository;

namespace InPlayWiseData.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IEventRepository _event;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IEventRepository Events
        {
            get
            {
                _event ??= new EventRepository(_context);
                return _event;
            }
        }

        public int Save()
        {
            var a = _context.SaveChanges();
            return a;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                _context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true) ;
            GC.SuppressFinalize(this) ;
        }

    }
}
