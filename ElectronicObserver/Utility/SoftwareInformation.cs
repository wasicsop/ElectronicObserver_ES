using ElectronicObserver.Properties;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility {

	/// <summary>
	/// ソフトウェアの情報を保持します。
	/// </summary>
	public static class SoftwareInformation {

		/// <summary>
		/// ソフトウェア名(日本語)
		/// </summary>
		public static string SoftwareNameJapanese {
			get {
				return Resources.SoftwareName;
			}
		}

		/// <summary>
		/// ソフトウェア名(英語)
		/// </summary>
		public static string SoftwareNameEnglish {
			get {
				return Resources.SoftwareName;
			}
		}

		/// <summary>
		/// バージョン(日本語, ソフトウェア名を含みます)
		/// </summary>
		public static string VersionJapanese {
			get {
				return SoftwareNameJapanese + Resources.Version;
				//return SoftwareNameJapanese + "二〇型改三";
			}
		}

		/// <summary>
		/// バージョン(英語)
		/// </summary>
		public static string VersionEnglish {
			get
            {
                return Resources.Version;
            }
		}


		/// <summary>
		/// 更新日時
		/// </summary>
		public static DateTime UpdateTime {
			get {
				return DateTimeHelper.CSVStringToTime( "2015/10/12 00:00:00" );
			}
		}



		private static System.Net.WebClient client;
		private static readonly Uri uri = new Uri("https://www.dropbox.com/s/0xvbhtxnh5wfk5s/version.txt?dl=1");

		public static void CheckUpdate() {

			if ( !Utility.Configuration.Config.Life.CheckUpdateInformation )
				return;

			if ( client == null ) {
				client = new System.Net.WebClient();
				client.Encoding = new System.Text.UTF8Encoding( false );
				client.DownloadStringCompleted += DownloadStringCompleted;
			}

			if ( !client.IsBusy )
				client.DownloadStringAsync( uri );
		}

		private static void DownloadStringCompleted( object sender, System.Net.DownloadStringCompletedEventArgs e ) {

			if ( e.Error != null ) {

				Utility.ErrorReporter.SendErrorReport( e.Error, Resources.UpdateCheckFailed );
				return;

			}

			if ( e.Result.StartsWith( "<!DOCTYPE html>" ) ) {

				Utility.Logger.Add( 3, Resources.BadUpdateURI );
				return;

			}


			try {

				using ( var sr = new System.IO.StringReader( e.Result ) ) {

					DateTime date = DateTimeHelper.CSVStringToTime( sr.ReadLine() );
					string version = sr.ReadLine();
					string description = sr.ReadToEnd();

					if ( UpdateTime < date ) {

						Utility.Logger.Add( 3, Resources.NewVersionFound + version );

						var result = System.Windows.Forms.MessageBox.Show(
							string.Format( Resources.AskForUpdate,
							version, description ),
							Resources.Update, System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Information,
							System.Windows.Forms.MessageBoxDefaultButton.Button1 );


						if ( result == System.Windows.Forms.DialogResult.Yes ) {

							System.Diagnostics.Process.Start( "http://tumblr.rkitsune.com/elecobs" );

						} else if ( result == System.Windows.Forms.DialogResult.Cancel ) {

							Utility.Configuration.Config.Life.CheckUpdateInformation = false;

						}

					} else {

						Utility.Logger.Add( 1, Resources.VersionCurrent );

					}

				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, Resources.UpdateConnectionFailed );
			}

		}

	}

}
