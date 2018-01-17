using System;
using System.Collections.Generic;
using System.Text;

namespace FotbaollsInfo
{
    public class Teams
    {
        public int Id { get; set; }
        public Nullable<int> Team_Id { get; set; }
        public string Name { get; set; }
        public string Country_ { get; set; }
        public string Stadium_ { get; set; }
        public string HomePageURL_ { get; set; }
        public string WIKILink_ { get; set; }
    }
}
