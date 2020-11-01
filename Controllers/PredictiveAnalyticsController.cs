using Syncfusion.PMML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SyncfusionMvcApplication3
{
     public class PredictiveAnalyticsController : Controller
     {
         public ActionResult PredictiveAnalyticsFeatures()
        {
            return View();
        }
        [HttpPost]
         public ActionResult PredictiveAnalyticsFeatures(string PetalLength, string PetalWidth, string SepalLength, string SepalWidth)
        {
            string pmmlFilePath = Server.MapPath("~/App_Data/IrisTree.pmml");
            PMMLEvaluator pmmlEvaluator = new PMMLEvaluatorFactory().GetPMMLEvaluatorInstance(pmmlFilePath);
            var anonymousType = new
            {
                SepalLength = SepalLength,
                SepalWidth = SepalWidth,
                PetalLength = PetalLength,
                PetalWidth = PetalWidth,
            };
            PredictedResult predictedResult = pmmlEvaluator.GetResult(anonymousType, null);
            var result = predictedResult.PredictedValue;
            ViewBag.value = result;
            return Content(ViewBag.value);
        }
    }
}
