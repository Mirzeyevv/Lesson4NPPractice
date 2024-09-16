using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XamlOfClientSide
{

    public partial class MainWindow : Window
    {

        TcpClient client = new TcpClient();
        List<string> processes = new List<string>();
        IPEndPoint endpoint;
        public MainWindow()
        {

            int port = 27001;
            IPAddress ip = IPAddress.Parse("192.168.0.171");
            endpoint = new IPEndPoint(ip, port);

            //client.Connect(endpoint);


            InitializeComponent();


            List<string> list = new List<string>();
            list.Add("Run");
            list.Add("Kill");

            ComboBoxOfCommandType.ItemsSource = list;

            ListBox.ItemsSource = processes;

        }



        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {


                if (client.Connected)
                {

                    var command = new CommandMyOwnType("Refresh", "GimmeMyProcesses");

                    string commandJson = JsonSerializer.Serialize(command);
                    byte[] data = Encoding.UTF8.GetBytes(commandJson);
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);


                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string processesJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);


                    List<string>? processList = JsonSerializer.Deserialize<List<string>>(processesJson);

                    ListBox.ItemsSource = null;
                    ListBox.ItemsSource = processList;



                }


            }
            catch { }


        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                client.Connect(endpoint);

                if (client.Connected)
                {

                    var command = new CommandMyOwnType(ComboBoxOfCommandType.SelectedItem.ToString()!, TextBox.Text);

                    string commandJson = JsonSerializer.Serialize(command);
                    byte[] data = Encoding.UTF8.GetBytes(commandJson);

                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);


                }

                

            }
            catch { }

        }

    }


}
