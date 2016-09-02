using ElectronicObserver.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver {
	static class Program {
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main() {

			var mutex = new System.Threading.Mutex( false, Application.ExecutablePath.Replace( '\\', '/' ) );

			if ( !mutex.WaitOne( 0, false ) ) {
				// 多重起動禁止
				MessageBox.Show(Utility.SoftwareInformation.SoftwareNameEnglish + Properties.Resources.NoMultipleStart, Utility.SoftwareInformation.SoftwareNameEnglish, MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new FormMain() );

			mutex.ReleaseMutex();
		}
	}
}
