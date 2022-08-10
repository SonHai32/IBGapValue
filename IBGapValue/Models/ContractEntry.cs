using System;
using System.Collections.Generic;
using System.Text;

namespace IBGapValue.Models
{
    public class ContractEntry
    {
        public string Country { get; set; }
        public string ExchangeName { get; set; }
        public string ExchangeSymbol { get; set; }
        public string InstructmentSymbol { get; set; }
        public string InstructmentType { get; set; }
    }
}