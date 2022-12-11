using Microsoft.AspNetCore.Components;
using System.Data.SqlClient;
using System;
using System.Threading.Tasks;
using TwoPhaseCommit.Data;
using TwoPhaseCommit.Service;
using System.Collections.Generic;

namespace TwoPhaseCommit.Pages
{
    public class SiteBase : ComponentBase
    {
        protected SqlConnection con = new SqlConnection("Data Source=DESKTOP-LMJQODQ;Initial Catalog=Transections;Integrated Security=false;Trusted_Connection=yes;");

        protected TwoPhaseEntity entity = new TwoPhaseEntity();

        private coordinatorService servive = new coordinatorService();
        protected List<TemporaryChange> Temporaries = new List<TemporaryChange>();

        protected override void OnInitialized()
        {
            SqlCommand cmd = new SqlCommand("Select * from TemeroryChanges", con);
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Temporaries.Add(new TemporaryChange
                {
                    Id = Convert.ToInt32(sdr["Id"]),
                    ChangeBalance = Convert.ToInt32(sdr["TempChanges"])
                });
            }
            con.CloseAsync();

        }

        protected async Task SendChangesToAllNode(TwoPhaseEntity phaseEntity)
        {
            if(phaseEntity.Balance != 0)
            {
                servive.SendMessageToNodes(phaseEntity);
                this.OnInitialized();
                StateHasChanged();
            }
        }
    }

}
