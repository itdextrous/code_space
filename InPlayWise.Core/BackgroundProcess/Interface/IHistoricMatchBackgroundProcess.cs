using InPlayWise.Data.Entities.SportsEntities;

namespace InPlayWise.Core.BackgroundProcess.Interface
{
    public interface IHistoricMatchBackgroundProcess
	{

		Task<bool> SeedMatchesInMemory();

		Task<bool> CompleteMatchesInfo();

		Task CompleteMatchInfo(RecentMatchModel match);

	}
}
