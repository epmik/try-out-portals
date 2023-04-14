using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.IO;

public class LogManager
{
    public enum LogLevel
    {
        Trace,
        Debug,
        Warn,
        Error,
        Fatal,
        None,
    }

    private static readonly LogManager _instance = new LogManager();

    private NLog.Logger _logger;

    private Dictionary<string, object> _traceChangeDictionary = new Dictionary<string, object>();

    public LogLevel MinLogLevel = LogLevel.Error;

    static LogManager()
    {
    }

    private LogManager()
    {
    }

    public static LogManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private NLog.Logger Initialize()
    {
        if (_logger != null)
        {
            return _logger;
        }

        var config = new NLog.Config.LoggingConfiguration();

        //var consoleTarget = new ConsoleTarget("console");
        //config.AddTarget("console", consoleTarget);

        //var logsPath = @"d:\temp\_logs";
        var dir = Path.Combine(Application.persistentDataPath, "_logs");// logsPath + "\\app" + "\\" + Environment.UserName;
        //var dir = logsPath + "\\app" + "\\" + Environment.UserName;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var filepath = Path.Combine(dir, DateTime.Now.ToString("yyyyMMdd.HHmm.ss", CultureInfo.InvariantCulture) + ".log");

        UnityEngine.Debug.Log($"Logging to: {filepath}");

        var fileTarget = new NLog.Targets.FileTarget("file")
        {
            FileName = filepath,
            //Layout = "${date:format=yyyyMMdd:HH:mm:ss:fff} ${message}"
            //Layout = "${date:format=HH:mm:ss:fff} ${message}"
            Layout = @"${level:uppercase=true}|${date:format=HH\:mm\:ss.fff} ${message}",
        };
        config.AddTarget("file", fileTarget);

        //var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
        //config.LoggingRules.Add(rule1);

        var rule2 = new NLog.Config.LoggingRule("*", LoggingRuleLogLevel(MinLogLevel), fileTarget);
        config.LoggingRules.Add(rule2);

        //NLog.Common.InternalLogger.LogToConsole = true;

        NLog.LogManager.ThrowExceptions = true;

        NLog.LogManager.Configuration = config;

        _logger = NLog.LogManager.GetLogger("file");

        return _logger;
    }

    public void EnableLogLevel(LogLevel level)
    {
        Initialize();

        MinLogLevel = level;

        if (level == LogLevel.None)
        {
            NLog.LogManager.SuspendLogging();
        }
        else
        {
            if (!NLog.LogManager.IsLoggingEnabled())
            {
                NLog.LogManager.ResumeLogging();
            }

            foreach (var rule in NLog.LogManager.Configuration.LoggingRules)
            {
                // Iterate over all levels up to and including the target, (re)enabling them.
                for (int i = (int)LogManager.LogLevel.Trace; i <= (int)level; i++)
                {
                    rule.EnableLoggingForLevel(LoggingRuleLogLevel((LogManager.LogLevel)i));
                }
            }
        }

        NLog.LogManager.ReconfigExistingLoggers();
    }

    private static NLog.LogLevel LoggingRuleLogLevel(LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Trace:
                return NLog.LogLevel.Trace;
            case LogLevel.Debug:
                return NLog.LogLevel.Debug;
            case LogLevel.Warn:
                return NLog.LogLevel.Warn;
            case LogLevel.Error:
                return NLog.LogLevel.Error;
            case LogLevel.Fatal:
                return NLog.LogLevel.Fatal;
            default:
                throw new NotImplementedException();
        }
    }

    public void Trace(string message)
    {
        if (MinLogLevel > LogLevel.Trace)
        {
            return;
        }

        Initialize().Trace(message);
    }

    public void TraceChange<TValue>(TValue value, string message, string key) where TValue : IEquatable<TValue>
    {
        if (MinLogLevel > LogLevel.Trace || !HasChanged(value, key))
        {
            return;
        }

        Initialize().Trace($"{message} | {key}");
    }

    private bool HasChanged<TValue>(TValue value, string key) where TValue : IEquatable<TValue>
    {
        if(!_traceChangeDictionary.ContainsKey(key))
        {
            _traceChangeDictionary.Add(key, value);
        }

        if(!_traceChangeDictionary[key].Equals(value))
        {
            _traceChangeDictionary[key] = value;

            return true;
        }

        return false;
    }

    public void Debug(string message)
    {
        if (MinLogLevel > LogLevel.Debug)
        {
            return;
        }
        Initialize().Debug(message);
    }

    public void Warn(string message)
    {
        if (MinLogLevel > LogLevel.Warn)
        {
            return;
        }
        Initialize().Warn(message);
    }

    public void Error(string message)
    {
        if (MinLogLevel > LogLevel.Error)
        {
            return;
        }
        Initialize().Error(message);
    }

    public void Fatal(string message)
    {
        if (MinLogLevel > LogLevel.Fatal)
        {
            return;
        }
        Initialize().Fatal(message);
    }
}
