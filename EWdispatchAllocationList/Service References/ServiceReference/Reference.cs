﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 5.0.61118.0
// 
namespace EWdispatchAllocationList.ServiceReference {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AllocationModelDetail", Namespace="http://schemas.datacontract.org/2004/07/Cats.Models")]
    public partial class AllocationModelDetail : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Nullable<int> BeneficiariesField;
        
        private System.Nullable<int> FDPIDField;
        
        private string FDPNameField;
        
        private string RegionField;
        
        private string WoredaField;
        
        private string ZoneField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> Beneficiaries {
            get {
                return this.BeneficiariesField;
            }
            set {
                if ((this.BeneficiariesField.Equals(value) != true)) {
                    this.BeneficiariesField = value;
                    this.RaisePropertyChanged("Beneficiaries");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> FDPID {
            get {
                return this.FDPIDField;
            }
            set {
                if ((this.FDPIDField.Equals(value) != true)) {
                    this.FDPIDField = value;
                    this.RaisePropertyChanged("FDPID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FDPName {
            get {
                return this.FDPNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FDPNameField, value) != true)) {
                    this.FDPNameField = value;
                    this.RaisePropertyChanged("FDPName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Region {
            get {
                return this.RegionField;
            }
            set {
                if ((object.ReferenceEquals(this.RegionField, value) != true)) {
                    this.RegionField = value;
                    this.RaisePropertyChanged("Region");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Woreda {
            get {
                return this.WoredaField;
            }
            set {
                if ((object.ReferenceEquals(this.WoredaField, value) != true)) {
                    this.WoredaField = value;
                    this.RaisePropertyChanged("Woreda");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Zone {
            get {
                return this.ZoneField;
            }
            set {
                if ((object.ReferenceEquals(this.ZoneField, value) != true)) {
                    this.ZoneField = value;
                    this.RaisePropertyChanged("Zone");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="ServiceReference.EWDispatchService")]
    public interface EWDispatchService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:EWDispatchService/DoWork", ReplyAction="urn:EWDispatchService/DoWorkResponse")]
        System.IAsyncResult BeginDoWork(System.AsyncCallback callback, object asyncState);
        
        void EndDoWork(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:EWDispatchService/Cats.Service.IEWDispatchService.GetAllocationModelDetail", ReplyAction="urn:EWDispatchService/Cats.Service.IEWDispatchService.GetAllocationModelDetailRes" +
            "ponse")]
        System.IAsyncResult BeginCatsServiceIEWDispatchServiceGetAllocationModelDetail(EWdispatchAllocationList.ServiceReference.CatsServiceIEWDispatchServiceGetAllocationModelDetailRequest request, System.AsyncCallback callback, object asyncState);
        
        EWdispatchAllocationList.ServiceReference.CatsServiceIEWDispatchServiceGetAllocationModelDetailResponse EndCatsServiceIEWDispatchServiceGetAllocationModelDetail(System.IAsyncResult result);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Cats.Service.IEWDispatchService.GetAllocationModelDetail", WrapperNamespace="", IsWrapped=true)]
    public partial class CatsServiceIEWDispatchServiceGetAllocationModelDetailRequest {
        
        public CatsServiceIEWDispatchServiceGetAllocationModelDetailRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Cats.Service.IEWDispatchService.GetAllocationModelDetailResponse", WrapperNamespace="", IsWrapped=true)]
    public partial class CatsServiceIEWDispatchServiceGetAllocationModelDetailResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Cats.Service.IEWDispatchService.GetAllocationModelDetailResult", Namespace="", Order=0)]
        public System.Collections.ObjectModel.ObservableCollection<EWdispatchAllocationList.ServiceReference.AllocationModelDetail> CatsServiceIEWDispatchServiceGetAllocationModelDetailResult;
        
        public CatsServiceIEWDispatchServiceGetAllocationModelDetailResponse() {
        }
        
        public CatsServiceIEWDispatchServiceGetAllocationModelDetailResponse(System.Collections.ObjectModel.ObservableCollection<EWdispatchAllocationList.ServiceReference.AllocationModelDetail> CatsServiceIEWDispatchServiceGetAllocationModelDetailResult) {
            this.CatsServiceIEWDispatchServiceGetAllocationModelDetailResult = CatsServiceIEWDispatchServiceGetAllocationModelDetailResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface EWDispatchServiceChannel : EWdispatchAllocationList.ServiceReference.EWDispatchService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CatsServiceIEWDispatchServiceGetAllocationModelDetailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public CatsServiceIEWDispatchServiceGetAllocationModelDetailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<EWdispatchAllocationList.ServiceReference.AllocationModelDetail> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<EWdispatchAllocationList.ServiceReference.AllocationModelDetail>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EWDispatchServiceClient : System.ServiceModel.ClientBase<EWdispatchAllocationList.ServiceReference.EWDispatchService>, EWdispatchAllocationList.ServiceReference.EWDispatchService {
        
        private BeginOperationDelegate onBeginDoWorkDelegate;
        
        private EndOperationDelegate onEndDoWorkDelegate;
        
        private System.Threading.SendOrPostCallback onDoWorkCompletedDelegate;
        
        private BeginOperationDelegate onBeginCatsServiceIEWDispatchServiceGetAllocationModelDetailDelegate;
        
        private EndOperationDelegate onEndCatsServiceIEWDispatchServiceGetAllocationModelDetailDelegate;
        
        private System.Threading.SendOrPostCallback onCatsServiceIEWDispatchServiceGetAllocationModelDetailCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public EWDispatchServiceClient() {
        }
        
        public EWDispatchServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EWDispatchServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EWDispatchServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EWDispatchServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> DoWorkCompleted;
        
        public event System.EventHandler<CatsServiceIEWDispatchServiceGetAllocationModelDetailCompletedEventArgs> CatsServiceIEWDispatchServiceGetAllocationModelDetailCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult EWdispatchAllocationList.ServiceReference.EWDispatchService.BeginDoWork(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginDoWork(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void EWdispatchAllocationList.ServiceReference.EWDispatchService.EndDoWork(System.IAsyncResult result) {
            base.Channel.EndDoWork(result);
        }
        
        private System.IAsyncResult OnBeginDoWork(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((EWdispatchAllocationList.ServiceReference.EWDispatchService)(this)).BeginDoWork(callback, asyncState);
        }
        
        private object[] OnEndDoWork(System.IAsyncResult result) {
            ((EWdispatchAllocationList.ServiceReference.EWDispatchService)(this)).EndDoWork(result);
            return null;
        }
        
        private void OnDoWorkCompleted(object state) {
            if ((this.DoWorkCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.DoWorkCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void DoWorkAsync() {
            this.DoWorkAsync(null);
        }
        
        public void DoWorkAsync(object userState) {
            if ((this.onBeginDoWorkDelegate == null)) {
                this.onBeginDoWorkDelegate = new BeginOperationDelegate(this.OnBeginDoWork);
            }
            if ((this.onEndDoWorkDelegate == null)) {
                this.onEndDoWorkDelegate = new EndOperationDelegate(this.OnEndDoWork);
            }
            if ((this.onDoWorkCompletedDelegate == null)) {
                this.onDoWorkCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnDoWorkCompleted);
            }
            base.InvokeAsync(this.onBeginDoWorkDelegate, null, this.onEndDoWorkDelegate, this.onDoWorkCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult EWdispatchAllocationList.ServiceReference.EWDispatchService.BeginCatsServiceIEWDispatchServiceGetAllocationModelDetail(EWdispatchAllocationList.ServiceReference.CatsServiceIEWDispatchServiceGetAllocationModelDetailRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginCatsServiceIEWDispatchServiceGetAllocationModelDetail(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private System.IAsyncResult BeginCatsServiceIEWDispatchServiceGetAllocationModelDetail(System.AsyncCallback callback, object asyncState) {
            EWdispatchAllocationList.ServiceReference.CatsServiceIEWDispatchServiceGetAllocationModelDetailRequest inValue = new EWdispatchAllocationList.ServiceReference.CatsServiceIEWDispatchServiceGetAllocationModelDetailRequest();
            return ((EWdispatchAllocationList.ServiceReference.EWDispatchService)(this)).BeginCatsServiceIEWDispatchServiceGetAllocationModelDetail(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EWdispatchAllocationList.ServiceReference.CatsServiceIEWDispatchServiceGetAllocationModelDetailResponse EWdispatchAllocationList.ServiceReference.EWDispatchService.EndCatsServiceIEWDispatchServiceGetAllocationModelDetail(System.IAsyncResult result) {
            return base.Channel.EndCatsServiceIEWDispatchServiceGetAllocationModelDetail(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        private System.Collections.ObjectModel.ObservableCollection<EWdispatchAllocationList.ServiceReference.AllocationModelDetail> EndCatsServiceIEWDispatchServiceGetAllocationModelDetail(System.IAsyncResult result) {
            EWdispatchAllocationList.ServiceReference.CatsServiceIEWDispatchServiceGetAllocationModelDetailResponse retVal = ((EWdispatchAllocationList.ServiceReference.EWDispatchService)(this)).EndCatsServiceIEWDispatchServiceGetAllocationModelDetail(result);
            return retVal.CatsServiceIEWDispatchServiceGetAllocationModelDetailResult;
        }
        
        private System.IAsyncResult OnBeginCatsServiceIEWDispatchServiceGetAllocationModelDetail(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return this.BeginCatsServiceIEWDispatchServiceGetAllocationModelDetail(callback, asyncState);
        }
        
        private object[] OnEndCatsServiceIEWDispatchServiceGetAllocationModelDetail(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<EWdispatchAllocationList.ServiceReference.AllocationModelDetail> retVal = this.EndCatsServiceIEWDispatchServiceGetAllocationModelDetail(result);
            return new object[] {
                    retVal};
        }
        
        private void OnCatsServiceIEWDispatchServiceGetAllocationModelDetailCompleted(object state) {
            if ((this.CatsServiceIEWDispatchServiceGetAllocationModelDetailCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CatsServiceIEWDispatchServiceGetAllocationModelDetailCompleted(this, new CatsServiceIEWDispatchServiceGetAllocationModelDetailCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CatsServiceIEWDispatchServiceGetAllocationModelDetailAsync() {
            this.CatsServiceIEWDispatchServiceGetAllocationModelDetailAsync(null);
        }
        
        public void CatsServiceIEWDispatchServiceGetAllocationModelDetailAsync(object userState) {
            if ((this.onBeginCatsServiceIEWDispatchServiceGetAllocationModelDetailDelegate == null)) {
                this.onBeginCatsServiceIEWDispatchServiceGetAllocationModelDetailDelegate = new BeginOperationDelegate(this.OnBeginCatsServiceIEWDispatchServiceGetAllocationModelDetail);
            }
            if ((this.onEndCatsServiceIEWDispatchServiceGetAllocationModelDetailDelegate == null)) {
                this.onEndCatsServiceIEWDispatchServiceGetAllocationModelDetailDelegate = new EndOperationDelegate(this.OnEndCatsServiceIEWDispatchServiceGetAllocationModelDetail);
            }
            if ((this.onCatsServiceIEWDispatchServiceGetAllocationModelDetailCompletedDelegate == null)) {
                this.onCatsServiceIEWDispatchServiceGetAllocationModelDetailCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCatsServiceIEWDispatchServiceGetAllocationModelDetailCompleted);
            }
            base.InvokeAsync(this.onBeginCatsServiceIEWDispatchServiceGetAllocationModelDetailDelegate, null, this.onEndCatsServiceIEWDispatchServiceGetAllocationModelDetailDelegate, this.onCatsServiceIEWDispatchServiceGetAllocationModelDetailCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override EWdispatchAllocationList.ServiceReference.EWDispatchService CreateChannel() {
            return new EWDispatchServiceClientChannel(this);
        }
        
        private class EWDispatchServiceClientChannel : ChannelBase<EWdispatchAllocationList.ServiceReference.EWDispatchService>, EWdispatchAllocationList.ServiceReference.EWDispatchService {
            
            public EWDispatchServiceClientChannel(System.ServiceModel.ClientBase<EWdispatchAllocationList.ServiceReference.EWDispatchService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginDoWork(System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[0];
                System.IAsyncResult _result = base.BeginInvoke("DoWork", _args, callback, asyncState);
                return _result;
            }
            
            public void EndDoWork(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("DoWork", _args, result);
            }
            
            public System.IAsyncResult BeginCatsServiceIEWDispatchServiceGetAllocationModelDetail(EWdispatchAllocationList.ServiceReference.CatsServiceIEWDispatchServiceGetAllocationModelDetailRequest request, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = request;
                System.IAsyncResult _result = base.BeginInvoke("CatsServiceIEWDispatchServiceGetAllocationModelDetail", _args, callback, asyncState);
                return _result;
            }
            
            public EWdispatchAllocationList.ServiceReference.CatsServiceIEWDispatchServiceGetAllocationModelDetailResponse EndCatsServiceIEWDispatchServiceGetAllocationModelDetail(System.IAsyncResult result) {
                object[] _args = new object[0];
                EWdispatchAllocationList.ServiceReference.CatsServiceIEWDispatchServiceGetAllocationModelDetailResponse _result = ((EWdispatchAllocationList.ServiceReference.CatsServiceIEWDispatchServiceGetAllocationModelDetailResponse)(base.EndInvoke("CatsServiceIEWDispatchServiceGetAllocationModelDetail", _args, result)));
                return _result;
            }
        }
    }
}
