using InPlayWise.Common.DTO;
using InPlayWise.Data.SportsEntities;
using InPlayWiseCommon.Wrappers;

namespace InPlayWise.Core.IServices
{
    public interface ICleverLabelServices
    {
        Task<Result<CleverLabelsDto>> GetAllLabels(string teamId);


    }
}
