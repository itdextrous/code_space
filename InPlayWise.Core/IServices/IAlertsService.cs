using InPlayWise.Common.DTO;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface IAlertsService
    {

        Task<Result<bool>> SetMatchAlert(MatchAlertRequestDto alert);
        Task<bool> SendAllAlerts();

    }
}