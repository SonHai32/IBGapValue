using IBApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBGapValue.Models
{
    public class CustomBar
    {
        public string InstructmentSymbol { get; set; }
        public Bar Bar { get; set; }
    }
}