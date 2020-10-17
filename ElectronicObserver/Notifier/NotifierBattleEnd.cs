using System.Collections.Generic;
using System.Linq;
using System.Timers;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Notifier
{
	public class NotifierBattleEnd : NotifierBase
	{
		public Configuration.ConfigurationData.ConfigNotifierBattleEnd Config { get; }
		private Timer? Timer { get; set; }

		public NotifierBattleEnd(Configuration.ConfigurationData.ConfigNotifierBattleEnd config)
			: base(config)
		{
			Initialize();

			Config = config;
		}

		private void Initialize()
		{
			DialogData.Title = "Battle notification";

			APIObserver o = APIObserver.Instance;

			o["api_port/port"].ResponseReceived += CloseAll;

			o["api_req_sortie/battleresult"].ResponseReceived += BattleFinished;
			o["api_req_combined_battle/battleresult"].ResponseReceived += BattleFinished;

			o["api_req_sortie/battle"].ResponseReceived += BattleStarted;
			o["api_req_battle_midnight/battle"].ResponseReceived += BattleStarted;
			o["api_req_battle_midnight/sp_midnight"].ResponseReceived += BattleStarted;
			o["api_req_sortie/airbattle"].ResponseReceived += BattleStarted;
			o["api_req_sortie/ld_airbattle"].ResponseReceived += BattleStarted;
			o["api_req_sortie/night_to_day"].ResponseReceived += BattleStarted;
			o["api_req_sortie/ld_shooting"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/battle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/battle_water"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/airbattle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/midnight_battle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/sp_midnight"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/ld_airbattle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/ec_battle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/ec_midnight_battle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/ec_night_to_day"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/each_battle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/each_battle_water"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/ld_shooting"].ResponseReceived += BattleStarted;
		}

		private void BattleStarted(string apiname, dynamic data)
		{
			Timer = new Timer
			{
				Enabled = Config.IdleTimerEnabled,
				Interval = Config.IdleSeconds * 1000,
				AutoReset = false,
			};

			Timer.Elapsed += (s, e) =>
			{
				Notify("Idle warning.");
			};
		}

		private void BattleFinished(string apiname, dynamic data)
		{
			Timer?.Stop();
			Notify("Battle ended.");
		}

		void CloseAll(string apiname, dynamic data)
		{
			DialogData.OnCloseAll();
		}

		public void Notify(string message)
		{
			DialogData.Message = message;
			base.Notify();
		}


		public override void ApplyToConfiguration(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
		{
			base.ApplyToConfiguration(config);

			if (config is Configuration.ConfigurationData.ConfigNotifierBattleEnd c)
			{
				c.IdleTimerEnabled = Config.IdleTimerEnabled;
				c.IdleSeconds = Config.IdleSeconds;
			}
		}
	}
}