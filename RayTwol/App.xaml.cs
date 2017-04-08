using System;
using System.Threading;
using System.Windows.Threading;

namespace RayTwol
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //Editor.ExtractBackground("JUNF2");
            Editor.mainWindow = new MainWindow();
            Editor.mainWindow.ShowDialog();
        }
    }
}
