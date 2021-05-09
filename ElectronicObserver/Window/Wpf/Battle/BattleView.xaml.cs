using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;

namespace ElectronicObserver.Window.Wpf.Battle
{
	/// <summary>
    /// Interaction logic for BattleView.xaml
    /// </summary>
    public partial class BattleView : UserControl
    {
	    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		    "ViewModel", typeof(BattleViewModel), typeof(BattleView), new PropertyMetadata(default(BattleViewModel)));

	    public BattleViewModel ViewModel
	    {
		    get => (BattleViewModel) GetValue(ViewModelProperty);
		    set => SetValue(ViewModelProperty, value);
	    }

        public BattleView()
        {
            InitializeComponent();
        }

		// not really sure how to do this mvvm style
        private void FrameworkElement_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
			var bm = KCDatabase.Instance.Battle;

			if (bm == null || bm.BattleMode == BattleManager.BattleModes.Undefined)
				e.Handled = true;
		}
    }
}
