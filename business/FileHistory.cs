using System.Collections.Generic;

namespace business
{
    public class FileHistory: HitObject
    {
        public List<FileSnap>FileSnaps{get; set;} = new();
    }
}

