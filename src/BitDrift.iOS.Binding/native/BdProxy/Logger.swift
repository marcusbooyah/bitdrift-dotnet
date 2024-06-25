import Capture

@objc(Logger)
public class Logger: NSObject {

    @objc
    public static func configure(key: String, url: String, strategy: String) {
        do
        {
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
        } catch {
           print("Unexpected error: \(error).")
        }
    }

    @objc
    public static func log(_ level: Double, message: String, fields: [String: String]) {
        do
        {
            switch level {
            case 0.0:
                Capture.Logger.logTrace(message, fields: fields)
            case 1.0:
                Capture.Logger.logDebug(message, fields: fields)
            case 2.0:
                Capture.Logger.logInfo(message, fields: fields)
            case 3.0:
                Capture.Logger.logWarning(message, fields: fields)
            case 4.0:
                Capture.Logger.logError(message, fields: fields)
            default:
                return
            }
        } catch {
           print("Unexpected error: \(error).")
        }
    }
    
    @objc
    public static func sessionId() -> String
    {
        do
        {
            let result = Capture.Logger.sessionID;
            return result!;
        } catch {
           print("Unexpected error: \(error).")
           return ""
        }
    }
    
    @objc
    public static func sessionUrl() -> String
    {
        do
        {
            let result = Capture.Logger.sessionURL;
            return result!;
        } catch {
           print("Unexpected error: \(error).")
           return ""
        }
    }
    
    @objc
    public static func startNewSession()
    {
        do
        {
            Capture.Logger.startNewSession();
        } catch {
           print("Unexpected error: \(error).")
        }
    }
    
    @objc
    public static func addField(key: String, value: String)
    {
        do
        {
            Capture.Logger.addField(withKey: key, value: value);
        } catch {
           print("Unexpected error: \(error).")
        }
    }
    
    @objc
    public static func removeField(key: String)
    {
        do
        {
            Capture.Logger.removeField(withKey: key)
        } catch {
           print("Unexpected error: \(error).")
        }
    }
}
