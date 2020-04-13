using System.ComponentModel.DataAnnotations;

namespace ElectronicObserverTypes
{
	public enum ShipTypes
	{
        Unknown,

        /// <summary>海防艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "Escort")]
        Escort = 1,

        /// <summary>駆逐艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "Destroyer")]
        Destroyer = 2,

        /// <summary>軽巡洋艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "LightCruiser")]
        LightCruiser = 3,

        /// <summary>重雷装巡洋艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "TorpedoCruiser")]
        TorpedoCruiser = 4,

        /// <summary>重巡洋艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "HeavyCruiser")]
        HeavyCruiser = 5,

        /// <summary>航空巡洋艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "AviationCruiser")]
        AviationCruiser = 6,

        /// <summary>軽空母</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "LightAircraftCarrier")]
        LightAircraftCarrier = 7,

        /// <summary>巡洋戦艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "Battlecruiser")]
        Battlecruiser = 8,

        /// <summary>戦艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "Battleship")]
        Battleship = 9,

        /// <summary>航空戦艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "AviationBattleship")]
        AviationBattleship = 10,

        /// <summary>正規空母</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "AircraftCarrier")]
        AircraftCarrier = 11,

        /// <summary>超弩級戦艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "SuperDreadnoughts")]
        SuperDreadnoughts = 12,

        /// <summary>潜水艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "Submarine")]
        Submarine = 13,

        /// <summary>潜水空母</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "SubmarineAircraftCarrier")]
        SubmarineAircraftCarrier = 14,

        /// <summary>輸送艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "Transport")]
        Transport = 15,

        /// <summary>水上機母艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "SeaplaneTender")]
        SeaplaneTender = 16,

        /// <summary>揚陸艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "AmphibiousAssaultShip")]
        AmphibiousAssaultShip = 17,

        /// <summary>装甲空母</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "ArmoredAircraftCarrier")]
        ArmoredAircraftCarrier = 18,

        /// <summary>工作艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "RepairShip")]
        RepairShip = 19,

        /// <summary>潜水母艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "SubmarineTender")]
        SubmarineTender = 20,

        /// <summary>練習巡洋艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "TrainingCruiser")]
        TrainingCruiser = 21,

        /// <summary>補給艦</summary>
        [Display(ResourceType = typeof(Properties.ShipTypes), Name = "FleetOiler")]
        FleetOiler = 22,

	}

    public enum ShipTypeGroup
    {
        [Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "Destroyers")]
        Destroyers,
        [Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "Escorts")]
        Escorts,
        [Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "LightCruisers")]
        LightCruisers,
        [Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "HeavyCruisers")]
        HeavyCruisers,
        [Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "Battleships")]
        Battleships,
        [Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "Carriers")]
        Carriers,
        [Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "Submarines")]
        Submarines,
        [Display(ResourceType = typeof(Properties.ShipTypeGroups), Name = "Auxiliaries")]
        Auxiliaries
    }
}
