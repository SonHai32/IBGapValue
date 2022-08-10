using System;
using System.Collections.Generic;
using System.Text;

namespace IBGapValue.Models
{
    public class StockInfo
    {
        public string InstructmentSymbol { get; set; }
        public string InstructmentName { get; set; }
        public double PreviousClosingPrice { get; set; }
        public double CurrentOpeningPrice { get; set; }

        public double GapValue
        {
            get => Math.Round(CurrentOpeningPrice - PreviousClosingPrice, 3);
        }

        public double GapPercent
        {
            get => Math.Round((GapValue / PreviousClosingPrice) * 100, 3);
        }
    }
}