using DxLibDLL;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MMD_Model_Viewer_Setup
{
    public partial class Map_Mode : Window
    {
        string Path = Directory.GetCurrentDirectory();
        string FilePath = "";
        new string Name = "";
        bool IsClosing = false;
        bool IsModelCreating = false;
        int Map_Scale = 0;
        int Map_X = 0;
        int Map_Y = 0;
        int Map_Z = 0;
        int Map_Rotate = 0;
        bool Sky_Enable = false;
        public Map_Mode()
        {
            InitializeComponent();
            MouseLeftButtonDown += (sender, e) => { DragMove(); };
            Wait_P.Opacity = 0;
            Wait_T.Opacity = 0;
            Wait_P.Visibility = Visibility.Hidden;
            Wait_T.Visibility = Visibility.Hidden;
            Sky_C.Items.Add("無効");
            Sky_C.Items.Add("有効");
            Sky_C.SelectedIndex = 0;
            if (File.Exists(Path + "/Resources/Map_Setting.dat"))
            {
                try
                {
                    XDocument item = XDocument.Load(Path + "/Resources/Map_Setting.dat");
                    XElement item_01 = item.Element("Map_Setting");
                    Map_Scale_T.Text = item_01.Element("Map_Scale").Value;
                    Map_Position_X.Text = item_01.Element("Map_X").Value;
                    Map_Position_Y.Text = item_01.Element("Map_Y").Value;
                    Map_Position_Z.Text = item_01.Element("Map_Z").Value;
                    Map_Rotate_T.Text = item_01.Element("Map_Rotate").Value;
                    if (bool.Parse(item_01.Element("Sky_Enable").Value))
                    {
                        Sky_C.SelectedIndex = 1;
                    }
                    else
                    {
                        Sky_C.SelectedIndex = 0;
                    }
                    Name = item_01.Element("Name").Value;
                    Map_Select_T.Text = "選択されているマップ:" + Name;
                }
                catch
                {
                    File.Delete(Path + "/Resources/Map_Setting.dat");
                    MessageBox.Show("マップの設定ファイルが破損しています。ソフトを再起動してください。");
                    IsClosing = true;
                    Close();
                }
            }
            Window_Show();
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
        private async void Exit_B_Click(object sender, RoutedEventArgs e)
        {
            if (!IsClosing)
            {
                bool IsCreate = false;
                if (IsModelCreating)
                {
                    MessageBoxResult result = MessageBox.Show("処理が実行中です。終了しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                    else
                    {
                        IsCreate = true;
                    }
                }
                IsClosing = true;
                while (Opacity > 0)
                {
                    Opacity -= 0.025;
                    await Task.Delay(1000 / 60);
                }
                if (IsCreate)
                {
                    Directory.Delete(Path + "/Resources/Map/UserMap", true);
                }
                Close();
            }
        }
        private async void Back_B_Click(object sender, RoutedEventArgs e)
        {
            if (!IsClosing)
            {
                bool IsCreate = false;
                if (IsModelCreating)
                {
                    MessageBoxResult result = MessageBox.Show("処理が実行中です。戻りますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                    else
                    {
                        IsCreate = true;
                    }
                }
                IsClosing = true;
                while (Opacity > 0)
                {
                    Opacity -= 0.025;
                    await Task.Delay(1000 / 60);
                }
                Setting_Mode f = new Setting_Mode();
                f.Show();
                if (IsCreate)
                {
                    Directory.Delete(Path + "/Resources/Map/UserMap", true);
                }
                Close();
            }
        }
        private async void Map_Select_B_Click(object sender, RoutedEventArgs e)
        {
            if (IsModelCreating)
            {
                MessageBox.Show("処理が実行中です。");
                return;
            }
            if (File.Exists(Path + "/Resources/Map/UserMap/Model.mv1"))
            {
                MessageBoxResult result = MessageBox.Show("既にマップが参照されています。依存のマップに上書きしますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog()
            {
                Title = "マップモデルを参照",
                Multiselect = false,
                Filter = "MMDマップモデルファイル (*.pmx;*.pmd;*.x)|*.pmx;*.pmd;*.x"
            };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Directory_Name = ofd.FileName.Replace(System.IO.Path.GetFileName(ofd.FileName), "");
                if (Directory.GetFiles(Directory_Name, "*", SearchOption.AllDirectories).Length >= 100)
                {
                    MessageBoxResult result = MessageBox.Show("選択したファイルのフォルダ内に100個以上ファイルが存在します。\n選択したファイルが存在するフォルダをコピーしますがよろしいですか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                await Task.Delay(10);
                if (Directory.Exists(Path + "/Resources/Map/UserMap"))
                {
                    Directory.Delete(Path + "/Resources/Map/UserMap", true);
                }
                string[] a = Directory.GetFiles(Directory_Name, "*", SearchOption.AllDirectories);
                foreach (string Files in a)
                {
                    string File_Path = Files.Replace(Directory_Name, "");
                    string File_Path_01 = File_Path.Replace(System.IO.Path.GetFileName(Files), "");
                    if (!Directory.Exists(Path + "/Resources/Map/UserMap/" + File_Path_01))
                    {
                        Directory.CreateDirectory(Path + "/Resources/Map/UserMap/" + File_Path_01);
                    }
                    if (File_Path_01 == "")
                    {
                        File.Copy(Files, Path + "/Resources/Map/UserMap/" + System.IO.Path.GetFileName(Files), true);
                    }
                    else
                    {
                        File.Copy(Files, Path + "/Resources/Map/UserMap/" + File_Path_01 + System.IO.Path.GetFileName(Files), true);
                    }
                }
                File.Copy(ofd.FileName, Path + "/Resources/Map/UserMap/Model" + System.IO.Path.GetExtension(ofd.FileName), true);
                File.Delete(Path + "/Resources/Map/UserMap/" + System.IO.Path.GetFileName(ofd.FileName));
                Map_Select_T.Text = "選択されているマップ:" + System.IO.Path.GetFileName(ofd.FileName);
                Name = System.IO.Path.GetFileName(ofd.FileName);
                FilePath = Path + "/Resources/Map/UserMap/Model" + System.IO.Path.GetExtension(ofd.FileName);
            }
        }
        private void Map_Scale_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("マップのサイズを変更します。この項目に5と入力した場合5倍に拡大されたモデルが描画されます。(間違っても変な数字にしないでください)");
        }
        private async void Save_B_Click(object sender, RoutedEventArgs e)
        {
            if (IsModelCreating)
            {
                MessageBox.Show("処理が実行中です。");
                return;
            }
            if (Map_Scale_T.Text == "")
            {
                MessageBox.Show("マップサイズの欄が空白です。");
                return;
            }
            else if (Map_Scale_T.Text == "0")
            {
                MessageBox.Show("マップサイズを0にすることはできません。");
                return;
            }
            try
            {
                Map_X = int.Parse(Map_Position_X.Text);
                Map_Y = int.Parse(Map_Position_Y.Text);
                Map_Z = int.Parse(Map_Position_Z.Text);
            }
            catch
            {
                MessageBox.Show("位置が正しくありません。");
                return;
            }
            try
            {
                Map_Scale = int.Parse(Map_Scale_T.Text);
            }
            catch
            {
                MessageBox.Show("マップサイズが正しくありません。");
                return;
            }
            try
            {
                Map_Rotate = int.Parse(Map_Rotate_T.Text);
            }
            catch
            {
                MessageBox.Show("横回転の項目に誤りがあります。");
                return;
            }
            IsModelCreating = true;
            Menu.Visibility = Visibility.Visible;
            Wait_P.Visibility = Visibility.Visible;
            Wait_T.Visibility = Visibility.Visible;
            while (Wait_T.Opacity < 1)
            {
                Wait_P.Opacity += 0.05;
                Wait_T.Opacity += 0.05;
                await Task.Delay(1000 / 60);
            }
            bool Wait = true;
            Task task_01 = Task.Run(() =>
            {
                Task task_02 = Task.Run(() =>
                {
                    int ModelHandle = DX.MV1LoadModel(FilePath);
                    DX.MV1SaveModelToMV1File(ModelHandle, Path + "/Resources/Map/UserMap/Model.mv1", DX.MV1_SAVETYPE_NORMAL, 0, DX.FALSE, 0, 0, 0, 0);
                    DX.MV1DeleteModel(ModelHandle);
                });
                task_02.Wait();
                Wait = false;
            });
            while (Wait)
            {
                await Task.Delay(1000);
            }
            Wait_T.Text = "処理が完了しました。";
            await Task.Delay(500);
            while (Wait_T.Opacity > 0)
            {
                Wait_P.Opacity -= 0.05;
                Wait_T.Opacity -= 0.05;
                await Task.Delay(1000 / 60);
            }
            Wait_P.Visibility = Visibility.Hidden;
            Wait_T.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Hidden;
            IsModelCreating = false;
            StreamWriter s = File.CreateText(Path + "/Resources/Map_Setting.dat");
            s.Close();
            Map_Setting obj = new Map_Setting
            {
                Map_Scale = Map_Scale,
                Map_X = Map_X,
                Map_Y = Map_Y,
                Map_Z = Map_Z,
                Map_Rotate = Map_Rotate,
                Sky_Enable = Sky_Enable,
                Name = Name
            };
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Map_Setting));
            StreamWriter sw = new StreamWriter(Path + "/Resources/Map_Setting.dat", false, new System.Text.UTF8Encoding(false));
            serializer.Serialize(sw, obj);
            sw.Close();
            XDocument item = XDocument.Load(Path + "/Resources/Setting.dat");
            XElement item_01 = item.Element("Setting_Save");
            string Temp = "";
            string Replace = "";
            if (item_01.Element("Map_Select").Value == "0")
            {
                Temp = "町";
                Replace = "<Map_Select>0</Map_Select>";
            }
            else if (item_01.Element("Map_Select").Value == "1")
            {
                Temp = "宇宙";
                Replace = "<Map_Select>1</Map_Select>";
            }
            else
            {
                return;
            }
            MessageBoxResult result = MessageBox.Show("現在指定されているマップは" + Temp + "です。ユーザーマップに変更しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                StreamReader str = new StreamReader(Path + "/Resources/Setting.dat");
                string Read = str.ReadToEnd();
                str.Close();
                File.Delete(Path + "/Resources/Setting.dat");
                StreamWriter stw = File.CreateText(Path + "/Resources/Setting.dat");
                stw.Write(Read.Replace(Replace, "<Map_Select>2</Map_Select>"));
                stw.Close();
            }
        }
        private void Map_Scale_T_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }
        private void Delete_B_Click(object sender, RoutedEventArgs e)
        {
            if (IsModelCreating)
            {
                MessageBox.Show("処理が実行中です。");
                return;
            }
            if (!Directory.Exists(Path + "/Resources/Map/UserMap"))
            {
                MessageBox.Show("ユーザーマップが存在しません。");
                return;
            }
            MessageBoxResult result = MessageBox.Show("ユーザーマップを削除しますか？この操作は取り消せません。", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                Directory.Delete(Path + "/Resources/Map/UserMap", true);
                File.Delete(Path + "/Resources/Map_Setting.dat");
                XDocument item = XDocument.Load(Path + "/Resources/Setting.dat");
                XElement item_01 = item.Element("Setting_Save");
                if (item_01.Element("Map_Select").Value == "2")
                {
                    StreamReader str = new StreamReader(Path + "/Resources/Setting.dat");
                    string Read = str.ReadToEnd();
                    str.Close();
                    File.Delete(Path + "/Resources/Setting.dat");
                    StreamWriter stw = File.CreateText(Path + "/Resources/Setting.dat");
                    stw.Write(Read.Replace("<Map_Select>2</Map_Select>", "<Map_Select>0</Map_Select>"));
                    stw.Close();
                }
                Map_Select_T.Text = "選択されているマップ:";
                MessageBox.Show("ユーザーマップを削除しました。");
            }
        }
        private void Sky_C_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Sky_C.SelectedIndex == 0)
            {
                Sky_Enable = false;
            }
            else
            {
                Sky_Enable = true;
            }
        }
        private void Sky_Help_B_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("マップに空のテクスチャが入っている場合有効にする必要はありません。");
        }
        private void Configs_Save_B_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter s = File.CreateText(Path + "/Resources/Map_Setting.dat");
            s.Close();
            Map_Setting obj = new Map_Setting
            {
                Map_Scale = Map_Scale,
                Map_X = Map_X,
                Map_Y = Map_Y,
                Map_Z = Map_Z,
                Map_Rotate = Map_Rotate,
                Sky_Enable = Sky_Enable,
                Name = Name
            };
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Map_Setting));
            StreamWriter sw = new StreamWriter(Path + "/Resources/Map_Setting.dat", false, new System.Text.UTF8Encoding(false));
            serializer.Serialize(sw, obj);
            sw.Close();
            MessageBox.Show("保存しました。");
        }
    }
}
public class Map_Setting
{
    public int Map_Scale;
    public int Map_X;
    public int Map_Y;
    public int Map_Z;
    public int Map_Rotate;
    public bool Sky_Enable;
    public string Name;
}