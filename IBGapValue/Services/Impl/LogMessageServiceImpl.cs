using System;
using System.Collections.Generic;
using System.Text;

namespace IBGapValue.Services.Impl
{
    public class LogMessageServiceImpl : IMessageLogService
    {
        public string CurrentTime => DateTime.Now.ToLongTimeString();

        public void LogError(string message)
        {
            Console.WriteLine(GetMessage(MessageType.ERROR, message));
            Console.ResetColor();
        }

        public void LogInfo(string message)
        {
            Console.WriteLine(GetMessage(MessageType.INFO, message));
            Console.ResetColor();
        }

        public void LogSuccess(string message)
        {
            Console.WriteLine(GetMessage(MessageType.SUCCESS, message));
            Console.ResetColor();
        }

        private string GetMessage(MessageType messageType, string message)
        {
            ConsoleColor consoleColor = ConsoleColor.White;
            string messageTypeTag = "INFO";
            switch (messageType)
            {
                case MessageType.ERROR:
                    consoleColor = ConsoleColor.Red;
                    messageTypeTag = "ERROR";
                    break;

                case MessageType.SUCCESS:
                    consoleColor = ConsoleColor.Green;
                    messageTypeTag = "SUCCESS";
                    break;
            }
            Console.ForegroundColor = consoleColor;
            return $"[{messageTypeTag} - {CurrentTime}] : {message}";
        }
    }
}