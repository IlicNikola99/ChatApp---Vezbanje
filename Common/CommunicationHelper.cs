using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common
{
    public class CommunicationHelper
    {
        private NetworkStream stream;
        private BinaryFormatter formatter;
        private Socket socket;


        public CommunicationHelper(Socket socket)
        {
            this.socket = socket;
            this.stream = new NetworkStream(socket);
            this.formatter = new BinaryFormatter();
        }

        public void Send<T>(T obj) where T : class
        {
            formatter.Serialize(stream, obj);
            Debug.WriteLine(">>>>>>Poslato");

        }

        public T Recieve<T>() where T : class
        {


                Debug.WriteLine(">>>>>>Primljeno");
                return (T)formatter.Deserialize(stream);

            
          


        }
    }
}