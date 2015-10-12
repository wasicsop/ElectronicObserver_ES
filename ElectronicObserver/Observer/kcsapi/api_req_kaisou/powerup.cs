using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou {

	public class powerup : APIBase {


		public override void OnRequestReceived( Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;


			db.Fleet.LoadFromRequest( APIName, data );


			foreach ( string id in data["api_id_items"].Split( ",".ToCharArray() ) ) {

				int shipID = int.Parse( id );


				ShipData ship = db.Ships[shipID];
				for ( int i = 0; i < ship.Slot.Count; i++ ) {
					if ( ship.Slot[i] != -1 )
						db.Equipments.Remove( ship.Slot[i] );
				}

				Utility.Logger.Add( 2, ship.NameWithLevel + LoggerRes.Decomissioned );
				db.Ships.Remove( shipID );

			}

			base.OnRequestReceived( data );
		}


		public override void OnResponseReceived( dynamic data ) {

			KCDatabase db = KCDatabase.Instance;

			var ship = db.Ships[(int)data.api_ship.api_id];
			if ( ship != null )
				ship.LoadFromResponse( APIName, data.api_ship );

			db.Fleet.LoadFromResponse( APIName, data.api_deck );


			if ( Utility.Configuration.Config.Log.ShowSpoiler )
				Utility.Logger.Add( 2, string.Format( LoggerRes.ModernizationResult, ship.NameWithLevel, ( (int)data.api_powerup_flag ) != 0 ? Window.GeneralRes.Success : Window.GeneralRes.Failure ) );

			base.OnResponseReceived( (object)data );
		}



		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return true; } }


		public override string APIName {
			get { return "api_req_kaisou/powerup"; }
		}

	}


}
