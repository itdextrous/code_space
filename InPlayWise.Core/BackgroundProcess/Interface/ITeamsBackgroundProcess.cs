using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Core.BackgroundProcess.Interface
{
    public interface ITeamsBackgroundProcess
    {
        Task<bool> SeedTeamsInMemory();
        Task<bool> SeedCompetitionsInMemory();
    }
}
