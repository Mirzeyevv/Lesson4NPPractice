
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lesson4_NP_Practice {
    internal class Server
    {
        
        private static void Main(string[] args)
        {
            

            var port = 27001;
            var ip = IPAddress.Parse("192.168.0.171");
            var ep = new IPEndPoint(ip, port);


            var listener = new TcpListener(ep);

            listener.Start();
            Console.WriteLine($"Waiting at: {listener.LocalEndpoint}");

            while (true)
            {
                
                var client = listener.AcceptTcpClient();
                var processes = new List<string>();

               

                var task = Task.Run(() =>
                {

                    var DidProcessWork = true;
                    Console.WriteLine($"Connected : {client.Client.RemoteEndPoint}");

                    var stream = client.GetStream();

                    byte[] buffer = new byte[256];
                    int sr = stream.Read(buffer, 0, buffer.Length);
                    string commandJson = Encoding.UTF8.GetString(buffer, 0, sr);


                    CommandMyOwnType command = JsonSerializer.Deserialize<CommandMyOwnType>(commandJson)!;
                    Console.WriteLine($"Received command: {command.CommandType} {command.ProcessName}");


                    var namecheck = true;

                    if (command.CommandType == "Run")
                    {

                        

                        try {

                            foreach (string process in processes)
                            {

                                if (process == command.ProcessName) { namecheck = false; DidProcessWork = false; }

                            }

                            if (namecheck) { Process.Start(command.ProcessName); processes.Add(command.ProcessName); }

                        }

                        catch (Exception ex) { Console.WriteLine(ex.Message); DidProcessWork = false; }
                        
                    }

                    if (command.CommandType == "Kill")
                    {

                        try
                        {

                            foreach (string process in processes)
                            {

                                if(process== command.ProcessName) {

                                    Process[] runningProcesses = Process.GetProcessesByName(process);
                                    foreach (Process ps in runningProcesses)
                                    {
                                        ps.Kill();
                                        ps.WaitForExit();
                                        ps.Dispose();
                                    }

                                }

                            }

                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); DidProcessWork = false; }

                    }

                    if(command.CommandType == "Refresh")
                    {
                        try {
                            Console.WriteLine("Client requested process list");

                            string processesJson = JsonSerializer.Serialize(processes);

                            byte[] processesSendingToClient = Encoding.UTF8.GetBytes(processesJson);
                            stream.Write(processesSendingToClient, 0, processesSendingToClient.Length);
                        }
                        catch { DidProcessWork = false; }
                        

                        
                    }

                    

                    byte[] response = Encoding.UTF8.GetBytes(DidProcessWork.ToString());
                    stream.Write(response, 0, response.Length);
                    

                });



            };
        }
    }
}
