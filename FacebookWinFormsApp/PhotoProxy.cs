using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    public class PhotoProxy
    {
        public Photo m_Photo { get; set; }
        public string m_PhotoDescription { get; set; }

        public string CreatedTimeAndAge()
        {
            return string.Format(@"Created Time: {0}
This photo published {1} days ago", m_Photo.CreatedTime, PhotoAge());
        }

        private string PhotoAge()
        {
           return ((DateTime.Now.Subtract((DateTime)m_Photo.CreatedTime)).Days).ToString();
        }

        public override string ToString()
        {
            return string.Format(m_Photo.From.Name);
        }

        public string SpeakString()
        {
            if(m_PhotoDescription != null)
            {
                return string.Format(@"description of the photo: {0}, created at: {1}", m_PhotoDescription, m_Photo.CreatedTime);
            }

            return string.Format(@"created at: {0}", m_Photo.CreatedTime);
        }
    }
}
