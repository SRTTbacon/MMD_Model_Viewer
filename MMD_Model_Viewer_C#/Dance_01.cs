using System.Windows.Forms;
using DxLibDLL;
using System;
using System.Drawing;
using System.IO;
using WMPLib;

namespace MMD_Model_Viewer
{
    public partial class MMD_Model_Viewer : Form
    {
        //定義
        readonly WindowsMediaPlayer Player = new WindowsMediaPlayer();
        readonly string Path = Directory.GetCurrentDirectory();
        int MoveAnimFrameIndex;
        int F_KEY_Tap = 0;
        int M_KEY_Tap = 0;
        int K_KEY_Tap = 0;
        int J_KEY_Tap = 0;
        int N_KEY_Tap = 0;
        int Music_Volume = 175;
        int G_Key_Down = 0;
        int M_R_Key_Down = 0;
        int Pan = 0;
        int Stop_Key = 0;
        int Play_Key = 0;
        int Camera_Number = 1;
        int Model_Number = 0;
        int Select_Chara = 0;
        int First_Music_Pitch = 44100;
        int Mouse_Sensitivity = 40;
        int ShaderHandle;
        int ModelHandle_00;
        int ModelHandle_01;
        int ModelHandle_02;
        int ModelHandle_03;
        int ModelHandle_04;
        int Model_Physics_00;
        int Model_Physics_01;
        int Model_Physics_02;
        int Model_Physics_03;
        int Model_Physics_04;
        int CameraHandle1;
        int CameraHandle2;
        int CameraHandle3;
        int MusicHandle;
        int SkyHandle;
        int MapHandle;
        int OceanHandle;
        int AttachIndex_00;
        int AttachIndex_01;
        int AttachIndex_02;
        int AttachIndex_03;
        int AttachIndex_04;
        int Camera_Number_First = 0;
        int FPS = 60;
        int Model_Shadow_00;
        int Map_Shadow_00;
        float FPS_Kakeru = 1f;
        float x = 0f;
        float y = 0f;
        float z = 0f;
        float angleY = 0f;
        float cameraX = 0f;
        float cameraY = 3f;
        float cameraZ = -5f;
        float targetX = 0f;
        float targetY = 0f;
        float targetZ = 0f;
        float Camera_Distance = 10f;
        float Camera_Side = 0.2f;
        float Camera_Tall = 0f;
        float Model_01_Position_X = 0f;
        float Model_02_Position_X = 0f;
        float Model_03_Position_X = 0f;
        float Model_04_Position_X = 0f;
        float Model_05_Position_X = 0f;
        float Model_Distance = 0f;
        float Chara_X_0 = 0f, Chara_Y_0 = 0f, Chara_Z_0 = 0f;
        float Stop_Play_Time;
        float Linear_Plus = 0f;
        float Linear_Not_VMD = 1f;
        float TotalTime;
        float PlayTime;
        float PlayTime_Plus = 0.75f;
        float PlayTime_Plus_Temp = 0.75f;
        float PlayTime_Plus_Temp_Kakeru = 1f;
        bool FPS_Camera = false;
        bool Music_Mute = false;
        bool Camera_VMD = true;
        bool Stop = false;
        bool IsHorror_Mode_And_Map_0 = false;
        bool IsModelPositionDistance = false;
        bool IsShowMessageBox = true;
        bool IsModelPositionSet = false;
        bool IsFrameRateLock = false;
        bool IsMusicNotChange = false;
        bool IsWMPUse = false;
        bool C_P_Enable = true;
        bool C_O_Enable = true;
        bool IsClosing = false;
        bool IsPhysicsSet = true;
        bool IsModelPhysicsSet = true;
        bool IsShadowMode = true;
        bool IsClosed = false;
        bool IsOpacityChange = false;
        bool IsOpacityChange_Down = false;
        bool IsOpacityChange_Up = false;
        double Pitch_Set = 1.0;
        double Pitch_Change_Size = 0.1;
        double Pitch_Min = 0.3;
        double Pitch_Max = 1.6;
        static Point p = new Point();
        readonly double w = Screen.GetBounds(p).Width;
        readonly double h = Screen.GetBounds(p).Height;
        long Stop_Time = 0;
        DX.VECTOR Model_Position_01;
        DX.VECTOR Model_Position_02;
        DX.VECTOR Model_Position_03;
        DX.VECTOR Model_Position_04;
        DX.VECTOR Model_Position_05;
        DX.VECTOR TempPosition1;
        DX.VECTOR TempPosition2;
        DX.VECTOR CameraPosition;
        DX.VECTOR CameraLookAtPosition;
        DX.VECTOR Camera_Location;
        void SetupVMDCameraMotionParam(int CameraHandle, float Time)
        {
            //MMDのカメラを反映
            DX.MATRIX VRotMat, HRotMat, MixRotMat, TwistRotMat;
            DX.VECTOR CamLoc, CamDir;
            DX.VECTOR Location, Rotation, CamUp;
            float Length, ViewAngle;
            Location = DX.MV1GetAnimKeyDataToVectorFromTime(CameraHandle, 0, Time);
            Rotation = DX.MV1GetAnimKeyDataToVectorFromTime(CameraHandle, 1, Time);
            Length = DX.MV1GetAnimKeyDataToLinearFromTime(CameraHandle, 2, Time);
            ViewAngle = DX.MV1GetAnimKeyDataToLinearFromTime(CameraHandle, 3, Time);
            VRotMat = DX.MGetRotX(-Rotation.x);
            HRotMat = DX.MGetRotY(-Rotation.y);
            MixRotMat = DX.MMult(VRotMat, HRotMat);
            CamDir = DX.VTransform(DX.VGet(0.0f, 0.0f, 1.0f), MixRotMat);
            TwistRotMat = DX.MGetRotAxis(CamDir, -Rotation.z);
            MixRotMat = DX.MMult(MixRotMat, TwistRotMat);
            CamLoc = DX.VTransform(DX.VGet(0.0f, 0.0f, Length), MixRotMat);
            CamLoc = DX.VAdd(CamLoc, Location);
            CamUp = DX.VTransform(DX.VGet(0.0f, 1.0f, 0.0f), MixRotMat);
            Location = DX.VAdd(CamLoc, CamDir);
            DX.SetupCamera_Perspective((ViewAngle / 180.0f * 3.141592f) + Linear_Plus);
            DX.SetCameraPositionAndTargetAndUpVec(CamLoc, Location, CamUp);
        }
        void Model_Move()
        {
            Model_01_Position_X += Model_Distance;
            Model_02_Position_X += Model_Distance;
            Model_03_Position_X += Model_Distance;
            if (Model_Number == 2)
            {
                if (Model_01_Position_X == Model_Distance + Model_Distance / 2)
                {
                    Model_01_Position_X = -Model_Distance / 2;
                }
                if (Model_02_Position_X == Model_Distance + Model_Distance / 2)
                {
                    Model_02_Position_X = -Model_Distance / 2;
                }
                if (Model_03_Position_X == Model_Distance + Model_Distance / 2)
                {
                    Model_03_Position_X = -Model_Distance / 2;
                }
            }
            else if (Model_Number == 3)
            {
                if (Model_01_Position_X == Model_Distance + Model_Distance)
                {
                    Model_01_Position_X = -Model_Distance;
                }
                if (Model_02_Position_X == Model_Distance + Model_Distance)
                {
                    Model_02_Position_X = -Model_Distance;
                }
                if (Model_03_Position_X == Model_Distance + Model_Distance)
                {
                    Model_03_Position_X = -Model_Distance;
                }
            }
            else if (Model_Number == 4)
            {
                if (Model_01_Position_X == Model_Distance + Model_Distance + Model_Distance / 2)
                {
                    Model_01_Position_X = -Model_Distance - Model_Distance / 2;
                }
                if (Model_02_Position_X == Model_Distance + Model_Distance + Model_Distance / 2)
                {
                    Model_02_Position_X = -Model_Distance - Model_Distance / 2;
                }
                if (Model_03_Position_X == Model_Distance + Model_Distance + Model_Distance / 2)
                {
                    Model_03_Position_X = -Model_Distance - Model_Distance / 2;
                }
            }
            else if (Model_Number == 5)
            {
                if (Model_01_Position_X == Model_Distance + Model_Distance + Model_Distance)
                {
                    Model_01_Position_X = -Model_Distance - Model_Distance;
                }
                if (Model_02_Position_X == Model_Distance + Model_Distance + Model_Distance)
                {
                    Model_02_Position_X = -Model_Distance - Model_Distance;
                }
                if (Model_03_Position_X == Model_Distance + Model_Distance + Model_Distance)
                {
                    Model_03_Position_X = -Model_Distance - Model_Distance;
                }
            }
        }
        void Model_Opacity_Change()
        {
            if (IsOpacityChange_Down)
            {
                float Opacity = DX.MV1GetOpacityRate(ModelHandle_00);
                if (Opacity <= 0.0f)
                {
                    IsOpacityChange_Down = false;
                    IsOpacityChange_Up = true;
                    Model_Move();
                }
                else
                {
                    DX.MV1SetOpacityRate(ModelHandle_00, Opacity - (float)(0.05 * FPS_Kakeru));
                    DX.MV1SetOpacityRate(ModelHandle_01, Opacity - (float)(0.05 * FPS_Kakeru));
                    DX.MV1SetOpacityRate(ModelHandle_02, Opacity - (float)(0.05 * FPS_Kakeru));
                    DX.MV1SetOpacityRate(ModelHandle_03, Opacity - (float)(0.05 * FPS_Kakeru));
                    DX.MV1SetOpacityRate(ModelHandle_04, Opacity - (float)(0.05 * FPS_Kakeru));
                }
            }
            else if (IsOpacityChange_Up)
            {
                float Opacity = DX.MV1GetOpacityRate(ModelHandle_00);
                if (Opacity >= 1.0f)
                {
                    IsOpacityChange_Up = false;
                    IsOpacityChange = false;
                }
                else
                {
                    DX.MV1SetOpacityRate(ModelHandle_00, Opacity + (float)(0.05 * FPS_Kakeru));
                    DX.MV1SetOpacityRate(ModelHandle_01, Opacity + (float)(0.05 * FPS_Kakeru));
                    DX.MV1SetOpacityRate(ModelHandle_02, Opacity + (float)(0.05 * FPS_Kakeru));
                    DX.MV1SetOpacityRate(ModelHandle_03, Opacity + (float)(0.05 * FPS_Kakeru));
                    DX.MV1SetOpacityRate(ModelHandle_04, Opacity + (float)(0.05 * FPS_Kakeru));
                }
            }
        }
        void Dance_Loop()
        {
            //メインコード
            CameraLookAtPosition.x = 0;
            CameraLookAtPosition.y = 0;
            CameraLookAtPosition.z = 0;
            float CameraVAngle = 0f;
            float FPS_CameraHAngle = 0f;
            float FPS_CameraVAngle = 0f;
            float SinParam = 0f;
            float CosParam = 0f;
            AttachIndex_00 = DX.MV1AttachAnim(ModelHandle_00, 0, -1, DX.FALSE);
            AttachIndex_01 = DX.MV1AttachAnim(ModelHandle_01, 0, -1, DX.FALSE);
            AttachIndex_02 = DX.MV1AttachAnim(ModelHandle_02, 0, -1, DX.FALSE);
            AttachIndex_03 = DX.MV1AttachAnim(ModelHandle_03, 0, -1, DX.FALSE);
            AttachIndex_04 = DX.MV1AttachAnim(ModelHandle_04, 0, -1, DX.FALSE);
            TotalTime = DX.MV1GetAttachAnimTotalTime(ModelHandle_00, AttachIndex_00);
            PlayTime = 2f;
            double nextframe = Environment.TickCount;
            float wait = 1000f / FPS;
            //ループ
            while (!IsClosed)
            {
                if (Environment.TickCount >= nextframe)
                {
                    DX.ClearDrawScreen();
                    if (IsFrameRateLock)
                    {
                        if (IsWMPUse)
                        {
                            PlayTime = (float)Player.controls.currentPosition * 30;
                        }
                        else
                        {
                            PlayTime = DX.GetSoundCurrentTime(MusicHandle) * 3 / 100;
                        }
                    }
                    else
                    {
                        PlayTime += PlayTime_Plus;
                    }
                    //マウスの移動量を取得
                    int Move_MouseX = Cursor.Position.X - (int)w / 2;
                    int Move_MouseY = Cursor.Position.Y - (int)h / 2;
                    if (PlayTime >= TotalTime)
                    {
                        //曲が終わったら最初から
                        if (IsWMPUse)
                        {
                            Player.controls.currentPosition = 0;
                        }
                        else
                        {
                            DX.StopSoundMem(MusicHandle);
                            DX.SetSoundCurrentPosition(0, MusicHandle);
                            DX.PlaySoundMem(MusicHandle, DX.DX_PLAYTYPE_LOOP, DX.FALSE);
                        }
                        PlayTime = 0f;
                        Stop = false;
                    }
                    if (!Stop && !IsFrameRateLock && (int)PlayTime % 300 == 0)
                    {
                        if (IsWMPUse)
                        {
                            PlayTime = (float)Player.controls.currentPosition * 30;
                        }
                        else
                        {
                            PlayTime = DX.GetSoundCurrentTime(MusicHandle) * 3 / 100;
                        }
                    }
                    if (FPS_Camera == false && Camera_VMD == false)
                    {
                        //キャラクターの当たり判定の位置を更新
                        if (IsModelPhysicsSet)
                        {
                            int Frame_01 = DX.MV1SearchFrame(ModelHandle_00, "頭");
                            float Model_01_X = DX.MV1GetFramePosition(ModelHandle_00, Frame_01).x;
                            float Model_01_Z = DX.MV1GetFramePosition(ModelHandle_00, Frame_01).z;
                            int Frame_02 = DX.MV1SearchFrame(ModelHandle_01, "頭");
                            float Model_02_X = DX.MV1GetFramePosition(ModelHandle_01, Frame_02).x;
                            float Model_02_Z = DX.MV1GetFramePosition(ModelHandle_01, Frame_02).z;
                            int Frame_03 = DX.MV1SearchFrame(ModelHandle_02, "頭");
                            float Model_03_X = DX.MV1GetFramePosition(ModelHandle_02, Frame_03).x;
                            float Model_03_Z = DX.MV1GetFramePosition(ModelHandle_02, Frame_03).z;
                            int Frame_04 = DX.MV1SearchFrame(ModelHandle_03, "頭");
                            float Model_04_X = DX.MV1GetFramePosition(ModelHandle_03, Frame_04).x;
                            float Model_04_Z = DX.MV1GetFramePosition(ModelHandle_03, Frame_04).z;
                            int Frame_05 = DX.MV1SearchFrame(ModelHandle_04, "頭");
                            float Model_05_X = DX.MV1GetFramePosition(ModelHandle_04, Frame_04).x;
                            float Model_05_Z = DX.MV1GetFramePosition(ModelHandle_04, Frame_05).z;
                            DX.MV1SetPosition(Model_Physics_00, DX.VGet(Model_01_X, 0f, Model_01_Z));
                            DX.MV1SetPosition(Model_Physics_01, DX.VGet(Model_02_X, 0f, Model_02_Z));
                            DX.MV1SetPosition(Model_Physics_02, DX.VGet(Model_03_X, 0f, Model_03_Z));
                            DX.MV1SetPosition(Model_Physics_03, DX.VGet(Model_04_X, 0f, Model_04_Z));
                            DX.MV1SetPosition(Model_Physics_04, DX.VGet(Model_05_X, 0f, Model_05_Z));
                            CollRef();
                        }
                        //TPSモードでなく、MMDのカメラでもない場合のカメラの移動
                        if (DX.CheckHitKey(DX.KEY_INPUT_W) > 0)
                        {
                            if (IsPhysicsSet || IsModelPhysicsSet)
                            {
                                //CameraPositionCollCheckがtrueだった場合のみカメラを移動
                                float X_Temp = (float)Math.Sin(-angleY) * 0.125f * FPS_Kakeru;
                                float Z_Temp = (float)Math.Cos(-angleY) * 0.125f * FPS_Kakeru;
                                if (CameraPositionCollCheck(DX.VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                                {
                                    x += (float)Math.Sin(-angleY) * 0.05f * FPS_Kakeru;
                                }
                                if (CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                                {
                                    z += (float)Math.Cos(-angleY) * 0.05f * FPS_Kakeru;
                                }
                            }
                            else
                            {
                                x += (float)Math.Sin(-angleY) * 0.05f * FPS_Kakeru;
                                z += (float)Math.Cos(-angleY) * 0.05f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_A) > 0)
                        {
                            if (IsPhysicsSet || IsModelPhysicsSet)
                            {
                                float X_Temp = (float)Math.Sin(-angleY - 1.5f) * 0.125f * FPS_Kakeru;
                                float Z_Temp = (float)Math.Cos(-angleY - 1.5f) * 0.125f * FPS_Kakeru;
                                if (CameraPositionCollCheck(DX.VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                                {
                                    x += (float)Math.Sin(-angleY - 1.5f) * 0.05f * FPS_Kakeru;
                                }
                                if (CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                                {
                                    z += (float)Math.Cos(-angleY - 1.5f) * 0.05f * FPS_Kakeru;
                                }
                            }
                            else
                            {
                                x += (float)Math.Sin(-angleY - 1.5f) * 0.05f * FPS_Kakeru;
                                z += (float)Math.Cos(-angleY - 1.5f) * 0.05f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_S) > 0)
                        {
                            if (IsPhysicsSet || IsModelPhysicsSet)
                            {
                                float X_Temp = (float)Math.Sin(-angleY) * -0.125f * FPS_Kakeru;
                                float Z_Temp = (float)Math.Cos(-angleY) * -0.125f * FPS_Kakeru;
                                if (CameraPositionCollCheck(DX.VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                                {
                                    x += (float)Math.Sin(-angleY) * -0.05f * FPS_Kakeru;
                                }
                                if (CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                                {
                                    z += (float)Math.Cos(-angleY) * -0.05f * FPS_Kakeru;
                                }
                            }
                            else
                            {
                                x += (float)Math.Sin(-angleY) * -0.05f * FPS_Kakeru;
                                z += (float)Math.Cos(-angleY) * -0.05f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_D) > 0)
                        {
                            if (IsPhysicsSet || IsModelPhysicsSet)
                            {
                                float X_Temp = (float)Math.Sin(-angleY - 1.5f) * -0.125f * FPS_Kakeru;
                                float Z_Temp = (float)Math.Cos(-angleY - 1.5f) * -0.125f * FPS_Kakeru;
                                if (CameraPositionCollCheck(DX.VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                                {
                                    x += (float)Math.Sin(-angleY - 1.5f) * -0.05f * FPS_Kakeru;
                                }
                                if (CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                                {
                                    z += (float)Math.Cos(-angleY - 1.5f) * -0.05f * FPS_Kakeru;
                                }
                            }
                            else
                            {
                                x += (float)Math.Sin(-angleY - 1.5f) * -0.05f * FPS_Kakeru;
                                z += (float)Math.Cos(-angleY - 1.5f) * -0.05f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_W) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                        {
                            if (IsPhysicsSet || IsModelPhysicsSet)
                            {
                                float X_Temp = (float)Math.Sin(-angleY) * 0.125f * FPS_Kakeru;
                                float Z_Temp = (float)Math.Cos(-angleY) * 0.125f * FPS_Kakeru;
                                if (CameraPositionCollCheck(DX.VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                                {
                                    x += (float)Math.Sin(-angleY) * 0.125f * FPS_Kakeru;
                                }
                                if (CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                                {
                                    z += (float)Math.Cos(-angleY) * 0.125f * FPS_Kakeru;
                                }
                            }
                            else
                            {
                                x += (float)Math.Sin(-angleY) * 0.125f * FPS_Kakeru;
                                z += (float)Math.Cos(-angleY) * 0.125f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_A) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                        {
                            if (IsPhysicsSet || IsModelPhysicsSet)
                            {
                                float X_Temp = (float)Math.Sin(-angleY - 1.5f) * 0.125f * FPS_Kakeru;
                                float Z_Temp = (float)Math.Cos(-angleY - 1.5f) * 0.125f * FPS_Kakeru;
                                if (CameraPositionCollCheck(DX.VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                                {
                                    x += (float)Math.Sin(-angleY - 1.5f) * 0.125f * FPS_Kakeru;
                                }
                                if (CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                                {
                                    z += (float)Math.Cos(-angleY - 1.5f) * 0.125f * FPS_Kakeru;
                                }
                            }
                            else
                            {
                                x += (float)Math.Sin(-angleY - 1.5f) * 0.125f * FPS_Kakeru;
                                z += (float)Math.Cos(-angleY - 1.5f) * 0.125f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_S) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                        {
                            if (IsPhysicsSet || IsModelPhysicsSet)
                            {
                                float X_Temp = (float)Math.Sin(-angleY) * -0.125f * FPS_Kakeru;
                                float Z_Temp = (float)Math.Cos(-angleY) * -0.125f * FPS_Kakeru;
                                if (CameraPositionCollCheck(DX.VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                                {
                                    x += (float)Math.Sin(-angleY) * -0.125f * FPS_Kakeru;
                                }
                                if (CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                                {
                                    z += (float)Math.Cos(-angleY) * -0.125f * FPS_Kakeru;
                                }
                            }
                            else
                            {
                                x += (float)Math.Sin(-angleY) * -0.125f * FPS_Kakeru;
                                z += (float)Math.Cos(-angleY) * -0.125f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_D) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                        {
                            if (IsPhysicsSet || IsModelPhysicsSet)
                            {
                                float X_Temp = (float)Math.Sin(-angleY - 1.5f) * -0.125f * FPS_Kakeru;
                                float Z_Temp = (float)Math.Cos(-angleY - 1.5f) * -0.125f * FPS_Kakeru;
                                if (CameraPositionCollCheck(DX.VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                                {
                                    x += (float)Math.Sin(-angleY - 1.5f) * -0.125f * FPS_Kakeru;
                                }
                                if (CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                                {
                                    z += (float)Math.Cos(-angleY - 1.5f) * -0.125f * FPS_Kakeru;
                                }
                            }
                            else
                            {
                                x += (float)Math.Sin(-angleY - 1.5f) * -0.125f * FPS_Kakeru;
                                z += (float)Math.Cos(-angleY - 1.5f) * -0.125f * FPS_Kakeru;
                            }
                        }
                    }
                    else
                    {
                        //TPSモードだった場合
                        if (DX.CheckHitKey(DX.KEY_INPUT_W) > 0)
                        {
                            if (Camera_Tall <= 2f)
                            {
                                Camera_Tall += 0.5f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_A) > 0)
                        {
                            if (Camera_Side >= -0.8f)
                            {
                                Camera_Side -= 0.2f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_S) > 0)
                        {
                            if (Camera_Tall >= -15f)
                            {
                                Camera_Tall -= 0.5f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_D) > 0)
                        {
                            if (Camera_Side <= 2f)
                            {
                                Camera_Side += 0.2f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_W) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                        {
                            if (Camera_Tall <= 2f)
                            {
                                Camera_Tall += 0.5f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_A) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                        {
                            if (Camera_Side >= -0.8f)
                            {
                                Camera_Side -= 0.35f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_S) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                        {
                            if (Camera_Tall >= -15f)
                            {
                                Camera_Tall -= 0.5f * FPS_Kakeru;
                            }
                        }
                        if (DX.CheckHitKey(DX.KEY_INPUT_D) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                        {
                            if (Camera_Side <= 2f)
                            {
                                Camera_Side += 0.35f * FPS_Kakeru;
                            }
                        }
                    }
                    if (!CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                    {
                        //カメラがモデルと重なっていたらzを変更
                        z += (float)Math.Cos(-angleY - 1.5f) * -0.125f * FPS_Kakeru;
                    }
                    //カメラ関連
                    if (DX.CheckHitKey(DX.KEY_INPUT_LSHIFT) > 0)
                    {
                        if (FPS_Camera == true)
                        {
                            if (Camera_Distance >= 5f)
                            {
                                Camera_Distance -= 0.5f * FPS_Kakeru;
                            }
                        }
                        else
                        {
                            if (CameraVAngle >= -50f)
                            {
                                CameraVAngle -= 1f * FPS_Kakeru;
                            }
                            float Y_Temp = DX.GetCameraPosition().y - 0.5f * FPS_Kakeru;
                            if (CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), Y_Temp + (y * 10), cameraZ + (z * 10))))
                            {
                                cameraY = DX.GetCameraPosition().y - 0.5f * FPS_Kakeru;
                            }
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_SPACE) > 0)
                    {
                        if (FPS_Camera == true)
                        {
                            if (Camera_Distance <= 60)
                            {
                                Camera_Distance += 0.5f * FPS_Kakeru;
                            }
                        }
                        else
                        {
                            if (CameraVAngle <= 50f)
                            {
                                CameraVAngle += 1f * FPS_Kakeru;
                            }
                            float Y_Temp = DX.GetCameraPosition().y + 0.5f * FPS_Kakeru;
                            if (CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), Y_Temp + (y * 10), cameraZ + (z * 10))))
                            {
                                cameraY = DX.GetCameraPosition().y + 0.5f * FPS_Kakeru;
                            }
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_LSHIFT) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                    {
                        if (FPS_Camera == true)
                        {
                            if (Camera_Distance >= 5f)
                            {
                                Camera_Distance -= 1f * FPS_Kakeru;
                            }
                        }
                        else
                        {
                            if (CameraVAngle >= -50f)
                            {
                                CameraVAngle -= 2f * FPS_Kakeru;
                            }
                            float Y_Temp = DX.GetCameraPosition().y - 1.25f * FPS_Kakeru;
                            if (CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), Y_Temp + (y * 10), cameraZ + (z * 10))))
                            {
                                cameraY = DX.GetCameraPosition().y - 1.25f * FPS_Kakeru;
                            }
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_SPACE) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                    {
                        if (FPS_Camera == true)
                        {
                            if (Camera_Distance <= 60)
                            {
                                Camera_Distance += 1f * FPS_Kakeru;
                            }
                        }
                        else
                        {
                            if (CameraVAngle <= 50f)
                            {
                                CameraVAngle += 2f * FPS_Kakeru;
                            }
                            float Y_Temp = DX.GetCameraPosition().y + 1.25f * FPS_Kakeru;
                            if (CameraPositionCollCheck(DX.VGet(cameraX + (x * 10), Y_Temp + (y * 10), cameraZ + (z * 10))))
                            {
                                cameraY = DX.GetCameraPosition().y + 1.25f * FPS_Kakeru;
                            }
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_LEFT) > 0)
                    {
                        if (FPS_Camera == true)
                        {
                            FPS_CameraHAngle -= 1.5f * FPS_Kakeru;
                        }
                        else if (Camera_VMD == false)
                        {
                            targetZ += 0.02f * FPS_Kakeru;
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_RIGHT) > 0)
                    {
                        if (FPS_Camera == true)
                        {
                            FPS_CameraHAngle += 1.5f * FPS_Kakeru;
                        }
                        else if (Camera_VMD == false)
                        {
                            targetZ -= 0.02f * FPS_Kakeru;
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_UP) > 0)
                    {
                        if (FPS_Camera == true)
                        {
                            if (Linear_Not_VMD > 0.075f)
                            {
                                Linear_Not_VMD -= 0.005f * FPS_Kakeru;
                            }
                        }
                        else
                        {
                            if (Camera_VMD == false)
                            {
                                if (Linear_Not_VMD > 0.075f)
                                {
                                    Linear_Not_VMD -= 0.005f * FPS_Kakeru;
                                }
                            }
                            else
                            {
                                if (CameraVAngle <= 5f)
                                {
                                    CameraVAngle += 0.1f * FPS_Kakeru;
                                }
                                Linear_Plus -= 0.005f * FPS_Kakeru;
                            }
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_DOWN) > 0)
                    {
                        if (FPS_Camera == true)
                        {
                            if (Linear_Not_VMD < 2f)
                            {
                                Linear_Not_VMD += 0.005f * FPS_Kakeru;
                            }
                        }
                        else
                        {
                            if (Camera_VMD == false)
                            {
                                if (Linear_Not_VMD < 2f)
                                {
                                    Linear_Not_VMD += 0.005f * FPS_Kakeru;
                                }
                            }
                            else
                            {
                                if (CameraVAngle >= -5f)
                                {
                                    CameraVAngle -= 0.1f * FPS_Kakeru;
                                }
                                Linear_Plus += 0.005f * FPS_Kakeru;
                            }
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_LEFT) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                    {
                        if (FPS_Camera == false && Camera_VMD == false)
                        {
                            targetZ += 0.04f * FPS_Kakeru;
                        }
                        else
                        {
                            FPS_CameraHAngle -= 4f * FPS_Kakeru;
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_RIGHT) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                    {
                        if (FPS_Camera == false && Camera_VMD == false)
                        {
                            targetZ -= 0.04f * FPS_Kakeru;
                        }
                        else
                        {
                            FPS_CameraHAngle += 4f * FPS_Kakeru;
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_UP) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                    {
                        if (FPS_Camera == false)
                        {
                            if (Camera_VMD == false)
                            {
                                if (CameraVAngle <= 5f)
                                {
                                    CameraVAngle += 0.2f * FPS_Kakeru;
                                }
                                if (Linear_Not_VMD > 0.075f)
                                {
                                    Linear_Not_VMD -= 0.01f * FPS_Kakeru;
                                }
                            }
                            else
                            {
                                Linear_Plus -= 0.01f * FPS_Kakeru;
                            }
                        }
                        else
                        {
                            if (Linear_Not_VMD > 0.075f)
                            {
                                Linear_Not_VMD -= 0.01f * FPS_Kakeru;
                            }
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_DOWN) > 0 && DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) > 0)
                    {
                        if (FPS_Camera == false)
                        {
                            if (Camera_VMD == false)
                            {
                                if (Linear_Not_VMD < 2f)
                                {
                                    Linear_Not_VMD += 0.01f * FPS_Kakeru;
                                }
                            }
                            else
                            {
                                if (CameraVAngle >= -5f)
                                {
                                    CameraVAngle -= 0.2f * FPS_Kakeru;
                                }
                                Linear_Plus += 0.01f * FPS_Kakeru;
                            }
                        }
                        else
                        {
                            if (Linear_Not_VMD < 2f)
                            {
                                Linear_Not_VMD += 0.01f * FPS_Kakeru;
                            }
                        }
                    }
                    //ショートカット関連
                    if (DX.CheckHitKey(DX.KEY_INPUT_K) > 0 && Stop == false)
                    {
                        //5,10秒進む
                        K_KEY_Tap += 1;
                        if (K_KEY_Tap == 1)
                        {
                            if (IsWMPUse)
                            {
                                if (DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) != 0)
                                {
                                    Player.controls.currentPosition += 10;
                                }
                                else
                                {
                                    Player.controls.currentPosition += 5;
                                }
                                PlayTime = (float)Player.controls.currentPosition * 30;
                            }
                            else
                            {
                                long PlayTime_Music = DX.GetSoundCurrentTime(MusicHandle);
                                if (DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) != 0)
                                {
                                    DX.StopSoundMem(MusicHandle);
                                    DX.SetSoundCurrentTime(PlayTime_Music + 10000, MusicHandle);
                                    DX.PlaySoundMem(MusicHandle, DX.DX_PLAYTYPE_LOOP, DX.FALSE);
                                    PlayTime = DX.GetSoundCurrentTime(MusicHandle) * 3 / 100;
                                }
                                else
                                {
                                    DX.StopSoundMem(MusicHandle);
                                    DX.SetSoundCurrentTime(PlayTime_Music + 5000, MusicHandle);
                                    DX.PlaySoundMem(MusicHandle, DX.DX_PLAYTYPE_LOOP, DX.FALSE);
                                    PlayTime = DX.GetSoundCurrentTime(MusicHandle) * 3 / 100;
                                }
                            }
                        }
                    }
                    else
                    {
                        K_KEY_Tap = 0;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_J) > 0 && Stop == false)
                    {
                        //5,10秒戻る
                        J_KEY_Tap += 1;
                        if (J_KEY_Tap == 1)
                        {
                            if (IsWMPUse)
                            {
                                double Position = Player.controls.currentPosition;
                                if (DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) != 0)
                                {
                                    if (Position >= 10)
                                    {
                                        Player.controls.currentPosition -= 10;
                                        PlayTime = (float)Player.controls.currentPosition * 30;
                                    }
                                    else
                                    {
                                        Player.controls.currentPosition = 0;
                                        PlayTime = 5f;
                                    }
                                }
                                else
                                {
                                    if (Position >= 5)
                                    {
                                        Player.controls.currentPosition -= 5;
                                        PlayTime = (float)Player.controls.currentPosition * 30;
                                    }
                                    else
                                    {
                                        Player.controls.currentPosition = 0;
                                        PlayTime = 5f;
                                    }
                                }
                            }
                            else
                            {
                                long PlayTime_Music = DX.GetSoundCurrentTime(MusicHandle);
                                DX.StopSoundMem(MusicHandle);
                                if (DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) != 0)
                                {
                                    if (PlayTime >= 300.0f)
                                    {
                                        DX.StopSoundMem(MusicHandle);
                                        DX.SetSoundCurrentTime(PlayTime_Music - 10000, MusicHandle);
                                        DX.PlaySoundMem(MusicHandle, DX.DX_PLAYTYPE_LOOP, DX.FALSE);
                                    }
                                    else
                                    {
                                        DX.StopSoundMem(MusicHandle);
                                        DX.SetSoundCurrentTime(0, MusicHandle);
                                        DX.PlaySoundMem(MusicHandle, DX.DX_PLAYTYPE_LOOP, DX.FALSE);
                                    }
                                }
                                else
                                {
                                    if (PlayTime >= 150.0f)
                                    {
                                        DX.StopSoundMem(MusicHandle);
                                        DX.SetSoundCurrentTime(PlayTime_Music - 5000, MusicHandle);
                                        DX.PlaySoundMem(MusicHandle, DX.DX_PLAYTYPE_LOOP, DX.FALSE);
                                    }
                                    else
                                    {
                                        DX.StopSoundMem(MusicHandle);
                                        DX.SetSoundCurrentTime(0, MusicHandle);
                                        DX.PlaySoundMem(MusicHandle, DX.DX_PLAYTYPE_LOOP, DX.FALSE);
                                    }
                                }
                                PlayTime = (float)DX.GetSoundCurrentTime(MusicHandle) * 3 / 100;
                            }
                        }
                    }
                    else
                    {
                        J_KEY_Tap = 0;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_M) != 0 && DX.CheckHitKey(DX.KEY_INPUT_R) != 0)
                    {
                        //曲とモーションを合わせる
                        M_R_Key_Down++;
                        if (M_R_Key_Down == 1)
                        {
                            if (IsWMPUse)
                            {
                                PlayTime = (float)Player.controls.currentPosition * 30;
                            }
                            else
                            {
                                PlayTime = DX.GetSoundCurrentTime(MusicHandle) * 3 / 100;
                            }
                        }
                    }
                    else
                    {
                        M_R_Key_Down = 0;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_F) != 0)
                    {
                        //キャラクター視点に切り替える
                        F_KEY_Tap += 1;
                        if (F_KEY_Tap == 1)
                        {
                            if (Select_Chara == 0)
                            {
                                FPS_Camera = true;
                                Select_Chara = 1;
                            }
                            else if (Select_Chara == 1 && Model_Number >= Select_Chara + 1)
                            {
                                FPS_Camera = true;
                                Select_Chara = 2;
                            }
                            else if (Select_Chara == 2 && Model_Number >= Select_Chara + 1)
                            {
                                FPS_Camera = true;
                                Select_Chara = 3;
                            }
                            else if (Select_Chara == 3 && Model_Number >= Select_Chara + 1)
                            {
                                FPS_Camera = true;
                                Select_Chara = 4;
                            }
                            else if (Select_Chara == 4 && Model_Number >= Select_Chara + 1)
                            {
                                FPS_Camera = true;
                                Select_Chara = 5;
                            }
                            else
                            {
                                FPS_Camera = false;
                                Select_Chara = 0;
                            }
                        }
                    }
                    else
                    {
                        F_KEY_Tap = 0;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_LCONTROL) != 0 && DX.CheckHitKey(DX.KEY_INPUT_M) > 0)
                    {
                        //ミュート
                        M_KEY_Tap += 1;
                        if (M_KEY_Tap == 1)
                        {
                            if (Music_Mute == false)
                            {
                                Music_Mute = true;
                                if (IsWMPUse)
                                {
                                    Player.settings.volume = 0;
                                }
                                else
                                {
                                    DX.ChangeVolumeSoundMem(0, MusicHandle);
                                }
                            }
                            else
                            {
                                Music_Mute = false;
                            }
                        }
                    }
                    else
                    {
                        M_KEY_Tap = 0;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_M) != 0 && DX.CheckHitKey(DX.KEY_INPUT_S) != 0)
                    {
                        //時間停止
                        Stop_Key++;
                        if (Stop_Key == 1 && Stop == false)
                        {
                            Stop = true;
                            if (IsWMPUse)
                            {
                                Player.controls.pause();
                            }
                            else
                            {
                                Stop_Time = DX.GetSoundCurrentPosition(MusicHandle);
                                DX.StopSoundMem(MusicHandle);
                            }
                            Stop_Play_Time = PlayTime_Plus;
                            PlayTime_Plus = 0f;
                        }
                    }
                    else
                    {
                        Stop_Key = 0;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_M) != 0 && DX.CheckHitKey(DX.KEY_INPUT_P) != 0)
                    {
                        //停止していたら再生
                        Play_Key++;
                        if (Play_Key == 1 && Stop == true)
                        {
                            Stop = false;
                            if (IsWMPUse)
                            {
                                Player.controls.play();
                            }
                            else
                            {
                                DX.SetSoundCurrentPosition(Stop_Time, MusicHandle);
                                DX.PlaySoundMem(MusicHandle, DX.DX_PLAYTYPE_LOOP, DX.FALSE);
                            }
                            PlayTime_Plus = Stop_Play_Time;
                        }
                    }
                    else
                    {
                        Play_Key = 0;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_V) > 0 && DX.CheckHitKey(DX.KEY_INPUT_P) > 0)
                    {
                        //音量を上げる
                        if (Music_Mute == false)
                        {
                            Music_Volume += 2;
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_V) > 0 && DX.CheckHitKey(DX.KEY_INPUT_O) > 0)
                    {
                        //音量を下げる
                        if (Music_Mute == false)
                        {
                            Music_Volume -= 2;
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_C) > 0 && DX.CheckHitKey(DX.KEY_INPUT_P) > 0 && Stop == false)
                    {
                        //再生速度を上げる
                        if (Pitch_Set <= Pitch_Max && C_P_Enable)
                        {
                            if (IsWMPUse && C_P_Enable)
                            {
                                C_P_Enable = false;
                            }
                            if (IsWMPUse)
                            {
                                Pitch_Set += Pitch_Change_Size * FPS_Kakeru;
                            }
                            else
                            {
                                Pitch_Set += Pitch_Change_Size / 10 * FPS_Kakeru;
                            }
                            double Pitch = First_Music_Pitch * Pitch_Set;
                            if (IsWMPUse)
                            {
                                if (!IsMusicNotChange)
                                {
                                    Player.settings.rate = Pitch_Set;
                                }
                                PlayTime_Plus_Temp_Kakeru += (float)Pitch_Change_Size * FPS_Kakeru;
                                PlayTime_Plus = PlayTime_Plus_Temp * PlayTime_Plus_Temp_Kakeru;
                                Player.controls.currentPosition = PlayTime / 30;
                            }
                            else
                            {
                                if (!IsMusicNotChange)
                                {
                                    DX.SetFrequencySoundMem((int)Pitch, MusicHandle);
                                }
                                PlayTime_Plus_Temp_Kakeru += (float)Pitch_Change_Size / 10 * FPS_Kakeru;
                                PlayTime_Plus = PlayTime_Plus_Temp * PlayTime_Plus_Temp_Kakeru;
                            }
                        }
                    }
                    else
                    {
                        C_P_Enable = true;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_C) > 0 && DX.CheckHitKey(DX.KEY_INPUT_O) > 0 && Stop == false)
                    {
                        //再生速度を下げる
                        if (Pitch_Set >= Pitch_Min && C_O_Enable)
                        {
                            if (IsWMPUse && C_O_Enable)
                            {
                                C_O_Enable = false;
                            }
                            if (IsWMPUse)
                            {
                                Pitch_Set -= Pitch_Change_Size * FPS_Kakeru;
                            }
                            else
                            {
                                Pitch_Set -= Pitch_Change_Size / 10 * FPS_Kakeru;
                            }
                            double Pitch = First_Music_Pitch * Pitch_Set;
                            if (IsWMPUse)
                            {
                                if (!IsMusicNotChange)
                                {
                                    Player.settings.rate = Pitch_Set;
                                }
                                PlayTime_Plus_Temp_Kakeru -= (float)Pitch_Change_Size * FPS_Kakeru;
                                PlayTime_Plus = PlayTime_Plus_Temp * PlayTime_Plus_Temp_Kakeru;
                                Player.controls.currentPosition = PlayTime / 30;
                            }
                            else
                            {
                                if (!IsMusicNotChange)
                                {
                                    DX.SetFrequencySoundMem((int)Pitch, MusicHandle);
                                }
                                PlayTime_Plus_Temp_Kakeru -= (float)Pitch_Change_Size / 10 * FPS_Kakeru;
                                PlayTime_Plus = PlayTime_Plus_Temp * PlayTime_Plus_Temp_Kakeru;
                            }
                        }
                    }
                    else
                    {
                        C_O_Enable = true;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_G) != 0)
                    {
                        //MMDのカメラに切り替える
                        G_Key_Down++;
                        if (G_Key_Down == 1)
                        {
                            if (Camera_Number_First != 0)
                            {
                                if (Camera_VMD == true)
                                {
                                    Camera_VMD = false;
                                }
                                else
                                {
                                    Camera_VMD = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        G_Key_Down = 0;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_R) != 0 && DX.CheckHitKey(DX.KEY_INPUT_E) != 0)
                    {
                        //曲を最初から
                        if (IsWMPUse)
                        {
                            Player.controls.currentPosition = 0;
                        }
                        else
                        {
                            DX.StopSoundMem(MusicHandle);
                            DX.SetSoundCurrentPosition(0, MusicHandle);
                            DX.PlaySoundMem(MusicHandle, DX.DX_PLAYTYPE_LOOP, DX.FALSE);
                        }
                        PlayTime = 0f;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_P) != 0 && DX.CheckHitKey(DX.KEY_INPUT_D) != 0)
                    {
                        //パンを下げる
                        //*パンは、左右の音量比のこと。下げれば右が小さくなる
                        Pan--;
                        if (IsWMPUse)
                        {
                            //なぜか適応されない...
                            double Temp = Pan / 255 * 100;
                            Player.settings.balance = (int)Temp;
                        }
                        else
                        {
                            DX.ChangePanSoundMem(Pan, MusicHandle);
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_P) != 0 && DX.CheckHitKey(DX.KEY_INPUT_U) != 0)
                    {
                        //パンを上げる
                        Pan++;
                        if (IsWMPUse)
                        {
                            //...
                            double Temp = Pan / 255 * 100;
                            Player.settings.balance = (int)Temp;
                        }
                        else
                        {
                            DX.ChangePanSoundMem(Pan, MusicHandle);
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_P) != 0 && DX.CheckHitKey(DX.KEY_INPUT_R) != 0)
                    {
                        //ピッチ、パンを初期化
                        Pan = 0;
                        DX.ChangePanSoundMem(Pan, MusicHandle);
                        Pitch_Set = 1;
                        double Pitch = First_Music_Pitch * Pitch_Set;
                        if (IsWMPUse)
                        {
                            Player.settings.balance = 0;
                            Player.settings.rate = 1;
                            Player.controls.currentPosition = PlayTime / 30;
                        }
                        else
                        {
                            DX.SetFrequencySoundMem((int)Pitch, MusicHandle);
                        }
                        PlayTime_Plus_Temp_Kakeru = 1f;
                        PlayTime_Plus = PlayTime_Plus_Temp * PlayTime_Plus_Temp_Kakeru;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_C) != 0 && DX.CheckHitKey(DX.KEY_INPUT_1) != 0)
                    {
                        //カメラ1に切り替える
                        if (Camera_Number_First >= 1)
                        {
                            Camera_Number = 1;
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_C) != 0 && DX.CheckHitKey(DX.KEY_INPUT_2) != 0)
                    {
                        //カメラ2に切り替える
                        if (Camera_Number_First >= 2)
                        {
                            Camera_Number = 2;
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_C) != 0 && DX.CheckHitKey(DX.KEY_INPUT_3) != 0)
                    {
                        //カメラ3に切り替える
                        if (Camera_Number_First >= 3)
                        {
                            Camera_Number = 3;
                        }
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_C) != 0 && DX.CheckHitKey(DX.KEY_INPUT_Z) != 0)
                    {
                        //カメラの角度、視野を初期化
                        targetZ = 0f;
                        Linear_Not_VMD = 1f;
                        Linear_Plus = 0f;
                    }
                    if (DX.CheckHitKey(DX.KEY_INPUT_C) != 0 && DX.CheckHitKey(DX.KEY_INPUT_R) != 0)
                    {
                        //カメラ全般を初期化
                        if (Camera_VMD == true)
                        {
                            Linear_Plus = 0f;
                        }
                        else
                        {
                            Linear_Not_VMD = 1f;
                            targetZ = 0f;
                            targetX = 0f;
                            angleY = 0f;
                            targetY = 0f;
                            cameraX = 0f;
                            cameraY = 3f;
                            cameraZ = -5f;
                            x = 0f;
                            y = 0f;
                            z = 0f;
                        }
                    }
                    if (IsModelPositionDistance && DX.CheckHitKey(DX.KEY_INPUT_N) != 0)
                    {
                        //並び替える
                        N_KEY_Tap++;
                        if (N_KEY_Tap == 1 && Model_Number > 1)
                        {
                            if (!IsOpacityChange)
                            {
                                IsOpacityChange = true;
                                IsOpacityChange_Down = true;
                            }
                        }
                    }
                    else
                    {
                        N_KEY_Tap = 0;
                    }
                    //マウスで視点を変えれるように(TPSモードも対応)
                    if (IsShowMessageBox == false && Camera_VMD == false && FPS_Camera == false)
                    {
                        float Temp = targetX + (float)Move_MouseY / (Mouse_Sensitivity * 10) * Linear_Not_VMD;
                        if (Temp >= -1.14f && Temp <= 1.3f)
                        {
                            targetX += (float)Move_MouseY / (Mouse_Sensitivity * 10) * Linear_Not_VMD;
                        }
                        else if (Temp >= -1.14f)
                        {
                            targetX = 1.3f;
                        }
                        else if (Temp <= 1.3f)
                        {
                            targetX = -1.14f;
                        }
                        angleY -= (float)Move_MouseX / (Mouse_Sensitivity * 10) * Linear_Not_VMD;
                        targetY += (float)Move_MouseX / (Mouse_Sensitivity * 10) * Linear_Not_VMD;
                    }
                    if (IsShowMessageBox == false && FPS_Camera == true)
                    {
                        FPS_CameraHAngle -= (float)Move_MouseX / 10;
                        if (FPS_CameraVAngle - (float)Move_MouseY / 10 <= 60f && FPS_CameraVAngle - (float)Move_MouseY / 10 >= -40)
                        {
                            FPS_CameraVAngle -= (float)Move_MouseY / 10;
                        }
                        else if (FPS_CameraVAngle - (float)Move_MouseY / 10 >= 60f)
                        {
                            FPS_CameraVAngle = 60f;
                        }
                        else if (FPS_CameraVAngle - (float)Move_MouseY / 10 <= -40f)
                        {
                            FPS_CameraVAngle = -40f;
                        }
                    }
                    //TPSモード時のカメラ
                    if (FPS_Camera == true)
                    {
                        DX.SetupCamera_Perspective(Linear_Not_VMD);
                        if (Select_Chara == 1)
                        {
                            Chara_Select_Draw(ModelHandle_00);
                        }
                        else if (Select_Chara == 2)
                        {
                            Chara_Select_Draw(ModelHandle_01);
                        }
                        else if (Select_Chara == 3)
                        {
                            Chara_Select_Draw(ModelHandle_02);
                        }
                        else if (Select_Chara == 4)
                        {
                            Chara_Select_Draw(ModelHandle_03);
                        }
                        else if (Select_Chara == 5)
                        {
                            Chara_Select_Draw(ModelHandle_04);
                        }
                        CameraLookAtPosition.x += Camera_Side;
                        CameraLookAtPosition.y += Camera_Tall;
                        Camera_Location.x += Camera_Side;
                        Camera_Location.y += Camera_Tall;
                        SinParam = (float)Math.Sin(FPS_CameraVAngle / 180.0f * 3.14f);
                        CosParam = (float)Math.Cos(FPS_CameraVAngle / 180.0f * 3.14f);
                        TempPosition1.x = 0.0f;
                        TempPosition1.y = SinParam * Camera_Distance;
                        TempPosition1.z = -CosParam * Camera_Distance;
                        SinParam = (float)Math.Sin(FPS_CameraHAngle / 180.0f * 3.14f);
                        CosParam = (float)Math.Cos(FPS_CameraHAngle / 180.0f * 3.14f);
                        TempPosition2.x = CosParam * TempPosition1.x - SinParam * TempPosition1.z;
                        TempPosition2.y = TempPosition1.y;
                        TempPosition2.z = SinParam * TempPosition1.x + CosParam * TempPosition1.z;
                        CameraPosition = DX.VAdd(TempPosition2, CameraLookAtPosition);
                        DX.SetCameraPositionAndTarget_UpVecY(CameraPosition, Camera_Location);
                    }
                    else
                    {
                        //カメラのモーションがあれば実行なければマウスの角度を反映
                        if (Camera_VMD == true)
                        {
                            if (Camera_Number_First != 0)
                            {
                                if (Camera_Number == 1)
                                {
                                    SetupVMDCameraMotionParam(CameraHandle1, PlayTime);
                                }
                                else if (Camera_Number == 2)
                                {
                                    SetupVMDCameraMotionParam(CameraHandle2, PlayTime);
                                }
                                else if (Camera_Number == 3)
                                {
                                    SetupVMDCameraMotionParam(CameraHandle3, PlayTime);
                                }
                            }
                        }
                        else
                        {
                            DX.SetupCamera_Perspective(Linear_Not_VMD);
                            DX.SetCameraPositionAndAngle(DX.VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + (z * 10)), targetX, targetY, targetZ);
                        }
                    }
                    //フォーカスがなくなればマウスカーソルを表示させる
                    LostFocus += delegate
                    {
                        IsShowMessageBox = true;
                        Cursor.Show();
                    };
                    //フォーカスが戻れば非表示
                    GotFocus += delegate
                    {
                        IsShowMessageBox = false;
                        Cursor.Hide();
                    };
                    //影の描画
                    if (IsShadowMode)
                    {
                        Shadow_Draw();
                    }
                    //モデルの透明度を変更
                    if (IsOpacityChange)
                    {
                        Model_Opacity_Change();
                    }
                    //モデルの描画
                    //ホラーモードでは描画するタイミングが異なるため分けている
                    if (!IsHorror_Mode_And_Map_0)
                    {
                        DX.MV1DrawModel(SkyHandle);
                        DX.MV1SetRotationXYZ(ModelHandle_00, DX.VGet(0f, 0f, 0f));
                        DX.MV1SetRotationXYZ(ModelHandle_01, DX.VGet(0f, 0f, 0f));
                        DX.MV1SetRotationXYZ(ModelHandle_02, DX.VGet(0f, 0f, 0f));
                        DX.MV1SetRotationXYZ(ModelHandle_03, DX.VGet(0f, 0f, 0f));
                        DX.MV1SetRotationXYZ(ModelHandle_04, DX.VGet(0f, 0f, 0f));
                        if (IsModelPositionSet)
                        {
                            DX.MV1SetPosition(ModelHandle_00, Model_Position_01);
                            DX.MV1SetPosition(ModelHandle_01, Model_Position_02);
                            DX.MV1SetPosition(ModelHandle_02, Model_Position_03);
                            DX.MV1SetPosition(ModelHandle_03, Model_Position_04);
                            DX.MV1SetPosition(ModelHandle_04, Model_Position_05);
                        }
                        else
                        {
                            DX.MV1SetPosition(ModelHandle_00, DX.VGet(Model_01_Position_X, 0f, 0f));
                            DX.MV1SetPosition(ModelHandle_01, DX.VGet(Model_02_Position_X, 0f, 0f));
                            DX.MV1SetPosition(ModelHandle_02, DX.VGet(Model_03_Position_X, 0f, 0f));
                            DX.MV1SetPosition(ModelHandle_03, DX.VGet(Model_04_Position_X, 0f, 0f));
                            DX.MV1SetPosition(ModelHandle_04, DX.VGet(Model_05_Position_X, 0f, 0f));
                        }
                        DX.MV1SetAttachAnimTime(ModelHandle_00, AttachIndex_00, PlayTime);
                        DX.MV1SetAttachAnimTime(ModelHandle_01, AttachIndex_01, PlayTime);
                        DX.MV1SetAttachAnimTime(ModelHandle_02, AttachIndex_02, PlayTime);
                        DX.MV1SetAttachAnimTime(ModelHandle_03, AttachIndex_03, PlayTime);
                        DX.MV1SetAttachAnimTime(ModelHandle_04, AttachIndex_04, PlayTime);
                        if (!IsOpacityChange)
                        {
                            DX.MV1DrawModel(ModelHandle_00);
                            DX.MV1DrawModel(ModelHandle_01);
                            DX.MV1DrawModel(ModelHandle_02);
                            DX.MV1DrawModel(ModelHandle_03);
                            DX.MV1DrawModel(ModelHandle_04);
                        }
                        DX.MV1DrawModel(MapHandle);
                        DX.MV1DrawModel(OceanHandle);
                        if (IsOpacityChange)
                        {
                            DX.MV1DrawModel(ModelHandle_00);
                            DX.MV1DrawModel(ModelHandle_01);
                            DX.MV1DrawModel(ModelHandle_02);
                            DX.MV1DrawModel(ModelHandle_03);
                            DX.MV1DrawModel(ModelHandle_04);
                        }
                        DX.DrawRotaGraph((int)w, (int)h, 10, 0, ShaderHandle, DX.TRUE);
                        DX.MV1SetRotationXYZ(SkyHandle, DX.VGet(0f, DX.MV1GetRotationXYZ(SkyHandle).y + 0.0001f, 0f));
                    }
                    else
                    {
                        DX.MV1DrawModel(SkyHandle);
                        DX.MV1DrawModel(MapHandle);
                        DX.MV1DrawModel(OceanHandle);
                        DX.DrawRotaGraph((int)w, (int)h, 10, 0, ShaderHandle, DX.TRUE);
                        DX.MV1SetRotationXYZ(SkyHandle, DX.VGet(0f, DX.MV1GetRotationXYZ(SkyHandle).y + 0.0001f, 0f));
                        DX.MV1SetRotationXYZ(ModelHandle_00, DX.VGet(0f, 0f, 0f));
                        DX.MV1SetRotationXYZ(ModelHandle_01, DX.VGet(0f, 0f, 0f));
                        DX.MV1SetRotationXYZ(ModelHandle_02, DX.VGet(0f, 0f, 0f));
                        DX.MV1SetRotationXYZ(ModelHandle_03, DX.VGet(0f, 0f, 0f));
                        DX.MV1SetRotationXYZ(ModelHandle_04, DX.VGet(0f, 0f, 0f));
                        if (IsModelPositionSet)
                        {
                            DX.MV1SetPosition(ModelHandle_00, Model_Position_01);
                            DX.MV1SetPosition(ModelHandle_01, Model_Position_02);
                            DX.MV1SetPosition(ModelHandle_02, Model_Position_03);
                            DX.MV1SetPosition(ModelHandle_03, Model_Position_04);
                            DX.MV1SetPosition(ModelHandle_04, Model_Position_05);
                        }
                        else
                        {
                            DX.MV1SetPosition(ModelHandle_00, DX.VGet(Model_01_Position_X, 0f, 0f));
                            DX.MV1SetPosition(ModelHandle_01, DX.VGet(Model_02_Position_X, 0f, 0f));
                            DX.MV1SetPosition(ModelHandle_02, DX.VGet(Model_03_Position_X, 0f, 0f));
                            DX.MV1SetPosition(ModelHandle_03, DX.VGet(Model_04_Position_X, 0f, 0f));
                            DX.MV1SetPosition(ModelHandle_04, DX.VGet(Model_05_Position_X, 0f, 0f));
                        }
                        DX.MV1SetAttachAnimTime(ModelHandle_00, AttachIndex_00, PlayTime);
                        DX.MV1DrawModel(ModelHandle_00);
                        DX.MV1SetAttachAnimTime(ModelHandle_01, AttachIndex_01, PlayTime);
                        DX.MV1DrawModel(ModelHandle_01);
                        DX.MV1SetAttachAnimTime(ModelHandle_02, AttachIndex_02, PlayTime);
                        DX.MV1DrawModel(ModelHandle_02);
                        DX.MV1SetAttachAnimTime(ModelHandle_03, AttachIndex_03, PlayTime);
                        DX.MV1DrawModel(ModelHandle_03);
                        DX.MV1SetAttachAnimTime(ModelHandle_04, AttachIndex_04, PlayTime);
                        DX.MV1DrawModel(ModelHandle_04);
                    }
                    DX.SetUseShadowMap(0, -1);
                    DX.SetUseShadowMap(1, -1);
                    //ミュートでない場合、曲の音量を反映
                    if (Music_Mute == false)
                    {
                        if (IsWMPUse)
                        {
                            double Temp = (double)Music_Volume / 255 * 100;
                            Player.settings.volume = (int)Temp;
                        }
                        else
                        {
                            DX.ChangeVolumeSoundMem(Music_Volume, MusicHandle);
                        }
                    }
                    if (IsShowMessageBox == false)
                    {
                        //マウスを固定
                        Cursor.Position = new Point((int)w / 2, (int)h / 2);
                        Cursor.Hide();
                    }
                    else
                    {
                        Cursor.Show();
                    }
                    DX.ScreenFlip();
                    nextframe += wait;
                }
                Application.DoEvents();
            }
        }
        void Shadow_Draw()
        {
            //影の描画
            float Min_X;
            float Min_Z;
            float Max_X;
            float Max_Z;
            int Frame_01 = DX.MV1SearchFrame(ModelHandle_00, "頭");
            int Frame_02 = DX.MV1SearchFrame(ModelHandle_01, "頭");
            int Frame_03 = DX.MV1SearchFrame(ModelHandle_02, "頭");
            int Frame_04 = DX.MV1SearchFrame(ModelHandle_03, "頭");
            int Frame_05 = DX.MV1SearchFrame(ModelHandle_04, "頭");
            float Model_Position_01_X = DX.MV1GetFramePosition(ModelHandle_00, Frame_01).x;
            float Model_Position_01_Z = DX.MV1GetFramePosition(ModelHandle_00, Frame_01).z;
            float Model_Position_02_X = DX.MV1GetFramePosition(ModelHandle_01, Frame_02).x;
            float Model_Position_02_Z = DX.MV1GetFramePosition(ModelHandle_01, Frame_02).z;
            float Model_Position_03_X = DX.MV1GetFramePosition(ModelHandle_02, Frame_03).x;
            float Model_Position_03_Z = DX.MV1GetFramePosition(ModelHandle_02, Frame_03).z;
            float Model_Position_04_X = DX.MV1GetFramePosition(ModelHandle_03, Frame_04).x;
            float Model_Position_04_Z = DX.MV1GetFramePosition(ModelHandle_03, Frame_04).z;
            float Model_Position_05_X = DX.MV1GetFramePosition(ModelHandle_04, Frame_05).x;
            float Model_Position_05_Z = DX.MV1GetFramePosition(ModelHandle_04, Frame_05).z;
            Min_X = Model_Position_01_X;
            Max_X = Model_Position_01_X;
            Min_Z = Model_Position_01_Z;
            Max_Z = Model_Position_01_Z;
            if (Min_X >= Model_Position_02_X)
            {
                Min_X = Model_Position_02_X;
            }
            if (Min_X >= Model_Position_03_X)
            {
                Min_X = Model_Position_03_X;
            }
            if (Min_X >= Model_Position_04_X)
            {
                Min_X = Model_Position_04_X;
            }
            if (Min_X >= Model_Position_05_X)
            {
                Min_X = Model_Position_05_X;
            }
            if (Max_X <= Model_Position_02_X)
            {
                Max_X = Model_Position_02_X;
            }
            if (Max_X <= Model_Position_03_X)
            {
                Max_X = Model_Position_03_X;
            }
            if (Max_X <= Model_Position_04_X)
            {
                Max_X = Model_Position_04_X;
            }
            if (Max_X <= Model_Position_05_X)
            {
                Max_X = Model_Position_05_X;
            }
            if (Min_Z >= Model_Position_02_Z)
            {
                Min_Z = Model_Position_02_Z;
            }
            if (Min_Z >= Model_Position_03_Z)
            {
                Min_Z = Model_Position_03_Z;
            }
            if (Min_Z >= Model_Position_04_Z)
            {
                Min_Z = Model_Position_04_Z;
            }
            if (Min_Z >= Model_Position_05_Z)
            {
                Min_Z = Model_Position_05_Z;
            }
            if (Max_Z <= Model_Position_02_Z)
            {
                Max_Z = Model_Position_02_Z;
            }
            if (Max_Z <= Model_Position_03_Z)
            {
                Max_Z = Model_Position_03_Z;
            }
            if (Max_Z <= Model_Position_04_Z)
            {
                Max_Z = Model_Position_04_Z;
            }
            if (Max_Z <= Model_Position_05_Z)
            {
                Max_Z = Model_Position_05_Z;
            }
            DX.SetShadowMapDrawArea(Model_Shadow_00, DX.VGet(Min_X - 20f, -1f, Min_Z - 20f), DX.VGet(Max_X + 20f, 30f, Max_Z + 20f));
            DX.ShadowMap_DrawSetup(Model_Shadow_00);
            DX.MV1DrawModel(ModelHandle_00);
            DX.MV1DrawModel(ModelHandle_01);
            DX.MV1DrawModel(ModelHandle_02);
            DX.MV1DrawModel(ModelHandle_03);
            DX.MV1DrawModel(ModelHandle_04);
            DX.ShadowMap_DrawEnd();
            DX.SetUseShadowMap(0, Model_Shadow_00);
            DX.SetUseShadowMap(1, Map_Shadow_00);
        }
        void CollRef()
        {
            //当たり判定用のモデルの位置を更新
            DX.MV1RefreshCollInfo(Model_Physics_00, -1);
            DX.MV1RefreshCollInfo(Model_Physics_01, -1);
            DX.MV1RefreshCollInfo(Model_Physics_02, -1);
            DX.MV1RefreshCollInfo(Model_Physics_03, -1);
            DX.MV1RefreshCollInfo(Model_Physics_04, -1);
        }
        bool CameraPositionCollCheck(DX.VECTOR CameraPosition)
        {
            if (IsPhysicsSet && IsModelPhysicsSet)
            {
                //当たり判定(すべて)
                DX.MV1_COLL_RESULT_POLY_DIM HRes_Map;
                DX.MV1_COLL_RESULT_POLY_DIM Model_01;
                DX.MV1_COLL_RESULT_POLY_DIM Model_02;
                DX.MV1_COLL_RESULT_POLY_DIM Model_03;
                DX.MV1_COLL_RESULT_POLY_DIM Model_04;
                DX.MV1_COLL_RESULT_POLY_DIM Model_05;
                HRes_Map = DX.MV1CollCheck_Capsule(MapHandle, -1, CameraPosition, CameraPosition, 1.3f);
                Model_01 = DX.MV1CollCheck_Capsule(Model_Physics_00, -1, CameraPosition, CameraPosition, 1.3f);
                Model_02 = DX.MV1CollCheck_Capsule(Model_Physics_01, -1, CameraPosition, CameraPosition, 1.3f);
                Model_03 = DX.MV1CollCheck_Capsule(Model_Physics_02, -1, CameraPosition, CameraPosition, 1.3f);
                Model_04 = DX.MV1CollCheck_Capsule(Model_Physics_03, -1, CameraPosition, CameraPosition, 1.3f);
                Model_05 = DX.MV1CollCheck_Capsule(Model_Physics_04, -1, CameraPosition, CameraPosition, 1.3f);
                int HitNum_Map = HRes_Map.HitNum;
                int HitNum_Model_01 = Model_01.HitNum;
                int HitNum_Model_02 = Model_02.HitNum;
                int HitNum_Model_03 = Model_03.HitNum;
                int HitNum_Model_04 = Model_03.HitNum;
                int HitNum_Model_05 = Model_03.HitNum;
                DX.MV1CollResultPolyDimTerminate(HRes_Map);
                DX.MV1CollResultPolyDimTerminate(Model_01);
                DX.MV1CollResultPolyDimTerminate(Model_02);
                DX.MV1CollResultPolyDimTerminate(Model_03);
                DX.MV1CollResultPolyDimTerminate(Model_04);
                DX.MV1CollResultPolyDimTerminate(Model_05);
                if (HitNum_Map != 0 || HitNum_Model_01 != 0 || HitNum_Model_02 != 0 || HitNum_Model_03 != 0 || HitNum_Model_04 != 0 || HitNum_Model_05 != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (IsPhysicsSet)
            {
                //マップの当たり判定のみ
                DX.MV1_COLL_RESULT_POLY_DIM HRes_Map;
                HRes_Map = DX.MV1CollCheck_Capsule(MapHandle, -1, CameraPosition, CameraPosition, 1.3f);
                int HitNum_Map = HRes_Map.HitNum;
                DX.MV1CollResultPolyDimTerminate(HRes_Map);
                if (HitNum_Map != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (IsModelPhysicsSet)
            {
                //モデルの当たり判定のみ
                DX.MV1_COLL_RESULT_POLY_DIM Model_01;
                DX.MV1_COLL_RESULT_POLY_DIM Model_02;
                DX.MV1_COLL_RESULT_POLY_DIM Model_03;
                DX.MV1_COLL_RESULT_POLY_DIM Model_04;
                DX.MV1_COLL_RESULT_POLY_DIM Model_05;
                Model_01 = DX.MV1CollCheck_Capsule(Model_Physics_00, -1, CameraPosition, CameraPosition, 1.3f);
                Model_02 = DX.MV1CollCheck_Capsule(Model_Physics_01, -1, CameraPosition, CameraPosition, 1.3f);
                Model_03 = DX.MV1CollCheck_Capsule(Model_Physics_02, -1, CameraPosition, CameraPosition, 1.3f);
                Model_04 = DX.MV1CollCheck_Capsule(Model_Physics_03, -1, CameraPosition, CameraPosition, 1.3f);
                Model_05 = DX.MV1CollCheck_Capsule(Model_Physics_04, -1, CameraPosition, CameraPosition, 1.3f);
                int HitNum_Model_01 = Model_01.HitNum;
                int HitNum_Model_02 = Model_02.HitNum;
                int HitNum_Model_03 = Model_03.HitNum;
                int HitNum_Model_04 = Model_03.HitNum;
                int HitNum_Model_05 = Model_03.HitNum;
                DX.MV1CollResultPolyDimTerminate(Model_01);
                DX.MV1CollResultPolyDimTerminate(Model_02);
                DX.MV1CollResultPolyDimTerminate(Model_03);
                DX.MV1CollResultPolyDimTerminate(Model_04);
                DX.MV1CollResultPolyDimTerminate(Model_05);
                if (HitNum_Model_01 != 0 || HitNum_Model_02 != 0 || HitNum_Model_03 != 0 || HitNum_Model_04 != 0 || HitNum_Model_05 != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        void Pitch_Set_Void()
        {
            //Setting.datの設定のピッチを反映
            double Pitch = First_Music_Pitch * Pitch_Set;
            if (IsWMPUse)
            {
                Player.settings.rate = Pitch_Set;
            }
            else
            {
                DX.SetFrequencySoundMem((int)Pitch, MusicHandle);
            }
            PlayTime_Plus = PlayTime_Plus_Temp * PlayTime_Plus_Temp_Kakeru;
        }
        void Chara_Select_Draw(int ModelHandle)
        {
            //TPSモードのカメラ位置を指定
            MoveAnimFrameIndex = DX.MV1SearchFrame(ModelHandle, "頭");
            Chara_X_0 = DX.MV1GetFramePosition(ModelHandle, MoveAnimFrameIndex).x;
            Chara_Y_0 = DX.MV1GetFramePosition(ModelHandle, MoveAnimFrameIndex).y;
            Chara_Z_0 = DX.MV1GetFramePosition(ModelHandle, MoveAnimFrameIndex).z;
            Camera_Location = DX.MV1GetFramePosition(ModelHandle, MoveAnimFrameIndex);
            CameraLookAtPosition = DX.MV1GetFramePosition(ModelHandle, MoveAnimFrameIndex);
        }
    }
}