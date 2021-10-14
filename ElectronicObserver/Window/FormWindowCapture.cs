using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Integrate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Wpf.WinformsWrappers;
using WeifenLuo.WinFormsUI.Docking;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ElectronicObserver.Window;

/// <summary>
/// ウィンドウキャプチャ
/// </summary>
public partial class FormWindowCapture : DockContent
{

	public static string WarningMessage => Properties.Window.FormWindowCapture.WarningMessage;

	private object parent;

	public List<FormIntegrate> CapturedWindows { get; } = new();

	public FormWindowCapture(object parent)
	{
		InitializeComponent();

		this.parent = parent;
		this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)IconContent.FormWindowCapture]);
		this.windowCaptureButton.Image = ResourceManager.Instance.Icons.Images[(int)IconContent.FormWindowCapture];

		SystemEvents.SystemShuttingDown += SystemEvents_SystemShuttingDown;
	}

	private void SystemEvents_SystemShuttingDown()
	{
		DetachAll();
	}

	/// <summary>
	/// FormIntegrateが新しく作られたら追加
	/// </summary>
	public void AddCapturedWindow(FormIntegrate form)
	{
		CapturedWindows.Add(form);
		if (parent is FormMainViewModel main)
		{
			FormIntegrateViewModel? integrate = main.Views
				.OfType<FormIntegrateViewModel>()
				.FirstOrDefault(i => i.ContentId == form.PersistString);

			if (integrate is not null)
			{
				integrate.WinformsControl = form;
			}
			else
			{
				main.Views.Add(new FormIntegrateViewModel(form, main) {Visibility = Visibility.Visible});
			}
		}
	}

	/// <summary>
	/// ウィンドウを取り込めていないFormIntegrateでウィンドウの検索と取り込みを実行
	/// </summary>
	public void AttachAll()
	{
		CapturedWindows.ForEach(form => form.Grab());
	}

	/// <summary>
	/// 取り込んだウィンドウを全て開放
	/// </summary>
	public void DetachAll()
	{
		// ウィンドウのzオーダー維持のためデタッチはアタッチの逆順で行う
		for (int i = CapturedWindows.Count; i > 0; --i)
		{
			CapturedWindows[i - 1].Detach();
		}
	}

	/// <summary>
	/// FormIntegrateを全て破棄する
	/// </summary>
	public void CloseAll()
	{
		DetachAll();
		CapturedWindows.ForEach(form => form.Close());
		CapturedWindows.Clear();
		if (parent is FormMainViewModel main)
		{
			List<FormIntegrateViewModel> integrates = main.Views.OfType<FormIntegrateViewModel>().ToList();
			foreach (FormIntegrateViewModel integrate in integrates)
			{
				main.Views.Remove(integrate);
			}
		}
	}

	private void windowCaptureButton_WindowCaptured(IntPtr hWnd)
	{

		int capacity = WinAPI.GetWindowTextLength(hWnd) * 2;
		StringBuilder stringBuilder = new StringBuilder(capacity);
		WinAPI.GetWindowText(hWnd, stringBuilder, stringBuilder.Capacity);

		if (MessageBox.Show(stringBuilder.ToString() + "\r\n" + WarningMessage,
			    Properties.Window.FormWindowCapture.WindowCaptureConfirmation, 
			    MessageBoxButtons.YesNo, MessageBoxIcon.Question)
		    == System.Windows.Forms.DialogResult.Yes)
		{

			FormIntegrate form = new FormIntegrate(parent);
			form.Show(hWnd);
		}
	}

	protected override string GetPersistString()
	{
		return "WindowCapture";
	}


}