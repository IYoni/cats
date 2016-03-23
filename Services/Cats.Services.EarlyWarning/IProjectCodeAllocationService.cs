﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface IProjectCodeAllocationService
    {
        bool AddProjectCodeAllocationDetail(ProjectCodeAllocation _ProjectCodeAllocationDetail);
        bool AddProjectCodeAllocation(ProjectCodeAllocation _ProjectCodeAllocationDetail, int requisitionId, bool IsLastAssignment);

        bool EditProjectCodeAllocationDetail(ProjectCodeAllocation _ProjectCodeAllocationDetail);

        bool DeleteProjectCodeAllocationDetail(ProjectCodeAllocation _ProjectCodeAllocationDetail);
        
        bool DeleteById(int id);
        
        bool Save();

        bool Save(ProjectCodeAllocation _ProjectAllocation);

        IEnumerable<ProjectCodeAllocation> GetAllProjectCodeAllocationDetail();

        ProjectCodeAllocation FindById(int id);

        IEnumerable<ProjectCodeAllocation> FindBy(Expression<Func<ProjectCodeAllocation, bool>> predicate);

       // bool SaveProjectCodeAllocation(IEnumerable<ProjectCodeAllocation> projectAllocations);
        List<HubAllocation> GetHubAllocation(Expression<Func<HubAllocation, bool>> predicate);
        List<HubAllocation> GetAllRequisitionsInHubAllocation();
        IEnumerable<HubAllocation> Get(
           Expression<Func<HubAllocation, bool>> filter = null,
           Func<IQueryable<HubAllocation>, IOrderedQueryable<HubAllocation>> orderBy = null,
           string includeProperties = "");

        List<ProjectCodeAllocation> GetHubAllocationByHubID(int status);
        List<ProjectCodeAllocation> GetHubAllocationByID(int hubID);
        int GetAllocatedAmountBySI(int hubId, int siIndex);
        int GetAllocatedAmountByPC(int hubId, int pcIndex);
    }
}
