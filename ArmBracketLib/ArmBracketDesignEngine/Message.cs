using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArmBracketDesignLibrary.Helpers;
// 

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class Message
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Constructors

        public Message()
        {
        }

        public Message(MessageCategory cat, string text)
        {
            Category = cat;
            MessageText = text;
        }

        public Message(MessageCategory cat, string messageText, Exception ex)
        {
            Category = cat;
            MessageText = messageText;
            ExceptionMessage = ex.Message;
            StackTrace = ex.StackTrace;
        }


        #endregion  // Constructors

        #region Properties

        public MessageCategory Category { get; set; }

        public string MessageText { get; set; }

        public string ExceptionMessage { get; set; }

        public string StackTrace { get; set; }

        #endregion  // Properties

       
    }
}
