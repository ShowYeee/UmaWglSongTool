using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UmaWglSongTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (StreamReader r = new StreamReader("assests/SongList.json"))
            {
                string json = r.ReadToEnd();
                var source = JsonConvert.DeserializeObject<List<JsonResult>>(json);
                if(source != null)
                {
                    SongList_1.ItemsSource = source[0].Datas.Select(item => { item.ImgPath = System.IO.Path.GetFullPath($"assests/songs/{item.Id}.jpg"); return item; }).ToList<ListModel>();
                    SongList_2.ItemsSource = source[1].Datas.Select(item => { item.ImgPath = System.IO.Path.GetFullPath($"assests/songs/{item.Id}.jpg"); return item; }).ToList<ListModel>();
                    SongList_3.ItemsSource = source[2].Datas.Select(item => { item.ImgPath = System.IO.Path.GetFullPath($"assests/songs/{item.Id}.jpg"); return item; }).ToList<ListModel>();
                    SongList_4.ItemsSource = source[3].Datas.Select(item => { item.ImgPath = System.IO.Path.GetFullPath($"assests/songs/{item.Id}.jpg"); return item; }).ToList<ListModel>();
                    SongList_5.ItemsSource = source[4].Datas.Select(item => { item.ImgPath = System.IO.Path.GetFullPath($"assests/songs/{item.Id}.jpg"); return item; }).ToList<ListModel>();
                }

                ComputeTotal();
            }
        }

        private void IsChecked_Click(object sender, RoutedEventArgs e)
        {
            ComputeTotal();
        }

        private void ComputeTotal()
        {
            var dataSource_1 = SongList_1.ItemsSource as IEnumerable<ListModel>;
            var dataSource_2 = SongList_2.ItemsSource as IEnumerable<ListModel>;
            var dataSource_3 = SongList_3.ItemsSource as IEnumerable<ListModel>;
            var dataSource_4 = SongList_4.ItemsSource as IEnumerable<ListModel>;
            var dataSource_5 = SongList_5.ItemsSource as IEnumerable<ListModel>;

            #region 所需點數計算
            var totalList = new List<ResultModel>();

            var unCheckedRows_1 = dataSource_1.Where(item => item.IsChecked == false);
            totalList.Add(new ResultModel
            {
                Space = "① 第一年",
                Da = unCheckedRows_1.Sum(x => x.Da),
                Pa = unCheckedRows_1.Sum(x => x.Pa),
                Vo = unCheckedRows_1.Sum(x => x.Vo),
                Vi = unCheckedRows_1.Sum(x => x.Vi),
                Me = unCheckedRows_1.Sum(x => x.Me),
            });
          
            var unCheckedRows_2 = dataSource_2.Where(item => item.IsChecked == false);
            totalList.Add(new ResultModel
            {
                Space = "② 第二年 - 前半",
                Da = unCheckedRows_2.Sum(x => x.Da),
                Pa = unCheckedRows_2.Sum(x => x.Pa),
                Vo = unCheckedRows_2.Sum(x => x.Vo),
                Vi = unCheckedRows_2.Sum(x => x.Vi),
                Me = unCheckedRows_2.Sum(x => x.Me),
            });
 
            var unCheckedRows_3 = dataSource_3.Where(item => item.IsChecked == false);
            totalList.Add(new ResultModel
            {
                Space = "③ 第二年 - 後半",
                Da = unCheckedRows_3.Sum(x => x.Da),
                Pa = unCheckedRows_3.Sum(x => x.Pa),
                Vo = unCheckedRows_3.Sum(x => x.Vo),
                Vi = unCheckedRows_3.Sum(x => x.Vi),
                Me = unCheckedRows_3.Sum(x => x.Me),
            });

    
            var unCheckedRows_4 = dataSource_4.Where(item => item.IsChecked == false);
            totalList.Add(new ResultModel
            {
                Space = "④ 第三年",
                Da = unCheckedRows_4.Sum(x => x.Da),
                Pa = unCheckedRows_4.Sum(x => x.Pa),
                Vo = unCheckedRows_4.Sum(x => x.Vo),
                Vi = unCheckedRows_4.Sum(x => x.Vi),
                Me = unCheckedRows_4.Sum(x => x.Me),
            });

            totalList.Add(new ResultModel
            {
                Space = "① + ②",
                Da = totalList[0].Da + totalList[1].Da,
                Pa = totalList[0].Pa + totalList[1].Pa,
                Vo = totalList[0].Vo + totalList[1].Vo,
                Vi = totalList[0].Vi + totalList[1].Vi,
                Me = totalList[0].Me + totalList[1].Me,
            });

            totalList.Add(new ResultModel
            {
                Space = "① + ② + ③",
                Da = totalList[0].Da + totalList[1].Da + totalList[2].Da,
                Pa = totalList[0].Pa + totalList[1].Pa + totalList[2].Pa,
                Vo = totalList[0].Vo + totalList[1].Vo + totalList[2].Vo,
                Vi = totalList[0].Vi + totalList[1].Vi + totalList[2].Vi,
                Me = totalList[0].Me + totalList[1].Me + totalList[2].Me,
            });

            totalList.Add(new ResultModel
            {
                Space = "總計",
                Da = totalList[0].Da + totalList[1].Da + totalList[2].Da + totalList[3].Da,
                Pa = totalList[0].Pa + totalList[1].Pa + totalList[2].Pa + totalList[3].Pa,
                Vo = totalList[0].Vo + totalList[1].Vo + totalList[2].Vo + totalList[3].Vo,
                Vi = totalList[0].Vi + totalList[1].Vi + totalList[2].Vi + totalList[3].Vi,
                Me = totalList[0].Me + totalList[1].Me + totalList[2].Me + totalList[3].Me,
            });

            TotalList.ItemsSource = totalList;
            #endregion

            #region 其他計算
            int count_1 = dataSource_1.Where(item => item.IsChecked == true).Count();
            int count_2 = dataSource_2.Where(item => item.IsChecked == true).Count();
            int count_3 = dataSource_3.Where(item => item.IsChecked == true).Count();
            int count_4 = dataSource_4.Where(item => item.IsChecked == true).Count();
            int count_5 = dataSource_5.Where(item => item.IsChecked == true).Count();
            int songCount = count_1 + count_2 + count_3 + count_4 + count_5;
            SongCountLabel.Content = $"所持歌曲數: {songCount}";
            #endregion

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dataSource_1 = SongList_1.ItemsSource as IEnumerable<ListModel>;
            var dataSource_2 = SongList_2.ItemsSource as IEnumerable<ListModel>;
            var dataSource_3 = SongList_3.ItemsSource as IEnumerable<ListModel>;
            var dataSource_4 = SongList_4.ItemsSource as IEnumerable<ListModel>;
            var dataSource_5 = SongList_5.ItemsSource as IEnumerable<ListModel>;

            SongList_1.ItemsSource = dataSource_1.Select(item => { item.IsChecked = false; return item; }).ToList<ListModel>();
            SongList_2.ItemsSource = dataSource_2.Select(item => { item.IsChecked = false; return item; }).ToList<ListModel>();
            SongList_3.ItemsSource = dataSource_3.Select(item => { item.IsChecked = false; return item; }).ToList<ListModel>();
            SongList_4.ItemsSource = dataSource_4.Select(item => { item.IsChecked = false; return item; }).ToList<ListModel>();
            SongList_5.ItemsSource = dataSource_5.Select(item => { item.IsChecked = false; return item; }).ToList<ListModel>();

            ComputeTotal();

        }
    }

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
        public string? Effect { get; set; }
        public string? Innate { get; set; }
        public string? ImgPath { get; set; }
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
}
