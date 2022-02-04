using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicObserver.Window.Wpf.Quest;
/// <summary>
/// Interaction logic for QuestView.xaml
/// </summary>
public partial class QuestView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		nameof(ViewModel), typeof(QuestViewModel), typeof(QuestView), new PropertyMetadata(default(QuestViewModel)));

	public QuestViewModel ViewModel
	{
		get => (QuestViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public QuestView()
	{
		InitializeComponent();
	}
}
