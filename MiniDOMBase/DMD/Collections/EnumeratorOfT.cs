using System;
using System.Collections;
using System.Collections.Generic;

namespace minidom
{
    public class Enumerator<T> : IEnumerator<T>
    {
        private IEnumerator m_Base;

        public Enumerator(IEnumerator @base)
        {
            DMDObject.IncreaseCounter(this);
            m_Base = @base;
        }

        public T Current
        {
            get
            {
                return (T)m_Base.Current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return m_Base.Current;
            }
        }

        public bool MoveNext()
        {
            return m_Base.MoveNext();
        }

        public void Reset()
        {
            m_Base.Reset();
        }

        public void Dispose()
        {
            if (m_Base is IDisposable)
            {
                ((IDisposable)m_Base).Dispose();
            }
            else
            {
                m_Base.Reset();
            }

            m_Base = null;
        }

        ~Enumerator()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}