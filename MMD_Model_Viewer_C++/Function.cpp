#include <string>
#include <fstream>


//�t�@�C�������݂��邩���擾(���݂���ꍇtrue��Ԃ�)
bool File_Exist(std::string Path)
{
    std::ifstream ifs(Path.c_str());
    bool Temp = ifs.is_open();
    ifs.close();
    return Temp;
}
//�t�@�C���̊g���q���擾(��:Picture.png = .png)
std::string File_Get_Ex(std::string Path)
{
    std::string fullpath = Path;
    return fullpath.substr(fullpath.find_last_of("."));
}
//�����񂩂�bool�l���擾(true�ȊO�̕�����false��Ԃ�)
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