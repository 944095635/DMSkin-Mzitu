using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Mzitu.Model
{
    public class Image:ModelObject
    { 

        private string href;
        /// <summary>
        /// 详细链接
        /// </summary>
        public string Href
        {
            get => href;
            set{
                href = value;
                OnPropertyChanged("Href");
            }
                
        }


        private string image;
        /// <summary>
        /// 详细链接
        /// </summary>
        public string ImageUrl
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged("ImageUrl");
            }

        }

        private string name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }

        }


        private string id;
        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }

        }


        private string time;
        /// <summary>
        /// 时间
        /// </summary>
        public string Time
        {
            get => time;
            set
            {
                time = value;
                OnPropertyChanged("Time");
            }

        }

        private string count;
        /// <summary>
        /// 名称
        /// </summary>
        public string Count
        {
            get => count;
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }

        }
    }
}
