using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Model.Dtos.Tracks
{
    public class TrackDto
    {
        public string Name { get; set; }
    }

    public class TopTrackDto() : TrackDto
    {
        public int PurchaseCount { get; set; }
        public string Artist { get; set; }
    }
}
