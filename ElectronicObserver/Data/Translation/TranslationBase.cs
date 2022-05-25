using System;
using System.IO;
using System.Text.Json;
using DynaJson;

namespace ElectronicObserver.Data.Translation;

public abstract class TranslationBase
{
	public TranslationBase() { }
	public abstract void Initialize();

	// todo: use the generic version instead of this one for all translation files
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

	protected T? Load<T>(string path) where T : class
	{
		try
		{
			using StreamReader sr = new StreamReader(path);

			return JsonSerializer.Deserialize<T>(sr.ReadToEnd());
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
