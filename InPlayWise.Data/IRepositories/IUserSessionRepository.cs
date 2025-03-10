﻿using InPlayWise.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Data.IRepositories
{
    public interface IUserSessionRepository
    {
        Task<bool> UpdateUserSessionRepo(UserSession userSession);
    }
}
