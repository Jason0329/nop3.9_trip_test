using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.ECPay.Models;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Payments.ECPay.Controllers
{
    public class PaymentECPayController : BasePaymentController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreContext _storeContext;
        private readonly ILogger _logger;
        private readonly IWebHelper _webHelper;
        private readonly PaymentSettings _paymentSettings;
        private readonly ECPayPaymentSettings _ECPayPaymentSettings;
        private readonly ShoppingCartSettings _shoppingCartSettings;

        public PaymentECPayController(IWorkContext workContext,
            IStoreService storeService, 
            ISettingService settingService, 
            IPaymentService paymentService, 
            IOrderService orderService, 
            IOrderProcessingService orderProcessingService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IStoreContext storeContext,
            ILogger logger, 
            IWebHelper webHelper,
            PaymentSettings paymentSettings,
            ECPayPaymentSettings ECPayPaymentSettings,
            ShoppingCartSettings shoppingCartSettings)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._paymentService = paymentService;
            this._orderService = orderService;
            this._orderProcessingService = orderProcessingService;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._storeContext = storeContext;
            this._logger = logger;
            this._webHelper = webHelper;
            this._paymentSettings = paymentSettings;
            this._ECPayPaymentSettings = ECPayPaymentSettings;
            this._shoppingCartSettings = shoppingCartSettings;
        }
        
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var ECPayPaymentSettings = _settingService.LoadSetting<ECPayPaymentSettings>(storeScope);

            var model = new ConfigurationModel();
            model.MerchantID = ECPayPaymentSettings.MerchantID;
            model.Returnurl = ECPayPaymentSettings.Returnurl;
            model.HashKey = ECPayPaymentSettings.HashKey;
            model.HashIV = ECPayPaymentSettings.HashIV;


            return View("~/Plugins/Payments.ECPay/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var ECPayPaymentSettings = _settingService.LoadSetting<ECPayPaymentSettings>(storeScope);

            //save settings
            ECPayPaymentSettings.MerchantID = model.MerchantID;
            ECPayPaymentSettings.Returnurl = model.Returnurl;
            ECPayPaymentSettings.HashKey = model.HashKey;
            ECPayPaymentSettings.HashIV = model.HashIV;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        //action displaying notification (warning) to a store owner about inaccurate PayPal rounding
        [ValidateInput(false)]
        public ActionResult RoundingWarning(bool passProductNamesAndTotals)
        {
            //prices and total aren't rounded, so display warning
            if (passProductNamesAndTotals && !_shoppingCartSettings.RoundPricesDuringCalculation)
                return Json(new { Result = _localizationService.GetResource("Plugins.Payments.ECPay.RoundingWarning") }, JsonRequestBehavior.AllowGet);

            return Json(new { Result = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            
            return View("~/Plugins/Payments.ECPay/Views/PaymentInfo.cshtml");
        }

        [NonAction]
        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            var warnings = new List<string>();
            return warnings;
        }

        [NonAction]
        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            var paymentInfo = new ProcessPaymentRequest();
            return paymentInfo;
        }

        
        public ActionResult RedirectToECPay(int Id)
        {
            Order CustomerOrder = _orderService.GetOrderById(Id);
            ECPayService oPayment = new ECPayService();
            oPayment.ECPayOrderSendPrepare(CustomerOrder);

            var model = new SendToECPayData();
            model.SendData = oPayment.ECPayOrderSendString();
            //var model = oPayment.ECPayOrderSendString();

            
            return View("~/Plugins/Payments.ECPay/Views/RedirectToECPay.cshtml", model);
        }
       
    }
}