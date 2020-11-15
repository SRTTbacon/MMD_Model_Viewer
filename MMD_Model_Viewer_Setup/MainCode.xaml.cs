using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Threading.Tasks;
using DxLibDLL;
using System;
using System.Xml.Linq;

namespace MMD_Model_Viewer_Setup
{
    public partial class MainCode : Window
    {
        string Path = Directory.GetCurrentDirectory();
        int MMD_Number_C = 0;
        string MMD_Model_Motion_Not_MV1_List = "";
        bool IsStarted = true;
        bool IsMV1Creating = false;
        string Music_Path = "";
        string MMD_Model_All = "";
        bool IsSetting_Mode_Showing = false;
        public MainCode()
        {
            InitializeComponent();
            DX.SetAlwaysRunFlag(DX.TRUE);
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);
            DX.SetUseFPUPreserveFlag(DX.TRUE);
            DX.SetWaitVSyncFlag(DX.FALSE);
            DX.SetOutApplicationLogValidFlag(DX.FALSE);
            DX.SetDoubleStartValidFlag(DX.TRUE);
            DX.SetMouseDispFlag(DX.TRUE);
            DX.SetUseDXArchiveFlag(DX.TRUE);
            DX.SetUserWindow(Handle);
            DX.SetWindowVisibleFlag(DX.FALSE);
            if (DX.DxLib_Init() < 0)
            {
                MessageBox.Show("初期化エラー");
                return;
            }
            MouseLeftButtonDown += (sender, e) => { DragMove(); };
            Window_Show();
            Progress_B.Visibility = Visibility.Hidden;
            Progress_T.Visibility = Visibility.Hidden;
            if (File.Exists(Path + "/Data.dat"))
            {
                XDocument item = XDocument.Load(Path + "/Data.dat");
                XElement item_01 = item.Element("Save_This_Window_List");
                string Message = item_01.Element("Lists").Value;
                string[] aa = Message.Split('\n');
                MMD_Number_C = int.Parse(item_01.Element("MMD_Number").Value);
                MMD_Number.Text = item_01.Element("MMD_Number").Value;
                Music_Path = item_01.Element("Music_Path").Value;
                if (Music_Path == "")
                {
                    Music_T.Text = "選択された曲:未選択";
                }
                else
                {
                    Music_T.Text = "選択された曲:" + System.IO.Path.GetFileName(item_01.Element("Music_Path").Value);
                }
                int Main = 0;
                while (true)
                {
                    MMD_Model_List.Items.Add(aa[Main]);
                    MMD_Model_All += aa[Main] + "\n";
                    Main++;
                    if (aa.Length - 1 == Main)
                    {
                        break;
                    }
                }
            }
        }
        public IntPtr Handle
        {
            get
            {
                var helper = new System.Windows.Interop.WindowInteropHelper(this);
                return helper.Handle;
            }
        }
        async void Window_Show()
        {
            Opacity = 0;
            while (true)
            {
                if (Opacity >= 1 || IsStarted == false)
                {
                    IsStarted = false;
                    break;
                }
                Opacity += 0.05;
                await Task.Delay(30);
            }
        }
        private void MMD_Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
        }
        private void MMD_Number_B_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.Parse(MMD_Number.Text) >= 1 && int.Parse(MMD_Number.Text) <= 5)
                {
                    MMD_Number_C = int.Parse(MMD_Number.Text);
                }
                else
                {
                    MessageBox.Show("数字は1以上5以下にしてください。");
                }
            }
            catch
            {
                MessageBox.Show("数字以外を入力することはできません。");
            }
        }
        private async void MMD_Model_Select_Click(object sender, RoutedEventArgs e)
        {
            if (IsMV1Creating == true)
            {
                Creating_Error_Message();
                return;
            }
            if (MMD_Number_C != 0)
            {
                if (Model_Overwrite_C.IsChecked == true && MMD_Model_List.SelectedIndex == -1 && MMD_Model_List.Items.Count != 0)
                {
                    MessageBox.Show("モデルを上書きする場合、上書きしたいモデルをリストから選択する必要があります。");
                    return;
                }
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog
                {
                    Title = "MMDモデルを参照",
                    Multiselect = true,
                    Filter = "MMDモデルファイル (*.pmx;*.pmd;*.x)|*.pmx;*.pmd;*.x"
                };
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        foreach (string File in ofd.FileNames)
                        {
                            if (Path + @"\Resources\Chara\" + (MMD_Model_List.SelectedIndex + 1) + @"\Model" + System.IO.Path.GetExtension(File) == File)
                            {
                                MessageBox.Show("コピー元とコピー先が同じです。このファイルはスキップされます。\nコピー元:" + File);
                                continue;
                            }
                            if ((MMD_Model_List.Items.Count + 1) > MMD_Number_C && Model_Overwrite_C.IsChecked == false)
                            {
                                MessageBox.Show("これ以上モデルを入れることができません。");
                                break;
                            }
                            else
                            {
                                string Directory_Name = File.Replace(System.IO.Path.GetFileName(File), "");
                                int Number = 1;
                                if (Model_Overwrite_C.IsChecked == true)
                                {
                                    int SelectedIndex = MMD_Model_List.SelectedIndex;
                                    if (System.IO.File.Exists(Path + "/Resources/Chara/" + (SelectedIndex + 1) + "/Model000.vmd"))
                                    {
                                        System.IO.File.Copy(Path + "/Resources/Chara/" + (SelectedIndex + 1) + "/Model000.vmd", Path + "/Resources/Chara/Model000.vmd", true);
                                    }
                                    await Task.Delay(10);
                                    if (Directory.Exists(Path + "/Resources/Chara/" + (SelectedIndex + 1)))
                                    {
                                        Directory.Delete(Path + "/Resources/Chara/" + (SelectedIndex + 1), true);
                                    }
                                    string[] Files = MMD_Model_All.Split('\n');
                                    string aaa = MMD_Model_List.Items[SelectedIndex].ToString();
                                    string Temp = MMD_Model_List.Items[SelectedIndex].ToString().Substring(aaa.LastIndexOf('/'));
                                    Files[SelectedIndex] = Path + "/Resources/Chara/" + (SelectedIndex + 1) + "/Model" + System.IO.Path.GetExtension(File) + "\n";
                                    MMD_Model_All = "";
                                    foreach (string File111 in Files)
                                    {
                                        MMD_Model_All += File111;
                                    }
                                    MMD_Model_List.Items[SelectedIndex] = (SelectedIndex + 1) + ":" + System.IO.Path.GetFileName(File) + "   " + Temp;
                                    Directory.CreateDirectory(Path + "/Resources/Chara/" + (SelectedIndex + 1));
                                    if (System.IO.File.Exists(Path + "/Resources/Chara/Model000.vmd"))
                                    {
                                        System.IO.File.Copy(Path + "/Resources/Chara/Model000.vmd", Path + "/Resources/Chara/" + (SelectedIndex + 1) + "/Model000.vmd", true);
                                        System.IO.File.Delete(Path + "/Resources/Chara/Model000.vmd");
                                    }
                                }
                                else
                                {
                                    while (true)
                                    {
                                        if (!Directory.Exists(Path + "/Resources/Chara/" + Number))
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            Number++;
                                        }
                                    }
                                    MMD_Model_List.Items.Add(Number + ":" + System.IO.Path.GetFileName(File));
                                    MMD_Model_All += Path + "/Resources/Chara/" + MMD_Model_List.Items.Count + "/Model" + System.IO.Path.GetExtension(File) + "\n";
                                }
                                string[] a = Directory.GetFiles(Directory_Name, "*", SearchOption.AllDirectories);
                                foreach (string Files in a)
                                {
                                    string File_Path = Files.Replace(Directory_Name, "");
                                    string File_Path_01 = File_Path.Replace(System.IO.Path.GetFileName(Files), "");
                                    if (!Directory.Exists(Path + "/Resources/Chara/" + Number + "/" + File_Path_01))
                                    {
                                        Directory.CreateDirectory(Path + "/Resources/Chara/" + Number + "/" + File_Path_01);
                                    }
                                    if (File_Path_01 == "")
                                    {
                                        System.IO.File.Copy(Files, Path + "/Resources/Chara/" + Number + "/" + System.IO.Path.GetFileName(Files), true);
                                    }
                                    else
                                    {
                                        System.IO.File.Copy(Files, Path + "/Resources/Chara/" + Number + "/" + File_Path_01 + System.IO.Path.GetFileName(Files), true);
                                    }
                                }
                                System.IO.File.Copy(File, Path + "/Resources/Chara/" + Number + "/Model" + System.IO.Path.GetExtension(File), true);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("エラーが発生しました。もう一度お試しください。");
                    }
                }
            }
            else
            {
                MessageBox.Show("先にMMDの人数を指定してください。");
            }
        }
        private async void MMD_Motion_Select_Click(object sender, RoutedEventArgs e)
        {
            if (IsMV1Creating == true)
            {
                Creating_Error_Message();
                return;
            }
            if (MMD_Number_C == 0 || MMD_Model_List.Items.Count == 0 || MMD_Model_List.SelectedIndex == -1)
            {
                MessageBox.Show("モデルが選択されていません。");
            }
            else
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog
                {
                    Title = "モーションを参照",
                    Multiselect = false,
                    Filter = "モーションファイル (*.vmd)|*.vmd"
                };
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        if (ofd.FileName == Path + @"\Resources\Chara\" + (MMD_Model_List.SelectedIndex + 1) + @"\Model000.vmd")
                        {
                            MessageBox.Show("コピー元とコピー先が同じです。このファイルはスキップされます。\nコピー元:" + ofd.FileName);
                            return;
                        }
                        int Index = MMD_Model_List.SelectedIndex;
                        if (MMD_Model_List.Items[Index].ToString().Contains("/"))
                        {
                            string aaa = MMD_Model_List.Items[Index].ToString();
                            string Temp = MMD_Model_List.Items[Index].ToString().Substring(aaa.LastIndexOf("/") - 3);
                            MMD_Model_List.Items[Index] = aaa.Replace(Temp, "");
                        }
                        await Task.Delay(10);
                        MMD_Model_List.Items[Index] += "   /   " + System.IO.Path.GetFileName(ofd.FileName);
                        File.Copy(ofd.FileName, Path + "/Resources/Chara/" + (Index + 1) + "/Model000.vmd", true);
                    }
                    catch
                    {
                        MessageBox.Show("エラーが発生しました。もう一度お試しください。");
                    }
                }
            }
        }
        private async void Exit_B_Click(object sender, RoutedEventArgs e)
        {
            if (IsMV1Creating == true && Opacity >= 1)
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("処理が実行中です。終了しますか？", "確認", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (result == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            IsStarted = false;
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
        private async void Save_B_Click(object sender, RoutedEventArgs e)
        {
            if (IsMV1Creating == true)
            {
                Creating_Error_Message();
                return;
            }
            MessageBoxResult result = MessageBox.Show("実行しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                if (MMD_Model_List.Items.Count != MMD_Number_C)
                {
                    MessageBox.Show("モデル人数とモデルデータの数が合いません。");
                    return;
                }
                string[] File_L = MMD_Model_All.Split('\n');
                for (int Number_01 = 1; Number_01 <= MMD_Number_C; Number_01++)
                {
                    string 拡張子 = System.IO.Path.GetExtension(File_L[Number_01 - 1]);
                    if (File.Exists(Path + "/Resources/Chara/" + Number_01 + "/Model" + 拡張子) || File.Exists(Path + "/Resources/Chara/" + Number_01 + "/Model.pmd"))
                    {
                        continue;
                    }
                    else
                    {
                        MessageBox.Show(Number_01 + "番目のモデルが存在しません。削除された可能性があるため続行できません。\n一度クリアしてやり直すことをお勧めします。");
                        return;
                    }
                }
                if (Music_Path == "")
                {
                    System.Windows.Forms.DialogResult result2 = System.Windows.Forms.MessageBox.Show("曲が選択されていません。実行しますか？", "確認", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                    if (result2 == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }
                int Number = 0;
                bool Number_OK = true;
                Progress_B.Visibility = Visibility.Visible;
                Progress_T.Visibility = Visibility.Visible;
                Progress_B.Opacity = 1;
                Progress_T.Opacity = 1;
                Progress_T.Text = "1つ目の項目を処理しています...";
                IsMV1Creating = true;
                string MMD_L = "";
                if (File.Exists(Path + "/Resources/Chara/1/Model.mv1"))
                {
                    File.Delete(Path + "/Resources/Chara/1/Model.mv1");
                }
                if (File.Exists(Path + "/Resources/Chara/2/Model.mv1"))
                {
                    File.Delete(Path + "/Resources/Chara/2/Model.mv1");
                }
                if (File.Exists(Path + "/Resources/Chara/3/Model.mv1"))
                {
                    File.Delete(Path + "/Resources/Chara/3/Model.mv1");
                }
                if (File.Exists(Path + "/Resources/Chara/4/Model.mv1"))
                {
                    File.Delete(Path + "/Resources/Chara/4/Model.mv1");
                }
                if (File.Exists(Path + "/Resources/Chara/5/Model.mv1"))
                {
                    File.Delete(Path + "/Resources/Chara/5/Model.mv1");
                }
                DX.MV1SetLoadModel_PMD_PMX_AnimationFPSMode(120);
                while (Number + 1 <= MMD_Number_C)
                {
                    if (File.Exists(Path + "/Resources/Chara/" + (Number + 1) + "/Model.mv1"))
                    {
                        if (Number_OK == false)
                        {
                            Number++;
                            Progress_T.Text = (Number + 1) + "つ目の項目を処理しています...";
                            Number_OK = true;
                        }
                    }
                    else if (Number_OK == true)
                    {
                        Number_OK = false;
                        Task task = Task.Run(() =>
                        {
                            int ModelHandle = DX.MV1LoadModel(File_L[Number]);
                            DX.MV1SaveModelToMV1File(ModelHandle, Path + "/Resources/Chara/" + (Number + 1) + "/Model.mv1", DX.MV1_SAVETYPE_NORMAL, 0, DX.FALSE, 0, 0, 0, 0);
                            DX.MV1DeleteModel(ModelHandle);
                            MMD_L += File_L[Number] + "\n";
                        });
                    }
                    await Task.Delay(100);
                }
                if (Music_Path != "")
                {
                    if (Music_Path != Path + @"\Resources\Music\UserMusic" + System.IO.Path.GetExtension(Music_Path))
                    {
                        File.Copy(Music_Path, Path + "/Resources/Music/UserMusic" + System.IO.Path.GetExtension(Music_Path), true);
                    }
                }
                StreamWriter s = File.CreateText(Path + "/Load_Data.dat");
                s.Close();
                Save obj = new Save
                {
                    MMD_Number_C = MMD_Number_C,
                    MMD_Model_List = MMD_L,
                    Music_Path = Music_Path
                };
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Save));
                StreamWriter sw = new StreamWriter(Path + "/Load_Data.dat", false, new System.Text.UTF8Encoding(false));
                serializer.Serialize(sw, obj);
                sw.Close();
                Progress_T.Text = "処理が完了しました。";
                await Task.Delay(1000);
                while (true)
                {
                    if (Progress_T.Opacity <= 0)
                    {
                        Progress_T.Text = "";
                        Progress_B.Visibility = Visibility.Hidden;
                        Progress_T.Visibility = Visibility.Hidden;
                        break;
                    }
                    Progress_B.Opacity -= 0.01;
                    Progress_T.Opacity -= 0.01;
                    await Task.Delay(30);
                }
                IsMV1Creating = false;
            }
        }
        private void Clear_B_Click(object sender, RoutedEventArgs e)
        {
            if (IsMV1Creating == true)
            {
                Creating_Error_Message();
                return;
            }
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("ユーザーデータをクリアしますか？\nこの操作では既に作成されたモデルも削除されます。", "確認", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                MMD_Model_List.Items.Clear();
                MMD_Number.Text = "0";
                MMD_Number_C = 0;
                Music_Path = "";
                Music_T.Text = "選択された曲:未選択";
                MMD_Model_All = "";
                int Number = 1;
                File.Delete(Path + "/Data.dat");
                File.Delete(Path + "/Load_Data.dat");
                while (true)
                {
                    if (Directory.Exists(Path + "/Resources/Chara/" + Number))
                    {
                        Directory.Delete(Path + "/Resources/Chara/" + Number, true);
                    }
                    else
                    {
                        break;
                    }
                    Number++;
                }
            }
        }
        private void Music_B_Click(object sender, RoutedEventArgs e)
        {
            if (IsMV1Creating == true)
            {
                Creating_Error_Message();
                return;
            }
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog
            {
                Title = "曲を選ぶドン！",
                Multiselect = false,
                Filter = "再生ファイル (*.mp3;*.wav;*.ogg)|*.mp3;*.wav;*.ogg"
            };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Music_Path = ofd.FileName;
                Music_T.Text = "選択された曲:" + System.IO.Path.GetFileName(ofd.FileName);
            }
        }
        void Creating_Error_Message()
        {
            MessageBox.Show("処理が実行中です。");
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MMD_Model_List.Items.Count != 0)
            {
                int Number = 0;
                while (true)
                {
                    if (Number == MMD_Model_List.Items.Count)
                    {
                        StreamWriter s = File.CreateText(Path + "/Data.dat");
                        s.Close();
                        Save_This_Window_List obj = new Save_This_Window_List
                        {
                            Lists = MMD_Model_Motion_Not_MV1_List,
                            MMD_Number = MMD_Number_C,
                            Music_Path = Music_Path
                        };
                        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Save_This_Window_List));
                        StreamWriter sw = new StreamWriter(Path + "/Data.dat", false, new System.Text.UTF8Encoding(false));
                        serializer.Serialize(sw, obj);
                        sw.Close();
                        break;
                    }
                    else
                    {
                        MMD_Model_Motion_Not_MV1_List += MMD_Model_List.Items[Number] + "\n";
                        Number++;
                    }
                }
            }
        }
        private async void Window_2_B_Click(object sender, RoutedEventArgs e)
        {
            if (IsMV1Creating == true && Opacity >= 1)
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("処理が実行中です。画面を移動しますか？", "確認", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button2);
                if (result == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            if (IsSetting_Mode_Showing == false)
            {
                IsSetting_Mode_Showing = true;
                IsStarted = false;
                while (true)
                {
                    if (Opacity <= 0)
                    {
                        Setting_Mode f = new Setting_Mode();
                        f.Show();
                        Close();
                        break;
                    }
                    Opacity -= 0.05;
                    await Task.Delay(30);
                }
            }
        }
        private void MMD_Camera_Select_B_Click(object sender, RoutedEventArgs e)
        {
        start:
            if (IsMV1Creating == true)
            {
                Creating_Error_Message();
                return;
            }
            if (File.Exists(Path + "/Resources/UserCamera1.vmd") && File.Exists(Path + "/Resources/UserCamera1.vmd") && File.Exists(Path + "/Resources/UserCamera1.vmd"))
            {
                MessageBoxResult result = MessageBox.Show("カメラモーションは最大3つまでです。登録されているカメラモーションをクリアしますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    File.Delete(Path + "/Resources/UserCamera1.vmd");
                    File.Delete(Path + "/Resources/UserCamera2.vmd");
                    File.Delete(Path + "/Resources/UserCamera3.vmd");
                    MessageBox.Show("クリアしました。");
                    goto start;
                }
            }
            else
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog
                {
                    Multiselect = true,
                    Title = "カメラのモーションファイルを選択してください。",
                    Filter = "MMDモーションファイル (*.vmd)|*.vmd"
                };
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (string Files in ofd.FileNames)
                    {
                        int Number = 1;
                        if (File.Exists(Path + "/Resources/UserCamera1.vmd"))
                        {
                            Number++;
                        }
                        if (File.Exists(Path + "/Resources/UserCamera2.vmd"))
                        {
                            Number++;
                        }
                        if (Number == 3 && File.Exists(Path + "/Resources/UserCamera3.vmd"))
                        {
                            MessageBox.Show("モーションファイルが既に3つあります。これ以上追加できません。");
                            return;
                        }
                        else
                        {
                            File.Copy(Files, Path + "/Resources/UserCamera" + Number + ".vmd", true);
                        }
                    }
                }
            }
        }
        private void MMD_Camera_Clear_B_Click(object sender, RoutedEventArgs e)
        {
            if (IsMV1Creating == true)
            {
                Creating_Error_Message();
                return;
            }
            bool IsDelected = false;
            if (File.Exists(Path + "/Resources/UserCamera1.vmd"))
            {
                IsDelected = true;
            }
            else if (File.Exists(Path + "/Resources/UserCamera2.vmd"))
            {
                IsDelected = true;
            }
            else if (File.Exists(Path + "/Resources/UserCamera3.vmd"))
            {
                IsDelected = true;
            }
            File.Delete(Path + "/Resources/UserCamera1.vmd");
            File.Delete(Path + "/Resources/UserCamera2.vmd");
            File.Delete(Path + "/Resources/UserCamera3.vmd");
            if (IsDelected)
            {
                MessageBox.Show("クリアしました。");
            }
            else
            {
                MessageBox.Show("カメラが既に存在しません。");
            }
        }
    }
}
public class Save
{
    public int MMD_Number_C;
    public string MMD_Model_List;
    public string Music_Path;
}
public class Save_This_Window_List
{
    public string Lists;
    public int MMD_Number;
    public string Music_Path;
}