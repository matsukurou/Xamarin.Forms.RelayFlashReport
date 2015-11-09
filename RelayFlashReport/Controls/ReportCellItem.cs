using System;

namespace RelayFlashReport
{
    public class ReportCellItem
    {
        public string Number { get; set; }

        public string Name { get; set; }

        public string Lap { get; set; }

        public string Total { get; set; }

        /// <summary>
        /// 速報セルに設定するアイテム
        /// </summary>
        public ReportCellItem()
        {
        }
    }
}

