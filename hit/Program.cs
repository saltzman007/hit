using System;
using business;
using store;

namespace hit
{
    class Program
    {
        static void Main(string[] args)
        {

            Hit hit = new Hit(new Persist());
            var type = typeof(Hit);
            var myMethod = type.GetMethod(args[0]);

            myMethod.Invoke(null, new object[]{args} );
        }
    }
}
