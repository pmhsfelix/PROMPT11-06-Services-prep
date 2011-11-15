using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xmpp
{
    class CompletedSynchronouslyResult : IAsyncResult
    {
        #region IAsyncResult Members
        object state;

        public CompletedSynchronouslyResult(object state)
        {
            this.state = state;
        }

        public object AsyncState
        {
            get { return state; }
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public bool CompletedSynchronously
        {
            get { return true; }
        }

        public bool IsCompleted
        {
            get { return true; }
        }

        #endregion
    }
}
