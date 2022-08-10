using System;
using System.Collections.Generic;
using System.Text;

namespace IBGapValue.Services
{
    public enum MessageType
    {
        ERROR, INFO, SUCCESS
    }

    public interface IMessageLogService
    {
        void LogInfo(string message);

        void LogSuccess(string message);

        void LogError(string message);
    }
}