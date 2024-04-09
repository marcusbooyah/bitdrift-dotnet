package com.euarctos.bitdrift

import android.graphics.Paint.Cap
import io.bitdrift.capture.Capture
import io.bitdrift.capture.Configuration
import io.bitdrift.capture.providers.session.SessionStrategy
import io.bitdrift.capture.replay.SessionReplayConfiguration
import okhttp3.HttpUrl.Companion.toHttpUrl

fun Init(key: String, url: String, sessionStrategy: String = "activity-based") {
    var strategy: SessionStrategy? = null;
    when (sessionStrategy){
        "fixed" -> strategy = SessionStrategy.Fixed()
        "activity-based" -> strategy = SessionStrategy.ActivityBased()
    }

    Capture.Logger.configure(
        apiKey = key,
        apiUrl = url.toHttpUrl(),
        sessionStrategy = strategy!!,
        configuration = Configuration(
            sessionReplayConfiguration = SessionReplayConfiguration(
                skipReplayComposeViews = false,
                captureIntervalMs = 3000,
            )
        )
    )
}

fun Log(level: Double, message: String, jsFields: Map<String, String>?) {

    when (level) {
        0.0 -> Capture.Logger.logTrace(jsFields) { message }
        1.0 -> Capture.Logger.logDebug(jsFields) { message }
        2.0 -> Capture.Logger.logInfo(jsFields) { message }
        3.0 -> Capture.Logger.logWarning(jsFields) { message }
        4.0 -> Capture.Logger.logError(jsFields) { message }
    }
}

fun SessionId(): String?{
    return Capture.Logger.sessionId;
}
fun SessionUrl(): String?{
    return Capture.Logger.sessionUrl;
}

fun StartNewSession(){
    Capture.Logger.startNewSession();
}

fun AddField(key: String, value: String){
    Capture.Logger.addField(key, value);
}

fun RemoveField(key: String){
    Capture.Logger.removeField(key);
}