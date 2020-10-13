using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Task_Manager_Client
{
    public partial class Client_Form : Form, IView
    {
        public Dictionary<int, string> Tasks { get; set; }

        public event EventHandler<EventArgs> Recover;
        public event EventHandler<EventArgs> Shutdown;//Task
        public event EventHandler<EventArgs> Create;
        public event EventHandler<EventArgs> Sign;

        public Client_Form()
        {
            InitializeComponent();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            Recover?.Invoke(this, EventArgs.Empty);
        }

        private void Kill_Click(object sender, EventArgs e)
        {
            Shutdown?.Invoke(this, EventArgs.Empty);
        }

        private void Create_Click(object sender, EventArgs e)
        {
            Create?.Invoke(this, EventArgs.Empty);
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            Sign?.Invoke(this, EventArgs.Empty);
        }

        public string GetTaskName()
        {
            return textBox1.Text;
        }

        public string GetIP()
        {
            return textBox2.Text;
        }

        public int GetID()
        {
            int i = 0;
            foreach (var item in Tasks.Keys)
                if (i++ == listBox1.SelectedIndex)
                {
                    i = item;
                    break;
                }
            //MessageBox.Show(i.ToString());
            return i;
        }

        public void InputInfo()
        {
            listBox1.Items.Clear();
            foreach (var item in Tasks)
                listBox1.Items.Add(item.Value);
        }

        private void Connect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (new Regex("[^\\d|\\.|\\\b]").IsMatch(e.KeyChar.ToString()))// \b == Backspace == KeyChar(8)
                e.Handled = true;
        }
    }
}