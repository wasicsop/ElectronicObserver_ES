using CefSharp;
using CefSharp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.CefOp
{
	/// <summary>
	/// (たぶん)ドラッグ&ドロップを無効化します。
	/// </summary>
	public class DragHandler : IDragHandler
	{

		bool IDragHandler.OnDragEnter(IWebBrowser chromiumWebBrowser, IBrowser browser, IDragData dragData, DragOperationsMask mask)
		{
			return true;
		}

		void IDragHandler.OnDraggableRegionsChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IList<DraggableRegion> regions)
		{
			throw new NotImplementedException();
		}
	}
}
