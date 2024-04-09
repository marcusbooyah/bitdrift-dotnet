## Environment setup

Install prerequisites:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [.NET MAUI workload](https://github.com/dotnet/core/blob/main/release-notes/6.0/install-maui.md#cli-installation) (via Visual Studio or CLI ```dotnet workload install maui```)
- [Android SDK](https://developer.android.com/tools)
- [Android Studio](https://developer.android.com/studio)
- [Objective-Sharpie](https://aka.ms/objective-sharpie)
- [Visual Studio](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/download)
- [Xcode](https://developer.apple.com/xcode/)
- [Xcode Command Line Tools](https://developer.apple.com/download/all/?q=command%20line%20tools) (```xcode-select --install```)

> [!NOTE]
> 
> It's possible to install the [Android SDK](https://developer.android.com/tools) and/or the [Xcode Command Line Tools](https://developer.apple.com/download/all/?q=command%20line%20tools) in a standalone manner. However, installation of the [Xcode Command Line Tools](https://developer.apple.com/download/all/?q=command%20line%20tools) is typically handled via [Xcode](https://developer.apple.com/xcode/). Likewise, [Android SDK](https://developer.android.com/tools) installation is also typically handled via [Android Studio](https://developer.android.com/studio) and/or the [.NET MAUI VS Code Extension](https://aka.ms/mauidevkit-marketplace) as-per the [.NET MAUI Getting Started](https://learn.microsoft.com/dotnet/maui/get-started/installation?view=net-maui-8.0&tabs=visual-studio-code#install-visual-studio-code-and-the-net-maui-extension) documentation.

## Building

The goal is to have bindings and samples building 100% through normal MSBuild invocations.

Each .NET Binding project contains some additional MSBuild logic to help obtain and build the native SDK dependencies along with the native slim binding project.  In some cases this means the target will download native SDKs if they are not already present.

In the `eng/` folder you will find `Common.android.targets` and `Common.macios.targets` files which contain some custom build targets to help with this, and are imported into the binding projects.

### Repository Conventions

Top level folders in the repository generally represent a slim binding around a single native SDK, or in some cases (e.g. Firebase) a related group/set of native SDKs.

Under this top level folder you will find one or both of `android` and `macios` folders, which contain native projects defining the slim wrapper API, .NET binding projects to bind the slim wrapper API, and optionally a platform specific sample showing how to reference the binding in a .NET MAUI app.

Inside of each platform folder will be a `native` folder containing the Xcode or Android Studio Project which references the native SDK dependencies and contains java or Swift code defining the slim wrapper API.

### Modifying the Slim Wrapper API

If the existing API surface in a given sample doesn't expose the functionality you need in your own project from the native SDKs, that's ok, it's time to make your own modifications!

#### Mac/iOS Native Project

Inside the Xcode project you will find one or more .Swift files which define the public API surface for the Slim Binding.  For example, the `register` method for Firebase Messaging is defined as below:

```Swift
@objc(FirebaseMessaging)
public class FirebaseMessaging : NSObject {

    @objc(register:completion:)
    public static func register(apnsToken: NSData, completion: @escaping (String?, NSError?) -> Void) {
        let data = Data(referencing: apnsToken);
        Messaging.messaging().apnsToken = data
        Messaging.messaging().token(completion: { fid, error in
            completion(fid, error as NSError?)
        })
    }
    // ...
}
```

> NOTE: Slim wrapper API types which will be used by the .NET Binding must be declared as `public` and need to be annoted with `@objc(NameOfType)` and methods also need to be `public`, and can also benefit from similar annotations `@objc(methodName:parameter1:)` where the name and parameters are specified which help influence the binding which objective sharpie will generate.


#### Mac/iOS Binding Project

You can see in this method that the public API surface only uses types which iOS for .NET already is aware of: `NSData`, `String`, `NSError` and a callback.

In the `FirebaseMessaging.Binding` project, the `ApiDefinitions.cs` file contains the binding definition for this slim wrapper API:

```csharp
using System;
using Foundation;

namespace Firebase
{
    [BaseType(typeof(NSObject))]
    interface FirebaseMessaging
    {
        [Static]
        [Export("register:completion:")]
        [Async]
        void Register(NSData nativePush, Action<string?, NSError?> completion);

        // ...
    }
}
```

#### Modifying Mac/iOS

Let's say you want to add a method for unregistering.  The Swift code would look something like this:

```Swift
@objc(unregister:)
public static func unregister(completion: @escaping (NSError?) -> Void) {
    // need delegate to watch for fcmToken updates
    Messaging.messaging().deleteToken(completion: { error in
        completion(error as NSError?)
    })
}
```

The other half will be to update the `ApiDefinitions.cs` file in the binding project to expose this new method.  There are two ways you can go about this:

1. You can manually add the required code
2. When the binding project builds, objective sharpie is run and an `ApiDefinitions.cs` file is generated inside of the `native/macios/messaging/.build/Binding` folder (this path will vary based on the project you are working on of course).  You can try to find the relevant changes from this file and copy them over manually, or try copying over the whole file and looking at the diff to find the part you need.

In this case, the changes to `ApiDefinitions.cs` would be:

```csharp
[Static]
[Export("unregister:")]
[Async]
void UnRegister(Action completion);
```

Once you've made these changes, you can rebuild the Binding project, and the new API will be ready to use from your .NET MAUI project.

> NOTE: Binding projects for Mac/iOS are not using source generators, and so the project system and inteillisense may not know about the new API's until you've rebuilt the binding project, and reload the solution so that the project reference picks up the newer assembly which was built.  Your app project should still compile regardless of intellisense errors.

#### Android Native Project

Inside the Android Studio project you will find a module directory which contains .java definining the public API surface for the Slim Binding.  For example, the `initialize` method for Facebook is defined as below:

```java
package com.microsoft.mauifacebook;

import android.app.Activity;
import android.app.Application;
import android.os.Bundle;
import android.util.Log;

import com.facebook.LoggingBehavior;
import com.facebook.appevents.AppEventsLogger;

public class FacebookSdk {

    static AppEventsLogger _logger;

    public static void initialize(Activity activity, Boolean isDebug) {
        Application application = activity.getApplication();

        if (isDebug) {
            com.facebook.FacebookSdk.setIsDebugEnabled(true);
        }

        com.facebook.FacebookSdk.addLoggingBehavior(LoggingBehavior.APP_EVENTS);

        AppEventsLogger.activateApp(application);

        _logger = AppEventsLogger.newLogger(activity);
    }

    // ...
}
```

#### Android Binding Project

You can see in this method that the public API surface only uses types which Android for .NET already is aware of: `Activity` and `Boolean`.

In the `Facebook.Android.Binding` project, the `Transforms/Metadata.xml` file contains only some xml to describe how to map the java package name (`com.microsoft.mauifacebook`) to a more C# friendly namespace (`Facebook`).  Generally android bindings are more 'automatic' than  Mac/iOS at this point, and you rarely should need to make changes to these transform files.

```xml
<metadata>
    <attr path="/api/package[@name='com.microsoft.mauifacebook']" name="managedName">Facebook</attr>
</metadata>
```

#### Modifying Android

Let's say you want to add a method for logging an event.  The java code would look something like this:

```java
public static void logEvent(String eventName) {
    _logger.logEvent(eventName);
}
```

From this simple change, binding project requires no updates to the `Transforms/Metadata.xml` or other files.  You can simply rebuild the Binding project, and the new API will be ready to use from your .NET MAUI project.

> NOTE: Binding projects for Android are not using source generators, and so the project system and inteillisense may not know about the new API's until you've rebuilt the binding project, and reload the solution so that the project reference picks up the newer assembly which was built.  Your app project should still compile regardless of intellisense errors.

### Using this with your own .NET App/Project

There are several ways you can use these samples in your own project.

1. Submodule or otherwise clone this repo into your project, and reference the projects directly as outlined above in the [Get Started section](#get-started)
2. Build the binding projects and consume the .dll assembly artifacts

> NOTE: Getting this repository building in CI and producing assembly and/or NuGet artifacts is a near term goal but is not currently available.


## Contributing

There are a number of ways you might consider contributing back to this project.

### Expanding Slim Wrapper API Surface

If you feel your modifications to expose more functionality for an existing slim binding project in this repository would be generic and useful enough to the majority of developers, pull requests are welcome!  Please keep in mind that for a contribution to be considered, it needs to be broadly applicable to .NET developers and it may require some collaboration with maintainers to refine the API surface changes.

### Adding new Slim Wrappers

The goal of this repository is to provide a solid foundation of starting points for interop with native SDKs.  Given the most challenging part of creating a slim binding is generally the boilerplate setup and figuring out the native dependency chain and acquisition, we welcome contributions of new libraries as long as they follow the repository conventions and can be considered useful to a wide enough audience (e.g. an internal company's native SDK would not be a good candidate).