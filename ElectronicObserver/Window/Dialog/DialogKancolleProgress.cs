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
        // name, base ship ID
        Dictionary<string, int> ShipIds = new Dictionary<string, int>();

        public DialogKancolleProgress()
        {
            InitializeComponent();

            GetShipIds();
            GetShipLevels();
 
            GenerateListOriginal();

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

            // DataGridView size
            ClientSize = new Size(1250, 600);
        }

        private void GenerateListOriginal()
        {
            // generated in excel
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add("", "", "", "", "", "駆逐艦", "", "", "", "海防艦", "", "軽巡", "", "重巡", "", "戦艦", "", "空母", "", "その他", "");
            dataGridView1.Rows.Add("", "神風", ShipLevels[ShipIds["神風"]], "綾波", ShipLevels[ShipIds["綾波"]], "朝潮", ShipLevels[ShipIds["朝潮"]], "夕雲", ShipLevels[ShipIds["夕雲"]], "占守", ShipLevels[ShipIds["占守"]], "天龍", ShipLevels[ShipIds["天龍"]], "古鷹", ShipLevels[ShipIds["古鷹"]], "金剛", ShipLevels[ShipIds["金剛"]], "鳳翔", ShipLevels[ShipIds["鳳翔"]], "伊168", ShipLevels[ShipIds["伊168"]]);
            dataGridView1.Rows.Add("", "朝風", ShipLevels[ShipIds["朝風"]], "敷波", ShipLevels[ShipIds["敷波"]], "大潮", ShipLevels[ShipIds["大潮"]], "巻雲", ShipLevels[ShipIds["巻雲"]], "国後", ShipLevels[ShipIds["国後"]], "龍田", ShipLevels[ShipIds["龍田"]], "加古", ShipLevels[ShipIds["加古"]], "比叡", ShipLevels[ShipIds["比叡"]], "龍驤", ShipLevels[ShipIds["龍驤"]], "伊8", ShipLevels[ShipIds["伊8"]]);
            dataGridView1.Rows.Add("", "春風", ShipLevels[ShipIds["春風"]], "天霧", ShipLevels[ShipIds["天霧"]], "満潮", ShipLevels[ShipIds["満潮"]], "風雲", ShipLevels[ShipIds["風雲"]], "択捉", ShipLevels[ShipIds["択捉"]], "球磨", ShipLevels[ShipIds["球磨"]], "青葉", ShipLevels[ShipIds["青葉"]], "榛名", ShipLevels[ShipIds["榛名"]], "飛揚", ShipLevels[ShipIds["飛揚"]], "伊13", ShipLevels[ShipIds["伊13"]]);
            dataGridView1.Rows.Add("", "松風", ShipLevels[ShipIds["松風"]], "狭霧", ShipLevels[ShipIds["狭霧"]], "荒潮", ShipLevels[ShipIds["荒潮"]], "長波", ShipLevels[ShipIds["長波"]], "松輪", ShipLevels[ShipIds["松輪"]], "多摩", ShipLevels[ShipIds["多摩"]], "衣笠", ShipLevels[ShipIds["衣笠"]], "霧島", ShipLevels[ShipIds["霧島"]], "隼鷹", ShipLevels[ShipIds["隼鷹"]], "伊14", ShipLevels[ShipIds["伊14"]]);
            dataGridView1.Rows.Add("", "旗風", ShipLevels[ShipIds["旗風"]], "朧", ShipLevels[ShipIds["朧"]], "山雲", ShipLevels[ShipIds["山雲"]], "高波", ShipLevels[ShipIds["高波"]], "対馬", ShipLevels[ShipIds["対馬"]], "北上", ShipLevels[ShipIds["北上"]], "妙高", ShipLevels[ShipIds["妙高"]], "Bismarck", ShipLevels[ShipIds["Bismarck"]], "祥鳳", ShipLevels[ShipIds["祥鳳"]], "伊19", ShipLevels[ShipIds["伊19"]]);
            dataGridView1.Rows.Add("", "睦月", ShipLevels[ShipIds["睦月"]], "曙", ShipLevels[ShipIds["曙"]], "朝雲", ShipLevels[ShipIds["朝雲"]], "藤波", ShipLevels[ShipIds["藤波"]], "佐渡", ShipLevels[ShipIds["佐渡"]], "大井", ShipLevels[ShipIds["大井"]], "那智", ShipLevels[ShipIds["那智"]], "Littorio", ShipLevels[ShipIds["Littorio"]], "瑞鳳", ShipLevels[ShipIds["瑞鳳"]], "伊26", ShipLevels[ShipIds["伊26"]]);
            dataGridView1.Rows.Add("", "如月", ShipLevels[ShipIds["如月"]], "漣", ShipLevels[ShipIds["漣"]], "霰", ShipLevels[ShipIds["霰"]], "浜波", ShipLevels[ShipIds["浜波"]], "福江", ShipLevels[ShipIds["福江"]], "木曾", ShipLevels[ShipIds["木曾"]], "足柄", ShipLevels[ShipIds["足柄"]], "Roma", ShipLevels[ShipIds["Roma"]], "千歳", ShipLevels[ShipIds["千歳"]], "伊58", ShipLevels[ShipIds["伊58"]]);
            dataGridView1.Rows.Add("", "弥生", ShipLevels[ShipIds["弥生"]], "潮", ShipLevels[ShipIds["潮"]], "霞", ShipLevels[ShipIds["霞"]], "沖波", ShipLevels[ShipIds["沖波"]], "日振", ShipLevels[ShipIds["日振"]], "長良", ShipLevels[ShipIds["長良"]], "羽黒", ShipLevels[ShipIds["羽黒"]], "Iowa", ShipLevels[ShipIds["Iowa"]], "千代田", ShipLevels[ShipIds["千代田"]], "伊400", ShipLevels[ShipIds["伊400"]]);
            dataGridView1.Rows.Add("", "卯月", ShipLevels[ShipIds["卯月"]], "暁", ShipLevels[ShipIds["暁"]], "陽炎", ShipLevels[ShipIds["陽炎"]], "岸波", ShipLevels[ShipIds["岸波"]], "大東", ShipLevels[ShipIds["大東"]], "五十鈴", ShipLevels[ShipIds["五十鈴"]], "高雄", ShipLevels[ShipIds["高雄"]], "Гангут", ShipLevels[ShipIds["Гангут"]], "春日丸", ShipLevels[ShipIds["春日丸"]], "伊401", ShipLevels[ShipIds["伊401"]]);
            dataGridView1.Rows.Add("", "皐月", ShipLevels[ShipIds["皐月"]], "響", ShipLevels[ShipIds["響"]], "不知火", ShipLevels[ShipIds["不知火"]], "朝潮", ShipLevels[ShipIds["朝潮"]], "", "", "由良", ShipLevels[ShipIds["由良"]], "愛宕", ShipLevels[ShipIds["愛宕"]], "Richelieu", ShipLevels[ShipIds["Richelieu"]], "神鷹", ShipLevels[ShipIds["神鷹"]], "まるゆ", ShipLevels[ShipIds["まるゆ"]]);
            dataGridView1.Rows.Add("", "水無月", ShipLevels[ShipIds["水無月"]], "雷", ShipLevels[ShipIds["雷"]], "黒潮", ShipLevels[ShipIds["黒潮"]], "早霜", ShipLevels[ShipIds["早霜"]], "", "", "名取", ShipLevels[ShipIds["名取"]], "摩耶", ShipLevels[ShipIds["摩耶"]], "長門", ShipLevels[ShipIds["長門"]], "Gambier Bay", ShipLevels[ShipIds["Gambier Bay"]], "U-511", ShipLevels[ShipIds["U-511"]]);
            dataGridView1.Rows.Add("", "文月", ShipLevels[ShipIds["文月"]], "電", ShipLevels[ShipIds["電"]], "親潮", ShipLevels[ShipIds["親潮"]], "清霜", ShipLevels[ShipIds["清霜"]], "", "", "鬼怒", ShipLevels[ShipIds["鬼怒"]], "鳥海", ShipLevels[ShipIds["鳥海"]], "陸奥", ShipLevels[ShipIds["陸奥"]], "赤城", ShipLevels[ShipIds["赤城"]], "Luigi Torelli", ShipLevels[ShipIds["Luigi Torelli"]]);
            dataGridView1.Rows.Add("", "長月", ShipLevels[ShipIds["長月"]], "初春", ShipLevels[ShipIds["初春"]], "初風", ShipLevels[ShipIds["初風"]], "秋月", ShipLevels[ShipIds["秋月"]], "", "", "阿武隈", ShipLevels[ShipIds["阿武隈"]], "最上", ShipLevels[ShipIds["最上"]], "大和", ShipLevels[ShipIds["大和"]], "加賀", ShipLevels[ShipIds["加賀"]], "秋津洲", ShipLevels[ShipIds["秋津洲"]]);
            dataGridView1.Rows.Add("", "菊月", ShipLevels[ShipIds["菊月"]], "子日", ShipLevels[ShipIds["子日"]], "雪風", ShipLevels[ShipIds["雪風"]], "照月", ShipLevels[ShipIds["照月"]], "", "", "川内", ShipLevels[ShipIds["川内"]], "三隈", ShipLevels[ShipIds["三隈"]], "武蔵", ShipLevels[ShipIds["武蔵"]], "蒼龍", ShipLevels[ShipIds["蒼龍"]], "瑞穂", ShipLevels[ShipIds["瑞穂"]]);
            dataGridView1.Rows.Add("", "三日月", ShipLevels[ShipIds["三日月"]], "若葉", ShipLevels[ShipIds["若葉"]], "天津風", ShipLevels[ShipIds["天津風"]], "初月", ShipLevels[ShipIds["初月"]], "", "", "神通", ShipLevels[ShipIds["神通"]], "鈴谷", ShipLevels[ShipIds["鈴谷"]], "Warspite", ShipLevels[ShipIds["Warspite"]], "飛龍", ShipLevels[ShipIds["飛龍"]], "大鯨", ShipLevels[ShipIds["大鯨"]]);
            dataGridView1.Rows.Add("", "望月", ShipLevels[ShipIds["望月"]], "初霜", ShipLevels[ShipIds["初霜"]], "時津風", ShipLevels[ShipIds["時津風"]], "涼月", ShipLevels[ShipIds["涼月"]], "", "", "那珂", ShipLevels[ShipIds["那珂"]], "熊野", ShipLevels[ShipIds["熊野"]], "Nelson", ShipLevels[ShipIds["Nelson"]], "翔鶴", ShipLevels[ShipIds["翔鶴"]], "神威", ShipLevels[ShipIds["神威"]]);
            dataGridView1.Rows.Add("", "吹雪", ShipLevels[ShipIds["吹雪"]], "白露", ShipLevels[ShipIds["白露"]], "浦風", ShipLevels[ShipIds["浦風"]], "島風", ShipLevels[ShipIds["島風"]], "", "", "夕張", ShipLevels[ShipIds["夕張"]], "利根", ShipLevels[ShipIds["利根"]], "扶桑", ShipLevels[ShipIds["扶桑"]], "瑞鶴", ShipLevels[ShipIds["瑞鶴"]], "Commandant Teste", ShipLevels[ShipIds["Commandant Teste"]]);
            dataGridView1.Rows.Add("", "白雪", ShipLevels[ShipIds["白雪"]], "時雨", ShipLevels[ShipIds["時雨"]], "磯風", ShipLevels[ShipIds["磯風"]], "Z1", ShipLevels[ShipIds["Z1"]], "", "", "阿賀野", ShipLevels[ShipIds["阿賀野"]], "筑摩", ShipLevels[ShipIds["筑摩"]], "山城", ShipLevels[ShipIds["山城"]], "雲龍", ShipLevels[ShipIds["雲龍"]], "明石", ShipLevels[ShipIds["明石"]]);
            dataGridView1.Rows.Add("", "初雪", ShipLevels[ShipIds["初雪"]], "村雨", ShipLevels[ShipIds["村雨"]], "浜風", ShipLevels[ShipIds["浜風"]], "Z3", ShipLevels[ShipIds["Z3"]], "", "", "能代", ShipLevels[ShipIds["能代"]], "Prinz Eugen", ShipLevels[ShipIds["Prinz Eugen"]], "伊勢", ShipLevels[ShipIds["伊勢"]], "天城", ShipLevels[ShipIds["天城"]], "香取", ShipLevels[ShipIds["香取"]]);
            dataGridView1.Rows.Add("", "深雪", ShipLevels[ShipIds["深雪"]], "夕立", ShipLevels[ShipIds["夕立"]], "谷風", ShipLevels[ShipIds["谷風"]], "Maestrale", ShipLevels[ShipIds["Maestrale"]], "", "", "矢矧", ShipLevels[ShipIds["矢矧"]], "Zara", ShipLevels[ShipIds["Zara"]], "日向", ShipLevels[ShipIds["日向"]], "葛城", ShipLevels[ShipIds["葛城"]], "鹿島", ShipLevels[ShipIds["鹿島"]]);
            dataGridView1.Rows.Add("", "叢雲", ShipLevels[ShipIds["叢雲"]], "春雨", ShipLevels[ShipIds["春雨"]], "野分", ShipLevels[ShipIds["野分"]], "Libeccio", ShipLevels[ShipIds["Libeccio"]], "", "", "酒匂", ShipLevels[ShipIds["酒匂"]], "Pola", ShipLevels[ShipIds["Pola"]], "", "", "Graf Zeppelin", ShipLevels[ShipIds["Graf Zeppelin"]], "あきつ丸", ShipLevels[ShipIds["あきつ丸"]]);
            dataGridView1.Rows.Add("", "磯波", ShipLevels[ShipIds["磯波"]], "五月雨", ShipLevels[ShipIds["五月雨"]], "嵐", ShipLevels[ShipIds["嵐"]], "Jervis", ShipLevels[ShipIds["Jervis"]], "", "", "大淀", ShipLevels[ShipIds["大淀"]], "", "", "", "", "Aquila", ShipLevels[ShipIds["Aquila"]], "速吸", ShipLevels[ShipIds["速吸"]]);
            dataGridView1.Rows.Add("", "浦波", ShipLevels[ShipIds["浦波"]], "海風", ShipLevels[ShipIds["海風"]], "萩風", ShipLevels[ShipIds["萩風"]], "Ташкент", ShipLevels[ShipIds["Ташкент"]], "", "", "Gotland", ShipLevels[ShipIds["Gotland"]], "", "", "", "", "Saratoga", ShipLevels[ShipIds["Saratoga"]], "", "");
            dataGridView1.Rows.Add("", "", "", "山風", ShipLevels[ShipIds["山風"]], "舞風", ShipLevels[ShipIds["舞風"]], "Samuel B.Roberts", ShipLevels[ShipIds["Samuel B.Roberts"]], "", "", "", "", "", "", "", "", "大鳳", ShipLevels[ShipIds["大鳳"]], "", "");
            dataGridView1.Rows.Add("", "", "", "江風", ShipLevels[ShipIds["江風"]], "秋雲", ShipLevels[ShipIds["秋雲"]], "", "", "", "", "", "", "", "", "", "", "Ark Royal", ShipLevels[ShipIds["Ark Royal"]], "", "");
            dataGridView1.Rows.Add("", "", "", "涼風", ShipLevels[ShipIds["涼風"]], "", "", "", "", "", "", "", "", "", "", "", "", "Intrepid", ShipLevels[ShipIds["Intrepid"]], "", "");
        }

        private void GenerateListEnglish()
        {

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
            int level = 0;

            int.TryParse(value.ToString(), out level);

            if(level==175)
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
            foreach (KeyValuePair<string, int> entry in ShipIds)
            {
                ShipLevels.Add(entry.Value, 0);
            }

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

        private void GetShipIds()
        {
            ShipIds.Add("神風", 471);
            ShipIds.Add("朝風", 472);
            ShipIds.Add("春風", 473);
            ShipIds.Add("松風", 474);
            ShipIds.Add("旗風", 475);

            ShipIds.Add("睦月", 1);
            ShipIds.Add("如月", 2);
            ShipIds.Add("弥生", 164);
            ShipIds.Add("卯月", 165);
            ShipIds.Add("皐月", 28);
            ShipIds.Add("水無月", 481);
            ShipIds.Add("文月", 29);
            ShipIds.Add("長月", 6);
            ShipIds.Add("菊月", 30);
            ShipIds.Add("三日月", 7);
            ShipIds.Add("望月", 31);

            ShipIds.Add("吹雪", 9);
            ShipIds.Add("白雪", 10);
            ShipIds.Add("初雪", 32);
            ShipIds.Add("深雪", 11);
            ShipIds.Add("叢雲", 33);
            ShipIds.Add("磯波", 12);
            ShipIds.Add("浦波", 486);

            ShipIds.Add("綾波", 13);
            ShipIds.Add("敷波", 14);
            ShipIds.Add("天霧", 479);
            ShipIds.Add("狭霧", 480);
            ShipIds.Add("朧", 93);
            ShipIds.Add("曙", 15);
            ShipIds.Add("漣", 94);
            ShipIds.Add("潮", 16);

            ShipIds.Add("暁", 34);
            ShipIds.Add("響", 35);
            ShipIds.Add("雷", 36);
            ShipIds.Add("電", 37);

            ShipIds.Add("初春", 38);
            ShipIds.Add("子日", 39);
            ShipIds.Add("若葉", 40);
            ShipIds.Add("初霜", 41);

            ShipIds.Add("白露", 42);
            ShipIds.Add("時雨", 43);
            ShipIds.Add("村雨", 44);
            ShipIds.Add("夕立", 45);
            ShipIds.Add("春雨", 405);
            ShipIds.Add("五月雨", 46);
            ShipIds.Add("海風", 458);
            ShipIds.Add("山風", 457);
            ShipIds.Add("江風", 459);
            ShipIds.Add("涼風", 47);

            ShipIds.Add("朝潮", 95);
            ShipIds.Add("大潮", 96);
            ShipIds.Add("満潮", 97);
            ShipIds.Add("荒潮", 98);
            ShipIds.Add("山雲", 414);
            ShipIds.Add("朝雲", 413);
            ShipIds.Add("霰", 48);
            ShipIds.Add("霞", 49);

            ShipIds.Add("陽炎", 17);
            ShipIds.Add("不知火", 18);
            ShipIds.Add("黒潮", 19);
            ShipIds.Add("親潮", 456);
            ShipIds.Add("初風", 190);
            ShipIds.Add("雪風", 20);
            ShipIds.Add("天津風", 181);
            ShipIds.Add("時津風", 186);
            ShipIds.Add("浦風", 168);
            ShipIds.Add("磯風", 167);
            ShipIds.Add("浜風", 170);
            ShipIds.Add("谷風", 169);
            ShipIds.Add("野分", 415);
            ShipIds.Add("嵐", 454);
            ShipIds.Add("萩風", 455);
            ShipIds.Add("舞風", 122);
            ShipIds.Add("秋雲", 132);

            ShipIds.Add("夕雲", 133);
            ShipIds.Add("巻雲", 134);
            ShipIds.Add("風雲", 453);
            ShipIds.Add("長波", 135);
            ShipIds.Add("高波", 424);
            ShipIds.Add("藤波", 485);
            ShipIds.Add("浜波", 484);
            ShipIds.Add("沖波", 452);
            ShipIds.Add("岸波", 527);
            ShipIds.Add("朝霜", 425);
            ShipIds.Add("早霜", 409);
            ShipIds.Add("清霜", 410);

            ShipIds.Add("秋月", 421);
            ShipIds.Add("照月", 422);
            ShipIds.Add("初月", 423);
            ShipIds.Add("涼月", 532);

            ShipIds.Add("島風", 50);

            ShipIds.Add("Z1", 174);
            ShipIds.Add("Z3", 175);

            ShipIds.Add("Maestrale", 575);
            ShipIds.Add("Libeccio", 443);

            ShipIds.Add("Jervis", 519);

            ShipIds.Add("Ташкент", 516);

            ShipIds.Add("Samuel B.Roberts", 561);

            ShipIds.Add("占守", 517);
            ShipIds.Add("国後", 518);

            ShipIds.Add("択捉", 524);
            ShipIds.Add("松輪", 525);
            ShipIds.Add("対馬", 540);
            ShipIds.Add("佐渡", 531);
            ShipIds.Add("福江", 565);

            ShipIds.Add("日振", 551);
            ShipIds.Add("大東", 552);

            ShipIds.Add("天龍", 51);
            ShipIds.Add("龍田", 52);

            ShipIds.Add("球磨", 99);
            ShipIds.Add("多摩", 100);
            ShipIds.Add("北上", 25);
            ShipIds.Add("大井", 24);
            ShipIds.Add("木曾", 101);

            ShipIds.Add("長良", 21);
            ShipIds.Add("五十鈴", 22);
            ShipIds.Add("由良", 23);
            ShipIds.Add("名取", 53);
            ShipIds.Add("鬼怒", 113);
            ShipIds.Add("阿武隈", 114);

            ShipIds.Add("川内", 54);
            ShipIds.Add("神通", 55);
            ShipIds.Add("那珂", 56);

            ShipIds.Add("夕張", 115);

            ShipIds.Add("阿賀野", 137);
            ShipIds.Add("能代", 138);
            ShipIds.Add("矢矧", 139);
            ShipIds.Add("酒匂", 140);

            ShipIds.Add("大淀", 183);

            ShipIds.Add("Gotland", 574);

            ShipIds.Add("古鷹", 59);
            ShipIds.Add("加古", 60);

            ShipIds.Add("青葉", 61);
            ShipIds.Add("衣笠", 123);

            ShipIds.Add("妙高", 62);
            ShipIds.Add("那智", 63);
            ShipIds.Add("足柄", 64);
            ShipIds.Add("羽黒", 65);

            ShipIds.Add("高雄", 66);
            ShipIds.Add("愛宕", 67);
            ShipIds.Add("摩耶", 68);
            ShipIds.Add("鳥海", 69);

            ShipIds.Add("最上", 70);
            ShipIds.Add("三隈", 120);
            ShipIds.Add("鈴谷", 124);
            ShipIds.Add("熊野", 125);

            ShipIds.Add("利根", 71);
            ShipIds.Add("筑摩", 72);

            ShipIds.Add("Prinz Eugen", 176);

            ShipIds.Add("Zara", 448);
            ShipIds.Add("Pola", 449);

            ShipIds.Add("金剛", 78);
            ShipIds.Add("比叡", 79);
            ShipIds.Add("榛名", 85);
            ShipIds.Add("霧島", 86);

            ShipIds.Add("Bismarck", 171);

            ShipIds.Add("Littorio", 441);
            ShipIds.Add("Roma", 442);

            ShipIds.Add("Iowa", 440);

            ShipIds.Add("Гангут", 511);

            ShipIds.Add("Richelieu", 492);

            ShipIds.Add("長門", 80);
            ShipIds.Add("陸奥", 81);

            ShipIds.Add("大和", 131);
            ShipIds.Add("武蔵", 143);

            ShipIds.Add("Warspite", 439);

            ShipIds.Add("Nelson", 571);

            ShipIds.Add("扶桑", 26);
            ShipIds.Add("山城", 27);

            ShipIds.Add("伊勢", 77);
            ShipIds.Add("日向", 87);

            ShipIds.Add("鳳翔", 89);

            ShipIds.Add("龍驤", 76);

            ShipIds.Add("飛揚", 75);
            ShipIds.Add("隼鷹", 92);

            ShipIds.Add("祥鳳", 74);
            ShipIds.Add("瑞鳳", 116);

            ShipIds.Add("千歳", 102);
            ShipIds.Add("千代田", 103);

            ShipIds.Add("春日丸", 521);

            ShipIds.Add("神鷹", 534);

            ShipIds.Add("Gambier Bay", 544);

            ShipIds.Add("赤城", 83);

            ShipIds.Add("加賀", 84);

            ShipIds.Add("蒼龍", 90);

            ShipIds.Add("飛龍", 91);

            ShipIds.Add("翔鶴", 110);
            ShipIds.Add("瑞鶴", 111);

            ShipIds.Add("雲龍", 404);
            ShipIds.Add("天城", 331);
            ShipIds.Add("葛城", 332);

            ShipIds.Add("Graf Zeppelin", 432);

            ShipIds.Add("Aquila", 444);

            ShipIds.Add("Saratoga", 433);

            ShipIds.Add("大鳳", 153);

            ShipIds.Add("Ark Royal", 515);

            ShipIds.Add("Intrepid", 549);

            ShipIds.Add("伊168", 126);

            ShipIds.Add("伊8", 128);

            ShipIds.Add("伊13", 494);
            ShipIds.Add("伊14", 495);

            ShipIds.Add("伊19", 191);
            ShipIds.Add("伊26", 483);

            ShipIds.Add("伊58", 127);

            ShipIds.Add("伊400", 493);
            ShipIds.Add("伊401", 155);

            ShipIds.Add("まるゆ", 163);

            ShipIds.Add("U-511", 431);

            ShipIds.Add("Luigi Torelli", 535);

            ShipIds.Add("秋津洲", 445);

            ShipIds.Add("瑞穂", 451);

            ShipIds.Add("大鯨", 184);

            ShipIds.Add("神威", 162);

            ShipIds.Add("Commandant Teste", 491);

            ShipIds.Add("明石", 182);

            ShipIds.Add("香取", 154);
            ShipIds.Add("鹿島", 465);

            ShipIds.Add("あきつ丸", 161);

            ShipIds.Add("速吸", 460);
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
                // first row only
                if (e.RowIndex == 1 && e.ColumnIndex >= 1 && e.ColumnIndex <= 20)
                {
                    BorderTop(p, e);
                    BorderBottom(p, e);

                    if (e.ColumnIndex == 1)
                        BorderLeft(p, e);

                    if (e.ColumnIndex >= 8 && e.ColumnIndex % 2 == 0)
                        BorderRight(p, e);
                }

                if (e.RowIndex > 1 && e.ColumnIndex > 0)
                {
                    if (e.ColumnIndex == 1 && e.RowIndex <= 24)
                        BorderLeft(p, e);

                    if (((e.ColumnIndex == 2 || e.ColumnIndex == 4 || e.ColumnIndex == 16 || e.ColumnIndex == 18) && e.RowIndex <= 27) ||
                        (e.ColumnIndex == 6 && e.RowIndex <= 26) ||
                        (e.ColumnIndex == 8 && e.RowIndex <= 25) ||
                        ((e.ColumnIndex == 10 || e.ColumnIndex == 12) && e.RowIndex <= 24) ||
                        (e.ColumnIndex == 20 && e.RowIndex <= 23) ||
                        (e.ColumnIndex == 14 && e.RowIndex <= 22))
                    {
                        BorderRight(p, e);
                    }
                    
                    if ((e.ColumnIndex - 1) / 2 == 0)
                    {
                        if (e.RowIndex == 6 || e.RowIndex == 17 || e.RowIndex == 24)
                            BorderBottom(p, e);
                    }

                    if ((e.ColumnIndex - 1) / 2 == 1)
                    {
                        if (e.RowIndex == 9 || e.RowIndex == 13 || e.RowIndex == 17 || e.RowIndex == 27)
                            BorderBottom(p, e);
                    }

                    if ((e.ColumnIndex - 1) / 2 == 2)
                    {
                        if (e.RowIndex == 9 || e.RowIndex == 26)
                            BorderBottom(p, e);
                    }

                    if ((e.ColumnIndex - 1) / 2 == 3)
                    {
                        if (e.RowIndex == 13 || e.RowIndex == 17 || e.RowIndex == 18 || e.RowIndex == 20 || e.RowIndex == 22 || e.RowIndex == 23 || e.RowIndex == 24 || e.RowIndex == 25)
                            BorderBottom(p, e);
                    }

                    if ((e.ColumnIndex - 1) / 2 == 4)
                    {
                        if (e.RowIndex == 3 || e.RowIndex == 8 || e.RowIndex == 10)
                            BorderBottom(p, e);
                    }

                    if ((e.ColumnIndex - 1) / 2 == 5)
                    {
                        if (e.RowIndex == 3 || e.RowIndex == 8 || e.RowIndex == 14 || e.RowIndex == 17 || e.RowIndex == 18 || e.RowIndex == 22 || e.RowIndex == 23 || e.RowIndex == 24)
                            BorderBottom(p, e);
                    }

                    if ((e.ColumnIndex - 1) / 2 == 6)
                    {
                        if (e.RowIndex == 3 || e.RowIndex == 5 || e.RowIndex == 9 || e.RowIndex == 13 || e.RowIndex == 17 || e.RowIndex == 19 || e.RowIndex == 20 || e.RowIndex == 22)
                            BorderBottom(p, e);
                    }

                    if ((e.ColumnIndex - 1) / 2 == 7)
                    {
                        if ((e.ColumnIndex - 1) == 5 || e.RowIndex == 6 || e.RowIndex == 8 || e.RowIndex == 9 || e.RowIndex == 10 || e.RowIndex == 11 || e.RowIndex == 13 || e.RowIndex == 15 || e.RowIndex == 16 || e.RowIndex == 17 || e.RowIndex == 19 || e.RowIndex == 21)
                            BorderBottom(p, e);
                    }

                    if ((e.ColumnIndex - 1) / 2 == 8)
                    {
                        if (e.RowIndex != 4 && e.RowIndex != 6 && e.RowIndex != 8 && e.RowIndex != 17 && e.RowIndex != 19 && e.RowIndex != 20)
                            BorderBottom(p, e);
                    }

                    if ((e.ColumnIndex - 1) / 2 == 9)
                    {
                        if (e.RowIndex != 4 && e.RowIndex != 6 && e.RowIndex != 9 && e.RowIndex != 20 && e.RowIndex <= 23)
                            BorderBottom(p, e);
                    }
                }
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
