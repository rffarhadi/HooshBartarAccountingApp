using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HooshBartarAccountingApp
{
    class Reports
    {
        public string Tarikh { get; set; }
        public Int64 IdHesab { get; set; }
        public string NameHesab { get; set; }
        public string SubGroupHesab { get; set; }
        public string TabagehHesab { get; set; }
        public Int64? BalanceTotalVolume { get; set; }
        public double? TodayBalanceAvg { get; set; }
        public Int64? BedehkarTurnOver { get; set; }
        public Int64? BestankarTurnOver { get; set; }
        public Int64? Balance { get; set; }
        public string Mahiyat { get; set; }
    }

    class ReportOfDafater
    {
        public decimal? IdSanad { get; set; }
        public string Tarikh { get; set; }
        public Int64? IdHesab { get; set; }
        public string NameHesab { get; set; }
        public string GroupHesab { get; set; }
        public string SubGroupHesab { get; set; }
        public string SanadTozih { get; set; }
        public int? BuySellVol { get; set; }
        public int? ArzeshAfzudehTax { get; set; }
        public int? EnteghaTax { get; set; }
        public int? KarmozdMoameleh { get; set; }
        public Int64? BedehkarTurnOver { get; set; }
        public Int64? BestankarTurnOver { get; set; }
        public Int64? Balance { get; set; }
        public string Mahiyat { get; set; }
    }
}
