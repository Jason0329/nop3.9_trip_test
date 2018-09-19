using Nop.Core.Plugins;
using Nop.Services.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Orders;
using Nop.Services.Localization;
using Nop.Core.Domain.Payments;
using System.Web.Routing;
using Nop.Plugin.Payments.ECPay.Controllers;
using ECPay.Payment.Integration;
using System.Web;

namespace Nop.Plugin.Payments.ECPay
{
    public class ECPayPaymentProcessor : BasePlugin, IPaymentMethod
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private HttpContextBase _httpContext;


        #endregion

        #region Ctor

        public ECPayPaymentProcessor(ILocalizationService localizationService
            ,HttpContextBase httpContext)
        {
            this._localizationService = localizationService;
            this._httpContext = httpContext;
        }

        #endregion

        #region Properties
        public bool SupportCapture
        {
            get { return false; }
        }

        public bool SupportPartiallyRefund
        {
            get { return false; }
        }

        public bool SupportRefund
        {
            get { return false; }
        }

        public bool SupportVoid
        {
            get { return false; }
        }

        public RecurringPaymentType RecurringPaymentType
        {
            get { return RecurringPaymentType.NotSupported; }
        }

        public PaymentMethodType PaymentMethodType
        {
            get { return PaymentMethodType.Redirection; }
        }

        public bool SkipPaymentInfo
        {
            get { return false; }
        }

        public string PaymentMethodDescription
        {
            get { return _localizationService.GetResource("Plugins.Payments.PayPalStandard.PaymentMethodDescription"); }
        }
        #endregion

        #region Methods

        ///It is not support recurring for this version
        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest) 
        {
            var result = new CancelRecurringPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            //let's ensure that at least 5 seconds passed after order is placed
            //P.S. there's no any particular reason for that. we just do it
            if ((DateTime.UtcNow - order.CreatedOnUtc).TotalSeconds < 5)
                return false;

            return true;
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            var result = new CapturePaymentResult();
            result.AddError("Capture method not supported");
            return result;
        }

        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            //var result = this.CalculateAdditionalFee(_orderTotalCalculationService, cart,
            //    _paypalStandardPaymentSettings.AdditionalFee, _paypalStandardPaymentSettings.AdditionalFeePercentage);
            return 0;
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "PaymentECPay";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Payments.ECPay.Controllers" }, { "area", null } };
        }

        public Type GetControllerType()
        {
            return typeof(PaymentECPayController);
        }

        public void GetPaymentInfoRoute(out string actionName, out string controllerName, out System.Web.Routing.RouteValueDictionary routeValues)
        {
            actionName = "PaymentInfo";
            controllerName = "PaymentECPay";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Payments.ECPay.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Returns a value indicating whether payment method should be hidden during checkout
        /// If the website will release to other country in the future, this function is required
        /// </summary>
        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            //you can put any logic here
            //for example, hide this payment method if all products in the cart are downloadable
            //or hide this payment method if current customer is from certain country
            return false;
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            List<string> enErrors = new List<string>();
            ECPayService oPayment = new ECPayService();
            try
            {
                oPayment.ECPayOrderSendPrepare(postProcessPaymentRequest);
                _httpContext.Response.Redirect("/ECPay/RedirectToECPay/" + postProcessPaymentRequest.Order.Id);
                //_httpContext.Response.Redirect(urlToRedirect);
            }
            catch(Exception ex)
            {
                enErrors.Add(ex.Message);
            }

            oPayment.ECPayOrderSend();
        }
        /// <summary>
        /// Process a payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            result.NewPaymentStatus = PaymentStatus.Pending;
            return result;
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        /// <summary>
        /// Refunds a payment
        /// </summary>
        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            var result = new RefundPaymentResult();
            result.AddError("Refund method not supported");
            return result;
        }

        /// <summary>
        /// Voids a payment
        /// </summary>
        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            var result = new VoidPaymentResult();
            result.AddError("Void method not supported");
            return result;
        }
        #endregion

    }
}
