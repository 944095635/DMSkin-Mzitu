using DMSkin.Core;
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

        public static HomePageViewModel Initialization()
        {
            if (_HomePageViewModel == null)
            {
                _HomePageViewModel = new HomePageViewModel();
            }
            return _HomePageViewModel;
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        private HomePageViewModel()
        {
            //加载第一页数据
            LoadData();
        }


        internal void NextPage()
        {
            LoadData();
        }


        public void LoadData()
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
                     Execute.OnUIThread(()=> { ImageList.Clear(); });
                 }

                 foreach (var item in list)
                 {
                     Execute.OnUIThread(() => { ImageList.Add(item); });
                 }
                 Thread.Sleep(500);

                 Execute.OnUIThread(() => { PageCount = pageSize; });

                 RunState = false;
             }), new Action<Exception>((ex) =>
             {
                 Execute.OnUIThread(() => { PageIndex--; });
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
