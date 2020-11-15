using System;
using System.Windows.Forms;

namespace MMD_Model_Viewer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MMD_Model_Viewer());
        }
    }
}