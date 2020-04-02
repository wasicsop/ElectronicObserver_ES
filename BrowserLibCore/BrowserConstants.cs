using System;
using System.IO;

namespace BrowserLibCore
{
	public class BrowserConstants
	{
#if DEBUG
		public static string CachePath => "BrowserCache";
#else
		public static string CachePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"ElectronicObserver\CEF");
#endif
	}
}