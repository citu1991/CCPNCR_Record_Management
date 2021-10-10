using CCPNCR_Record_Management.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CCPNCR_Record_Management.App_Data
{
    [HandleError]
    public class DataBaseAccess
    {
        SqlConnection con = null;
        private String conStr;
        string result = "";

        public DataBaseAccess()
        {
            conStr = ConfigurationManager.ConnectionStrings["RMSConnection"].ToString();
        }
        public List<CountFiles> Count()
        {
            
            List<CountFiles> listCountFiles = new List<CountFiles>();
            using (con = new SqlConnection(conStr))
            {
                DataSet ds = get_DataSetWithPar("spFileMasterRecord", "CountFiles", "@UserId", HttpContext.Current.Session["UserId"].ToString());
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        CountFiles totalfiles = new CountFiles();
                        totalfiles.CountTotal_File = int.Parse(ds.Tables[0].Rows[0]["Total"].ToString());
                        listCountFiles.Add(totalfiles);
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        CountFiles totalfiles_Inside = new CountFiles();
                        totalfiles_Inside.CountTotal_Inside = int.Parse(ds.Tables[1].Rows[0]["Total"].ToString());
                        listCountFiles.Add(totalfiles_Inside);
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        CountFiles totalfiles_Outside = new CountFiles();
                        totalfiles_Outside.CountTotal_Outside = int.Parse(ds.Tables[2].Rows[0]["Total"].ToString());
                        listCountFiles.Add(totalfiles_Outside);
                    }
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        CountFiles totalfiles_Pending = new CountFiles();
                        totalfiles_Pending.CountTotal_Pending = int.Parse(ds.Tables[3].Rows[0]["Total"].ToString());
                        listCountFiles.Add(totalfiles_Pending);
                    }
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        CountFiles totalfiles_UserWise = new CountFiles();
                        totalfiles_UserWise.CountTotal_Userwise = int.Parse(ds.Tables[4].Rows[0]["Total"].ToString());
                        listCountFiles.Add(totalfiles_UserWise);
                    }
                }               
                return listCountFiles;
            }
        }



      
        public string SubmitApproval(int filerecordid,int fileinoutrecordid,string approvalremarks)
        {
            string result = "";
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileMasterRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "SubmitFileApproval");
                    cmd.Parameters.AddWithValue("@FileRecordId", filerecordid);
                    cmd.Parameters.AddWithValue("@FileIn_OutRecordId", fileinoutrecordid);
                    cmd.Parameters.AddWithValue("@FCApprovalRemarksByUser", approvalremarks);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else { return result = "Failed"; }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString(); 
            }
            return result;
        }



        public string FileReturn(int filerecordid, int fileinoutrecordid, string returnremarks)
        {
            string result = "";
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileMasterRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "ReturnFile");
                    cmd.Parameters.AddWithValue("@FileRecordId", filerecordid);
                    cmd.Parameters.AddWithValue("@FileIn_OutRecordId", fileinoutrecordid);
                    cmd.Parameters.AddWithValue("@FCReturnRemarksByUser", returnremarks);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else { return result = "Failed"; }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return result;
        }


        public string RejectApproval(int filerecordid, int fileinoutrecordid, string rejectionremarks)
        {
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileMasterRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "rejectapproval");
                    cmd.Parameters.AddWithValue("@FCRejectionRemarksByUser", rejectionremarks);
                    cmd.Parameters.AddWithValue("@FileRecordId", filerecordid);
                    cmd.Parameters.AddWithValue("@FileIn_OutRecordId", fileinoutrecordid);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else { return result = "Failed"; }
                }


            }
            catch (Exception ex)
            {
                return ex.Message.ToString();//Exception
            }
        }
          
        public string GetRKRemarks(int filerecordid, int fileinoutrecordid)
        {
            string result = "";
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileMasterRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "getRKRemarks");
                    cmd.Parameters.AddWithValue("@FileRecordId", filerecordid);
                    cmd.Parameters.AddWithValue("@FileIn_OutRecordId", fileinoutrecordid);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0 && dt != null)
                    {
                        result = dt.Rows[0]["FCRemarksByRK"].ToString();                       
                    }
                    else
                    {
                        result = "No Remarks Found";

                    }                   
                }
            }
            catch (Exception ex)
            {
                result = "No Remarks Found";
            }
            return result;
        }


        public RoleUserVM LoadRole(int UserId)
        {
           
            List<RoleUserVM> listRole = new List<RoleUserVM>();
            List<RoleUserVM> listUser = new List<RoleUserVM>();
            RoleUserVM RUVM = new RoleUserVM();

            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spSignUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "loadRole");
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();
                if (ds.Tables.Count > 0 && ds != null)
                {
                    foreach (DataRow data in ds.Tables[0].Rows)
                    {

                        RoleUserVM role = new RoleUserVM();
                        role.RoleId = int.Parse(data["RoleId"].ToString());
                        role.RoleName = data["RoleName"].ToString();
                        listRole.Add(role);
                       
                    }
                    foreach (DataRow data in ds.Tables[1].Rows)
                    {
                        RoleUserVM user = new RoleUserVM();
                        user.UserId = int.Parse(data["UserId"].ToString());
                        user.RoleId = int.Parse(data["RoleId"].ToString());
                        user.UserName = data["UserName"].ToString();
                        listUser.Add(user);
                    }
                }
                RUVM.RoleList = listRole;
                RUVM.UserList= listUser;
                return RUVM;
            }
        }


        public List<FileRecord> FileInsideRecord()
        {
            //FileRecord FileRecord = new FileRecord();
            List<FileRecord> listfileRecord = new List<FileRecord>();

            using (con = new SqlConnection(conStr))
            {
                DataTable dt = get_DataTable("spFileMasterRecord", "fileinsiderecord");
                if (dt.Rows.Count > 0 && dt != null)
                {
                    foreach (DataRow data in dt.Rows)
                    {
                        listfileRecord.Add(new FileRecord
                        {
                            // FileRecord filerecord = new FileRecord();
                            FileVolumeName = data["FileVolumeName"].ToString(),
                            FileRecordNumber = data["FileRecordNumber"].ToString(),
                            FileRecordName = data["FileRecordName"].ToString(),
                            NPStart = int.Parse(data["NPStart"].ToString()),
                            NPEnd = int.Parse(data["NPEnd"].ToString()),
                            DateonlastNP = DateTime.Parse(data["DateonlastNP"].ToString()),
                            SignedByonlastNP = data["SignedByonlastNP"].ToString(),
                            CPStart = int.Parse(data["CPStart"].ToString()),
                            CPEnd = int.Parse(data["CPEnd"].ToString()),
                            //listfileRecord.Add(filerecord);
                        });
                    }

                }

                //FileRecord.List_File_Record = listfileRecord;
                return listfileRecord;

            }
        }


        public List<FileRecord> FileOutsideRecord()
        {
            //FileRecord FileRecord = new FileRecord();
            List<FileRecord> listfileRecord = new List<FileRecord>();

            using (con = new SqlConnection(conStr))
            {
                DataTable dt = get_DataTable("spFileMasterRecord", "fileOutsiderecord");
                if (dt.Rows.Count > 0 && dt != null)
                {
                    foreach (DataRow data in dt.Rows)
                    {
                        listfileRecord.Add(new FileRecord
                        {
                            // FileRecord filerecord = new FileRecord();
                            FileVolumeName = data["FileVolumeName"].ToString(),
                            FileRecordNumber = data["FileRecordNumber"].ToString(),
                            FileRecordName = data["FileRecordName"].ToString(),
                            NPStart = int.Parse(data["NPStart"].ToString()),
                            NPEnd = int.Parse(data["NPEnd"].ToString()),
                            DateonlastNP = DateTime.Parse(data["DateonlastNP"].ToString()),
                            SignedByonlastNP = data["SignedByonlastNP"].ToString(),
                            CPStart = int.Parse(data["CPStart"].ToString()),
                            CPEnd = int.Parse(data["CPEnd"].ToString()),
                            FileCalledByUser = data["FileCalledByUser"].ToString(),
                            FileCalledDate = DateTime.Parse(data["FileCalledDate"].ToString()).ToString("dd/MM/yyyy"),
                            // listfileRecord.Add(filerecord);
                        });
                    }

                }
                return listfileRecord;
            }
        }


        public List<FileRecord> FileOutsideRecordByUser(int RoleId,int UserId)
        {
            //FileRecord FileRecord = new FileRecord();
            List<FileRecord> listfileRecord = new List<FileRecord>();

            using (con = new SqlConnection(conStr))
            {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileMasterRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "fileOutsiderecordbyuser");
                    cmd.Parameters.AddWithValue("@RoleId", RoleId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    foreach (DataRow data in dt.Rows)
                    {
                        listfileRecord.Add(new FileRecord
                        {
                            // FileRecord filerecord = new FileRecord();
                            FileIn_OutRecordId = int.Parse(data["FileIn_OutRecordId"].ToString()),
                            FileRecordId = int.Parse(data["FileRecordId"].ToString()),
                             FileVolumeName = data["FileVolumeName"].ToString(),
                            FileRecordNumber = data["FileRecordNumber"].ToString(),
                            FileRecordName = data["FileRecordName"].ToString(),
                            NPStart = int.Parse(data["NPStart"].ToString()),
                            NPEnd = int.Parse(data["NPEnd"].ToString()),
                            DateonlastNP = DateTime.Parse(data["DateonlastNP"].ToString()),
                            SignedByonlastNP = data["SignedByonlastNP"].ToString(),
                            CPStart = int.Parse(data["CPStart"].ToString()),
                            CPEnd = int.Parse(data["CPEnd"].ToString()),
                            FileCalledByUser = data["FileCalledByUser"].ToString(),
                            FileCalledDate = DateTime.Parse(data["FileCalledDate"].ToString()).ToString("dd/MM/yyyy"),
                            // listfileRecord.Add(filerecord);
                        });
                    }

                }
                return listfileRecord;
            }
        }


        public FileRecord filePendingforApproval(int userid)
        {
            FileRecord FileRecord = new FileRecord();
            List<FileRecord> listfileRecord = new List<FileRecord>();
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileMasterRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "filependingforapproval");
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0 && dt != null)
                    {
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            FileRecord filerecord = new FileRecord(); 
                            filerecord.FileIn_OutRecordId = int.Parse(dt.Rows[i]["FileIn_OutRecordId"].ToString());
                            filerecord.FileRecordId = int.Parse(dt.Rows[i]["FileRecordId"].ToString());                            
                            filerecord.FileRecordNumber = dt.Rows[i]["FileRecordNumber"].ToString();
                            filerecord.FileRecordName = dt.Rows[i]["FileRecordName"].ToString();
                            filerecord.NPStart = int.Parse(dt.Rows[i]["NPStart"].ToString());
                            filerecord.NPEnd = int.Parse(dt.Rows[i]["NPEnd"].ToString());
                            filerecord.DateonlastNP = DateTime.Parse(dt.Rows[i]["DateonlastNP"].ToString());
                            filerecord.SignedByonlastNP = dt.Rows[i]["SignedByonlastNP"].ToString();
                            filerecord.CPStart = int.Parse(dt.Rows[i]["CPStart"].ToString());
                            filerecord.CPEnd = int.Parse(dt.Rows[i]["CPEnd"].ToString());
                            listfileRecord.Add(filerecord);
                        }

                    }
                    else
                    {
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            FileRecord filerecord = new FileRecord();
                            filerecord.FileRecordId = 0;
                            listfileRecord.Add(filerecord);
                        }

                    }
                    FileRecord.List_File_Record = listfileRecord;
                    return FileRecord;
                }
            }
            catch (Exception ex)
            {
                FileRecord filerecord = new FileRecord();
                filerecord.FileRecordId = 0;
                listfileRecord.Add(filerecord);
                ex.Message.ToString();
                FileRecord.List_File_Record = listfileRecord;
                return FileRecord;
            }
        }
        public List<FileRecord> fileSentforApproval()
        {
            //FileRecord FileRecord = new FileRecord();
            List<FileRecord> listfileRecord = new List<FileRecord>();
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    DataTable dt = get_DataTable("spFileMasterRecord", "filesentforapproval");
                    if (dt.Rows.Count > 0 && dt != null)
                    {
                        foreach (DataRow data in dt.Rows)
                        {
                            listfileRecord.Add(new FileRecord
                            {
                           // FileRecord filerecord = new FileRecord();
                           FileRecordId = int.Parse(data["FileRecordId"].ToString()),
                           UserName = data["UserName"].ToString(),
                           ApprovalSentDate = DateTime.Parse(data["ApprovalSentDate"].ToString()).ToString("dd/MM/yyyy"),
                           FileRecordNumber = data["FileRecordNumber"].ToString(),
                           FileRecordName = data["FileRecordName"].ToString(),
                           NPStart = int.Parse(data["NPStart"].ToString()),
                           NPEnd = int.Parse(data["NPEnd"].ToString()),
                           DateonlastNP = DateTime.Parse(data["DateonlastNP"].ToString()),
                           SignedByonlastNP = data["SignedByonlastNP"].ToString(),
                           CPStart = int.Parse(data["CPStart"].ToString()),
                           CPEnd = int.Parse(data["CPEnd"].ToString()),
                            //listfileRecord.Add(filerecord);
                        });
                    }
                    }

                    else
                    {
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            FileRecord filerecord = new FileRecord();
                            filerecord.FileRecordId = 0;
                            listfileRecord.Add(filerecord);
                        }

                    }
                    //FileRecord.List_File_Record = listfileRecord;
                    //return FileRecord;
                    return listfileRecord;
                }
            }
            catch (Exception ex)
            {
                FileRecord filerecord = new FileRecord();
                filerecord.FileRecordId = 0;
                listfileRecord.Add(filerecord);
                ex.Message.ToString();
                return listfileRecord;               
            }
        }
        public int GetNotification(int Userid)
        {
            int count = 0;
            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spSignUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "NotificationCount");
                cmd.Parameters.AddWithValue("@UserId", Userid);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    count = int.Parse(dt.Rows[0]["NotificationCount"].ToString());
                }
                else {
                    count = 0;
                }
                return count;
            }
        }

        public FileInOut LoadInOut()
        {
            FileInOut FileInOut = new FileInOut();
            List<FileInOut> listfilein = new List<FileInOut>();
            List<FileInOut> listfileout = new List<FileInOut>();
            List<FileInOut> listuser = new List<FileInOut>();
            using (con = new SqlConnection(conStr))
                {
                    DataSet ds = get_DataSet("spFileMasterRecord", "LoadIN_Out");
                    if (ds.Tables.Count > 0)
                    {
                        for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FileInOut filetype = new FileInOut();
                            filetype.FileRecordId = int.Parse(ds.Tables[0].Rows[i]["FileRecordId"].ToString());
                            filetype.FileRecordCompleteName = ds.Tables[0].Rows[i]["FileName"].ToString();
                            listfilein.Add(filetype);
                        }
                        for (var i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            FileInOut user = new FileInOut();
                            user.UserId = int.Parse(ds.Tables[1].Rows[i]["UserId"].ToString());
                            user.UserName = ds.Tables[1].Rows[i]["UserName"].ToString();
                            listuser.Add(user);
                        }
                        for (var i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            FileInOut filetype = new FileInOut();
                            filetype.FileRecordId = int.Parse(ds.Tables[2].Rows[i]["FileRecordId"].ToString());
                            filetype.FileRecordCompleteName = ds.Tables[2].Rows[i]["FileName"].ToString();                            
                            listfileout.Add(filetype);
                        }
                    FileInOut.LIST_FileIn = listfilein;
                    FileInOut.LIST_FileOut = listfileout;
                    FileInOut.LIST_User = listuser;
                   
                    }
                    else
                    {
                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FileInOut filetype = new FileInOut();                       
                        filetype.FileRecordId = 0;                       
                        listfilein.Add(filetype);
                    }
                    for (var i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        FileInOut user = new FileInOut();
                        user.UserId = int.Parse(ds.Tables[1].Rows[i]["UserId"].ToString());
                        user.UserName = ds.Tables[1].Rows[i]["UserName"].ToString();
                        listuser.Add(user);
                    }
                    for (var i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        FileInOut filetype = new FileInOut();                        
                        filetype.FileRecordId = 0;                       
                        listfileout.Add(filetype);
                    }
                    FileInOut.LIST_FileIn = listfilein;
                    FileInOut.LIST_FileOut = listfileout;
                    FileInOut.LIST_User = listuser;
                }
                    return FileInOut;
                }
        }
        public string InsertFileIn_Out(int UserId,string fileRecordIds, string rkremarks)
        {

            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileIn_OutStatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "InsertFileIn_OutRecord");
                    cmd.Parameters.AddWithValue("@FCByUserId", UserId); 
                    cmd.Parameters.AddWithValue("@FileRecordIds", fileRecordIds+",");
                    cmd.Parameters.AddWithValue("@FCRemarksByRK", rkremarks);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else
                    {
                        result = cmd.Parameters["@Result"].Value.ToString(); // error
                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                result= ex.Message.ToString();
                return result;//Exception
            }

        }

        public string GetFileInRecord(string fileIds, string rkremarks)
        {

            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileIn_OutStatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "GetFileInRecord");                    
                    cmd.Parameters.AddWithValue("@FileRecordIds", fileIds + ",");                    
                    cmd.Parameters.AddWithValue("@FCReturnRemarksByUser", rkremarks);

                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else
                    {
                        result = cmd.Parameters["@Result"].Value.ToString(); // error
                        return result;
                    }
                 }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
                return result;//Exception
            }

        }
        public string DeleteFileRecord(int fileRecordId)
        {
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileMasterRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "DeleteFileRecord");
                    cmd.Parameters.AddWithValue("@FileRecordId", fileRecordId);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else { return result = "Deletion Failed"; }
                }


            }
            catch (Exception ex)
            {
                return ex.Message.ToString();//Exception
            }
        }
        public FileMasterRecordVM FileRecordMaster()
        {
            FileMasterRecordVM FileMasterRecord = new FileMasterRecordVM();
            List<FileMasterRecordVM> listfilemasterRecord = new List<FileMasterRecordVM>();

            List<FileType> listfiletype = new List<FileType>();
            List<FileVolume> listfilevolume = new List<FileVolume>();
            using (con = new SqlConnection(conStr))
            {
                DataSet  ds = get_DataSet("spFileMasterRecord", "GetType_Volume");
                if (ds.Tables.Count > 0)
                {
                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FileType filetype = new FileType();
                        filetype.FileTypeId = int.Parse(ds.Tables[0].Rows[i]["FileTypeId"].ToString());
                        filetype.FileTypeName = ds.Tables[0].Rows[i]["FileTypeName"].ToString();
                        listfiletype.Add(filetype);
                    }
                    for (var i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        FileVolume filevolume = new FileVolume();
                        filevolume.FileVolumeId = int.Parse(ds.Tables[1].Rows[i]["FileVolumeId"].ToString());
                        filevolume.FileVolumeName = ds.Tables[1].Rows[i]["FileVolumeName"].ToString();
                        listfilevolume.Add(filevolume);
                    }
                    FileMasterRecord.LIST_FileType = listfiletype;
                    FileMasterRecord.LIST_FileVolume = listfilevolume;
                }
                else
                {
                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FileType filetype = new FileType();
                        filetype.FileTypeId = 0;
                        filetype.FileTypeName = "";
                        listfiletype.Add(filetype);
                    }
                    for (var i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        FileVolume filevolume = new FileVolume();
                        filevolume.FileVolumeId = 0;
                        filevolume.FileVolumeName = "";
                        listfilevolume.Add(filevolume);
                    }
                    FileMasterRecord.LIST_FileType = listfiletype;
                    FileMasterRecord.LIST_FileVolume = listfilevolume;
                }
                return FileMasterRecord;
            }
        }
        
        public string InsertFileRecord(FileMasterRecordVM model)
        {

            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileMasterRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "InsertFileRecord");
                    cmd.Parameters.AddWithValue("@FileTypeId", model.FileTypeId);
                    cmd.Parameters.AddWithValue("@FileVolumeId", model.FileVolumeId);
                    cmd.Parameters.AddWithValue("@FileRecordName", model.FileRecordName);
                    cmd.Parameters.AddWithValue("@FileRecordNumber", model.FileRecordNumber);
                    cmd.Parameters.AddWithValue("@NPStart", model.NPStart);
                    cmd.Parameters.AddWithValue("@NPEnd", model.NPEnd);
                    cmd.Parameters.AddWithValue("@DateonlastNP", model.DateonlastNP);
                    cmd.Parameters.AddWithValue("@SignedByonlastNP", model.SignedByonlastNP);
                    cmd.Parameters.AddWithValue("@CPStart", model.CPStart);
                    cmd.Parameters.AddWithValue("@CPEnd", model.CPEnd);
                    cmd.Parameters.AddWithValue("@User", HttpContext.Current.Session["UserName"].ToString());
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else
                    {
                        result = cmd.Parameters["@Result"].Value.ToString(); // error
                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();//Exception
            }

        }
        public string UpdateFileRecord(FormCollection model)
        {

            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileMasterRecord", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "UpdateFileRecord");
                    cmd.Parameters.AddWithValue("@FileRecordId", model["item.FileRecordId"]);
                    cmd.Parameters.AddWithValue("@FileTypeId", model["item.FileTypeId"]);
                    cmd.Parameters.AddWithValue("@FileVolumeId", model["item.FileVolumeId"]);
                    cmd.Parameters.AddWithValue("@FileRecordName", model["item.FileRecordName"]);
                    cmd.Parameters.AddWithValue("@FileRecordNumber", model["item.FileRecordNumber"]);
                    cmd.Parameters.AddWithValue("@NPStart", model["item.NPStart"]);
                    cmd.Parameters.AddWithValue("@NPEnd", model["item.NPEnd"]);
                    cmd.Parameters.AddWithValue("@DateonlastNP",DateTime.Parse(model["item.DateonlastNP"]));
                    cmd.Parameters.AddWithValue("@SignedByonlastNP", model["item.SignedByonlastNP"]);
                    cmd.Parameters.AddWithValue("@CPStart", model["item.CPStart"]);
                    cmd.Parameters.AddWithValue("@CPEnd", model["item.CPEnd"]);
                    cmd.Parameters.AddWithValue("@User", HttpContext.Current.Session["UserName"].ToString());
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();
                        if (result == "Failed") { result = "Failed"; } else { result = "Error"; }                      
                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
                return result;//Exception
            }

        }

        public FileMasterRecordVM EditFileRecord(int fileRecordId)
        {
            FileMasterRecordVM FileMasterRecord = new FileMasterRecordVM();
            List<FileMasterRecordVM> listfilemasterRecord = new List<FileMasterRecordVM>();

            List<FileRecord> listfilerecord = new List<FileRecord>();
            List<FileType> listfiletype = new List<FileType>();
            List<FileVolume> listfilevolume = new List<FileVolume>();
            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spFileMasterRecord", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "GetFileRecord");
                cmd.Parameters.AddWithValue("@FileRecordId", fileRecordId);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);                
                if (ds.Tables.Count > 0)
                {
                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FileType filetype = new FileType();
                        filetype.FileTypeId = int.Parse(ds.Tables[0].Rows[i]["FileTypeId"].ToString());
                        filetype.FileTypeName = ds.Tables[0].Rows[i]["FileTypeName"].ToString();
                        listfiletype.Add(filetype);
                    }
                    for (var i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        FileVolume filevolume = new FileVolume();
                        filevolume.FileVolumeId = int.Parse(ds.Tables[1].Rows[i]["FileVolumeId"].ToString());
                        filevolume.FileVolumeName = ds.Tables[1].Rows[i]["FileVolumeName"].ToString();
                        listfilevolume.Add(filevolume);
                    }
                    for (var i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        FileRecord filerecord = new FileRecord();
                        filerecord.FileRecordId= int.Parse(ds.Tables[2].Rows[i]["FileRecordId"].ToString());
                        filerecord.FileVolumeId = int.Parse(ds.Tables[2].Rows[i]["FileVolumeId"].ToString());
                        filerecord.FileTypeId = int.Parse(ds.Tables[2].Rows[i]["FileTypeId"].ToString());
                        filerecord.FileRecordNumber = ds.Tables[2].Rows[i]["FileRecordNumber"].ToString();
                        filerecord.FileRecordName = ds.Tables[2].Rows[i]["FileRecordName"].ToString();
                        filerecord.NPStart = int.Parse(ds.Tables[2].Rows[i]["NPStart"].ToString());
                        filerecord.NPEnd = int.Parse(ds.Tables[2].Rows[i]["NPEnd"].ToString());
                        filerecord.DateonlastNP =DateTime.Parse(ds.Tables[2].Rows[i]["DateonlastNP"].ToString());
                        filerecord.SignedByonlastNP = ds.Tables[2].Rows[i]["SignedByonlastNP"].ToString();
                        filerecord.CPStart = int.Parse(ds.Tables[2].Rows[i]["CPStart"].ToString());
                        filerecord.CPEnd = int.Parse(ds.Tables[2].Rows[i]["CPEnd"].ToString());
                        listfilerecord.Add(filerecord);
                    }
                    FileMasterRecord.LIST_FileType = listfiletype;
                    FileMasterRecord.LIST_FileVolume = listfilevolume;
                    FileMasterRecord.LIST_FileRecord = listfilerecord;
                }
                else
                {
                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FileType filetype = new FileType();
                        filetype.FileTypeId = 0;
                        filetype.FileTypeName = "";
                        listfiletype.Add(filetype);
                    }
                    for (var i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        FileVolume filevolume = new FileVolume();
                        filevolume.FileVolumeId = 0;
                        filevolume.FileVolumeName = "";
                        listfilevolume.Add(filevolume);
                    }
                    for (var i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        FileMasterRecordVM file = new FileMasterRecordVM();
                        file.FileRecordId = 0;
                        listfilemasterRecord.Add(file);
                    }
                    FileMasterRecord.LIST_FileType = listfiletype;
                    FileMasterRecord.LIST_FileVolume = listfilevolume;
                    FileMasterRecord.LIST_FileRecord = listfilerecord;
                }
                return FileMasterRecord;
            }
        }

        public List<FileLastStatus> GetFileLastStatus(int FileRecordId, string Message)
        {

            List<FileLastStatus> listfileRecord = new List<FileLastStatus>();
            try
            {
                DataTable dt = get_LastStatus("spFileMasterRecord", "getFileLatestRemarks", FileRecordId, Message);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    if (Message == "Pending" || Message == "Out" || Message == "Inside" || Message == "file_Log")
                    {
                        if (dt.Rows.Count > 0 && dt != null)
                        {
                            foreach (DataRow data in dt.Rows)
                            {
                                listfileRecord.Add(new FileLastStatus
                                {
                                    FileRecordId=FileRecordId,
                                    Message=Message,
                                    FCRemarksByRK = data["FCRemarksByRK"].ToString(),
                                    UserFullName = data["UserFullName"].ToString(),
                                    FCReturnRemarksByUser = data["FCReturnRemarksByUser"].ToString(),
                                    FCReturnDate = data["FCReturnDate"].ToString(),

                                    FCApprovalRemarksByUser = data["FCApprovalRemarksByUser"].ToString(),
                                    FCRejectionRemarksByUser = data["FCRejectionRemarksByUser"].ToString(),
                                    FCApprovalDate = data["FCApprovalDate"].ToString(),
                                    FCRejectionDate = data["FCRejectionDate"].ToString(),

                                });
                            }
                        }                        
                    }
                    else
                    {
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            FileLastStatus filestatus = new FileLastStatus();
                            filestatus.Error = "File Not Called Yet";
                            listfileRecord.Add(filestatus);
                        }                        
                    }                   
                }
                else
                {
                    FileLastStatus filestatus = new FileLastStatus();
                    filestatus.Error = "There is no Movement yet.";
                    listfileRecord.Add(filestatus);
                }              

                return listfileRecord;
            }
            catch (Exception ex)
            {

                FileLastStatus filestatus = new FileLastStatus();
                filestatus.Error = ex.Message.ToString();
                listfileRecord.Add(filestatus);    
                return listfileRecord;
            }            
        }


        public List<FileRecord> GetAllFileRecord()
        {
           // FileRecord FileRecord = new FileRecord();
            List<FileRecord> listfileRecord = new List<FileRecord>();
            try{
                    using (con = new SqlConnection(conStr))
                    {
                        DataTable dt = get_DataTable("spFileMasterRecord", "GetAllFileRecord");
                    if (dt.Rows.Count > 0 && dt != null)
                    {
                        foreach (DataRow data in dt.Rows)
                        {
                            listfileRecord.Add(new FileRecord
                            {
                                //FileRecord filerecord = new FileRecord();
                                FileRecordId = Convert.ToInt32(data["FileRecordId"]),
                                IsFileInRecord = int.Parse(data["FileIn_OutStatus"].ToString()),
                                IsApprovalSent = int.Parse(data["IsApprovalSent"].ToString()),
                                IsApproved = int.Parse(data["IsApproved"].ToString()),
                                FileTypeName = data["FileTypeName"].ToString(),
                                FileVolumeName = data["FileVolumeName"].ToString(),
                                FileRecordNumber = data["FileRecordNumber"].ToString(),
                                FileRecordName = data["FileRecordName"].ToString(),
                                NPStart = int.Parse(data["NPStart"].ToString()),
                                NPEnd = int.Parse(data["NPEnd"].ToString()),
                                DateonlastNP = DateTime.Parse(data["DateonlastNP"].ToString()),
                                SignedByonlastNP = data["SignedByonlastNP"].ToString(),
                                CPStart = int.Parse(data["CPStart"].ToString()),
                                CPEnd = int.Parse(data["CPEnd"].ToString()),
                                // listfileRecord.Add(filerecord);
                            });

                        }
                    }
                    else
                    {
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            FileRecord filerecord = new FileRecord();
                            filerecord.FileRecordId = 0;
                            listfileRecord.Add(filerecord);
                        }

                    }

                        return listfileRecord;
                    }
            }catch (Exception ex)
            {
                FileRecord filerecord = new FileRecord();
                filerecord.FileRecordId = 0;
                listfileRecord.Add(filerecord);
                ex.Message.ToString();
                return listfileRecord;
            }
        }



        public DataTable ExportData(string message)
        {
            string mode = "";
            if (message == "EXCELAllFILE" || message == "PDFAllFILE")
            {
                mode = "ExportAllFiles";
            }
            else if (message == "EXCELAllINSIDEFILE" || message == "PDFAllINSIDEFILE")
            {
                mode = "ExportAllInsideFiles"; 
            }
            else if (message == "EXCELAllOUTSIDEFILE" || message == "PDFAllOUTSIDEFILE")
            {
                mode = "ExportAllOutsideFiles"; 
            }
            else if (message == "EXCELAllFILESENTFORAPPROVAL" || message == "PDFAllFILEENTFORAPPROVAL")
            {
                mode = "ExportAllfilesentforapproval";
            }
            using (con = new SqlConnection(conStr))
            {
                DataTable dt = get_DataTable("spReports", mode);
                return dt;
            }
        }

        public string DeleteFileType(int fileTypeId)
        {
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileType", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "DeleteFileType");
                    cmd.Parameters.AddWithValue("@FileTypeId", fileTypeId);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        if (cmd.Parameters["@Result"].Value.ToString() == "Success")
                        {
                            result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        } 
                    }
                    else if (cmd.Parameters["@Result"].Value.ToString() == "Record Linked")
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //Record Linked 
                    }
                    else { return result = "Deletion Failed"; }
                    return result;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();//Exception
            }
        }

        public string UpdateFileType(int fileTypeId, string FileType)
        {
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileType", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "UpdateFileType");
                    cmd.Parameters.AddWithValue("@FileTypeId", fileTypeId);
                    cmd.Parameters.AddWithValue("@FileTypeName", FileType);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else { return result = "Updation Failed"; }
                }


            }
            catch (Exception ex)
            {
                return ex.Message.ToString();//Exception
            }
        }
        public FileType GetFileType(int fileTypeId)
        {
            FileType ft = new FileType();
            
            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spFileType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "GetFileType");
                cmd.Parameters.AddWithValue("@FileTypeId", fileTypeId);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ft.FileTypeId=int.Parse(dt.Rows[0]["FileTypeId"].ToString());
                    ft.FileTypeName = dt.Rows[0]["FileTypeName"].ToString();
                    return ft;
                }
                else {
                    ft.FileTypeId = 0;
                    ft.FileTypeName = "";
                    return ft;
                }
               
            }
        }

        public FileType LoadFileType()
        {           
            FileType FileType = new FileType();
            List<FileType> listfiletype = new List<FileType>();
            using (con = new SqlConnection(conStr))
            {
                DataTable dt = get_DataTable("spFileType", "GetFileTypeList");
                if (dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        FileType filetype = new FileType();
                        filetype.FileTypeId = int.Parse(dt.Rows[i]["FileTypeId"].ToString());
                        filetype.FileTypeName = dt.Rows[i]["FileTypeName"].ToString();
                        listfiletype.Add(filetype);
                    }
                    FileType.ListFileType = listfiletype;
                }
                else
                {
                    FileType.FileTypeId = 0;
                    FileType.ListFileType = listfiletype;
                }
                return FileType;
            }
        }

        public string insertFileType(FileType model)
        {
           
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileType", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "insertFileType");
                    cmd.Parameters.AddWithValue("@FileTypeName", model.FileTypeName);                  
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else
                    {
                        result = cmd.Parameters["@Result"].Value.ToString(); // error
                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();//Exception
            }

        }

        public string DeleteFileVolume(int fileVolumeId)
        {
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileVolume", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "DeleteFileVolume");
                    cmd.Parameters.AddWithValue("@FileVolumeId", fileVolumeId);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        if (cmd.Parameters["@Result"].Value.ToString() == "Success")
                        {
                            result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        }
                    }
                    else if (cmd.Parameters["@Result"].Value.ToString() == "Record Linked")
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //Record Linked 
                    }
                    else { return result = "Deletion Failed"; }
                    return result;
                }                    
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();//Exception
            }
        }

        public string UpdateFileVolume(int fileVolumeId, string FileVolume)
        {
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileVolume", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "UpdateFileVolume");
                    cmd.Parameters.AddWithValue("@FileVolumeId", fileVolumeId);
                    cmd.Parameters.AddWithValue("@FileVolumeName", FileVolume);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else { return result = "Updation Failed"; }
                }


            }
            catch (Exception ex)
            {
                return ex.Message.ToString();//Exception
            }
        }
        public FileVolume GetFileVolume(int fileVolumeId)
        {
            FileVolume ft = new FileVolume();

            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spFileVolume", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "GetFileVolume");
                cmd.Parameters.AddWithValue("@FileVolumeId", fileVolumeId);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ft.FileVolumeId = int.Parse(dt.Rows[0]["FileVolumeId"].ToString());
                    ft.FileVolumeName = dt.Rows[0]["FileVolumeName"].ToString();
                    return ft;
                }
                else
                {
                    ft.FileVolumeId = 0;
                    ft.FileVolumeName = "";
                    return ft;
                }

            }
        }
        public FileVolume LoadFileVolume()
        {
            FileVolume FileVolume = new FileVolume();
            List<FileVolume> listfilevolume = new List<FileVolume>();
            using (con = new SqlConnection(conStr))
            {
                DataTable dt = get_DataTable("spFileVolume", "GetFileVolumeList");
                if (dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        FileVolume filevolume = new FileVolume();
                        filevolume.FileVolumeId = int.Parse(dt.Rows[i]["FileVolumeId"].ToString());
                        filevolume.FileVolumeName = dt.Rows[i]["FileVolumeName"].ToString();
                        listfilevolume.Add(filevolume);
                    }
                    FileVolume.ListFileVolume = listfilevolume;
                }
                else
                {
                    FileVolume.FileVolumeId = 0;
                    FileVolume.ListFileVolume = listfilevolume;
                }
                return FileVolume;
            }
        }
        public string insertFileVolume(FileVolume model)
        {

            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spFileVolume", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "insertFileVolume");
                    cmd.Parameters.AddWithValue("@FileVolumeName", model.FileVolumeName);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else
                    {
                        result = cmd.Parameters["@Result"].Value.ToString(); // error
                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();//Exception
            }

        }
        public SignUp getProfile(int userid)
        {
            SignUp user = new SignUp();
            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spSignUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "GetProfile");
                cmd.Parameters.AddWithValue("@UserId", userid);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    user.UserId = int.Parse(dt.Rows[0]["UserId"].ToString());
                    user.UserName = dt.Rows[0]["UserName"].ToString();
                    user.UserFName = dt.Rows[0]["UserFName"].ToString();
                    user.UserLName = dt.Rows[0]["UserLName"].ToString();
                    user.UserEmail = dt.Rows[0]["UserEmail"].ToString();
                    user.UserMobile = dt.Rows[0]["UserMobile"].ToString();
                }
                return user;
            }
        }
        public int UpdateProfile(SignUp user)
        {
            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spSignUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "updateProfile");
                cmd.Parameters.AddWithValue("@UserId", user.UserId);
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@UserFName", user.UserFName);
                cmd.Parameters.AddWithValue("@UserLName", user.UserLName);
                cmd.Parameters.AddWithValue("@UserEmail", user.UserEmail);
                cmd.Parameters.AddWithValue("@UserMobile", user.UserMobile);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return 1;
                }
                return 0;
            }
        }

        public int UpdatePassword(string Newpwd)
        {
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spSignUp", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "updatePassword");
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Current.Session["UserId"]);
                    cmd.Parameters.AddWithValue("@UserPwd", Encrypt(Newpwd.Trim()));
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        return 1;
                    }
                    else { return 0; }
                    
                }
            }
            catch (Exception epwd) {
                 return 0; }
        }

        public string SignUp(string UserName, string FName, string LName, string Email, string Password, string Mobile)
        {
            string result = "";
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spSignUp", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "SignUp");
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@UserFName", FName);
                    cmd.Parameters.AddWithValue("@UserLName", LName);
                    cmd.Parameters.AddWithValue("@UserEmail", Email);
                    cmd.Parameters.AddWithValue("@UserMobile", Mobile);
                    cmd.Parameters.AddWithValue("@UserPwd", Encrypt(Password.Trim()));
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        result = cmd.Parameters["@Result"].Value.ToString();  //success 
                        return result;
                    }
                    else
                    {
                        result = cmd.Parameters["@Result"].Value.ToString(); // error
                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();//Exception
            }

        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Login(Login users)
        {
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("spSignUp", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "Login");
                    cmd.Parameters.AddWithValue("@UserName", users.UserName);
                    cmd.Parameters.AddWithValue("@UserPwd", Encrypt(users.Password.Trim()));
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Close();
                    if (int.Parse(dt.Rows[0][0].ToString()) != 0)
                    {
                        bool val = Convert.ToBoolean(dt.Rows[0]["IsActiveUser"].ToString());
                        if (val)
                        {
                            HttpContext.Current.Session["UserId"] = dt.Rows[0]["UserId"].ToString();
                            HttpContext.Current.Session["UserName"] = dt.Rows[0]["UserName"].ToString();
                            HttpContext.Current.Session["UserFullName"] = dt.Rows[0]["UserFullName"].ToString();
                            HttpContext.Current.Session["UserRoleType"] = dt.Rows[0]["UserRoleType"].ToString();
                            HttpContext.Current.Session["RoleId"] = dt.Rows[0]["RoleId"].ToString();
                            if (int.Parse(dt.Rows[0]["RoleId"].ToString()) == 1)//1 for Admin page
                            {
                                return "Admin";
                            }
                            else if (int.Parse(dt.Rows[0]["RoleId"].ToString()) == 2)//2 for Record Keeper
                            {
                                return "Record Keeper";
                            }
                            else if (int.Parse(dt.Rows[0]["RoleId"].ToString()) == 3)//2 for Record Keeper
                            {
                                return "User";
                            }
                            else
                            {
                                return "No Such User Exists.";
                            }
                        }
                        else
                        {
                            return "In-Active User Please Contact Admin.";
                        }

                    }
                    else
                    {
                        return "Invalid Username and Password or Role not Assigned Yet.";
                    }
                }

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();//Exception
            }

        }
        
        public int CheckUserNameAvailability(string UserName)
        {
            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spSignUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "chkUnameAvailability");
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string v = dt.Rows[0]["Result"].ToString();
                    if (v == "Available")
                    {
                        return 1;
                    }
                    else if (v == "Not-Available")
                    {
                        return 0;
                    }
                }
                return 0;
            }
        }


        public int CheckUserEmail(string UserEmail)
        {
            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spSignUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "chkUserEmail");
                cmd.Parameters.AddWithValue("@UserEmail", UserEmail);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string v = dt.Rows[0]["Result"].ToString();
                    if (v == "Available")
                    {
                        return 1;
                    }
                    else if (v == "Not-Available")
                    {
                        return 0;
                    }
                }
                return 0;
            }
        }



       




        public string ConfirmUserAction(int UserId, string ActionOnUser)
        {
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spSignUp", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "ConfirmUserAction");
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@Message", ActionOnUser);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    if (cmd.Parameters["@Result"].Value.ToString() == "Activated")
                    {
                        return cmd.Parameters["@Result"].Value.ToString();
                    }
                    else if (cmd.Parameters["@Result"].Value.ToString() == "Deactivated")
                    {
                        return cmd.Parameters["@Result"].Value.ToString();
                    }
                    else if (cmd.Parameters["@Result"].Value.ToString() == "Record Linked")// Data Linked With User
                    {
                        return cmd.Parameters["@Result"].Value.ToString();
                    }
                    else
                    {
                        return "Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return ex.Message.ToString();
            }
        }
        public int CheckPassword(string password)
        {
            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spSignUp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "chkpassword");
                cmd.Parameters.AddWithValue("@UserPwd", Encrypt(password.Trim()));
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string v = dt.Rows[0]["Result"].ToString();
                    if (v == "Matched")
                    {
                        return 1;
                    }
                    else if (v == "Not-Matched")
                    {
                        return 0;
                    }
                }
                return 0;
            }
        }
        public SignUp getActive_Inactive()
        {
            SignUp USERS = new SignUp();
            List<SignUp> listuser = new List<SignUp>();
            DataBaseAccess db = new DataBaseAccess();
            DataTable dt = db.get_DataTable("spSignUp", "GetUsers");
            if (dt.Rows.Count > 0)
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    SignUp users = new SignUp();
                    users.UserId = int.Parse(dt.Rows[i]["UserId"].ToString());
                    users.UserName = dt.Rows[i]["UserName"].ToString();
                    users.UserFName = dt.Rows[i]["UserFName"].ToString();
                    users.UserLName = dt.Rows[i]["UserLName"].ToString();
                    users.UserEmail = dt.Rows[i]["UserEmail"].ToString();
                    users.UserMobile = dt.Rows[i]["UserMobile"].ToString();
                    users.IsActive = bool.Parse(dt.Rows[i]["IsActiveUser"].ToString());
                    listuser.Add(users);
                }
                USERS.User_List = listuser;
            }
            else {
                USERS.UserId = 0;
                listuser.Add(USERS);
                USERS.User_List = listuser;
            }
            return USERS;
        }

        public SignUpRole UsersRoleAssign()
        {
            SignUpRole User_Role = new SignUpRole();
            
            List<SelectListItem> item = new List<SelectListItem>();            

            List<Role> listrole = new List<Role>();
            List<SignUp> listsignup = new List<SignUp>();
            List<SignUpRole> listuserrole = new List<SignUpRole>();
            
            DataBaseAccess db = new DataBaseAccess();
            DataSet ds = db.get_DataSet("spSignUp", "getUserAndRoles");
            if (ds.Tables.Count > 0)
            {               
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Role role = new Role();
                        role.RoleId = int.Parse(ds.Tables[0].Rows[i]["RoleId"].ToString());
                        role.RoleName = ds.Tables[0].Rows[i]["RoleName"].ToString();
                        listrole.Add(role);
                    }
                    User_Role.Roles = listrole;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (var i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        SignUp signup = new SignUp();
                        signup.UserId = int.Parse(ds.Tables[1].Rows[i]["UserId"].ToString());
                        signup.UserName = ds.Tables[1].Rows[i]["UserName"].ToString();                       
                        listsignup.Add(signup);
                    }
                    User_Role.Signups = listsignup;
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (var i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        SignUpRole signuprole = new SignUpRole();
                        signuprole.UserId = int.Parse(ds.Tables[2].Rows[i]["UserId"].ToString());
                        signuprole.UserName = ds.Tables[2].Rows[i]["UserName"].ToString();
                        signuprole.UserEmail = ds.Tables[2].Rows[i]["UserEmail"].ToString();
                        signuprole.RoleId = int.Parse(ds.Tables[2].Rows[i]["RoleId"].ToString());
                        signuprole.RoleName = ds.Tables[2].Rows[i]["RoleName"].ToString();
                        listuserrole.Add(signuprole);
                    }
                    User_Role.SignUps_Roles = listuserrole;
                }
            }
            else
            {
                User_Role.UserId = 0;
                return User_Role;
            }
            return User_Role;
        }
        public string AssignRolestoUsers(SignUpRole model)
        {
            string result = "";
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("spSignUp", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mode", "assignRoletoUser");
                    cmd.Parameters.AddWithValue("@UserId", model.UserId);
                    cmd.Parameters.AddWithValue("@RoleId", model.RoleId);
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        return result="Success";
                    }
                    else
                    {
                        if (cmd.Parameters["@Result"].Value.ToString() == "Duplicate RK")
                        {
                            return result="Duplicate RK";
                        }
                        else if (cmd.Parameters["@Result"].Value.ToString() == "Failed")
                        {
                            return result="Failed";
                        }
                            
                    }                   
                }
            }
            catch (Exception e) { return result = "Failed"; }
            return result;
        }
        public DataTable get_DataTable( string proc,string mode)
        {
            DataTable dt = new DataTable();
            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(proc, con);
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr != null)
                {
                    dt.Load(dr);
                    dr.Close();
                }
                return dt;
            }
          }
        public DataSet get_DataSetWithPar(string proc, string mode,string modepar="",string par="")
        {
            DataSet ds = new DataSet();           
            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(proc, con);
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue(modepar, par);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                if (da != null)
                {
                    da.Fill(ds);
                }
                return ds;
            }
        }

        public DataSet get_DataSet(string proc, string mode)
        {
            DataSet ds = new DataSet();
            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(proc, con);
                cmd.Parameters.AddWithValue("@mode", mode);                
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                if (da != null)
                {
                    da.Fill(ds);
                }
                return ds;
            }
        }

        public DataTable get_LastStatus(string proc, string mode,int FileRecordId, string Message)
        {
            DataTable dt = new DataTable();
            using (con = new SqlConnection(conStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(proc, con);
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@FileRecordId", FileRecordId);
                cmd.Parameters.AddWithValue("@Message", Message);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr != null)
                {
                    dt.Load(dr);
                    dr.Close();
                }
                return dt;
            }
        }
    }
}