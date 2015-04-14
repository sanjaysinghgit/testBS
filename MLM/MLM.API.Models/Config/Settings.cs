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
        [DefaultValue("MLM")]
        public string Company { get; set; }

        [DefaultValue("10")]
        public decimal TDSWithPAN { get; set; }

        [DefaultValue("20")]
        public decimal TDSWithOutPAN { get; set; }

        [DefaultValue("0")]
        public decimal VoucherProcessingCharge { get; set; }

        [DefaultValue("0")]
        public decimal VoucherProcessingChargeCapping { get; set; }

        [DefaultValue("15")]
        public int SaveIncomeDaysDuration { get; set; }

        [DefaultValue("100")]
        public int SaveIncomeAmount { get; set; }

        [DefaultValue(BinaryIncomeType.ByPair)]
        public BinaryIncomeType BinaryIncomeSetting { get; set; }

        /// <summary>
        /// Not for Recent. Other companies follow fix pairincome rule where it will be applicable.
        /// So we can say companies who follow multiple rules for joining kit do not use this.
        /// </summary>
        [DefaultValue("200")]
        public int PairIncomeAmout { get; set; }

        [DefaultValue("1")]
        public int PVAmount { get; set; }

        [DefaultValue("10000")]
        public int WeeklyBinaryCapping { get; set; }

        [DefaultValue("50000")]
        public int MonthlyBinaryCapping { get; set; }

        [DefaultValue("10000")]
        public int WeeklyRepurchaseCapping { get; set; }

        [DefaultValue("50000")]
        public int MonthlyRepurchaseCapping { get; set; }

        [DefaultValue(VoucherMode.Weekly)]
        public VoucherMode VoucherStatus { get; set; }

        //ClubIncome c1= new ClubIncome { PairCount=50, DaysDuration=180 };
        //[DefaultValue(c1)]
        //public ClubIncome ClubOne { get; set; }

        //[DefaultValue(new ClubIncome { PairCount = 100, DaysDuration = 180 })]
        //public ClubIncome ClubTwo { get; set; }

        //[DefaultValue(new ClubIncome { PairCount = 150, DaysDuration = 180 })]
        //public ClubIncome ClubThree { get; set; }

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

    public enum BinaryIncomeType
    {
        ByPair = 1,
        ByPV = 2,
    }
}
