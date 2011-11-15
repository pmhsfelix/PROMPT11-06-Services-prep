using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace Xmpp
{
    class MyQueue<T>
        where T: class
    {

        public class Reader : IAsyncResult
        {
            #region IAsyncResult Members

            object state;
            EventWaitHandle handle;
            bool completed;
            bool cs;
            object lok = new object();
            T t;
            AsyncCallback callback;

            public Reader(object state, bool cs, AsyncCallback acb, T t)
            {
                this.state = state;
                this.cs = cs;
                this.completed = cs;
                handle = new EventWaitHandle(cs, EventResetMode.ManualReset);
                this.t = t;
                callback = acb;
            }

            public void Complete(T t)
            {
                lock (lok)
                {
                    this.t = t;
                    completed = true;
                    handle.Set();
                    if(callback != null){
                        callback(this);
                    }                  
                }
            }

            public T Item
            {
                get { return t; }
            }

            public object AsyncState
            {
                get { return state; }
            }

            public WaitHandle AsyncWaitHandle
            {
                get { return handle; }
            }

            public bool CompletedSynchronously
            {
                get { return cs; }
            }

            public bool IsCompleted
            {
                get
                {
                    bool r;
                    lock (lok)
                    {
                        r = completed;
                    }
                    return r;
                }
            }

            #endregion
        }

        Queue<T> Q;
        Queue<Reader> R;

        public MyQueue()
        {
            Q = new Queue<T>();
            R = new Queue<Reader>();
        }

        public IAsyncResult BeginDequeue(AsyncCallback callback, object state)
        {
            Reader r;
            lock (Q)
            {
                if (Q.Count == 0)
                {
                    r = new Reader(state, false, callback, null);
                    R.Enqueue(r);
                }
                else
                {
                    T t = Q.Dequeue();
                    r = new Reader(state, true, callback, t);
                    callback(r);
                }
            }
            return r;
        }

        public T EndDequeue(IAsyncResult ar)
        {
            ar.AsyncWaitHandle.WaitOne();
            Reader r = ar as Reader;
            return r.Item;
        }

        public void Enqueue(T item)
        {
            lock (Q)
            {
                if (R.Count != 0)
                {
                    Reader r = R.Dequeue();
                    r.Complete(item);
                }
                else
                {
                    Q.Enqueue(item);
                }                
            }
        }       
    }
}
