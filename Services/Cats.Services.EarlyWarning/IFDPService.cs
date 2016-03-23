﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface IFDPService
    {

        bool AddFDP(FDP fdp);
        bool DeleteFDP(FDP fdp);
        bool DeleteById(int id);
        bool EditFDP(FDP fdp);
        FDP FindById(int id);
        List<FDP> GetAllFDP();
        List<FDP> FindBy(Expression<Func<FDP, bool>> predicate);

        IEnumerable<FDP> Get(Expression<Func<FDP, bool>> filter = null,
                             Func<IQueryable<FDP>, IOrderedQueryable<FDP>> orderBy = null,
                             string includeProperties = "");

    }
}


