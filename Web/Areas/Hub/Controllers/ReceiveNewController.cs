﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cats.Areas.Hub.Models;
using Cats.Models.Hubs;
using Cats.Models.Hubs.ViewModels;
using Cats.Services.Hub;
using Cats.Web.Hub;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Telerik.Web.Mvc;


namespace Cats.Areas.Hub.Controllers
{
    public class ReceiveNewController : BaseController
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IReceiptAllocationService _receiptAllocationService;
        private readonly IReceiveService _receiveService;
        private readonly IReceiveDetailService _receiveDetailService;
        private readonly ICommodityService _commodityService;
        private readonly IUnitService _unitService;
        private readonly IStoreService _storeService;
        private readonly ITransactionService _transactionService;
        private readonly IDonorService _donorService;
        private readonly IHubService _hub;
        private readonly ITransporterService _transporterService;
        private readonly IShippingInstructionService _shippingInstructionService;
        private Guid _receiptAllocationId;

        public ReceiveNewController(IUserProfileService userProfileService,
            IReceiptAllocationService receiptAllocationService,
            IReceiveService receiveService,
            ICommodityService commodityService,
            IUnitService unitService,
            IStoreService storeService,
            ITransactionService transactionService,
            IDonorService donorService,
            IHubService hub,
            ITransporterService transporterService, IShippingInstructionService shippingInstructionService, IReceiveDetailService receiveDetailService)
            : base(userProfileService)
        {
            _userProfileService = userProfileService;
            _receiptAllocationService = receiptAllocationService;
            _receiveService = receiveService;
            _commodityService = commodityService;
            _unitService = unitService;
            _storeService = storeService;
            _transactionService = transactionService;
            _donorService = donorService;
            _hub = hub;
            _transporterService = transporterService;
            _shippingInstructionService = shippingInstructionService;
            _receiveDetailService = receiveDetailService;
        }

        #region Action

        public ReceiveNewViewModel ModeltoNewView(Receive receive) //string receiveId, string grn)
        {


            
            
            var receiptAllocation = _receiptAllocationService.FindById(receive.ReceiptAllocationID.GetValueOrDefault());

            var user = _userProfileService.GetUser(User.Identity.Name);

            var viewModel = _receiveService.ReceiptAllocationToReceive(receiptAllocation);
            viewModel.CurrentHub = user.DefaultHub.Value;
            viewModel.UserProfileId = user.UserProfileID;
            var hubOwner = _hub.FindById(user.DefaultHub.Value);
            viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
            viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(receive.ReceiptAllocationID.GetValueOrDefault());
            //var commodities = _commodityService.GetAllCommodityViewModelsByParent(receiptAllocation.CommodityID);
            //ViewData["commodities"] = commodities;
            //ViewData["units"] = _unitService.GetAllUnitViewModels();

            viewModel.CreatedDate = receive.CreatedDate;
            viewModel.Grn = receive.GRN;
            viewModel.ReceiptDate = receive.ReceiptDate;
                                                      viewModel.SiNumber=receiptAllocation.SINumber;
                                                           viewModel.ReceiptDate = viewModel.ReceiptDate;
            viewModel.ReceiptAllocationId = receive.ReceiptAllocationID.GetValueOrDefault();
            viewModel.ReceiveId = receive.ReceiveID;

                //viewModel.StackNumber
            viewModel.WayBillNo = receive.WayBillNo;
            viewModel.SiNumber = receiptAllocation.SINumber;
            viewModel.ProjectCode = receiptAllocation.ProjectNumber;
            //viewModel.Program = .FindById(receiptAllocation.ProgramID).Name;
            viewModel.ProgramId = receiptAllocation.ProgramID;
                //viewModel.CommodityType = _CommodityTypeRepository.FindById(receiptAllocation.Commodity.CommodityTypeID).Name,
            viewModel.CommodityTypeId = receiptAllocation.Commodity.CommodityTypeID;
            viewModel.CommoditySourceTypeId = receiptAllocation.CommoditySourceID;

           
            if (CommoditySource.Constants.LOAN == receiptAllocation.CommoditySourceID
                || CommoditySource.Constants.SWAP == receiptAllocation.CommoditySourceID
                || CommoditySource.Constants.TRANSFER == receiptAllocation.CommoditySourceID
                || CommoditySource.Constants.REPAYMENT == receiptAllocation.CommoditySourceID)
            {
                if (receiptAllocation.SourceHubID.HasValue)
                {
                    
                    viewModel.SourceHub = _hub.FindById(receiptAllocation.SourceHubID.GetValueOrDefault(0)).Name; 
                }
            }

            if (CommoditySource.Constants.LOCALPURCHASE == receiptAllocation.CommoditySourceID)
            {
                viewModel.SupplierName = receiptAllocation.SupplierName;
                viewModel.PurchaseOrder = receiptAllocation.PurchaseOrder;
            }

            viewModel.CommoditySource = receiptAllocation.CommoditySource.Name;
            viewModel.CommoditySourceTypeId = receiptAllocation.CommoditySourceID;
            viewModel.ReceivedByStoreMan = receive.ReceivedByStoreMan;
            ReceiveDetail receivedetail = receive.ReceiveDetails.FirstOrDefault();
            viewModel.StoreId = receive.StoreId.GetValueOrDefault();
            viewModel.StackNumber = receive.StackNumber.GetValueOrDefault();
            viewModel.ReceiveDetailNewViewModel = new ReceiveDetailNewViewModel
                                                      {
                                                          CommodityId = receivedetail.CommodityID,
                                                          CommodityChildID=receivedetail.CommodityChildID,
                                                          ReceivedQuantityInMt = 
                                                              receivedetail.QuantityInMT,
                                                          ReceivedQuantityInUnit =
                                                              receivedetail.QuantityInUnit,
                                                              SentQuantityInMt=receivedetail.SentQuantityInMT,
                                                              SentQuantityInUnit=receivedetail.SentQuantityInUnit,
                                                              UnitId=receivedetail.UnitID,
                                                              Description=receivedetail.Description,
                                                              ReceiveId=receivedetail.ReceiveID,
            ReceiveDetailId=receivedetail.ReceiveDetailID,
            
                                                      };
            
            viewModel.WeightBridgeTicketNumber = receive.WeightBridgeTicketNumber;
            viewModel.WeightBeforeUnloading = receive.WeightBeforeUnloading;
            viewModel.WeightAfterUnloading = receive.WeightAfterUnloading;
            viewModel.TransporterId = receive.TransporterID;
            viewModel.DriverName = receive.DriverName;
            viewModel.PlateNoPrime = receive.PlateNo_Prime;
            viewModel.PlateNoTrailer = receive.PlateNo_Trailer;
            viewModel.PortName = receive.PortName;
            viewModel.Remark = receive.Remark;
                
            return viewModel;
        
        }


        public ActionResult Create(string receiptAllocationId,string grn)
        {
            ViewBag.isEditMode = false;
            if (grn != null)
            {

                ViewBag.isEditMode = true;
                return View(ModeltoNewView(_receiveService.FindById(Guid.Parse(receiptAllocationId))));
            }
            
            if (String.IsNullOrEmpty(receiptAllocationId)) return View();
            _receiptAllocationId = Guid.Parse(receiptAllocationId);

            var receiptAllocation = _receiptAllocationService.FindById(_receiptAllocationId);

            var user = _userProfileService.GetUser(User.Identity.Name);

            if (receiptAllocation == null ||
                (user.DefaultHub == null || receiptAllocation.HubID != user.DefaultHub.Value)) return View();

            

            var viewModel = _receiveService.ReceiptAllocationToReceive(receiptAllocation);
            viewModel.CurrentHub = user.DefaultHub.Value;
            viewModel.UserProfileId = user.UserProfileID;
            var hubOwner = _hub.FindById(user.DefaultHub.Value);
            viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
            viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
            //var commodities = _commodityService.GetAllCommodityViewModelsByParent(receiptAllocation.CommodityID);
            //ViewData["commodities"] = commodities;
            //ViewData["units"] = _unitService.GetAllUnitViewModels();
            //viewModel.ReceiveDetailNewViewModel.CommodityId = receiptAllocation.CommodityID;
            
            //since the commodity that comes from allocation is the child look for the parent for saving later.
            var parentCommodityId =
                _commodityService.FindById(receiptAllocation.CommodityID).ParentID ??
                receiptAllocation.CommodityID;

            viewModel.ReceiveDetailNewViewModel = new ReceiveDetailNewViewModel
                                                      {
                                                          CommodityId=parentCommodityId,
                                                          CommodityChildID=receiptAllocation.CommodityID,
                                                          //UnitId=receiptAllocation.UnitID.GetValueOrDefault(),
                                                      };
            ViewBag.Commodities = _commodityService.GetAllCommodity().Where(l => l.ParentID == null).Where(l => l.CommodityTypeID == 1).Select(c => new CommodityModel() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewBag.SubCommodities = _commodityService.GetAllSubCommodities().Where(l => l.ParentID != null).Where(l => l.CommodityTypeID == 1).Select(c => new SubCommodity() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewBag.Units = _unitService.GetAllUnit().Select(u => new UnitModel() { Id = u.UnitID, Name = u.Name }).ToList();
           
            return View(viewModel);
        }

        public ActionResult Commodities(Guid? receiptAllocationId, string grn, Guid? receiveId)
        {
            ViewBag.receiveId = receiveId;
            ViewBag.Commodities = _commodityService.GetAllCommodity().Where(l => l.ParentID == null).Where(l => l.CommodityTypeID == 1).Select(c => new CommodityModel() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewBag.SubCommodities = _commodityService.GetAllSubCommodities().Where(l => l.ParentID != null).Where(l => l.CommodityTypeID == 1).Select(c => new SubCommodity() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewBag.Units = _unitService.GetAllUnit().Select(u => new UnitModel() { Id = u.UnitID, Name = u.Name }).ToList();
            ViewBag.SI = _shippingInstructionService.GetAllShippingInstruction().Select(s => new Cats.Models.Hubs.ViewModels.ShippingInstructionModel() { Id = s.ShippingInstructionID, Value = s.Value }).ToList();
            
            return View("Commodities");
        }

        public ActionResult ReadCommoditiesFromReceive([DataSourceRequest] DataSourceRequest request, Guid? receiveId)
        {
            ViewBag.Commodities = _commodityService.GetAllCommodity().Where(l => l.ParentID == null).Where(l => l.CommodityTypeID == 1).Select(c => new CommodityModel() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewBag.SubCommodities = _commodityService.GetAllSubCommodities().Where(l => l.ParentID != null).Where(l => l.CommodityTypeID == 1).Select(c => new SubCommodity() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewBag.Units = _unitService.GetAllUnit().Select(u => new UnitModel() { Id = u.UnitID, Name = u.Name }).ToList();
            ViewBag.SI = _shippingInstructionService.GetAllShippingInstruction().Select(s => new Cats.Models.Hubs.ViewModels.ShippingInstructionModel() { Id = s.ShippingInstructionID, Value = s.Value }).ToList();
            
            if (receiveId != null)
            {
                var receiveDetailsViewModels =
                    (from receives in _receiveService.FindById((Guid)receiveId).ReceiveDetails
                     let transaction =
                         receives.TransactionGroup.Transactions.FirstOrDefault(
                             p => p.QuantityInMT > 0 || p.QuantityInUnit > 0)
                     where transaction != null
                     let amount = transaction.QuantityInMT
                     select new ReceiveDetailsViewModel()
                     {

                         CommodityId = receives.CommodityID,
                         Description = receives.Description,
                         SentQuantityInMt = receives.SentQuantityInMT,
                         SentQuantityInUnit = receives.SentQuantityInUnit,
                         ReceivedQuantityInMt = transaction.QuantityInMT,
                         ReceivedQuantityInUnit = transaction.QuantityInUnit,
                         SiNumber = transaction.ShippingInstructionID,
                         CommodityChildID = receives.CommodityChildID ?? 0,
                         UnitId = receives.UnitID,
                         ReceiveDetailsId = receives.ReceiveDetailID,
                         ReceiveDetailsIdString = receives.ReceiveDetailID.ToString()
                     }).ToList();

                return Json(receiveDetailsViewModels.ToDataSourceResult(request));
            }
            else
            {
                return Json(new List<ReceiveDetailsViewModel>().ToDataSourceResult(request));
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateCommoditiesFromReceive([DataSourceRequest] DataSourceRequest request, ReceiveDetailsViewModel receiveDetailsViewModels, Guid receiveId)
        {
            var results = new List<ReceiveDetailsViewModel>();
            if (ModelState.IsValid)
            {
                var receiveModel = ModeltoNewView(_receiveService.FindById(receiveId));
                ReceiveDetailsViewModel rdvm;
                if (!receiveDetailsViewModels.ReceiveDetailsId.HasValue)
                {
                    rdvm = new ReceiveDetailsViewModel();
                    rdvm = receiveDetailsViewModels;
                    
                    rdvm.ReceiveDetailsId = Guid.NewGuid();
                    receiveModel.ReceiveDetailsViewModels = new List<ReceiveDetailsViewModel> {rdvm};
                    _transactionService.ReceiptDetailsTransaction(receiveModel);

                    results.Add(receiveDetailsViewModels);
                }
                else 
                {
                    rdvm = new ReceiveDetailsViewModel();
                    rdvm = receiveDetailsViewModels;
                    
                    receiveModel.ReceiveDetailsViewModels = new List<ReceiveDetailsViewModel> { rdvm };
                    _transactionService.ReceiptDetailsTransaction(receiveModel, false, true);

                    results.Add(receiveDetailsViewModels);
                }
                
            }
            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Create(ReceiveNewViewModel viewModel)
        {
            //Todo: change to support multiple receive detail 
            ViewBag.isEditMode = false;
            if (!string.IsNullOrEmpty(viewModel.Grn))
            {
                ViewBag.isEditMode = true;
            }
            var user = _userProfileService.GetUser(User.Identity.Name);
            var hubOwner = _hub.FindById(user.DefaultHub.Value);
            
            if (viewModel.ReceiveId != Guid.Empty)
            {
                _receiptAllocationId =
                    _receiveService.FindById(viewModel.ReceiveId).ReceiptAllocationID.GetValueOrDefault();
                viewModel.ReceiptAllocationId = _receiptAllocationId;
            }
            else
            {

                _receiptAllocationId = viewModel.ReceiptAllocationId;
            }

            #region Fix to ModelState

            switch (viewModel.CommoditySourceTypeId)
            {
                case CommoditySource.Constants.DONATION:
                    ModelState.Remove("SourceHub");
                    ModelState.Remove("SupplierName");
                    ModelState.Remove("PurchaseOrder");
                    break;
                case CommoditySource.Constants.LOCALPURCHASE:
                    ModelState.Remove("SourceHub");
                    break;
                default:
                    ModelState.Remove("DonorID");
                    ModelState.Remove("ResponsibleDonorID");
                    ModelState.Remove("SupplierName");
                    ModelState.Remove("PurchaseOrder");
                    break;
            }

            #endregion

            if (!ModelState.IsValid)
            {
                viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
                viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
                return View(viewModel);
            }


            //check if the detail are not null 
            if (viewModel.ReceiveDetailNewViewModel != null)
            {
                #region GRN validation

                if (!_receiveService.IsGrnUnique(viewModel.Grn))
                {

                    if (viewModel.ReceiveId == Guid.Empty || _receiveService.FindById(viewModel.ReceiveId).GRN!=viewModel.Grn)
                 
                    {
                        ModelState.AddModelError("GRN", @"GRN already existed");
                        viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
                        viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
                        return View(viewModel);
                    }
                }

                #endregion

                #region Validate receive amount

                //if (_receiveService.IsReceiveExcedeAllocation(viewModel.ReceiveDetailNewViewModel,
                //    viewModel.ReceiptAllocationId))
                //{
                //    viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
                //    ModelState.AddModelError("ReceiveId", "you are trying to receive more than allocated");
                //    viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
                //    return View(viewModel);
                //}

                #endregion

                #region Validate Receive Amount not excide Sent one 

                if (_receiveService.IsReceiveGreaterThanSent(viewModel.ReceiveDetailNewViewModel))
                {
                    viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
                    ModelState.AddModelError("ReceiveId", "You can't receive more than sent item");
                    viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
                    return View(viewModel);
                }

                #endregion


                //check if it is loan and not a false GRN
                //if (viewModel.CommoditySourceTypeId == CommoditySource.Constants.LOAN && !viewModel.IsFalseGRN && viewModel.SelectedGRN !=null)// this means it is the orginal GRN
                //{
                //    _transactionService.ReceiptTransactionForLoanFromNGOs(viewModel);
                //    return RedirectToAction("Index", "Receive");
                //}

                //Save transaction 
                if (viewModel.ReceiveId != Guid.Empty)
                {
                    //reverse the transaction
                    Receive prevmodel = _receiveService.FindById((viewModel.ReceiveId));

                    _transactionService.ReceiptTransaction(ModeltoNewView(prevmodel), true);

                }
                _transactionService.ReceiptTransaction(viewModel);
                var receiveID =
                    _receiveService.GetAllReceive()
                        .Where(r => r.ReceiptAllocationID == viewModel.ReceiptAllocationId)
                        .Select(r => r.ReceiveID)
                        .LastOrDefault();
                return RedirectToAction("Commodities", "ReceiveNew", new { @receiptAllocationId = viewModel.ReceiptAllocationId, @grn = viewModel.Grn, @receiveId = receiveID });
                //return RedirectToAction("Index", "Receive");
            }
            viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
            viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
            ModelState.AddModelError("ReceiveDetails", "Please add at least one commodity");
            viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
            viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
            ViewBag.Commodities = _commodityService.GetAllCommodity().Where(l => l.ParentID == null).Where(l => l.CommodityTypeID == 1).Select(c => new CommodityModel() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewBag.SubCommodities = _commodityService.GetAllSubCommodities().Where(l => l.ParentID != null).Where(l => l.CommodityTypeID == 1).Select(c => new SubCommodity() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewBag.Units = _unitService.GetAllUnit().Select(u => new UnitModel() { Id = u.UnitID, Name = u.Name }).ToList();
           
            return View(viewModel);
        }

        public JsonResult AllocationStatus(string receiptAllocationId)
        {
            _receiptAllocationId = Guid.Parse(receiptAllocationId);
            return Json(_receiveService.GetAllocationStatus(_receiptAllocationId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReceiveDetails_Create([DataSourceRequest] DataSourceRequest request, ReceiveDetailsViewModel receiveDetailsViewModel, Guid receiveId)
        {
            if (receiveDetailsViewModel != null && ModelState.IsValid)
            {
                //SessionProductRepository.Insert(receiveDetailsViewModel);

            }

            return Json(new[] { receiveDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Combobox

        public JsonResult GetUnities()
        {
            return Json(_unitService.GetAllUnitViewModels(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCommodities(string receiptAllocationId)
        {
            _receiptAllocationId = Guid.Parse(receiptAllocationId);

            var receiptAllocation = _receiptAllocationService.FindById(_receiptAllocationId);
            
            return Json(_commodityService.GetAllCommodityViewModelsByParent(receiptAllocation.CommodityID),
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStroes(int hubId)
        {
            return Json((from c in _storeService.GetStoreByHub(hubId)
                         select new StoreViewModel
                         {
                             StoreId = c.StoreID,
                             Name = c.Name
                         }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGRNList(string siNo)
        {
            var GRNs = new List<GRNViewModel>();
            GRNs.AddRange(_receiveService.FindBy(f => f.ReceiptAllocation.IsFalseGRN && f.ReceiptAllocation.SINumber == siNo).Select(s => new GRNViewModel
                                                                                                    {
                                                                                                        Name = s.GRN,
                                                                                                        Id=s.ReceiveID
                                                                                                    }));


            return Json(GRNs, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetStacks(int? storeId)
        {
            if (storeId == null)
                return Json(new SelectList(Enumerable.Empty<StackViewModel>()), JsonRequestBehavior.AllowGet);
            var store = _storeService.FindById(storeId.Value);
            var stacks = new List<StackViewModel>();
            stacks.AddRange(store.Stacks.Select(i => new StackViewModel { Name = i.ToString(), Id = i }));
            return Json(stacks, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetResponsibleDonor()
        {
            return Json(from c in _donorService.GetAllDonor()
                            .Where(p => p.IsResponsibleDonor == true)
                            .DefaultIfEmpty()
                            .OrderBy(p => p.Name)
                        select new DonorViewModel
                        {
                            DonorId = c.DonorID,
                            Name = c.Name
                        }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSourceDonor()
        {
            return Json(from c in _donorService.GetAllDonor()
                            .Where(p => p.IsSourceDonor == true)
                            .DefaultIfEmpty()
                            .OrderBy(p => p.Name)
                        select new DonorViewModel
                        {
                            DonorId = c.DonorID,
                            Name = c.Name
                        }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTransporter()
        {
            return Json(from c in _transporterService.GetAllTransporter().DefaultIfEmpty().OrderBy(o => o.Name)
                        select new TransporterViewModel
                        {
                            TransporterId = c.TransporterID,
                            Name = c.Name
                        }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AutoCompleteCommodity(string term)
        {
            var result = (from commodity in _commodityService.GetAllCommodity()
                          where commodity.Name.ToLower().StartsWith(term.ToLower())
                          select commodity.Name);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Private Method

        private void PopulateCombox(int parentCommodity)
        {
            var commodities = _commodityService.GetAllCommodityViewModelsByParent(parentCommodity);
            ViewData["commodities"] = commodities;
            ViewData["units"] = _unitService.GetAllUnitViewModels();
        }

        #endregion

    }
}
