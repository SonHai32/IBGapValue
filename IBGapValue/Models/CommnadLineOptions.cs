using CommandLine;

namespace IBGapValue.Models
{
    internal class CommandLineOptions
    {
        [Option(shortName: 'e', longName: "exchange", Required = true, HelpText = "Exchange Symbol")]
        public string ExchangeSymbol { get; set; }
    }
}