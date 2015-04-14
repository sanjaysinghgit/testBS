using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
    public class Agent : MLMBaseEntity
    {
        // TODO: We will use user profile properties in Agent.
        // Agent will also be user.
        // Need to Move user profile logic in Model Project

        /// <summary>
        /// Around 10 digit alphanumeric code
        /// </summary>
        /// 
        public string Code { get; set; }
        public string SponsorCode { get; set; }
        public string IntroducerCode { get; set; }
        public string LeftAgent { get; set; }
        public string RightAgent { get; set; }
        public AgentPosition Position { get; set; }
        //public int CarryLeft { get; set; }
        //public int CarryRight { get; set; }
        public int TotalPaidPairs { get; set; }
        public bool SaveIncomeStatus { get; set; }
        public bool VoucherStatus { get; set; }


        public System.Nullable<DateTime> ActivationDate { get; set; }

        public EPin EPin { get; set; }

        //Agent Joining status
        public AgentStatus Status { get; set; }


        
    }






////     CREATE PROCEDURE [dbo].[Agent_Tree]  
////        @Code nvarchar(max)
////            AS  
////BEGIN 
////  WITH TreeReport (AgentCode,IntroducerCode,SponsorCode,Position,Level)
////    AS
////                    (
////                        //--Create base record T(0)
////                        SELECT a.Code,a.IntroducerCode,a.SponsorCode,a.Position,0 AS Level
////                            FROM dbo.Agent AS a where a.Code=@Code
////                        UNION ALL 
////                        //-- Recurse till T(n)
////                        SELECT a.Code,a.IntroducerCode,a.SponsorCode,a.Position,Level + 1
////                            FROM dbo.Agent AS a 
////                            INNER JOIN TreeReport AS d ON a.SponsorCode = d.AgentCode
////                    )

                    

////                   SELECT 'SrNo' = ROW_NUMBER() OVER(ORDER BY Level asc ),
////                       AgentCode,IntroducerCode,SponsorCode,Position,Level FROM TreeReport "
////                            }
////END





    public enum AgentPosition
    {
        Left = 1,
        Right = 2
    }


    public enum AgentStatus
    {
        Available = 1,
        Pedning = 2, //Guest and active
        Associate = 3, //Not required????
        Executive = 4, // Agent and active
        Suspend = 5
    }  




}
