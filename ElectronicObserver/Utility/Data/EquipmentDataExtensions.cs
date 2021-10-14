using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Utility.Data;

public static class EquipmentDataExtensions
{
	public static bool IsSeaplane(this IEquipmentDataMaster equip) => equip.CategoryType switch
	{
		EquipmentTypes.SeaplaneRecon => true,
		EquipmentTypes.SeaplaneBomber => true,
		_ => false
	};

	public static bool IsSuisei634(this IEquipmentData? equip) => equip?.EquipmentId switch
	{
		EquipmentId.CarrierBasedBomber_SuiseiModel22_634AirGroup => true,
		EquipmentId.CarrierBasedBomber_SuiseiModel22_634AirGroupSkilled => true,
		EquipmentId.CarrierBasedBomber_SuiseiModel12_634AirGroupwType3ClusterBombs => true,
		_ => false
	};

	public static bool IsZuiun(this IEquipmentData? equip) => equip?.EquipmentId switch
	{
		EquipmentId.SeaplaneBomber_Zuiun => true,
		EquipmentId.SeaplaneBomber_Zuiun_634AirGroup => true,
		EquipmentId.SeaplaneBomber_ZuiunModel12 => true,
		EquipmentId.SeaplaneBomber_ZuiunModel12_634AirGroup => true,
		EquipmentId.SeaplaneBomber_Zuiun_631AirGroup => true,
		EquipmentId.SeaplaneBomber_Zuiun_634AirGroupSkilled => true,
		EquipmentId.SeaplaneBomber_ZuiunKaiNi_634AirGroup => true,
		EquipmentId.SeaplaneBomber_ZuiunKaiNi_634AirGroupSkilled => true,
		_ => false
	};

	/// <summary>
	/// Aircraft that aren't night aircraft but can still participate in cvnci
	/// </summary>
	public static bool IsNightCapableAircraft(this IEquipmentData? equip) =>
		equip?.MasterEquipment.IsSwordfish == true ||
		equip?.EquipmentId == EquipmentId.CarrierBasedBomber_SuiseiModel12_wType31PhotoelectricFuzeBombs ||
		equip?.EquipmentId == EquipmentId.CarrierBasedBomber_ZeroFighterbomberModel62_IwaiSquadron;

}