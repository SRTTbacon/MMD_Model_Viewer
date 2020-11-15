#include <string>
#include <fstream>


//ファイルが存在するかを取得(存在する場合trueを返す)
bool File_Exist(std::string Path)
{
    std::ifstream ifs(Path.c_str());
    bool Temp = ifs.is_open();
    ifs.close();
    return Temp;
}
//ファイルの拡張子を取得(例:Picture.png = .png)
std::string File_Get_Ex(std::string Path)
{
    std::string fullpath = Path;
    return fullpath.substr(fullpath.find_last_of("."));
}
//文字列からbool値を取得(true以外の文字はfalseを返す)
bool Bool_Replace(std::string Moji)
{
    if (Moji == "true")
    {
        return true;
    }
    else
    {
        return false;
    }
}