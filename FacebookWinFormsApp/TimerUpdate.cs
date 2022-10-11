using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BasicFacebookFeatures
{
    public class TimerUpdate
    {
        private Timer m_Timer;

        public event Action DoWhenTimePassed;

        public TimerUpdate(int i_UpdateTime)
        {
            m_Timer = new Timer();
            m_Timer.Interval = i_UpdateTime;
            m_Timer.Elapsed += OnTimePassed;
            m_Timer.Enabled = true;
        }

        public void OnTimePassed(object i_Source, ElapsedEventArgs e)
        {
            DoWhenTimePassed?.Invoke();
        }
    }
}

