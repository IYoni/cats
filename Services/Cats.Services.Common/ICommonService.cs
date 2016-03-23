﻿
      using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
      using Cats.Models;
      using Cats.Models.Constant;

namespace Cats.Services.Common
{
    public interface ICommonService : IDisposable
    {
        IEnumerable<CommodityType> GetCommodityTypes(
         Expression<Func<CommodityType, bool>> filter = null,
         Func<IQueryable<CommodityType>, IOrderedQueryable<CommodityType>> orderBy = null,
         string includeProperties = "");

        IEnumerable<Commodity> GetCommodities(
          Expression<Func<Commodity, bool>> filter = null,
          Func<IQueryable<Commodity>, IOrderedQueryable<Commodity>> orderBy = null,
          string includeProperties = "");
        IEnumerable<Detail> GetDetails(
          Expression<Func<Detail, bool>> filter = null,
          Func<IQueryable<Detail>, IOrderedQueryable<Detail>> orderBy = null,
          string includeProperties = "");
        IEnumerable<Donor> GetDonors(
            Expression<Func<Donor, bool>> filter = null,
            Func<IQueryable<Donor>, IOrderedQueryable<Donor>> orderBy = null,
            string includeProperties = "");
        IEnumerable<AdminUnit> GetAminUnits(
            Expression<Func<AdminUnit, bool>> filter = null,
            Func<IQueryable<AdminUnit>, IOrderedQueryable<AdminUnit>> orderBy = null,
            string includeProperties = "");
        IEnumerable<Ration> GetRations(
           Expression<Func<Ration, bool>> filter = null,
           Func<IQueryable<Ration>, IOrderedQueryable<Ration>> orderBy = null,
           string includeProperties = "");

        IEnumerable<Program> GetPrograms(
            Expression<Func<Program, bool>> filter = null,
            Func<IQueryable<Program>, IOrderedQueryable<Program>> orderBy = null,
            string includeProperties = "");
        string GetStatusName(WORKFLOW workflow, int statusId);
        List<WorkflowStatus> GetStatus(WORKFLOW workflow);

        IEnumerable<Season> GetSeasons(
            Expression<Func<Season, bool>> filter = null,
            Func<IQueryable<Season>, IOrderedQueryable<Season>> orderBy = null,
            string includeProperties = "");
       List<Plan> GetPlan(string programName);
        List<Plan> GetPlan(int programID);
        List<Plan> GetRequisitionGeneratedPlan(int programID, int zoneID);
        List<FDP> GetFDPs(int woredaID);
        List<Plan> GetPlans();
        List<Commodity> GetRationCommodity(int id);
        List<AdminUnit> FindBy(Expression<Func<AdminUnit, bool>> predicate);
        List<AdminUnit> GetRegions();
        List<AdminUnit> GetZones(int regionId);
        List<AdminUnit> GetWoreda(int zoneId);
        List<SupportType> GetAllSupportType();
        int GetZoneID(int woredaID);
        int GetRegion(int zoneID);
        List<CommoditySource> GetCommoditySource();
        int GetShippingInstruction(string siNumber);
        List<Hub> GetAllHubs();
        List<GiftCertificate> GetAllGiftCertificates();
        string GetCommditySourceName(int id);
        int GetWoredaBeneficiaryNo(int planId, int woredaId, int roundOrMonth, int program);

        List<HubStoreViewModel> GetHubsAndStores();
    }
}

          
      