using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Xml.Linq;
using DxLibDLL;
using System.Threading.Tasks;
using System;

namespace MMD_Model_Viewer
{
    public partial class MMD_Model_Viewer : Form
    {
        //念のためクリックされるまで常に最前面に表示される
        private void MMD_Model_Viewer_MouseClick(object sender, MouseEventArgs e)
        {
            TopMost = false;
        }
        public MMD_Model_Viewer()
        {
            InitializeComponent();
            //起動時の処理
            DX.SetAlwaysRunFlag(DX.TRUE);
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);
            DX.SetUseFPUPreserveFlag(DX.TRUE);
            DX.SetWaitVSyncFlag(DX.FALSE);
            DX.SetOutApplicationLogValidFlag(DX.FALSE);
            DX.SetDoubleStartValidFlag(DX.TRUE);
            DX.SetUseDXArchiveFlag(DX.TRUE);
            DX.SetUserWindow(Handle);
            DX.SetWindowVisibleFlag(DX.TRUE);
            DX.SetZBufferBitDepth(24);
            DX.SetCreateDrawValidGraphZBufferBitDepth(24);
            DX.SetMouseDispFlag(DX.FALSE);
            if (DX.DxLib_Init() == -1)
            {
                return;
            }
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);
            //現在時刻によってロード中の色を変更(いらない...?)
            int Hour = DateTime.Now.Hour;
            if (Hour >= 6 && Hour <= 18)
            {
                BackColor = Color.White;
                Load_T.BackColor = Color.White;
                Load_T.ForeColor = Color.Black;
            }
            else
            {
                BackColor = Color.Black;
                Load_T.BackColor = Color.Black;
                Load_T.ForeColor = Color.White;
            }
            //フルスクリーン
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
            Load_T.Location = new Point((int)(w / 3.32), (int)(h / 2.03));
            Cursor.Hide();
        }
        async void Window_Show()
        {
            Opacity = 0;
            while (Opacity < 1 && !IsClosing)
            {
                Opacity += 0.05;
                await Task.Delay(1000 / 60);
            }
            Model_Load();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //終了
            DX.DxLib_End();
        }
        private async void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                //Escキーが押されたら終了ダイアログを表示
                TopMost = false;
                IsShowMessageBox = true;
                Screen s = Screen.FromControl(this);
                int h = s.Bounds.Height;
                int w = s.Bounds.Width;
                Cursor.Position = new Point(w / 2, h / 2);
                Cursor.Show();
                Focus();
                Mouse_Show();
                DialogResult result = MessageBox.Show("終了しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    IsClosing = true;
                    Opacity = 1;
                    int Down_Speed = Music_Volume / 18;
                    while (true)
                    {
                        Music_Volume -= Down_Speed;
                        Opacity -= 0.05;
                        if (Opacity <= 0)
                        {
                            IsClosed = true;
                            Application.Exit();
                            Close();
                            break;
                        }
                        await Task.Delay(1000 / 60);
                    }
                }
                else
                {
                    IsShowMessageBox = false;
                }
            }
        }
        async void Mouse_Show()
        {
            while (IsShowMessageBox)
            {
                Cursor.Show();
                await Task.Delay(100);
            }
        }
        void Model_Position_Change_By_SettingFile(float Distance)
        {
            //モデル間隔をセット
            Model_Distance = Distance;
            if (Model_Number == 1)
            {
                Model_01_Position_X = 0f;
            }
            else if (Model_Number == 2)
            {
                Model_01_Position_X = -Distance / 2;
                Model_02_Position_X = Distance / 2;
            }
            else if (Model_Number == 3)
            {
                Model_01_Position_X = -Distance;
                Model_02_Position_X = 0f;
                Model_03_Position_X = Distance;
            }
            else if (Model_Number == 4)
            {
                Model_01_Position_X = -Distance - Distance / 2;
                Model_02_Position_X = -Distance / 2;
                Model_03_Position_X = Distance / 2;
                Model_04_Position_X = Distance + Distance / 2;
            }
            else if (Model_05_Position_X == 5)
            {
                Model_01_Position_X = -Distance - Distance;
                Model_02_Position_X = -Distance;
                Model_03_Position_X = 0f;
                Model_04_Position_X = Distance;
                Model_05_Position_X = Distance + Distance;
            }
        }
        void Model_Load()
        {
            float Shadow_Size = 500f;
            float Shadow_Height = 250f;
            DX.VECTOR Shadow_Angle = DX.VGet(-0.5f, -0.75f, 0.5f);
            Task task = Task.Run(() =>
            {
                DX.ChangeLightTypeDir(DX.VGet(0f, 0.0f, 2.0f));
                DX.MV1SetLoadModel_PMD_PMX_AnimationFPSMode(120);
                if (!File.Exists(Path + "/Resources/Setting.dat"))
                {
                    MessageBox.Show("設定ファイルが存在しませんでした。\n付属のソフトで作成してください。");
                    return;
                }
                //別ソフトで指定した設定を反映
                XDocument Setting_01 = XDocument.Load(Path + "/Resources/Setting.dat");
                XElement Setting_02 = Setting_01.Element("Setting_Save");
                if (!File.Exists(Path + "/Load_Data.dat"))
                {
                    ModelHandle_00 = DX.MV1LoadModel(Path + "/Resources/Chara/IA/Model.mv1");
                    ModelHandle_01 = DX.MV1LoadModel(Path + "/Resources/Chara/Test_01/Model.mv1");
                    ModelHandle_02 = DX.MV1LoadModel(Path + "/Resources/Chara/Alice/Model.mv1");
                    MusicHandle = DX.LoadSoundMem(Path + "/Resources/Music/Default.mp3");
                    Player.URL = Path + "/Resources/Music/Default.mp3";
                    First_Music_Pitch = DX.GetFrequencySoundMem(MusicHandle);
                    Model_Number = 3;
                    //カメラのモーションがあれば反映
                    if (File.Exists(Path + "/Resources/Camera.vmd"))
                    {
                        CameraHandle1 = DX.MV1LoadModel(Path + "/Resources/Camera.vmd");
                        Camera_Number_First++;
                    }
                    if (File.Exists(Path + "/Resources/Camera1.vmd"))
                    {
                        CameraHandle2 = DX.MV1LoadModel(Path + "/Resources/Camera1.vmd");
                        Camera_Number_First++;
                    }
                    if (File.Exists(Path + "/Resources/Camera2.vmd"))
                    {
                        CameraHandle3 = DX.MV1LoadModel(Path + "/Resources/Camera2.vmd");
                        Camera_Number_First++;
                    }
                }
                else
                {
                    try
                    {
                        //別ソフトで指定したモデルやカメラをロード
                        XDocument item_01 = XDocument.Load(Path + "/Load_Data.dat");
                        XElement item_02 = item_01.Element("Save");
                        int Number = int.Parse(item_02.Element("MMD_Number_C").Value);
                        Model_Number = int.Parse(item_02.Element("MMD_Number_C").Value);
                        //カメラのモーションがあれば反映
                        if (File.Exists(Path + "/Resources/UserCamera1.vmd"))
                        {
                            CameraHandle1 = DX.MV1LoadModel(Path + "/Resources/UserCamera1.vmd");
                            Camera_Number_First++;
                        }
                        if (File.Exists(Path + "/Resources/UserCamera2.vmd"))
                        {
                            CameraHandle2 = DX.MV1LoadModel(Path + "/Resources/UserCamera2.vmd");
                            Camera_Number_First++;
                        }
                        if (File.Exists(Path + "/Resources/UserCamera3.vmd"))
                        {
                            CameraHandle3 = DX.MV1LoadModel(Path + "/Resources/UserCamera3.vmd");
                            Camera_Number_First++;
                        }
                        int ZERO = 1;
                        while (true)
                        {
                            if (ZERO == 1)
                            {
                                ModelHandle_00 = DX.MV1LoadModel(Path + "/Resources/Chara/1/Model.mv1");
                            }
                            else if (ZERO == 2)
                            {
                                ModelHandle_01 = DX.MV1LoadModel(Path + "/Resources/Chara/2/Model.mv1");
                            }
                            else if (ZERO == 3)
                            {
                                ModelHandle_02 = DX.MV1LoadModel(Path + "/Resources/Chara/3/Model.mv1");
                            }
                            else if (ZERO == 4)
                            {
                                ModelHandle_03 = DX.MV1LoadModel(Path + "/Resources/Chara/4/Model.mv1");
                            }
                            else if (ZERO == 5)
                            {
                                ModelHandle_04 = DX.MV1LoadModel(Path + "/Resources/Chara/5/Model.mv1");
                            }
                            if (ZERO >= Number)
                            {
                                break;
                            }
                            ZERO++;
                        }
                        if (item_02.Element("Music_Path").Value != "")
                        {
                            //曲を指定
                            MusicHandle = DX.LoadSoundMem(Path + "/Resources/Music/UserMusic" + System.IO.Path.GetExtension(item_02.Element("Music_Path").Value));
                            Player.URL = Path + "/Resources/Music/UserMusic" + System.IO.Path.GetExtension(item_02.Element("Music_Path").Value);
                            First_Music_Pitch = DX.GetFrequencySoundMem(MusicHandle);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("ユーザーデータが破損しています。もう一度作り直す必要があります。");
                        return;
                    }
                }
                try
                {
                    //設定ファイルから情報に応じて設定
                    //マップ
                    if (Setting_02.Element("Map_Select").Value == "0")
                    {
                        //町
                        MapHandle = DX.MV1LoadModel(Path + "/Resources/Map/Stage/サン・マルコ広場_Ver1.00.mv1");
                        SkyHandle = DX.MV1LoadModel(Path + "/Resources/Sky/Dome_X503.mv1");
                        OceanHandle = DX.MV1LoadModel(Path + "/Resources/Map/Ocean/Ocean.mv1");
                        DX.MV1SetScale(MapHandle, DX.VGet(10f, 10f, 10f));
                        DX.MV1SetScale(OceanHandle, DX.VGet(10f, 10f, 10f));
                        DX.MV1SetScale(SkyHandle, DX.VGet(10f, 10f, 10f));
                        DX.MV1SetPosition(MapHandle, DX.VGet(-20f, 0f, -300f));
                        DX.MV1SetPosition(OceanHandle, DX.VGet(-20f, -35f, -100f));
                        DX.MV1SetRotationXYZ(MapHandle, DX.VGet(0f, 0.15f, 0f));
                    }
                    else if (Setting_02.Element("Map_Select").Value == "1")
                    {
                        //宇宙
                        MapHandle = DX.MV1LoadModel(Path + "/Resources/Map/Space/Model.mv1");
                        SkyHandle = DX.MV1LoadModel(Path + "/Resources/Sky/Dome_X503.mv1");
                        DX.MV1SetScale(MapHandle, DX.VGet(1f, 1f, 1f));
                        DX.MV1SetScale(SkyHandle, DX.VGet(10f, 10f, 10f));
                        DX.MV1SetPosition(MapHandle, DX.VGet(0f, 0.43f, 40f));
                        DX.MV1SetRotationXYZ(MapHandle, DX.VGet(0f, 0f, 0f));
                        Shadow_Size = 100f;
                        Shadow_Height = 50f;
                    }
                    else if (Setting_02.Element("Map_Select").Value == "2")
                    {
                        //ユーザーマップ
                        XDocument item_01 = XDocument.Load(Path + "/Resources/Map_Setting.dat");
                        XElement item_02 = item_01.Element("Map_Setting");
                        MapHandle = DX.MV1LoadModel(Path + "/Resources/Map/UserMap/Model.mv1");
                        float Size = float.Parse(item_02.Element("Map_Scale").Value);
                        float Map_X = float.Parse(item_02.Element("Map_X").Value);
                        float Map_Y = float.Parse(item_02.Element("Map_Y").Value);
                        float Map_Z = float.Parse(item_02.Element("Map_Z").Value);
                        int Map_Rotate_Y = int.Parse(item_02.Element("Map_Rotate").Value);
                        if (bool.Parse(item_02.Element("Sky_Enable").Value))
                        {
                            SkyHandle = DX.MV1LoadModel(Path + "/Resources/Sky/Dome_X503.mv1");
                        }
                        DX.MV1SetScale(MapHandle, DX.VGet(Size, Size, Size));
                        DX.MV1SetPosition(MapHandle, DX.VGet(Map_X, Map_Y, Map_Z));
                        DX.MV1SetRotationXYZ(MapHandle, DX.VGet(0f, Map_Rotate_Y * 3.141592f / 180f, 0f));
                    }
                    //モデル同士の間隔が0でなければロード
                    if (Setting_02.Element("Model_Position").Value != "0")
                    {
                        IsModelPositionDistance = true;
                        Model_Position_Change_By_SettingFile(float.Parse(Setting_02.Element("Model_Position").Value));
                    }
                    else
                    {
                        IsModelPositionDistance = false;
                    }
                    //ライト　　　0が有効で1が無効
                    if (Setting_02.Element("Light_Select").Value == "0")
                    {
                        DX.SetLightEnable(DX.TRUE);
                        ShaderHandle = DX.LoadGraph(Path + "/Resources/Effects/Shader.png");
                        if (Setting_02.Element("Map_Select").Value == "1")
                        {
                            DX.ChangeLightTypePoint(DX.VGet(-500f, 850f, -100f), 10000f, 0.01f, 0.01f, 0.01f);
                            DX.CreatePointLightHandle(DX.VGet(0f, 100f, 500f), 1500f, 0f, 0.00125f, 0f);
                            DX.CreatePointLightHandle(DX.VGet(0f, 100f, -500f), 1500f, 0f, 0.00125f, 0f);
                        }
                        else
                        {
                            DX.ChangeLightTypePoint(DX.VGet(-500f, 850f, -100f), 10000f, 0.01f, 0.01f, 0.01f);
                            DX.CreatePointLightHandle(DX.VGet(0f, 1000f, 500f), 1500f, 0f, 0.00125f, 0f);
                            DX.CreatePointLightHandle(DX.VGet(0f, 1000f, -500f), 1500f, 0f, 0.00125f, 0f);
                        }
                        DX.SetLightAngle(1f, -1f);
                    }
                    else if (Setting_02.Element("Light_Select").Value == "1")
                    {
                        //ライトが無効の場合かつホラーモードが有効か
                        DX.SetLightEnable(DX.FALSE);
                        if (Setting_02.Element("Horror_Select").Value == "0")
                        {
                            DX.CreatePointLightHandle(DX.VGet(0f, 1000f, 0f), 500f, 0f, 0.00525f, 0f);
                        }
                        else if (Setting_02.Element("Map_Select").Value == "0" || Setting_02.Element("Map_Select").Value == "1")
                        {
                            SkyHandle = DX.MV1LoadModel(Path + "/Resources/Sky/Moon.mv1");
                            IsHorror_Mode_And_Map_0 = true;
                        }
                        else if (Setting_02.Element("Map_Select").Value == "2")
                        {
                            if (Setting_02.Element("Horror_Sky_Select").Value == "0")
                            {
                                SkyHandle = DX.MV1LoadModel(Path + "/Resources/Sky/Moon.mv1");
                                IsHorror_Mode_And_Map_0 = true;
                            }
                        }
                    }
                    if (Setting_02.Element("Music_Mode").Value == "0")
                    {
                        IsWMPUse = false;
                    }
                    else
                    {
                        IsWMPUse = true;
                    }
                    IsShadowMode = bool.Parse(Setting_02.Element("Shadow_Mode").Value);
                    if (IsShadowMode)
                    {
                        if (int.Parse(Setting_02.Element("Shadow_Angle").Value) == 0)
                        {
                            Shadow_Angle = DX.VGet(-0.5f, -0.75f, 0.5f);
                        }
                        else if (int.Parse(Setting_02.Element("Shadow_Angle").Value) == 1)
                        {
                            Shadow_Angle = DX.VGet(0.5f, -0.75f, 0.5f);
                        }
                        else if (int.Parse(Setting_02.Element("Shadow_Angle").Value) == 2)
                        {
                            Shadow_Angle = DX.VGet(-0.5f, -0.75f, -0.5f);
                        }
                        else if (int.Parse(Setting_02.Element("Shadow_Angle").Value) == 3)
                        {
                            Shadow_Angle = DX.VGet(0.5f, -0.75f, -0.5f);
                        }
                    }
                    //ピッチや音量をセット
                    First_Music_Pitch = DX.GetFrequencySoundMem(MusicHandle);
                    Music_Volume = int.Parse(Setting_02.Element("Volume").Value);
                    Pitch_Set = double.Parse(Setting_02.Element("Pitch").Value);
                    PlayTime_Plus_Temp_Kakeru = float.Parse(Setting_02.Element("Pitch").Value);
                    Pan = int.Parse(Setting_02.Element("Pan").Value);
                    DX.ChangePanSoundMem(Pan, MusicHandle);
                    Pitch_Set_Void();
                }
                catch
                {
                    File.Delete(Path + "/Resources/Setting.dat");
                    MessageBox.Show("設定ファイルが破損しているので削除しました。。\n付属のソフトで作成してください。\nソフトは強制終了されます。");
                    return;
                }
                if (File.Exists(Path + "/Resources/Advance_Setting.dat"))
                {
                    //詳細設定のファイルがあればロード
                    try
                    {
                        //マウス感度、ピッチの幅、モデル1～5の位置、FPSなど
                        XDocument Setting_03 = XDocument.Load(Path + "/Resources/Advance_Setting.dat");
                        XElement Setting_04 = Setting_03.Element("Advance_Save");
                        Mouse_Sensitivity = int.Parse(Setting_04.Element("Mouse_Sensitivity").Value);
                        Pitch_Change_Size = double.Parse(Setting_04.Element("Pitch_Interval").Value);
                        IsModelPositionSet = bool.Parse(Setting_04.Element("IsModelPositionSet").Value);
                        if (IsModelPositionSet)
                        {
                            Model_Position_01 = DX.VGet(float.Parse(Setting_04.Element("Model_01_X").Value), float.Parse(Setting_04.Element("Model_01_Y").Value), float.Parse(Setting_04.Element("Model_01_Z").Value));
                            Model_Position_02 = DX.VGet(float.Parse(Setting_04.Element("Model_02_X").Value), float.Parse(Setting_04.Element("Model_02_Y").Value), float.Parse(Setting_04.Element("Model_02_Z").Value));
                            Model_Position_03 = DX.VGet(float.Parse(Setting_04.Element("Model_03_X").Value), float.Parse(Setting_04.Element("Model_03_Y").Value), float.Parse(Setting_04.Element("Model_03_Z").Value));
                            Model_Position_04 = DX.VGet(float.Parse(Setting_04.Element("Model_04_X").Value), float.Parse(Setting_04.Element("Model_04_Y").Value), float.Parse(Setting_04.Element("Model_04_Z").Value));
                            Model_Position_05 = DX.VGet(float.Parse(Setting_04.Element("Model_05_X").Value), float.Parse(Setting_04.Element("Model_05_Y").Value), float.Parse(Setting_04.Element("Model_05_Z").Value));
                        }
                        Pitch_Min = double.Parse(Setting_04.Element("Pitch_Min").Value);
                        Pitch_Max = double.Parse(Setting_04.Element("Pitch_Max").Value);
                        IsFrameRateLock = bool.Parse(Setting_04.Element("FrameRateLock").Value);
                        IsMusicNotChange = bool.Parse(Setting_04.Element("IsMusicNotChange").Value);
                        IsPhysicsSet = bool.Parse(Setting_04.Element("IsPhysicsSet").Value);
                        IsModelPhysicsSet = bool.Parse(Setting_04.Element("IsModelPhysicsSet").Value);
                        int FPS_01 = int.Parse(Setting_04.Element("FPS").Value);
                        //FPSの値によってモーションの速度が異なるため指定する
                        if (FPS_01 == 0)
                        {
                            PlayTime_Plus = 1f;
                            PlayTime_Plus_Temp = 1f;
                            FPS = 30;
                            FPS_Kakeru = 1.4f;
                        }
                        else if (FPS_01 == 1)
                        {
                            PlayTime_Plus = 0.5f;
                            PlayTime_Plus_Temp = 0.5f;
                            FPS = 60;
                            FPS_Kakeru = 0.7f;
                        }
                        else if (FPS_01 == 2)
                        {
                            PlayTime_Plus = 0.25f;
                            PlayTime_Plus_Temp = 0.25f;
                            FPS = 120;
                            FPS_Kakeru = 0.35f;
                        }
                    }
                    catch
                    {
                        File.Delete(Path + "/Resources/Advance_Setting.dat");
                        MessageBox.Show("詳細設定のファイルが破損しているので削除しました、もう一度作成する必要があります。\nソフトは強制終了されます。");
                        return;
                    }
                }
                if (Model_Number >= 1)
                {
                    Model_Physics_00 = DX.MV1LoadModel(Path + "/Resources/Map/Coll/Model.mv1");
                }
                if (Model_Number >= 2)
                {
                    Model_Physics_01 = DX.MV1LoadModel(Path + "/Resources/Map/Coll/Model.mv1");
                }
                if (Model_Number >= 3)
                {
                    Model_Physics_02 = DX.MV1LoadModel(Path + "/Resources/Map/Coll/Model.mv1");
                }
                if (Model_Number >= 4)
                {
                    Model_Physics_03 = DX.MV1LoadModel(Path + "/Resources/Map/Coll/Model.mv1");
                }
                if (Model_Number >= 5)
                {
                    Model_Physics_04 = DX.MV1LoadModel(Path + "/Resources/Map/Coll/Model.mv1");
                }
                Model_Shadow_00 = DX.MakeShadowMap(8192, 8192);
                Map_Shadow_00 = DX.MakeShadowMap(16384, 16384);
                DX.SetShadowMapLightDirection(Model_Shadow_00, Shadow_Angle);
                DX.SetShadowMapLightDirection(Map_Shadow_00, Shadow_Angle);
                DX.SetLightDirection(Shadow_Angle);
                DX.SetShadowMapDrawArea(Model_Shadow_00, DX.VGet(-30f, -1f, -30f), DX.VGet(30f, 30f, 30f));
                DX.SetShadowMapDrawArea(Map_Shadow_00, DX.VGet(-Shadow_Size, -1f, -Shadow_Size), DX.VGet(Shadow_Size, Shadow_Height, Shadow_Size));
                DX.ShadowMap_DrawSetup(Map_Shadow_00);
                DX.MV1DrawModel(MapHandle);
                DX.ShadowMap_DrawEnd();
                DX.MV1SetupCollInfo(MapHandle, -1);
                DX.MV1SetupCollInfo(Model_Physics_00, -1);
                DX.MV1SetupCollInfo(Model_Physics_01, -1);
                DX.MV1SetupCollInfo(Model_Physics_02, -1);
                DX.MV1SetupCollInfo(Model_Physics_03, -1);
                DX.MV1SetupCollInfo(Model_Physics_04, -1);
                //モデルの初期位置を設定
                DX.MV1SetScale(SkyHandle, DX.VGet(50f, 50f, 50f));
                DX.MV1SetRotationXYZ(SkyHandle, DX.VGet(0f, 0f, 0f));
                DX.MV1SetLoadModelUsePhysicsMode(DX.DX_LOADMODEL_PHYSICS_REALTIME);
                DX.MV1SetScale(ModelHandle_00, DX.VGet(1f, 1f, 1f));
                DX.MV1SetRotationXYZ(ModelHandle_00, DX.VGet(0f, 1.5f, 0f));
                DX.MV1SetScale(ModelHandle_01, DX.VGet(1f, 1f, 1f));
                DX.MV1SetRotationXYZ(ModelHandle_01, DX.VGet(0f, 1.5f, 0f));
                DX.MV1SetScale(ModelHandle_02, DX.VGet(1f, 1f, 1f));
                DX.MV1SetRotationXYZ(ModelHandle_02, DX.VGet(0f, 1.5f, 0f));
                DX.MV1SetScale(ModelHandle_03, DX.VGet(1f, 1f, 1f));
                DX.MV1SetRotationXYZ(ModelHandle_03, DX.VGet(0f, 1.5f, 0f));
                DX.MV1SetScale(ModelHandle_04, DX.VGet(1f, 1f, 1f));
                DX.MV1SetRotationXYZ(ModelHandle_04, DX.VGet(0f, 1.5f, 0f));
                DX.SetCameraNearFar(1f, 10000.0f);
                DX.SetCameraPositionAndTarget_UpVecY(DX.VGet(0.0f, 3f, -5.0f), DX.VGet(0f, 0f, 0f));
                if (IsWMPUse)
                {
                    Player.controls.play();
                    Player.settings.volume = Music_Volume / 255 * 100;
                }
                else
                {
                    Player.settings.volume = 100;
                    Player.controls.stop();
                    DX.PlaySoundMem(MusicHandle, DX.DX_PLAYTYPE_LOOP, DX.FALSE);
                    DX.ChangeVolumeSoundMem(Music_Volume, MusicHandle);
                }
                PlayTime = 10f;
            });
            task.Wait();
            Load_T.Visible = false;
            IsShowMessageBox = false;
            Dance_Loop();
            TopMost = true;
            Focus();
            Activate();
        }
        private void MMD_Model_Viewer_Load(object sender, System.EventArgs e)
        {
            Window_Show();
        }
    }
}