using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Client
{

    public partial class Page2 : Page
    {
        public static int PORT_IN = 8005;
        public static int PORT_OUT = 8006;
        private Mediation.MediationSoapClient server;
        public Config config { set; get; }
        private List<DAL.File> allFiles = new List<DAL.File>();
        private List<DAL.User> fileUsers = new List<DAL.User>();
        private int index = -1;
        private long fileSize;
        private string userData;
        private string filesData;
        private SocketsManager manager;
        private MainWindow main;
        private List<DAL.File> files;

        public Page2(Config config, string userData, string filesData, MainWindow main)
        {
            InitializeComponent();
            //this.Search_DataGrid = new DataGrid();
            Search_DataGrid.AutoGenerateColumns = false;
            Search_DataGrid.IsReadOnly = true;
            Search_DataGrid.CanUserAddRows = false;
            Search_DataGrid.CanUserDeleteRows = false;
            Search_DataGrid.CanUserReorderColumns = false;
            Search_DataGrid.CanUserResizeColumns = false;
            Search_DataGrid.CanUserResizeRows = false;
            Search_DataGrid.CanUserSortColumns = false;
            this.config = config;
            this.userData = userData;
            this.filesData = filesData;
            this.server = new Mediation.MediationSoapClient();
            this.manager = new SocketsManager(config.path);
            this.main = main;
            Task.Factory.StartNew(() => manager.HandleIncomingFile(config.ip, PORT_IN));
            Task.Factory.StartNew(() => manager.HandleIncomingRequest(config.ip, PORT_OUT));
            List<DAL.File> files = JsonConvert.DeserializeObject<List<DAL.File>>(server.getFiles());
            files.RemoveAll(file => file.allUsers == config.name);
            Files_DataGrid.ItemsSource = files;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string fileName = Search_TextBox.Text.ToString();
            string listData;
            this.index = -1;
            this.fileSize = 0;
            try
            {
                if (fileName != null || fileName != "")
                {
                    files = JsonConvert.DeserializeObject<List<DAL.File>>(server.getFiles());
                    listData = server.fileRequest(userData, fileName);
                    fileUsers = JsonConvert.DeserializeObject<List<DAL.User>>(listData);
                    if (fileUsers.Count == 0)
                    {
                        MessageBox.Show("No such file.");
                    }
                    else
                    {
                        DAL.User temp = new DAL.User();
                        foreach (var u in fileUsers)
                        {
                            if (config.name == u.name)
                            {
                                temp = u;
                            }
                        }
                        fileUsers.Remove(temp);
                        Search_DataGrid.ItemsSource = fileUsers;
                        foreach (var file in this.files)
                        {
                            if (file != null)
                            {
                                if (file.name == fileName)
                                {
                                    fileSize = file.size;
                                }
                            }
                        }
                        this.calculateSize(fileSize);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("File not found.");
            }
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            if (index != -1)
            {
                manager.avg = 0;
                manager.time = 0;
                Time_Lable.Content = 0;
                Speed_Lable.Content = 0;
                ProgressBar.Value = 0;
                DAL.User user = fileUsers.ElementAt<DAL.User>(index);
                string filename = Search_TextBox.Text;
                Task.Factory.StartNew(() => manager.SendRequest(user.ip, PORT_OUT, filename));
                manager.DataReceived += new SocketsManager.FileRecievedEventHandler(downloadFinised);
            }
        }

        private void calculateSize(long size)
        {
            if (size > 1e+9)
            {
                FileSize_Lable.Content = size / 1e+9 + " GB";
            }
            else if (fileSize > 1000000)
            {
                FileSize_Lable.Content = size / 1000000 + " MB";
            }
            else if (fileSize > 1000)
            {
                FileSize_Lable.Content = size / 1000 + " KB";
            }
            else
            {
                FileSize_Lable.Content = size + " B";
            }
        }

        private void itemSelected(object sender, SelectedCellsChangedEventArgs e)
        {
            index = Search_DataGrid.Items.IndexOf(Search_DataGrid.CurrentItem);
        }

        private void Reflection_Click(object sender, RoutedEventArgs e)
        {
            List<Type> allTypes = getTypes();
            try
            {
                if (allTypes != null)
                {
                    foreach (var t in allTypes)
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@t.FullName + ".txt"))
                        {
                            file.WriteLine("Class Name: " + t.FullName);
                            file.WriteLine("    Class Methods and Attributes:");
                            foreach (var item in t.GetMembers())
                            {
                                MethodInfo method = t.GetMethod(item.Name.ToString());
                                if (method != null)
                                {
                                    file.WriteLine("        Method: " + method.Name.ToString());
                                    foreach (var parameter in method.GetParameters())
                                    {
                                        file.WriteLine("        Parameters: " + parameter.ToString());
                                    }
                                }
                                else
                                {
                                    file.WriteLine("        Attributes: " + item.Name.ToString() + " , " + item.MemberType.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception exc)
            {
                exc.ToString();
                MessageBox.Show("Error while reading the dll. Maybe overloading methods.");
            }
        }

        private List<Type> getTypes()
        {
           List<Type> types = new List<Type>();
            try
            {
                Assembly assembly = Assembly.LoadFrom(Search_textBox.Text);
                foreach (Type type in assembly.GetTypes())
                {
                    types.Add(type);
                }
                return types;
            }
            catch (Exception ex)
            {
                MessageBox.Show("File is not DLL, enter a DLL path.");
                ex.ToString();
            }
            return null;
        }

        private void downloadFinised(object source, double downloadFinished)
        {
            server.logout(userData, filesData);
            this.filesData = JsonConvert.SerializeObject(this.getFiles(config.path));
            server.login(userData, filesData);
            Dispatcher.BeginInvoke(new ThreadStart(() => progress(downloadFinished)));
        }

        private List<DAL.File> getFiles(string path)
        {
            var files = new DirectoryInfo(path).GetFiles();
            List<DAL.File> list = new List<DAL.File>();
            foreach (var file in files)
            {
                DAL.File newFile = new DAL.File();
                newFile.name = file.Name;
                newFile.size = file.Length;
                newFile.id = 0;
                newFile.allUsers = config.name;
                list.Add(newFile);
            }
            return list;
        }

        private void progress(double downloadFinished)
        {
            if (downloadFinished == -1)
            {
                ProgressBar.Value = 100;
                double t = ConvertMillisecondsToMinutes((double)manager.time);
                Time_Lable.Content = t.ToString();
                Speed_Lable.Content = manager.avg.ToString();
            }
            else
            {
                ProgressBar.Value = (double)(downloadFinished / fileSize) * 100;
            }
        }

        public static double ConvertMillisecondsToMinutes(double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).TotalMinutes;
        }

    }
}
