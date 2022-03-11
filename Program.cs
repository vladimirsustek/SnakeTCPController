using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// Synchronous socket client = program waits until server responds
public class SnakeTCPController
{

    public static void InitiateClient(string key)
    {
        if (key == "P") key = "PP";
        // Data buffer for incoming data.  
        byte[] bytes = new byte[1024];
        const string clientIP = "192.168.100.2";
        const string ServerIP = "192.168.100.1";
        const int serverPort = 8000;

        // Connect to a remote device.  
        try
        {
            System.Net.IPAddress server = System.Net.IPAddress.Parse(ServerIP);
            System.Net.IPAddress client = System.Net.IPAddress.Parse(clientIP);
            IPEndPoint remoteEP = new IPEndPoint(server, serverPort);

            // Create a TCP/IP  socket.  
            Socket sender = new Socket(client.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                sender.Connect(remoteEP);
                
                byte[] msg = Encoding.ASCII.GetBytes(key);

                // Send the data through the socket.  
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device.  
                int bytesRec = sender.Receive(bytes);
                //Console.WriteLine("Send = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                // Release the socket.  
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void ReadKey()
    {
        ConsoleKey key;
        do
        {
            while (!Console.KeyAvailable) ;

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.W || key == ConsoleKey.A || 
                key == ConsoleKey.S || key == ConsoleKey.D || 
                key == ConsoleKey.P || key == ConsoleKey.Q || 
                key == ConsoleKey.R)
            {
                InitiateClient(key.ToString());
            }

        } while (key != ConsoleKey.Escape);

    }
    public static int Main(String[] args)
    {
        Console.WriteLine("Program automatically opens port, may send W A S D P to console, ESC closes port and quits");

        ReadKey();

        Console.WriteLine("Program Closed");
        Thread.Sleep(1000);

        return 0;
    }
}