using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class PhaseNightBattleAttackViewModel : AttackViewModelBase
{
	public BattleIndex AttackerIndex { get; }
	public IShipData Attacker { get; }
	public int AttackerHpBeforeAttack { get; }

	public BattleIndex DefenderIndex { get; }
	public IShipData Defender { get; }
	public int DefenderHpBeforeAttack { get; }

	private List<NightAttack> Attacks { get; }
	private IEquipmentData? UsedDamecon { get; }
	public string DamageDisplay { get; }

	public PhaseNightBattleAttackViewModel(BattleFleets fleets, BattleIndex attacker,
		BattleIndex defender, NightAttackKind attackType, List<PhaseNightBattleDefender> defenders)
	{
		AttackerIndex = attacker;
		Attacker = fleets.GetShip(AttackerIndex)!;
		DefenderIndex = defender;
		Defender = fleets.GetShip(DefenderIndex)!;
		Attacks = defenders
			.Select(d => new NightAttack
			{
				Attacker = Attacker,
				Defender = fleets.GetShip(d.Defender)!,
				AttackKind = attackType,
				Damage = d.Damage,
				GuardsFlagship = d.GuardsFlagship,
				CriticalFlag = d.CriticalFlag,
			})
			.ToList();

		AttackerHpBeforeAttack = Attacker.HPCurrent;
		DefenderHpBeforeAttack = Defender.HPCurrent;

		int hpAfterAttacks = Math.Max(0, DefenderHpBeforeAttack - Attacks.Sum(a => a.Damage));

		if (hpAfterAttacks <= 0 && GetDamecon(Defender) is { } damecon)
		{
			UsedDamecon = damecon;
		}

		DamageDisplay =
			$"[{ElectronicObserverTypes.Attacks.NightAttack.AttackDisplay(attackType)}] " +
			$"{string.Join(", ", Attacks.Select(AttackDisplay))}";

		if (DefenderHpBeforeAttack > 0 && DefenderHpBeforeAttack != hpAfterAttacks)
		{
			DamageDisplay += $" ({DefenderHpBeforeAttack} → {hpAfterAttacks})";
		}

		DamageDisplay += UsedDamecon switch
		{
			{ EquipmentId: EquipmentId.DamageControl_EmergencyRepairGoddess } => $"　{BattleRes.GoddessActivated} HP{Defender.HPMax}",
			{ EquipmentId: EquipmentId.DamageControl_EmergencyRepairPersonnel } => $"　{BattleRes.DameconActivated} HP{(int)(Defender.HPMax * 0.2)}",
			_ => null,
		};
	}

	private static string AttackDisplay(NightAttack nightAttack)
		=> AttackDisplay(nightAttack.GuardsFlagship, nightAttack.Damage, nightAttack.CriticalFlag);
}
