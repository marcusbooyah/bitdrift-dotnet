using System;
using System.Collections.Generic;

using Com.Euarctos.Bitdrift;

namespace BitDrift;

public static partial class Capture
{
    internal static void ConfigureNative(string apiKey, string url, string sessionStrategy)
    {
        if (IsConfigured)
        {
            return;
        }
        
        BitDriftModuleKt.Init(apiKey, url, sessionStrategy);
    }

    internal static string SessionIdNative()
    {
        return BitDriftModuleKt.SessionId();
    }
    internal static string SessionUrlNative()
    {
        return BitDriftModuleKt.SessionUrl();
    }

    internal static void StartNewSessionNative()
    {
        BitDriftModuleKt.StartNewSession();
    }
    
    internal static void AddFieldNative(string key, string value)
    {
        BitDriftModuleKt.AddField(key, value);
    }
    internal static void RemoveFieldNative(string key)
    {
        BitDriftModuleKt.RemoveField(key);
    }

    internal static void LogNative(double level, string message, IDictionary<string, string> arguments = null)
    {
        BitDriftModuleKt.Log(level, message, arguments);
    }
}