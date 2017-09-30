using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mzitu
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
            loading.Start();
            DataContext = HomePageViewModel.Initialization(Dispatcher, new Action(() =>
            {
                loading.Stop();
            }));
        }

        ScrollViewer sv;

        private void ImageListControl_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sv == null)
            {
                sv = e.OriginalSource as ScrollViewer;
            }
            if (sv.ScrollableHeight == sv.VerticalOffset)
            {
                LoadData();
            }
        }

        public void LoadData()
        {
            loading.Start();
            HomePageViewModel.Initialization().NextPage(new Action(() =>
            {
                if (!HomePageViewModel.Initialization().Waterfall)
                {
                    ImageListControl.ScrollIntoView(HomePageViewModel.Initialization().ImageList[0]);
                }
                loading.Stop();
            }));
        }

        private void Page_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.Content.ToString() == "首页")
                {
                    HomePageViewModel.Initialization().PageIndex = 0;
                    LoadData();
                }

                if (btn.Content.ToString() == "上一页")
                {
                    HomePageViewModel.Initialization().PageIndex -= 2;
                    LoadData();
                }

                if (btn.Content.ToString() == "下一页")
                {
                    LoadData();
                }

                if (btn.Content.ToString() == "尾页")
                {
                    HomePageViewModel.Initialization().PageIndex = HomePageViewModel.Initialization().PageCount - 1;
                    LoadData();
                }

            }
        }
    }
}
