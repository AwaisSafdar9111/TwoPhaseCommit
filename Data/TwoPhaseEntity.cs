using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwoPhaseCommit.Data
{
    public class TwoPhaseEntity
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; }

        public string CustomerName { get; set; }

        public int Balance { get; set; }

        public int TemChanges { get; set; }
    }
}
