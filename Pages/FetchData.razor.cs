using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TwoPhaseCommit.Data;

namespace TwoPhaseCommit.Pages
{
    public class FetchDataBase : ComponentBase
    {
        protected List<TwoPhaseEntity> twoPhaseEntity = new List<TwoPhaseEntity>();
        protected SqlConnection con = new SqlConnection("Data Source=DESKTOP-LMJQODQ;Initial Catalog=Transections;Integrated Security=false;Trusted_Connection=yes;");

        protected List<TemporaryChange> Temporaries= new List<TemporaryChange>();
        protected string Transectionstatus = string.Empty;

        protected bool IsAbortClicked;
        protected void Abort ()
        {
            IsAbortClicked = true;
            twoPhaseEntity = new List<TwoPhaseEntity>();
            Transectionstatus = "Transaction Aborted";
        }
        protected override void OnInitialized()
        {
            SqlCommand cmd = new SqlCommand("Select * from TemeroryChanges", con);
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Temporaries.Add(new TemporaryChange
                {
                    ChangeBalance = Convert.ToInt32(sdr["TempChanges"])
                });
            }
            con.CloseAsync();
            refreshdata();
        }

        

        private void refreshdata()
        {
            twoPhaseEntity = new List<TwoPhaseEntity>();
            SqlCommand cmd = new SqlCommand("Select * from tbl_transections", con);
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                twoPhaseEntity.Add(new TwoPhaseEntity
                {
                    Id = Convert.ToInt32(sdr["Id"]),
                    AccountNumber = sdr["AccountNumber"].ToString(),
                    CustomerName = sdr["CustomerName"].ToString(),
                    Balance = Convert.ToInt32(sdr["Balance"]),
                    TemChanges = Temporaries.Select(e=>e.ChangeBalance).LastOrDefault()

                });
            }
            con.Close();
        }

        protected void SaveTransection(TwoPhaseEntity entity)
        {
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            try
            {

                SqlCommand cmd = new SqlCommand("Update tbl_transections set Balance = Balance + " + entity.TemChanges + " where AccountNumber = '" + entity.AccountNumber + "'", con, trans);
                cmd.ExecuteNonQuery();
                if (!IsAbortClicked)
                {

                    trans.Commit();
                    Transectionstatus = "Transaction Successful";
                    entity = new TwoPhaseEntity();
                }
                else
                {
                    trans.Rollback();
                    Transectionstatus = "Transaction Aborted";
                    IsAbortClicked = false;

                }

            }
            catch (Exception ex)
            {

                trans.Rollback();
                Transectionstatus = "Transaction Failed";
            }
            con.Close();
            refreshdata();
        }
    }
}