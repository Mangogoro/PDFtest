using System;

namespace FineBillBus
{
    /// <summary>
    /// Utility 的摘要描述。
    /// </summary>
    /// 
    public class APUtility
    {
        /// <summary>
        /// NLog 紀錄
        /// </summary>
        private static NLog.Logger NLogger = NLog.LogManager.GetCurrentClassLogger();



    }




    /// <summary>
    /// 字串擴充函式
    /// </summary>
    public static class StringExtension
    {

        /// <summary>
        /// 字串是否為NULL
        /// </summary>
        /// <returns></returns>
        public static bool IsNull(this string value)
        {
            return string.IsNullOrEmpty(value);
        }


        /// <summary>
        /// 如果傳入值為NULL or "" 則將其轉換為空字串""，如果傳入值非NULL則傳回原值
        /// </summary>
        /// <param name="oSourceValue"></param>
        /// <param name="sElseValue">默認為空字串</param>
        /// <returns>轉換後的value</returns>
        public static string To_TrimString(this object oSourceValue, string sElseValue = "")
        {
            if (oSourceValue == null || string.IsNullOrEmpty(oSourceValue.ToString()))
            {
                return sElseValue;
            }
            else
            {
                return oSourceValue.ToString().Trim();
            }
        }


        /// <summary>
        /// 數字轉換為加逗號的數字
        /// </summary>
        /// <returns></returns>
        public static string ToMoneyStr(this decimal iNumStr)
        {
            return String.Format("{0:$#,##0;-$#,##0;$0}", iNumStr);
        }

        /// <summary>
        /// 數字轉換為加逗號的數字
        /// </summary>
        /// <returns></returns>
        public static string ToMoneyStr(this decimal? iNumStr)
        {
            if (!iNumStr.HasValue)
            {
                return "$0";
            }

            return String.Format("{0:$#,##0;-$#,##0;$0}", iNumStr.Value);
        }

        /// <summary>
        /// 數字轉換為加逗號的數字
        /// </summary>
        /// <returns></returns>
        public static string ToMoneyStr(this string iNumStr)
        {
            if (iNumStr.IsNull())
            {
                return "$0";
            }

            decimal dNumStr;
            if (!Decimal.TryParse(iNumStr, out dNumStr))
            {
                return iNumStr;
            }


            return String.Format("{0:$#,##0;-$#,##0;$0}", dNumStr);
        }


        /// <summary>
        /// 去除小數點，轉成整數
        /// </summary>
        /// <returns></returns>
        public static int ToInt(this decimal? iNumStr)
        {
            if (iNumStr.HasValue)
            {
                return (int)iNumStr.Value;
            }
            return 0;
        }

    }

}