using IBApi;

namespace IBGapValue.Services.Impl
{
    public class IBMessageServiceImp : IIBMessageService
    {
        public int CurrentReqBarId { get; set; } = -1;
        public int ReqBarIdCount { get; set; } = 0;

        public event HandleMessageSend OnReceivedMessage;

        public event HandleMessageEnd OnMessageEnd;

        public event HandleReceivedMarketScanner OnRecievedMarketScanner;

        public event HandleReceivedMarketScannerEnd OnReceivedMarketScannerEnd;

        public event HandleReceivedErrorMessage OnReceivedErrorMessage;

        public event HandleReqBarDone OnReqBarDone;

        public void EndMessage(int reqId)
        {
            OnMessageEnd?.Invoke(reqId);
        }

        public void NotifyReceivedErrorMessage(int reqId)
        {
            OnReceivedErrorMessage?.Invoke(reqId);
        }

        public void NotifyReceivedMarketScanner(int reqId, Contract contract)
        {
            OnRecievedMarketScanner?.Invoke(reqId, contract);
        }

        public void NotifyReceivedMarketScannerEnd()
        {
            OnReceivedMarketScannerEnd?.Invoke();
        }

        public void NotifyReqBarDone(int reqId)
        {
            OnReqBarDone?.Invoke(reqId);
        }

        public void SendMessage(int reqId, Bar bar)
        {
            OnReceivedMessage?.Invoke(reqId, bar);
        }
    }
}