using InPlayWiseData.Data;
using InPlayWiseData.Entities;
using InPlayWiseData.IRepositories;

namespace InPlayWiseData.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(EventModel entity)
        {
            _context.Events.Add(entity);
        }

        public void Delete(EventModel entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventModel> FindAll()
        {
            return _context.Events.ToList();
        }

        public EventModel FindByCondition(String id)
        {
            return _context.Events.FirstOrDefault(x => x.IsDeleted.Equals(false) && x.EventId.Equals(id));
        }

        public void Update(EventModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
