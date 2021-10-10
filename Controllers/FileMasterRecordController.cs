using CCPNCR_Record_Management.App_Data;
using CCPNCR_Record_Management.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using PagedList;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ClosedXML.Excel;
using iTextSharp.tool.xml;
using System.Linq;

namespace CCPNCR_Record_Management.Controllers
{
    [HandleError]
    [SessionAuthorize]
    public class FileMasterRecordController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserId"] != null)
            {
                ViewBag.NotificationCount = db.GetNotification(int.Parse(Session["UserId"].ToString()));
            }
            else
            {
                Session.Abandon();
                Session.RemoveAll();
                Session["UserId"] = null;
                Session["UserName"] = null;
                Session["UserFullName"] = null;
                Session["UserRoleType"] = null;
                FormsAuthentication.SignOut();
                filterContext.Result = new RedirectResult("/Login/Login");
            }

        }
        DataBaseAccess db = new DataBaseAccess();
        // GET: FileMasterRecord

        public string GetRKRemarks(int FileRecordId, int FileIn_OutRecordId)
        {
            return db.GetRKRemarks(FileRecordId, FileIn_OutRecordId);
        }

        public ActionResult FileInsideRecord(int? page)
        {
            IPagedList<FileRecord> AllFileInsideRecord = null;
            ViewBag.Validation = TempData["Validation"] as string;
            ViewBag.Message = TempData["Message"] as string;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            AllFileInsideRecord = db.FileInsideRecord().ToPagedList(pageNumber, pageSize);          
            return View(AllFileInsideRecord);
        }
        public ActionResult FileOutsideRecord(int? page)
        {
            IPagedList<FileRecord> AllFileOutsideRecord = null;
            ViewBag.Validation = TempData["Validation"] as string;
            ViewBag.Message = TempData["Message"] as string;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            AllFileOutsideRecord = db.FileOutsideRecord().ToPagedList(pageNumber, pageSize);
            return View(AllFileOutsideRecord);
            
        }

        public ActionResult FileOutsideRecordByUser(int? page)
        {
            IPagedList<FileRecord> AllFileOutsideRecordbtUser = null;
            ViewBag.Validation = TempData["Validation"] as string;
            ViewBag.Message = TempData["Message"] as string;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            AllFileOutsideRecordbtUser = db.FileOutsideRecordByUser( int.Parse(Session["RoleId"].ToString()), int.Parse(Session["UserId"].ToString())).ToPagedList(pageNumber, pageSize);
            return View(AllFileOutsideRecordbtUser);

        }

        public ActionResult FileSentforApproval(int? page)
        {
            IPagedList<FileRecord> AllFilesSentForApproval = null;
            ViewBag.Validation = TempData["Validation"] as string;
            ViewBag.Message = TempData["Message"] as string;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            AllFilesSentForApproval = db.fileSentforApproval().ToPagedList(pageNumber, pageSize);
            return View(AllFilesSentForApproval);
            
        }


        public ActionResult InsertFileRecord()
        {
            return View(db.FileRecordMaster());
        }

        [HttpPost]
        public ActionResult InsertFileRecord(FileMasterRecordVM model, string submitButton)
        {
            if(submitButton!=null)
            {
                ModelState.Clear();
                return View(db.FileRecordMaster()); 
            }

            if (ModelState.IsValid)
            {
                if (model.NPStart >= model.NPEnd)
                {
                    ViewBag.Validation = "Noting First Page Must be Less than Noting Last Page";
                    return View(db.FileRecordMaster());
                }
                else if (model.CPStart >= model.CPEnd)
                {
                    ViewBag.Validation = "First Chain Page Must be Less than Last Chain Page";
                    return View(db.FileRecordMaster());
                }
                else if (ModelState.IsValid)
                {
                    string result = db.InsertFileRecord(model);
                    if (result == "Success") { ModelState.Clear(); }
                    ViewBag.Message = result;
                }
            }
            else {
                ViewBag.Validation = "All Fields are required.";
            }
           
            return View(db.FileRecordMaster());
        }

        public ActionResult GetAllFileRecord(int? page)
        {
            IPagedList<FileRecord> AllFileRecord = null;
            ViewBag.Validation = TempData["Validation"] as string;
            ViewBag.Message = TempData["Message"] as string;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            AllFileRecord = db.GetAllFileRecord().ToPagedList(pageNumber, pageSize);
            return View(AllFileRecord);

        }

        public ActionResult getMovementHistory(int FileRecordId, string Message, int? page)
        {   
            IPagedList<FileLastStatus> filelaststatus = null;            
            ViewBag.Validation = TempData["Validation"] as string;
            ViewBag.Message = TempData["Message"] as string;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            filelaststatus = db.GetFileLastStatus(FileRecordId, Message).ToPagedList(pageNumber, pageSize);
            return PartialView("_FileLastStatus", filelaststatus);
        }

        public FileResult ExporttoExcel(string ExportTypeMessage)
        {
            DataTable dtRecords = db.ExportData(ExportTypeMessage);
            dtRecords.TableName = "Report";           
            using (XLWorkbook woekBook = new XLWorkbook())
            {
                woekBook.Worksheets.Add(dtRecords);
                using (MemoryStream stream = new MemoryStream())
                {
                    woekBook.SaveAs(stream);
                    string Filename = "Report_" + DateTime.Now.ToString("dd/MM/yyyy") + ".xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Filename);
                }
            }
        }

        public void ExporttoPdf(string ExportTypeMessage)
        {
            GeneratePdf pdf = new GeneratePdf();
            DataTable dt = db.ExportData(ExportTypeMessage);            
            pdf.ExportToPdf(dt, ExportTypeMessage);
        }


        public ActionResult EditFileRecord(int fileRecordId)
        {
            FileMasterRecordVM model = db.EditFileRecord(fileRecordId);
            return View(model);
        }

        [HttpPost]      
        public ActionResult UpdateFileRecord(string resetButton, FormCollection model)
        {
          
            if (resetButton != null)
            {
                ModelState.Clear();
                return RedirectToAction("GetAllFileRecord", "FileMasterRecord");
            }

            if (ModelState.IsValid)
            {
                if (int.Parse(model["item.NPStart"]) >= int.Parse(model["item.NPEnd"]))
                {
                    TempData["Validation"] = "Noting First Page Must be Less than Noting Last Page";
                    return RedirectToAction("GetAllFileRecord", "FileMasterRecord");
                }
                else if (int.Parse(model["item.CPStart"]) >= int.Parse(model["item.CPEnd"]))
                {
                    TempData["Validation"] = "First Chain Page Must be Less than Last Chain Page";
                    return RedirectToAction("GetAllFileRecord", "FileMasterRecord");
                }
                else if (ModelState.IsValid)
                {
                    string result = db.UpdateFileRecord(model);
                    if (result == "Success") { ModelState.Clear(); }
                    
                    TempData["Message"]= result;
                    return RedirectToAction("GetAllFileRecord", "FileMasterRecord");
                }
            }

            return RedirectToAction("GetAllFileRecord", "FileMasterRecord");
        }

        public ActionResult Load_In_out()
        {
            ViewBag.Validation = TempData["Validation"] as string;
            ViewBag.Message = TempData["Message"] as string;
            return View(db.LoadInOut());
        }


        [HttpPost]
        public ActionResult FileIn_Out(string btncancel,string btnInrecord,FileInOut model,string[] LIST_FileIn,string[] LIST_FileOut ,string rkremarks)
        {
            
            if (btncancel != null)
            {
                ModelState.Clear();
                return RedirectToAction("Load_In_out", "FileMasterRecord");
            }
            else if (btnInrecord != null && LIST_FileOut!=null && LIST_FileOut.Length > 0 && rkremarks != "")
            {
                string fileOutids = string.Join(",", LIST_FileOut);
                string res = db.GetFileInRecord(fileOutids, rkremarks);
                if (res == "Success")
                {
                    TempData["Message"] = "Success In Record";
                }
                else if (res != "Success")
                {
                    TempData["Message"] = "Failed In Record";
                }
            }
            else if (model.UserId > 0 && LIST_FileIn!=null && LIST_FileIn.Length > 0 && rkremarks != "")
            {
                string fileids = string.Join(",", LIST_FileIn);
                string result = db.InsertFileIn_Out(model.UserId, fileids, rkremarks);
                if (result == "Success")
                {
                    TempData["Message"] = "Success";
                }
                else if (result != "Success")
                {
                    TempData["Message"] = "Failed";
                }
            }
            else
            {
                TempData["Validation"] = "Please Select UserName/Enter Remarks/Select File if file is sent for Approval else Select File and Enter Remarks in case of File ReturnGetAllFileRecord.";
                return RedirectToAction("Load_In_out", "FileMasterRecord");
            }
            return RedirectToAction("Load_In_out", "FileMasterRecord");
        }


        

    }
}