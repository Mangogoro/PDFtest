
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UnitTest
{
    public class TestUtility
    {

    }
}


public class TestUtilityQQQQQq
{
	/*
	private string getPDFFile(AlreadyAppEditDetailModel q)
	{
		List<string> htmlist = new List<string>();

		string msg = "";

		int i = 0;

		int colspan = 6;
		int width = 1200;
		int reppage = 0;
		int replines = 20;
		int lineheight = 40;
		double reppages = (double)Math.Ceiling((double)q.ReportAppEditList.Count / Convert.ToDouble(replines));

		string th = "";
		string th1 = "";

		string papersize = "A4";
		string paperorient = "H";
		string f = "";
		string title = "";
		string title1 = "";
		string qrydate1 = "";
		string qrydate2 = "";
		try
		{
			title = "使用者異動事件報表(資料倉儲系統)";
			qrydate1 = q.ReportAppEditList.FirstOrDefault().EventTime.Substring(0, 10);
			qrydate2 = qrydate1;

			title1 = "日期：" + qrydate1;
			i = 0; reppage = 0;

			th = "<center><table cellpadding=0  cellspacing=0 width=" + width.ToString() + " align=center border=0>";
			string logopath = Path.Combine(@"D:\WebSite\img", "logo.jpg");
			th += "<tr>" +
					  "<td width=auto><img src=" + logopath + " alt=Logo style='vertical-align: middle; margin-right: 10px; width: 75%; height: auto;'></td>";
			th += "<td colspan=" + (colspan - 2) + " nowrap width=100% align=left style='font-size:18pt;font-weight:bold;'>" + title + "</td>" +
			  "</tr>";
			th += "<tr><td colspan=" + (colspan) + " nowrap align=center>" + title1 + "</td>";

			th1 = "<table border=1 width='" + width.ToString() + "' cellpadding=1 cellspacing=0 bordercolor=black style='border-collapse: collapse; border: 1px solid black;'>";
			th1 += @"<tr><th nowrap style='text-align:center'>#</th>";

			th1 += @"<th nowrap style='text-align:center'>事件</th>
							<th nowrap style='text-align:center'>事件時間</th>
							<th nowrap style='text-align:center'>使用者</th>
							<th nowrap style='text-align:center'>用戶端</th>
							<th nowrap style='text-align:center'>應用程式</th>
							<th nowrap style='text-align:center'>目標物件</th>
							<th nowrap style='text-align:center'>指令</th>";
			colspan = 10;


			th1 += @"<th nowrap style='text-align:center'>事件日期</th>";
			th1 += @"<th nowrap style='text-align:center'>簽核說明</th>";



			th1 += @"</tr>";

			#region 查無資料
			if (q.ReportAppEditList.Count == 0)
			{
				string tabletitle0 = th;

				tabletitle0 += "<tr><td nowrap>表號：DWN000" + 2 + "</td><td colspan=" + (colspan - 1) + " nowrap align=right>製表日期：" +
					String.Format("{0:s}", DateTime.Now).ToString().Substring(0, 10);
				tabletitle0 += "</td></tr>";

				msg = tabletitle0 + "</table>" + th1;
				msg += "<tr><td colspan='" + colspan.ToString() + "'>查　無　紀　錄</td></tr>";

				htmlist.Add(msg);

			}
			#endregion 查無資料
			else
			{

				foreach (var data in q.ReportAppEditList)
				{
					msg = "";

					if (i == 0 || i % replines == 0)
					{
						if (reppage++ > 0)
						{
							msg += "</table><div style='page-break-after:always'>&nbsp;</div>";
						}
						msg += th;
						msg += "<tr><td nowrap>表號：DWN000" + 2 + "</td><td colspan=" + (colspan - 1) + " nowrap align=right>頁數 " + reppage.ToString() + " 總頁數 " + reppages.ToString() + " 製表日期：" + String.Format("{0:s}", DateTime.Now).ToString().Substring(0, 10) + " </td></tr>";
						msg += "</table>";
						msg += th1;
					}
					i++;
					msg += "<tr height=" + lineheight.ToString() + "> ";
					msg += "<td nowrap style=\"text-align:right;\">" + i.ToString() + ".</td>";
					msg += @"<td>" + data.Event + @"</td>
										<td nowrap style='text-align:center'>" + data.EventTime.ToString().Replace(" ", "<br>") + @"</td>
										<td nowrap style='text-align:center'>" + data.UserID + @"</td>
										<td nowrap style='text-align:center'>" + data.Client + @"</td>
										<td nowrap style='text-align:center'>" + data.Rtn + @"</td>
										<td nowrap style='text-align:center'>" + data.DstObj + @"</td>
										<td>" + getMsgLen(data.Command, 100) + @"</td>";
					msg += @"<td nowrap style='text-align:center'>" + data.EventTime.Substring(0, 10) + @"</td>";

					msg += @"<td >" + data.Remark + @"</td>";

					msg += "</tr>";

					htmlist.Add(msg);

				}
			}


			htmlist.Add("</table>");


			Dictionary<string, string> aSIGN = new Dictionary<string, string>();
			aSIGN["DW_DWPGP"] = "";
			aSIGN["DW_DWPCH1"] = "";
			aSIGN["DW_DWPCH2"] = "";
			aSIGN["DW_DWPCH3"] = "";
			htmlist.Add("<table width='" + width.ToString() + "'><tr>"
					+ @"<td nowrap height=50>經辦：</td><td nowrap width=20%>" +
					q.AuditHis.Where(x => x.Status == "新申請").OrderByDescending(x => x.UpdateTime).FirstOrDefault().Name + "<br>" +
					q.AuditHis.Where(x => x.Status == "新申請").OrderByDescending(x => x.UpdateTime).FirstOrDefault().UpdateTime
					+ @"</td><td nowrap>覆核：</td><td nowrap width=20%>" +
					q.AuditHis.Where(x => x.Status == "第1階段審核通過").OrderByDescending(x => x.UpdateTime).FirstOrDefault().Inputter + "<br>" +
					q.AuditHis.Where(x => x.Status == "第1階段審核通過").OrderByDescending(x => x.UpdateTime).FirstOrDefault().UpdateTime
					+ @"</td><td nowrap>主管核章：</td><td nowrap width=20%>" +
					q.AuditHis.Where(x => x.Status == "第2階段審核通過").OrderByDescending(x => x.UpdateTime).FirstOrDefault().Inputter + "<br>" +
					q.AuditHis.Where(x => x.Status == "第2階段審核通過").OrderByDescending(x => x.UpdateTime).FirstOrDefault().UpdateTime
					+ @"</td><td nowrap width=20%>" +
					q.AuditHis.Where(x => x.Status == "申請已生效").OrderByDescending(x => x.UpdateTime).FirstOrDefault().Inputter + "<br>" +
					q.AuditHis.Where(x => x.Status == "申請已生效").OrderByDescending(x => x.UpdateTime).FirstOrDefault().UpdateTime
					+ @"</td></table>");



			string htmlcode = string.Join(Environment.NewLine, htmlist.ToArray());

			UrlHelper Url = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
			//watermark = empid + ".png";



			string fileName = q.ReportAppEditList.FirstOrDefault().RefID + ".pdf";
			f = Path.Combine(@"D:\WebSite\" + archivesPath, fileName);
			htmlcode = @"<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
						<style><!--p,tr,td,a {font-family:標楷體;font-size:11pt}--></style>
						<body background='" + Url.Content("~/") + "CommonLib/images/logow.png'>" + htmlcode + "</body>";

			if (!wkhtmltopdf(htmlcode, f, paperorient, papersize, 5, 5, 5, 5))
			{
				f = "wkhtmltopdf Error";
			}

		}
		catch (Exception ex)
		{
			AddSystemLog(ex.ToString());
		}
		return f;
	}


	public Boolean wkhtmltopdf(string htmlcode, string pdffile, string pageorientation = "V", string pagesize = "A4", float top = 10, float bottom = 10, float left = 10, float right = 10)
	{
		Boolean ret = false;
		string htmlfile = pdffile.Replace(".pdf", ".html");
		try
		{
			File.WriteAllText(htmlfile, htmlcode, Encoding.UTF8);
		}
		catch (Exception ex)
		{
			log.Debug("wkhtmltopdf " + ex.ToString());
			return false;
		}

		string orientation = (pageorientation.Equals("V")) ? "Portrait" : "Landscape";
		string binpath = HttpContext.Current.Server.MapPath("~/bin/");
		string cmd = binpath + @"wkhtmltopdf";
		string option = " --quiet --page-size " + pagesize + " --orientation " + orientation + " -B " + bottom.ToString() + " -L " + left.ToString() + " -R " + right.ToString() + " -T " + top.ToString();
		log.Debug(cmd + " " + option + " " + htmlfile + " " + pdffile);

		System.Diagnostics.Process oProcess = new System.Diagnostics.Process()
		{
			StartInfo = new System.Diagnostics.ProcessStartInfo()
			{
				FileName = cmd,
				Arguments = option + " " + htmlfile + " " + pdffile,
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			},
			EnableRaisingEvents = true,
		};
		//委派一個EventHandler
		oProcess.Exited += (s, e) =>
		{
			System.Diagnostics.Process oTemp = (System.Diagnostics.Process)s;
			oTemp.Close();
		};
		//開始執行
		oProcess.Start();
		string output = oProcess.StandardOutput.ReadToEnd();
		oProcess.WaitForExit();
		//System.Diagnostics.Process.Start(cmd, option + " " + htmlfile + " " + pdffile);
		//System.Threading.Thread.Sleep(5000);

		//System.Diagnostics.Process oProcess = new System.Diagnostics.Process();
		//oProcess.Start(cmd, option + " " + htmlfile + " " + pdffile);
		////System.Threading.Thread.Sleep(5000);

		long length = new System.IO.FileInfo(pdffile).Length;

		int sec = 0;
		while ((!fileExist(pdffile) || length == 0) && sec < 10)
		{
			sec++;
			length = new System.IO.FileInfo(pdffile).Length;
			log.Debug(pdffile + " size=" + length.ToString() + " is not exist or size = 0 sec=" + sec.ToString());
			System.Threading.Thread.Sleep(5000);
		}
		log.Debug(pdffile + " Create Finish ");

		//logger.Debug(pdffile + " size=" + length.ToString() + " sec=" + sec.ToString());
		if (fileExist(pdffile)) ret = true;
		try
		{
			//System.Threading.Thread.Sleep(5000);
			//File.Delete(htmlfile);
		}
		catch { }
		return ret;
	}
	public string getMsgLen(string str, int n)
	{

		if (str.Length > n)
		{
			str = str.Substring(0, n) + "...";
		}
		return str;
	}
	public Boolean fileExist(string f)
	{
		FileInfo fi = new FileInfo(f);
		Boolean ret = false;
		if (fi.Exists)
		{
			ret = true;
		}
		return ret;
	}
	*/
}