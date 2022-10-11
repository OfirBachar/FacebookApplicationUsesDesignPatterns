using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class PhotosCollection : IEnumerable<PhotoProxy>
    {
        private List<PhotoProxy> m_PhotosProxyCollection;

        public PhotosCollection()
        {
            m_PhotosProxyCollection = new List<PhotoProxy>();
        }

        public IEnumerator<PhotoProxy> GetEnumerator()
        {
            foreach(PhotoProxy photo in m_PhotosProxyCollection)
            {
                yield return photo;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(PhotoProxy i_PhotoToAdd)
        {
            m_PhotosProxyCollection.Add(i_PhotoToAdd);
        }

        public void Remove(PhotoProxy i_PhotoToRemove)
        {
            m_PhotosProxyCollection.Remove(i_PhotoToRemove);
        }
    }
}
