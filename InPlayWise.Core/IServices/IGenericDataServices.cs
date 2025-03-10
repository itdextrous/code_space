namespace InPlayWise.Core.IServices
{
    public interface IGenericServices<T> where T : class
    {
        IEnumerable<T> FindAll();
        T FindByCondition(int id);
    }
}
