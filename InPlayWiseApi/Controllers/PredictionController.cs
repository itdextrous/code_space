using InPlayWise.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseApi.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [EnableCors("AllowAllOrigins")]
    public class PredictionController : Controller
    {
        private readonly ILogger<PredictionController> _logger;
        //private readonly IPredictionServices _predictionService;

        //public PredictionController(ILogger<PredictionController> logger, IPredictionServices prediction)
        //{
        //    _predictionService = prediction;
        //    _logger = logger;
        //}

        //[HttpGet("trainModelAndUpdateDb")]
        //public async Task<IActionResult> TrainModelAndUpdateDatabase()
        //{
        //    return Ok(await _predictionService.TrainModelAndUpdateDatabase());
        //}


        //[HttpGet("GetAllOpportunitiesDto")]
        //public async Task<IActionResult> GetAllOpportunitiesDto()
        //{
        //    return Ok(await _predictionService.GetAllOpportunitiesDto());
        //}


        /// <summary>
        /// Retrieves prediction results as JSON data.
        /// </summary>
        /// <returns>An IActionResult containing JSON data representing prediction results.</returns>

        //[HttpGet("goalPredict/{matchId}")]
        //public async Task<IActionResult> PredictGoal(string matchId)
        //{
        //    try
        //    {
        //        return Ok(await _predictionService.PredictGoal(matchId));   
        //    }catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return Ok(false);
        //    }
        //}


        //[HttpGet("fetchPredictionData")]
        //public async Task<IActionResult> FetchPredictionData()
        //{
        //    return Ok(await _predictionService.FetchPredictionData());
        //}

        //[HttpGet("trainModel")]
        //public async Task<IActionResult> TrainModel()
        //{
        //    return Ok(await _predictionService.TrainModel());
        //}

        //[HttpGet("TrainAll")]
        //public async Task<IActionResult> TrainAll()
        //{
        //    return Ok(await _predictionService.TrainAll());
        //}

        //[HttpPost("UploadPredictionData")]
        //public async Task<IActionResult> UploadPredictionData()
        //{
        //    return Ok(await _predictionService.UploadDailyPredictionData());
        //}

    }
}
