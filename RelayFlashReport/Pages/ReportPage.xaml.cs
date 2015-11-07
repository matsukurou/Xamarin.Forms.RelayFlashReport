using System;
using System.Collections.Generic;
using System.Diagnostics;

using Xamarin.Forms;

namespace RelayFlashReport
{
    public partial class ReportPage : ContentPage
    {
        TimeSpan TotalTime = TimeSpan.Zero;

        TimeSpan BeforeTotalTime = TimeSpan.Zero;

        TimeSpan CurrentRunnerTime { get { return (TotalTime - BeforeTotalTime); } }

        Stopwatch stopWatch = new Stopwatch();

        public ReportPage()
        {
            InitializeComponent();

            for (int i = 0; i < 11; i++)
            {
                // 表示位置合わせ
                var number = (i + 1).ToString();
                if (i + 1 < 10)
                    number = "  " + (i + 1).ToString();
                
                // 設定アイテムパラメータを生成
                var item = new ReportCellItem()
                {
                    Number = number,
                    Name = "Runner " + (i + 1).ToString() + "            ",
                    Lap = TimeSpanToString(TimeSpan.Zero, false),
                    Total = TimeSpanToString(TimeSpan.Zero, false),
                };
                // アイテムレイアウトを設定
                var cell = new ReportCell();
                // レイアウトにアイテムを設定
                cell.BindingContext = item;
                // ページにアイテムを追加
                ListReport.Children.Add(cell);
            }

            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        void Initialize()
        {
            LabelTotalTime.Text = TimeSpanToString(TimeSpan.Zero);

            LabelCurrentRunnderTime.Text = TimeSpanToString(TimeSpan.Zero);

            foreach (View view in ListReport.Children)
            {
                if (view is ReportCell)
                {
                    // 設定するCellを取得
                    var cell = (ReportCell)view;

                    // 記録済みフラグをOFF
                    cell.IsRecorded = false;

                    // トータルタイムを設定
                    cell.TotalTime = TimeSpanToString(TimeSpan.Zero, false);

                    // ラップタイムを設定
                    cell.LapTime = TimeSpanToString(TimeSpan.Zero, false);

                    // ここまでのトータルタイムを保管
                    BeforeTotalTime = TotalTime;

                    break;
                }
            }
        }

        /// <summary>
        /// スタートボタン押し
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void ButtonStart_Clicked(object sender, EventArgs e)
        {
            // ストップウォッチをリセットしてスタート
            stopWatch.Restart();

            ButtonStart.IsVisible = false;

            ButtonLap.IsVisible = true;

            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
                {
                    // 全経過時間を設定
                    TotalTime = stopWatch.Elapsed;

                    // 全経過時間をラベルに設定
                    LabelTotalTime.Text = TimeSpanToString(TotalTime);

                    // 現在走者の経過時間をラベルに設定
                    LabelCurrentRunnderTime.Text = TimeSpanToString(CurrentRunnerTime);

                    return true;
                });
        }

        /// <summary>
        /// ラップボタン押し
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void ButtonLap_Clicked(object sender, EventArgs e)
        {
            // タイムを記録
            RecordTime();
        }

        /// <summary>
        /// ストップボタン押し
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void ButtonStop_Clicked(object sender, EventArgs e)
        {
            // ストップウォッチ停止
            stopWatch.Stop();

            // タイムを記録
            RecordTime();

            ButtonReset.IsVisible = true;
        }

        /// <summary>
        /// リセットボタン押し
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void ButtonReset_Clicked(object sender, EventArgs e)
        {
            // ストップウォッチ初期化
            stopWatch.Reset();

            // 初期化
            Initialize();
        }

        /// <summary>
        /// タイムを保管
        /// </summary>
        void RecordTime()
        {
            foreach (View view in ListReport.Children)
            {
                if (view is ReportCell)
                {
                    // 設定するCellを取得
                    var cell = (ReportCell)view;

                    // 記録済みのランナーか判定
                    if (cell.IsRecorded)
                        continue;

                    // 記録済みに設定
                    cell.IsRecorded = true;

                    // トータルタイムを設定
                    cell.TotalTime = TimeSpanToString(TotalTime, false);

                    // ラップタイムを設定
                    cell.LapTime = TimeSpanToString(CurrentRunnerTime, false);

                    // ここまでのトータルタイムを保管
                    BeforeTotalTime = TotalTime;

                    break;
                }
            }
        }

        /// <summary>
        /// TimeSpanを表示形式のStringへ変換
        /// </summary>
        /// <returns>The span to string.</returns>
        /// <param name="timeSpan">Time span.</param>
        string TimeSpanToString(TimeSpan timeSpan, bool isMilliseconds = true)
        {
            if (isMilliseconds)
                return timeSpan.ToString(@"hh\:mm\:ss\:ff");
            else
                return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }

    public class ReportCellItem
    {
        public string Number { get; set; }

        public string Name { get; set; }

        public string Lap { get; set; }

        public string Total { get; set; }

        public ReportCellItem()
        {
            
        }
    }
}

