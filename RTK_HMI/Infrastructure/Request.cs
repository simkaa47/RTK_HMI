using System;

namespace RTK_HMI.Infrastructure
{
    public class Request
    {
        public Action Action = delegate { };
        public Request(Action action)
        {
            Action = action;
        }
       
    }
}
