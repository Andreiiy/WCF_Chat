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
using System.Windows.Shapes;
using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for ChatLayaut.xaml
    /// </summary>
    public partial class ChatLayaut : Window,ChatClient.ServiceChat.IServiceChatCallback
    {
        public string name;
        public int id;

        bool Isconnected = false;
        
        ServiceChatClient client;

        public ChatLayaut(string name)
        {
            InitializeComponent();
            this.name = name;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConectUser();
        }
        void ConectUser()
        {

            if (!Isconnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
               id= client.Connect(name);
                Isconnected = true;
                btndisconnect.Content = "Disconnect";
            }
        }

        void DisconectUser()
        {
            if (Isconnected)
            {
                client.Disconnect(id);
                Isconnected = false;
                client = null;
                btndisconnect.Content = "Connect";

            }
        }
        private void btndisconnect_Click(object sender, RoutedEventArgs e)
        {
            if (!Isconnected)
                ConectUser();
            else
                DisconectUser();
        }

        private void btndisconnect_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void MsgCallback(string msg)
        {
            lbmsges.Items.Add(msg);
            lbmsges.ScrollIntoView(lbmsges.Items[lbmsges.Items.Count - 1]);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconectUser();
        }

        private void tbMsg_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (client != null)
                client.SendMsg(tbMsg.Text, id);
                tbMsg.Text = String.Empty;
            }
        }

        void GetClientsConected()
        {
          
        }
    }
}
