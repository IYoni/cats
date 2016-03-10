﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Cats.Helpers;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Services.Administration;
using Cats.Services.EarlyWarning;
using Cats.Services.Security;
using Cats.Services.Transaction;
using Cats.ViewModelBinder;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Cats.Helpers;
using IAdminUnitService = Cats.Services.EarlyWarning.IAdminUnitService;
using IHubService = Cats.Services.EarlyWarning.IHubService;


namespace Cats.Areas.Logistics.Controllers
{
    [Authorize]
    public class DispatchAllocationController : Controller
    {
        //
        // GET: /Logistics/DispatchAllocation/

        private readonly IReliefRequisitionService _reliefRequisitionService;
        private readonly IHubService _hubService;
        private readonly IHubAllocationService _hubAllocationService;
        private readonly IAdminUnitService _adminUnitService;
        private readonly INeedAssessmentService _needAssessmentService;
        private readonly IAllocationByRegionService _allocationByRegionService;
        private readonly IUserAccountService _userAccountService;
        private readonly ITransactionService _transactionService;
        //private readonly IStoreService _storeService;

        public DispatchAllocationController(IReliefRequisitionService reliefRequisitionService,
            IHubService hubService, IAdminUnitService adminUnitService,
            INeedAssessmentService needAssessmentService,
            IHubAllocationService hubAllocationService,
            IUserAccountService userAccountService,
            IAllocationByRegionService allocationByRegionService, ITransactionService transactionService)
        {
            _reliefRequisitionService = reliefRequisitionService;
            _hubService = hubService;
            _adminUnitService = adminUnitService;
            _needAssessmentService = needAssessmentService;
            _hubAllocationService = hubAllocationService;
            _userAccountService = userAccountService;
            _allocationByRegionService = allocationByRegionService;
            _transactionService = transactionService;
            
        }




        public ActionResult Index(int regionId = -1)
        {


            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            //hubContext.Clients.All.receiveNotification("this is a sample data");

            ViewBag.regionId = regionId;
            ViewBag.Region = new SelectList(_adminUnitService.GetRegions(), "AdminUnitID", "Name");
            return View();
        }
        public ActionResult AllocationAdjustment(int requisitionId)
        {
            var requisition = _reliefRequisitionService.FindById(requisitionId);
            var data = new List<int> { requisitionId, requisition.RegionID.Value };
            return View(data);
        }

        //#region "test"

        //public ActionResult Main()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public JsonResult HubAllocationByRegion(int regionId = -1)
        //{
        //    List<AllocationByRegion> requisititions = null;
        //    requisititions = regionId != -1 ? _AllocationByRegionService.FindBy(r => r.Status == (int)ReliefRequisitionStatus.HubAssigned && r.RegionID == regionId) : _AllocationByRegionService.FindBy(r => r.Status == (int)ReliefRequisitionStatus.HubAssigned);

        //    var requisitionViewModel = BindAllocation(requisititions);// HubAllocationViewModelBinder.ReturnRequisitionGroupByReuisitionNo(requisititions);

        //    return Json(requisitionViewModel,JsonRequestBehavior.AllowGet);
        //}


        //public JsonResult AllocatedProjectCode(int regionId = -1,int status=-1)
        //{
        //    if (regionId < 0 || status < 0) return Json(new List<RequisitionViewModel>(), JsonRequestBehavior.AllowGet);
        //    var requisititions = new List<ReliefRequisition>();
        //    requisititions = _reliefRequisitionService.FindBy(r => r.Status == status && r.RegionID == regionId);

        //    var requisitionViewModel = HubAllocationViewModelBinder.ReturnRequisitionGroupByReuisitionNo(requisititions);
        //    return Json(requisitionViewModel,JsonRequestBehavior.AllowGet);
        //}


        //#endregion

        public ActionResult GetRegions()
        {
            IOrderedEnumerable<RegionsViewModel> regions = _needAssessmentService.GetRegions();
            return Json(regions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HubAllocation([DataSourceRequest]DataSourceRequest request, int regionId)
        {
            List<AllocationByRegion> requisititions = null;
            requisititions = regionId != -1
                                 ? _allocationByRegionService.FindBy(
                                     r =>
                                     r.Status == (int)ReliefRequisitionStatus.HubAssigned && r.RegionID == regionId)
                                 : null;// _AllocationByRegionService.FindBy(r => r.Status == (int)ReliefRequisitionStatus.HubAssigned);

            var requisitionViewModel = BindAllocation(requisititions);// HubAllocationViewModelBinder.ReturnRequisitionGroupByReuisitionNo(requisititions);

            return Json(requisitionViewModel.ToDataSourceResult(request));
        }

        public ActionResult AllocateProjectCode([DataSourceRequest]DataSourceRequest request, int regionId, int status)
        {
            ViewBag.requestStatus = status;
            List<ReliefRequisition> requisititions = null;
            if (regionId == -1 || status == -1) return Json((new List<RequisitionViewModel>()).ToDataSourceResult(request));
            if (status==(int)ReliefRequisitionStatus.ProjectCodeAssigned)
            {
                requisititions = _reliefRequisitionService.FindBy(r => r.Status == status || r.Status == (int)ReliefRequisitionStatus.SiPcAllocationApproved && r.RegionID == regionId).OrderByDescending(t => t.RequisitionID).ToList();
            }
            else
            {
                requisititions = _reliefRequisitionService.FindBy(
                r =>
                r.Status == status && r.RegionID == regionId).OrderByDescending(t => t.RequisitionID).ToList();
            }
            
            var requisitionViewModel = HubAllocationViewModelBinder.ReturnRequisitionGroupByReuisitionNo(requisititions);
            return Json(requisitionViewModel.ToDataSourceResult(request));
        }


        public ActionResult IndexFromNotification(int paramRegionId, int recordId)
        {
            ViewBag.regionId = paramRegionId;
            NotificationHelper.MakeNotificationRead(recordId);
            return RedirectToAction("Hub", new { regionId = paramRegionId });

        }

        public ActionResult AssignHubFromLogisticsDashboard(int paramRegionId)
        {
            ViewBag.regionId = paramRegionId;
            return RedirectToAction("Hub", new { regionId = paramRegionId });

        }
        public ActionResult Hub(int regionId)
        {
            if (regionId != -1)
            {
                ViewBag.regionId = regionId;
                ViewBag.RegionName = _adminUnitService.GetRegions().Where(r => r.AdminUnitID == regionId).Select(r => r.Name).Single();
                ViewData["Hubs"] = _hubService.FindBy(h => h.HubOwnerID == 1);
               // ViewData["Stores"] = _storeService.FindBy(s => s.Hub.HubOwnerID == 1); //get DRMFSS stores
                return View();
            }
            return View();
        }



        [HttpGet]
        public JsonResult ReadRequisitions(int regionId)
        {
            var requisititions = _reliefRequisitionService.FindBy(r => r.Status == (int)ReliefRequisitionStatus.Approved && r.RegionID == regionId);
            var requisitionViewModel = HubAllocationViewModelBinder.ReturnRequisitionGroupByReuisitionNo(requisititions);
            return Json(requisitionViewModel, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Http.HttpPost]
        public JsonResult Save(List<Allocation> allocation)
        {
            var userName = HttpContext.User.Identity.Name;
            var user = _userAccountService.GetUserDetail(userName);

            try
            {
                foreach (var all in allocation)
                {
                    
                    var hubAllocated = _hubAllocationService.FindBy(h => h.RequisitionID == all.ReqId).FirstOrDefault();
                    if(hubAllocated!=null)
                    {
                        

                        hubAllocated.AllocatedBy = user.UserProfileID;
                        hubAllocated.AllocationDate = DateTime.Now.Date;
                        hubAllocated.HubID = all.HubId;


                        _hubAllocationService.EditHubAllocation(hubAllocated);
                    }
                    else
                    {
                        _hubAllocationService.AddHubAllocations(allocation, user.UserProfileID);
                    }
               

                }
               
                ModelState.AddModelError("Success", @"Allocation is Saved.");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }

        }


        public ActionResult RegionId(int id)
        {
            return RedirectToAction("Index", new { regionId = id });
        }



        public List<HubAllocationByRegionViewModel> BindAllocation(List<AllocationByRegion> reliefRequisitions)
        {

            try
            {
                if (reliefRequisitions == null)
                    return new List<HubAllocationByRegionViewModel>();

                //var result = (reliefRequisitions.Select(req => new HubAllocationByRegionViewModel()
                //{
                //    Region = req.Name,
                //    RegionId = (int)req.RegionID,
                //    AdminUnitID = (int)req.RegionID,
                //    Hub = req.Hub,
                //    AllocatedAmount = ((decimal)req.Amount).ToPreferedWeightUnit()
                //}));

                var r = new List<HubAllocationByRegionViewModel>();

                foreach (var allocationByRegion in reliefRequisitions)
                {
                    var allc = new HubAllocationByRegionViewModel();

                    allc.Region = allocationByRegion.Name;
                    allc.RegionId = (int)allocationByRegion.RegionID;
                    allc.AdminUnitID = (int)allocationByRegion.RegionID;
                    allc.Hub = allocationByRegion.Hub;
                    allc.AllocatedAmount = ((decimal)allocationByRegion.Amount).ToPreferedWeightUnit();

                    r.Add(allc);
                }

                return Enumerable.Cast<HubAllocationByRegionViewModel>(r).ToList();
            }
            catch
            {

                return new List<HubAllocationByRegionViewModel>();
            }


        }
        
        public ActionResult RejectRequsition(int id)
        {
            var requistion = _reliefRequisitionService.FindById(id);
            if (requistion != null)
            {
                requistion.Status = (int)ReliefRequisitionStatus.Rejected;
                _reliefRequisitionService.EditReliefRequisition(requistion);

                return RedirectToAction("Index", new { regionId = requistion.RegionID });
            }
            return RedirectToAction("Index");
        }

        public ActionResult UncommitRequsition(int id)
        {
            var requistion = _reliefRequisitionService.FindById(id);
            if (requistion != null)
            {


                _transactionService.PostSIAllocationUncommit(id);
                return RedirectToAction("Index", new { regionId = requistion.RegionID });
            }
            return RedirectToAction("Index");
        }
        public ActionResult ApproveSiPcAllocation(int id)
        {
             var requistion = _reliefRequisitionService.FindById(id);
             if (requistion != null)
             {
                 requistion.Status = (int)ReliefRequisitionStatus.SiPcAllocationApproved;
                 _reliefRequisitionService.EditReliefRequisition(requistion);
             }
             return RedirectToAction("Index");
        }

    }
}
