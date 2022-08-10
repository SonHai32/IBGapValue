using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBGapValue.Models
{
    public class ContractEntryClassMap : ClassMap<ContractEntry>
    {
        public ContractEntryClassMap()
        {
            Map(m => m.Country).Name("Country");
            Map(m => m.ExchangeName).Name("Exchange Name");
            Map(m => m.ExchangeSymbol).Name("Exchange Symbol");
            Map(m => m.InstructmentSymbol).Name("Instrument Symbol");
            Map(m => m.InstructmentType).Name("Instrument Type");
        }
    }
}