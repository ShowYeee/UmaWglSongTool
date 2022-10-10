using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using F23.StringSimilarity;
using UmaWglSongTool.Utility;
using System.Threading;

namespace UmaWglSongTool
{

    [ValueConversion(typeof(int), typeof(String))]
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int price = (int)value;
            if (price == 0)
            {
                return "-";
            }
            return price.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        private List<ListModel> _Datas = new List<ListModel>();
        private bool _IsCapture = false;
        private ConfigModel _config = new ConfigModel();
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();

            #region 初始化組件屬性
            CaptcureBtn.Content = "擷取視窗";
            YearSelect.SelectedIndex = 0;
            InitConfig();
            #endregion

            _IsCapture = false;      
        }

        #region 組件事件
        private void IsChecked_Click(object sender, RoutedEventArgs e)
        {
            SetGridDataIsCheck();
            GridReload();
        }

        private void YearSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridReload();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearGridData();
            GridReload();
        }

        #endregion

        #region 函式

        private void InitConfig()
        {
            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();
                var source = JsonConvert.DeserializeObject<ConfigModel>(json);
                if (source != null)
                {
                    _config = source;
                    ProcessNameText.Text = _config.Capture.ProcessName;
                }

            }
        }

        private void LoadData()
        {
            using (StreamReader r = new StreamReader("assests/SongList.json"))
            {
                string json = r.ReadToEnd();
                var source = JsonConvert.DeserializeObject<List<ListModel>>(json);
                if (source != null)
                {
                    _Datas = source;
                    // 轉換成 Path
                    _Datas.Select(item => { item.ImgPath = System.IO.Path.GetFullPath($"assests/image/songs/{item.Id}.jpg"); return item; }).ToList<ListModel>();
                }
            }
        }

        private void GridReload()
        {
            Action action = () =>
            {
                var datas = GetYearOfData(YearSelect.SelectedIndex);
                SongList.ItemsSource = null;
                SongList.ItemsSource = datas;
                ComputeTotal();
            };
            YearSelect.Dispatcher.BeginInvoke(action);
        }

        private void SetGridDataIsCheck()
        {
            var TableData = SongList.ItemsSource as IEnumerable<ListModel>;
            if (TableData != null)
            {
                foreach (var item in TableData)
                {
                    _Datas.SingleOrDefault(x => x.Id == item.Id).IsChecked = item.IsChecked;
                }
            }
        }

        private List<ListModel> GetYearOfData(int index)
        {
            var datas = new List<ListModel>();
            var TableData = SongList.ItemsSource as IEnumerable<ListModel>;

            if (index > 0)
            {
                datas = _Datas.Where(x => x.Year == index).ToList<ListModel>();
            }
            else
            {
                datas = _Datas;
            }
            return datas;
        }

        private void ClearGridData()
        {
            _Datas = _Datas.Select(item => { item.IsChecked = false; return item; }).ToList<ListModel>();
        }

        private void ComputeTotal()
        {
            #region Performance 所需
            var totalList = new List<ResultModel>();

            var dataSource_1 = _Datas.Where(x => x.Year == 1);
            var dataSource_2 = _Datas.Where(x => x.Year == 2);
            var dataSource_3 = _Datas.Where(x => x.Year == 3);
            var dataSource_4 = _Datas.Where(x => x.Year == 4);
            var dataSource_5 = _Datas.Where(x => x.Year == 5);

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


            SongsCountText.Content = _Datas.Where(x => x.IsChecked == true).Count();
            CardText.Content = _Datas.Where(x => x.IsChecked == true).Sum(x => x.LiveInfo.Card);
            GoodAtText.Content = $"{_Datas.Where(x => x.IsChecked == true).Sum(x => x.LiveInfo.GoodAt)} %";
            FriendshipText.Content = $"{_Datas.Where(x => x.IsChecked == true).Sum(x => x.LiveInfo.Friendship)} %";
        }

        private void CaptcureBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_IsCapture)
            {
                _IsCapture = false;
                tokenSource?.Cancel();
                CaptcureBtn.Content = "擷取視窗";
            }
            else
            {
                _IsCapture = true;
                tokenSource = new CancellationTokenSource();
                Task.Run(AutoCapture, tokenSource.Token);
            }

            ProcessNameText.IsEnabled = !_IsCapture;
        }

        private void AutoCapture()
        {
            int num = 0;
            string pName = string.Empty;

            Action actionP = () => { pName = ProcessNameText.Text; };
            ProcessNameText.Dispatcher.BeginInvoke(actionP);

            while (!tokenSource.IsCancellationRequested)
            {
                double threshold = _config.Capture.Threshold;
                int delay = _config.Capture.Delay;

                try
                {
                    string fileName = "tmp/Capture.jpg";
                    CaptureHelper capture = new CaptureHelper();
                    OcrHelper ocr = new OcrHelper();
                    capture.GetWindowCaptureByName(pName, fileName);
                    string str = ocr.GetTextByImage(fileName).Replace("\n", "");

                    var jw = new JaroWinkler();
                    var item = _Datas.Where(x => jw.Similarity(str, $"「{x.Name}」の楽曲を習得の期待度が上がった") >= threshold).FirstOrDefault();
                    if (item != null)
                    {
                        _Datas.SingleOrDefault(x => x.Id == item.Id).IsChecked = true;
                        GridReload();
                    }
                    num++;

                    Action action = () => { CaptcureBtn.Content = $"擷取視窗 (scan: {num})"; };
                    CaptcureBtn.Dispatcher.BeginInvoke(action);

                    Thread.Sleep(delay);
                }
                catch (Exception e)
                {
                    _IsCapture = false;
                    tokenSource?.Cancel();
                    Action action = () => {
                        CaptcureBtn.Content = "擷取錯誤!";
                        ProcessNameText.IsEnabled = !_IsCapture;
                    };
                    CaptcureBtn.Dispatcher.BeginInvoke(action);
                }
            }
        }

        #endregion
    }
}
