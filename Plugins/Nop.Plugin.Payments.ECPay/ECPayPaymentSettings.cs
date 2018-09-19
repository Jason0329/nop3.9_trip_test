using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.ECPay
{
    public class ECPayPaymentSettings : ISettings
    {
        public string MerchantID { get; set; }
        public string Returnurl { get; set; }
        public string HashKey { get; set; }
        public string HashIV { get; set; }



    }
}
