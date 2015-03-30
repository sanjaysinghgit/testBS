using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models.Config
{
    public class Setting : MLMBaseEntity
    {
        public string Company { get; set; }

        [DefaultValue("15")]
        public int SaveIncomeDaysDuration { get; set; }

        /// <summary>
        /// Not for Recent. Other companies follow fix pairincome rule where it will be applicable.
        /// So we can say companies who follow multiple rules for joining kit do not use this.
        /// </summary>
        [DefaultValue("200")]
        public int PairIncomeAmout { get; set; }

        [DefaultValue("1")]
        public int PVAmount { get; set; }

        //ClubIncome c1= new ClubIncome { PairCount=50, DaysDuration=180 };
        //[DefaultValue(c1)]
        //public ClubIncome ClubOne { get; set; }

        //[DefaultValue(new ClubIncome { PairCount = 100, DaysDuration = 180 })]
        //public ClubIncome ClubTwo { get; set; }

        //[DefaultValue(new ClubIncome { PairCount = 150, DaysDuration = 180 })]
        //public ClubIncome ClubThree { get; set; }

        [DefaultValue(VoucherMode.Monthly)]
        public VoucherMode RepurchasingVoucher { get; set; }

        [DefaultValue(VoucherMode.Weekly)]
        public VoucherMode BinaryVoucher { get; set; }

    }
    public class ClubIncome
    {
        public int PairCount { get; set; }
        public int DaysDuration { get; set; }
    }
    public enum VoucherMode
    {
        Weekly = 1,
        ByMonthly = 2,
        Monthly = 3
    }
}
