namespace Revive.Redux.Common
{
    public class LoggingManager
    {
        #region Members

        private static ILogService Logger;
        #endregion
        public static ILogService GetLogInstance()
        {
            if (Logger == null)
            {
                Logger = new LogService();
            }
            return Logger;
        }
    }
}
