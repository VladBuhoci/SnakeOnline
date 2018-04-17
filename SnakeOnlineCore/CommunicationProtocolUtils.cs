using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnlineCore
{
    public sealed class CommunicationProtocolUtils
    {
        private static BinaryFormatter binaryFormatter = new BinaryFormatter();

        /// <summary>
        ///     Private constructor, so the class cannot be instantiated..
        /// </summary>
        private CommunicationProtocolUtils() { }

        /// <summary>
        ///     Wraps up the data in an array of bytes, ready to be sent over the network.
        /// </summary>
        /// 
        /// <param name="uniqueID">Unique ID of the user's socket, or -1 if it's the server.</param>
        /// <param name="command">Any of the enumerated values.</param>
        /// <param name="dataToSend">Actual data that needs to be sent to other sockets. This can be anything.</param>
        /// 
        /// <returns>
        ///     Returns an array of bytes which can be sent to other sockets, where it can be deserialized
        ///         and thus, have its data accessed.
        /// </returns>
        public static byte[] MakeNetworkCommand(int uniqueID, CommunicationProtocol command, Object dataToSend)
        {
            uniqueID = uniqueID >= 0 ? uniqueID : -1;
            CommunicationPacketWrapper wrapper = new CommunicationPacketWrapper(uniqueID, command, dataToSend);
            MemoryStream stream = new MemoryStream();

            binaryFormatter.Serialize(stream, wrapper);

            return stream.ToArray();
        }

        public static int GetIDFromCommand(byte[] commandData)
        {
            MemoryStream stream = new MemoryStream(commandData);
            CommunicationPacketWrapper wrapper = (CommunicationPacketWrapper) binaryFormatter.Deserialize(stream);

            return wrapper.uniqueID;
        }

        public static CommunicationProtocol GetProtocolValueFromCommand(byte[] commandData)
        {
            MemoryStream stream = new MemoryStream(commandData);
            CommunicationPacketWrapper wrapper = (CommunicationPacketWrapper) binaryFormatter.Deserialize(stream);

            return wrapper.protocolCommand;
        }

        public static Object GetDataFromCommand(byte[] commandData)
        {
            MemoryStream stream = new MemoryStream(commandData);
            CommunicationPacketWrapper wrapper = (CommunicationPacketWrapper) binaryFormatter.Deserialize(stream);

            return wrapper.data;
        }
    }
}