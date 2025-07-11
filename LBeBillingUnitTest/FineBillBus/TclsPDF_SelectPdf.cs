﻿using System;
using FineBillBus;
using NUnit.Framework;
using System.Runtime.Caching;
using System.Collections.Generic;
using System.Linq;
using FineBillBus.Jobs;
using System.IO;
using SelectPdf;

namespace UnitTest
{
    [TestFixture()]
    public class TclsPDF_SelectPdf
    {


        [TestCase()]
        public void PDF測試_ConvertHTMLToPDF()
        {


            HtmlToPdf toPdf = new HtmlToPdf();
            toPdf.Options.PdfPageSize = PdfPageSize.A4;
            toPdf.Options.MarginRight = 1;
            toPdf.Options.MarginLeft = 1;
            toPdf.Options.WebPageWidth += 200;
            PdfDocument pdf = toPdf.ConvertHtmlString(shtmlText);

            //寫入磁碟
            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\..\_output\{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            pdf.Save(outputPath);
            pdf.Close();


            Assert.IsTrue(true);
        }




        string shtmlText = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"" >
<html>
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=big5"" //>
<title>qryBillFormForNotify</title>
</head>
<body  >
<form method=""post"" action=""./qryBillFormForNotify.aspx?UFD_PKey=10107&amp;UploadCode=AP&amp;MasterFilePKey=260&amp;DATAPKEY=260&amp;UFMPKey=1735&amp;SPPKey=73218&amp;ULPKey=10087&amp;NotifyType=PDF&amp;PDFNotice=1&amp;PermID=99999568"" id=""frmBillFormForNotify"">
<input type=""hidden"" name=""__VIEWSTATE"" id=""__VIEWSTATE"" value=""/wEPDwUKMTg2NDAzMDAxOGRkD3FFOKlIf0zEFPzsLn3axxBT9ls="" />
<input type=""hidden"" name=""__VIEWSTATEGENERATOR"" id=""__VIEWSTATEGENERATOR"" value=""5E72EE3D"" />
<div id=""divBillContent""><font face=""新細明體""></font>&nbsp;<table cellspacing=""1"" cellpadding=""0"" style=""height:300px;width:100%;border-style:Solid;border-width:1px;"">
<tr>
<td align=""center"" valign=""middle"" Coordinates=""0-0"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-0"" style=""height:0%;WUnit:%;width:5%;HUnit:%;""></td><td align=""center"" valign=""middle"" Coordinates=""2-0"" style=""height:0%;WUnit:%;width:35%;HUnit:%;""></td><td align=""center"" valign=""middle"" Coordinates=""3-0"" style=""height:0%;WUnit:%;width:20%;HUnit:%;""></td><td align=""center"" valign=""middle"" Coordinates=""4-0"" style=""height:0%;WUnit:%;width:35%;HUnit:%;""></td><td align=""center"" valign=""middle"" Coordinates=""5-0"" style=""height:0%;WUnit:%;width:5%;HUnit:%;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-1"" style=""height:20;WUnit:;width:0;HUnit:;""></td><td align=""center"" Coordinates=""1-1"" colspan=""5"" rowspan=""1"" style=""height:0;WUnit:;width:0;HUnit:;""><span style=""FONT-SIZE:32px;""><b><u><font face=""細明體"">電金部測試1</font></u></b></span></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-2"" style=""height:20;WUnit:;width:0;HUnit:;""></td><td align=""center"" Coordinates=""1-2"" colspan=""5"" rowspan=""1"" style=""height:0;WUnit:;width:0;HUnit:;""><span style=""FONT-SIZE:18px;""><b><font face=""細明體"">帳款通知單</font></b></span></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-3"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-3"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""2-3"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""3-3"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""4-3"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""5-3"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-4"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-4"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""left"" Coordinates=""2-4"" style=""height:0;WUnit:;width:0;HUnit:;""><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">通知日期：</font></b></span><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">民國113年11月13日</font></b></span></td><td align=""center"" valign=""middle"" Coordinates=""3-4"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""4-4"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""5-4"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-5"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-5"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""left"" Coordinates=""2-5"" style=""height:0;WUnit:;width:0;HUnit:;""><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">收款人統編：</font></b></span><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">99****68</font></b></span></td><td align=""center"" valign=""middle"" Coordinates=""3-5"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""4-5"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""5-5"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-6"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-6"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""left"" Coordinates=""2-6"" style=""height:0;WUnit:;width:0;HUnit:;""><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">收款人匯款戶名：</font></b></span><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">戶名1</font></b></span></td><td align=""center"" valign=""middle"" Coordinates=""3-6"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""left"" Coordinates=""4-6"" style=""height:0;WUnit:;width:0;HUnit:;""><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">收款人名稱：</font></b></span><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">使用者1</font></b></span></td><td align=""center"" valign=""middle"" Coordinates=""5-6"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-7"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-7"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""left"" Coordinates=""2-7"" style=""height:0;WUnit:;width:0;HUnit:;""><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">匯入銀行代碼：</font></b></span><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">0050027</font></b></span></td><td align=""center"" valign=""middle"" Coordinates=""3-7"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""left"" Coordinates=""4-7"" style=""height:0;WUnit:;width:0;HUnit:;""><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">匯入帳號：</font></b></span><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">016001****27</font></b></span></td><td align=""center"" valign=""middle"" Coordinates=""5-7"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-8"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-8"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""left"" Coordinates=""2-8"" style=""height:0;WUnit:;width:0;HUnit:;""><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">帳款金額：</font></b></span><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">$3,001</font></b></span></td><td align=""center"" valign=""middle"" Coordinates=""3-8"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""left"" Coordinates=""4-8"" style=""height:0;WUnit:;width:0;HUnit:;""><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">匯款日：</font></b></span><span style=""FONT-SIZE:14px;""><b><font face=""細明體"">民國113年11月14日</font></b></span></td><td align=""center"" valign=""middle"" Coordinates=""5-8"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-9"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-9"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""2-9"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""3-9"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""4-9"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""5-9"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-10"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-10"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""2-10"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""3-10"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""4-10"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""5-10"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-11"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-11"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""2-11"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""3-11"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""4-11"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""5-11"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-12"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-12"" colspan=""5"" rowspan=""1"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-13"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-13"" colspan=""5"" rowspan=""1"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-14"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-14"" colspan=""5"" rowspan=""1"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-15"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-15"" colspan=""5"" rowspan=""1"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr><tr>
<td align=""center"" valign=""middle"" Coordinates=""0-16"" style=""height:0;WUnit:;width:0;HUnit:;""></td><td align=""center"" valign=""middle"" Coordinates=""1-16"" colspan=""5"" rowspan=""1"" style=""height:0;WUnit:;width:0;HUnit:;""></td>
</tr>
</table></div>
</form>
</body>
</html>";

    }
}
