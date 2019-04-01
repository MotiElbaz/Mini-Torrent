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
using DAL;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.ComponentModel;

namespace Client
{
    public partial class MainWindow : Window
    {
        public const string FILE_NAME = "Config2019.json";
        public const int PORT_IN = 8005;
        public const int PORT_OUT = 8006;
        private Mediation.MediationSoapClient server;
        private Config config;
        private string IPAddress;
        private string userData;
        private string filesData;
        private string path;

        public MainWindow()
        {
            IPAddress = "";
            this.loadConfig();
            server = new Mediation.MediationSoapClient();
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            if (config == null)
            {
                MessageBox.Show("Config is invalid, Enter new values.");
            }
            if (config == null || checkBox.IsChecked == false)
            {
                config = new Config();
                config.name = textBox.Text;
                config.password = passwordBox.Password;
                config.port = PORT_IN;
                config.ip = getIPAddress();
                if (this.path == null || this.path == "")
                {
                    MessageBox.Show("You must enter a path.");
                    return;
                }
                config.path = this.path;
            }
            user.name = config.name;
            user.password = config.password;
            user.ip = config.ip;
            user.port = config.port;
            user.isAvailable = true;
            this.userData = JsonConvert.SerializeObject(user);
            this.filesData = JsonConvert.SerializeObject(this.getFiles(config.path));
            string answer = server.login(userData, filesData);
            if (answer != "User login.")
            {
                MessageBox.Show(answer);
            }
            else
            {
                using (StreamWriter file = System.IO.File.CreateText(@FILE_NAME))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, config);
                }
                Page2 page2 = new Page2(config, userData, filesData, this);
                this.Content = page2;
                //MessageBox.Show(answer);
            }
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

        private void Path_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult unused = dialog.ShowDialog();
                this.path = dialog.SelectedPath;
                if (config != null)
                {
                    config.path = this.path;
                }
            }
        }

        private void loadConfig()
        {
            config = null;
            try
            {
                StreamReader reader = new StreamReader(@FILE_NAME);
                string json = reader.ReadToEnd();
                this.config = JsonConvert.DeserializeObject<Config>(json);
                reader.Close();
            }
            catch (Exception e) {
                e.ToString();
            }
        }

        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            this.filesData = JsonConvert.SerializeObject(this.getFiles(config.path));
            server.logout(userData, filesData);
            this.OnClosed(e);
        }

        private string getIPAddress()
        {
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }

    }
}
