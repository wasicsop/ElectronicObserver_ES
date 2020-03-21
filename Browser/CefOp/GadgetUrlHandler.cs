using CefSharp;
using CefSharp.Handler;

namespace Browser.CefOp
{
	internal class GadgetUrlHandler : ResourceRequestHandler
	{
		protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
		{
			if (request.Url.Contains(@"gadget_html5"))
				return new GadgetReplaceFilter("203.104.209.7/gadget_html5/", "18.176.189.52/gadget_html5/");
			return base.GetResourceResponseFilter(chromiumWebBrowser, browser, frame, request, response);
		}
	}
}