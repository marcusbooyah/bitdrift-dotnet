namespace BitDrift;
internal static class BitDriftExtensions
{
    internal static string ToNativeString(this SessionStrategy sessionStrategy){
        return sessionStrategy switch
        {
            SessionStrategy.ActivityBased => "activity-based",
            SessionStrategy.Fixed => "fixed",
            _ => ""
        };
    }
}