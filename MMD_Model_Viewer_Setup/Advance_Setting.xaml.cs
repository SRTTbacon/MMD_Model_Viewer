using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MMD_Model_Viewer_Setup
{
    public partial class Advance_Setting : Window
    {
        bool IsClosing = false;
        bool IsModelPositionSet = true;
        int Mouse_Sensitivity = 20;
        int Model_Number_Now = 1;
        int FPS = 1;
        double Pitch_Interval = 0.05;
        double Pitch_Min = 0.3;
        double Pitch_Max = 1.6;
        string Path = Directory.GetCurrentDirectory();
        string Model_01_X = "0";
        string Model_01_Y = "0";
        string Model_01_Z = "0";
        string Model_02_X = "0";
        string Model_02_Y = "0";
        string Model_02_Z = "0";
        string Model_03_X = "0";
        string Model_03_Y = "0";
        string Model_03_Z = "0";
        string Model_04_X = "0";
        string Model_04_Y = "0";
        string Model_04_Z = "0";
        string Model_05_X = "0";
        string Model_05_Y = "0";
        string Model_05_Z = "0";
        public Advance_Setting()
        {
            InitializeComponent();
            MouseLeftButtonDown += (sender, e) => { DragMove(); };
            FPS_C.Items.Add("        30");
            FPS_C.Items.Add("        60");
            FPS_C.Items.Add("       120");
            FPS_C.SelectedIndex = 1;
            Physics_Map_C.IsChecked = true;
            Physics_Model_C.IsChecked = true;
            Window_Show();
            if (File.Exists(Path + "/Resources/Advance_Setting.dat"))
            {
                try
                {
                    XDocument item = XDocument.Load(Path + "/Resources/Advance_Setting.dat");
                    XElement item_01 = item.Element("Advance_Save");
                    Model_01_X = item_01.Element("Model_01_X").Value;
                    Model_01_Y = item_01.Element("Model_01_Y").Value;
                    Model_01_Z = item_01.Element("Model_01_Z").Value;
                    Model_02_X = item_01.Element("Model_02_X").Value;
                    Model_02_Y = item_01.Element("Model_02_Y").Value;
                    Model_02_Z = item_01.Element("Model_02_Z").Value;
                    Model_03_X = item_01.Element("Model_03_X").Value;
                    Model_03_Y = item_01.Element("Model_03_Y").Value;
                    Model_03_Z = item_01.Element("Model_03_Z").Value;
                    Model_04_X = item_01.Element("Model_04_X").Value;
                    Model_04_Y = item_01.Element("Model_04_Y").Value;
                    Model_04_Z = item_01.Element("Model_04_Z").Value;
                    Model_05_X = item_01.Element("Model_05_X").Value;
                    Model_05_Y = item_01.Element("Model_05_Y").Value;
                    Model_05_Z = item_01.Element("Model_05_Z").Value;
                    FPS_C.SelectedIndex = int.Parse(item_01.Element("FPS").Value);
                    Mouse_Sensitivity = int.Parse(item_01.Element("Mouse_Sensitivity").Value);
                    Pitch_Interval = double.Parse(item_01.Element("Pitch_Interval").Value);
                    Pitch_Min_S.Value = double.Parse(item_01.Element("Pitch_Min").Value);
                    Pitch_Max_S.Value = double.Parse(item_01.Element("Pitch_Max").Value);
                    FrameRateLock_C.IsChecked = bool.Parse(item_01.Element("FrameRateLock").Value);
                    Music_Not_Change_C.IsChecked = bool.Parse(item_01.Element("IsMusicNotChange").Value);
                    Physics_Map_C.IsChecked = bool.Parse(item_01.Element("IsPhysicsSet").Value);
                    Physics_Model_C.IsChecked = bool.Parse(item_01.Element("IsModelPhysicsSet").Value);
                    Mouse_Sensitivity_S.Value = Mouse_Sensitivity;
                    Mouse_Sensitivity_T.Text = Mouse_Sensitivity.ToString();
                    Pitch_Interval_S.Value = Pitch_Interval;
                    Pitch_Interval_T.Text = Pitch_Interval.ToString();
                    Pitch_Min_T.Text = Pitch_Min.ToString();
                    Pitch_Max_T.Text = Pitch_Max.ToString();
                    Location_Change();
                }
                catch
                {
                    IsClosing = true;
                    MessageBox.Show("詳細設定ファイルが破損しています。ファイルは削除されます。\n念のためソフトは強制終了されます。");
                    File.Delete(Path + "/Resources/Advance_Setting.dat");
                    Close();
                }
            }
        }
        async void Window_Show()
        {
            Opacity = 0;
            while (Opacity < 1 && IsClosing == false)
            {
                Opacity += 0.025;
                await Task.Delay(1000 / 60);
            }
        }
        private async void Back_B_Click(object sender, RoutedEventArgs e)
        {
            if (!IsClosing)
            {
                IsClosing = true;
                while (Opacity > 0)
                {
                    Opacity -= 0.025;
                    await Task.Delay(1000 / 60);
                }
                Setting_Mode f = new Setting_Mode();
                f.Show();
                Close();
            }
        }
        private async void Exit_B_Click(object sender, RoutedEventArgs e)
        {
            if (!IsClosing)
            {
                IsClosing = true;
                while (Opacity > 0)
                {
                    Opacity -= 0.025;
                    await Task.Delay(1000 / 60);
                }
                Close();
            }
        }
        private void Model_Back_B_Click(object sender, RoutedEventArgs e)
        {
            if (Model_Number_Now > 1)
            {
                Model_Number_Now--;
                Location_Change();
            }
        }
        private void Model_Next_B_Click(object sender, RoutedEventArgs e)
        {
            if (Model_Number_Now < 5)
            {
                Model_Number_Now++;
                Location_Change();
            }
        }
        void Location_Change()
        {
            Model_Number_T.Text = "モデル:" + Model_Number_Now;
            if (Model_Number_Now == 1)
            {
                Location_X.Text = Model_01_X;
                Location_Y.Text = Model_01_Y;
                Location_Z.Text = Model_01_Z;
            }
            else if (Model_Number_Now == 2)
            {
                Location_X.Text = Model_02_X;
                Location_Y.Text = Model_02_Y;
                Location_Z.Text = Model_02_Z;
            }
            else if (Model_Number_Now == 3)
            {
                Location_X.Text = Model_03_X;
                Location_Y.Text = Model_03_Y;
                Location_Z.Text = Model_03_Z;
            }
            else if (Model_Number_Now == 4)
            {
                Location_X.Text = Model_04_X;
                Location_Y.Text = Model_04_Y;
                Location_Z.Text = Model_04_Z;
            }
            else if (Model_Number_Now == 5)
            {
                Location_X.Text = Model_05_X;
                Location_Y.Text = Model_05_Y;
                Location_Z.Text = Model_05_Z;
            }
        }
        private void Location_X_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Model_Number_Now == 1)
            {
                Model_01_X = Location_X.Text;
            }
            else if (Model_Number_Now == 2)
            {
                Model_02_X = Location_X.Text;
            }
            else if (Model_Number_Now == 3)
            {
                Model_03_X = Location_X.Text;
            }
            else if (Model_Number_Now == 4)
            {
                Model_04_X = Location_X.Text;
            }
            else if (Model_Number_Now == 5)
            {
                Model_05_X = Location_X.Text;
            }
        }
        private void Location_Y_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Model_Number_Now == 1)
            {
                Model_01_Y = Location_Y.Text;
            }
            else if (Model_Number_Now == 2)
            {
                Model_02_Y = Location_Y.Text;
            }
            else if (Model_Number_Now == 3)
            {
                Model_03_Y = Location_Y.Text;
            }
            else if (Model_Number_Now == 4)
            {
                Model_04_Y = Location_Y.Text;
            }
            else if (Model_Number_Now == 5)
            {
                Model_05_Y = Location_Y.Text;
            }
        }
        private void Location_Z_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Model_Number_Now == 1)
            {
                Model_01_Z = Location_Z.Text;
            }
            else if (Model_Number_Now == 2)
            {
                Model_02_Z = Location_Z.Text;
            }
            else if (Model_Number_Now == 3)
            {
                Model_03_Z = Location_Z.Text;
            }
            else if (Model_Number_Now == 4)
            {
                Model_04_Z = Location_Z.Text;
            }
            else if (Model_Number_Now == 5)
            {
                Model_05_Z = Location_Z.Text;
            }
        }
        private void Model_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("モデル1～5の座標を指定します。これを設定すると通常設定画面の\"モデルの位置間隔\"の項目が無視されます。\n指定しない場合は0と入力します。");
        }
        private void Save_B_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Pitch_Min > Pitch_Max)
                {
                    MessageBox.Show("ピッチ範囲の最小値が最大値を超えています。");
                    return;
                }
                if (Model_01_X == "0" && Model_01_Y == "0" && Model_01_Z == "0" && Model_02_X == "0" && Model_02_Y == "0" && Model_02_Z == "0" && Model_03_X == "0" && Model_03_Y == "0" && Model_03_Z == "0" && 
                    Model_04_X == "0" && Model_04_Y == "0" && Model_04_Z == "0" && Model_05_X == "0" && Model_05_Y == "0" && Model_05_Z == "0")
                {
                    IsModelPositionSet = false;
                }
                Advance_Save obj2 = new Advance_Save();
                StreamWriter s = File.CreateText(Path + "/Resources/Advance_Setting.dat");
                s.Close();
                Advance_Save obj = new Advance_Save
                {
                    Model_01_X = int.Parse(Model_01_X),
                    Model_01_Y = int.Parse(Model_01_Y),
                    Model_01_Z = int.Parse(Model_01_Z),
                    Model_02_X = int.Parse(Model_02_X),
                    Model_02_Y = int.Parse(Model_02_Y),
                    Model_02_Z = int.Parse(Model_02_Z),
                    Model_03_X = int.Parse(Model_03_X),
                    Model_03_Y = int.Parse(Model_03_Y),
                    Model_03_Z = int.Parse(Model_03_Z),
                    Model_04_X = int.Parse(Model_04_X),
                    Model_04_Y = int.Parse(Model_04_Y),
                    Model_04_Z = int.Parse(Model_04_Z),
                    Model_05_X = int.Parse(Model_05_X),
                    Model_05_Y = int.Parse(Model_05_Y),
                    Model_05_Z = int.Parse(Model_05_Z),
                    FPS = FPS,
                    Mouse_Sensitivity = Mouse_Sensitivity,
                    Pitch_Interval = (double)Math.Round((decimal)Pitch_Interval, 2, MidpointRounding.AwayFromZero),
                    FrameRateLock = FrameRateLock_C.IsChecked.Value,
                    IsMusicNotChange = Music_Not_Change_C.IsChecked.Value,
                    IsModelPositionSet = IsModelPositionSet,
                    IsPhysicsSet = Physics_Map_C.IsChecked.Value,
                    IsModelPhysicsSet = Physics_Model_C.IsChecked.Value,
                    Pitch_Min = Pitch_Min,
                    Pitch_Max = Pitch_Max
                };
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Advance_Save));
                StreamWriter sw = new StreamWriter(Path + "/Resources/Advance_Setting.dat", false, new System.Text.UTF8Encoding(false));
                serializer.Serialize(sw, obj);
                sw.Close();
                MessageBox.Show("保存しました。");
                if (File.Exists(Path + "/Resources/Setting.dat"))
                {
                    XDocument item = XDocument.Load(Path + "/Resources/Setting.dat");
                    XElement item_01 = item.Element("Setting_Save");
                    double Pitch_Set = double.Parse(item_01.Element("Pitch").Value);
                    if (Pitch_Set < Pitch_Min || Pitch_Set > Pitch_Max)
                    {
                        MessageBox.Show("通常設定のピッチが現在のピッチの範囲外です。どちらかの設定を修正することをお勧めします。\n現在の通常設定のピッチ : " + Pitch_Set);
                    }
                }
            }
            catch
            {
                File.Delete(Path + "/Resources/Advance_Setting.dat");
                MessageBox.Show("エラー:数字以外が入力されているか、空欄がある可能性があります。もう一度確認してください。");
            }
        }
        private void Mouse_Sensitivity_S_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Mouse_Sensitivity = (int)Mouse_Sensitivity_S.Value;
            if (IsLoaded)
            {
                Mouse_Sensitivity_T.Text = ((int)Mouse_Sensitivity_S.Value).ToString();
            }
        }
        private void Pitch_Interval_S_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Pitch_Interval = Pitch_Interval_S.Value;
            if (IsLoaded)
            {
                Pitch_Interval_T.Text = Math.Round((decimal)Pitch_Interval, 2, MidpointRounding.AwayFromZero).ToString();
            }
        }
        private void Mouse_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("マウスの感度を指定します。数値が小さいほど感度が高くなり、数値が大きいほど低くなります。");
        }
        private void Pitch_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ピッチの操作間隔を指定します。これを設定するとC+PまたはC+Oキーを押したときの変更度が変わります。(語彙力崩壊)");
        }
        private void Reset_B_Click(object sender, RoutedEventArgs e)
        {
            Model_01_X = "0";
            Model_01_Y = "0";
            Model_01_Z = "0";
            Model_02_X = "0";
            Model_02_Y = "0";
            Model_02_Z = "0";
            Model_03_X = "0";
            Model_03_Y = "0";
            Model_03_Z = "0";
            Model_04_X = "0";
            Model_04_Y = "0";
            Model_04_Z = "0";
            Model_05_X = "0";
            Model_05_Y = "0";
            Model_05_Z = "0";
            FPS_C.SelectedIndex = 1;
            FrameRateLock_C.IsChecked = false;
            Mouse_Sensitivity_S.Value = 40;
            Pitch_Interval_S.Value = 0.1;
            IsModelPositionSet = false;
            Music_Not_Change_C.IsChecked = false;
            Physics_Map_C.IsChecked = true;
            Physics_Model_C.IsChecked = true;
            Pitch_Min_S.Value = 0.3;
            Pitch_Max_S.Value = 1.6;
            MessageBox.Show("初期化しました。保存を押すと初期状態の設定になります。");
        }
        private void FPS_C_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FPS = FPS_C.SelectedIndex;
        }
        private void Pitch_Min_S_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Pitch_Min = (double)Math.Round((decimal)Pitch_Min_S.Value, 2, MidpointRounding.AwayFromZero);
            if (IsLoaded)
            {
                Pitch_Min_T.Text = Pitch_Min.ToString();
            }
        }
        private void Pitch_Max_S_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Pitch_Max = (double)Math.Round((decimal)Pitch_Max_S.Value, 2, MidpointRounding.AwayFromZero);
            if (IsLoaded)
            {
                Pitch_Max_T.Text = Pitch_Max.ToString();
            }
        }
        private void FrameRateLock_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("処理落ちしていない場合有効にする必要はありません。この設定を有効にすると処理落ちしている場合曲とモーションが合うようになりますが、モーションがカクカクします。");
        }
        private void Music_Not_Change_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("これにチェックを入れるとC+OまたはC+Pキーを押した際に曲の再生速度は変更されずにモーションの速度のみ変更するようになります。");
        }
        private void Physics_Mode_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("マップの当たり判定を有効化するかを設定します。マップによっては初期位置から動けなくなる場合があるのでご注意ください。PCの性能によって処理落ちする場合があります。");
        }
        private void Physics_Model_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("キャラクターの当たり判定を有効化するか設定します。PCの性能によって処理落ちする場合があります。");
        }
    }
}
public class Advance_Save
{
    public int Model_01_X;
    public int Model_01_Y;
    public int Model_01_Z;
    public int Model_02_X;
    public int Model_02_Y;
    public int Model_02_Z;
    public int Model_03_X;
    public int Model_03_Y;
    public int Model_03_Z;
    public int Model_04_X;
    public int Model_04_Y;
    public int Model_04_Z;
    public int Model_05_X;
    public int Model_05_Y;
    public int Model_05_Z;
    public int FPS;
    public int Mouse_Sensitivity;
    public double Pitch_Interval;
    public double Pitch_Min;
    public double Pitch_Max;
    public bool FrameRateLock;
    public bool IsModelPositionSet;
    public bool IsMusicNotChange;
    public bool IsPhysicsSet;
    public bool IsModelPhysicsSet;
}