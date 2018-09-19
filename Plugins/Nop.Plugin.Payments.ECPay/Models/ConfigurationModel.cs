using ECPay.Payment.Integration;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;

namespace Nop.Plugin.Payments.ECPay.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        public string MerchantID { get; set; }
        public string Returnurl { get; set; }
        public string HashKey { get; set; }
        public string HashIV { get; set; }

        public string ReturnURL { get; set; }
        public string ClientBackURL { get; set; }
        public string OrderResultURL { get; set; }
        public string MerchantTradeNo { get; set; }
        public DateTime MerchantTradeDate { get; set; }
        public Decimal TotalAmount { get; set; }
        public string TradeDesc { get; set; }//交易描述
        public PaymentMethod ChoosePayment { get; set; }//使用的付款方式
        public string Remark { get; set; }//備註欄位
        public PaymentMethodItem ChooseSubPayment { get; set; }//使用的付款子項目
        public ExtraPaymentInfo NeedExtraPaidInfo { get; set; }//是否需要額外的付款資訊
        public DeviceType DeviceSource { get; set; }//來源裝置
        public string IgnorePayment { get; set; }
        public string PlatformID { get; set; }//特約合作平台商代號
        public HoldTradeType HoldTradeAMT { get; set; }
        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public int EncryptType { get; set; }//預設為 1
       
    }
}