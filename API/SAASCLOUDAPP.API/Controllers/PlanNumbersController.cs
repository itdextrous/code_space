using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using NLog;
using SAASCLOUDAPP.API.Configuration;
using SAASCLOUDAPP.API.Models;
using SAASCLOUDAPP.BusinessLayer;
using SAASCLOUDAPP.BusinessLayer.Dto;
using SAASCLOUDAPP.BusinessLayer.Services;
using SAASCLOUDAPP.CommonLayer.Models;
using Workfacta.Common.Enums;
using Workfacta.Logic.Extensions;
using Workfacta.Logic.Services;
using Workfacta.Models;
using Workfacta.Models.Library;
using Workfacta.Shared.Providers;

namespace SAASCLOUDAPP.API.Controllers
{
    [RoutePrefix("api/companies/{companyId}/teams/{teamId}/periods/{period}/numbers")]
    [Authorize]
    public class PlanNumbersController : BaseApiController
    {
        private readonly AuthorizationService _authorizationService;
        private readonly IEventLogger _eventLogger;
        private readonly FirebaseRepository _firebaseRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly IDateProvider _dateProvider;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public PlanNumbersController( AuthorizationService authorizationService, IEventLogger eventLogger, FirebaseRepository firebaseRepository, CompanyRepository companyRepository, IDateProvider dateProvider)
        {
            _model = model;
            _authorizationService = authorizationService;
            _eventLogger = eventLogger;
            _firebaseRepository = firebaseRepository;
            _companyRepository = companyRepository;
            _dateProvider = dateProvider;
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(ListNumbersDto))]
        public async Task<IHttpActionResult> ListNumbers(
            [Required] string companyId,
            [Required] string teamId,
            [Required] FiscalQuarterDto period
            )
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await ValidateAccess(companyId, teamId))
            {
                return Forbidden();
            }

            var company = await _model.Companies[companyId];
            var team = await company.Teams[teamId];
            var quarter = team.Quarter(period);
            var numbers = quarter.QuarterlyNumbers;
            await SafeEnsurePreviousQuarterRecurringNumbersCopied(numbers);

            var currentWeek = await quarter.GetCurrentWeek();
            var numberList = await numbers.All();

            var result = new ListNumbersDto
            {
                Numbers = await numberList.Select(async n => await GetNumberDto.From(n, currentWeek)).ToListAsync(),
                PlanningStatus = await numbers.GetStatus()
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("~/api/enterprise/{companyId}/numbers")]
        [ResponseType(typeof(List<SimpleNumberDto>))]
        public async Task<IHttpActionResult> GetEnterpriseNumbers([Required] string companyId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _authorizationService.CanAccessCompany(companyId) || !await _authorizationService.EnterprisePackEnabled(companyId))
                return Forbidden();

            var currentClientInstanceId = (await _model.Companies[companyId]).ParentClient.Id;
            var companies = (await _companyRepository.GetCompaniesMenuData(UserId))
                .Where(c => c.clientInstanceId == currentClientInstanceId).ToList();
            var now = _dateProvider.UtcNow;
            var result = new List<SimpleNumberDto>();
            foreach (var c in companies)
            {
                var company = await _model.Companies[c.companyId];
                foreach (var t in c.companyData)
                {
                    var team = await company.Teams[t.teamId];
                    var quarter = await team.Quarter(now);
                    var currentWeek = await quarter.GetCurrentWeek();
                    var numberList = (await quarter.QuarterlyNumbers.All()).ToList();
                    result.AddRange(numberList.Select(n => new SimpleNumberDto(n)));
                }
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("status")]
        [ResponseType(typeof(ListNumbersDto))]
        public async Task<IHttpActionResult> UpdateStatus(
            [Required] string companyId,
            [Required] string teamId,
            [Required] FiscalQuarterDto period,
            [Required] UpdatePlanningStatusDto dto
            )
        {
            if (dto == null) ModelState.AddModelError(nameof(dto), $"{nameof(dto)} is required");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await ValidateAccess(companyId, teamId))
            {
                return Forbidden();
            }

            var company = await _model.Companies[companyId];
            var team = await company.Teams[teamId];
            var quarter = team.Quarter(period);
            var numbers = quarter.QuarterlyNumbers;
            await SafeEnsurePreviousQuarterRecurringNumbersCopied(numbers);
            await numbers.SetStatus(dto.PlanningStatus);

            var currentWeek = await quarter.GetCurrentWeek();
            var numberList = await numbers.All();

            var result = new ListNumbersDto
            {
                Numbers = await numberList.Select(async n => await GetNumberDto.From(n, currentWeek)).ToListAsync(),
                PlanningStatus = await numbers.GetStatus()
            };

            return Ok(result);
        }

        [HttpPost]
        [ResponseType(typeof(List<int>))]
        [Route("schedule")]
        public async Task<IHttpActionResult> GetExpectedScheduleWeeks(
            [Required] string companyId,
            [Required] string teamId,
            [Required] FiscalQuarterDto period,
            [Required] UpdateScheduleDto dto
            )
        {
            if (dto == null) ModelState.AddModelError(nameof(dto), $"{nameof(dto)} is required");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await ValidateAccess(companyId, teamId))
            {
                return Forbidden();
            }

            var company = await _model.Companies[companyId];
            var team = await company.Teams[teamId];

            var schedule = await team.Quarter(period).GetExpectedSchedule(dto.ToEntity());
            return Ok(schedule.Select(w => w.Value).ToList());
        }

        [HttpGet]
        [Route("{numberId}")]
        [ResponseType(typeof(GetNumberDto))]
        public async Task<IHttpActionResult> GetNumber(
            [Required] string companyId,
            [Required] string teamId,
            [Required] FiscalQuarterDto period,
            [Required] string numberId
        )
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await ValidateAccess(companyId, teamId)) return Forbidden();

            var company = await _model.Companies[companyId];
            var team = await company.Teams[teamId];
            var quarter = team.Quarter(period);
            var currentWeek = await quarter.GetCurrentWeek();

            try
            {
                var number = await quarter.QuarterlyNumbers[numberId];
                return Ok(await GetNumberDto.From(number, currentWeek));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{numberId}/actions")]
        [ResponseType(typeof(List<CompanyTeamActionDto>))]
        public async Task<IHttpActionResult> GetActionsForNumber(
            [Required] string companyId,
            [Required] string teamId,
            [Required] FiscalQuarterDto period,
            [Required] string numberId
        )
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await ValidateAccess(companyId, teamId))
            {
                return Forbidden();
            }

            var company = await _model.Companies[companyId];
            var team = await company.Teams[teamId];
            var quarter = team.Quarter(period);

            try
            {
                var number = await quarter.QuarterlyNumbers[numberId];
                var actions = await number.LinkedItems.Actions;
                return Ok(actions.Select(a => (CompanyTeamActionDto)a).ToList());
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("global/{globalId}")]
        [ResponseType(typeof(GetNumberDto))]
        public async Task<IHttpActionResult> GetNumberByGlobalId(
            [Required] string companyId,
            [Required] string teamId,
            [Required] FiscalQuarterDto period,
            [Required] string globalId
        )
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await ValidateAccess(companyId, teamId))
            {
                return Forbidden();
            }

            var company = await _model.Companies[companyId];
            var team = await company.Teams[teamId];
            var quarter = team.Quarter(period);
            var currentWeek = await quarter.GetCurrentWeek();

            try
            {
                var number = await quarter.QuarterlyNumbers.TryGetByGlobalId(globalId);
                if (number == null) return NotFound();
                return Ok(await GetNumberDto.From(number, currentWeek));
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("")]
        [ResponseType(typeof(GetNumberDto))]
        public async Task<IHttpActionResult> AddNumber(
            [Required] string companyId,
            [Required] string teamId,
            [Required] FiscalQuarterDto period,
            [Required] UpdateNumberDto dto
        )
        {
            if (dto == null) ModelState.AddModelError(nameof(dto), $"{nameof(dto)} is required");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await ValidateAccess(companyId, teamId))
            {
                return Forbidden();
            }

            var company = await _model.Companies[companyId];
            var team = await company.Teams[teamId];
            var number = await team.Quarter(period).QuarterlyNumbers.Add();

            await dto.ApplyTo(number);

            await number.SaveAsync();
            await LogEventInternal(number, "Number Added");
            await NotifyChanges(number, isAdded: true);

            var currentWeek = await team.Quarter(period).GetCurrentWeek();
            return Ok(await GetNumberDto.From(number, currentWeek));
        }

        [HttpPut]
        [Route("{numberId}")]
        [ResponseType(typeof(GetNumberDto))]
        public async Task<IHttpActionResult> UpdateNumber(
            [Required] string companyId,
            [Required] string teamId,
            [Required] FiscalQuarterDto period,
            [Required] string numberId,
            [Required] UpdateNumberDto dto
        )
        {
            if (dto == null) ModelState.AddModelError(nameof(dto), $"{nameof(dto)} is required");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await ValidateAccess(companyId, teamId))
            {
                return Forbidden();
            }

            var company = await _model.Companies[companyId];
            var team = await company.Teams[teamId];

            Number number;
            try
            {
                number = await team.Quarter(period).QuarterlyNumbers[numberId];
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }

            await dto.ApplyTo(number);

            await number.SaveAsync();
            await LogEventInternal(number, "Number Updated");
            await NotifyChanges(number);

            var currentWeek = await team.Quarter(period).GetCurrentWeek();
            return Ok(await GetNumberDto.From(number, currentWeek));
        }

        [HttpDelete]
        [Route("{numberId}")]
        public async Task<IHttpActionResult> DeleteNumber(
            [Required] string companyId,
            [Required] string teamId,
            [Required] FiscalQuarterDto period,
            [Required] string numberId
        )
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await ValidateAccess(companyId, teamId))
            {
                return Forbidden();
            }

            var company = await _model.Companies[companyId];
            var team = await company.Teams[teamId];

            Number number;
            try
            {
                number = await team.Quarter(period).QuarterlyNumbers[numberId];
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }

            await number.DeleteAsync();
            await LogEventInternal(number, "Number Deleted");
            await NotifyChanges(number);

            return Ok();
        }

        private const string FileNameParam = "file";

        [HttpPost]
        [Route("{numberId}/attachment")]
        [ResponseType(typeof(GetNumberDto))]
        [AcceptsFile(FileNameParam)]
        public async Task<IHttpActionResult> AddNumberAttachment(
            [Required] string companyId,
            [Required] string teamId,
            [Required] FiscalQuarterDto period,
            [Required] string numberId
        )
        {
            var file = HttpContext.Current.Request.Files[FileNameParam];
            if (file == null) ModelState.AddModelError(FileNameParam, $"{FileNameParam} is required");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await ValidateAccess(companyId, teamId))
            {
                return Forbidden();
            }

            var company = await _model.Companies[companyId];
            var team = await company.Teams[teamId];

            Number number;
            try
            {
                number = await team.Quarter(period).QuarterlyNumbers[numberId];
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }
            await number.AddAttachment(file.FileName, file.ContentType, file.InputStream);
            await number.SaveAsync();
            await NotifyChanges(number);

            var currentWeek = await team.Quarter(period).GetCurrentWeek();
            return Ok(await GetNumberDto.From(number, currentWeek));
        }

        [HttpPut]
        [Route("{numberId}/attachment/delete")]
        [ResponseType(typeof(GetNumberDto))]
        public async Task<IHttpActionResult> DeleteNumberAttachments(
            [Required] string companyId,
            [Required] string teamId,
            [Required] FiscalQuarterDto period,
            [Required] string numberId,
            [FromBody, Required] DeleteAttachmentsDto dto
        )
        {
            if (dto == null) ModelState.AddModelError(nameof(dto), $"{nameof(dto)} is required");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await ValidateAccess(companyId, teamId))
            {
                return Forbidden();
            }

            var company = await _model.Companies[companyId];
            var team = await company.Teams[teamId];

            Number number;
            try
            {
                number = await team.Quarter(period).QuarterlyNumbers[numberId];
            }
            catch (ModelNotFoundException)
            {
                return NotFound();
            }

            dto.AttachmentPaths.ForEach(number.RemoveAttachment);
            await number.SaveAsync();
            await NotifyChanges(number);

            var currentWeek = await team.Quarter(period).GetCurrentWeek();
            return Ok(await GetNumberDto.From(number, currentWeek));
        }

        private async Task<bool> ValidateAccess(string companyId, string teamId)
        {
            return await _authorizationService.CanAccessCompany(companyId) &&
                await _authorizationService.CanAccessTeam(teamId);
        }

        private async Task LogEventInternal(Number number, string eventName)
        {
            await _eventLogger.LogEvent(EventLogType.Number, $"{eventName} - {number.Description}", UserId, number.Company.Id, number.Team.Id, number.Id, (number.FiscalQuarter.Year, number.FiscalQuarter.Value));
        }

        private async Task SafeEnsurePreviousQuarterRecurringNumbersCopied(QuarterlyNumbers numbers)
        {
            try
            {
                await numbers.EnsurePreviousQuarterRecurringNumbersCopied();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unable to copy recurring numbers to {numbers.FiscalQuarter} for team with ID \"{numbers.Team.Id}\".");
            }
        }

        private async Task NotifyChanges(Number number, bool isAdded = false, string screen = default)
        {
            await _firebaseRepository.saveRecordDetails(new CommonLayer.Models.FirebaseScreenModel
            {
                screenName = "Update",
                recordId = isAdded ? string.Empty : (string)number.Id,
                operationName = isAdded ? "Added Number" : "Updated Number",
                screen = screen != "Mobile" ? "web" : screen
            });
        }
    }
}