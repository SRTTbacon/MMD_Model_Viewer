#include "DxLib.h"
#include "Sub.h"
#include "Function.h"
#include "resource.h"
#include "D:\Downloads\Google Downloads\Bass_C++\c\bass.h"

int WINAPI WinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPSTR lpCmdLine, _In_ int nCmdShow)
{
	char cdir[255];
	GetCurrentDirectory(255, cdir);
	if (!File_Exist(std::string(cdir) + "/Resources/Setting.dat"))
	{
		MessageBox(NULL, "Setting.datが存在しません。付属のソフトで作成してください。", "エラー", MB_OK);
		return -1;
	}
	if (!File_Exist(std::string(cdir) + "/Resources/Advance_Setting.dat"))
	{
		MessageBox(NULL, "Advance_Setting.datが存在しません。付属のソフトで作成してください。", "エラー", MB_OK);
		return -1;
	}
	SetWindowIconID(IDI_ICON1);
	SetUseDirect3DVersion(DX_DIRECT3D_11);
	SetAlwaysRunFlag(TRUE);
	SetUseFPUPreserveFlag(TRUE);
	SetWaitVSyncFlag(FALSE);
	SetOutApplicationLogValidFlag(FALSE);
	SetDoubleStartValidFlag(TRUE);
	SetUseDXArchiveFlag(TRUE);
	SetWindowVisibleFlag(TRUE);
	SetWindowStyleMode(2);
	ChangeWindowMode(TRUE);
	SetMainWindowText("MMD_Model_Viewer_C++");
	SetGraphMode(1920, 1080, 32);
	SetWindowSize(1920, 1080);
	SetZBufferBitDepth(24);
	SetCreateDrawValidGraphZBufferBitDepth(24);
	SetMouseDispFlag(TRUE);
	if (DxLib_Init() == -1)
	{
		return -1;
	}
	BASS_Init(-1, 44100, 0, 0, NULL);
	SetChangeScreenModeGraphicsSystemResetFlag(FALSE);
	SetDrawScreen(DX_SCREEN_BACK);
	Model_Load();
	return -1;
}