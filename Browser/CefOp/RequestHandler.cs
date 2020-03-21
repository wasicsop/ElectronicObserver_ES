using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.CefOp
{
	public class CefRequestHandler : RequestHandler
	{
		public delegate void RenderProcessTerminatedEventHandler(string message);
		public event RenderProcessTerminatedEventHandler RenderProcessTerminated;

		/// <summary>
		/// 戻る/進む操作をブロックします。
		/// </summary>
		protected override bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
		{
			if ((request.TransitionType & TransitionType.ForwardBack) != 0)
			{
				return true;
			}
			return base.OnBeforeBrowse(chromiumWebBrowser, browser, frame, request, userGesture, isRedirect);
		}

		/// <summary>
		/// 描画プロセスが何らかの理由で落ちた際の処理を行います。
		/// </summary>
		protected override void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
		{
			// note: out of memory (例外コード: 0xe0000008) でクラッシュした場合、このイベントは呼ばれない

			string ret = "ブラウザの描画プロセスが";
			switch (status)
			{
				case CefTerminationStatus.AbnormalTermination:
					ret += "正常に終了しませんでした。";
					break;
				case CefTerminationStatus.ProcessWasKilled:
					ret += "何者かによって殺害されました。";
					break;
				case CefTerminationStatus.ProcessCrashed:
					ret += "クラッシュしました。";
					break;
				default:
					ret += "謎の死を遂げました。";
					break;
			}
			ret += "再読み込みすると復帰します。";

			RenderProcessTerminated(ret);
		}
		protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool iNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
		{
			//NOTE: In most cases you examine the request.Url and only handle requests you are interested in
			if (request.Url.Contains(@"/kcs2/resources/bgm/"))
			{
				return new ResRequestHandler();
			}
			if (request.Url.Contains("gadget_html5"))
				return new GadgetUrlHandler();
			return null;
		}
	}
}
