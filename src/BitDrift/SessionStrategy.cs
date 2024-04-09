using System;
using System.Runtime.Serialization;

namespace BitDrift;

/// <summary>
/// Events collected by the SDK are annotated with a session identifier (ID). This session identifier 
/// is utilized to group events emitted by the SDK and can be used to retrieve events from a specific session.
/// 
/// The SDK manages the used session identifier using one of the two following strategies:
/// </summary>
public enum SessionStrategy
{
    /// <summary>
    /// A session strategy that generates a new session ID after a certain period of app inactivity. 
    /// The inactivity duration is measured by the minutes elapsed since the last log. The session 
    /// ID is persisted to disk and survives app restarts.
    /// </summary>
    [EnumMember(Value = "activity-based")]
    ActivityBased = 0,
    
    /// <summary>
    /// A session strategy that never expires the session ID but does not survive process restart.
    /// </summary>
    [EnumMember(Value = "fixed")]
    Fixed
}
