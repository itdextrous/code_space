using Microsoft.AspNetCore.Mvc;

namespace InPlayWise.Core.IServices
{
    public interface IApiDataFetchServices
    {
        Task<IActionResult> DataFetcher(string url);
    }
}
