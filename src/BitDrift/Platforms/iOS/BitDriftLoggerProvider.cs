using Microsoft.Extensions.Logging;
using Microsoft.Maui.Devices;

using UIKit;

namespace BitDrift
{
    public partial class BitDriftLoggerProvider : ILoggerProvider
    {
        internal void AddPropertiesNative()
        {
#pragma warning disable CA1416 // Validate platform compatibility
            Capture.AddFieldNative("DeviceId", UIDevice.CurrentDevice.IdentifierForVendor.ToString());
            Capture.AddFieldNative("Model", DeviceInfo.Model);
            Capture.AddFieldNative("Manufacturer", DeviceInfo.Current.Manufacturer);
            Capture.AddFieldNative("Name", DeviceInfo.Current.Name);
            Capture.AddFieldNative("OsVersion", DeviceInfo.Current.VersionString);
            Capture.AddFieldNative("Idiom", DeviceInfo.Current.Idiom.ToString());
            Capture.AddFieldNative("Platform", DeviceInfo.Current.Platform.ToString());
#pragma warning restore CA1416 // Validate platform compatibility
        }
    }
}
