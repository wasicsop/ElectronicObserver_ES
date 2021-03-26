using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AvalonDock;
using AvalonDock.Controls;
using AvalonDock.Layout;
using ElectronicObserver.AvalonDockTesting;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ElectronicObserver.ViewModels
{
	public class FormMainViewModel : ObservableObject
	{
		public ObservableCollection<AnchorableViewModel> Views { get; } = new();

		public LogViewModel LogViewModel { get; }

		public ICommand OpenViewCommand { get; }
		
		public FormMainViewModel()
		{
			Views.Add(LogViewModel = new());

			OpenViewCommand = new RelayCommand<AnchorableViewModel>(OpenView);
		}

		private void OpenView(AnchorableViewModel view)
		{
			view.IsVisible = true;
			view.IsSelected = true;
			view.IsActive = true;
		}
	}
}