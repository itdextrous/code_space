using InPlayWiseData.Entities;

namespace InPlayWiseData.IRepositories
{
    public interface IEventRepository
    {
        IEnumerable<EventModel> FindAll();
        EventModel FindByCondition(string id);
        void Create(EventModel entity);
        void Update(EventModel entity);
        void Delete(EventModel entity);

    }
}
