using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL
    {
        private static DAL theDal;
        private TorrentDBContext db;
        private string conStr = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DELL\\Desktop\\Torrent\\Server\\Server\\App_Data\\TorrentDB.mdf;Integrated Security = True";

        public void addUser(User other)
        {
            using (db = new TorrentDBContext())
            {
                db.Users.InsertOnSubmit(other);
                db.SubmitChanges();
            }
        }

        public void removeUser(User other)
        {
            using (db = new TorrentDBContext())
            {
                foreach (var user in db.Users)
                {
                    if (user.name.Equals(other.name))
                    {
                        db.Users.DeleteOnSubmit(user);
                        db.SubmitChanges();
                    }
                }
            }
        }

        public void updateUser(User other)
        {
            using (db = new TorrentDBContext())
            {
                foreach (var user in db.Users)
                {
                    if (user.name.Equals(other.name))
                    {
                        db.Users.DeleteOnSubmit(user);
                        db.Users.InsertOnSubmit(other);
                        db.SubmitChanges();
                    }
                }
            }
        }

        public bool contain(User other)
        {
            using (db = new TorrentDBContext())
            {
                foreach (var user in db.Users)
                {
                    if (user.name.Equals(other.name))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void login(User other)
        {
            using (db = new TorrentDBContext())
            {
                foreach (var user in db.Users)
                {
                    if(user.name.Equals(other.name))
                    {
                        user.isAvailable = true;
                        db.SubmitChanges();
                    }
                }
            }
        }

        public void logout(User other)
        {
            using (db = new TorrentDBContext())
            {
                foreach (var user in db.Users)
                {
                    if(user.name.Equals(other.name))
                    {
                        user.isAvailable = false;
                        db.SubmitChanges();
                    }
                }
            }
        }

        public bool isValid(User other)
        {
            using (db = new TorrentDBContext())
            {
                foreach (var user in db.Users)
                {
                    if (user.name.Equals(other.name) && user.password.Equals(other.password))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public List<User> getUsers()
        {
            using (db = new TorrentDBContext())
            {
                return db.Users.ToList();
            }
        }

        public List<User> getActiveUsers()
        {
            List<User> list = new List<User>();
            using (db = new TorrentDBContext())
            {
                foreach (var user in db.Users)
                {
                    if (user.isAvailable == true)
                    {
                        list.Add(user);
                    }
                }
            }
            return list;
        }

        public void addFile(File other)
        {
            using (db = new TorrentDBContext())
            {
                db.Files.InsertOnSubmit(other);
                db.SubmitChanges();
            }
        }

        public void removeFile(File other , string userName)
        {
            using (db = new TorrentDBContext())
            {
                foreach (var file in db.Files)
                {
                    if (file.name.Equals(other.name) && file.allUsers.Equals(userName))
                    {
                        db.Files.DeleteOnSubmit(file);
                        db.SubmitChanges();
                    }
                }
            }
        }

        public List<User> getFileUsersList(string fileName)
        {
            using (db = new TorrentDBContext())
            {
                List<User> list = new List<User>();
                foreach (var file in db.Files)
                {
                    if (file.name.Equals(fileName))
                    {
                        foreach (var user in db.Users)
                        {
                            if (user.name.Equals(file.allUsers) && user.isAvailable == true)
                            {
                                list.Add(user);
                            }
                        }
                    }
                }
                return list;
            }
        }

        public List<File> getFiles()
        {
            using (db = new TorrentDBContext())
            {
                List<File> list = new List<File>();
                foreach (var file in db.Files)
                {
                    foreach (var user in db.Users)
                    {
                        if (user.isAvailable == true && !list.Contains(file))
                        {
                            list.Add(file);
                        }
                    }
                }
                return db.Files.ToList();
            }
        }

        public bool contain(string name)
        {
            using (db = new TorrentDBContext())
            {
                foreach (var file in db.Files)
                {
                    if (file.name.Equals(name))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool contain(User otherUser , File otherFile)
        {
            // Check if the user already got the file under his name.
            using (db = new TorrentDBContext())
            {
                foreach (var file in db.Files)
                {
                    if (file.name == otherFile.name && file.allUsers.Equals(otherFile.allUsers))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static DAL getInstance()
        {
            if (theDal == null)
            {
                theDal = new DAL();
            }
            return theDal;
        }

        public void enable(string name)
        {
            using (db = new TorrentDBContext())
            {
                foreach(var user in db.Users)
                {
                    if(user.name == name)
                    {
                        user.isAvailable = true;
                    }
                }
                db.SubmitChanges();
            }
        }

        public void disable(string name)
        {
            using (db = new TorrentDBContext())
            {
                foreach (var user in db.Users)
                {
                    if (user.name == name)
                    {
                        user.isAvailable = false;
                    }
                }
                db.SubmitChanges();
            }
        }

        public void deleteAllFiles()
        {
            using (db = new TorrentDBContext())
            {
                foreach(var file in db.Files)
                {
                    db.Files.DeleteOnSubmit(file);
                }
                db.SubmitChanges();
            }
        }

    }
}
