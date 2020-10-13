using System;
using System.Collections.Generic;

namespace Task_Manager_Client
{
    public interface IView
    {
        Dictionary<int, string> Tasks { get; set; }

        event EventHandler<EventArgs> Create;
        event EventHandler<EventArgs> Recover;
        event EventHandler<EventArgs> Sign;
        event EventHandler<EventArgs> Shutdown;
        //
        string GetIP();
        void InputInfo();
        int GetID();
        string GetTaskName();
    }
}