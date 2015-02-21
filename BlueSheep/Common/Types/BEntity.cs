using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Common.Types
{
    public class BEntity
    {
        public BEntity(int id, int cellId)
        {
            Id = id;
            CellId = cellId;
        }

        public int CellId { get; internal set; }
        public int Id { get; protected set; }
    }
}
