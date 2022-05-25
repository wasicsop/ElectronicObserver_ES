using System;
using System.Drawing;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Integrate;

/// <summary>
/// 選択中のウィンドウに表示する枠
/// </summary>
public partial class FormCandidate : Form
{

	private Pen blackPen = new Pen(Color.Black, 5);

	public FormCandidate()
	{
		InitializeComponent();
	}

	private void FormCandidate_Paint(object sender, PaintEventArgs e)
	{
		Graphics g = e.Graphics;
		g.DrawRectangle(blackPen, 2, 2, Width - 4, Height - 4);
	}

	private void FormCandidate_FormClosed(object sender, FormClosedEventArgs e)
	{
		blackPen.Dispose();
	}

	private void FormCandidate_Resize(object sender, EventArgs e)
	{
		Invalidate();
	}
}
