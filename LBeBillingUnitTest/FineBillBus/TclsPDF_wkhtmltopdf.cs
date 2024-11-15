using System;
using FineBillBus;
using NUnit.Framework;
using System.Runtime.Caching;
using System.Collections.Generic;
using System.Linq;
using FineBillBus.Jobs;
using System.IO;
using System.Text;
using System.Web;
using System.Diagnostics;

namespace UnitTest
{
    [TestFixture()]
    public class TclsPDF_wkhtmltopdf
    {

        //
        [TestCase()]
        public void PDF測試_ConvertHTMLToPDF()
        {
			////產生PDF資料
			byte[] oPdfFile = ConvertHTMLToPDF(shtmlText, "123", "");

			if (oPdfFile == null)
			{
				Assert.IsTrue(false);
				return;
			}


			//寫入磁碟
			string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\..\_output\{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            Console.WriteLine(outputPath);
			File.WriteAllBytes(outputPath, oPdfFile);

            Assert.IsTrue(true);
        }

		/// <summary>
		/// 將HTML內容轉換為PDF並回傳PDF的位元組陣列
		/// </summary>
		/// <param name="htmlText">HTML內容字串</param>
		/// <param name="userPassword">使用者密碼</param>
		/// <param name="ownerPassword">擁有者密碼</param>
		/// <returns>PDF的位元組陣列</returns>
		public byte[] ConvertHTMLToPDF(string htmlText, string userPassword, string ownerPassword)
		{
			try
			{
				// 檢查HTML內容是否為空，若為空則直接回傳null
				if (string.IsNullOrEmpty(htmlText))
				{
					return null;
				}

				// 定義HTML和PDF的儲存路徑
				string tempDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\_output");
				if (!Directory.Exists(tempDirectory))
				{
					Directory.CreateDirectory(tempDirectory); // 若目錄不存在則建立
				}

				string pdfFilePath = Path.Combine(tempDirectory, $"{DateTime.Now:yyyyMMdd_HHmmss}.pdf"); // 儲存PDF檔案的路徑
				string htmlFilePath = pdfFilePath.Replace(".pdf", ".html"); // 將HTML文件與PDF文件存放於相同目錄

				// 將HTML內容存為檔案
				File.WriteAllText(htmlFilePath, htmlText, Encoding.UTF8);

				// 定義wkhtmltopdf相關參數
				string orientation = "Portrait"; // 頁面方向：縱向
				string pageSize = "A4";          // 頁面大小：A4
				float marginTop = 10, marginBottom = 10, marginLeft = 10, marginRight = 10; // 頁面邊距

				// 設定wkhtmltopdf執行檔的位置，通常放在bin目錄
				string binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\ServerCOM\wkhtmltopdf");
				string cmd = Path.Combine(binPath, "wkhtmltopdf");
				string options = $"--quiet --page-size {pageSize} --orientation {orientation} -B {marginBottom} -L {marginLeft} -R {marginRight} -T {marginTop}";

				// 初始化Process來執行wkhtmltopdf
				Process process = new Process
				{
					StartInfo = new ProcessStartInfo
					{
						FileName = cmd, // wkhtmltopdf執行檔的路徑
						Arguments = $"{options} \"{htmlFilePath}\" \"{pdfFilePath}\"", // 命令行參數
						UseShellExecute = false,    // 不使用外殼啟動
						CreateNoWindow = true,      // 不顯示執行視窗
						RedirectStandardOutput = true, // 重新導向標準輸出
						RedirectStandardError = true  // 重新導向錯誤輸出
					}
				};

				// 開始執行wkhtmltopdf
				process.Start();
				string output = process.StandardOutput.ReadToEnd(); // 讀取標準輸出的內容
				string error = process.StandardError.ReadToEnd();   // 讀取錯誤輸出的內容
				process.WaitForExit(); // 等待命令執行完成

				// 若wkhtmltopdf執行失敗或PDF文件未生成，則拋出例外
				if (process.ExitCode != 0 || !File.Exists(pdfFilePath))
				{
					throw new Exception($"wkhtmltopdf執行失敗，錯誤訊息: {error}");
				}

				// 讀取生成的PDF檔案為byte[]陣列
				byte[] pdfBytes = File.ReadAllBytes(pdfFilePath);

				// 清理臨時檔案
				File.Delete(htmlFilePath);

				// 回傳PDF的位元組陣列
				return pdfBytes;
			}
			catch (Exception ex)
			{
				// 錯誤處理，記錄錯誤訊息
				Console.WriteLine($"產生PDF失敗，錯誤訊息: {ex}");
				return null;
			}
		}


		//public bool wkhtmltopdf(string htmlcode, string pdffile, string pageorientation = "V", string pagesize = "A4", float top = 10, float bottom = 10, float left = 10, float right = 10)
		//{
		//	bool ret = false;
		//	string htmlfile = pdffile.Replace(".pdf", ".html");
		//	try
		//	{
		//		File.WriteAllText(htmlfile, htmlcode, Encoding.UTF8);
		//	}
		//	catch (Exception ex)
		//	{
		//		Console.WriteLine("wkhtmltopdf " + ex.ToString());
		//		return false;
		//	}

		//	string orientation = (pageorientation.Equals("V")) ? "Portrait" : "Landscape";
		//	string binpath = HttpContext.Current.Server.MapPath("~/bin/");
		//	string cmd = binpath + @"wkhtmltopdf";
		//	string option = " --quiet --page-size " + pagesize + " --orientation " + orientation + " -B " + bottom.ToString() + " -L " + left.ToString() + " -R " + right.ToString() + " -T " + top.ToString();
		//	Console.WriteLine(cmd + " " + option + " " + htmlfile + " " + pdffile);

		//	System.Diagnostics.Process oProcess = new System.Diagnostics.Process()
		//	{
		//		StartInfo = new System.Diagnostics.ProcessStartInfo()
		//		{
		//			FileName = cmd,
		//			Arguments = option + " " + htmlfile + " " + pdffile,
		//			UseShellExecute = false,
		//			CreateNoWindow = true,
		//			RedirectStandardOutput = true,
		//			RedirectStandardError = true
		//		},
		//		EnableRaisingEvents = true,
		//	};
		//	//委派一個EventHandler
		//	oProcess.Exited += (s, e) =>
		//	{
		//		System.Diagnostics.Process oTemp = (System.Diagnostics.Process)s;
		//		oTemp.Close();
		//	};
		//	//開始執行
		//	oProcess.Start();
		//	string output = oProcess.StandardOutput.ReadToEnd();
		//	oProcess.WaitForExit();


		//	long length = new System.IO.FileInfo(pdffile).Length;

		//	int sec = 0;
		//	while ((!fileExist(pdffile) || length == 0) && sec < 10)
		//	{
		//		sec++;
		//		length = new System.IO.FileInfo(pdffile).Length;
		//		Console.WriteLine(pdffile + " size=" + length.ToString() + " is not exist or size = 0 sec=" + sec.ToString());
		//		System.Threading.Thread.Sleep(5000);
		//	}
		//	Console.WriteLine(pdffile + " Create Finish ");

		//	//logger.Debug(pdffile + " size=" + length.ToString() + " sec=" + sec.ToString());
		//	if (fileExist(pdffile)) ret = true;
		//	try
		//	{
		//		//System.Threading.Thread.Sleep(5000);
		//		//File.Delete(htmlfile);
		//	}
		//	catch { }
		//	return ret;
		//}


		//public bool fileExist(string f)
		//{
		//	FileInfo fi = new FileInfo(f);
		//	bool ret = false;
		//	if (fi.Exists)
		//	{
		//		ret = true;
		//	}
		//	return ret;
		//}



















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
