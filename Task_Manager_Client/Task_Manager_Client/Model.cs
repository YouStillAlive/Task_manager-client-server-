using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Task_Manager_Client
{
    class Model : IModel
    {
        public Socket Socket { get; set; }
        public IPAddress IP { get; set; }
        public Dictionary<int, string> Tasks { get; set; }

        public Model()
        {
            Tasks = new Dictionary<int, string>();
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }

        public async Task Take_Tasks()
        {
            await Task.Run(() =>
            {
                if (Socket.Connected)
                {
                    try
                    {
                        Send("0");                                          // 0 = Update
                                                                            // 1 = Kill
                                                                            // 2 = Create
                        byte[] bytes = new byte[10000];
                        int bytesRec = Socket.Receive(bytes);
                        Deserialize(bytes);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });
        }

        private void Deserialize(byte[] bytes)
        {
            using (var Stream = new MemoryStream(bytes))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Tasks = (Dictionary<int, string>)formatter.Deserialize(Stream);
            }
        }

        public async Task Kill_Task(int id)
        {
            await Task.Run(() =>
            {
                if (Socket.Connected)
                {
                    try
                    {
                        Send("1");                               // 0 = Update
                                                                 // 1 = Kill
                                                                 // 2 = Create
                        Send(id.ToString());
                        Check();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });
        }

        public async Task Add_Task(string ProcessName)
        {
            await Task.Run(() =>
            {
                if (Socket.Connected)
                {
                    try
                    {
                        Send("2");                                          // 0 = Update
                                                                            // 1 = Kill
                                                                            // 2 = Create
                        Send(ProcessName);
                        Check();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });
        }

        private void Check()
        {
            byte[] b = new byte[1];
            Socket.Receive(b);
            if (b[0] == 0)
                MessageBox.Show("Error!");
            else
                MessageBox.Show("Succes!");
        }

        //Socket.BeginConnect(EndP, null, null);
        public async void Connect_to_Server(string IPString)
        {
            await Task.Run(() =>
            {
                try
                {
                    IP = IPAddress.Parse(IPString);
                    IPEndPoint EndP = new IPEndPoint(IP, 5555);
                    Socket.Connect(EndP);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

        }

        private void Send(string Data)
        {
            byte[] command = Encoding.Default.GetBytes(Data);
            Socket.Send(command);
        }

        public void Close()
        {
            Socket.Close();
        }
    }
}
