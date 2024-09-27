using System;

namespace business
{
    public class HitObject
    {
        public string Name { get; set; }
        public object Hash{get;set;}
        public DateTime Creation{get; set;}
        public DateTime LastChange{get; set;}

        public HitObject()
        {
            Creation = LastChange = DateTime.Now;
        }
    }
}
