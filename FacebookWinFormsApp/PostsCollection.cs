using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    public class PostsCollection : IEnumerable<Post>
    {
        private List<Post> m_PostsCollection;

        public PostsCollection()
        {
            m_PostsCollection = new List<Post>();
        }

        public IEnumerator<Post> GetEnumerator()
        {
            foreach (Post post in m_PostsCollection)
            {
                yield return post;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Post i_PostToAdd)
        {
            m_PostsCollection.Add(i_PostToAdd);
        }

        public void Remove(Post i_PostToRemove)
        {
            m_PostsCollection.Remove(i_PostToRemove);
        }
    }
}

