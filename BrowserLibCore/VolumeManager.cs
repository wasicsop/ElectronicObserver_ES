﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;

namespace BrowserLibCore;

/// <summary>
/// 音量やミュートを管理します。
/// </summary>
public class VolumeManager
{

	public uint ProcessID { get; private set; }

	public VolumeManager(uint processID)
	{
		ProcessID = processID;
	}


	/// <summary>
	/// 音量操作のためのデータを取得します。
	/// </summary>
	/// <param name="checkProcessID">プロセス ID を引数にとり、目的のものであれば true を、そうでなければ false を返すデリゲート。</param>
	/// <returns>データ。取得に失敗した場合は null。</returns>
	private static ISimpleAudioVolume? GetVolumeObject(Predicate<uint> checkProcessID)
	{

		ISimpleAudioVolume? ret = null;

		// スピーカーデバイスの取得
		IMMDeviceEnumerator deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
		deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out IMMDevice? speakers);

		if (speakers is null)
		{
			Marshal.ReleaseComObject(deviceEnumerator);
			return null;
		}

		// 列挙のためにセッションマネージャをアクティベート
		Guid IID_IAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
		object o;
		speakers.Activate(ref IID_IAudioSessionManager2, 0, IntPtr.Zero, out o);
		IAudioSessionManager2 mgr = (IAudioSessionManager2)o;

		// セッションの列挙
		IAudioSessionEnumerator sessionEnumerator;
		mgr.GetSessionEnumerator(out sessionEnumerator);
		int count;
		sessionEnumerator.GetCount(out count);

		for (int i = 0; i < count; i++)
		{
			IAudioSessionControl ctl;
			IAudioSessionControl2? ctl2;

			sessionEnumerator.GetSession(i, out ctl);

			ctl2 = ctl as IAudioSessionControl2;

			uint pid = uint.MaxValue;

			if (ctl2 != null)
			{
				ctl2.GetProcessId(out pid);
			}

			if (checkProcessID(pid))
			{
				ret = ctl2 as ISimpleAudioVolume;
				break;
			}


			if (ctl != null)
				Marshal.ReleaseComObject(ctl);

			if (ctl2 != null)
				Marshal.ReleaseComObject(ctl2);
		}

		Marshal.ReleaseComObject(sessionEnumerator);
		Marshal.ReleaseComObject(mgr);
		Marshal.ReleaseComObject(speakers);
		Marshal.ReleaseComObject(deviceEnumerator);


		return ret;
	}





	/// <summary>
	/// 音量操作のためのデータを取得します。
	/// </summary>
	/// <param name="processID">対象のプロセスID。</param>
	/// <returns>データ。取得に失敗した場合は null。</returns>
	private static ISimpleAudioVolume? GetVolumeObject(uint processID) => GetVolumeObject(pid => processID == pid);

	// https://stackoverflow.com/questions/44823368/call-to-managementobjectsearchert-get-crashes-with-invalidcastexception
	// https://stackoverflow.com/questions/23454396/rpc-e-cantcallout-ininputsynccall-when-trying-to-access-usb-device
	private static Dictionary<uint, string> GetCommandLine(string processName)
	{
		Dictionary<uint, string> processes = new();

		// this throws if there's problems with WMI
		// https://github.com/ElectronicObserverEN/ElectronicObserver/issues/231
		try
		{
			Thread thread = new(() =>
			{
				string query =
					"SELECT ProcessId, CommandLine " +
					"FROM Win32_Process " +
					$"WHERE Name = \"{processName}\"" +
					"AND CommandLine LIKE \"%--utility-sub-type=audio.mojom.AudioService%\"";

				using ManagementObjectSearcher searcher = new(query);
				using ManagementObjectCollection objects = searcher.Get();

				foreach (ManagementBaseObject o in objects)
				{
					processes.Add((uint)o["ProcessId"], (string)o["CommandLine"]);
				}
			});
			thread.Start();
			thread.Join();
		}
		catch
		{
			// log?
		}
		

		return processes;
	}

	/// <summary>
	/// 音量操作のためのデータを取得します。 WebView2
	/// </summary>
	/// <param name="processName">対象のプロセス名。</param>
	/// <returns>データ。取得に失敗した場合は null。</returns>
	private static ISimpleAudioVolume? GetVolumeObject(string processName, out uint processID, string proxySettings)
	{
		// Process currentProcess = Process.GetCurrentProcess();
		// https://docs.microsoft.com/en-us/microsoft-edge/webview2/concepts/process-model?tabs=csharp#processes-in-the-webview2-runtime
		// EOBrowser -> WebView2 process -> WebView2 utility process (sound)
		// need to find the WebView2 processes whose grandparent is EOBrowser

		// it looks like the above assumption is wrong
		// somehow explorer can be the parent of the WebView2 process

		/*
		List<Process> processes = Process.GetProcessesByName(processName)
			.Select(p => (Process: p, Parent: GetParentProcess(p)))
			.Where(t => t.Parent is not null && GetParentProcess(t.Parent)?.Id == currentProcess.Id)
			.Select(t => t.Process)
			.ToList();
		*/

		Dictionary<uint, string> webView2AudioProcessArgs = GetCommandLine($"{processName}.exe");

		string? TryGetArgs(uint pid)
		{
			webView2AudioProcessArgs.TryGetValue(pid, out string? value);

			return value;
		}

		string filter = "--webview-exe-name=EOBrowser.exe";

		List<Process> processes = Process.GetProcessesByName(processName)
			.Select(p => (Process: p, Args: TryGetArgs((uint)p.Id)))
			.Where(t => t.Args?.Contains(filter) ?? false)
			.Select(t => t.Process)
			.ToList();

		uint succeededId = 0;
		var volume = GetVolumeObject(pid =>
		{
			if (processes.Any(p => p.Id == pid))
			{
				succeededId = pid;
				return true;
			}
			return false;
		});
		processID = succeededId;
		return volume;
	}

	/// <summary>
	/// 音量操作のためのデータを取得します。 CefSharp
	/// </summary>
	/// <param name="processName">対象のプロセス名。</param>
	/// <returns>データ。取得に失敗した場合は null。</returns>
	private static ISimpleAudioVolume GetVolumeObject(string processName, out uint processID)
	{
		var currentProcess = Process.GetCurrentProcess();
		var processes = Process.GetProcessesByName(processName).Where(p => GetParentProcess(p)?.Id == currentProcess.Id).ToArray();
		uint succeededId = 0;
		var volume = GetVolumeObject(pid =>
		{
			if (processes.Any(p => p.Id == pid))
			{
				succeededId = pid;
				return true;
			}
			return false;
		});
		processID = succeededId;
		return volume;
	}

	/// <summary>
	/// WebView2 implementation.
	/// </summary>
	public static VolumeManager? CreateInstanceByProcessName(string processName, string proxySettings)
	{
		var volume = GetVolumeObject(processName, out uint processID, proxySettings);
		if (volume != null)
		{
			Marshal.ReleaseComObject(volume);
			return new VolumeManager(processID);
		}
		else
		{
			return null;
		}
	}

	/// <summary>
	/// CefSharp implementation.
	/// </summary>
	public static VolumeManager CreateInstanceByProcessName(string processName)
	{
		var volume = GetVolumeObject(processName, out uint processID);
		if (volume != null)
		{
			Marshal.ReleaseComObject(volume);
			return new VolumeManager(processID);
		}
		else
		{
			return null;
		}
	}


	private static Process? GetParentProcess(Process process)
	{
		var pbi = new PROCESS_BASIC_INFORMATION();
		int status = NtQueryInformationProcess(process.Handle, 0, out pbi, Marshal.SizeOf(pbi), out int returnLength);
		if (status != 0)
			throw new System.ComponentModel.Win32Exception(status);

		try
		{
			return Process.GetProcessById((int)pbi.InheritedFromUniqueProcessId.ToUInt32());
		}
		catch (ArgumentException)
		{
			return null;        // process not found
		}
	}



	private const string ErrorMessageNotFound = "指定したプロセスIDの音量オブジェクトは存在しません。";

	/// <summary>
	/// 音量を取得します。
	/// </summary>
	/// <param name="processID">対象のプロセスID。</param>
	/// <returns>音量( 0.0 - 1.0 )。</returns>
	public static float GetApplicationVolume(uint processID)
	{
		ISimpleAudioVolume? volume = GetVolumeObject(processID);
		if (volume == null)
			throw new ArgumentException(ErrorMessageNotFound);

		float level;
		volume.GetMasterVolume(out level);

		Marshal.ReleaseComObject(volume);
		return level;
	}

	/// <summary>
	/// ミュート状態を取得します。
	/// </summary>
	/// <param name="processID">対象のプロセスID。</param>
	/// <returns>ミュートされていれば true。</returns>
	public static bool GetApplicationMute(uint processID)
	{
		ISimpleAudioVolume? volume = GetVolumeObject(processID);
		if (volume == null)
			throw new ArgumentException(ErrorMessageNotFound);

		bool mute;
		volume.GetMute(out mute);

		Marshal.ReleaseComObject(volume);
		return mute;
	}


	/// <summary>
	/// 音量を設定します。
	/// </summary>
	/// <param name="processID">対象のプロセスID。</param>
	/// <param name="level">音量( 0.0 - 1.0 )。</param>
	public static void SetApplicationVolume(uint processID, float level)
	{
		ISimpleAudioVolume? volume = GetVolumeObject(processID);
		if (volume == null)
			throw new ArgumentException(ErrorMessageNotFound);

		Guid guid = Guid.Empty;
		volume.SetMasterVolume(level, ref guid);

		Marshal.ReleaseComObject(volume);
	}

	/// <summary>
	/// ミュート状態を設定します。
	/// </summary>
	/// <param name="processID">対象のプロセスID。</param>
	/// <param name="mute">ミュートするなら true。</param>
	public static void SetApplicationMute(uint processID, bool mute)
	{
		ISimpleAudioVolume? volume = GetVolumeObject(processID);
		if (volume == null)
			throw new ArgumentException(ErrorMessageNotFound);

		Guid guid = Guid.Empty;
		volume.SetMute(mute, ref guid);

		Marshal.ReleaseComObject(volume);
	}

	/// <summary>
	/// ミュート状態をトグルします。
	/// </summary>
	/// <param name="processID">対象のプロセスID。</param>
	/// <returns>トグル後のミュート状態。</returns>
	public static bool ToggleMute(uint processID)
	{

		bool mute = !GetApplicationMute(processID);
		SetApplicationMute(processID, mute);
		return mute;
	}



	//インスタンス用

	public float Volume
	{
		get { return GetApplicationVolume(ProcessID); }
		set { SetApplicationVolume(ProcessID, value); }
	}

	public bool IsMute
	{
		get { return GetApplicationMute(ProcessID); }
		set { SetApplicationMute(ProcessID, value); }
	}

	/// <summary>
	/// ミュート状態をトグルします。
	/// </summary>
	/// <returns>トグル後のミュート状態。</returns>
	public bool ToggleMute()
	{
		return ToggleMute(ProcessID);
	}



	#region 呪文

	[ComImport]
	[Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
	internal class MMDeviceEnumerator
	{
	}

	internal enum EDataFlow
	{
		eRender,
		eCapture,
		eAll,
		EDataFlow_enum_count
	}

	internal enum ERole
	{
		eConsole,
		eMultimedia,
		eCommunications,
		ERole_enum_count
	}

	[Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IMMDeviceEnumerator
	{
		int NotImpl1();

		[PreserveSig]
		int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice? ppDevice);

		// the rest is not implemented
	}

	[Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IMMDevice
	{
		[PreserveSig]
		int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);

		// the rest is not implemented
	}

	[Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IAudioSessionManager2
	{
		int NotImpl1();
		int NotImpl2();

		[PreserveSig]
		int GetSessionEnumerator(out IAudioSessionEnumerator SessionEnum);

		// the rest is not implemented
	}

	[Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IAudioSessionEnumerator
	{
		[PreserveSig]
		int GetCount(out int SessionCount);

		[PreserveSig]
		int GetSession(int SessionCount, out IAudioSessionControl Session);
	}

	[Guid("F4B1A599-7266-4319-A8CA-E70ACB11E8CD"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IAudioSessionControl
	{
		int NotImpl1();

		[PreserveSig]
		int GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

		// the rest is not implemented
	}

	[Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface ISimpleAudioVolume
	{
		[PreserveSig]
		int SetMasterVolume(float fLevel, ref Guid EventContext);

		[PreserveSig]
		int GetMasterVolume(out float pfLevel);

		[PreserveSig]
		int SetMute(bool bMute, ref Guid EventContext);

		[PreserveSig]
		int GetMute(out bool pbMute);
	}

	public enum AudioSessionState
	{
		AudioSessionStateInactive = 0,
		AudioSessionStateActive = 1,
		AudioSessionStateExpired = 2
	}

	public enum AudioSessionDisconnectReason
	{
		DisconnectReasonDeviceRemoval = 0,
		DisconnectReasonServerShutdown = (DisconnectReasonDeviceRemoval + 1),
		DisconnectReasonFormatChanged = (DisconnectReasonServerShutdown + 1),
		DisconnectReasonSessionLogoff = (DisconnectReasonFormatChanged + 1),
		DisconnectReasonSessionDisconnected = (DisconnectReasonSessionLogoff + 1),
		DisconnectReasonExclusiveModeOverride = (DisconnectReasonSessionDisconnected + 1)
	}

	[Guid("24918ACC-64B3-37C1-8CA9-74A66E9957A8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAudioSessionEvents
	{
		[PreserveSig]
		int OnDisplayNameChanged([MarshalAs(UnmanagedType.LPWStr)] string NewDisplayName, Guid EventContext);
		[PreserveSig]
		int OnIconPathChanged([MarshalAs(UnmanagedType.LPWStr)] string NewIconPath, Guid EventContext);
		[PreserveSig]
		int OnSimpleVolumeChanged(float NewVolume, bool newMute, Guid EventContext);
		[PreserveSig]
		int OnChannelVolumeChanged(UInt32 ChannelCount, IntPtr NewChannelVolumeArray, UInt32 ChangedChannel, Guid EventContext);
		[PreserveSig]
		int OnGroupingParamChanged(Guid NewGroupingParam, Guid EventContext);
		[PreserveSig]
		int OnStateChanged(AudioSessionState NewState);
		[PreserveSig]
		int OnSessionDisconnected(AudioSessionDisconnectReason DisconnectReason);
	}

	[Guid("BFB7FF88-7239-4FC9-8FA2-07C950BE9C6D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAudioSessionControl2
	{
		[PreserveSig]
		int GetState(out AudioSessionState state);
		[PreserveSig]
		int GetDisplayName([Out(), MarshalAs(UnmanagedType.LPWStr)] out string name);
		[PreserveSig]
		int SetDisplayName([MarshalAs(UnmanagedType.LPWStr)] string value, Guid EventContext);
		[PreserveSig]
		int GetIconPath([Out(), MarshalAs(UnmanagedType.LPWStr)] out string Path);
		[PreserveSig]
		int SetIconPath([MarshalAs(UnmanagedType.LPWStr)] string Value, Guid EventContext);
		[PreserveSig]
		int GetGroupingParam(out Guid GroupingParam);
		[PreserveSig]
		int SetGroupingParam(Guid Override, Guid Eventcontext);
		[PreserveSig]
		int RegisterAudioSessionNotification(IAudioSessionEvents NewNotifications);
		[PreserveSig]
		int UnregisterAudioSessionNotification(IAudioSessionEvents NewNotifications);
		[PreserveSig]
		int GetSessionIdentifier([Out(), MarshalAs(UnmanagedType.LPWStr)] out string retVal);
		[PreserveSig]
		int GetSessionInstanceIdentifier([Out(), MarshalAs(UnmanagedType.LPWStr)] out string retVal);
		[PreserveSig]
		int GetProcessId(out UInt32 retvVal);
		[PreserveSig]
		int IsSystemSoundsSession();
		[PreserveSig]
		int SetDuckingPreference(bool optOut);
	}


	private struct PROCESS_BASIC_INFORMATION
	{
		public IntPtr ExitStatus;       // originally NtStatus
		public IntPtr PebBaseAddress;
		public UIntPtr AffinityMask;
		public int BasePriority;
		public UIntPtr UniqueProcessId;
		public UIntPtr InheritedFromUniqueProcessId;
	}

	[DllImport("NTDLL.DLL", SetLastError = true)]
	static extern int NtQueryInformationProcess(IntPtr hProcess, /*PROCESSINFOCLASS*/ int pic, out PROCESS_BASIC_INFORMATION pbi, int cb, out int pSize);

}



#endregion
