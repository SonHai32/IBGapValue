using IBApi;
using IBGapValue.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBGapValue.Services
{
    public delegate void HandleMessageSend(int reqId, Bar bar);

    public delegate void HandleMessageEnd(int reqId);

    public delegate void HandleReceivedMarketScanner(int reqId, Contract contract);

    public delegate void HandleReceivedMarketScannerEnd();

    public delegate void HandleReceivedErrorMessage(int reqId);

    public delegate void HandleReqBarDone(int reqId);

    public interface IIBMessageService
    {
        event HandleMessageSend OnReceivedMessage;

        event HandleMessageEnd OnMessageEnd;

        event HandleReceivedMarketScanner OnRecievedMarketScanner;

        event HandleReceivedMarketScannerEnd OnReceivedMarketScannerEnd;

        event HandleReceivedErrorMessage OnReceivedErrorMessage;

        event HandleReqBarDone OnReqBarDone;

        void SendMessage(int reqId, Bar bar);

        void EndMessage(int reqId);

        void NotifyReceivedMarketScanner(int reqId, Contract contract);

        void NotifyReceivedMarketScannerEnd();

        void NotifyReceivedErrorMessage(int reqId);

        void NotifyReqBarDone(int reqId);

        int CurrentReqBarId { get; set; }
        int ReqBarIdCount { get; set; }
    }
}