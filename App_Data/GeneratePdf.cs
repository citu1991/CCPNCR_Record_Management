using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Web;
using System.Data;
using System.IO;
using System.Web.Mvc;

namespace CCPNCR_Record_Management.App_Data
{
    [HandleError]
    public class GeneratePdf
    {
        public void ExportToPdf(DataTable myDataTable, string message)
        {
            string PDFHeader = "";
            String CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
            DataTable dt = myDataTable;
            PdfPTable PdfTable = new PdfPTable(dt.Columns.Count);
            PdfPCell PdfPCell = null;
            Font font9 = FontFactory.GetFont("ARIAL", 10);
            if (message == "EXCELAllFILE" || message == "PDFAllFILE")
            {
                PDFHeader = "Report of All File  [ Dated: " + CurrentDate + " ]";
                PdfTable.AddCell(new Phrase("Sr.No"));
                PdfTable.AddCell(new Phrase("File Type"));
                PdfTable.AddCell(new Phrase("File No-Volume"));
                PdfTable.AddCell(new Phrase("File Subject"));
                PdfTable.AddCell(new Phrase("NP Start"));
                PdfTable.AddCell(new Phrase("NP End"));
                PdfTable.AddCell(new Phrase("CP Start"));
                PdfTable.AddCell(new Phrase("CP End"));
                PdfTable.AddCell(new Phrase("Date on Last NP"));
                PdfTable.AddCell(new Phrase("Last NP Signed By"));

            }
            else if (message == "EXCELAllINSIDEFILE" || message == "PDFAllINSIDEFILE")
            {
                PDFHeader = "Report of All File Inside  [ Dated: " + CurrentDate + " ]";
                PdfTable.AddCell(new Phrase("Sr.No"));
                PdfTable.AddCell(new Phrase("Volume-File No"));
                PdfTable.AddCell(new Phrase("File Subject"));
                PdfTable.AddCell(new Phrase("NP Start"));
                PdfTable.AddCell(new Phrase("NP End"));
                PdfTable.AddCell(new Phrase("CP Start"));
                PdfTable.AddCell(new Phrase("CP End"));
                PdfTable.AddCell(new Phrase("Date on Last NP"));
                PdfTable.AddCell(new Phrase("Last NP Signed By"));
            }
            else if (message == "EXCELAllOUTSIDEFILE" || message == "PDFAllOUTSIDEFILE")
            {
                PDFHeader = "Report of All File Outside  [ Dated: " + CurrentDate + " ]";
                PdfTable.AddCell(new Phrase("Sr.No"));
                PdfTable.AddCell(new Phrase("Volume-File No"));
                PdfTable.AddCell(new Phrase("File Subject"));
                PdfTable.AddCell(new Phrase("NP Start"));
                PdfTable.AddCell(new Phrase("NP End"));
                PdfTable.AddCell(new Phrase("CP Start"));
                PdfTable.AddCell(new Phrase("CP End"));
                PdfTable.AddCell(new Phrase("Date on Last NP"));
                PdfTable.AddCell(new Phrase("Last NP Signed By"));
                PdfTable.AddCell(new Phrase("Currently with"));
                PdfTable.AddCell(new Phrase("From Date"));
            }
            else if (message == "EXCELAllFILESENTFORAPPROVAL" || message == "PDFAllFILEENTFORAPPROVAL")
            {
                PDFHeader = "Report of All File Sent For Approval  [ Dated: " + CurrentDate + " ]";
                PdfTable.AddCell(new Phrase("Sr.No"));
                PdfTable.AddCell(new Phrase("Approval Sent to"));
                PdfTable.AddCell(new Phrase("Approval Sent Date"));
                PdfTable.AddCell(new Phrase("File No-Volume"));

                PdfTable.AddCell(new Phrase("File Subject"));
                PdfTable.AddCell(new Phrase("NP Start"));
                PdfTable.AddCell(new Phrase("NP End"));
                PdfTable.AddCell(new Phrase("CP Start"));
                PdfTable.AddCell(new Phrase("CP End"));
                PdfTable.AddCell(new Phrase("Date on Last NP"));
                PdfTable.AddCell(new Phrase("Last NP Signed By"));
            }
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 5, 5, 10, 5);
            
            try
            {
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();
                Chunk c = new Chunk(PDFHeader, FontFactory.GetFont("Verdana", 11));
                Paragraph p = new Paragraph();
                p.Alignment = Element.ALIGN_CENTER;
                p.Add(c);
                pdfDoc.Add(p);
                pdfDoc.Add(new Chunk("\n"));

                
                
                if (dt != null)
                {
                    //Craete instance of the pdf table and set the number of column in that table  
                                       
                    for (int rows = 0; rows < dt.Rows.Count; rows++)
                    {
                        for (int column = 0; column < dt.Columns.Count; column++)
                        {
                            PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font9)));
                            PdfTable.AddCell(PdfPCell);
                        }
                    }
                    //PdfTable.SpacingBefore = 15f; // Give some space after the text or it may overlap the table            
                    pdfDoc.Add(PdfTable); // add pdf table to the document   
                }
                
                pdfDoc.Close();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename= ReportPdf_"+ CurrentDate + "_.pdf");
                System.Web.HttpContext.Current.Response.Write(pdfDoc);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();  
                
            }
            catch (DocumentException de)
            {
                System.Web.HttpContext.Current.Response.Write(de.Message);
            }
            catch (IOException ioEx)
            {
                System.Web.HttpContext.Current.Response.Write(ioEx.Message);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
        }
    }
}
