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
        private CommunicationProtocolUtils() { }

        // TODO: document this
        public static byte[] MakeCommand(int ? uniqueID, CommunicationProtocol command, String dataToSend)
        {
            StringBuilder commandAsString = new StringBuilder(uniqueID != null ? uniqueID.ToString() : "-1");

            commandAsString.AppendFormat("/{0}", command.ToString());
            commandAsString.AppendFormat("/{0}", dataToSend);

            return Encoding.ASCII.GetBytes(commandAsString.ToString());
        }

        public static int GetIDFromCommand(byte[] commandData)
        {
            return Int32.Parse(Encoding.ASCII.GetString(commandData).Split('/')[0]);
        }

        public static CommunicationProtocol GetProtocolFromCommand(byte[] commandData)
        {
            return (CommunicationProtocol) Enum.Parse(typeof(CommunicationProtocol), Encoding.ASCII.GetString(commandData).Split('/')[1]);
        }

        public static String GetDataFromCommand(byte[] commandData)
        {
            return Encoding.ASCII.GetString(commandData).Split('/')[2];
        }
    }
}
