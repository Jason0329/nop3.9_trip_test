using ECPay.Payment.Integration;
using Nop.Core.Domain.Orders;
using Nop.Services.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.ECPay
{
    public class ECPayService 
    {
        AllInOne oPayment;
        public ECPayService()
        {
            ECPayPaymentSettings ecPayPaymentSettings = new ECPayPaymentSettings();
            oPayment = new AllInOne();
            /* 服務參數 */
            oPayment.ServiceMethod = HttpMethod.HttpPOST;//介接服務時，呼叫 API 的方法
            oPayment.ServiceURL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";//要呼叫介接服務的網址
            oPayment.HashKey = "5294y06JbISpM5x9";//ECPay提供的Hash Key
            oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay提供的Hash IV
            oPayment.MerchantID = "2000132";//ECPay提供的特店編號

        }

        public void ECPayOrderSendPrepare(PostProcessPaymentRequest postProcessPaymentRequest)
        {
           
            oPayment.Send.ReturnURL = "http://example.com";//付款完成通知回傳的網址
            oPayment.Send.ClientBackURL = "http://www.ecpay.com.tw/";//瀏覽器端返回的廠商網址
            oPayment.Send.OrderResultURL = "";//瀏覽器端回傳付款結果網址
            oPayment.Send.MerchantTradeNo = postProcessPaymentRequest.Order.Id.ToString();//廠商的交易編號
            oPayment.Send.MerchantTradeDate = DateTime.Now;//廠商的交易時間
            oPayment.Send.TotalAmount = postProcessPaymentRequest.Order.OrderTotal;//交易總金額
            oPayment.Send.TradeDesc = "交易描述";//交易描述
            oPayment.Send.ChoosePayment = PaymentMethod.Credit;//使用的付款方式
            oPayment.Send.Remark = "";//備註欄位
            oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;//使用的付款子項目
            oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.No;//是否需要額外的付款資訊
            oPayment.Send.DeviceSource = DeviceType.PC;//來源裝置
            oPayment.Send.IgnorePayment = ""; //不顯示的付款方式
            oPayment.Send.PlatformID = "";//特約合作平台商代號
            oPayment.Send.HoldTradeAMT = HoldTradeType.No;
            oPayment.Send.CustomField1 = "";
            oPayment.Send.CustomField2 = "";
            oPayment.Send.EncryptType = 1;

            foreach (var OrderItem in postProcessPaymentRequest.Order.OrderItems)
            {
                oPayment.Send.Items.Add(new Item()
                {
                    Name = OrderItem.Product.Name,//商品名稱
                    Price = OrderItem.PriceInclTax,//商品單價
                    Currency = "新台幣",//幣別單位
                    Quantity = OrderItem.Quantity,//購買數量
                    URL = OrderItem.Product.Name,//商品的說明網址
                });
            }
            
        }

        public void ECPayOrderSendPrepare(Order CustomerOrder)
        {

            oPayment.Send.ReturnURL = "http://localhost:15537";//付款完成通知回傳的網址
            oPayment.Send.ClientBackURL = "http://www.ecpay.com.tw/";//瀏覽器端返回的廠商網址
            oPayment.Send.OrderResultURL = "";//瀏覽器端回傳付款結果網址
            oPayment.Send.MerchantTradeNo = "NopTestByJasln" + CustomerOrder.Id.ToString();//廠商的交易編號
            oPayment.Send.MerchantTradeDate = DateTime.Now;//廠商的交易時間
            oPayment.Send.TotalAmount = Decimal.Parse("1000");//交易總金額
            oPayment.Send.TradeDesc = "交易描述";//交易描述
            oPayment.Send.ChoosePayment = PaymentMethod.Credit;//使用的付款方式
            oPayment.Send.Remark = "";//備註欄位
            oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;//使用的付款子項目
            oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.No;//是否需要額外的付款資訊
            oPayment.Send.DeviceSource = DeviceType.PC;//來源裝置
            oPayment.Send.IgnorePayment = ""; //不顯示的付款方式
            oPayment.Send.PlatformID = "";//特約合作平台商代號
            oPayment.Send.HoldTradeAMT = HoldTradeType.No;
            oPayment.Send.CustomField1 = "";
            oPayment.Send.CustomField2 = "";
            oPayment.Send.EncryptType = 1;

            foreach (var OrderItem in CustomerOrder.OrderItems)
            {
                oPayment.Send.Items.Add(new Item()
                {
                    Name = OrderItem.Product.Name,//商品名稱
                    Price = 1000,//商品單價
                    Currency = "新台幣",//幣別單位
                    Quantity = OrderItem.Quantity,//購買數量
                    URL = OrderItem.Product.Name,//商品的說明網址
                });
            }

        }

        public string ECPayOrderSendString()
        {
            string ECPayHtmlString = "";
            oPayment.CheckOutString(ref ECPayHtmlString);
            return ECPayHtmlString;
        }

        public void ECPayOrderSend()
        {
            List<string> enErrors = new List<string>();
            string tt = "";
            try
            {
                
                enErrors.AddRange(oPayment.CheckOut());
                oPayment.CheckOutString(ref tt);
            }
            catch (Exception ex)
            {
                enErrors.Add(ex.Message);
            }
        }


        public string PaymentURL()
        {
            string tt = "";
            string ss = "";
            oPayment.CheckOutString(ref tt);
            oPayment.CheckOut(ss);
            return tt;
        }
      

        ~ECPayService()
        {
            
        }
    }
}
