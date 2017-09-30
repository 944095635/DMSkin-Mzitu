using DMSkin.WPF.Small;
using Mzitu.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Mzitu
{
    public class API
    {
        //http://www.mzitu.com/
        /// <summary>
        /// 首页
        /// </summary>
        public static void MainPage(int pageIndex,Action<List<Image>,int> action,Action<Exception> exaction)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    string URL = "http://www.mzitu.com/page/"+pageIndex+"/";
                    using (HttpWebResponse response = HTTP.CreateGetHttpResponse(URL))
                    {
                        //使用手册
                        //返回加密的GZIP
                        GZipStream g = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                        StreamReader myStreamReader = new StreamReader(g, Encoding.GetEncoding("UTF-8"));
                        String msg = myStreamReader.ReadToEnd();

                        //msg是首页的html
                        //<ul id="pins">  </ul>
                        // 定义正则表达式用来匹配 标签 
                        List<Image> list = new List<Image>();
                        int pageSize = 0;
                        var matches = Regex.Matches(msg, "<ul id=\"pins\">[\\s\\S]*?</ul>");//取出每个<tr>
                        foreach (Match mc in matches)
                        {
                            string allText = mc.Groups[0].Value;

                            var matchesItem = Regex.Matches(allText, "<li>[\\s\\S]*?</li>");//取出每个<tr>
                            foreach (Match mcItem in matchesItem)
                            {
                                Image image = new Image();
                                string ItemText = mcItem.Groups[0].Value;
                                //<a href="http://www.mzitu.com/104369" target="_blank">雪千寻VS雪千紫</a>
                                image.Name = Regex.Matches(ItemText, "<a[\\s\\S]*?</a>")[1].Value.Split('>')[1].Split('<')[0];
                                //<a href="http://www.mzitu.com/104369"
                                image.Href= Regex.Matches(ItemText, "<a[\\s\\S]*?target=")[0].Value.Split('"')[1];
                                //<span class="time">2017-09-30</span>
                                image.Time = Regex.Matches(ItemText, "time\"[\\s\\S]*?</span>")[0].Value.Split('>')[1].Split('<')[0];
                                //<span class="view">4,482次</span>
                                image.Count = Regex.Matches(ItemText, "view\"[\\s\\S]*?</span>")[0].Value.Split('>')[1].Split('<')[0];

                                //先放置初始图片 //异步加载图片
                                image.ImageUrl = Regex.Matches(ItemText, "data-original=[\\s\\S]*?/>")[0].Value.Split('\'')[1];

                                DownImage(image.Href.Substring(image.Href.LastIndexOf('/'))+".jpg", image.ImageUrl, new Action<string>((path) =>
                                {
                                    image.ImageUrl = path;
                                }));
                                list.Add(image);
                            }
                        }
                        //</span>153<span class="meta-nav
                        var page = Regex.Matches(msg, "/page/[\\s\\S]*?/");
                        foreach (Match mc in page)
                        {
                            if (mc.Groups[0]!=null&& mc.Groups[0].Value!=null)
                            {
                                string allText = mc.Groups[0].Value;
                                    allText = allText.Replace("/page/", "").Replace("/", "");
                                    int temp = Convert.ToInt32(allText);
                                    if (temp > pageSize)
                                    {
                                        pageSize = temp;
                                    }
                            }
                        }
                        //<a class="page-numbers"
                        //回传数据
                        action(list, pageSize);
                    }

                }
                catch (Exception ex)//全局错误-网络错误 操作错误
                {
                    exaction(ex);
                    //MessageBox.Show("首页数据解析失败!"+ex.Message);
                }
            });

        }

        //Request URL:http://i.meizitu.net/thumbs/2017/09/104369_30b22_236.jpg
        //Request Method:GET
        //Status Code:304 Not Modified
        //Remote Address:119.84.82.20:80
        //Response Headers
        //view source
        //Cache-Control:max-age=2592000
        //Connection:keep-alive
        //Date:Sat, 30 Sep 2017 18:52:17 GMT
        //ETag:"59cfaaac-56af"
        //Expires:Mon, 30 Oct 2017 14:31:13 GMT
        //Last-Modified:Sat, 30 Sep 2017 14:31:08 GMT
        ///PLCDN:HIT CHN-FZ-MIX13-UP-1121 27.155.94.210
        //PLCDN:HIT CHN-CQ-MIX101-1121 119.84.82.20
        //Server:nginx
        //X-Frame-Options:SAMEORIGIN
        //Request Headers
        //view source

        //Accept:text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
        //Accept-Encoding:gzip, deflate, sdch
        //Accept-Language:zh-CN,zh;q=0.8
        //Cache-Control:max-age=0
        //Connection:keep-alive
        //Host:i.meizitu.net
        //If-Modified-Since:Sat, 30 Sep 2017 14:31:08 GMT
        //If-None-Match:"59cfaaac-56af"
        //Referer:http://www.mzitu.com/
        //Upgrade-Insecure-Requests:1
        //User-Agent:Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36


        /// <summary>
        /// 下图片。缓存
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="action"></param>
        public static void DownImage(string name, string path, Action<string> action)
        {
            Task.Factory.StartNew(() => {
                string DirectoryPath = HTTP.RunPath + "images\\";
                string ImagePath = HTTP.RunPath + "images\\" + name;

                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                if (!File.Exists(ImagePath))
                {
                    using (WebClient wb = new WebClient())
                    {
                        wb.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                        wb.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                        wb.Headers.Add("Cache-Control", "max-age=0");
                        wb.Headers.Add("If-None-Match", "59cfaaac-56af");
                        wb.Headers.Add("Host", "i.meizitu.net");
                        wb.Headers.Add("Referer", "http://www.mzitu.com/");
                        wb.DownloadFile(path, ImagePath);
                    }
                }
                action(ImagePath);
            });
        }
    }
}
