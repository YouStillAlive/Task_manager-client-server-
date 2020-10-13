using System;
using System.Threading.Tasks;

namespace Task_Manager_Client
{
    class Presenter
    {
        IModel Model;
        IView View;

        public Presenter(IView view, IModel model)
        {
            View = view;
            Model = model;
            Subscription();
        }

        private void Subscription()
        {
            View.Sign += new EventHandler<EventArgs>(Connect);
            View.Shutdown += new EventHandler<EventArgs>(Kill);
            View.Create += new EventHandler<EventArgs>(Add);
            View.Recover += new EventHandler<EventArgs>(Update);
        }

        private async void Update(object sender, EventArgs e)
        {
            await CallUpdate();
        }

        private async Task CallUpdate()
        {
            await Model.Take_Tasks();
            View.Tasks = Model.Tasks;
            View.InputInfo();
        }

        private void Connect(object sender, EventArgs e)
        {
            Model.Connect_to_Server(View.GetIP());
        }

        private async void Kill(object sender, EventArgs e)
        {
            await Model.Kill_Task(View.GetID());
            await CallUpdate();
            await CallUpdate();
        }

        private async void Add(object sender, EventArgs e)
        {
            await Model.Add_Task(View.GetTaskName());
            await CallUpdate();
            await CallUpdate();
        }
    }
}
