using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Mzitu
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow APP;
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = APP;
            APP = new MainWindow();
            APP.Show();
        }

    }
}
