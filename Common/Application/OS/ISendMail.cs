using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SLD.Tezos.Client.OS
{
    public interface ISendMail
    {
		Task<bool> Send(string recipients, string subject, string body, string attachmentName, byte[] attachmentData);
    }
}
