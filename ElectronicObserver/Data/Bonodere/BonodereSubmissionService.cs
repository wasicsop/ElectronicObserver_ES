﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Wpf.SenkaLeaderboard;

namespace ElectronicObserver.Data.Bonodere;

public class BonodereSubmissionService
{
	private BonodereSubmissionTranslationViewModel BonodereSubmission { get; }

	private BonodereHttpClient BonodereClient { get; }

	public string Username { get; set; } = "";
	
	public BonodereSubmissionService(BonodereSubmissionTranslationViewModel translations)
	{
		BonodereSubmission = translations;
		BonodereClient = new(BonodereSubmission);

		_ = LoginFromSavedToken();

		Configuration.Instance.ConfigurationChanged += async () => await LoginFromSavedToken();
	}
	
	public async Task<BonodereLoginResponse?> Login(string login, SecureString password)
	{
		return await BonodereClient.Login(login, password);
	}

	public async Task LoginFromSavedToken()
	{

		if (string.IsNullOrEmpty(Configuration.Config.DataSubmission.BonodereToken)) return;

		try
		{
			BonodereUserDataResponse? loginResponse = await BonodereClient.LoginFromToken(Configuration.Config.DataSubmission.BonodereToken, Configuration.Config.DataSubmission.BonodereUserId);

			if (loginResponse is not null)
			{
				Username = loginResponse.User.UserInfo.Username;
			}
		}
		catch (Exception ex)
		{
			LogError(ex);
		}
	}

	public async Task Logout()
	{
		if (!BonodereClient.IsReady) return;

		try
		{
			await BonodereClient.Logout();
			Username = "";
		}
		catch (Exception ex)
		{
			LogError(ex);
		}
	}

	public async Task SubmitData(List<SenkaEntryModel> data)
	{
		if (!BonodereClient.IsReady) return;

		if (!IsDataValid(data))
		{
			Logger.Add(2, $"{BonodereSubmission.Error}: {BonodereSubmission.InconsistantDataDetected}");
			return;
		}

		try
		{
			await BonodereClient.SubmitData(data);
		}
		catch (Exception ex)
		{
			LogError(ex);
		}
	}

	private bool IsDataValid(List<SenkaEntryModel> data)
	{
		if (data.Any(entry => entry.Points < 0)) return false;
		if (data.Any(entry => entry.Position > 500)) return false;
		if (data.Any(entry => entry.Position <= 0)) return false;
		if (data.Any(entry => !entry.IsKnown)) return false;

		return true;
	}

	private void LogError(Exception e)
	{
		Logger.Add(2, BonodereSubmission.Error, e);
	}
}
