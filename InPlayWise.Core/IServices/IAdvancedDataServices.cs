using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseCore.IServices.IFootballServices
{
    public interface IAdvancedDataServices
    {
        public Task<IActionResult> SeasonTeamStatistics(string uuid);
        public Task<IActionResult> SeasonPlayerStatistics(string uuid);
        public Task<IActionResult> SeasonTopScorer(string uuid);
        public Task<IActionResult> TeamLineup(int page, int time, string uuid);
        public Task<IActionResult> BestLineup(int page, int time, string uuid);
        public Task<IActionResult> BestLineUpDetails(string uuid);
        public Task<IActionResult> TeamInjury(int page, int time, string uuid);
        public Task<IActionResult> PlayerTransfer(int page, int time, string uuid);
        public Task<IActionResult> FifaMensRanking(int pub_time);
        public Task<IActionResult> FifaWomenRanking(int pub_time);
        public Task<IActionResult> WorldClubsRanking();
        public Task<IActionResult> Bracket(string uuid);
        public Task<IActionResult> CoachCoachingResume(int page, int time, string uuid);

    }
}
