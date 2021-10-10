using CCPNCR_Record_Management.Controllers;
using CCPNCR_Record_Management.App_Data;
using CCPNCR_Record_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCPNCR_Record_Management.Controllers
{
    //[HandleError]
    //[SessionAuthorize]
    public class JSONCallsController : Controller
    {
        DataBaseAccess db = new DataBaseAccess();
        ResetPasswordController prl = new ResetPasswordController();

        public JsonResult CheckUserNameAvailability(string UserName)
        {
            return Json(db.CheckUserNameAvailability(UserName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatePassword(string Newpwd)
        {
            return Json(db.UpdatePassword(Newpwd), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendPwdResetLink(string UserEmail)
        {
            return Json(prl.SendPwdResetLink(UserEmail), JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckUserEmail(string UserEmail)
        {
            return Json(db.CheckUserEmail(UserEmail), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SignUp(string UserName, string FName, string LName, string Email, string Password, string Mobile)
        {
            string res=db.SignUp(UserName, FName, LName, Email, Password, Mobile);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckCurrentPassword(string password)
        {
            return Json(db.CheckPassword(password), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConfirmUserAction(int UserId,string ActionOnUser)
        {
            return Json(db.ConfirmUserAction(UserId, ActionOnUser), JsonRequestBehavior.AllowGet);
        }
        // GET: JSONCalls
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult DeleteFileType(int FileTypeId)
        {
            return Json(db.DeleteFileType(FileTypeId), JsonRequestBehavior.AllowGet);      
        }
        public JsonResult UpdateFileType(int FileTypeId,string FileType)
        {
            return Json(db.UpdateFileType(FileTypeId,FileType), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFileVolume(int FileVolumeId)
        {
            return Json(db.DeleteFileVolume(FileVolumeId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateFileVolume(int FileVolumeId, string FileVolume)
        {
            return Json(db.UpdateFileVolume(FileVolumeId, FileVolume), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFileRecord(int FileRecordId)
        {
            return Json(db.DeleteFileRecord(FileRecordId), JsonRequestBehavior.AllowGet);
        }


        public JsonResult RejectApproval(int FileRecordId, int FileIn_OutRecordId, string RejectionRemarks)
        {
            return Json(db.RejectApproval( FileRecordId, FileIn_OutRecordId, RejectionRemarks), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubmitApproval(int FileRecordId, int FileIn_OutRecordId ,string ApprovalRemarks)
        {
            return Json(db.SubmitApproval(FileRecordId,FileIn_OutRecordId, ApprovalRemarks), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FileReturn(int FileRecordId, int FileIn_OutRecordId, string ReturnRemarks)
        {
            return Json(db.FileReturn(FileRecordId, FileIn_OutRecordId, ReturnRemarks), JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetFileLatestStatus(int FileRecordId, string Message)
        //{
        //    return Json(db.GetFileLatestStatus(FileRecordId, Message), JsonRequestBehavior.AllowGet);
        //}
    }
}