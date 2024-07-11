import Capture
import os

@objc(Logger)
public class Logger: NSObject {

    private static let logger = os.Logger(subsystem: "com.bitdrift.dotnet", category: "main")
    
    @objc
    public static func configure(key: String, url: String, strategy: String) {
        logger.debug("Configuring BitDrift")
        switch strategy {
        case "fixed":
            Capture.Logger.configure(
                withAPIKey: key,
                sessionStrategy: SessionStrategy.fixed(),
                apiURL: URL(string: url)!)
        case "activity-based":
            Capture.Logger.configure(
                withAPIKey: key,
                sessionStrategy: SessionStrategy.activityBased(),
                apiURL: URL(string: url)!)
        default:
            return
        }
        
        logger.debug("BitDrift Configured")
    }

    @objc
    public static func log(_ level: Double, message: String, fields: [String: String]) {
        
        //let exception = tryBlock {
        switch level {
        case 0:
            logger.debug("logging trace")
            _ = tryBlock {
                Capture.Logger.logTrace(message, fields: fields)
            }
        case 1:
            logger.debug("logging debug")
            _ = tryBlock {
                Capture.Logger.logDebug(message, fields: fields)
            }
        case 2:
            logger.debug("logging info")
            _ = tryBlock {
                Capture.Logger.logInfo(message, fields: fields)
            }
        case 3:
            logger.debug("logging warning")
            _ = tryBlock {
                Capture.Logger.logWarning(message, fields: fields)
            }
        case 4:
            logger.debug("logging error")
            _ = tryBlock {
                Capture.Logger.logError(message, fields: fields)
            }
        default:
            logger.debug("Not logging, could not parse level: \(level)");
            return
        }
        //}
        
        //if exception != nil {
        //    if let excMsg = exception?.reason {
        //        logger.debug("\(excMsg)")
        //    }
        //}
        
    }
    
    @objc
    public static func sessionId() -> String
    {
        let result = Capture.Logger.sessionID;
        return result!;
    }
    
    @objc
    public static func sessionUrl() -> String
    {
        let result = Capture.Logger.sessionURL;
        return result!;
    }
    
    @objc
    public static func startNewSession()
    {
        Capture.Logger.startNewSession();
    }
    
    @objc
    public static func addField(key: String, value: String)
    {
        Capture.Logger.addField(withKey: key, value: value);
    }
    
    @objc
    public static func removeField(key: String)
    {
        Capture.Logger.removeField(withKey: key)
    }
}
