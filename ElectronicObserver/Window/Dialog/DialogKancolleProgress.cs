using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogKancolleProgress : Form
	{
        // base ship ID, level
        Dictionary<int, int> ShipLevels = new Dictionary<int, int>();

        List<DataGridViewCell> borderTop = new List<DataGridViewCell>();
        List<DataGridViewCell> borderBottom = new List<DataGridViewCell>();
        List<DataGridViewCell> borderLeft = new List<DataGridViewCell>();
        List<DataGridViewCell> borderRight = new List<DataGridViewCell>();

        List<int> boatIds = new List<int>();

        public DialogKancolleProgress()
        {
            InitializeComponent();

            GenerateList();
            ColorShipNames();

            DataGridViewCellStyle CS = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(38, 38, 38),
                ForeColor = Color.White,
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = Color.FromArgb(76, 76, 76),
                Font = new Font("Meiryo UI", 14F, GraphicsUnit.Pixel)
            };

            dataGridView1.DefaultCellStyle = CS;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.BackgroundColor = Color.FromArgb(38, 38, 38);
            dataGridView1.GridColor = Color.FromArgb(38, 38, 38);

            dataGridView1.CellPainting += dataGridView1_CellPainting;

            ControlHelper.SetDoubleBuffered(dataGridView1);

            // DataGridView size
            ClientSize = new Size(1250, 650);
        }

        private void AddStatistics()
        {
            int ShipCount = ShipLevels.Count(x => x.Value >= 0);

            dataGridView1.Rows[2].Cells[21].Value = "Missing: " + ShipLevels.Count(x => x.Value == 0) + "/" + ShipCount;
            dataGridView1.Rows[2].Cells[21].Style.ForeColor = GetLevelColor(0);
            dataGridView1.Rows[2].Cells[21].Style.SelectionForeColor = GetLevelColor(0);

            dataGridView1.Rows[3].Cells[21].Value = "Collection: " + ShipLevels.Count(x => x.Value > 0) + "/" + ShipCount;
            dataGridView1.Rows[3].Cells[21].Style.ForeColor = GetLevelColor(1);
            dataGridView1.Rows[3].Cells[21].Style.SelectionForeColor = GetLevelColor(1);

            dataGridView1.Rows[4].Cells[21].Value = "90+: " + ShipLevels.Count(x => x.Value >= 90) + "/" + ShipCount;
            dataGridView1.Rows[4].Cells[21].Style.ForeColor = GetLevelColor(90);
            dataGridView1.Rows[4].Cells[21].Style.SelectionForeColor = GetLevelColor(90);

            dataGridView1.Rows[5].Cells[21].Value = "99+: " + ShipLevels.Count(x => x.Value >= 99) + "/" + ShipCount;
            dataGridView1.Rows[5].Cells[21].Style.ForeColor = GetLevelColor(99);
            dataGridView1.Rows[5].Cells[21].Style.SelectionForeColor = GetLevelColor(99);

            dataGridView1.Rows[6].Cells[21].Value = "Perfection: " + ShipLevels.Count(x => x.Value == 175) + "/" + ShipCount;
            dataGridView1.Rows[6].Cells[21].Style.ForeColor = GetLevelColor(175);
            dataGridView1.Rows[6].Cells[21].Style.SelectionForeColor = GetLevelColor(175);
        }

        private void GenerateList()
        {
            var boats = KCDatabase.Instance.MasterShips.Values;
            boats = boats.Where(x => x.ShipID < 1500 && x.RemodelBeforeShipID == 0).OrderBy(x => x.SortNo);

            var Destroyer = boats.Where(x => x.ShipType == ShipTypes.Destroyer);
            var Escort = boats.Where(x => x.ShipType == ShipTypes.Escort);
            var LightCruiser = boats.Where(x => x.ShipType == ShipTypes.LightCruiser);
            var HeavyCruiser = boats.Where(x => x.ShipType == ShipTypes.HeavyCruiser);
            var Battleship = boats.Where(x => x.ShipType == ShipTypes.Battleship || x.ShipType == ShipTypes.Battlecruiser);
            var Carrier = boats.Where(x => x.ShipType == ShipTypes.AircraftCarrier || x.ShipType == ShipTypes.LightAircraftCarrier || x.ShipType == ShipTypes.ArmoredAircraftCarrier);
            var Others = boats.Where(x => x.ShipType == ShipTypes.Submarine || x.ShipType == ShipTypes.SubmarineAircraftCarrier || x.ShipType == ShipTypes.SeaplaneTender || x.ShipType == ShipTypes.FleetOiler || x.ShipType == ShipTypes.RepairShip || x.ShipType == ShipTypes.TrainingCruiser || x.ShipType == ShipTypes.AmphibiousAssaultShip || x.ShipType == ShipTypes.SubmarineTender);

            var groupedBoats = new List<IEnumerable<ShipDataMaster>> { Destroyer, Escort, LightCruiser, HeavyCruiser, Battleship, Carrier, Others };
            dataGridView1.Rows.Add(30);

            dataGridView1.Rows[1].Cells[5].Value = "駆逐艦";
            dataGridView1.Rows[1].Cells[9].Value = "海防艦";
            dataGridView1.Rows[1].Cells[11].Value = "軽巡";
            dataGridView1.Rows[1].Cells[13].Value = "重巡";
            dataGridView1.Rows[1].Cells[15].Value = "戦艦";
            dataGridView1.Rows[1].Cells[17].Value = "空母";
            dataGridView1.Rows[1].Cells[19].Value = "その他";

            int rowCount = 2;
            int columnCount = 1;

            int PreviousShipClass = 0;

            foreach (var boat in boats)
                ShipLevels.Add(boat.ShipID, 0);

            GetShipLevels();

            int MaxRowCount = 29;

            foreach (var group in groupedBoats)
            {
                foreach (var boat in group)
                {
                    if (boat.Name == "なし") continue;

                    if (boat.ShipClass != PreviousShipClass)
                    {
                        borderTop.Add(dataGridView1.Rows[rowCount].Cells[columnCount]);
                        borderTop.Add(dataGridView1.Rows[rowCount].Cells[columnCount+1]);

                        borderBottom.Add(dataGridView1.Rows[rowCount-1].Cells[columnCount]);
                        borderBottom.Add(dataGridView1.Rows[rowCount - 1].Cells[columnCount+1]);

                        if (rowCount + group.Count(x => x.ShipClass == boat.ShipClass) > MaxRowCount)
                        {
                            rowCount = 2;
                            columnCount += 2;
                        }

                        PreviousShipClass = boat.ShipClass;
                    }

                    dataGridView1.Rows[rowCount].Cells[columnCount].Value = boat.Name;
                    borderLeft.Add(dataGridView1.Rows[rowCount].Cells[columnCount]);

                    dataGridView1.Rows[rowCount].Cells[columnCount + 1].Value = ShipLevels[boat.ShipID];
                    borderRight.Add(dataGridView1.Rows[rowCount].Cells[columnCount + 1]);

                    rowCount++;

                    if (rowCount > MaxRowCount)
                    {
                        borderTop.Add(dataGridView1.Rows[rowCount].Cells[columnCount]);
                        borderTop.Add(dataGridView1.Rows[rowCount].Cells[columnCount+1]);

                        rowCount = 2;
                        columnCount += 2;
                    }
                }
                borderTop.Add(dataGridView1.Rows[rowCount].Cells[columnCount]);
                borderTop.Add(dataGridView1.Rows[rowCount].Cells[columnCount+1]);

                rowCount = 2;
                columnCount += 2;
            }

            AddStatistics();
        }

        private void ColorShipNames()
        {
            for (int i = 1; i < dataGridView1.ColumnCount - 1; i += 2)
                for (int j = 2; j < dataGridView1.RowCount; j++)
                {
                    Color c = GetLevelColor(dataGridView1.Rows[j].Cells[i + 1].Value);
                    dataGridView1.Rows[j].Cells[i].Style.ForeColor = c;
                    dataGridView1.Rows[j].Cells[i].Style.SelectionForeColor = c;
                }
        }

        private Color GetLevelColor(object value)
        {
            if (value == null)
                return Color.Black;

            int level = 0;

            int.TryParse(value.ToString(), out level);

            if (level==175)
                return Color.FromArgb(255, 51, 153);

            if (level >= 99)
                return Color.FromArgb(0, 176, 240);

            if (level >= 90)
                return Color.FromArgb(0, 176, 80);

            if (level >= 1)
                return Color.FromArgb(255, 255, 0);

            return Color.FromArgb(255, 0, 0);
        }

        private void GetShipLevels()
        {
            foreach (KeyValuePair<int, ShipData> entry in KCDatabase.Instance.Ships)
            {
                int baseID = BaseShipId(entry.Value.ShipID);
                if (ShipLevels.ContainsKey(baseID))
                {
                    if (ShipLevels[baseID] < entry.Value.Level)
                        ShipLevels[baseID] = entry.Value.Level;
                }
            }
        }

        private int BaseShipId(int shipID)
        {
            ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
            while (ship.RemodelBeforeShipID != 0)
                ship = ship.RemodelBeforeShip;
            return ship.ID;
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Handled = true;

            using (Brush b = new SolidBrush(dataGridView1.DefaultCellStyle.BackColor))
            {
                e.Graphics.FillRectangle(b, e.CellBounds);
            }

            using (Pen p = new Pen(Brushes.Black, 2))
            {
                if (e.RowIndex == 1 && e.ColumnIndex >= 1 && e.ColumnIndex <= 20)
                {
                    BorderTop(p, e);
                    BorderBottom(p, e);

                    if (e.ColumnIndex == 1)
                        BorderLeft(p, e);

                    if (e.ColumnIndex >= 8 && e.ColumnIndex % 2 == 0)
                        BorderRight(p, e);
                }

                if (borderTop.Contains(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex]))
                    BorderTop(p, e);

                if (borderBottom.Contains(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex]))
                    BorderBottom(p, e);

                if (borderLeft.Contains(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex]))
                    BorderLeft(p, e);

                if (borderRight.Contains(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex]))
                    BorderRight(p, e);
            }

            e.PaintContent(e.ClipBounds);
        }

        private void BorderTop(Pen p, DataGridViewCellPaintingEventArgs e)
        {
            e.Graphics.DrawLine(p, e.CellBounds.Left - 1, e.CellBounds.Top - 1, e.CellBounds.Right, e.CellBounds.Top - 1);
        }

        private void BorderBottom(Pen p, DataGridViewCellPaintingEventArgs e)
        {
            e.Graphics.DrawLine(p, e.CellBounds.Left - 1, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
        }

        private void BorderLeft(Pen p, DataGridViewCellPaintingEventArgs e)
        {
            e.Graphics.DrawLine(p, e.CellBounds.Left - 1, e.CellBounds.Top - 1, e.CellBounds.Left - 1, e.CellBounds.Bottom - 1);
        }

        private void BorderRight(Pen p, DataGridViewCellPaintingEventArgs e)
        {
            e.Graphics.DrawLine(p, e.CellBounds.Right - 1, e.CellBounds.Top - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
        }
    }
}
