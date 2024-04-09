#nullable enable
using System;
using Foundation;

namespace BitDriftModule
{
	[BaseType (typeof(NSObject))]
	interface Logger
	{
		[Static]
		[Export ("configureWithKey:url:strategy:")]
		void ConfigureWithKey (string key, string url, string strategy);

		[Static]
		[Export ("log:message:fields:")]
		void Log (double level, string message, NSDictionary<NSString, NSString> fields);

		[Static]
		[NullAllowed, Export ("sessionId")]
		string SessionId();

		[Static]
		[NullAllowed, Export ("sessionUrl")]
		string SessionUrl();

		[Static]
		[Export ("startNewSession")]
		void StartNewSession();

		[Static]
		[Export ("addFieldWithKey:value:")]
		void AddFieldWithKey (string key, string value);

		[Static]
		[Export ("removeFieldWithKey:")]
		void RemoveFieldWithKey (string key);
	}
}
