#pragma once
#include <string>

//ファイルが存在するかを取得(存在する場合trueを返す)
bool File_Exist(std::string Path);
//ファイルの拡張子を取得(例:Picture.png = .png)
std::string File_Get_Ex(std::string Path);
//文字列からbool値を取得(true以外の文字はfalseを返す)
bool Bool_Replace(std::string Moji);