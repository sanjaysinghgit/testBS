using MLM.API.DB;
using MLM.API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MLM.API
{
    public class SPHandler<T> : BaseHandler<T> where T : MLMBaseEntity
    {
        public MLMSqlRepository<MLMBaseEntity> SQLRepository { get; set; }

        public int getAgentsTree(string a)
        {

                        var sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@AgentCode", a),
                    };

                        using (var reader = SQLRepository.ExecuteProcedure("TestAgentTree", sqlParameters.ToArray()))
            {
                while (reader.Read())
                {
                }
            }
            return 1;
        }
    }
}