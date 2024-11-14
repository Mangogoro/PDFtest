
此專案使用的是 .NET Framework 4.8，有一個PDF產檔功能，用於將 HTML 轉換為 PDF，現行使用的是 iTextSharp 5.5 版本，但因專案需求，計畫降版至 iTextSharp 4.1.6，然而，專案中的 HTML 文件是動態生成且具有一定的複雜性，在降版至 iTextSharp 4.1.6 後，轉換的 PDF 文件出現了跑版和錯亂的情形，因此這個問題的主要是得克服這樣的HTML內容要能順利轉成PDF。
 
為協助理解問題，下面提供單元測試Github，並且附上降版前與降版後的 PDF 文件於Github的以下路徑：
 
降版前（使用 iTextSharp 5.5）：LBeBillingUnitTest\_output\預期結果_itextsharp5.5.pdf
降版後（使用 iTextSharp 4.1.6）：LBeBillingUnitTest\_output\降版後結果_itextsharp4.1.pdf
 
 
單元測試說明:
       專案中有一個單元測試，名為【PDF測試_ConvertHTMLToPDF】，執行此測試後，會在 LBeBillingUnitTest\_output 路徑下生成對應的 PDF 檔案，可用於比較降版前後的結果。
 
PDF 生成的程式碼封裝在 clsPDF 類別中，該類別的程式已調整為降版後的 iTextSharp 4.1.6；至於 HTML 轉換為 PDF 的這個HTML範例，則撰寫在 TclsPDF 單元測試內。
