using System.Threading.Tasks;
using Workfacta.Models;
using Workfacta.Models.Library;

namespace SAASCLOUDAPP.API.Providers
{
    public static class ModelExtensions
    {
        public static async Task<CompanyTeamQuarter> GetCompanyTeamQuarter(
            this WorkfactaModel model, 
            ModelId companyId, 
            ModelId teamId, 
            string qtr, 
            string fiscalYear)
        {
            var company = await model.Companies[companyId];
            var team = await company.Teams[teamId];

            return team.Quarter(new FiscalQuarter(int.Parse(qtr), int.Parse(fiscalYear)));
        }
    }
}