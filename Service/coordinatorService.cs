using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using TwoPhaseCommit.Data;

namespace TwoPhaseCommit.Service
{
    public class coordinatorService
    {
        protected SqlConnection con = new SqlConnection("Data Source=DESKTOP-LMJQODQ;Initial Catalog=Transections;Integrated Security=false;Trusted_Connection=yes;");

        public TwoPhaseEntity entity = new TwoPhaseEntity();
        public coordinatorService()
        {
                
        }

        public async Task<TwoPhaseEntity> SendMessageToNodes(TwoPhaseEntity tentity)
        {
            try
            {
                con.Open();
                entity.Balance = tentity.Balance;
                SqlCommand cmd = new SqlCommand("INSERT INTO TemeroryChanges (TempChanges) Values(" + tentity.Balance + ")",con);
                await cmd.ExecuteNonQueryAsync();
                return entity;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

    }
}
