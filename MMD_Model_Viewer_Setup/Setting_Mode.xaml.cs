using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace MMD_Model_Viewer_Setup
{
    public partial class Setting_Mode : Window
    {
        bool IsClosing = false;
        string Path = Directory.GetCurrentDirectory();
        int Volume;
        int Pan;
        double Pitch;
        int Model_Position;
        int Map_Select;
        int Light_Select;
        int Horror_Select;
        int Music_Mode;
        public Setting_Mode()
        {
            InitializeComponent();
            MouseLeftButtonDown += (sender, e) => { DragMove(); };
            Map_C.Items.Add("町");
            Map_C.Items.Add("宇宙");
            if (File.Exists(Path + "/Resources/Map/UserMap/Model.mv1"))
            {
                Map_C.Items.Add("ユーザーマップ");
            }
            Map_C.SelectedIndex = 0;
            Light_C.Items.Add("有効");
            Light_C.Items.Add("無効");
            Light_C.SelectedIndex = 0;
            Horror_C.Items.Add("無効");
            Horror_C.Items.Add("有効");
            Horror_C.SelectedIndex = 0;
            Music_Mode_C.Items.Add("Dxlib");
            Music_Mode_C.Items.Add("WMP(Bass)");
            Music_Mode_C.SelectedIndex = 0;
            Shadow_Mode_C.Items.Add("有効");
            Shadow_Mode_C.Items.Add("無効");
            Shadow_Mode_C.SelectedIndex = 0;
            Shadow_Angle_C.Items.Add("角度:左後");
            Shadow_Angle_C.Items.Add("角度:右後");
            Shadow_Angle_C.Items.Add("角度:左前");
            Shadow_Angle_C.Items.Add("角度:右前");
            Shadow_Angle_C.SelectedIndex = 0;
            Horror_Sky_C.Items.Add("有効");
            Horror_Sky_C.Items.Add("無効");
            Horror_Sky_C.SelectedIndex = 0;
            Horror_C.Visibility = Visibility.Hidden;
            Horror_T.Visibility = Visibility.Hidden;
            Horror_Help_B.Visibility = Visibility.Hidden;
            Horror_Sky_C.Visibility = Visibility.Hidden;
            Horror_Sky_T.Visibility = Visibility.Hidden;
            Horror_Sky_Help_B.Visibility = Visibility.Hidden;
            Window_Show();
            if (File.Exists(Path + "/Resources/Setting.dat"))
            {
                try
                {
                    XDocument item = XDocument.Load(Path + "/Resources/Setting.dat");
                    XElement item_01 = item.Element("Setting_Save");
                    First_Volume.Text = item_01.Element("Volume").Value;
                    First_Pan.Text = item_01.Element("Pan").Value;
                    First_Pitch.Text = item_01.Element("Pitch").Value;
                    First_Position.Text = item_01.Element("Model_Position").Value;
                    Music_Mode_C.SelectedIndex = int.Parse(item_01.Element("Music_Mode").Value);
                    int Map_Select = int.Parse(item_01.Element("Map_Select").Value);
                    if (Map_Select == 2)
                    {
                        if (File.Exists(Path + "/Resources/Map/UserMap/Model.mv1"))
                        {
                            Map_C.SelectedIndex = 2;
                        }
                        else
                        {
                            if (Directory.Exists(Path + "/Resources/Map/UserMap"))
                            {
                                Directory.Delete(Path + "/Resources/Map/UserMap");
                            }
                            Map_C.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        Map_C.SelectedIndex = Map_Select;
                    }
                    Light_C.SelectedIndex = int.Parse(item_01.Element("Light_Select").Value);
                    Horror_C.SelectedIndex = int.Parse(item_01.Element("Horror_Select").Value);
                    Shadow_Angle_C.SelectedIndex = int.Parse(item_01.Element("Shadow_Angle").Value);
                    if (bool.Parse(item_01.Element("Shadow_Mode").Value) == true)
                    {
                        Shadow_Mode_C.SelectedIndex = 0;
                    }
                    else
                    {
                        Shadow_Mode_C.SelectedIndex = 1;
                    }
                    if (Light_C.SelectedIndex == 1)
                    {
                        Horror_C.Visibility = Visibility.Visible;
                        Horror_T.Visibility = Visibility.Visible;
                        Horror_Help_B.Visibility = Visibility.Visible;
                        if (Horror_C.SelectedIndex == 0)
                        {
                            Horror_Sky_C.Visibility = Visibility.Hidden;
                            Horror_Sky_T.Visibility = Visibility.Hidden;
                            Horror_Sky_Help_B.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            Horror_Sky_C.Visibility = Visibility.Visible;
                            Horror_Sky_T.Visibility = Visibility.Visible;
                            Horror_Sky_Help_B.Visibility = Visibility.Visible;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("設定ファイルが破損しています。ファイルは削除されます。\n念のためソフトは強制終了されます。");
                    File.Delete(Path + "/Resources/Setting.dat");
                }
            }
        }
        async void Window_Show()
        {
            Opacity = 0;
            while (Opacity < 1 && IsClosing == false)
            {
                Opacity += 0.05;
                await Task.Delay(30);
            }
        }
        private async void Exit_B_Click(object sender, RoutedEventArgs e)
        {
            if (!IsClosing)
            {
                IsClosing = true;
                while (true)
                {
                    if (Opacity <= 0)
                    {
                        Close();
                        break;
                    }
                    Opacity -= 0.05;
                    await Task.Delay(30);
                }
            }
        }
        private async void Window_1_B_Click(object sender, RoutedEventArgs e)
        {
            if (!IsClosing)
            {
                IsClosing = true;
                while (true)
                {
                    if (Opacity <= 0)
                    {
                        MainCode f = new MainCode();
                        f.Show();
                        Close();
                        break;
                    }
                    Opacity -= 0.05;
                    await Task.Delay(30);
                }
            }
        }
        private void Number_Only_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }
        private void OK_B_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Volume = int.Parse(First_Volume.Text);
                Pan = int.Parse(First_Pan.Text);
                Pitch = double.Parse(First_Pitch.Text);
                Model_Position = int.Parse(First_Position.Text);
                Map_Select = Map_C.SelectedIndex;
                Light_Select = Light_C.SelectedIndex;
                Horror_Select = Horror_C.SelectedIndex;
                Music_Mode = Music_Mode_C.SelectedIndex;
            }
            catch
            {
                MessageBox.Show("不備があります。もう一度確認してください。\n数字以外を入力しないようにしてください。");
                return;
            }
            if (Volume > 255)
            {
                MessageBox.Show("音量の値が正しくありません。\n音量の有効な値は0～255です。");
                return;
            }
            if (Pan < -255 || Pan > 255)
            {
                MessageBox.Show("パンの値が正しくありません。パンの有効な値は-255～255です。");
                return;
            }
            if (Pitch < 0.3 || Pitch > 1.6)
            {
                MessageBox.Show("速度+ピッチの値が正しくありません。速度+ピッチの有効な値は0.3～1.6です。");
                return;
            }
            bool Shadow = false;
            if (Shadow_Mode_C.SelectedIndex == 0)
            {
                Shadow = true;
            }
            else
            {
                Shadow = false;
            }
            StreamWriter s = File.CreateText(Path + "/Resources/Setting.dat");
            s.Close();
            Setting_Save obj = new Setting_Save
            {
                Volume = Volume,
                Pan = Pan,
                Pitch = Pitch,
                Model_Position = Model_Position,
                Map_Select = Map_Select,
                Light_Select = Light_Select,
                Horror_Select = Horror_Select,
                Horror_Sky_Select = Horror_Sky_C.SelectedIndex,
                Music_Mode = Music_Mode,
                Shadow_Mode = Shadow,
                Shadow_Angle = Shadow_Angle_C.SelectedIndex
            };
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Setting_Save));
            StreamWriter sw = new StreamWriter(Path + "/Resources/Setting.dat", false, new System.Text.UTF8Encoding(false));
            serializer.Serialize(sw, obj);
            sw.Close();
            MessageBox.Show("適応しました。");
        }
        private void Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("モデル同士の距離を指定します。モデルのモーションが1人用でない限り0を推奨します。\nサンプル動画ではモーションが1人用のため間隔を18にしています。");
        }
        private void Light_C_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Light_C.SelectedIndex == 1)
            {
                Horror_C.Visibility = Visibility.Visible;
                Horror_T.Visibility = Visibility.Visible;
                Horror_Help_B.Visibility = Visibility.Visible;
            }
            else
            {
                Horror_C.Visibility = Visibility.Hidden;
                Horror_T.Visibility = Visibility.Hidden;
                Horror_Help_B.Visibility = Visibility.Hidden;
            }
        }
        private void Horror_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("これを有効にすると空間内に光がなくなり、暗闇の中の悪夢を見ることができます。");
        }
        private void Extra_Setting_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("設定を細かく指定できます。プレビューを行うことができないため地道に調整してください。");
        }
        private async void Extra_Setting_B_Click(object sender, RoutedEventArgs e)
        {
            if (!IsClosing)
            {
                IsClosing = true;
                while (Opacity > 0)
                {
                    Opacity -= 0.025;
                    await Task.Delay(1000 / 60);
                }
                Advance_Setting f = new Advance_Setting();
                f.Show();
                Close();
            }
        }
        private async void Map_B_Click(object sender, RoutedEventArgs e)
        {
            if (!IsClosing)
            {
                IsClosing = true;
                while (Opacity > 0)
                {
                    Opacity -= 0.025;
                    await Task.Delay(1000 / 60);
                }
                Map_Mode f = new Map_Mode();
                f.Show();
                Close();
            }
        }
        private void Music_Mode_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dxlibモードでは、再生速度とピッチの両方を変更します。WMPモードでは再生速度のみ変更しピッチは変更されません。C++ではWMPはBassモードになり、再生速度とピッチが独立して設定できます。詳しい違いはReadme.txtをお読みください。");
        }
        private void Shadow_Mode_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("影はキャラクターにのみ適応されます。有効化すると影の角度を指定できます。");
        }
        private void Shadow_Mode_C_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Shadow_Mode_C.SelectedIndex == 0)
            {
                Shadow_Angle_C.Visibility = Visibility.Visible;
            }
            else
            {
                Shadow_Angle_C.Visibility = Visibility.Hidden;
            }
        }
        private void Horror_Sky_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ホラーモード時の空のテクスチャを変更するかを指定します。有効化した場合空が赤っぽくなります。");
        }
        private void Horror_C_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Horror_C.Visibility == Visibility.Visible)
            {
                if (Horror_C.SelectedIndex == 0)
                {
                    Horror_Sky_C.Visibility = Visibility.Hidden;
                    Horror_Sky_T.Visibility = Visibility.Hidden;
                    Horror_Sky_Help_B.Visibility = Visibility.Hidden;
                }
                else
                {
                    Horror_Sky_C.Visibility = Visibility.Visible;
                    Horror_Sky_T.Visibility = Visibility.Visible;
                    Horror_Sky_Help_B.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
public class Setting_Save
{
    public int Volume;
    public int Pan;
    public int Model_Position;
    public int Map_Select;
    public int Light_Select;
    public int Horror_Select;
    public int Horror_Sky_Select;
    public int Music_Mode;
    public int Shadow_Angle;
    public double Pitch;
    public bool Shadow_Mode;
}