using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZJULife
{
    class RecommendApp
    {        
        public string AppIcon { get; set; }
        public string AppUri { get; set; }
        public string AppInfo { get; set; }

        public RecommendApp(string appIcon, string appUri, string appInfo)
        {
            this.AppIcon = appIcon;
            this.AppUri = appUri;
            this.AppInfo = appInfo;
        }

        public static List<RecommendApp> GetAppRecommendList()
        {
            List<RecommendApp> recommendApps = new List<RecommendApp>() {
           new RecommendApp("ms-appx:///Assets/AppRecommend/ZJUWiFi.png", "ms-windows-store:navigate?appid=a844ea06-fcc5-4c59-809f-43f7184924d3", "ZJUWiFi是ZJUWLAN一键、自动登录工具。将一键登录固定到桌面磁贴, 实现真正的一键登录, 甚至不会打开程序。"),
            new RecommendApp("ms-appx:///Assets/AppRecommend/QiuShiChao.png", "ms-windows-store:navigate?appid=cf508ccd-4fde-4ca1-9b93-72844c6d7dc8", "求是潮手机站,是浙江大学求是潮的新一代移动客户端,提供课表/活动/成绩/考试/校车等方便学生的查询。目前是beta0.5版,所以只提供课表/活动/功能。"),
            new RecommendApp("ms-appx:///Assets/AppRecommend/OneDo.png", "ms-windows-store:navigate?appid=e21e7ce7-e536-4788-86c1-b6f72625ce2b", " OneDo是由捷克开发者开发的任务计划app,简单易用,设计优雅。支持备份、恢复,支持动态透明磁贴。"),
            new RecommendApp("ms-appx:///Assets/AppRecommend/Realarm.png", "ms-windows-store:navigate?appid=26cad058-fc9a-4967-95f0-4e83c09de99e", "Realarm即悦尔闹钟,与OneDo系同一开发者计划app,该应用简洁大方,功能全面,设计精美支持通过小娜发出语音指令进行操控。"),
            new RecommendApp("ms-appx:///Assets/AppRecommend/OfficeLens.png", "ms-windows-store:navigate?appid=5681f21c-f257-4d62-83f5-5341788a5077", "Office Lens 可以剪裁及强化处理白板、黑板和文档的图片，还能识别其中的文字，并将它们保存到 OneNote。"),
            new RecommendApp("ms-appx:///Assets/AppRecommend/XodoDocs.png", "ms-windows-store:navigate?appid=8dcee3d6-6043-4b22-b1ee-9fe7f90b4b63", "Xodo Docs是一款优秀的PDF阅读器，无广告、内购，可直接添加下划线、高亮、笔记等。"),
            new RecommendApp("ms-appx:///Assets/AppRecommend/PicsArt.png", "ms-windows-store:navigate?appid=fd381b25-3475-4102-9599-817ced2f815b", "PicsArt摄影工作室是最流行的免费手机照片编辑器和增长最快的照片艺术家的社交网络之一,作品最大的画廊,让每个人都成为一个伟大的艺术家！")
            };
            ////recommendApps.Add(new RecommendApp(,, ""));
            return recommendApps;
        }

    }
}
