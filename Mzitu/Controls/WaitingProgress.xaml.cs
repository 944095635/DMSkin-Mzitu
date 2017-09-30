using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mzitu
{
    /// <summary>
    /// WaitingProgress.xaml 的交互逻辑
    /// </summary>
    public partial class WaitingProgress : UserControl
    {

        private Storyboard story;
        public WaitingProgress()
        {
            InitializeComponent();
            this.story = (base.Resources["waiting"] as Storyboard);
        }

        public void Start()
        {
            //已经启动
            if (Visibility==Visibility.Visible)
            {
                return;
            }
            Visibility = Visibility.Visible;
            story.Begin(this.loading, true);
        }

        public void Stop()
        {
            base.Dispatcher.BeginInvoke(new Action(() => {
                story.Pause(this.loading);
                Visibility = Visibility.Collapsed;
            }));
        }
    }
}
