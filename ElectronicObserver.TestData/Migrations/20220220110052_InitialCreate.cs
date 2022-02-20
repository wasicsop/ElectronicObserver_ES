using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.TestData.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "MasterEquipment",
				columns: table => new
				{
					EquipmentId = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Name = table.Column<string>(type: "TEXT", nullable: false),
					Armor = table.Column<int>(type: "INTEGER", nullable: false),
					Firepower = table.Column<int>(type: "INTEGER", nullable: false),
					Torpedo = table.Column<int>(type: "INTEGER", nullable: false),
					Bomber = table.Column<int>(type: "INTEGER", nullable: false),
					Aa = table.Column<int>(type: "INTEGER", nullable: false),
					Asw = table.Column<int>(type: "INTEGER", nullable: false),
					Accuracy = table.Column<int>(type: "INTEGER", nullable: false),
					Evasion = table.Column<int>(type: "INTEGER", nullable: false),
					LoS = table.Column<int>(type: "INTEGER", nullable: false),
					Luck = table.Column<int>(type: "INTEGER", nullable: false),
					Range = table.Column<int>(type: "INTEGER", nullable: false),
					CategoryType = table.Column<int>(type: "INTEGER", nullable: false),
					IconType = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MasterEquipment", x => x.EquipmentId);
				});

			migrationBuilder.CreateTable(
				name: "MasterShips",
				columns: table => new
				{
					ShipId = table.Column<int>(type: "INTEGER", nullable: false),
					AlbumNo = table.Column<int>(type: "INTEGER", nullable: false),
					SortId = table.Column<int>(type: "INTEGER", nullable: false),
					Name = table.Column<string>(type: "TEXT", nullable: false),
					NameReading = table.Column<string>(type: "TEXT", nullable: false),
					ShipType = table.Column<int>(type: "INTEGER", nullable: false),
					ShipClass = table.Column<int>(type: "INTEGER", nullable: false),
					RemodelAfterLevel = table.Column<int>(type: "INTEGER", nullable: false),
					RemodelAfterShipId = table.Column<int>(type: "INTEGER", nullable: false),
					RemodelBeforeShipId = table.Column<int>(type: "INTEGER", nullable: false),
					RemodelAmmo = table.Column<int>(type: "INTEGER", nullable: false),
					RemodelSteel = table.Column<int>(type: "INTEGER", nullable: false),
					NeedBlueprint = table.Column<int>(type: "INTEGER", nullable: false),
					NeedCatapult = table.Column<int>(type: "INTEGER", nullable: false),
					NeedActionReport = table.Column<int>(type: "INTEGER", nullable: false),
					NeedAviationMaterial = table.Column<int>(type: "INTEGER", nullable: false),
					NeedArmamentMaterial = table.Column<int>(type: "INTEGER", nullable: false),
					HpMin = table.Column<int>(type: "INTEGER", nullable: false),
					HpMax = table.Column<int>(type: "INTEGER", nullable: false),
					FirepowerMin = table.Column<int>(type: "INTEGER", nullable: false),
					FirepowerMax = table.Column<int>(type: "INTEGER", nullable: false),
					TorpedoMin = table.Column<int>(type: "INTEGER", nullable: false),
					TorpedoMax = table.Column<int>(type: "INTEGER", nullable: false),
					AaMin = table.Column<int>(type: "INTEGER", nullable: false),
					AaMax = table.Column<int>(type: "INTEGER", nullable: false),
					ArmorMin = table.Column<int>(type: "INTEGER", nullable: false),
					ArmorMax = table.Column<int>(type: "INTEGER", nullable: false),
					AswMin = table.Column<int>(type: "INTEGER", nullable: false),
					AswMax = table.Column<int>(type: "INTEGER", nullable: false),
					EvasionMin = table.Column<int>(type: "INTEGER", nullable: false),
					EvasionMax = table.Column<int>(type: "INTEGER", nullable: false),
					LosMin = table.Column<int>(type: "INTEGER", nullable: false),
					LosMax = table.Column<int>(type: "INTEGER", nullable: false),
					LuckMin = table.Column<int>(type: "INTEGER", nullable: false),
					LuckMax = table.Column<int>(type: "INTEGER", nullable: false),
					Speed = table.Column<int>(type: "INTEGER", nullable: false),
					Range = table.Column<int>(type: "INTEGER", nullable: false),
					SlotSize = table.Column<int>(type: "INTEGER", nullable: false),
					Aircraft1 = table.Column<int>(type: "INTEGER", nullable: true),
					Aircraft2 = table.Column<int>(type: "INTEGER", nullable: true),
					Aircraft3 = table.Column<int>(type: "INTEGER", nullable: true),
					Aircraft4 = table.Column<int>(type: "INTEGER", nullable: true),
					Aircraft5 = table.Column<int>(type: "INTEGER", nullable: true),
					DefaultSlot1 = table.Column<int>(type: "INTEGER", nullable: true),
					DefaultSlot2 = table.Column<int>(type: "INTEGER", nullable: true),
					DefaultSlot3 = table.Column<int>(type: "INTEGER", nullable: true),
					DefaultSlot4 = table.Column<int>(type: "INTEGER", nullable: true),
					DefaultSlot5 = table.Column<int>(type: "INTEGER", nullable: true),
					EquippableCategories = table.Column<string>(type: "TEXT", nullable: false),
					BuildingTime = table.Column<int>(type: "INTEGER", nullable: false),
					Material = table.Column<string>(type: "TEXT", nullable: false),
					PowerUp = table.Column<string>(type: "TEXT", nullable: false),
					Rarity = table.Column<int>(type: "INTEGER", nullable: false),
					MessageGet = table.Column<string>(type: "TEXT", nullable: false),
					MessageAlbum = table.Column<string>(type: "TEXT", nullable: false),
					Fuel = table.Column<int>(type: "INTEGER", nullable: false),
					Ammo = table.Column<int>(type: "INTEGER", nullable: false),
					VoiceFlag = table.Column<int>(type: "INTEGER", nullable: false),
					ResourceName = table.Column<string>(type: "TEXT", nullable: false),
					ResourceGraphicVersion = table.Column<string>(type: "TEXT", nullable: true),
					ResourceVoiceVersion = table.Column<string>(type: "TEXT", nullable: true),
					ResourcePortVoiceVersion = table.Column<string>(type: "TEXT", nullable: true),
					OriginalCostumeShipID = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MasterShips", x => x.ShipId);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "MasterEquipment");

			migrationBuilder.DropTable(
				name: "MasterShips");
		}
	}
}
