using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace RelayFlashReport
{
    public partial class ReportCell : ContentView
    {
        /// <summary>
        /// 記録済みかどうか判定
        /// </summary>
        /// <value><c>true</c> if this instance is recorded; otherwise, <c>false</c>.</value>
        public bool IsRecorded { get; set; }

        public ReportCell()
        {
            InitializeComponent();

            IsRecorded = false;
        }

        public string LapTime
        {
            get
            {
                return LabelLap.Text;
            }

            set
            {

                LabelLap.Text = value;
            }
        }

        public string TotalTime
        {
            get
            {
                return LabelTotal.Text;
            }

            set
            {

                LabelTotal.Text = value;
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return Layout.BackgroundColor;
            }
            set
            {
                Layout.BackgroundColor = value;
            }
        }

        public double NameWidth
        {
            set 
            {
                EntryName.WidthRequest = value;
            }
        }
    }
}