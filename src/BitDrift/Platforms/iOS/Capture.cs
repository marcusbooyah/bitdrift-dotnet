using System.Collections.Generic;
using System.Linq;

using Foundation;

namespace BitDrift;

public static partial class Capture
{
    internal static void ConfigureNative(string apiKey, string url, string sessionStrategy)
    {
        if (IsConfigured)
        {
            return;
        }
        
        BitDriftModule.Logger.ConfigureWithKey(apiKey, url, sessionStrategy);
    }

    internal static string SessionIdNative()
    {
        return BitDriftModule.Logger.SessionId();
    }
    internal static string SessionUrlNative()
    {
        return BitDriftModule.Logger.SessionUrl();
    }

    internal static void StartNewSessionNative()
    {
        BitDriftModule.Logger.StartNewSession();
    }
    
    internal static void AddFieldNative(string key, string value)
    {
        BitDriftModule.Logger.AddFieldWithKey(key, value);
    }
    internal static void RemoveFieldNative(string key)
    {
        BitDriftModule.Logger.RemoveFieldWithKey(key);
    }

    internal static void LogNative(double level, string message, IDictionary<string, string> arguments = null)
    {
        var keys = arguments?.Keys?.Select(x => new NSString(x))?.ToArray() ?? new NSString[0];
        var values = arguments?.Values?.Select(x => new NSString(x))?.ToArray() ?? new NSString[0];

        NSDictionary<NSString, NSString> nsArgs = new NSDictionary<NSString, NSString>(
            keys: keys,
            values: values);

        BitDriftModule.Logger.Log(level, message, nsArgs);
    }
}
