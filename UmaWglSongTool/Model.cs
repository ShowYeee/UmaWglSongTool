using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmaWglSongTool
{
    public class ListModel
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public string? Name { get; set; }
        public int Da { get; set; }
        public int Pa { get; set; }
        public int Vo { get; set; }
        public int Vi { get; set; }
        public int Me { get; set; }
        public string? Live_D { get; set; }
        public string? Now_D { get; set; }
        public string? ImgPath { get; set; }
        public int Year { get; set; }
        public LiveInfoModel LiveInfo { get; set; }
    }

    public class JsonResult
    {
        public int Space { get; set; }
        public List<ListModel> Datas { get; set; }
    }

    public class ResultModel
    {
        public string? Space { get; set; }
        public int Da { get; set; }
        public int Pa { get; set; }
        public int Vo { get; set; }
        public int Vi { get; set; }
        public int Me { get; set; }
    }

    public class WindowPosition
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
    }

    public class LiveInfoModel
    {
        public int Card { get; set; }
        public int GoodAt { get; set; }
        public int Friendship { get; set; }
    }
}
