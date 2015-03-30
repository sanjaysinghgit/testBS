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
    }
}