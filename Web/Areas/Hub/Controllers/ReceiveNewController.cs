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
        private readonly ICommodityService _commodityService;
        private readonly IUnitService _unitService;
        private readonly IStoreService _storeService;
        private readonly ITransactionService _transactionService;
        private readonly IDonorService _donorService;
        private readonly IHubService _hub;
        private readonly ITransporterService _transporterService;
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
            ITransporterService transporterService)
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
            if (grn != null)
            {

                
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
            ViewBag.Commodities = _commodityService.GetAllSubCommodities().Select(c => new CommodityModel() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewBag.Units = _unitService.GetAllUnit().Select(u => new UnitModel() { Id = u.UnitID, Name = u.Name }).ToList();
           
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Create(ReceiveNewViewModel viewModel)
        {
            //Todo: change to support multiple receive detail 

            var user = _userProfileService.GetUser(User.Identity.Name);
            var hubOwner = _hub.FindById(user.DefaultHub.Value);
            
            //when the combobox is disabled the commodity id is not submitted
            //var receiptAllocation = _receiptAllocationService.FindById(viewModel.ReceiptAllocationId);
            //viewModel.ReceiveDetailNewViewModel.CommodityId = receiptAllocation.CommodityID;
            
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

                return RedirectToAction("Index", "Receive");
            }
            viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
            viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
            ModelState.AddModelError("ReceiveDetails", "Please add at least one commodity");
            viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
            viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
            ViewBag.Commodities = _commodityService.GetAllSubCommodities().Select(c => new CommodityModel() { Id = c.CommodityID, Name = c.Name }).ToList();
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


        #region newReceive
        
        public virtual ActionResult ReadCommoditiesFromReceive(string receiveId)
        {
            var commodities = new List<ReceiveDetailsViewModel>();
            if (receiveId != "" && receiveId != null)
            {
                commodities = ReceiveDetailsViewModel.GenerateReceiveDetailModels(_receiveService.FindById(Guid.Parse(receiveId)).ReceiveDetails);

                UserProfile user = _userProfileService.GetUser(User.Identity.Name);

                foreach (var gridCommodities in commodities)
                {
                    if (user.PreferedWeightMeasurment.Equals("qn"))
                    {
                        gridCommodities.ReceivedQuantityInMt *= 10;
                        gridCommodities.SentQuantityInMt *= 10;
                    }
                }

                string str = Request["prev"];
                if (GetSelectedCommodities(str) != null)
                {
                    var allCommodities = GetSelectedCommodities(Request["prev"].ToString());
                    int count = -1;
                    //TODO Revise this section please
                    foreach (var receiveDetailsViewModel in allCommodities)
                    {
                        if (receiveDetailsViewModel.ReceiveDetailsId == null)
                        {
                            //receiveDetailViewModelComms.ReceiveDetailID = count--;
                            //receiveDetailsViewModel.ReceiveDetailCounter = count--;
                            commodities.Add(receiveDetailsViewModel);
                        }
                        //
                        // TODO the lines below are too nice to have but we need to look into the performance issue and 
                        //policies (i.e. editing should not be allowed ) may be only for quanitities 

                        else //replace the commodity read from the db by what's comming from the user
                        {
                            commodities.Remove(commodities.Find(p => p.ReceiveDetailsId == receiveDetailsViewModel.ReceiveDetailsId));
                            commodities.Add(receiveDetailsViewModel);
                        }
                    }
                }
            }
            else
            {
                string str = Request["prev"];
                if (GetSelectedCommodities(str) != null)
                {
                    commodities = GetSelectedCommodities(Request["prev"].ToString());
                    int count = -1;
                    foreach (var rdm in commodities)
                    {
                        //TODO: Revise this section
                        if (rdm.ReceiveDetailsId != null)
                        {
                            //rdm.ReceiveDetailID = count--;
                            //rdm.ReceiveDetailCounter = count--;
                        }
                    }
                }
            }

            ViewData["Commodities"] = _commodityService.GetAllSubCommodities().Select(c => new CommodityModel() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewData["Units"] = _unitService.GetAllUnit().Select(u => new UnitModel() { Id = u.UnitID, Name = u.Name }).ToList();
            return View(new GridModel(commodities));
        }
        private List<ReceiveDetailsViewModel> GetSelectedCommodities(string jsonArray)
        {
            List<ReceiveDetailsViewModel> commodities = new List<ReceiveDetailsViewModel>();
            if (!string.IsNullOrEmpty(jsonArray))
            {
                commodities = JsonConvert.DeserializeObject<List<ReceiveDetailsViewModel>>(jsonArray);
            }
            return commodities;
        }

        #endregion
    }
}
