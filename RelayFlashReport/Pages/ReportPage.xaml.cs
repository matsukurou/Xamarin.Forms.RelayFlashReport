using System;
using System.Collections.Generic;
using System.Diagnostics;

using Xamarin.Forms;

namespace RelayFlashReport
{
    public partial class ReportPage : ContentPage
    {
        const int CellCount = 21;

        /// <summary>
        /// スタート時間
        /// </summary>
        DateTime StartTime;

        /// <summary>
        /// 走りきった時間
        /// </summary>
        DateTime LastTime;

        /// <summary>
        /// 経過時間
        /// Continue時に使用
        /// </summary>
        TimeSpan ElapsedTime = TimeSpan.Zero;

        /// <summary>
        /// 速報中か判定
        /// </summary>
        bool isReporting;

        public ReportPage()
        {
            InitializeComponent();

            string[] runner = new string[]
            {
                    "神谷",
                    "秋山",
                    "江蔵",
                    "宮原",
                    "中村",
                    "青葉",
                    "松崎",
                    "神谷",
                    "秋山",
                    "江蔵",
                    "宮原",
                    "中村",
                    "青葉",
                    "松崎",
                    "神谷",
                    "秋山",
                    "江蔵",
                    "宮原",
                    "中村",
                    "青葉",
                    "松崎",
            };

            for (int i = 0; i < CellCount; i++)
            {
                // 表示位置合わせ
                var number = (i + 1).ToString();
                if (i + 1 < 10)
                    number = "  " + (i + 1).ToString();
                
                // 設定アイテムパラメータを生成
                var item = new ReportCellItem()
                {
                    Number = number,
                    Name = runner[i], //"Runner " + (i + 1).ToString() + "            ",
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
            // トータルタイムの初期化
            LabelTotalTime.Text = TimeSpanToString(TimeSpan.Zero);

            // ラップタイムの初期化
            LabelCurrentRunnderTime.Text = TimeSpanToString(TimeSpan.Zero);

            // セルの初期化
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

                    // 色の設定
                    cell.BackgroundColor = Color.Default;
                }
            }
        }

        /// <summary>
        /// 速報イベントを設定する
        /// </summary>
        void SetReportEvent()
        {
            // タイマーで10ms秒ごとに呼び出す
            // falseを返すとタイマー終了
            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
                {
                    if (isReporting == false)
                        return false;

                    // 速報を設定する
                    SetFlashReport(false);

                    return true;
                });
        }

        /// <summary>
        /// 速報の設定
        /// </summary>
        /// <param name="isSetLap">If set to <c>true</c> is set lap.</param>
        void SetFlashReport(bool isSetLap)
        {
            // 現在時間を取得
            var now = DateTime.Now;

            // トータルタイムを取得
            //TotalTime = stopWatch.Elapsed;
            var totalTime = now - LastTime + ElapsedTime;

            // ラップタイムを取得
            var lapTime = now - LastTime;

            // 全経過時間をラベルに設定
            //LabelTotalTime.Text = TimeSpanToString(totalTime);
            LabelTotalTime.Text = TimeSpanToString(totalTime);

            // 現在走者の経過時間をラベルに設定
            LabelCurrentRunnderTime.Text = TimeSpanToString(lapTime);

            if (isSetLap)
            {
                // 経過時間を保管
                ElapsedTime += (now - LastTime);

                // ここまでの時間を保管
                LastTime = now;

                // セルに速報値を設定する
                SetReportTimeCell(totalTime, lapTime);
            }                        
        }

        /// <summary>
        /// セルに速報値を設定する
        /// </summary>
        void SetReportTimeCell(TimeSpan totalTime, TimeSpan lapTime)
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
                    cell.TotalTime = TimeSpanToString(totalTime, false);

                    // ラップタイムを設定
                    cell.LapTime = TimeSpanToString(lapTime, false);

                    break;
                }
            }
        }

        /// <summary>
        /// セルの色を設定
        /// </summary>
        void SetReportTimeCellColor()
        {
            foreach (View view in ListReport.Children)
            {
                if (view is ReportCell)
                {
                    // 設定するCellを取得
                    var cell = (ReportCell)view;

                    cell.BackgroundColor = Color.Default;

                    // 記録済みのランナーか判定
                    if (cell.IsRecorded == false)
                    {
                        cell.BackgroundColor = Color.Fuchsia;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// ボタンの有効無効を設定
        /// </summary>
        /// <param name="isEnableStart">If set to <c>true</c> is enable start.</param>
        /// <param name="isEnableLap">If set to <c>true</c> is enable lap.</param>
        /// <param name="isEnableStop">If set to <c>true</c> is enable stop.</param>
        /// <param name="isEnableReset">If set to <c>true</c> is enable reset.</param>
        void SetButtonEnabled(bool isEnableStart, bool isEnableLap, bool isEnableStop, bool isEnableReset)
        {
            ButtonStart.IsEnabled = isEnableStart;
            ButtonLap.IsEnabled = isEnableLap;
            ButtonStop.IsEnabled = isEnableStop;
            ButtonReset.IsEnabled = isEnableReset;
        }

        /// <summary>
        /// スタートボタン押し
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void ButtonStart_Clicked(object sender, EventArgs e)
        {
            // 現在時間を取得
            var now = DateTime.Now;

            // スタート時間を保管
            StartTime = now;

            // ランナータイムを保管
            LastTime = now;

            // セルの色を設定
            SetReportTimeCellColor();

            // ボタンの有効無効を設定
            SetButtonEnabled(false, true, true, false);

            // 速報イベント開始
            isReporting = true;

            // 速報イベントを設定
            SetReportEvent();
        }
            
        /// <summary>
        /// ラップボタン押し
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void ButtonLap_Clicked(object sender, EventArgs e)
        {            
            // 速報値を設定
            SetFlashReport(true);

            // セルの色を設定
            SetReportTimeCellColor();
        }

        /// <summary>
        /// ストップボタン押し
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void ButtonStop_Clicked(object sender, EventArgs e)
        {
            if (isReporting == false)
                return;

            // 速報イベントを終了
            isReporting = false;

            // 速報値を設定する
            SetFlashReport(true);

            // ボタンの有効無効を設定
            SetButtonEnabled(true, false, false, true);
        }

        /// <summary>
        /// リセットボタン押し
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void ButtonReset_Clicked(object sender, EventArgs e)
        {
            // 初期化
            Initialize();

            // 経過時間をここで初期化
            ElapsedTime = TimeSpan.Zero;

            // ボタンの有効無効を設定
            SetButtonEnabled(true, false, false, false);
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
}

