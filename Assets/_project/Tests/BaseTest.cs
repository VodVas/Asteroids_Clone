using NUnit.Framework;
using UnityEngine;

namespace AsteroidsClone.Tests
{
    public abstract class BaseTest
    {
        private LogType _originalLogLevel;
        private StackTraceLogType _originalStackTrace;

        [OneTimeSetUp]
        public void BaseSetUp()
        {
            _originalLogLevel = Debug.unityLogger.filterLogType;
            _originalStackTrace = Application.GetStackTraceLogType(LogType.Warning);
            
            Debug.unityLogger.filterLogType = LogType.Error;
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
        }

        [OneTimeTearDown]
        public void BaseTearDown()
        {
            Debug.unityLogger.filterLogType = _originalLogLevel;
            Application.SetStackTraceLogType(LogType.Warning, _originalStackTrace);
        }
    }
}
