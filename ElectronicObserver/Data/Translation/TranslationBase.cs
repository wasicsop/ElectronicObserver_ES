using DynaJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ElectronicObserver.Data.Translation;

public abstract class TranslationBase
{
	public TranslationBase() { }
	public abstract void Initialize();
	public dynamic Load(string path)
	{
		try
		{
			using StreamReader sr = new StreamReader(path);
			{
				var json = JsonObject.Parse(sr.ReadToEnd());
				return json;
			}
		}
		catch (FileNotFoundException)
		{
			Utility.Logger.Add(3, GetType().Name + ": File does not exists.");
		}
		catch (DirectoryNotFoundException)
		{
			Utility.Logger.Add(3, GetType().Name + ": File does not exists.");
		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, "Failed to load " + GetType().Name);
		}

		return null;
	}
}