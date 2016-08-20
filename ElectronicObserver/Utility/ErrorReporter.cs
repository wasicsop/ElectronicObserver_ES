using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility {

	public class ErrorReporter {

		private const string _basePath = "ErrorReport";


		/// <summary>
		/// エラーレポートを作成します。
		/// </summary>
		/// <param name="ex">発生した例外。</param>
		/// <param name="message">追加メッセージ。</param>
		/// <param name="connectionName">エラーが発生したAPI名。省略可能です。</param>
		/// <param name="connectionData">エラーが発生したAPIの内容。省略可能です。</param>
		public static void SendErrorReport( Exception ex, string message, string connectionName = null, string connectionData = null ) {

			Utility.Logger.Add( 3, string.Format( "{0} : {1}", message, ex.Message ) );

			if ( Utility.Configuration.Config.Debug.AlertOnError )
				System.Media.SystemSounds.Hand.Play();


			if ( !Utility.Configuration.Config.Log.SaveErrorReport )
				return;


			string path = _basePath;

			if ( !Directory.Exists( path ) )
				Directory.CreateDirectory( path );


			path = string.Format( "{0}\\{1}.txt", path, DateTimeHelper.GetTimeStamp() );

			try {
				using ( StreamWriter sw = new StreamWriter( path, false, new System.Text.UTF8Encoding( false ) ) ) {

					sw.WriteLine( LoggerRes.ErrorReport, DateTime.Now );
					sw.WriteLine( LoggerRes.ErrorField, ex.GetType().Name );
					sw.WriteLine( ex.Message );
					sw.WriteLine( LoggerRes.AdditionalInfo, message );
					sw.WriteLine( LoggerRes.StackTrace );
					sw.WriteLine( ex.StackTrace );

					if ( connectionName != null && connectionData != null ) {
						sw.WriteLine();
						sw.WriteLine( LoggerRes.APIData, connectionName );
						sw.WriteLine( connectionData );
					}
				}

			} catch ( Exception ) {

				Utility.Logger.Add( 3, string.Format( LoggerRes.FailedSavingErrorReport, ex.Message, ex.StackTrace ) );
			}

		}

	}
}
