using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Task_Manager_Client
{
    public interface IModel
    {
        Socket Socket { get; set; }
        IPAddress IP { get; set; }
        Dictionary<int, string> Tasks { get; set; }

        Task Take_Tasks();
        Task Kill_Task(int id);
        Task Add_Task(string ProcessName);
        void Connect_to_Server(string IPString);
    }
}