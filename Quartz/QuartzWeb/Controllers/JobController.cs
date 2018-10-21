using QuartzCommon;
using QuartzWeb.Bussiness;
using QuartzWeb.Models;
using QuartzWeb.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuartzWeb.Controllers
{
    public class JobController : BaseController
    {
        // GET: Job
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult List()
        {
            var service = new JobService();
            var data = service.GeBackgroundJobInfoPagerList(this.GetPageParameter());
            var result = new ResponseResult() { success = true, message = "数据获取成功", data = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddPost(JobIndexViewModel model)
        {
            var result = new ResponseResult();
            var service = new JobService();
            var quartModel = new QuartzModel
            {
                JobName=model.Name,
                JobGroup=model.GroupName,
                Description=model.Description,
                JobClassName=model.ClassName,
                TriggerCron=model.CronExpression
            };
            result.success = service.InsertBackgroundJob(quartModel);
            return Json(result);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UpdateState(string GroupName,string JobName,int State)
        {
            var result = new ResponseResult();
            try
            {
                switch (State)
                {
                    //0-等待 1-暂停 2-正常执行 3-阻塞 4-错误
                    case 0:
                        QuartzHelper.StartJob(JobName, GroupName).GetAwaiter();
                        break;
                    case 1:
                        QuartzHelper.ResumeJob(JobName, GroupName).GetAwaiter();
                        break;
                    case 2:
                        QuartzHelper.PauseJob(JobName, GroupName).GetAwaiter();
                        break;
                }
                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteJob(List<JobDeleteViewModel> idList)
        {
            var result = new ResponseResult();
            
            try
            {
                foreach (var item in idList)
                {
                    QuartzHelper.DeleteJob(item.JobName, item.GroupName).GetAwaiter();
                }
                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }
            return Json(result);
        }
    }
}