#pragma once
#include <string>

//�t�@�C�������݂��邩���擾(���݂���ꍇtrue��Ԃ�)
bool File_Exist(std::string Path);
//�t�@�C���̊g���q���擾(��:Picture.png = .png)
std::string File_Get_Ex(std::string Path);
//�����񂩂�bool�l���擾(true�ȊO�̕�����false��Ԃ�)
bool Bool_Replace(std::string Moji);