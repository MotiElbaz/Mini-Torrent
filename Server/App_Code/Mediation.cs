using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft;
using Newtonsoft.Json;
using DAL;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class Mediation : System.Web.Services.WebService
{
    private static int idGenerator = 100;
    private DAL.DAL theDal;

    public Mediation()
    {
        theDal = DAL.DAL.getInstance();
    }

    [WebMethod]
    public string addUser(string data)
    {
        User user = JsonConvert.DeserializeObject<User>(data);
        if (theDal.contain(user))
        {
            return "User already exists.";
        }
        theDal.addUser(user);
        return "User Added.";
    }

    [WebMethod]
    public string removeUser(string data)
    {
        User user = JsonConvert.DeserializeObject<User>(data);
        if(!theDal.contain(user))
        {
            return "User not exists.";
        }
        theDal.removeUser(user);
        return "User deleted.";
    }

    [WebMethod]
    public string updateUser(string data)
    {
        User user = JsonConvert.DeserializeObject<User>(data);
        if (!theDal.contain(user))
        {
            return "User not exists.";
        }
        theDal.updateUser(user);
        return "User updated.";
    }

    [WebMethod]
    public string login(string userData,string filesData)
    {
        User user = JsonConvert.DeserializeObject<User>(userData);
        if (!theDal.isValid(user))
        {
            return "User credentials are not valid.";
        }
        List<File> list = JsonConvert.DeserializeObject<List<File>>(filesData);
        foreach(var file in list)
        {
            file.id = idGenerator++;
            if(!theDal.contain(user,file))
            {
                theDal.addFile(file);
            }
        }
        theDal.updateUser(user); // Incase of dynamic ip or port changed.
        theDal.login(user);
        return "User login.";
    }

    [WebMethod]
    public string logout(string userData, string filesData)
    {
        User user = JsonConvert.DeserializeObject<User>(userData);
        if (!theDal.isValid(user))
        {
            return "User credentials are not valid.";
        }
        List<File> list = JsonConvert.DeserializeObject<List<File>>(filesData);
        foreach (var file in list)
        {
            if (theDal.contain(user, file))
            {
                theDal.removeFile(file , user.name);
            }
        }
        theDal.logout(user);
        return "User logout.";
    }

    [WebMethod]
    public string fileRequest(string userData,string fileData)
    {
        User user = JsonConvert.DeserializeObject<User>(userData);
        if (!theDal.isValid(user))
        {
            return "User credentials are not valid.";
        }
        if(!theDal.contain(fileData))
        {
            return "File not exists.";
        }
        return JsonConvert.SerializeObject(theDal.getFileUsersList(fileData));
    }

    [WebMethod]
    public string getFiles()
    {
        return JsonConvert.SerializeObject(theDal.getFiles());
    }

    [WebMethod]
    public string getUsers()
    {
        return JsonConvert.SerializeObject(theDal.getUsers());
    }

    [WebMethod]
    public string getActiveUsers()
    {
        return JsonConvert.SerializeObject(theDal.getActiveUsers());
    }

    [WebMethod]
    public string isAdmin(string data)
    {
        User user = JsonConvert.DeserializeObject<User>(data);
        if (!theDal.isValid(user) || (user.name != "admin" && user.password != "admin"))
        {
            return "User is not admin.";
        }
        return JsonConvert.SerializeObject(theDal.getUsers());
    }

    [WebMethod]
    public string enable(string name)
    {
        theDal.enable(name);
        return "User enabled.";
    }

    [WebMethod]
    public string disable(string name)
    {
        theDal.disable(name);
        return "User disabled.";
    }

    [WebMethod]
    public void deleteAllFiles()
    {
        theDal.deleteAllFiles();
    }

}
