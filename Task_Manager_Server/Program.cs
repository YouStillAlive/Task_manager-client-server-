using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Task_Manager_Server
{
    class Program
    {
        static private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        static private IPEndPoint IPEnd = new IPEndPoint(IPAddress.Any, 5555);
        static private Dictionary<int, string> Tasks { set; get; }

        [Obsolete]
        private static void Main(string[] args)
        {
            Console.WriteLine("Server IP - " + Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString());
            //Get();
            Start();
            Console.ReadLine();
            socket.Close();
        }

        private static void Kill(int id)
        {
            Process.GetProcessById(id).Kill();
        }

        private static void Get()
        {
            Tasks = new Dictionary<int, string>();
            var AllProcess = from pr in Process.GetProcesses(".") orderby pr.ProcessName select pr;
            foreach (var item in AllProcess)
                Tasks.Add(item.Id, item.ProcessName);
        }

        private static async void Start()
        {
            await Task.Run(() =>
            {
                socket.Bind(IPEnd);
                socket.Listen(11);
                while (true)
                {
                    Socket handler = socket.Accept();
                    Console.WriteLine("User - " + handler.RemoteEndPoint.ToString() + " connected!");
                    Receive(handler);
                }
            });
        }
        private static async void Receive(Socket handler)
        {
            await Task.Run(() =>
            {
                try
                {
                    byte[] Buffer = new byte[128];
                    while (true)
                    {
                        int countb = TakeCommand(handler, ref Buffer);
                        string TextBuffer = Encoding.Default.GetString(Buffer, 0, countb);
                        switch (TextBuffer)                                               //0 = update
                                                                                          //1 = kill
                                                                                          //2 = create
                        {
                            case "0":
                                Get();
                                SendTask(handler);
                                break;
                            case "1":
                                countb = TakeCommand(handler, ref Buffer);
                                TextBuffer = Encoding.Default.GetString(Buffer, 0, countb);
                                Kill(Convert.ToInt32(TextBuffer));
                                Console.WriteLine("Process " + TextBuffer + " destroyed!");
                                handler.Send(new byte[] { 1 });
                                break;
                            case "2":
                                countb = TakeCommand(handler, ref Buffer);
                                TextBuffer = Encoding.Default.GetString(Buffer, 0, countb);
                                Process.Start(TextBuffer);
                                Console.WriteLine("Process " + TextBuffer + " work!");
                                handler.Send(new byte[] { 1 });
                                break;
                            default:
                                break;
                        }
                        //Receive(handler);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Сервер: " + ex.Message);
                    handler.Send(new byte[] { 0 });
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            });
        }

        static private int TakeCommand(Socket socket, ref byte[] buffer)// Like 0, 1, 2 || id/name process
        {
            buffer = new byte[128];
            return socket.Receive(buffer);
        }

        static private void SendTask(Socket handler)
        {
            byte[] sendbufer = Serialize();
            handler.Send(sendbufer);
            Console.WriteLine("Tasks were sent! To client " + handler.RemoteEndPoint.ToString());
        }

        static private byte[] Serialize()
        {
            using (var Stream = new MemoryStream())
            {
                BinaryFormatter Formatter = new BinaryFormatter();
                Formatter.Serialize(Stream, Tasks);
                return Stream.ToArray();
            }
        }
    }
}
