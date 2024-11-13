using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.rtf.document;
using iTextSharp.text.html.simpleparser;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace FineBillBus.Jobs
{

    public class clsPDF
    {
        #region [定義變數]

        private static NLog.Logger NLogger = NLog.LogManager.GetCurrentClassLogger();

        Document oPDFDocument = new Document(PageSize.A4.Rotate());

        #endregion [定義變數]



        #region [Method]

        /// <summary>
        /// 自URL取得HTML並輸出PDF內容
        /// </summary>
        /// <param name="URL">產生PDF用網址</param>
        /// <param name="UserPassword">使用者密碼</param>
        /// <param name="OwnerPassword">管理者密碼</param>
        /// <returns></returns>
        public byte[] ConvertURLToPDF(string URL, string UserPassword, string OwnerPassword)
        {
            try
            {
                // [未傳入URL時回傳空值]

                if (URL == "")
                {
                    return null;
                }


                // [取HTML]

                string shtmlText = getHtmlfromUrl(URL);


                // [產生PDF內容]

                return ConvertHTMLToPDF(shtmlText, UserPassword, OwnerPassword);

            }
            catch (Exception ex)
            {
                // [錯誤處理]

                NLogger.Error("PDF產生失敗，錯誤訊息:" + ex.ToString());

                return null;
            }
        }

        /// <summary>
        /// 自HTML輸出PDF內容
        /// </summary>
        /// <param name="htmlText">產生PDF用HTML</param>
        /// <param name="UserPassword">使用者密碼</param>
        /// <param name="OwnerPassword">管理者密碼</param>
        /// <returns></returns>
        public byte[] ConvertHTMLToPDF(string htmlText, string UserPassword, string OwnerPassword)
        {
            try
            {
                // [未傳入HTML時回傳空值]
                if (string.IsNullOrEmpty(htmlText))
                {
                    return null;
                }

                //NLogger.Info($"old htmlText:{htmlText}");
                // 預處理 HTML
                htmlText = PreprocessHtml(htmlText);

//                NLogger.Info(@"

//");
                //NLogger.Info($"new htmlText:{htmlText}");

                // [產生PDF內容]
                Document oPDFDoc = new Document();//要寫PDF的文件，建構子沒填的話預設直式A4

                using (Common.Memory_Stream msPDFStream = new Common.Memory_Stream())
                {
                    PdfWriter oPdfWriter = PdfWriter.GetInstance(oPDFDoc, msPDFStream);

                    //指定文件預設開檔時的縮放為100%
                    PdfDestination oPDFDest = new PdfDestination(PdfDestination.XYZ, 0, oPDFDoc.PageSize.Height, 1f);

                    //// [有設定密碼時，則將密碼加入PDF檔中]
                    //if (!string.IsNullOrEmpty(UserPassword))
                    //{
                    //    //OwnerPassword為空值時則帶入UserPassword
                    //byte[] userPwd = System.Text.Encoding.ASCII.GetBytes(UserPassword);
                    //byte[] ownerPwd = string.IsNullOrEmpty(OwnerPassword)
                    //    ? userPwd
                    //    : System.Text.Encoding.ASCII.GetBytes(OwnerPassword);

                    //oPdfWriter.SetEncryption(userPwd,
                    //                        ownerPwd,
                    //                        PdfWriter.ALLOW_SCREENREADERS,
                    //                        PdfWriter.STRENGTH40BITS);
                    //}

                    //開啟Document文件 
                    oPDFDoc.Open();

                    // 使用 HTMLWorker 處理 HTML (替代 XMLWorkerHelper)
                    StyleSheet styles = new StyleSheet();
                    styles.LoadTagStyle("body", "encoding", "UTF-8");

                    HTMLWorker htmlWorker = new HTMLWorker(oPDFDoc);
                    using (StringReader sr = new StringReader(htmlText))
                    {
                        htmlWorker.Parse(sr);
                    }

                        //將pdfDest設定的資料寫到PDF檔
                        PdfAction action = PdfAction.GotoLocalPage(1, oPDFDest, oPdfWriter);
                        oPdfWriter.SetOpenAction(action);

                        oPDFDoc.Close();

                        //回傳PDF檔案
                        return msPDFStream.ToArray();
                    }
                }
            catch (Exception ex)
            {
                // [錯誤處理]
                NLogger.Error("產生PDF失敗，錯誤訊息:" + ex.ToString());

                //NLogger.Error($"htmlText:{htmlText}");

                return null;
            }
        }


        private string PreprocessHtml(string html)
        {
            // 移除 DOCTYPE
            //html = Regex.Replace(html, "<!DOCTYPE[^>]*>", "", RegexOptions.IgnoreCase);

            // 修正 meta 標籤
            html = Regex.Replace(html, @"<meta[^>]*/>", "", RegexOptions.IgnoreCase);

            //// 移除 form 標籤和隱藏欄位
            //html = Regex.Replace(html, @"<form[^>]*>", "", RegexOptions.IgnoreCase);
            //html = Regex.Replace(html, @"</form>", "", RegexOptions.IgnoreCase);
            //html = Regex.Replace(html, @"<input[^>]*>", "", RegexOptions.IgnoreCase);

            //// 清理空的 style 屬性
            //html = Regex.Replace(html, @"style=\""[^""]*WUnit:[^""]*HUnit:[^""]*\""", "", RegexOptions.IgnoreCase);

            //// 移除 Coordinates 屬性
            //html = Regex.Replace(html, @"Coordinates=\""[^""]*\""", "", RegexOptions.IgnoreCase);

            //// 修正表格寬度和高度
            //html = Regex.Replace(html, @"style=""height:0;", @"style=""height:auto;", RegexOptions.IgnoreCase);
            //html = Regex.Replace(html, @"style=""width:0;", @"style=""width:auto;", RegexOptions.IgnoreCase);

            //// 確保表格有正確的寬度
            //html = html.Replace(@"style=""height:300px;width:100%;", @"style=""width:100%;");

            return html;
        }


        /// <summary>
        /// 將Html文字 輸出到PDF檔裡
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public byte[] ConvertHtmlTextToPDF(string htmlText)
        {
            using (var memoryStream = new MemoryStream())
            {
                CreateFromHtml_Image(htmlText).CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 將HTML轉成PDF，並處理base64的圖片
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public Stream CreateFromHtml_Image(string html)
        {
            var stream = new MemoryStream();
            var doc = new Document(PageSize.A4.Rotate());
            var ms = new MemoryStream();

            try
                    {
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                        writer.CloseStream = false;
                        doc.Open();

                // 將 HTML 轉為圖片的處理
                // 假設已將 HTML 轉換為圖像，將圖像加入 PDF 中
                // 此處需自行處理 HTML 轉圖像的實作或考慮嵌入其他解決方案

                            doc.Close();
                            ms.Position = 0;
                            ms.CopyTo(stream);
                            stream.Position = 0;
                        }
            finally
            {
                doc?.Close();
                ms?.Close();
            }

            return stream;
        }




        /// <summary>
        /// 欲取得HTML的網址
        /// </summary>
        /// <param name="URL"></param>
        /// <returns>取得的HTML內容</returns>
        private string getHtmlfromUrl(string URL)
        {
            #region [連線至URL取HTML並過濾內容]

            try
            {
                #region [未傳入URL時回傳空值]

                if (URL == "")
                {
                    return "";
                }

                #endregion [未傳入URL時回傳空值]

                #region [讀取URL內容]

                string shtmlText = getWebContent(URL);

                #endregion [讀取URL內容]

                #region [碰到<meta節點須將結尾加上/>結束符號，避免itextsharp錯誤]

                string[] ContentLines = shtmlText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries); //忽略空行

                StringBuilder sbTempAll = new StringBuilder();

                for (int i = 0; i < ContentLines.Length; i++)
                {
                    string sTempLine = ContentLines[i].Trim();

                    if (sTempLine.Contains("<meta"))
                    {
                        Console.WriteLine(i + "]]]:" + sTempLine);
                        sTempLine = sTempLine.Replace(">", "/>");
                    }

                    //if (sTempLine.Contains("<img"))
                    //{
                    //    Console.WriteLine(i + "]]]:" + sTempLine);
                    //    sTempLine = sTempLine.Replace(">", "/>");
                    //}

                    sbTempAll.AppendLine(sTempLine);
                }

                #endregion [碰到<meta節點須將結尾加上/>結束符號，避免itextsharp錯誤]

                return sbTempAll.ToString();
            }
            catch (WebException we)
            {
                #region [錯誤處理]

                NLogger.Error("取得網址內容錯誤，錯誤訊息:" + we.ToString());

                return "";

                #endregion [錯誤處理]
            }
            catch (Exception ex)
            {
                #region [錯誤處理]

                NLogger.Error("程式發生錯誤，錯誤訊息:" + ex.ToString());

                return "";

                #endregion [錯誤處理]
            }

            #endregion [連線至URL取HTML並過濾內容]
        }

        /// <summary>
        /// 取得網站內容(包含HTML, CSS, JavaScript)
        /// </summary>
        /// <returns>網站內容</returns>
        private string getWebContent(string _sUrl)
        {
            try
            {
                WebClient oWebClient = new WebClient();
                //獲取或設置用於取得Internet資料時進行身分驗證的憑證
                oWebClient.Credentials = CredentialCache.DefaultCredentials;
                //從指定網址取得網站內容
                Byte[] pageData = oWebClient.DownloadData(_sUrl);

                #region [判斷讀入字串編碼]

                Stream sTmpPageData = new MemoryStream(pageData);

                StreamReader sr = new StreamReader(sTmpPageData);
                //由Stream讀取編碼
                string sEncode = sr.CurrentEncoding.CodePage.ToString();

                //string pageHtml = (sEncode == "65001") ? Encoding.UTF8.GetString(pageData) : Encoding.GetEncoding(950).GetString(pageData);

                #endregion [判斷讀入字串編碼]

                //return pageHtml;
                return Encoding.GetEncoding(950).GetString(pageData);
            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());
                return webEx.Message;
            }
        }

        public bool IsBig5(string Data)
        {
            //帶入字串先以CodePage 950(Big5)轉成byte陣列
            byte[] btUnKnow = Encoding.GetEncoding(950).GetBytes(Data);
            //將byte陣列以CodePage 950(Big)轉成字串
            string result = Encoding.GetEncoding(950).GetString(btUnKnow);
            //將帶入字串及轉出字串比對，如相同則編碼為Big5
            return (result == Data);
        }

        #endregion [Method]
    }

    #region [Unicode字型處理]

    public class UnicodeFontFactory : iTextSharp.text.FontFactoryImp
    {
        //arial unicode MS是完整的unicode字型。
        private static readonly string arialFontPath =
            Path.Combine(
                Path.Combine(
                    Directory.GetParent(
                        Environment.GetFolderPath(Environment.SpecialFolder.System)).FullName,
                        "Fonts"), "arialuni.ttf");

        //標楷體
        private static readonly string chinesePath =
            Path.Combine(
                Path.Combine(
                    Directory.GetParent(
                        Environment.GetFolderPath(Environment.SpecialFolder.System)).FullName,
                        "Fonts"), "KAIU.TTF");

        public override iTextSharp.text.Font GetFont(string fontname, string encoding,
            bool embedded, float size, int style, iTextSharp.text.Color color, bool cached)
        {
            iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(
                chinesePath, iTextSharp.text.pdf.BaseFont.IDENTITY_H,
                iTextSharp.text.pdf.BaseFont.EMBEDDED);

            return new iTextSharp.text.Font(baseFont, size, style, color);
        }
    }

    #endregion [Unicode字型處理]


    public class CustomImageTagProcessor
    {
        public IList<IElement> ProcessImageTag(IDictionary<string, string> attributes)
        {
            string src;
            if (!attributes.TryGetValue("src", out src) || string.IsNullOrEmpty(src))
                return new List<IElement>(1);

            if (src.StartsWith("data:image/", StringComparison.InvariantCultureIgnoreCase))
            {
                // 處理 base64 編碼的圖片資料
                var base64Data = src.Substring(src.IndexOf(",") + 1);
                var imagedata = Convert.FromBase64String(base64Data);
                var image = iTextSharp.text.Image.GetInstance(imagedata);

                // 直接添加 Image 作為 Element 並返回
                var list = new List<IElement> { image };
                return list;
            }
            else
            {
                // 其他處理可自定義
                return new List<IElement>(1);
            }
        }
    }


}