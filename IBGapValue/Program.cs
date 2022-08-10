using CommandLine;
using ConsoleTables;
using IBApi;
using IBGapValue.Helpers;
using IBGapValue.Models;
using IBGapValue.Resouces;
using IBGapValue.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IBGapValue
{
    internal class Program
    {
        private static IBMessageServiceImp _iBMessageService => Singleton<IBMessageServiceImp>.Instance;
        private static List<CustomBar> _listBarResponse = new List<CustomBar>();
        private static List<Contract> _contractList = new List<Contract>();
        private static EWrapperImpl ibClient;
        private static string _exchangeSymbol;
        private static LogMessageServiceImpl _logMessageService = new LogMessageServiceImpl();

        private static async Task<int> Main(string[] args)
        {
            return await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(async (CommandLineOptions opts) =>
                {
                    try
                    {
                        _exchangeSymbol = opts.ExchangeSymbol;
                        _iBMessageService.OnReceivedMessage += _iBMessageService_OnReceivedMessage;
                        _iBMessageService.OnMessageEnd += _iBMessageService_OnMessageEnd;
                        _iBMessageService.OnRecievedMarketScanner += _iBMessageService_OnRecievedMarketScanner;
                        _iBMessageService.OnReceivedMarketScannerEnd += _iBMessageService_OnReceivedMarketScannerEnd;
                        _iBMessageService.OnReqBarDone += _iBMessageService_OnReqBarDone;
                        ibClient = new EWrapperImpl();
                        ibClient.ClientSocket.eConnect("127.0.0.1", 7497, 222);

                        var reader = new EReader(ibClient.ClientSocket, ibClient.Signal);
                        reader.Start();
                        new Thread(() =>
                        {
                            while (ibClient.ClientSocket.IsConnected())
                            {
                                ibClient.Signal.waitForSignal();
                                reader.processMsgs();
                            }
                        })
                        { IsBackground = true }.Start();

                        // Pause here until the connection is complete
                        while (ibClient.NextOrderId <= 0) { }
                        var locationCode = LocationCodes.ListLocationCode.Where(locationCode => locationCode.Contains(_exchangeSymbol)).FirstOrDefault();
                        if (null == locationCode)
                            throw new Exception("Exchange does not support");
                        ScannerSubscription scanSub = new ScannerSubscription();
                        scanSub.Instrument = locationCode.Split(".")[0];
                        scanSub.LocationCode = locationCode;
                        scanSub.StockTypeFilter = "ALL";
                        //scanSub.ScanCode = "HIGH_OPEN_GAP";
                        //scanSub.ScanCode = "HOT_BY_VOLUME";
                        scanSub.ScanCode = "COMBO_ALL_VOLUME_ASC";
                        List<TagValue> scannerTagValues = new List<TagValue>();
                        _logMessageService.LogInfo($"Scanning Data from {_exchangeSymbol} with {scanSub.ScanCode}...");
                        ibClient.ClientSocket.reqScannerSubscription(1000, scanSub, null, scannerTagValues);
                        var currentTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

                        Thread.Sleep(900000000);
                        return await Task.FromResult(1);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return -3;
                    }
                }, err => Task.FromResult(-1));
        }

        private static void _iBMessageService_OnReqBarDone(int reqId)
        {
            throw new NotImplementedException();
        }

        private static void ScanMarketData()
        {
            _logMessageService.LogSuccess($"{_exchangeSymbol} has been fetched");
            FetchData();
            //ResloveConfict();
        }

        private static void _iBMessageService_OnReceivedMarketScannerEnd()
        {
            if (!_contractList.Any())
                return;
            ibClient.ClientSocket.cancelScannerSubscription(1000);
            ScanMarketData();
        }

        private static void _iBMessageService_OnRecievedMarketScanner(int reqId, Contract contract)
        {
            _contractList.Add(contract);
        }

        private static void _iBMessageService_OnMessageEnd(int reqId)
        {
        }

        private static void ResloveConfict()
        {
            if (!_contractList.Any() || !_listBarResponse.Any())
                return;
            List<StockInfo> stockInfos = new List<StockInfo>();
            foreach (var contract in _contractList)
            {
                var barOfContract = _listBarResponse.Where(bar => bar.InstructmentSymbol.Equals(contract.Symbol)).ToList();
                if (!barOfContract.Any())
                    continue;
                var currentBar = barOfContract[barOfContract.Count - 1];
                var previousBar = barOfContract[barOfContract.Count - 2];
                if (currentBar == null || previousBar == null)
                    continue;
                StockInfo stockInfo = new StockInfo()
                {
                    CurrentOpeningPrice = currentBar.Bar.Open,
                    PreviousClosingPrice = previousBar.Bar.Close,
                    InstructmentName = contract.Symbol,
                    InstructmentSymbol = contract.Symbol
                };

                stockInfos.Add(stockInfo);
            }

            var table = new ConsoleTable("Instructment Name", "Instructment Symbol", "Previous Closed Price", "Current Opening Price", "Gap Value", "Gap Percent");
            foreach (var stockInfo in stockInfos)
            {
                table.AddRow(stockInfo.InstructmentName, stockInfo.InstructmentSymbol, stockInfo.PreviousClosingPrice + " USD", stockInfo.CurrentOpeningPrice + " USD", stockInfo.GapValue + " USD", stockInfo.GapPercent + "%");
            }
            table.Write();
        }

        private static void _iBMessageService_OnReceivedMessage(int reqId, Bar bar)
        {
            _listBarResponse.Add(new CustomBar() { InstructmentSymbol = _contractList[reqId].Symbol, Bar = bar });
        }

        private static void FetchData()
        {
            if (!_contractList.Any())
                return;
            var currentTime = DateTime.Now.AddMinutes(-20).ToString("yyyyMMdd HH:mm:ss");
            for (int i = 0; i < _contractList.Count; i++)
            {
                Contract contract = new Contract();
                contract.Symbol = _contractList[i].Symbol;
                contract.Exchange = _exchangeSymbol;
                contract.SecType = _contractList[i].SecType;
                ibClient.ClientSocket.reqMarketDataType(3);
                ibClient.ClientSocket.reqHistoricalData(i, contract, currentTime, "10 D", "1 day", "MIDPOINT", 1, 1, false, null);
            }
        }
    }
}