using ElectronicObserver.Data;
using ElectronicObserverTypes;
using Moq;

namespace ElectronicObserverCoreTests
{
	public static class Equipment
	{
		public static IEquipmentData MainGun41K2Triple(int level = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.Level).Returns(level);
			mock.Setup(e => e.MasterEquipment.Firepower).Returns(23);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.MainGunLarge);
			mock.Setup(e => e.MasterEquipment.IsMainGun).Returns(true);

			return mock.Object;
		}

		public static IEquipmentData MainGun46Kai(int level = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.Level).Returns(level);
			mock.Setup(e => e.MasterEquipment.Firepower).Returns(27);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.MainGunLarge);
			mock.Setup(e => e.MasterEquipment.IsMainGun).Returns(true);

			return mock.Object;
		}

		public static IEquipmentData OTO(int level = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.Level).Returns(level);
			mock.Setup(e => e.MasterEquipment.Firepower).Returns(8);
			mock.Setup(e => e.MasterEquipment.Accuracy).Returns(1);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.SecondaryGun);
			mock.Setup(e => e.MasterEquipment.IsSecondaryGun).Returns(true);

			return mock.Object;
		}

		public static IEquipmentData Quint(int level = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.Level).Returns(level);
			mock.Setup(e => e.MasterEquipment.Torpedo).Returns(12);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.Torpedo);
			mock.Setup(e => e.MasterEquipment.IsTorpedo).Returns(true);

			return mock.Object;
		}

		public static IEquipmentData YuraReconSkilled(int level = 0, int proficiency = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.MasterEquipment.LOS).Returns(8);
			mock.Setup(e => e.MasterEquipment.Firepower).Returns(2);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneRecon);

			return mock.Object;
		}

		public static IEquipmentData Zuiunk2(int level = 0, int proficiency = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.Level).Returns(level);
			mock.Setup(e => e.MasterEquipment.Firepower).Returns(2);
			mock.Setup(e => e.EquipmentId).Returns(EquipmentId.SeaplaneBomber_ZuiunKaiNi_634AirGroup);
			mock.Setup(e => e.MasterEquipment.LOS).Returns(7);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneBomber);

			return mock.Object;
		}

		public static IEquipmentData Zuiun634(int level = 0, int proficiency = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.Level).Returns(level);
			mock.Setup(e => e.EquipmentId).Returns(EquipmentId.SeaplaneBomber_Zuiun_634AirGroup);
			mock.Setup(e => e.MasterEquipment.LOS).Returns(6);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneBomber);

			return mock.Object;
		}


		public static IEquipmentData T1ApShell(int level = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.Level).Returns(level);
			mock.Setup(e => e.MasterEquipment.Firepower).Returns(9);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.APShell);

			return mock.Object;
		}

		public static IEquipmentData ReppuuKaiNiE(int level = 0, int proficiency = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.MasterEquipment.Firepower).Returns(2);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.CarrierBasedFighter);
			mock.Setup(e => e.MasterEquipment.IsNightFighter).Returns(true);
			mock.Setup(e => e.MasterEquipment.IsNightAircraft).Returns(true);

			return mock.Object;
		}

		public static IEquipmentData ReppuuKaiNiESkilled(int level = 0, int proficiency = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.MasterEquipment.Firepower).Returns(2);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.CarrierBasedFighter);
			mock.Setup(e => e.MasterEquipment.IsNightFighter).Returns(true);
			mock.Setup(e => e.MasterEquipment.IsNightAircraft).Returns(true);

			return mock.Object;
		}

		public static IEquipmentData T97NightAttacker(int level = 0, int proficiency = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.MasterEquipment.Torpedo).Returns(7);
			mock.Setup(e => e.MasterEquipment.ASW).Returns(6);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.CarrierBasedTorpedo);
			mock.Setup(e => e.MasterEquipment.IsNightAttacker).Returns(true);
			mock.Setup(e => e.MasterEquipment.IsNightAircraft).Returns(true);

			return mock.Object;
		}

		public static IEquipmentData SwordfishMk3Skilled(int level = 0, int proficiency = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.MasterEquipment.Firepower).Returns(4);
			mock.Setup(e => e.MasterEquipment.Torpedo).Returns(8);
			mock.Setup(e => e.MasterEquipment.ASW).Returns(10);
			mock.Setup(e => e.MasterEquipment.Accuracy).Returns(4);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.CarrierBasedTorpedo);
			mock.Setup(e => e.MasterEquipment.IsSwordfish).Returns(true);

			return mock.Object;
		}

		public static IEquipmentData T52CIwai(int level = 0, int proficiency = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.MasterEquipment.LOS).Returns(1);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.CarrierBasedFighter);

			return mock.Object;
		}

		public static IEquipmentData T99Egusa(int level = 0, int proficiency = 0)
		{
				var mock = new Mock<IEquipmentData>();

				mock.Setup(e => e.MasterEquipment.Bomber).Returns(11);
				mock.Setup(e => e.MasterEquipment.LOS).Returns(3);
				mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.CarrierBasedBomber);

				return mock.Object;
		}

		public static IEquipmentData T97Tomonaga(int level = 0, int proficiency = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.MasterEquipment.Torpedo).Returns(11);
			mock.Setup(e => e.MasterEquipment.LOS).Returns(4);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.CarrierBasedTorpedo);

			return mock.Object;
		}


		public static IEquipmentData NightScamp(int level = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.MasterEquipment.Firepower).Returns(1);
			mock.Setup(e => e.MasterEquipment.IsNightAviationPersonnel).Returns(true);

			return mock.Object;
		}

		public static IEquipmentData HFDF(int level = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.MasterEquipment.ASW).Returns(15);
			mock.Setup(e => e.MasterEquipment.IsSonar).Returns(true);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.Sonar);

			return mock.Object;
		}

		public static IEquipmentData AswTorpedo(int level = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.MasterEquipment.ASW).Returns(20);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.DepthCharge);

			return mock.Object;
		}

		public static IEquipmentData Type2DepthCharge(int level = 0)
		{
			var mock = new Mock<IEquipmentData>();

			mock.Setup(e => e.MasterEquipment.ASW).Returns(7);
			mock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.DepthCharge);
			mock.Setup(e => e.MasterEquipment.IsDepthCharge).Returns(true);

			return mock.Object;
		}
	}
}
