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
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
            DataContext = HomePageViewModel.Initialization();
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
            HomePageViewModel.Initialization().NextPage();


            if (!HomePageViewModel.Initialization().Waterfall)
            {
                if (HomePageViewModel.Initialization().ImageList.Count>1)
                {
                    ImageListControl.ScrollIntoView(HomePageViewModel.Initialization().ImageList[0]);
                }
            }
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

        /// <summary>
        /// 选中子项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageListControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImageListControl.SelectedItem is Model.Image image)
            {
                ImageViewViewModel.Initialization().Id = image.Id;
                ImageViewViewModel.Initialization().LoadImage();

                App.APP.ShowImageControl();
            }
        }
    }
}
