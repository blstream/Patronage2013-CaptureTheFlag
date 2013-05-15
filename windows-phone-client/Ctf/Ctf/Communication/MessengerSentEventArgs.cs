﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctf
{
    public class MessengerSentEventArgs : EventArgs
    {
        public readonly string message;
        public readonly int errorCode;

        //JIRA 3 [RC] podmienic eerror code dla apk
        public MessengerSentEventArgs(string message, int errorCode = ErrorCode.APPLICATION_ERROR)
        {
            this.message = message;
            this.errorCode = errorCode;
        }
    }
}
