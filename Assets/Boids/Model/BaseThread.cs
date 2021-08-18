using System.Threading;
using System.Collections;

namespace Boids.Model
{
    public abstract class BaseThread
    {
        private object m_Handle = new object();
        private bool m_IsDone = false;
        private bool m_IsLaunched = false;
        private Thread m_Thread = null;

        public bool IsDone
        {
            get
            {
                bool tmp;
                lock (m_Handle)
                    tmp = m_IsDone;
                return tmp;
            }

            private set
            {
                lock (m_Handle)
                    m_IsDone = value;
            }
        }

        public bool IsLaunched
        {
            get
            {
                bool tmp;
                lock (m_Handle)
                    tmp = m_IsLaunched;
                return tmp;
            }
            
            private set
            {
                lock (m_Handle)
                    m_IsLaunched = value;
            }
        }

        public virtual void Start()
        {
            if (!IsLaunched)
            {
                IsLaunched = true;
                m_Thread = new Thread(Run);
                m_Thread.Start();
            }
        }

        public virtual void Abort()
        {
            m_Thread.Abort();
            IsDone = true;
            IsLaunched = false;
        }

        //  DON'T use the Unity methods here
        protected virtual void ThreadWork() { }

        //  This method can be executed by the Unity main thread when the thread's work is done
        protected virtual void OnFinished() { }

        //  This method is for Unity coroutine. Just use: yield return StartCoroutine(thread.WaitingFor());
        public IEnumerator WaitingFor()
        {
            while (!Update())
                yield return null;
        }

        //  This method can be executed by Unity's Update or FixedUpdate
        public virtual bool Update()
        {
            if (IsDone)
            {
                OnFinished();
                return true;
            }
            return false;
        }

        private void Run()
        {
            ThreadWork();
            IsDone = true;
            IsLaunched = false;
        }
    }
}