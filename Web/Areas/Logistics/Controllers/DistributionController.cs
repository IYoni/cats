﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Areas.Logistics.Models;
using Cats.Helpers;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.Hubs;
using Cats.Services.Administration;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;
using Cats.Services.Hub;
using Cats.Services.Logistics;
using Cats.Services.Procurement;
using Cats.Services.Security;
using Cats.ViewModelBinder;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Cats.Helpers;
namespace Cats.Areas.Logistics.Controllers
{
    public class DistributionController : Controller
    {
        private ITransportOrderService _transportOrderService;
        private IWorkflowStatusService _workflowStatusService;

        private IDispatchAllocationService _dispatchAllocationService;
        private IDistributionService _distributionService;
        private IDispatchService _dispatchService;
        private IDistributionDetailService _distributionDetailService;
        private INotificationService _notificationService;
        private IActionTypesService _actionTypeService;

        public DistributionController(ITransportOrderService transportOrderService,
                                      IWorkflowStatusService workflowStatusService,
                                      IDispatchAllocationService dispatchAllocationService,
                                      IDistributionService distributionService,
            IDispatchService dispatchService,
            IDistributionDetailService distributionDetailService,
            INotificationService notificationService, IActionTypesService actionTypeService)
        {
            _transportOrderService = transportOrderService;
            _workflowStatusService = workflowStatusService;
            _dispatchAllocationService = dispatchAllocationService;
            _distributionService = distributionService;
            _dispatchService = dispatchService;
            _distributionDetailService = distributionDetailService;
            _notificationService = notificationService;
            _actionTypeService = actionTypeService;
        }
        //
        // GET: /Logistics/Distribution/

        public ActionResult Index()
        {
            ViewBag.TransportOrderId = 3073;
            return View();
        }
        public ActionResult Dispatches(int id)
        {
            //id--transportorderid
            var transportOrder = _transportOrderService.Get(t => t.TransportOrderID == id, null, "Transporter").FirstOrDefault();
            var statuses = _workflowStatusService.GetStatus(WORKFLOW.TRANSPORT_ORDER);
            var datePref = UserAccountHelper.UserCalendarPreference();
            ViewBag.TransportOrderId = id;
            var transportOrderViewModel = TransportOrderViewModelBinder.BindTransportOrderViewModel(transportOrder,
                                                                                                    datePref, statuses);

            var dispatch = _dispatchAllocationService.GetTransportOrderDispatches(id);

            foreach (var dispatchViewModel in dispatch)
            {
                var dispatchId = dispatchViewModel.DispatchID;
                var distribution = _distributionService.FindBy(t => t.DispatchID == dispatchId).FirstOrDefault();
                dispatchViewModel.GRNReceived = distribution != null;
                if (distribution != null)
                    dispatchViewModel.DistributionID = distribution.DistributionID;
            }
            var dispatchView = SetDatePreference(dispatch);
            var target = new TransportOrderDispatchViewModel { DispatchViewModels = dispatchView.Where(t => !t.GRNReceived).ToList(), DispatchViewModelsWithGRN = dispatchView.Where(t => t.GRNReceived).ToList(), TransportOrderViewModel = transportOrderViewModel };
            return View(target);
        }
        private List<DispatchViewModel> SetDatePreference(List<DispatchViewModel> dispatches)
        {
            foreach (var dispatchViewModel in dispatches)
            {
                dispatchViewModel.CreatedDatePref =
                    dispatchViewModel.CreatedDate.ToCTSPreferedDateFormat(UserAccountHelper.UserCalendarPreference());
                dispatchViewModel.DispatchDatePref =
                    dispatchViewModel.DispatchDate.ToCTSPreferedDateFormat(UserAccountHelper.UserCalendarPreference());
            }
            return dispatches;
        }

        public ActionResult ReceiveGRN(Guid dispatchId)
        {
            var dispatch = _dispatchService.Get(t => t.DispatchID == dispatchId, null,
                "FDP,FDP.AdminUnit,FDP.AdminUnit.AdminUnit2,FDP.AdminUnit.AdminUnit2.AdminUnit2,Transporter,Hub").FirstOrDefault();

            var distribution = CreateGoodsReceivingNote(dispatch);
            return View(distribution);
        }
        [HttpPost]
        public ActionResult ReceiveGRN(DistributionViewModel distributionViewModel)
        {
            if (ModelState.IsValid)
            {
                int transportOrderId = 0;

                var dispatch = _dispatchService.Get(t => t.DispatchID == distributionViewModel.DispatchID, null,
                    "DispatchDetails,DispatchAllocation").FirstOrDefault();


                var distribution = new Distribution();
                distribution.DeliveryBy = distributionViewModel.DeliveryBy;
                distribution.DeliveryDate = distributionViewModel.DeliveryDate;
                distribution.DispatchID = distributionViewModel.DispatchID;
                distribution.DistributionID = distributionViewModel.DistributionID;
                distribution.DocumentReceivedBy = distributionViewModel.DocumentReceivedBy;
                distribution.DocumentReceivedDate = distributionViewModel.DocumentReceivedDate;
                distribution.DonorID = distributionViewModel.DonorID;
                distribution.DriverName = distributionViewModel.DriverName;
                distribution.FDPID = distributionViewModel.FDPID;
                distribution.HubID = distributionViewModel.HubID;
                distribution.InvoiceNo = distributionViewModel.InvoiceNo;
                distribution.PlateNoPrimary = distributionViewModel.PlateNoPrimary;
                distribution.PlateNoTrailler = distributionViewModel.PlateNoTrailler;
                distribution.ReceivedBy = distributionViewModel.ReceivedBy;
                distribution.ReceivedDate = distributionViewModel.ReceivedDate;
                distribution.ReceivingNumber = distributionViewModel.ReceivingNumber;
                distribution.RequisitionNo = distributionViewModel.RequisitionNo;
                distribution.TransporterID = distributionViewModel.TransporterID;
                distribution.WayBillNo = distributionViewModel.WayBillNo;
                if (dispatch != null)
                {


                    foreach (var dispatchDetail in dispatch.DispatchDetails)
                    {
                        var distributionDetail = new DistributionDetail();
                        distributionDetail.DistributionID = distribution.DistributionID;
                        distributionDetail.DistributionDetailID = Guid.NewGuid();
                        distributionDetail.CommodityID = dispatchDetail.CommodityID;
                        distributionDetail.ReceivedQuantity = 0;
                        distributionDetail.SentQuantity = dispatchDetail.RequestedQuantityInMT;
                        distributionDetail.UnitID = dispatchDetail.UnitID;
                        distribution.DistributionDetails.Add(distributionDetail);

                    }


                    _distributionService.AddDistribution(distribution);

                    var dispatchAllocation = dispatch.DispatchAllocation;
                    if (dispatchAllocation != null)
                    {
                        transportOrderId = dispatchAllocation.TransportOrderID.HasValue ? dispatchAllocation.TransportOrderID.Value : 0;
                    }
                }
                return RedirectToAction("EditGRN", "Distribution", new { Area = "Logistics", id = distribution.DistributionID });
            }

            return View(distributionViewModel);
        }

        public ActionResult EditGRN(Guid id)
        {
            var distribution = _distributionService.Get(t => t.DistributionID == id, null,
                "FDP,FDP.AdminUnit,FDP.AdminUnit.AdminUnit2,FDP.AdminUnit.AdminUnit2.AdminUnit2,Hub").FirstOrDefault();

            var distributionViewModel = EditGoodsReceivingNote(distribution);
            return View(distributionViewModel);
        }
        public ActionResult ReadDeliveryNotes([DataSourceRequest]DataSourceRequest request, int id)
        {
            var dispatchIds =
                _dispatchService.Get(t => t.DispatchAllocation.TransportOrderID == id).Select(t => t.DispatchID).ToList();

            var distributions = _distributionService.Get(t => dispatchIds.Contains(t.DispatchID.Value), null, "DistributionDetails").ToList();

            var distributionViewModels = distributions.Select(EditGoodsReceivingNote);
            return Json(distributionViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadDeliveryNotesDiscripancy([DataSourceRequest]DataSourceRequest request, int id)
        {
            var dispatchIds =
                _dispatchService.Get(t => t.DispatchAllocation.TransporterID == id).Select(t => t.DispatchID).ToList();

            var distributions = _distributionService.Get(t => dispatchIds.Contains(t.DispatchID.Value), null, "DistributionDetails").ToList();

            var distributionViewModels = distributions.Select(EditGoodsReceivingNote).Select(t=>t.ContainsDiscripancy);
            return Json(distributionViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditGRN(DistributionViewModel distributionViewModel)
        {
            if (ModelState.IsValid)
            {
                int transportOrderId = 0;

                var distribution = _distributionService.Get(t => t.DistributionID == distributionViewModel.DistributionID, null,
                    "DistributionDetails,FDP,Hub").FirstOrDefault();

                var dispatch = _dispatchService.Get(t => t.DispatchID == distributionViewModel.DispatchID, null,
                   "DispatchAllocation").FirstOrDefault();

                distribution.DeliveryBy = distributionViewModel.DeliveryBy;
                distribution.DeliveryDate = distributionViewModel.DeliveryDate;


                distribution.DocumentReceivedBy = distributionViewModel.DocumentReceivedBy;
                distribution.DocumentReceivedDate = distributionViewModel.DocumentReceivedDate;

                distribution.DriverName = distributionViewModel.DriverName;

                distribution.InvoiceNo = distributionViewModel.InvoiceNo;
                distribution.PlateNoPrimary = distributionViewModel.PlateNoPrimary;
                distribution.PlateNoTrailler = distributionViewModel.PlateNoTrailler;
                distribution.ReceivedBy = distributionViewModel.ReceivedBy;
                distribution.ReceivedDate = distributionViewModel.ReceivedDate;
                distribution.ReceivingNumber = distributionViewModel.ReceivingNumber;
                distribution.RequisitionNo = distributionViewModel.RequisitionNo;
                distribution.TransporterID = distributionViewModel.TransporterID;
                distribution.WayBillNo = distributionViewModel.WayBillNo;
                _distributionService.EditDistribution(distribution);
                if (dispatch.DispatchAllocation.TransportOrderID.HasValue)
                    transportOrderId = dispatch.DispatchAllocation.TransportOrderID.Value;
                return RedirectToAction("Dispatches", "Distribution", new { Area = "Logistics", id = transportOrderId });
            }

            return View(distributionViewModel);
        }

        public ActionResult ReadDistributionDetail([DataSourceRequest]DataSourceRequest request, Guid distribtionId)
        {
            var distributionDetails =
                _distributionDetailService.Get(t => t.DistributionID == distribtionId, null, "Commodity,Unit").
                    ToList();

            var distributionDetailsViewModels = BindDistributionDetailViewModel(distributionDetails);

            return Json(distributionDetailsViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        private List<DistributionDetailViewModel> BindDistributionDetailViewModel(List<DistributionDetail> distributionDetails)
        {
            var distributionDetailViewModels = new List<DistributionDetailViewModel>();
            foreach (var distributionDetail in distributionDetails)
            {
                var distributionDetailViewModel = new DistributionDetailViewModel();
                distributionDetailViewModel.CommodityID = distributionDetail.CommodityID;
                distributionDetailViewModel.Commodity = distributionDetail.Commodity.Name;
                distributionDetailViewModel.DistributionDetailID = distributionDetail.DistributionDetailID;
                distributionDetailViewModel.DistributionID = distributionDetail.DistributionID;
                distributionDetailViewModel.ReceivedQuantity = distributionDetail.ReceivedQuantity;
                distributionDetailViewModel.SentQuantity = distributionDetail.SentQuantity;
                distributionDetailViewModel.UnitID = distributionDetail.UnitID;
                distributionDetailViewModel.Unit = distributionDetail.Unit.Name;
                distributionDetailViewModels.Add(distributionDetailViewModel);
            }
            return distributionDetailViewModels;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateDistributionDetail([DataSourceRequest] DataSourceRequest request, DistributionDetailViewModel distributionDetail)
        {
            if (distributionDetail != null && ModelState.IsValid)
            {
                var target = _distributionDetailService.FindById(distributionDetail.DistributionDetailID);
                if (target != null)
                {
                    target.ReceivedQuantity = distributionDetail.ReceivedQuantity;
                    _distributionDetailService.EditDistributionDetail(target);
                }
            }

            if (distributionDetail.ReceivedQuantity < distributionDetail.SentQuantity)
            {
                var distribution =
                    _distributionService.FindBy(t => t.DistributionID == distributionDetail.DistributionID).
                        FirstOrDefault();
                var id = distribution.DispatchID;
                var dispatch = _dispatchService.Get(t => t.DispatchID == id, null, "DispatchAllocation,DispatchAllocation.Transporter").FirstOrDefault();
                if (dispatch != null)
                {
                    var transportOrderId = dispatch.DispatchAllocation.TransportOrderID.HasValue
                                             ? dispatch.DispatchAllocation.TransportOrderID.Value
                                             : 0;

                    SendNotification(transportOrderId,
                        dispatch.DispatchAllocation.Transporter.Name);
                }

            }
            return Json(new[] { distributionDetail }.ToDataSourceResult(request, ModelState));
        }
        private void SendNotification(int transportOrderId, string transporterName)
        {
            try
            {
                string destinationURl;
                if (Request.Url.Host != null)
                {
                    if (Request.Url.Host == "localhost")
                    {
                        destinationURl = "http://" + Request.Url.Authority +
                                         "/Logistics/Distribution/Dispatches/" +
                                         transportOrderId;
                    }
                    else
                    {
                        destinationURl = "http://" + Request.Url.Authority +
                                        Request.ApplicationPath +
                                         "/Logistics/Distribution/Dispatches/" +
                                         transportOrderId;
                    }

                    _notificationService.AddNotificationForProcurmentForGRNDiscripancy(destinationURl, transportOrderId, transporterName);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult ReceivingNote(Guid distributionId)
        {
            return View();
        }
        private DistributionViewModel CreateGoodsReceivingNote(Dispatch dispatch)
        {
            if (dispatch == null) return new DistributionViewModel();
            var distribution = new DistributionViewModel();
            distribution.DeliveryDate = DateTime.Today;
            distribution.DispatchID = dispatch.DispatchID;
            distribution.DeliveryDate = DateTime.Today;
            distribution.DocumentReceivedBy = UserAccountHelper.GetUser(User.Identity.Name).UserProfileID;
            distribution.DocumentReceivedDate = DateTime.Today;
            distribution.DistributionID = Guid.NewGuid();
            //distribution.DonorID=dispatch.
            distribution.DriverName = dispatch.DriverName;
            distribution.FDPID = dispatch.FDPID.Value;
            distribution.HubID = dispatch.HubID;
            distribution.TransporterID = dispatch.TransporterID;
            distribution.InvoiceNo = dispatch.GIN;
            distribution.PlateNoPrimary = dispatch.PlateNo_Prime;
            distribution.PlateNoTrailler = dispatch.PlateNo_Trailer;
            distribution.RequisitionNo = dispatch.RequisitionNo;
            distribution.FDP = dispatch.FDP.Name;
            distribution.Region = dispatch.FDP.AdminUnit.AdminUnit2.AdminUnit2.Name;
            distribution.Zone = dispatch.FDP.AdminUnit.AdminUnit2.Name;
            distribution.Woreda = dispatch.FDP.AdminUnit.Name;
            distribution.Hub = dispatch.Hub.Name;
            distribution.Transporter = dispatch.Transporter.Name;


            //foreach (var dispatchDetail in dispatch.DispatchDetails)
            //{
            //    var distributionDetail = new DistributionDetail();
            //    distributionDetail.DistributionID = distribution.DistributionID;
            //    distributionDetail.DistributionDetailID = Guid.NewGuid();
            //    distributionDetail.CommodityID = dispatchDetail.CommodityID;
            //    distributionDetail.ReceivedQuantity = 0;
            //    distributionDetail.SentQuantity = dispatchDetail.RequestedQuantityInMT;
            //    distributionDetail.UnitID = dispatchDetail.UnitID;


            //}
            return distribution;
        }
        private DistributionViewModel EditGoodsReceivingNote(Distribution distribution)
        {

            if (distribution == null) return new DistributionViewModel();
            var dispatch = _dispatchService.Get(t => t.DispatchID == distribution.DispatchID, null,
               "FDP,FDP.AdminUnit,FDP.AdminUnit.AdminUnit2,FDP.AdminUnit.AdminUnit2.AdminUnit2,Transporter,Hub").FirstOrDefault();

            var distributionViewModel = new DistributionViewModel();

            distributionViewModel.DispatchID = distribution.DispatchID;
            distributionViewModel.DeliveryDate = distribution.DeliveryDate;
            distributionViewModel.DocumentReceivedBy = distribution.DocumentReceivedBy;
            distributionViewModel.DocumentReceivedDate = distribution.DocumentReceivedDate;
            distributionViewModel.DistributionID = distribution.DistributionID;
            //distribution.DonorID=dispatch.
            distributionViewModel.DriverName = distribution.DriverName;
            distributionViewModel.FDPID = distribution.FDPID;
            distributionViewModel.HubID = distribution.HubID;
            distributionViewModel.TransporterID = distribution.TransporterID;
            distributionViewModel.InvoiceNo = distribution.InvoiceNo;
            distributionViewModel.PlateNoPrimary = distribution.PlateNoPrimary;
            distributionViewModel.PlateNoTrailler = distribution.PlateNoTrailler;
            distributionViewModel.RequisitionNo = distribution.RequisitionNo;
            distributionViewModel.FDP = dispatch.FDP.Name;
            distributionViewModel.Region = dispatch.FDP.AdminUnit.AdminUnit2.AdminUnit2.Name;
            distributionViewModel.Zone = dispatch.FDP.AdminUnit.AdminUnit2.Name;
            distributionViewModel.Woreda = dispatch.FDP.AdminUnit.Name;
            distributionViewModel.Hub = dispatch.Hub.Name;
            distributionViewModel.DeliveryBy = distribution.DeliveryBy;
            distributionViewModel.ReceivedBy = distribution.ReceivedBy;
            distributionViewModel.ReceivedDate = distribution.ReceivedDate;
            distributionViewModel.ReceivingNumber = distribution.ReceivingNumber;
            distributionViewModel.InvoiceNo = distribution.InvoiceNo;
            distributionViewModel.WayBillNo = distribution.WayBillNo;
            distributionViewModel.RequisitionNo = distribution.RequisitionNo;
            distributionViewModel.Transporter = dispatch.Transporter.Name;
            distributionViewModel.Status = distribution.Status;
            distributionViewModel.ActionTypeRemark = distribution.ActionTypeRemark;
            var pref = UserAccountHelper.UserCalendarPreference();
            distributionViewModel.DeliveryDatePref = distribution.DeliveryDate.HasValue
                                                         ? distribution.DeliveryDate.Value.ToCTSPreferedDateFormat(pref)
                                                         : "";
            distributionViewModel.ReceivedDatePref = distribution.ReceivedDate.HasValue
                                                         ? distribution.ReceivedDate.Value.ToCTSPreferedDateFormat(pref)
                                                         : "";
            distributionViewModel.DocumentReceivedDatePref = distribution.DocumentReceivedDate.HasValue
                                                                 ? distribution.DocumentReceivedDate.Value.
                                                                       ToCTSPreferedDateFormat(pref)
                                                                 : "";

            distributionViewModel.ContainsDiscripancy =
                distribution.DistributionDetails.Any(t => t.ReceivedQuantity < t.SentQuantity);
            //foreach (var dispatchDetail in dispatch.DispatchDetails)
            //{
            //    var distributionDetail = new DistributionDetail();
            //    distributionDetail.DistributionID = distribution.DistributionID;
            //    distributionDetail.DistributionDetailID = Guid.NewGuid();
            //    distributionDetail.CommodityID = dispatchDetail.CommodityID;
            //    distributionDetail.ReceivedQuantity = 0;
            //    distributionDetail.SentQuantity = dispatchDetail.RequestedQuantityInMT;
            //    distributionDetail.UnitID = dispatchDetail.UnitID;


            //}
            return distributionViewModel;
        }

        public ActionResult DiscripancyAction(Guid id)
        {
            var distribution = _distributionService.Get(t => t.DistributionID == id, null,
                "FDP,FDP.AdminUnit,FDP.AdminUnit.AdminUnit2,FDP.AdminUnit.AdminUnit2.AdminUnit2,Hub").FirstOrDefault();
            ViewBag.ActionTypes = new SelectList(_actionTypeService.GetAllActionType(), "ActionId", "Name");
            var distributionViewModel = EditGoodsReceivingNote(distribution);
            return View(distributionViewModel);
        }

        public ActionResult SaveDiscripancy(DistributionViewModel _distributionViewModel, FormCollection collection)
        {

            var actionType = int.Parse(collection["Actiontype"].ToString(CultureInfo.InvariantCulture));
            var remark = collection["Remark"].ToString(CultureInfo.InvariantCulture);
            var TO = collection.Keys[2].ToString(CultureInfo.InvariantCulture);
            var distribution = _distributionService.Get(t => t.DistributionID == _distributionViewModel.DistributionID).Single();
            distribution.Status = (int)Cats.Models.Constant.DistributionStatus.Closed;
            distribution.ActionType = actionType;
            distribution.ActionTypeRemark = remark;
            _distributionService.EditDistribution(distribution);
            return RedirectToAction("Dispatches", new { id = TO });
        }

    }
}
