using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using common;

namespace business
{
    public class Hit
    {
        public Hit(IStore store)
        {
            Store = store;
            //Repo = (Repo)Store.Load();
        }
        ~Hit()
        {
            Store.Store(Repo);
        }

        static public IStore Store{get;set;}
        static Repo Repo;
        static public void init(string[] args)
        {
            if(Directory.Exists(".hit"))
                Directory.Delete(".hit", true);
            
            DirectoryInfo directoryInfo = Directory.CreateDirectory(".hit");

            File.WriteAllText(".hit/HEAD", "ref: refs/heads/master");

            Directory.CreateDirectory(".hit/refs");
            Directory.CreateDirectory(".hit/refs/heads");
            Directory.CreateDirectory(".hit/refs/tags");

            Directory.CreateDirectory(".hit/objects");
  
            Console.WriteLine($"Initialized empty Hit repository in {directoryInfo.FullName}");

            Repo.Branches.Add(new Branch(){Name = "master", Active = true});

            Store.Store(Repo);
        }
        static public void add (string[] args)
        {
            string filename = args[1];
            
            if(ActiveBranch().FileHistories.Where(x => x.Name == filename).Any())
                throw new ConstraintException("already existing!");

            FileSnap fileSnap = new FileSnap(){FileState = FileState.Staged, Content = File.ReadAllBytes(filename)};
            FileHistory fileHistory = new FileHistory(){Name = filename};
            fileHistory.FileSnaps.Add(fileSnap);

            ActiveBranch().FileHistories.Add(fileHistory);
        }
        static Branch ActiveBranch()
        {
            return Repo.Branches.Where(x => x.Active).First();
        }
        static public void status (string[] args)
        {
            Console.WriteLine($"On branch {ActiveBranch().Name}");
 
            if(!ActiveBranch().FileHistories.Any())
                Console.WriteLine("No commits yet");
            
            if(!ActiveBranch().FileHistories.Any())
                Console.WriteLine("nothing to commit (create/copy files and use hit add to track)");
            
        }

    }
}

