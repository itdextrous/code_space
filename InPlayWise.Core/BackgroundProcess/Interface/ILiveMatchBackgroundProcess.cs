namespace InPlayWise.Core.BackgroundProcess.Interface
{
    public interface ILiveMatchBackgroundProcess
    {
        Task<bool> UploadAndUpdateLiveMatches();
        //Task<bool> deleteFinishedMatches();
        Task<bool> RefreshFilters();
    }
}
