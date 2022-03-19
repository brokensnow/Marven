#region Using
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using static REAccess.Mobile.Common.Constants;
#endregion

namespace REAccess.Mobile.Common.Utils
{
    public static class ToolFunc
    {
        /// <summary>
        /// 时间格式转换
        /// </summary>
        public static string DatetimeFormatter(DateTime? dateTime)
        {
            var datetimeStr = string.Empty;
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd";
            datetimeStr = Convert.ToDateTime(dateTime, dtFormat).ToString("yyyy-MM-dd");

            return datetimeStr;
        }

        /// <summary>
        /// 千分位转换
        /// </summary>
        public static string ThousandFormatter(string matterValue)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(matterValue))
            {
                var pointNum = matterValue.Split('.').Last();
                if (matterValue.IndexOf(".") == -1)
                {
                    result = float.Parse(matterValue).ToString("N0");
                }
                else if (pointNum.Length == 1)
                {
                    result = float.Parse(matterValue).ToString("N1").Replace(".0","");
                }
                else if (pointNum.Length >= 2)
                {
                    result = float.Parse(matterValue).ToString("N2").Replace(".00", "");
                }
            }

            return result;
        }
        public static string ThousandFormatter(string matterValue, int place)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(matterValue))
            {
                switch (place)
                {
                    case 0:
                        result = float.Parse(matterValue).ToString("N0");
                        break;
                    case 1:
                        result = float.Parse(matterValue).ToString("N1").Replace(".0", "");
                        break;
                    case 2:
                        result = float.Parse(matterValue).ToString("N2").Replace(".00", "");
                        break ;
                    default:
                        result = matterValue;
                        break;
                }
            }

            return result;
        }
        public static string BillionConversionFunc(string matterValue)
        {
            string result = string.Empty;
            if(float.Parse(matterValue) > 1000)
            {
                result = ThousandFormatter((float.Parse(matterValue) / 10000).ToString(),1) + IndustryRankUnit.ByAmount;
            }
            else
            {
                result = ThousandFormatter(matterValue) + IndustryRankUnit.AmountUnit;
            }

            return result;
        }
    }
}
