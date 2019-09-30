using CefSharp;
using CefSharp.Handler;
namespace Browser.CefOp
{
	internal class Cef_pixihandler : ResourceRequestHandler
	{
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
	}
}