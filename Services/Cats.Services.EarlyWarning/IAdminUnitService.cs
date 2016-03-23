﻿
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface IAdminUnitService
    {
        bool AddAdminUnit(AdminUnit adminUnit);
        bool DeleteAdminUnit(AdminUnit adminUnit);
        bool DeleteById(int id);
        bool EditAdminUnit(AdminUnit adminUnit);
        AdminUnit FindById(int id);
        List<AdminUnit> GetAllAdminUnit();
        List<AdminUnit> FindBy(Expression<Func<AdminUnit, bool>> predicate);
        List<AdminUnit> GetRegions();
        List<AdminUnit> GetZones(int regionId);
        List<AdminUnit> GetWoreda(int zoneId);
        List<AdminUnit> GetAllZones(int regionID);
        //JsonResult GetAdminUnits();
    }
}


