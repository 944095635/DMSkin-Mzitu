using Mzitu.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Mzitu
{
    public class HomePageViewModel : ModelObject
    {
        private static HomePageViewModel _HomePageViewModel;

        public static HomePageViewModel Initialization(Action action)
        {
            if (_HomePageViewModel == null)
            {
                _HomePageViewModel = new HomePageViewModel(action);
            }
            return _HomePageViewModel;
        }

        public static HomePageViewModel Initialization()
        {
            return _HomePageViewModel;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private HomePageViewModel(Action action)
        {
            //加载第一页数据
            LoadData(action);
        }


        internal void NextPage(Action action)
        {
            LoadData(action);
        }


        public void LoadData(Action action)
        {
            if (RunState)
            {
                return;
            }
            RunState = true;
            PageIndex++;
            //加载第一页数据
            API.MainPage(PageIndex, new Action<List<Image>, int>((list, pageSize) =>
             {

                //homePage.Dispatcher.Invoke();
                if (!Waterfall)//瀑布
                {
                     App.DispatcherHelper.Invoke(new Action(() =>
                     {
                         ImageList.Clear();
                     }));
                 }

                 foreach (var item in list)
                 {
                     App.DispatcherHelper.Invoke(new Action(() =>
                     {
                         ImageList.Add(item);
                     }));
                 }
                 Thread.Sleep(500);
                 App.DispatcherHelper.Invoke(new Action(() =>
                 {
                     PageCount = pageSize;
                     action();//关闭动画
                }));
                 RunState = false;
             }), new Action<Exception>((ex) =>
             {
                 App.DispatcherHelper.Invoke(new Action(() =>
                 {
                     PageIndex--;
                     action();//关闭动画
                }));
                 RunState = false;
             }));
        }
        int pageCount = 0;
        public int PageCount
        {
            get { return pageCount; }
            set
            {
                pageCount = value;
                OnPropertyChanged("PageCount");
            }
        }


        int pageIndex = 0;
        public int PageIndex
        {
            get { return pageIndex; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                pageIndex = value;
                OnPropertyChanged("PageIndex");
            }
        }


        bool waterfall = true;
        public bool Waterfall
        {
            get { return waterfall; }
            set
            {
                waterfall = value;
                OnPropertyChanged("Waterfall");
            }
        }

        private ObservableCollection<Image> imageList;
        public ObservableCollection<Image> ImageList
        {
            get
            {
                if (imageList == null)
                {
                    imageList = new ObservableCollection<Image>();
                }
                return imageList;
            }
            set
            {
                imageList = value;
                OnPropertyChanged("ImageList");
            }
        }


        private bool runState = false;
        public bool RunState
        {
            get
            {
                return runState;
            }
            set
            {
                runState = value;
                OnPropertyChanged("RunState");
            }
        }
    }
}
