using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAASCLOUDAPP.BusinessLayer.Dto;
using Workfacta.Data.Repositories.Interfaces;
using Workfacta.Logic.Dto;
using Workfacta.Logic.Services;

namespace SAASCLOUDAPP.BusinessLayer.Services
{
    public class PeriodService
    {
        private readonly IPlanningPeriodService _planningPeriodService;
        private readonly ICompanyRepository _companyRepository;

        public PeriodService(IPlanningPeriodService planningPeriodService, ICompanyRepository companyRepository)
        {
            _planningPeriodService = planningPeriodService;
            _companyRepository = companyRepository;
        }

        public async Task<PeriodGroupDto> GetPeriodGroupByDate(string companyId, DateTime dateTimeUtc)
        {
            var company = await _companyRepository.FindById(companyId);
            if (company == null) return null;

            var companyDate = Helper.GetCurrentDateByTimeZone(company, dateTimeUtc);
            var targetDate = DateTime.SpecifyKind(companyDate, DateTimeKind.Utc).Date;

            return new PeriodGroupDto
            {
                Current = await _planningPeriodService.GetPlanningPeriod(company, targetDate),
                Group = await _planningPeriodService.GetAllPlanningPeriods(company, targetDate)
            };
        }

        public async Task<List<PlanningPeriodDetailsDto>> GetPeriodGroupByYear(string companyId, int financialYear)
        {
            var company = await _companyRepository.FindById(companyId);
            if (company == null) return null;

            return await _planningPeriodService.GetAllPlanningPeriods(company, financialYear);
        }
    }
}
