using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.CefOp
{
	public class Cef_ResRequestHandler : ResourceRequestHandler
	{
		/**
		bool pixiSettingEnabled;
		public Cef_RequestHandler(bool pixiSettingEnabled) : base()
		{
			this.pixiSettingEnabled = pixiSettingEnabled;
		}
		/// <summary>
		/// レスポンスの置換制御を行います。
		/// </summary>
		protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
		{
			bool pixiSettingEnabled = false;
			if (pixiSettingEnabled && request.Url.Contains(@"/kcs2/index.php"))
				return new ResponseFilterPixiSetting();

			return base.GetResourceResponseFilter(chromiumWebBrowser, browser, frame, request, response);
		}
		**/
		/// <summary>
		/// 特定の通信をブロックします。
		/// </summary>
		protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
		{
			// ログイン直後に勝手に遷移させられ、ブラウザがホワイトアウトすることがあるためブロックする
			if (request.Url.Contains(@"/rt.gsspat.jp/"))
			{
				return CefReturnValue.Cancel;
			}
			// remove range request to allow bgm cachings
			if (request.Url.Contains(@"kcs2/resources/bgm"))
			{
				var headers = request.Headers;
				headers.Remove("Range");
				request.Headers = headers;
			}
			return base.OnBeforeResourceLoad(chromiumWebBrowser, browser, frame, request, callback);
		}



	}
}
