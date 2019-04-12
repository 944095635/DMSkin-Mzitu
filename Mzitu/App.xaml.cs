using DMSkin.Core;
using System.Windows;
using System.Windows.Threading;

namespace Mzitu
{
    public partial class App : Application
    {
        public static MainWindow APP;
        protected override void OnStartup(StartupEventArgs e)
        {
            Execute.InitializeWithDispatcher();

            MainWindow = APP;
            APP = new MainWindow();
            APP.Show();
        }
    }
}
