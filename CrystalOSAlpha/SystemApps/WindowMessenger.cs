using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.SystemApps
{
    class WindowMessenger
    {
        public static List<WindowMessage> Message = new List<WindowMessage>();
        public static void Send(WindowMessage message)
        {
            Message.Add(message);
        }
        public static WindowMessage Recieve(string From, string To)
        {
            foreach(var v in Message)
            {
                if(v.From == From && v.To == To)
                {
                    return v;
                }
            }
            return null;
        }
    }
    class WindowMessage
    {
        public string Message { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public WindowMessage(string message, string from, string to)
        {
            Message = message;
            From = from;
            To = to;
        }
    }
}
