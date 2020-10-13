using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_Manager_Client
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Presenter presenter;
            IModel model = new Model();
            var view = new Client_Form();
            
            Application.EnableVisualStyles();
            presenter = new Presenter(view, model);
            view.ShowDialog();

            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Client_Form());
        }
    }
}
