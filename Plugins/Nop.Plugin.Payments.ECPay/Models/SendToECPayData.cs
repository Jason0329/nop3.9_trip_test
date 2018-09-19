using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Payments.ECPay.Models
{
    public class SendToECPayData : BaseNopModel
    {
        [AllowHtml]
        public string SendData { get; set; }
    }
}
