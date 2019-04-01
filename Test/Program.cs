using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using DAL;
using Newtonsoft.Json;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Class1 c1 = new Test.Class1();
            Class2 c2 = new Test.Class2();
            Console.WriteLine(c1.divide(2, 1));
            Console.WriteLine(c1.subtract(2, 1));
            Console.WriteLine(c2.sum(2, 1));
            Console.WriteLine(c2.multi(2, 1));
        }
    }
}

/*DAL.DAL theDal = DAL.DAL.getInstance();

            var user = new User();
            user.name = "Moti";
            user.password = "1234";
            user.ip = "100.100.100.100";
            user.port = 8086;
            user.isAvailable = true;
            theDal.addUser(user);

            var user2 = new User();
            user2.name = "Elbaz";
            user2.password = "4321";
            user2.ip = "200.200.200.200";
            user2.port = 8086;
            user2.isAvailable = false;
            theDal.addUser(user2);

            var file = new File();
            file.id = 1;
            file.name = "File1";
            file.size = 4;
            file.allUsers = "Moti";
            theDal.addFile(file);

            var file2 = new File();
            file2.id = 2;
            file2.name = "File2";
            file2.size = 4;
            file2.allUsers = "Elbaz";
            theDal.addFile(file2);

            Console.WriteLine(theDal.contain(file));
            Console.WriteLine(theDal.getFileUsersList("File1").Count);

            foreach (var u in theDal.getFileUsersList("File1"))
            {
                Console.WriteLine(u.name + " , " + file.name);
            }

            theDal.login("Elbaz");

            Console.WriteLine(theDal.contain(file));
            Console.WriteLine(theDal.getFileUsersList("File1").Count);

            foreach (var u in theDal.getFileUsersList("File1"))
            {
                Console.WriteLine(u.name + " , " + file.name);
            }

            foreach (var f in theDal.getFiles())
            {
                Console.WriteLine(f.name);
            }*/
