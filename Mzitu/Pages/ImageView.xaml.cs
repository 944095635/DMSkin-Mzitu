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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mzitu
{
    /// <summary>
    /// ImageView.xaml 的交互逻辑
    /// </summary>
    public partial class ImageView : UserControl
    {
        public ImageView()
        {
            InitializeComponent();

            this.DataContext = ImageViewViewModel.Initialization();
        }

        private bool isMouseLeftButtonDown = false;
        Point previousMousePoint = new Point(0, 0);
    

        private void img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseLeftButtonDown = true;
            previousMousePoint = e.GetPosition(img);
        }

        private void img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseLeftButtonDown = false;
        }

        private void img_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseLeftButtonDown = false;
        }

        private void img_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseLeftButtonDown == true)
            {
                Point position = e.GetPosition(img);
                tlt.X += position.X - this.previousMousePoint.X;
                tlt.Y += position.Y - this.previousMousePoint.Y;
            }
        }

        private void img_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point centerPoint = e.GetPosition(img);

            double val = (double)e.Delta / 2000;   //描述鼠标滑轮滚动
            if (sfr.ScaleX < 0.52 && sfr.ScaleY < 0.52 && e.Delta < 0)
            {
                return;
            }
            sfr.CenterX = centerPoint.X;
            sfr.CenterY = centerPoint.Y;

            
            sfr.ScaleX += val;
            sfr.ScaleY += val;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            ImageViewViewModel.Initialization().Next();
        }

        private void BtnPreviou_Click(object sender, RoutedEventArgs e)
        {
            ImageViewViewModel.Initialization().Previou(); 
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
            
            sfr.ScaleX =1;
            sfr.ScaleY = 1;
            tlt.X = 0;
            tlt.Y = 0;

            ImageViewViewModel.Initialization().PageIndex = 1;
        }
    }
}
