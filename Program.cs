using System.Net;
using System.Net.Sockets;
using System.Text;

class WizBulbDiscovery
{
    // Function to send a basic discovery message and receive responses
    public static async Task DiscoverWizBulbsAsync(int timeout = 5000)
    {
        // Basic discovery message with just the method field
        string discoveryMessage = "{\"method\":\"getSystemConfig\"}";
        byte[] messageBytes = Encoding.UTF8.GetBytes(discoveryMessage);

        // Create a UDP client
        UdpClient udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;

        // WiZ bulbs listen on port 38899
        IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, 38899);

        try
        {
            // Send the broadcast message
            await udpClient.SendAsync(messageBytes, messageBytes.Length, broadcastEndPoint);
            Console.WriteLine("Broadcast message sent to discover WiZ bulbs...");

            // Set receive timeout
            udpClient.Client.ReceiveTimeout = timeout;

            // Listen for responses
            while (true)
            {
                try
                {
                    // Receive response from any device
                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    string receivedData = Encoding.UTF8.GetString(result.Buffer);

                    Console.WriteLine("Received response from {0}:", result.RemoteEndPoint);
                    Console.WriteLine(receivedData);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.TimedOut)
                    {
                        Console.WriteLine("Discovery process timed out.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error: {0}", ex.Message);
                        break;
                    }
                }
            }
        }
        finally
        {
            udpClient.Close();
        }
    }

    static async Task Main(string[] args)
    {
        try
        {
            await DiscoverWizBulbsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
