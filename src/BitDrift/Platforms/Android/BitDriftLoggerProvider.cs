using Com.Euarctos.Bitdrift;

using Microsoft.Extensions.Logging;
using Microsoft.Maui.Devices;

using static Android.Provider.Settings;

namespace BitDrift
{
    public partial class BitDriftLoggerProvider : ILoggerProvider
    {
        internal void AddPropertiesNative()
        {
            Capture.AddFieldNative("DeviceId", global::Android.Provider.Settings.Secure.GetString(global::Android.App.Application.Context.ContentResolver, Secure.AndroidId));
            Capture.AddFieldNative("Model", DeviceInfo.Model);
            Capture.AddFieldNative("Manufacturer", DeviceInfo.Current.Manufacturer);
            Capture.AddFieldNative("Name", DeviceInfo.Current.Name);
            Capture.AddFieldNative("OsVersion", DeviceInfo.Current.VersionString);
            Capture.AddFieldNative("Idiom", DeviceInfo.Current.Idiom.ToString());
            Capture.AddFieldNative("Platform", DeviceInfo.Current.Platform.ToString());
        }
    }
}
