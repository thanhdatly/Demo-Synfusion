using Syncfusion.EJ.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace SyncfusionMvcApplication3
{
    public partial class pivotClientController: Controller
    {
        public ActionResult PivotClientFeatures()
        {
              return View();
        } 
        public void ExportPivotClient()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
            PivotClientExport olapClient = new PivotClientExport();
            string args = System.Web.HttpContext.Current.Request.Form.GetValues(0)[0];
            olapClient.ExportPivotClient(string.Empty, args, System.Web.HttpContext.Current.Response);
        } 
    }
}
