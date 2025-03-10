namespace InPlayWise.Core.BackgroundProcess.Interface
{
    public interface IUserDataBackgroundProcess
    {
        Task<bool> UpdateUserAccumulators(string userId);
    }
}
