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

        public static HomePageViewModel Initialization(Dispatcher _dispatcher, Action action)
        {
            if (_HomePageViewModel == null)
            {
                _HomePageViewModel = new HomePageViewModel(_dispatcher, action);
            }
            return _HomePageViewModel;
        }

        public static HomePageViewModel Initialization()
        {
            return _HomePageViewModel;
        }


        Dispatcher dispatcher;
        /// <summary>
        /// 初始化数据
        /// </summary>
        private HomePageViewModel(Dispatcher _dispatcher, Action action)
        {
            dispatcher = _dispatcher;
            //加载第一页数据
            LoadData(action);
        }

        bool loadState = false;

        internal void NextPage(Action action)
        {
            LoadData(action);
        }


        public void LoadData(Action action)
        {
            if (loadState)
            {
                return;
            }
            loadState = true;
            PageIndex++;
            //加载第一页数据
            API.MainPage(PageIndex, new Action<List<Image>, int>((list, pageSize) =>
             {

                //homePage.Dispatcher.Invoke();
                if (!Waterfall)//瀑布
                {
                     dispatcher.Invoke(new Action(() =>
                     {
                         ImageList.Clear();
                     }));
                 }

                 foreach (var item in list)
                 {
                     dispatcher.Invoke(new Action(() =>
                     {
                         ImageList.Add(item);
                     }));
                 }
                 Thread.Sleep(500);
                 dispatcher.Invoke(new Action(() =>
                 {
                     PageCount = pageSize;
                     action();//关闭动画
                }));
                 loadState = false;
             }), new Action<Exception>((ex) =>
             {
                 dispatcher.Invoke(new Action(() =>
                 {
                     PageIndex--;
                     action();//关闭动画
                }));
                 loadState = false;
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


    }
}
