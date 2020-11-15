#include "DxLib.h"
#include "Function.h"
#include <math.h>
#include <string>
#include "tinyxml2.h"
#include "D:\Downloads\Google Downloads\Bass_C++\c\bass.h"
#include "D:\Downloads\Google Downloads\Bass_C++\c\bass_fx.h"

using namespace tinyxml2;
using String = std::string;

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
int ModelHandle_Sub_00;
int ModelHandle_Sub_01;
int ModelHandle_Sub_02;
int ModelHandle_Sub_03;
int ModelHandle_Sub_04;
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
int w;
int h;
int Test = 0;
int Stream;
float FPS_Kakeru = 1.0f;
float x = 0.0f;
float y = 0.0f;
float z = 0.0f;
float angleY = 0.0f;
float cameraX = 0.0f;
float cameraY = 3.0f;
float cameraZ = -5.0f;
float targetX = 0.0f;
float targetY = 0.0f;
float targetZ = 0.0f;
float Camera_Distance = 10.0f;
float Camera_Side = 0.2f;
float Camera_Tall = 0.0f;
float Model_01_Position_X = 0.0f;
float Model_02_Position_X = 0.0f;
float Model_03_Position_X = 0.0f;
float Model_04_Position_X = 0.0f;
float Model_05_Position_X = 0.0f;
float Model_Distance = 0.0f;
float Chara_X_0 = 0.0f, Chara_Y_0 = 0.0f, Chara_Z_0 = 0.0f;
float Stop_Play_Time;
float Linear_Plus = 0.0f;
float Linear_Not_VMD = 1.0f;
float TotalTime;
float PlayTime;
float PlayTime_Plus = 0.75f;
float PlayTime_Plus_Temp = 0.75f;
float PlayTime_Plus_Temp_Kakeru = 1.0f;
float Model_01_X, Model_01_Y, Model_01_Z;
float Model_02_X, Model_02_Y, Model_02_Z;
float Model_03_X, Model_03_Y, Model_03_Z;
float Model_04_X, Model_04_Y, Model_04_Z;
float Model_05_X, Model_05_Y, Model_05_Z;
float Music_BASS_Pitch = 0.0f;
bool FPS_Camera = false;
bool Music_Mute = false;
bool Camera_VMD = true;
bool Stop = false;
bool IsHorror_Mode_And_Map_0 = false;
bool IsModelPositionDistance = false;
bool IsModelPositionSet = false;
bool IsFrameRateLock = false;
bool IsMusicNotChange = false;
bool IsClosing = false;
bool IsPhysicsSet = true;
bool IsModelPhysicsSet = true;
bool IsShadowMode = true;
bool IsBASSMode = false;
bool IsOpacityChange = false;
bool IsOpacityChange_Down = false;
bool IsOpacityChange_Up = false;
double Pitch_Set = 1.0;
double Pitch_Change_Size = 0.1;
double Pitch_Min = 0.3;
double Pitch_Max = 1.6;
long Stop_Time = 0;
char cdir[255];
VECTOR Model_Position_01;
VECTOR Model_Position_02;
VECTOR Model_Position_03;
VECTOR Model_Position_04;
VECTOR Model_Position_05;
VECTOR TempPosition1;
VECTOR TempPosition2;
VECTOR CameraPosition;
VECTOR CameraLookAtPosition;
VECTOR Camera_Location;
void Pitch_Set_Void()
{
    //Setting.datの設定のピッチを反映
    double Pitch = First_Music_Pitch * Pitch_Set;
    SetFrequencySoundMem((int)Pitch, MusicHandle);
    PlayTime_Plus = PlayTime_Plus_Temp * PlayTime_Plus_Temp_Kakeru;
}
void CollRef()
{
    //当たり判定用のモデルの位置を更新
    MV1RefreshCollInfo(Model_Physics_00, -1);
    MV1RefreshCollInfo(Model_Physics_01, -1);
    MV1RefreshCollInfo(Model_Physics_02, -1);
    MV1RefreshCollInfo(Model_Physics_03, -1);
    MV1RefreshCollInfo(Model_Physics_04, -1);
}
bool CameraPositionCollCheck(VECTOR CameraPosition)
{
    if (IsPhysicsSet && IsModelPhysicsSet)
    {
        //当たり判定(すべて)
        MV1_COLL_RESULT_POLY_DIM HRes_Map;
        MV1_COLL_RESULT_POLY_DIM Model_01;
        MV1_COLL_RESULT_POLY_DIM Model_02;
        MV1_COLL_RESULT_POLY_DIM Model_03;
        MV1_COLL_RESULT_POLY_DIM Model_04;
        MV1_COLL_RESULT_POLY_DIM Model_05;
        HRes_Map = MV1CollCheck_Capsule(MapHandle, -1, CameraPosition, CameraPosition, 1.3f);
        Model_01 = MV1CollCheck_Capsule(Model_Physics_00, -1, CameraPosition, CameraPosition, 1.3f);
        Model_02 = MV1CollCheck_Capsule(Model_Physics_01, -1, CameraPosition, CameraPosition, 1.3f);
        Model_03 = MV1CollCheck_Capsule(Model_Physics_02, -1, CameraPosition, CameraPosition, 1.3f);
        Model_04 = MV1CollCheck_Capsule(Model_Physics_03, -1, CameraPosition, CameraPosition, 1.3f);
        Model_05 = MV1CollCheck_Capsule(Model_Physics_04, -1, CameraPosition, CameraPosition, 1.3f);
        int HitNum_Map = HRes_Map.HitNum;
        int HitNum_Model_01 = Model_01.HitNum;
        int HitNum_Model_02 = Model_02.HitNum;
        int HitNum_Model_03 = Model_03.HitNum;
        int HitNum_Model_04 = Model_03.HitNum;
        int HitNum_Model_05 = Model_03.HitNum;
        MV1CollResultPolyDimTerminate(HRes_Map);
        MV1CollResultPolyDimTerminate(Model_01);
        MV1CollResultPolyDimTerminate(Model_02);
        MV1CollResultPolyDimTerminate(Model_03);
        MV1CollResultPolyDimTerminate(Model_04);
        MV1CollResultPolyDimTerminate(Model_05);
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
        MV1_COLL_RESULT_POLY_DIM HRes_Map;
        HRes_Map = MV1CollCheck_Capsule(MapHandle, -1, CameraPosition, CameraPosition, 1.3f);
        int HitNum_Map = HRes_Map.HitNum;
        MV1CollResultPolyDimTerminate(HRes_Map);
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
        MV1_COLL_RESULT_POLY_DIM Model_01;
        MV1_COLL_RESULT_POLY_DIM Model_02;
        MV1_COLL_RESULT_POLY_DIM Model_03;
        MV1_COLL_RESULT_POLY_DIM Model_04;
        MV1_COLL_RESULT_POLY_DIM Model_05;
        Model_01 = MV1CollCheck_Capsule(Model_Physics_00, -1, CameraPosition, CameraPosition, 1.3f);
        Model_02 = MV1CollCheck_Capsule(Model_Physics_01, -1, CameraPosition, CameraPosition, 1.3f);
        Model_03 = MV1CollCheck_Capsule(Model_Physics_02, -1, CameraPosition, CameraPosition, 1.3f);
        Model_04 = MV1CollCheck_Capsule(Model_Physics_03, -1, CameraPosition, CameraPosition, 1.3f);
        Model_05 = MV1CollCheck_Capsule(Model_Physics_04, -1, CameraPosition, CameraPosition, 1.3f);
        int HitNum_Model_01 = Model_01.HitNum;
        int HitNum_Model_02 = Model_02.HitNum;
        int HitNum_Model_03 = Model_03.HitNum;
        int HitNum_Model_04 = Model_03.HitNum;
        int HitNum_Model_05 = Model_03.HitNum;
        MV1CollResultPolyDimTerminate(Model_01);
        MV1CollResultPolyDimTerminate(Model_02);
        MV1CollResultPolyDimTerminate(Model_03);
        MV1CollResultPolyDimTerminate(Model_04);
        MV1CollResultPolyDimTerminate(Model_05);
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
void Shadow_Draw()
{
    //影の描画
    float Min_X;
    float Min_Z;
    float Max_X;
    float Max_Z;
    int Frame_01 = MV1SearchFrame(ModelHandle_00, "頭");
    int Frame_02 = MV1SearchFrame(ModelHandle_01, "頭");
    int Frame_03 = MV1SearchFrame(ModelHandle_02, "頭");
    int Frame_04 = MV1SearchFrame(ModelHandle_03, "頭");
    int Frame_05 = MV1SearchFrame(ModelHandle_04, "頭");
    float Model_Position_01_X = MV1GetFramePosition(ModelHandle_00, Frame_01).x;
    float Model_Position_01_Z = MV1GetFramePosition(ModelHandle_00, Frame_01).z;
    float Model_Position_02_X = MV1GetFramePosition(ModelHandle_01, Frame_02).x;
    float Model_Position_02_Z = MV1GetFramePosition(ModelHandle_01, Frame_02).z;
    float Model_Position_03_X = MV1GetFramePosition(ModelHandle_02, Frame_03).x;
    float Model_Position_03_Z = MV1GetFramePosition(ModelHandle_02, Frame_03).z;
    float Model_Position_04_X = MV1GetFramePosition(ModelHandle_03, Frame_04).x;
    float Model_Position_04_Z = MV1GetFramePosition(ModelHandle_03, Frame_04).z;
    float Model_Position_05_X = MV1GetFramePosition(ModelHandle_04, Frame_05).x;
    float Model_Position_05_Z = MV1GetFramePosition(ModelHandle_04, Frame_05).z;
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
    SetShadowMapDrawArea(Model_Shadow_00, VGet(Min_X - 20.0f, -1.0f, Min_Z - 20.0f), VGet(Max_X + 20.0f, 30.0f, Max_Z + 20.0f));
    ShadowMap_DrawSetup(Model_Shadow_00);
    MV1DrawModel(ModelHandle_00);
    MV1DrawModel(ModelHandle_01);
    MV1DrawModel(ModelHandle_02);
    MV1DrawModel(ModelHandle_03);
    MV1DrawModel(ModelHandle_04);
    ShadowMap_DrawEnd();
    SetUseShadowMap(0, Model_Shadow_00);
    SetUseShadowMap(1, Map_Shadow_00);
}
class Fps 
{
    int mStartTime;
    int mCount;
    float mFps;

public:
    Fps() 
    {
        mStartTime = 0;
        mCount = 0;
        mFps = 0;
    }
    bool Update() 
    {
        if (mCount == 0) 
        {
            mStartTime = GetNowCount();
        }
        if (mCount >= FPS) 
        {
            int t = GetNowCount();
            mFps = 1000.0f / ((t - mStartTime) / (float)FPS);
            mCount = 0;
            mStartTime = t;
        }
        mCount++;
        return true;
    }
    void Draw() 
    {
        DrawFormatString(0, 0, GetColor(255, 255, 255), "%.1f", mFps);
    }
    void Wait() 
    {
        //指定フレームまで待つ
        int tookTime = GetNowCount() - mStartTime;
        int waitTime = mCount * 1000 / FPS - tookTime;
        if (waitTime > 0) {
            Sleep(waitTime);
        }
    }
};
void Chara_Select_Draw(int ModelHandle)
{
    //TPSモードのカメラ位置を指定
    MoveAnimFrameIndex = MV1SearchFrame(ModelHandle, "頭");
    Chara_X_0 = MV1GetFramePosition(ModelHandle, MoveAnimFrameIndex).x;
    Chara_Y_0 = MV1GetFramePosition(ModelHandle, MoveAnimFrameIndex).y;
    Chara_Z_0 = MV1GetFramePosition(ModelHandle, MoveAnimFrameIndex).z;
    Camera_Location = MV1GetFramePosition(ModelHandle, MoveAnimFrameIndex);
    CameraLookAtPosition = MV1GetFramePosition(ModelHandle, MoveAnimFrameIndex);
}
void SetupVMDCameraMotionParam(int CameraHandle, float Time)
{
    //MMDのカメラを反映(なぜか反映されないときがある)
    MATRIX VRotMat, HRotMat, MixRotMat, TwistRotMat;
    VECTOR CamLoc, CamDir;
    VECTOR Location, Rotation, CamUp;
    float Length, ViewAngle;
    Location = MV1GetAnimKeyDataToVectorFromTime(CameraHandle, 0, Time);
    Rotation = MV1GetAnimKeyDataToVectorFromTime(CameraHandle, 1, Time);
    Length = MV1GetAnimKeyDataToLinearFromTime(CameraHandle, 2, Time);
    ViewAngle = MV1GetAnimKeyDataToLinearFromTime(CameraHandle, 3, Time);
    VRotMat = MGetRotX(-Rotation.x);
    HRotMat = MGetRotY(-Rotation.y);
    MixRotMat = MMult(VRotMat, HRotMat);
    CamDir = VTransform(VGet(0.0f, 0.0f, 1.0f), MixRotMat);
    TwistRotMat = MGetRotAxis(CamDir, -Rotation.z);
    MixRotMat = MMult(MixRotMat, TwistRotMat);
    CamLoc = VTransform(VGet(0.0f, 0.0f, Length), MixRotMat);
    CamLoc = VAdd(CamLoc, Location);
    CamUp = VTransform(VGet(0.0f, 1.0f, 0.0f), MixRotMat);
    Location = VAdd(CamLoc, CamDir);
    SetupCamera_Perspective((ViewAngle / 180.0f * 3.141592f) + Linear_Plus);
    SetCameraPositionAndTargetAndUpVec(CamLoc, Location, CamUp);
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
        float Opacity = MV1GetOpacityRate(ModelHandle_00);
        if (Opacity <= 0.0f)
        {
            IsOpacityChange_Down = false;
            IsOpacityChange_Up = true;
            Model_Move();
        }
        else
        {
            MV1SetOpacityRate(ModelHandle_00, Opacity - (float)(0.05 * FPS_Kakeru));
            MV1SetOpacityRate(ModelHandle_01, Opacity - (float)(0.05 * FPS_Kakeru));
            MV1SetOpacityRate(ModelHandle_02, Opacity - (float)(0.05 * FPS_Kakeru));
            MV1SetOpacityRate(ModelHandle_03, Opacity - (float)(0.05 * FPS_Kakeru));
            MV1SetOpacityRate(ModelHandle_04, Opacity - (float)(0.05 * FPS_Kakeru));
        }
    }
    else if (IsOpacityChange_Up)
    {
        float Opacity = MV1GetOpacityRate(ModelHandle_00);
        if (Opacity >= 1.0f)
        {
            IsOpacityChange_Up = false;
            IsOpacityChange = false;
        }
        else
        {
            MV1SetOpacityRate(ModelHandle_00, Opacity + (float)(0.05 * FPS_Kakeru));
            MV1SetOpacityRate(ModelHandle_01, Opacity + (float)(0.05 * FPS_Kakeru));
            MV1SetOpacityRate(ModelHandle_02, Opacity + (float)(0.05 * FPS_Kakeru));
            MV1SetOpacityRate(ModelHandle_03, Opacity + (float)(0.05 * FPS_Kakeru));
            MV1SetOpacityRate(ModelHandle_04, Opacity + (float)(0.05 * FPS_Kakeru));
        }
    }
}
void Dance_Loop()
{
    //メインコード
    CameraLookAtPosition.x = 0;
    CameraLookAtPosition.y = 0;
    CameraLookAtPosition.z = 0;
    float CameraVAngle = 0.0f;
    float FPS_CameraHAngle = 0.0f;
    float FPS_CameraVAngle = 0.0f;
    float SinParam = 0.0f;
    float CosParam = 0.0f;
    AttachIndex_00 = MV1AttachAnim(ModelHandle_00, 0, -1, FALSE);
    AttachIndex_01 = MV1AttachAnim(ModelHandle_01, 0, -1, FALSE);
    AttachIndex_02 = MV1AttachAnim(ModelHandle_02, 0, -1, FALSE);
    AttachIndex_03 = MV1AttachAnim(ModelHandle_03, 0, -1, FALSE);
    AttachIndex_04 = MV1AttachAnim(ModelHandle_04, 0, -1, FALSE);
    TotalTime = MV1GetAttachAnimTotalTime(ModelHandle_00, AttachIndex_00);
    PlayTime = 2.0f;
    Fps fps;
    SetUseZBuffer3D(TRUE);
    SetWriteZBuffer3D(TRUE);
    //ループ
    while (ProcessMessage() == 0)
    {
        //Escapeキーで終了
        if (CheckHitKey(KEY_INPUT_ESCAPE) != 0)
        {
            break;
        }
        //fpsを更新
        fps.Update();
        ClearDrawScreen();
        //処理落ち対策
        if (IsFrameRateLock)
        {
            if (IsBASSMode)
            {
                double Now = BASS_ChannelBytes2Seconds(Stream, BASS_ChannelGetPosition(Stream, BASS_POS_BYTE));
                PlayTime = (float)Now * 30;
            }
            else
            {
                PlayTime = (float)GetSoundCurrentTime(MusicHandle) * 3 / 100;
            }
        }
        else
        {
            PlayTime += PlayTime_Plus;
        }
        //マウスの移動量を取得
        int Position_X_Now = 0;
        int Position_Y_Now = 0;
        GetMousePoint(&Position_X_Now, &Position_Y_Now);
        //1フレームあたりのマウスの移動距離を取得
        int Move_MouseX = (int)Position_X_Now - (int)w / 2;
        int Move_MouseY = (int)Position_Y_Now - (int)h / 2;
        //ウィンドウがアクティブか
        bool ActiveFlag = (GetMainWindowHandle() == GetForegroundWindow());
        if (PlayTime >= TotalTime)
        {
            //曲が終わったら最初から
            PlayTime = 0.0f;
            Stop = false;
            if (IsBASSMode)
            {
                __int64 Time2 = BASS_ChannelSeconds2Bytes(Stream, 0);
                BASS_Start();
                BASS_ChannelSetPosition(Stream, Time2, BASS_POS_BYTE);
            }
            else
            {
                //LONGLONG(1)以上にしないと曲の再生位置がおかしくなる
                StopSoundMem(MusicHandle);
                SetSoundCurrentPosition(static_cast<LONGLONG>(1), MusicHandle);
                PlaySoundMem(MusicHandle, DX_PLAYTYPE_LOOP, FALSE);
            }
        }
        //5秒ごとにモーションの再生位置を修正
        if (IsBASSMode && !Stop && !IsFrameRateLock)
        {
            if ((int)PlayTime % 300 == 0)
            {
                double Now = BASS_ChannelBytes2Seconds(Stream, BASS_ChannelGetPosition(Stream, BASS_POS_BYTE));
                PlayTime = (float)Now * 30;
            }
        }
        else if (!Stop && !IsFrameRateLock)
        {
            if ((int)PlayTime % 300 == 0)
            {
                PlayTime = (float)GetSoundCurrentTime(MusicHandle) * 3 / 100;
            }
        }
        if (FPS_Camera == false && Camera_VMD == false)
        {
            //キャラクターの当たり判定の位置を更新
            if (IsModelPhysicsSet)
            {
                int Frame_01 = MV1SearchFrame(ModelHandle_00, "頭");
                float Model_01_X = MV1GetFramePosition(ModelHandle_00, Frame_01).x;
                float Model_01_Z = MV1GetFramePosition(ModelHandle_00, Frame_01).z;
                int Frame_02 = MV1SearchFrame(ModelHandle_01, "頭");
                float Model_02_X = MV1GetFramePosition(ModelHandle_01, Frame_02).x;
                float Model_02_Z = MV1GetFramePosition(ModelHandle_01, Frame_02).z;
                int Frame_03 = MV1SearchFrame(ModelHandle_02, "頭");
                float Model_03_X = MV1GetFramePosition(ModelHandle_02, Frame_03).x;
                float Model_03_Z = MV1GetFramePosition(ModelHandle_02, Frame_03).z;
                int Frame_04 = MV1SearchFrame(ModelHandle_03, "頭");
                float Model_04_X = MV1GetFramePosition(ModelHandle_03, Frame_04).x;
                float Model_04_Z = MV1GetFramePosition(ModelHandle_03, Frame_04).z;
                int Frame_05 = MV1SearchFrame(ModelHandle_04, "頭");
                float Model_05_X = MV1GetFramePosition(ModelHandle_04, Frame_04).x;
                float Model_05_Z = MV1GetFramePosition(ModelHandle_04, Frame_05).z;
                MV1SetPosition(Model_Physics_00, VGet(Model_01_X, Model_01_Y, Model_01_Z));
                MV1SetPosition(Model_Physics_01, VGet(Model_02_X, Model_02_Y, Model_02_Z));
                MV1SetPosition(Model_Physics_02, VGet(Model_03_X, Model_03_Y, Model_03_Z));
                MV1SetPosition(Model_Physics_03, VGet(Model_04_X, Model_04_Y, Model_04_Z));
                MV1SetPosition(Model_Physics_04, VGet(Model_05_X, Model_05_Y, Model_05_Z));
                CollRef();
            }
            //TPSモードでなく、MMDのカメラでもない場合のカメラの移動
            if (CheckHitKey(KEY_INPUT_W) > 0)
            {
                if (IsPhysicsSet || IsModelPhysicsSet)
                {
                    //CameraPositionCollCheckがtrueだった場合のみカメラを移動
                    float X_Temp = (float)sin(-angleY) * 0.125f * FPS_Kakeru;
                    float Z_Temp = (float)cos(-angleY) * 0.125f * FPS_Kakeru;
                    if (CameraPositionCollCheck(VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                    {
                        x += (float)sin(-angleY) * 0.05f * FPS_Kakeru;
                    }
                    if (CameraPositionCollCheck(VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                    {
                        z += (float)cos(-angleY) * 0.05f * FPS_Kakeru;
                    }
                }
                else
                {
                    x += (float)sin(-angleY) * 0.05f * FPS_Kakeru;
                    z += (float)cos(-angleY) * 0.05f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_A) > 0)
            {
                if (IsPhysicsSet || IsModelPhysicsSet)
                {
                    float X_Temp = (float)sin(-static_cast<double>(angleY) - 1.5f) * 0.125f * FPS_Kakeru;
                    float Z_Temp = (float)cos(-static_cast<double>(angleY) - 1.5f) * 0.125f * FPS_Kakeru;
                    if (CameraPositionCollCheck(VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                    {
                        x += (float)sin(-static_cast<double>(angleY) - 1.5f) * 0.05f * FPS_Kakeru;
                    }
                    if (CameraPositionCollCheck(VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                    {
                        z += (float)cos(-static_cast<double>(angleY) - 1.5f) * 0.05f * FPS_Kakeru;
                    }
                }
                else
                {
                    x += (float)sin(-static_cast<double>(angleY) - 1.5f) * 0.05f * FPS_Kakeru;
                    z += (float)cos(-static_cast<double>(angleY) - 1.5f) * 0.05f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_S) > 0)
            {
                if (IsPhysicsSet || IsModelPhysicsSet)
                {
                    float X_Temp = (float)sin(-angleY) * -0.125f * FPS_Kakeru;
                    float Z_Temp = (float)cos(-angleY) * -0.125f * FPS_Kakeru;
                    if (CameraPositionCollCheck(VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                    {
                        x += (float)sin(-angleY) * -0.05f * FPS_Kakeru;
                    }
                    if (CameraPositionCollCheck(VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                    {
                        z += (float)cos(-angleY) * -0.05f * FPS_Kakeru;
                    }
                }
                else
                {
                    x += (float)sin(-angleY) * -0.05f * FPS_Kakeru;
                    z += (float)cos(-angleY) * -0.05f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_D) > 0)
            {
                if (IsPhysicsSet || IsModelPhysicsSet)
                {
                    float X_Temp = (float)sin(-static_cast<double>(angleY) - 1.5f) * -0.125f * FPS_Kakeru;
                    float Z_Temp = (float)cos(-static_cast<double>(angleY) - 1.5f) * -0.125f * FPS_Kakeru;
                    if (CameraPositionCollCheck(VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                    {
                        x += (float)sin(-static_cast<double>(angleY) - 1.5f) * -0.05f * FPS_Kakeru;
                    }
                    if (CameraPositionCollCheck(VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                    {
                        z += (float)cos(-static_cast<double>(angleY) - 1.5f) * -0.05f * FPS_Kakeru;
                    }
                }
                else
                {
                    x += (float)sin(-static_cast<double>(angleY) - 1.5f) * -0.05f * FPS_Kakeru;
                    z += (float)cos(-static_cast<double>(angleY) - 1.5f) * -0.05f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_W) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
            {
                if (IsPhysicsSet || IsModelPhysicsSet)
                {
                    float X_Temp = (float)sin(-angleY) * 0.125f * FPS_Kakeru;
                    float Z_Temp = (float)cos(-angleY) * 0.125f * FPS_Kakeru;
                    if (CameraPositionCollCheck(VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                    {
                        x += (float)sin(-angleY) * 0.125f * FPS_Kakeru;
                    }
                    if (CameraPositionCollCheck(VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                    {
                        z += (float)cos(-angleY) * 0.125f * FPS_Kakeru;
                    }
                }
                else
                {
                    x += (float)sin(-angleY) * 0.125f * FPS_Kakeru;
                    z += (float)cos(-angleY) * 0.125f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_A) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
            {
                if (IsPhysicsSet || IsModelPhysicsSet)
                {
                    float X_Temp = (float)sin(-static_cast<double>(angleY) - 1.5f) * 0.125f * FPS_Kakeru;
                    float Z_Temp = (float)cos(-static_cast<double>(angleY) - 1.5f) * 0.125f * FPS_Kakeru;
                    if (CameraPositionCollCheck(VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                    {
                        x += (float)sin(-static_cast<double>(angleY) - 1.5f) * 0.125f * FPS_Kakeru;
                    }
                    if (CameraPositionCollCheck(VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                    {
                        z += (float)cos(-static_cast<double>(angleY) - 1.5f) * 0.125f * FPS_Kakeru;
                    }
                }
                else
                {
                    x += (float)sin(-static_cast<double>(angleY) - 1.5f) * 0.125f * FPS_Kakeru;
                    z += (float)cos(-static_cast<double>(angleY) - 1.5f) * 0.125f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_S) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
            {
                if (IsPhysicsSet || IsModelPhysicsSet)
                {
                    float X_Temp = (float)sin(-angleY) * -0.125f * FPS_Kakeru;
                    float Z_Temp = (float)cos(-angleY) * -0.125f * FPS_Kakeru;
                    if (CameraPositionCollCheck(VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                    {
                        x += (float)sin(-angleY) * -0.125f * FPS_Kakeru;
                    }
                    if (CameraPositionCollCheck(VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                    {
                        z += (float)cos(-angleY) * -0.125f * FPS_Kakeru;
                    }
                }
                else
                {
                    x += (float)sin(-angleY) * -0.125f * FPS_Kakeru;
                    z += (float)cos(-angleY) * -0.125f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_D) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
            {
                if (IsPhysicsSet || IsModelPhysicsSet)
                {
                    float X_Temp = (float)sin(-static_cast<double>(angleY) - 1.5f) * -0.125f * FPS_Kakeru;
                    float Z_Temp = (float)cos(-static_cast<double>(angleY) - 1.5f) * -0.125f * FPS_Kakeru;
                    if (CameraPositionCollCheck(VGet(cameraX + ((x + X_Temp) * 10), cameraY + (y * 10), cameraZ + (z * 10))))
                    {
                        x += (float)sin(-static_cast<double>(angleY) - 1.5f) * -0.125f * FPS_Kakeru;
                    }
                    if (CameraPositionCollCheck(VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + ((z + Z_Temp) * 10))))
                    {
                        z += (float)cos(-static_cast<double>(angleY) - 1.5f) * -0.125f * FPS_Kakeru;
                    }
                }
                else
                {
                    x += (float)sin(-static_cast<double>(angleY) - 1.5f) * -0.125f * FPS_Kakeru;
                    z += (float)cos(-static_cast<double>(angleY) - 1.5f) * -0.125f * FPS_Kakeru;
                }
            }
        }
        else
        {
            //TPSモードだった場合
            if (CheckHitKey(KEY_INPUT_W) > 0)
            {
                if (Camera_Tall <= 2.0f)
                {
                    Camera_Tall += 0.5f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_A) > 0)
            {
                if (Camera_Side >= -0.8f)
                {
                    Camera_Side -= 0.2f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_S) > 0)
            {
                if (Camera_Tall >= -15.0f)
                {
                    Camera_Tall -= 0.5f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_D) > 0)
            {
                if (Camera_Side <= 2.0f)
                {
                    Camera_Side += 0.2f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_W) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
            {
                if (Camera_Tall <= 2.0f)
                {
                    Camera_Tall += 0.5f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_A) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
            {
                if (Camera_Side >= -0.8f)
                {
                    Camera_Side -= 0.35f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_S) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
            {
                if (Camera_Tall >= -15.0f)
                {
                    Camera_Tall -= 0.5f * FPS_Kakeru;
                }
            }
            if (CheckHitKey(KEY_INPUT_D) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
            {
                if (Camera_Side <= 2.0f)
                {
                    Camera_Side += 0.35f * FPS_Kakeru;
                }
            }
        }
        if (!CameraPositionCollCheck(VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + (z * 10))))
        {
            //カメラがモデルと重なっていたらzを変更
            z += (float)cos(-static_cast<double>(angleY) - 1.5f) * -0.125f * FPS_Kakeru;
        }
        //カメラ関連
        if (CheckHitKey(KEY_INPUT_LSHIFT) > 0)
        {
            if (FPS_Camera == true)
            {
                if (Camera_Distance >= 5.0f)
                {
                    Camera_Distance -= 0.5f * FPS_Kakeru;
                }
            }
            else
            {
                if (CameraVAngle >= -50.0f)
                {
                    CameraVAngle -= 1.0f * FPS_Kakeru;
                }
                float Y_Temp = GetCameraPosition().y - 0.5f * FPS_Kakeru;
                if (CameraPositionCollCheck(VGet(cameraX + (x * 10), Y_Temp + (y * 10), cameraZ + (z * 10))))
                {
                    cameraY = GetCameraPosition().y - 0.5f * FPS_Kakeru;
                }
            }
        }
        if (CheckHitKey(KEY_INPUT_SPACE) > 0)
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
                if (CameraVAngle <= 50.0f)
                {
                    CameraVAngle += 1.0f * FPS_Kakeru;
                }
                float Y_Temp = GetCameraPosition().y + 0.5f * FPS_Kakeru;
                if (CameraPositionCollCheck(VGet(cameraX + (x * 10), Y_Temp + (y * 10), cameraZ + (z * 10))))
                {
                    cameraY = GetCameraPosition().y + 0.5f * FPS_Kakeru;
                }
            }
        }
        if (CheckHitKey(KEY_INPUT_LSHIFT) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
        {
            if (FPS_Camera == true)
            {
                if (Camera_Distance >= 5.0f)
                {
                    Camera_Distance -= 1.0f * FPS_Kakeru;
                }
            }
            else
            {
                if (CameraVAngle >= -50.0f)
                {
                    CameraVAngle -= 2.0f * FPS_Kakeru;
                }
                float Y_Temp = GetCameraPosition().y - 1.25f * FPS_Kakeru;
                if (CameraPositionCollCheck(VGet(cameraX + (x * 10), Y_Temp + (y * 10), cameraZ + (z * 10))))
                {
                    cameraY = GetCameraPosition().y - 1.25f * FPS_Kakeru;
                }
            }
        }
        if (CheckHitKey(KEY_INPUT_SPACE) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
        {
            if (FPS_Camera == true)
            {
                if (Camera_Distance <= 60.0)
                {
                    Camera_Distance += 1.0f * FPS_Kakeru;
                }
            }
            else
            {
                if (CameraVAngle <= 50.0f)
                {
                    CameraVAngle += 2.0f * FPS_Kakeru;
                }
                float Y_Temp = GetCameraPosition().y + 1.25f * FPS_Kakeru;
                if (CameraPositionCollCheck(VGet(cameraX + (x * 10), Y_Temp + (y * 10), cameraZ + (z * 10))))
                {
                    cameraY = GetCameraPosition().y + 1.25f * FPS_Kakeru;
                }
            }
        }
        if (CheckHitKey(KEY_INPUT_LEFT) > 0)
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
        if (CheckHitKey(KEY_INPUT_RIGHT) > 0)
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
        if (CheckHitKey(KEY_INPUT_UP) > 0)
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
                    if (CameraVAngle <= 5.0f)
                    {
                        CameraVAngle += 0.1f * FPS_Kakeru;
                    }
                    Linear_Plus -= 0.005f * FPS_Kakeru;
                }
            }
        }
        if (CheckHitKey(KEY_INPUT_DOWN) > 0)
        {
            if (FPS_Camera == true)
            {
                if (Linear_Not_VMD < 2.0f)
                {
                    Linear_Not_VMD += 0.005f * FPS_Kakeru;
                }
            }
            else
            {
                if (Camera_VMD == false)
                {
                    if (Linear_Not_VMD < 2.0f)
                    {
                        Linear_Not_VMD += 0.005f * FPS_Kakeru;
                    }
                }
                else
                {
                    if (CameraVAngle >= -5.0f)
                    {
                        CameraVAngle -= 0.1f * FPS_Kakeru;
                    }
                    Linear_Plus += 0.005f * FPS_Kakeru;
                }
            }
        }
        if (CheckHitKey(KEY_INPUT_LEFT) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
        {
            if (FPS_Camera == false && Camera_VMD == false)
            {
                targetZ += 0.04f * FPS_Kakeru;
            }
            else
            {
                FPS_CameraHAngle -= 4.0f * FPS_Kakeru;
            }
        }
        if (CheckHitKey(KEY_INPUT_RIGHT) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
        {
            if (FPS_Camera == false && Camera_VMD == false)
            {
                targetZ -= 0.04f * FPS_Kakeru;
            }
            else
            {
                FPS_CameraHAngle += 4.0f * FPS_Kakeru;
            }
        }
        if (CheckHitKey(KEY_INPUT_UP) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
        {
            if (FPS_Camera == false)
            {
                if (Camera_VMD == false)
                {
                    if (CameraVAngle <= 5.0f)
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
        if (CheckHitKey(KEY_INPUT_DOWN) > 0 && CheckHitKey(KEY_INPUT_LCONTROL) > 0)
        {
            if (FPS_Camera == false)
            {
                if (Camera_VMD == false)
                {
                    if (Linear_Not_VMD < 2.0f)
                    {
                        Linear_Not_VMD += 0.01f * FPS_Kakeru;
                    }
                }
                else
                {
                    if (CameraVAngle >= -5.0f)
                    {
                        CameraVAngle -= 0.2f * FPS_Kakeru;
                    }
                    Linear_Plus += 0.01f * FPS_Kakeru;
                }
            }
            else
            {
                if (Linear_Not_VMD < 2.0f)
                {
                    Linear_Not_VMD += 0.01f * FPS_Kakeru;
                }
            }
        }
        //ショートカット関連
        if (CheckHitKey(KEY_INPUT_K) > 0 && Stop == false)
        {
            //5,10秒進む
            K_KEY_Tap = K_KEY_Tap + 1;
            if (K_KEY_Tap == 1)
            {
                LONGLONG PlayTime_Music = GetSoundCurrentTime(MusicHandle);
                double Pos_Now = BASS_ChannelBytes2Seconds(Stream, BASS_ChannelGetPosition(Stream, BASS_POS_BYTE));
                if (CheckHitKey(KEY_INPUT_LCONTROL) != 0)
                {
                    if (IsBASSMode)
                    {
                        Pos_Now += 10;
                        __int64 BASS_Pos = BASS_ChannelSeconds2Bytes(Stream, Pos_Now);
                        BASS_ChannelSetPosition(Stream, BASS_Pos, BASS_POS_BYTE);
                        PlayTime = (float)Pos_Now * 30;
                    }
                    else
                    {
                        //警告が出るためstatic_cast<LONGLONG>を入れる
                        StopSoundMem(MusicHandle);
                        SetSoundCurrentTime(static_cast<LONGLONG>(PlayTime_Music) + 10000, MusicHandle);
                        PlaySoundMem(MusicHandle, DX_PLAYTYPE_LOOP, FALSE);
                        PlayTime = (float)GetSoundCurrentTime(MusicHandle) * 3 / 100;
                    }
                }
                else
                {
                    if (IsBASSMode)
                    {
                        Pos_Now += 5;
                        __int64 BASS_Pos = BASS_ChannelSeconds2Bytes(Stream, Pos_Now);
                        BASS_ChannelSetPosition(Stream, BASS_Pos, BASS_POS_BYTE);
                        PlayTime = (float)Pos_Now * 30;
                    }
                    else
                    {
                        StopSoundMem(MusicHandle);
                        SetSoundCurrentTime(static_cast<LONGLONG>(PlayTime_Music) + 5000, MusicHandle);
                        PlaySoundMem(MusicHandle, DX_PLAYTYPE_LOOP, FALSE);
                        PlayTime = (float)GetSoundCurrentTime(MusicHandle) * 3 / 100;
                    }
                }
            }
        }
        else
        {
            K_KEY_Tap = 0;
        }
        if (CheckHitKey(KEY_INPUT_J) > 0 && Stop == false)
        {
            //5,10秒戻る
            J_KEY_Tap = J_KEY_Tap + 1;
            if (J_KEY_Tap == 1)
            {
                LONGLONG PlayTime_Music = GetSoundCurrentTime(MusicHandle);
                double Pos_Now = BASS_ChannelBytes2Seconds(Stream, BASS_ChannelGetPosition(Stream, BASS_POS_BYTE));
                if (CheckHitKey(KEY_INPUT_LCONTROL) != 0)
                {
                    if (PlayTime >= 300.0f)
                    {
                        if (IsBASSMode)
                        {
                            Pos_Now -= 10;
                            __int64 BASS_Pos = BASS_ChannelSeconds2Bytes(Stream, Pos_Now);
                            BASS_ChannelSetPosition(Stream, BASS_Pos, BASS_POS_BYTE);
                            PlayTime = (float)Pos_Now * 30;
                        }
                        else
                        {
                            StopSoundMem(MusicHandle);
                            SetSoundCurrentTime(static_cast<LONGLONG>(PlayTime_Music) - 10000, MusicHandle);
                            PlaySoundMem(MusicHandle, DX_PLAYTYPE_LOOP, FALSE);
                            PlayTime = (float)GetSoundCurrentTime(MusicHandle) * 3 / 100;
                        }
                    }
                    else
                    {
                        if (IsBASSMode)
                        {
                            __int64 BASS_Pos = BASS_ChannelSeconds2Bytes(Stream, 0.25);
                            BASS_ChannelSetPosition(Stream, BASS_Pos, BASS_POS_BYTE);
                            PlayTime = 0.0f;
                        }
                        else
                        {
                            StopSoundMem(MusicHandle);
                            SetSoundCurrentTime(0, MusicHandle);
                            PlaySoundMem(MusicHandle, DX_PLAYTYPE_LOOP, FALSE);
                            PlayTime = 0.0f;
                        }
                    }
                }
                else
                {
                    if (PlayTime >= 150.0f)
                    {
                        if (IsBASSMode)
                        {
                            Pos_Now -= 5;
                            __int64 BASS_Pos = BASS_ChannelSeconds2Bytes(Stream, Pos_Now);
                            BASS_ChannelSetPosition(Stream, BASS_Pos, BASS_POS_BYTE);
                            PlayTime = (float)Pos_Now * 30;
                        }
                        else
                        {
                            StopSoundMem(MusicHandle);
                            SetSoundCurrentTime(static_cast<LONGLONG>(PlayTime_Music) - 5000, MusicHandle);
                            PlaySoundMem(MusicHandle, DX_PLAYTYPE_LOOP, FALSE);
                            PlayTime = (float)GetSoundCurrentTime(MusicHandle) * 3 / 100;
                        }
                    }
                    else
                    {
                        if (IsBASSMode)
                        {
                            __int64 BASS_Pos = BASS_ChannelSeconds2Bytes(Stream, 0.25);
                            BASS_ChannelSetPosition(Stream, BASS_Pos, BASS_POS_BYTE);
                            PlayTime = 0.0f;
                        }
                        else
                        {
                            StopSoundMem(MusicHandle);
                            SetSoundCurrentTime(0, MusicHandle);
                            PlaySoundMem(MusicHandle, DX_PLAYTYPE_LOOP, FALSE);
                            PlayTime = 0.0f;
                        }
                    }
                }
            }
        }
        else
        {
            J_KEY_Tap = 0;
        }
        if (CheckHitKey(KEY_INPUT_M) != 0 && CheckHitKey(KEY_INPUT_R) != 0)
        {
            //曲とモーションを合わせる
            M_R_Key_Down++;
            if (M_R_Key_Down == 1)
            {
                if (IsBASSMode)
                {
                    double Pos = BASS_ChannelBytes2Seconds(Stream, BASS_ChannelGetPosition(Stream, BASS_POS_BYTE));
                    PlayTime = (float)Pos * 30;
                }
                else
                {
                    PlayTime = (float)GetSoundCurrentTime(MusicHandle) * 3 / 100;
                }
            }
        }
        else
        {
            M_R_Key_Down = 0;
        }
        if (CheckHitKey(KEY_INPUT_F) != 0)
        {
            //キャラクター視点に切り替える
            F_KEY_Tap = F_KEY_Tap + 1;
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
        if (CheckHitKey(KEY_INPUT_LCONTROL) != 0 && CheckHitKey(KEY_INPUT_M) > 0)
        {
            //ミュート
            M_KEY_Tap = M_KEY_Tap + 1;
            if (M_KEY_Tap == 1)
            {
                if (Music_Mute == false)
                {
                    Music_Mute = true;
                    if (IsBASSMode)
                    {
                        BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_VOL, 0.0f);
                    }
                    else
                    {
                        ChangeVolumeSoundMem(0, MusicHandle);
                    }
                }
                else
                {
                    Music_Mute = false;
                    if (IsBASSMode)
                    {
                        BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_VOL, (float)((double)Music_Volume / (double)255));
                    }
                    else
                    {
                        ChangeVolumeSoundMem(Music_Volume, MusicHandle);
                    }
                }
            }
        }
        else
        {
            M_KEY_Tap = 0;
        }
        if (CheckHitKey(KEY_INPUT_M) != 0 && CheckHitKey(KEY_INPUT_S) != 0)
        {
            //時間停止
            Stop_Key++;
            if (Stop_Key == 1 && Stop == false)
            {
                Stop = true;
                if (IsBASSMode)
                {
                    BASS_Pause();
                }
                else
                {
                    Stop_Time = (long)GetSoundCurrentPosition(MusicHandle);
                    StopSoundMem(MusicHandle);
                }
                Stop_Play_Time = PlayTime_Plus;
                PlayTime_Plus = 0.0f;
            }
        }
        else
        {
            Stop_Key = 0;
        }
        if (CheckHitKey(KEY_INPUT_M) != 0 && CheckHitKey(KEY_INPUT_P) != 0)
        {
            //停止していたら再生
            Play_Key++;
            if (Play_Key == 1 && Stop == true)
            {
                Stop = false;
                if (IsBASSMode)
                {
                    BASS_Start();
                }
                else
                {
                    SetSoundCurrentPosition(Stop_Time, MusicHandle);
                    PlaySoundMem(MusicHandle, DX_PLAYTYPE_LOOP, FALSE);
                }
                PlayTime_Plus = Stop_Play_Time;
            }
        }
        else
        {
            Play_Key = 0;
        }
        if (CheckHitKey(KEY_INPUT_V) > 0 && CheckHitKey(KEY_INPUT_P) > 0)
        {
            //音量を上げる
            if (Music_Mute == false)
            {
                if (Music_Volume < 255)
                {
                    Music_Volume += 2;
                    if (IsBASSMode)
                    {
                        BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_VOL, (float)((double)Music_Volume / (double)255));
                    }
                    else
                    {
                        ChangeVolumeSoundMem(Music_Volume, MusicHandle);
                    }
                }
            }
        }
        if (CheckHitKey(KEY_INPUT_V) > 0 && CheckHitKey(KEY_INPUT_O) > 0)
        {
            //音量を下げる
            if (Music_Mute == false)
            {
                if (Music_Volume > 0)
                {
                    Music_Volume -= 2;
                    if (IsBASSMode)
                    {
                        BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_VOL, (float)((double)Music_Volume / (double)255));
                    }
                    else
                    {
                        ChangeVolumeSoundMem(Music_Volume, MusicHandle);
                    }
                }
            }
        }
        if (CheckHitKey(KEY_INPUT_C) > 0 && CheckHitKey(KEY_INPUT_P) > 0 && Stop == false)
        {
            //再生速度を上げる
            if (Pitch_Set <= Pitch_Max)
            {
                Pitch_Set += Pitch_Change_Size / 10 * FPS_Kakeru;
                if (IsBASSMode)
                {
                    //BASSの再生速度を変更
                    double BASS_Pitch = 100 - Pitch_Set * 100;
                    BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_TEMPO, (float)-BASS_Pitch);
                    double Now = BASS_ChannelBytes2Seconds(Stream, BASS_ChannelGetPosition(Stream, BASS_POS_BYTE));
                    PlayTime = (float)Now * 30;
                }
                else
                {
                    double Pitch = First_Music_Pitch * Pitch_Set;
                    if (!IsMusicNotChange)
                    {
                        SetFrequencySoundMem((int)Pitch, MusicHandle);
                    }
                }
                PlayTime_Plus_Temp_Kakeru += (float)Pitch_Change_Size / 10 * FPS_Kakeru;
                PlayTime_Plus = PlayTime_Plus_Temp * PlayTime_Plus_Temp_Kakeru;
            }
        }
        if (CheckHitKey(KEY_INPUT_C) > 0 && CheckHitKey(KEY_INPUT_O) > 0 && Stop == false)
        {
            //再生速度を下げる
            if (Pitch_Set >= Pitch_Min)
            {
                Pitch_Set -= Pitch_Change_Size / 10 * FPS_Kakeru;
                if (IsBASSMode)
                {
                    //BASSの再生速度を変更
                    double BASS_Pitch = 100 - Pitch_Set * 100;
                    BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_TEMPO, (float)-BASS_Pitch);
                    double Now = BASS_ChannelBytes2Seconds(Stream, BASS_ChannelGetPosition(Stream, BASS_POS_BYTE));
                    PlayTime = (float)Now * 30;
                }
                else
                {
                    double Pitch = First_Music_Pitch * Pitch_Set;
                    if (!IsMusicNotChange)
                    {
                        SetFrequencySoundMem((int)Pitch, MusicHandle);
                    }
                }
                PlayTime_Plus_Temp_Kakeru -= (float)Pitch_Change_Size / 10 * FPS_Kakeru;
                PlayTime_Plus = PlayTime_Plus_Temp * PlayTime_Plus_Temp_Kakeru;
            }
        }
        if (CheckHitKey(KEY_INPUT_H) > 0 && CheckHitKey(KEY_INPUT_P) > 0 && Stop == false)
        {
            //ピッチのみを上げる
            if (Music_BASS_Pitch < 20.0f)
            {
                Music_BASS_Pitch += 0.02f;
                BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_TEMPO_PITCH, Music_BASS_Pitch);
            }
        }
        if (CheckHitKey(KEY_INPUT_H) > 0 && CheckHitKey(KEY_INPUT_O) > 0 && Stop == false)
        {
            //ピッチのみを下げる
            if (Music_BASS_Pitch > -15.0f)
            {
                Music_BASS_Pitch -= 0.02f;
                BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_TEMPO_PITCH, Music_BASS_Pitch);
            }
        }
        if (CheckHitKey(KEY_INPUT_G) != 0)
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
        if (CheckHitKey(KEY_INPUT_R) != 0 && CheckHitKey(KEY_INPUT_E) != 0)
        {
            //フレームを最初から
            if (IsBASSMode)
            {
                __int64 Time2 = BASS_ChannelSeconds2Bytes(Stream, 0.4);
                BASS_ChannelSetPosition(Stream, Time2, BASS_POS_BYTE);
            }
            else
            {
                StopSoundMem(MusicHandle);
                SetSoundCurrentTime(static_cast<LONGLONG>(1), MusicHandle);
                PlaySoundMem(MusicHandle, DX_PLAYTYPE_LOOP, FALSE);
            }
            PlayTime = 0.0f;
        }
        if (CheckHitKey(KEY_INPUT_P) != 0 && CheckHitKey(KEY_INPUT_D) != 0)
        {
            //パンを下げる
            //*パンは、左右の音量比のこと。下げれば右が小さくなる
            Pan--;
            ChangePanSoundMem(Pan, MusicHandle);
        }
        if (CheckHitKey(KEY_INPUT_P) != 0 && CheckHitKey(KEY_INPUT_U) != 0)
        {
            //パンを上げる
            Pan++;
            ChangePanSoundMem(Pan, MusicHandle);
        }
        if (CheckHitKey(KEY_INPUT_P) != 0 && CheckHitKey(KEY_INPUT_R) != 0)
        {
            //ピッチ、パンを初期化
            Pan = 0;
            if (IsBASSMode)
            {
                Music_BASS_Pitch = 0;
                BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_TEMPO, 0);
                BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_TEMPO_PITCH, 0);
                double Now = BASS_ChannelBytes2Seconds(Stream, BASS_ChannelGetPosition(Stream, BASS_POS_BYTE));
                PlayTime = (float)Now * 30;
            }
            else
            {
                ChangePanSoundMem(Pan, MusicHandle);
                double Pitch = First_Music_Pitch;
                SetFrequencySoundMem((int)Pitch, MusicHandle);
            }
            Pitch_Set = 1;
            PlayTime_Plus_Temp_Kakeru = 1.0f;
            PlayTime_Plus = PlayTime_Plus_Temp * PlayTime_Plus_Temp_Kakeru;
        }
        if (CheckHitKey(KEY_INPUT_C) != 0 && CheckHitKey(KEY_INPUT_1) != 0)
        {
            //カメラ1に切り替える
            if (Camera_Number_First >= 1)
            {
                Camera_Number = 1;
            }
        }
        if (CheckHitKey(KEY_INPUT_C) != 0 && CheckHitKey(KEY_INPUT_2) != 0)
        {
            //カメラ2に切り替える
            if (Camera_Number_First >= 2)
            {
                Camera_Number = 2;
            }
        }
        if (CheckHitKey(KEY_INPUT_C) != 0 && CheckHitKey(KEY_INPUT_3) != 0)
        {
            //カメラ3に切り替える
            if (Camera_Number_First >= 3)
            {
                Camera_Number = 3;
            }
        }
        if (CheckHitKey(KEY_INPUT_C) != 0 && CheckHitKey(KEY_INPUT_Z) != 0)
        {
            //カメラの角度、視野を初期化
            targetZ = 0.0f;
            Linear_Not_VMD = 1.0f;
            Linear_Plus = 0.0f;
        }
        if (CheckHitKey(KEY_INPUT_C) != 0 && CheckHitKey(KEY_INPUT_R) != 0)
        {
            //カメラ全般を初期化
            if (Camera_VMD == true)
            {
                Linear_Plus = 0.0f;
            }
            else
            {
                Linear_Not_VMD = 1.0f;
                targetZ = 0.0f;
                targetX = 0.0f;
                angleY = 0.0f;
                targetY = 0.0f;
                cameraX = 0.0f;
                cameraY = 3.0f;
                cameraZ = -5.0f;
                x = 0.0f;
                y = 0.0f;
                z = 0.0f;
            }
        }
        if (IsModelPositionDistance && CheckHitKey(KEY_INPUT_N) != 0)
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
        if (ActiveFlag && Camera_VMD == false && FPS_Camera == false)
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
        //マウスでカメラの角度を変更
        if (ActiveFlag && FPS_Camera == true)
        {
            FPS_CameraHAngle -= (float)Move_MouseX / 10;
            if (FPS_CameraVAngle - (float)Move_MouseY / 10 <= 60.0f && FPS_CameraVAngle - (float)Move_MouseY / 10 >= -40)
            {
                FPS_CameraVAngle -= (float)Move_MouseY / 10;
            }
            else if (FPS_CameraVAngle - (float)Move_MouseY / 10 >= 60.0f)
            {
                FPS_CameraVAngle = 60.0f;
            }
            else if (FPS_CameraVAngle - (float)Move_MouseY / 10 <= -40.0f)
            {
                FPS_CameraVAngle = -40.0f;
            }
        }
        //TPSモード時のカメラ
        if (FPS_Camera == true)
        {
            SetupCamera_Perspective(Linear_Not_VMD);
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
            SinParam = (float)sin(static_cast<double>(FPS_CameraVAngle) / 180.0f * 3.14f);
            CosParam = (float)cos(static_cast<double>(FPS_CameraVAngle) / 180.0f * 3.14f);
            TempPosition1.x = 0.0f;
            TempPosition1.y = SinParam * Camera_Distance;
            TempPosition1.z = -CosParam * Camera_Distance;
            SinParam = (float)sin(static_cast<double>(FPS_CameraHAngle) / 180.0f * 3.14f);
            CosParam = (float)cos(static_cast<double>(FPS_CameraHAngle) / 180.0f * 3.14f);
            TempPosition2.x = CosParam * TempPosition1.x - SinParam * TempPosition1.z;
            TempPosition2.y = TempPosition1.y;
            TempPosition2.z = SinParam * TempPosition1.x + CosParam * TempPosition1.z;
            CameraPosition = VAdd(TempPosition2, CameraLookAtPosition);
            SetCameraPositionAndTarget_UpVecY(CameraPosition, Camera_Location);
        }
        else
        {
            //カメラのモーションがあれば実行なければマウスの角度を反映
            if (Camera_VMD == true)
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
            else
            {
                //通常カメラ
                SetupCamera_Perspective(Linear_Not_VMD);
                SetCameraPositionAndAngle(VGet(cameraX + (x * 10), cameraY + (y * 10), cameraZ + (z * 10)), targetX, targetY, targetZ);
            }
        }
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
            MV1DrawModel(SkyHandle);
            MV1SetRotationXYZ(ModelHandle_00, VGet(0.0f, 0.0f, 0.0f));
            MV1SetRotationXYZ(ModelHandle_01, VGet(0.0f, 0.0f, 0.0f));
            MV1SetRotationXYZ(ModelHandle_02, VGet(0.0f, 0.0f, 0.0f));
            MV1SetRotationXYZ(ModelHandle_03, VGet(0.0f, 0.0f, 0.0f));
            MV1SetRotationXYZ(ModelHandle_04, VGet(0.0f, 0.0f, 0.0f));
            if (IsModelPositionSet)
            {
                MV1SetPosition(ModelHandle_00, Model_Position_01);
                MV1SetPosition(ModelHandle_01, Model_Position_02);
                MV1SetPosition(ModelHandle_02, Model_Position_03);
                MV1SetPosition(ModelHandle_03, Model_Position_04);
                MV1SetPosition(ModelHandle_04, Model_Position_05);
            }
            else
            {
                MV1SetPosition(ModelHandle_00, VGet(Model_01_Position_X, 0.0f, 0.0f));
                MV1SetPosition(ModelHandle_01, VGet(Model_02_Position_X, 0.0f, 0.0f));
                MV1SetPosition(ModelHandle_02, VGet(Model_03_Position_X, 0.0f, 0.0f));
                MV1SetPosition(ModelHandle_03, VGet(Model_04_Position_X, 0.0f, 0.0f));
                MV1SetPosition(ModelHandle_04, VGet(Model_05_Position_X, 0.0f, 0.0f));
            }
            MV1SetAttachAnimTime(ModelHandle_00, AttachIndex_00, PlayTime);
            MV1SetAttachAnimTime(ModelHandle_01, AttachIndex_01, PlayTime);
            MV1SetAttachAnimTime(ModelHandle_02, AttachIndex_02, PlayTime);
            MV1SetAttachAnimTime(ModelHandle_03, AttachIndex_03, PlayTime);
            MV1SetAttachAnimTime(ModelHandle_04, AttachIndex_04, PlayTime);
            if (!IsOpacityChange)
            {
                MV1DrawModel(ModelHandle_00);
                MV1DrawModel(ModelHandle_01);
                MV1DrawModel(ModelHandle_02);
                MV1DrawModel(ModelHandle_03);
                MV1DrawModel(ModelHandle_04);
            }
            MV1DrawModel(MapHandle);
            MV1DrawModel(OceanHandle);
            if (IsOpacityChange)
            {
                MV1DrawModel(ModelHandle_00);
                MV1DrawModel(ModelHandle_01);
                MV1DrawModel(ModelHandle_02);
                MV1DrawModel(ModelHandle_03);
                MV1DrawModel(ModelHandle_04);
            }
            DrawRotaGraph((int)w, (int)h, 10, 0, ShaderHandle, TRUE);
            MV1SetRotationXYZ(SkyHandle, VGet(0.0f, MV1GetRotationXYZ(SkyHandle).y + 0.0001f, 0.0f));
        }
        else
        {
            MV1DrawModel(SkyHandle);
            MV1DrawModel(MapHandle);
            MV1DrawModel(OceanHandle);
            DrawRotaGraph((int)w, (int)h, 10, 0, ShaderHandle, TRUE);
            MV1SetRotationXYZ(SkyHandle, VGet(0.0f, MV1GetRotationXYZ(SkyHandle).y + 0.0001f, 0.0f));
            MV1SetRotationXYZ(ModelHandle_00, VGet(0.0f, 0.0f, 0.0f));
            MV1SetRotationXYZ(ModelHandle_01, VGet(0.0f, 0.0f, 0.0f));
            MV1SetRotationXYZ(ModelHandle_02, VGet(0.0f, 0.0f, 0.0f));
            MV1SetRotationXYZ(ModelHandle_03, VGet(0.0f, 0.0f, 0.0f));
            MV1SetRotationXYZ(ModelHandle_04, VGet(0.0f, 0.0f, 0.0f));
            if (IsModelPositionSet)
            {
                MV1SetPosition(ModelHandle_00, Model_Position_01);
                MV1SetPosition(ModelHandle_01, Model_Position_02);
                MV1SetPosition(ModelHandle_02, Model_Position_03);
                MV1SetPosition(ModelHandle_03, Model_Position_04);
                MV1SetPosition(ModelHandle_04, Model_Position_05);
            }
            else
            {
                MV1SetPosition(ModelHandle_00, VGet(Model_01_Position_X, 0.0f, 0.0f));
                MV1SetPosition(ModelHandle_01, VGet(Model_02_Position_X, 0.0f, 0.0f));
                MV1SetPosition(ModelHandle_02, VGet(Model_03_Position_X, 0.0f, 0.0f));
                MV1SetPosition(ModelHandle_03, VGet(Model_04_Position_X, 0.0f, 0.0f));
                MV1SetPosition(ModelHandle_04, VGet(Model_05_Position_X, 0.0f, 0.0f));
            }
            MV1SetAttachAnimTime(ModelHandle_00, AttachIndex_00, PlayTime);
            MV1DrawModel(ModelHandle_00);
            MV1SetAttachAnimTime(ModelHandle_01, AttachIndex_01, PlayTime);
            MV1DrawModel(ModelHandle_01);
            MV1SetAttachAnimTime(ModelHandle_02, AttachIndex_02, PlayTime);
            MV1DrawModel(ModelHandle_02);
            MV1SetAttachAnimTime(ModelHandle_03, AttachIndex_03, PlayTime);
            MV1DrawModel(ModelHandle_03);
            MV1SetAttachAnimTime(ModelHandle_04, AttachIndex_04, PlayTime);
            MV1DrawModel(ModelHandle_04);
        }
        SetUseShadowMap(0, -1);
        SetUseShadowMap(1, -1);
        //ミュートでない場合、曲の音量を反映
        if (Music_Mute == false)
        {
            ChangeVolumeSoundMem(Music_Volume, MusicHandle);
        }
        if (ActiveFlag)
        {
            //マウスを固定
            SetMousePoint((int)w / 2, (int)h / 2);
            SetMouseDispFlag(FALSE);
        }
        else if (!ActiveFlag)
        {
            SetMouseDispFlag(TRUE);
        }
        //FPSを表示させる場合はコメントを外す(現在のFPSは処理速度により変化しないため表示させていません。)
        //fps.Draw();
        //DrawFormatString(0, 0, GetColor(255, 255, 255), "%ld", BASS_ChannelGetPosition(Stream, BASS_POS_BYTE));
        ScreenFlip();
        fps.Wait();
    }
}
void Model_Position_Change_By_SettingFile(float Distance)
{
    //モデル間隔をセット
    Model_Distance = Distance;
    if (Model_Number == 1)
    {
        Model_01_Position_X = 0.0f;
    }
    else if (Model_Number == 2)
    {
        Model_01_Position_X = -Distance / 2;
        Model_02_Position_X = Distance / 2;
    }
    else if (Model_Number == 3)
    {
        Model_01_Position_X = -Distance;
        Model_02_Position_X = 0.0f;
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
        Model_03_Position_X = 0.0f;
        Model_04_Position_X = Distance;
        Model_05_Position_X = Distance + Distance;
    }
}
void Model_Load()
{
    //ロード
    //実行ディレクトリを取得
    GetCurrentDirectory(255, cdir);
    //画面サイズを取得
    GetWindowSize(&w, &h);
    //テキストを表示
    SetFontSize(36);
    ChangeFont("MS UI Gothic");
    ClearDrawScreen();
    DrawString((int)(w / 3), (int)(h / 2.03), "ロード中です。しばらくお待ちください・・・", GetColor(255, 255, 255));
    ScreenFlip();
    float Shadow_Size = 500.0f;
    float Shadow_Height = 250.0f;
    VECTOR Shadow_Angle = VGet(-0.5f, -0.75f, 0.5f);
    ChangeLightTypeDir(VGet(0.0f, 0.0f, 2.0f));
    MV1SetLoadModel_PMD_PMX_AnimationFPSMode(120);
    if (!File_Exist(cdir + std::string("/Resources/Setting.dat")))
    {
        MessageBox(NULL, TEXT("設定ファイルが存在しません。付属のソフトで作成してください。"), TEXT("エラー"), MB_OK);
        return;
    }
    if (!File_Exist(cdir + std::string("/Load_Data.dat")))
    {
        //ユーザー設定されていない場合、初期に入っているモデルや曲にする
        int StreamHandle = BASS_StreamCreateFile(false, String(cdir + String("/Resources/Music/Default.mp3")).c_str(), 0, 0, BASS_SAMPLE_FLOAT | BASS_STREAM_DECODE);
        Stream = BASS_FX_TempoCreate(StreamHandle, BASS_FX_FREESOURCE);
        ModelHandle_00 = MV1LoadModel(String(cdir + String("/Resources/Chara/IA/Model.mv1")).c_str());
        ModelHandle_01 = MV1LoadModel(String(cdir + String("/Resources/Chara/Test_01/Model.mv1")).c_str());
        ModelHandle_02 = MV1LoadModel(String(cdir + String("/Resources/Chara/Alice/Model.mv1")).c_str());
        MusicHandle = LoadSoundMem(String(cdir + String("/Resources/Music/Default.mp3")).c_str());
        First_Music_Pitch = GetFrequencySoundMem(MusicHandle);
        Model_Number = 3;
        //カメラのモーションがあれば反映
        if (File_Exist(cdir + String("/Resources/Camera.vmd")))
        {
            CameraHandle1 = MV1LoadModel(String(cdir + String("/Resources/Camera.vmd")).c_str());
            Camera_Number_First++;
        }
        if (File_Exist(cdir + String("/Resources/Camera1.vmd")))
        {
            CameraHandle2 = MV1LoadModel(String(cdir + String("/Resources/Camera1.vmd")).c_str());
            Camera_Number_First++;
        }
        if (File_Exist(cdir + String("/Resources/Camera2.vmd")))
        {
            CameraHandle3 = MV1LoadModel(String(cdir + String("/Resources/Camera2.vmd")).c_str());
            Camera_Number_First++;
        }
    }
    else
    {
        //ユーザー設定を適応
        tinyxml2::XMLDocument xml;
        xml.LoadFile(String(cdir + String("/Load_Data.dat")).c_str());
        XMLElement* root = xml.FirstChildElement("Save");
        Model_Number = std::stoi(root->FirstChildElement("MMD_Number_C")->GetText());
        if (File_Exist(cdir + String("/Resources/UserCamera1.vmd")))
        {
            CameraHandle1 = MV1LoadModel(String(cdir + String("/Resources/UserCamera1.vmd")).c_str());
            Camera_Number_First++;
            Sleep(10);
        }
        if (File_Exist(cdir + String("/Resources/UserCamera2.vmd")))
        {
            CameraHandle2 = MV1LoadModel(String(cdir + String("/Resources/UserCamera2.vmd")).c_str());
            Camera_Number_First++;
            Sleep(10);
        }
        if (File_Exist(cdir + String("/Resources/UserCamera3.vmd")))
        {
            CameraHandle3 = MV1LoadModel(String(cdir + String("/Resources/UserCamera3.vmd")).c_str());
            Camera_Number_First++;
            Sleep(10);
        }
        for (int Number = 0; Number <= 5; Number++)
        {
            if (Number == 0)
            {
                ModelHandle_00 = MV1LoadModel(String(cdir + String("/Resources/Chara/1/Model.mv1")).c_str());
            }
            else if (Number == 1)
            {
                ModelHandle_01 = MV1LoadModel(String(cdir + String("/Resources/Chara/2/Model.mv1")).c_str());
            }
            else if (Number == 2)
            {
                ModelHandle_02 = MV1LoadModel(String(cdir + String("/Resources/Chara/3/Model.mv1")).c_str());
            }
            else if (Number == 3)
            {
                ModelHandle_03 = MV1LoadModel(String(cdir + String("/Resources/Chara/4/Model.mv1")).c_str());
            }
            else if (Number == 4)
            {
                ModelHandle_04 = MV1LoadModel(String(cdir + String("/Resources/Chara/5/Model.mv1")).c_str());
            }
        }
        if (root->FirstChildElement("Music_Path")->GetText() != "")
        {
            String Ex = File_Get_Ex(root->FirstChildElement("Music_Path")->GetText());
            int StreamHandle = BASS_StreamCreateFile(false, String(cdir + String("/Resources/Music/UserMusic" + Ex)).c_str(), 0, 0, BASS_SAMPLE_FLOAT | BASS_STREAM_DECODE);
            Stream = BASS_FX_TempoCreate(StreamHandle, BASS_FX_FREESOURCE);
            MusicHandle = LoadSoundMem(String(cdir + String("/Resources/Music/UserMusic" + Ex)).c_str());
            First_Music_Pitch = GetFrequencySoundMem(MusicHandle);
        }
    }
    tinyxml2::XMLDocument Setting_01;
    Setting_01.LoadFile(String(cdir + String("/Resources/Setting.dat")).c_str());
    XMLElement* Setting_01_Root = Setting_01.FirstChildElement("Setting_Save");
    String Temp = Setting_01_Root->FirstChildElement("Map_Select")->GetText();
    if (Temp == "0")
    {
        //町マップ
        MapHandle = MV1LoadModel(String(cdir + String("/Resources/Map/Stage/サン・マルコ広場_Ver1.00.mv1")).c_str());
        SkyHandle = MV1LoadModel(String(cdir + String("/Resources/Sky/Dome_X503.mv1")).c_str());
        OceanHandle = MV1LoadModel(String(cdir + String("/Resources/Map/Ocean/Ocean.mv1")).c_str());
        MV1SetScale(MapHandle, VGet(10.0f, 10.0f, 10.0f));
        MV1SetScale(OceanHandle, VGet(10.0f, 10.0f, 10.0f));
        MV1SetScale(SkyHandle, VGet(10.0f, 10.0f, 10.0f));
        MV1SetPosition(MapHandle, VGet(-20.0f, 0.0f, -300.0f));
        MV1SetPosition(OceanHandle, VGet(-20.0f, -35.0f, -100.0f));
        MV1SetRotationXYZ(MapHandle, VGet(0.0f, 0.15f, 0.0f));
    }
    else if (Temp == "1")
    {
        //天空マップ
        MapHandle = MV1LoadModel(String(cdir + String("/Resources/Map/Space/Model.mv1")).c_str());
        SkyHandle = MV1LoadModel(String(cdir + String("/Resources/Sky/Dome_X503.mv1")).c_str());
        MV1SetScale(MapHandle, VGet(1.0f, 1.0f, 1.0f));
        MV1SetScale(SkyHandle, VGet(10.0f, 10.0f, 10.0f));
        MV1SetPosition(MapHandle, VGet(0.0f, 0.43f, 40.0f));
        MV1SetRotationXYZ(MapHandle, VGet(0.0f, 0.15f, 0.0f));
        Shadow_Size = 100.0f;
        Shadow_Height = 50.0f;
    }
    else if (Temp == "2")
    {
        //ユーザーマップ
        tinyxml2::XMLDocument Map_Setting_01;
        Map_Setting_01.LoadFile(String(cdir + String("/Resources/Map_Setting.dat")).c_str());
        XMLElement* Map_Setting_01_Root = Map_Setting_01.FirstChildElement("Map_Setting");
        MapHandle = MV1LoadModel(String(cdir + String("/Resources/Map/UserMap/Model.mv1")).c_str());
        SkyHandle = MV1LoadModel(String(cdir + String("/Resources/Sky/Dome_X503.mv1")).c_str());
        String Temp_01 = Map_Setting_01_Root->FirstChildElement("Sky_Enable")->GetText();
        if (Temp_01 == "true")
        {
            SkyHandle = MV1LoadModel(String(cdir + String("/Resources/Sky/Dome_X503.mv1")).c_str());
            MV1SetScale(SkyHandle, VGet(10.0f, 10.0f, 10.0f));
        }
        float Map_Size = std::stof(Map_Setting_01_Root->FirstChildElement("Map_Scale")->GetText());
        float Map_X = std::stof(Map_Setting_01_Root->FirstChildElement("Map_X")->GetText());
        float Map_Y = std::stof(Map_Setting_01_Root->FirstChildElement("Map_Y")->GetText());
        float Map_Z = std::stof(Map_Setting_01_Root->FirstChildElement("Map_Z")->GetText());
        int Map_Rotate_Y = std::stoi(Map_Setting_01_Root->FirstChildElement("Map_Rotate")->GetText());
        MV1SetScale(MapHandle, VGet(Map_Size, Map_Size, Map_Size));
        MV1SetPosition(MapHandle, VGet(Map_X, Map_Y, Map_Z));
        MV1SetRotationXYZ(MapHandle, VGet(0.0f, Map_Rotate_Y * 3.141592f / 180.0f, 0.0f));
    }
    //モデル同士の間隔を設定
    String Temp_02 = Setting_01_Root->FirstChildElement("Model_Position")->GetText();
    if (Temp_02 != "0")
    {
        IsModelPositionDistance = true;
        Model_Position_Change_By_SettingFile(std::stof(Setting_01_Root->FirstChildElement("Model_Position")->GetText()));
    }
    else
    {
        IsModelPositionDistance = false;
    }
    String Temp_03 = Setting_01_Root->FirstChildElement("Light_Select")->GetText();
    if (Temp_03 == "0")
    {
        //ライト有効の場合
        SetLightEnable(TRUE);
        ShaderHandle = LoadGraph(String(cdir + String("/Resources/Effects/Shader.png")).c_str());
        if (Temp == "1")
        {
            ChangeLightTypePoint(VGet(-500.0f, 850.0f, -100.0f), 10000.0f, 0.01f, 0.01f, 0.01f);
            CreatePointLightHandle(VGet(0.0f, 100.0f, 500.0f), 1500.0f, 0.0f, 0.00125f, 0.0f);
            CreatePointLightHandle(VGet(0.0f, 100.0f, -500.0f), 1500.0f, 0.0f, 0.00125f, 0.0f);
        }
        else
        {
            ChangeLightTypePoint(VGet(-500.0f, 850.0f, -100.0f), 10000.0f, 0.01f, 0.01f, 0.01f);
            CreatePointLightHandle(VGet(0.0f, 1000.0f, 500.0f), 1500.0f, 0.0f, 0.00125f, 0.0f);
            CreatePointLightHandle(VGet(0.0f, 1000.0f, -500.0f), 1500.0f, 0.0f, 0.00125f, 0.0f);
        }
        SetLightAngle(1.0f, -1.0f);
    }
    else
    {
        //ライト無効の場合
        SetLightEnable(FALSE);
        String Temp_99 = Setting_01_Root->FirstChildElement("Horror_Select")->GetText();
        if (Temp_99 == "0")
        {
            CreatePointLightHandle(VGet(0.0f, 1000.0f, 0.0f), 500.0f, 0.0f, 0.00525f, 0.0f);
        }
        else if (Temp == "0" || Temp == "1")
        {
            SkyHandle = MV1LoadModel(String(cdir + String("/Resources/Sky/Moon.mv1")).c_str());
            IsHorror_Mode_And_Map_0 = true;
        }
        else if (Temp == "2")
        {
            String Temp_100 = Setting_01_Root->FirstChildElement("Horror_Sky_Select")->GetText();
            if (Temp_100 == "0")
            {
                SkyHandle = MV1LoadModel(String(cdir + String("/Resources/Sky/Moon.mv1")).c_str());
                IsHorror_Mode_And_Map_0 = true;
            }
        }
    }
    //サウンドモードを適応
    String Temp_04 = Setting_01_Root->FirstChildElement("Music_Mode")->GetText();
    if (Temp_04 == "0")
    {
        IsBASSMode = false;
    }
    else if (Temp_04 == "1")
    {
        IsBASSMode = true;
    }
    if (Setting_01_Root->FirstChildElement("Shadow_Mode")->GetText() == "true")
    {
        //影の角度を設定
        String Temp_91 = Setting_01_Root->FirstChildElement("Shadow_Angle")->GetText();
        if (Temp_91 == "0")
        {
            Shadow_Angle = VGet(-0.5f, -0.75f, 0.5f);
        }
        else if (Temp_91 == "1")
        {
            Shadow_Angle = VGet(0.5f, -0.75f, 0.5f);
        }
        else if (Temp_91 == "2")
        {
            Shadow_Angle = VGet(-0.5f, -0.75f, -0.5f);
        }
        else if (Temp_91 == "3")
        {
            Shadow_Angle = VGet(0.5f, -0.75f, -0.5f);
        }
    }
    //ピッチや音量をセット
    First_Music_Pitch = GetFrequencySoundMem(MusicHandle);
    Music_Volume = std::stoi(Setting_01_Root->FirstChildElement("Volume")->GetText());
    Pitch_Set = std::stod(Setting_01_Root->FirstChildElement("Pitch")->GetText());
    PlayTime_Plus_Temp_Kakeru = std::stof(Setting_01_Root->FirstChildElement("Pitch")->GetText());
    Pan = std::stoi(Setting_01_Root->FirstChildElement("Pan")->GetText());
    ChangePanSoundMem(Pan, MusicHandle);
    Pitch_Set_Void();
    //詳細設定
    if (File_Exist(cdir + String("/Resources/Advance_Setting.dat")))
    {
        tinyxml2::XMLDocument Advance_Setting;
        Advance_Setting.LoadFile(String(cdir + String("/Resources/Advance_Setting.dat")).c_str());
        XMLElement* Advance_Setting_Root = Advance_Setting.FirstChildElement("Advance_Save");
        Mouse_Sensitivity = std::stoi(Advance_Setting_Root->FirstChildElement("Mouse_Sensitivity")->GetText());
        Pitch_Change_Size = std::stod(Advance_Setting_Root->FirstChildElement("Pitch_Interval")->GetText());
        IsModelPositionSet = Bool_Replace(Advance_Setting_Root->FirstChildElement("IsModelPositionSet")->GetText());
        if (IsModelPositionSet)
        {
            //モデルの位置が指定されていれば設定
            Model_01_X = std::stof(Advance_Setting_Root->FirstChildElement("Model_01_X")->GetText());
            Model_01_Y = std::stof(Advance_Setting_Root->FirstChildElement("Model_01_Y")->GetText());
            Model_01_Z = std::stof(Advance_Setting_Root->FirstChildElement("Model_01_Z")->GetText());
            Model_02_X = std::stof(Advance_Setting_Root->FirstChildElement("Model_02_X")->GetText());
            Model_02_Y = std::stof(Advance_Setting_Root->FirstChildElement("Model_02_Y")->GetText());
            Model_02_Z = std::stof(Advance_Setting_Root->FirstChildElement("Model_02_Z")->GetText());
            Model_03_X = std::stof(Advance_Setting_Root->FirstChildElement("Model_03_X")->GetText());
            Model_03_Y = std::stof(Advance_Setting_Root->FirstChildElement("Model_03_Y")->GetText());
            Model_03_Z = std::stof(Advance_Setting_Root->FirstChildElement("Model_03_Z")->GetText());
            Model_04_X = std::stof(Advance_Setting_Root->FirstChildElement("Model_04_X")->GetText());
            Model_04_Y = std::stof(Advance_Setting_Root->FirstChildElement("Model_04_Y")->GetText());
            Model_04_Z = std::stof(Advance_Setting_Root->FirstChildElement("Model_04_Z")->GetText());
            Model_05_X = std::stof(Advance_Setting_Root->FirstChildElement("Model_05_X")->GetText());
            Model_05_Y = std::stof(Advance_Setting_Root->FirstChildElement("Model_05_Y")->GetText());
            Model_05_Z = std::stof(Advance_Setting_Root->FirstChildElement("Model_05_Z")->GetText());
            Model_Position_01 = VGet(Model_01_X, Model_01_Y, Model_01_Z);
            Model_Position_02 = VGet(Model_02_X, Model_02_Y, Model_02_Z);
            Model_Position_03 = VGet(Model_03_X, Model_03_Y, Model_03_Z);
            Model_Position_04 = VGet(Model_04_X, Model_04_Y, Model_04_Z);
            Model_Position_05 = VGet(Model_05_X, Model_05_Y, Model_05_Z);
        }
        Pitch_Min = std::stod(Advance_Setting_Root->FirstChildElement("Pitch_Min")->GetText());
        Pitch_Max = std::stod(Advance_Setting_Root->FirstChildElement("Pitch_Max")->GetText());
        IsFrameRateLock = Bool_Replace(Advance_Setting_Root->FirstChildElement("FrameRateLock")->GetText());
        IsMusicNotChange = Bool_Replace(Advance_Setting_Root->FirstChildElement("IsMusicNotChange")->GetText());
        IsPhysicsSet = Bool_Replace(Advance_Setting_Root->FirstChildElement("IsPhysicsSet")->GetText());
        IsModelPhysicsSet = Bool_Replace(Advance_Setting_Root->FirstChildElement("IsModelPhysicsSet")->GetText());
        int FPS_01 = std::stoi(Advance_Setting_Root->FirstChildElement("FPS")->GetText());
        //FPSの値によってモーションの速度が異なるため指定する
        if (FPS_01 == 0)
        {
            PlayTime_Plus = 1.0f;
            PlayTime_Plus_Temp = 1.0f;
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
    if (Model_Number >= 1)
    {
        Model_Physics_00 = MV1LoadModel(String(cdir + String("/Resources/Map/Coll/Model.mv1")).c_str());
    }
    if (Model_Number >= 2)
    {
        Model_Physics_01 = MV1LoadModel(String(cdir + String("/Resources/Map/Coll/Model.mv1")).c_str());
    }
    if (Model_Number >= 3)
    {
        Model_Physics_02 = MV1LoadModel(String(cdir + String("/Resources/Map/Coll/Model.mv1")).c_str());
    }
    if (Model_Number >= 4)
    {
        Model_Physics_03 = MV1LoadModel(String(cdir + String("/Resources/Map/Coll/Model.mv1")).c_str());
    }
    if (Model_Number >= 5)
    {
        Model_Physics_04 = MV1LoadModel(String(cdir + String("/Resources/Map/Coll/Model.mv1")).c_str());
    }
    Model_Shadow_00 = MakeShadowMap(8192, 8192);
    Map_Shadow_00 = MakeShadowMap(16384, 16384);
    SetShadowMapLightDirection(Model_Shadow_00, Shadow_Angle);
    SetShadowMapLightDirection(Map_Shadow_00, Shadow_Angle);
    SetLightDirection(Shadow_Angle);
    SetShadowMapDrawArea(Model_Shadow_00, VGet(-30.0f, -1.0f, -30.0f), VGet(30.0f, 30.0f, 30.0f));
    SetShadowMapDrawArea(Map_Shadow_00, VGet(-Shadow_Size, -1.0f, -Shadow_Size), VGet(Shadow_Size, Shadow_Height, Shadow_Size));
    ShadowMap_DrawSetup(Map_Shadow_00);
    MV1DrawModel(MapHandle);
    ShadowMap_DrawEnd();
    MV1SetupCollInfo(MapHandle, -1);
    MV1SetupCollInfo(Model_Physics_00, -1);
    MV1SetupCollInfo(Model_Physics_01, -1);
    MV1SetupCollInfo(Model_Physics_02, -1);
    MV1SetupCollInfo(Model_Physics_03, -1);
    MV1SetupCollInfo(Model_Physics_04, -1);
    //モデルの初期位置を設定
    MV1SetScale(SkyHandle, VGet(50.0f, 50.0f, 50.0f));
    MV1SetRotationXYZ(SkyHandle, VGet(0.0f, 0.0f, 0.0f));
    MV1SetLoadModelUsePhysicsMode(DX_LOADMODEL_PHYSICS_REALTIME);
    MV1SetScale(ModelHandle_00, VGet(1.0f, 1.0f, 1.0f));
    MV1SetRotationXYZ(ModelHandle_00, VGet(0.0f, 1.5f, 0.0f));
    MV1SetScale(ModelHandle_01, VGet(1.0f, 1.0f, 1.0f));
    MV1SetRotationXYZ(ModelHandle_01, VGet(0.0f, 1.5f, 0.0f));
    MV1SetScale(ModelHandle_02, VGet(1.0f, 1.0f, 1.0f));
    MV1SetRotationXYZ(ModelHandle_02, VGet(0.0f, 1.5f, 0.0f));
    MV1SetScale(ModelHandle_03, VGet(1.0f, 1.0f, 1.0f));
    MV1SetRotationXYZ(ModelHandle_03, VGet(0.0f, 1.5f, 0.0f));
    MV1SetScale(ModelHandle_04, VGet(1.0f, 1.0f, 1.0f));
    MV1SetRotationXYZ(ModelHandle_04, VGet(0.0f, 1.5f, 0.0f));
    ModelHandle_Sub_00 = MV1DuplicateModel(ModelHandle_00);
    ModelHandle_Sub_01 = MV1DuplicateModel(ModelHandle_01);
    ModelHandle_Sub_02 = MV1DuplicateModel(ModelHandle_02);
    ModelHandle_Sub_03 = MV1DuplicateModel(ModelHandle_03);
    ModelHandle_Sub_04 = MV1DuplicateModel(ModelHandle_04);
    SetCameraNearFar(1.0f, 10000.0f);
    SetCameraPositionAndTarget_UpVecY(VGet(0.0f, 3.0f, -5.0f), VGet(0.0f, 0.0f, 0.0f));
    SetSoundCurrentTime(0, MusicHandle);
    SetSoundCurrentTime(static_cast<LONGLONG>(1), MusicHandle);
    if (IsBASSMode)
    {
        BASS_ChannelPlay(Stream, true);
        BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_VOL, (float)((double)Music_Volume / (double)255));
        BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_TEMPO_PITCH, Music_BASS_Pitch);
        BASS_ChannelSetAttribute(Stream, BASS_ATTRIB_TEMPO, 0.0f);
        __int64 Time2 = BASS_ChannelSeconds2Bytes(Stream, 0);
        BASS_ChannelSetPosition(Stream, Time2, BASS_POS_BYTE);
    }
    else
    {
        PlaySoundMem(MusicHandle, DX_PLAYTYPE_LOOP, FALSE);
        ChangeVolumeSoundMem(Music_Volume, MusicHandle);
    }
    PlayTime = 0.0f;
    Dance_Loop();
}