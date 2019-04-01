using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class SocketsManager
    {
        public long avg = 0;
        public long time = 0;
        public static int PORT_IN = 8005;
        public static int PORT_OUT = 8006;
        private string path;
        public delegate void FileRecievedEventHandler(object source, double received);
        public event FileRecievedEventHandler DataReceived;

        public SocketsManager(string path)
        {
            this.path = path;
        }

        public void HandleIncomingFile(string ip,int port)
        {
            try
            {
                IPAddress ipad = IPAddress.Parse(ip);
                TcpListener tcpListener = new TcpListener(ipad,port);
                tcpListener.Start();
                while (true)
                {
                    Socket handlerSocket = tcpListener.AcceptSocket();
                    if (handlerSocket.Connected)
                    {
                        string fileName = string.Empty;
                        NetworkStream networkStream = new NetworkStream(handlerSocket);
                        int thisRead = 0;
                        double dataReceived = 0;
                        int blockSize = 1024;
                        Byte[] dataByte = new Byte[blockSize];
                        handlerSocket.Receive(dataByte);
                        int fileNameLen = BitConverter.ToInt32(dataByte, 0);
                        fileName = Encoding.ASCII.GetString(dataByte, 4, fileNameLen);
                        Stream fileStream = File.OpenWrite(path +"\\" + fileName);
                        long size = fileStream.Length;
                        fileStream.Write(dataByte, 4 + fileNameLen, (1024 - (4 + fileNameLen)));
                        DateTime now = DateTime.Now;
                        while (true)
                        {
                            thisRead = networkStream.Read(dataByte, 0, blockSize);
                            dataReceived += thisRead;
                            fileStream.Write(dataByte, 0, thisRead);
                            DataReceived(this, dataReceived);
                            if (thisRead == 0)
                                break;
                        }
                        this.avg = size / (DateTime.Now.Millisecond - now.Millisecond);
                        this.time = DateTime.Now.Millisecond - now.Millisecond;
                        DataReceived(this, -1);
                        fileStream.Close();
                        handlerSocket = null;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void HandleIncomingRequest(string ip,int port)
        {
            try
            {
                IPAddress ipad = IPAddress.Parse(ip);
                TcpListener tcpListener = new TcpListener(ipad,port);
                tcpListener.Start();
                while (true)
                {
                    Socket handlerSocket = tcpListener.AcceptSocket();
                    if (handlerSocket.Connected)
                    {
                        string fileName = string.Empty;
                        NetworkStream networkStream = new NetworkStream(handlerSocket);
                        int blockSize = 1024;
                        Byte[] dataByte = new Byte[blockSize];
                        handlerSocket.Receive(dataByte);
                        fileName = Encoding.ASCII.GetString(dataByte);
                        SendFile(handlerSocket.RemoteEndPoint.ToString(), PORT_IN, path, fileName);
                        handlerSocket = null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void SendRequest(string remoteHostIP, int remoteHostPort, string shortFileName)
        {
            try
            {
                if (!string.IsNullOrEmpty(remoteHostIP))

                {

                    byte[] fileNameByte = Encoding.ASCII.GetBytes(shortFileName);

                    byte[] requestNameData = new byte[fileNameByte.Length];

                    byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

                    fileNameByte.CopyTo(requestNameData, 0);

                    TcpClient clientSocket = new TcpClient(remoteHostIP, remoteHostPort);

                    NetworkStream networkStream = clientSocket.GetStream();

                    networkStream.Write(requestNameData, 0, requestNameData.GetLength(0));
                    networkStream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void SendFile(string remoteHostIP, int remoteHostPort, string uploadPath, string shortFileName)
        {
            try
            {
                if (!string.IsNullOrEmpty(remoteHostIP))

                {
                    string fileName = shortFileName.Trim('\0');
                    byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);
                    string path = uploadPath + "\\" +fileName;
                    byte[] fileData = File.ReadAllBytes(path);

                    byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];

                    byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

                    fileNameLen.CopyTo(clientData, 0);

                    fileNameByte.CopyTo(clientData, 4);

                    fileData.CopyTo(clientData, 4 + fileNameByte.Length);

                    TcpClient clientSocket = new TcpClient(remoteHostIP.Split(':')[0], remoteHostPort);

                    NetworkStream networkStream = clientSocket.GetStream();

                    networkStream.Write(clientData, 0, clientData.GetLength(0));
                    networkStream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
