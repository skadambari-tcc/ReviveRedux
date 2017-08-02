using System;

namespace Revive.Redux.Common
{
    public interface ILogService
    {
        void LogInformation(string message, Exception exception = null);
        void LogInformationFormat(string format, params object[] args);
        void LogDebug(string message, Exception exception = null);
        void LogDebugFormat(string format, params object[] args);
        void LogWarning(string message, Exception exception = null);
        void LogWarningFormat(string format, params object[] args);
        void LogError(string message, Exception exception = null);
        void LogErrorFormat(string format, params object[] args);
        void LogException(string message, Exception exception = null);
        void LogExceptionFormat(string format, params object[] args);
        bool IsDebugEnabled { get; }
      //  void ChangeFilePath(string appenderName, string newFilename);

    }
}