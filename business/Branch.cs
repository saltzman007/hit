using System.Collections.Generic;

namespace business
{
    public class Branch : HitObject
    {
        public bool Active{get; set;}
        public List<FileHistory>FileHistories{get;set;} = new();
    }
}

