using ElectronicObserver.Window;

namespace ElectronicObserver.ViewModels.Translations
{
	public class FormBattleTranslationViewModel : TranslationBaseViewModel
	{
		public string ABText => Properties.Window.FormBattle.ABText;
		public string AerialPhaseJet => Properties.Window.FormBattle.AerialPhaseJet;
		public string AerialPhase1 => Properties.Window.FormBattle.AerialPhase1;
		public string AerialPhase2 => Properties.Window.FormBattle.AerialPhase2;
		public string AirDefense => Properties.Window.FormBattle.AirDefense;
		public string Contact => Properties.Window.FormBattle.Contact;
		public string None => Properties.Window.FormBattle.None;
		public string AACI => Properties.Window.FormBattle.AACI;
		public string AACIType => Properties.Window.FormBattle.AACIType;
		public string DidNotActivate => Properties.Window.FormBattle.DidNotActivate;
		public string AirBase => Properties.Window.FormBattle.AirBase;
		public string SupportExpedition => Properties.Window.FormBattle.SupportExpedition;
		public string FleetFriendShort => Properties.Window.FormBattle.FleetFriendShort;

		public string RightClickMenu_ShowBattleDetail => GeneralRes.RightClickMenu_ShowBattleDetail;
		public string RightClickMenu_ShowBattleResult => GeneralRes.RightClickMenu_ShowBattleResult;

		public string FormationFriend => GeneralRes.FriendlyFormation;
		public string Formation => GeneralRes.EncounterType;
		public string FormationEnemy => GeneralRes.EnemyFormation;

		public string AirStage2Friend => GeneralRes.ShotDown;
		public string AACutin => GeneralRes.AntiAir;
		public string AirStage2Enemy => GeneralRes.ShotDown;

		public string AirStage1Friend => GeneralRes.ShotDown;
		public string AirSuperiority => GeneralRes.AirSuperiority;
		public string AirStage1Enemy => GeneralRes.ShotDown;

		public string SearchingFriend => GeneralRes.FriendlyScout;
		public string Searching => GeneralRes.Scouting;
		public string SearchingEnemy => GeneralRes.EnemyScout;

		public string FleetFriend => Properties.Window.FormBattle.FleetFriend;
		public string FleetFriendEscort => Properties.Window.FormBattle.FleetFriendEscort;
		public string FleetEnemyEscort => Properties.Window.FormBattle.FleetEnemyEscort;
		public string FleetEnemy => Properties.Window.FormBattle.FleetEnemy;

		public string DamageFriend => Properties.Window.FormBattle.DamageFriend;
		public string WinRank => Properties.Window.FormBattle.WinRank;
		public string DamageEnemy => Properties.Window.FormBattle.DamageEnemy;

		public string Title => GeneralRes.Battle;
	}
}