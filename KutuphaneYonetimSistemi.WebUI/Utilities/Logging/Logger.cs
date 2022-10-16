using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;

namespace LibraryManagementSystem.WebUI.Utilities.Logging {
    public static class Logger {
        private static readonly Dictionary<string, ILog> loggers = new Dictionary<string, ILog>();
        public static ILog Default;
        static Logger() {
            XmlConfigurator.Configure();
            Default = GetLoggerInternal(ConfigurationManager.AppSettings["loggername"]);
        }

        #region Custom logger methods
        public static void Info(string readerIp, string message) {
            GetLoggerInternal(readerIp).Info(message);
        }
        public static void Info(string message) {
            var loggername = ConfigurationManager.AppSettings["loggername"];
            GetLoggerInternal(loggername).Info(message);
        }

        public static void Debug(string readerIp, string message) {
            GetLoggerInternal(readerIp).Debug(message);
        }

        public static void Warning(string readerIp, string message) {
            GetLoggerInternal(readerIp).Warn(message);
        }

        public static void Error(string readerIp, string message) {
            GetLoggerInternal(readerIp).Error(message);
        }
        public static void Error(MethodBase method, Exception exception, string message) {
            Default.Error($"[Message: {message}] " +
                          $"[Location: {method.DeclaringType.FullName}.{method.Name}()] " +
                          $"[Exception: {exception.Message}] " +
                          $"[StackTrace: {exception.StackTrace}] ");
        }
        public static void Error(string readerIp, MethodBase method, Exception exception, string message) {
            GetLoggerInternal(readerIp).Error(
                          $"[Message: {message}] " +
                          $"[Location: {method.DeclaringType.FullName}.{method.Name}()] " +
                          $"[Exception: {exception.Message}] " +
                          $"[StackTrace: {exception.StackTrace}] ");
        }

        public static void Fatal(string readerIp, string message) {
            GetLoggerInternal(readerIp).Fatal(message);
        }
        #endregion

        private static ILog GetLoggerInternal(string logger) {
            if (!loggers.ContainsKey(logger)) {
                var appender = CreateRollingFileAppender(logger);
                appender.ActivateOptions();
                loggers.Add(logger, LogManager.GetLogger(logger));
                ((log4net.Repository.Hierarchy.Logger)loggers[logger].Logger).AddAppender(appender);
            }
            return loggers[logger];
        }

        private static RollingFileAppender CreateRollingFileAppender(string readingPointIp) {
            var layout = new PatternLayout {
                ConversionPattern = $"[%date] [%-5level] [%thread] %message %newline"
            };
            layout.ActivateOptions();

            return new RollingFileAppender {
                Name = readingPointIp,
                AppendToFile = true,
                DatePattern = "yyyyMMdd",
                MaximumFileSize = "10MB",
                MaxSizeRollBackups = 10,
                RollingStyle = RollingFileAppender.RollingMode.Composite,
                File = $".\\logs\\BridgeAirportIntegrator\\{readingPointIp}\\{readingPointIp}.log",
                Layout = layout,
                Threshold = Level.Debug
            };
        }
    }
}