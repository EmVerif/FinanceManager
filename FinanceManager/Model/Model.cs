using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using FinanceManager.Common;

namespace FinanceManager.Model
{
    public class TradingInfo
    {
        public string DateStr { get; set; }
        public Decimal TotalBuyPrice { get; set; }
        public int BuyNum { get; set; }
        public Decimal TotalSellPrice { get; set; }
        public int SellNum { get; set; }
        public Decimal FinalPrice { get; set; }

        public TradingInfo()
        {
            DateStr = @"1900/1/1";
            TotalBuyPrice = 0;
            BuyNum = 0;
            TotalSellPrice = 0;
            SellNum = 0;
            FinalPrice = 0;
        }
    }

    public class CodeInfo
    {
        public string CompanyName;
        public SerializableDictionary<DateTime, TradingInfo> DayTradingInfosDict;

        public CodeInfo()
        {
            CompanyName = "";
            DayTradingInfosDict = new SerializableDictionary<DateTime, TradingInfo>();
        }
    }
}
