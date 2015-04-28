using MLM.DB;
using MLM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MLM
{
    public class SPRepository<T> : BaseRepository<T> where T : MLMBaseEntity
    {
        #region "Agent"

        public string GetAgentApplicableSponsorCode(string providedSponsorCode, AgentPosition agentPosition)
        {

            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@ProvidedSponsorCode", providedSponsorCode),
                new SqlParameter("@Position", agentPosition),
            };

            string AgentApplicableSponsorCode = null;
            using (var reader = SQLRepository.ExecuteProcedure("usp_GetAgentApplicableSponsorCode", sqlParameters.ToArray()))
            {
                while (reader.Read())
                {
                    AgentApplicableSponsorCode = reader["AgentApplicableSponsorCode"].ToString();
                }
            }
            return AgentApplicableSponsorCode;
        }

        public void AgentSponsorRegistrationConditions(string agentcode, string sponsorCode, string introducerCode, AgentPosition position)
        {

            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Agentcode", agentcode),
                new SqlParameter("@SponsorCode", sponsorCode),
                new SqlParameter("@IntroducerCode", introducerCode),
                new SqlParameter("@Position", position),
            };

            using (var reader = SQLRepository.ExecuteProcedure("usp_AgentSponsorRegistrationConditions", sqlParameters.ToArray()))
            {
                
            }
            
        }

        public List<AgentTreeViewModel> AgentTree(string agentcode)
        {

            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Agentcode", agentcode),
            };

            var AgentTreeViewModelData = new List<AgentTreeViewModel>();

            using (var reader = SQLRepository.ExecuteProcedure("usp_AgentTree", sqlParameters.ToArray()))
            {
                while (reader.Read())
                {
                    var agentTreeViewModel = new AgentTreeViewModel
                    {
                        SrNo = Convert.ToInt32(reader["SrNo"]),
                        AgentCode = reader["AgentCode"].ToString(),
                        SponsorCode = reader["SponsorCode"].ToString(),
                        IntroducerCode = reader["IntroducerCode"].ToString(),
                        Position = (AgentPosition)Convert.ToInt16(reader["Position"]),
                        Status = (AgentStatus)Convert.ToInt16(reader["status"]),
                        AgentName = reader["Name"].ToString(),
                    };
                    AgentTreeViewModelData.Add(agentTreeViewModel);
                }
            }
            return AgentTreeViewModelData;
        }

        public AgentDetailsViewModel GetAgentDetails(string agentCode)
        {

            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@agentCode", agentCode),
            };

            var agentDetails = new AgentDetailsViewModel();
            using (var reader = SQLRepository.ExecuteProcedure("usp_AgentDetails", sqlParameters.ToArray()))
            {
                while (reader.Read())
                {
                    agentDetails.TotalLeft = Convert.ToInt32(reader["leftcount"]);
                    agentDetails.TotalRight = Convert.ToInt32(reader["rightcount"]);
                    agentDetails.AgentCode = agentCode;
                }
            }
            return agentDetails;
        }


        #endregion

        #region "Payout"

        //public List<Payout> AgentsPayouts(DateTime startDate, DateTime endDate)
        //{

        //    var sqlParameters = new List<SqlParameter>
        //    {
        //        new SqlParameter("@VoucherStartDate", startDate),
        //        new SqlParameter("@VoucherEndDate", endDate),
        //    };

        //    var payoutList = new List<Payout>();

        //    using (var reader = SQLRepository.ExecuteProcedure("usp_AgentsPayouts", sqlParameters.ToArray()))
        //    {
        //        while (reader.Read())
        //        {
        //            var payout = new Payout
        //            {
        //                VoucherDate = Convert.ToDateTime(reader["VoucherDate"]),
        //                Agentcode = reader["Agentcode"].ToString(),
        //                PAN = reader["PAN"].ToString(),
        //                TotalLeftPair = Convert.ToInt32(reader["TotalLeftPair"]),
        //                TotalRightPair = Convert.ToInt32(reader["TotalRightPair"]),
        //                PairsInThisPayout = Convert.ToInt32(reader["PairsInThisPayout"]),
        //                SaveIncome = Convert.ToDecimal(reader["SaveIncome"]),
        //                PairIncome = Convert.ToDecimal(reader["PairIncome"]),
        //                TDS = Convert.ToDecimal(reader["TDS"]),
        //                ProcessingCharges = Convert.ToDecimal(reader["ProcessingCharges"]),
        //                NetIncome = Convert.ToDecimal(reader["NetIncome"]),
        //                DispatchedAmount = Convert.ToDecimal(reader["DispatchedAmount"]),
        //                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
        //                UpdateDate = Convert.ToDateTime(reader["UpdateDate"]),
        //                TotalLeftPairPV = Convert.ToInt32(reader["TotalLeftPairPV"]),
        //                TotalRightPairPV = Convert.ToInt32(reader["TotalRightPairPV"]),
        //                PairsPVInThisPayout = Convert.ToInt32(reader["PairsPVInThisPayout"])
        //            };
        //            payoutList.Add(payout);
        //        }
        //    }
        //    return payoutList;
        //}


        #endregion


    }
}