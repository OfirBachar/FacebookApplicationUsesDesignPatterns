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
    public sealed class FavoritesSingleton
    {
        private static FavoritesSingleton s_Instance = null;

        private static object s_LockObj = new Object();

        public PhotosCollection m_FavoritesPhotos;

        public PostsCollection m_FavoritesPosts;

        private FavoritesSingleton() 
        {
            m_FavoritesPhotos = new PhotosCollection();
            m_FavoritesPosts = new PostsCollection();
        }

        public static FavoritesSingleton Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (s_LockObj)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new FavoritesSingleton();
                        }
                    }
                }

                return s_Instance;
            }
        }

        public void AddPhoto(PhotoProxy i_PhotoToAdd)
        {
            m_FavoritesPhotos.Add(i_PhotoToAdd);
        }

        public void AddPost(Post i_PostToAdd)
        {
            m_FavoritesPosts.Add(i_PostToAdd);
        }

        public void RemovePhoto(PhotoProxy i_PhotoToRemove)
        {
            m_FavoritesPhotos.Remove(i_PhotoToRemove);
        }

        public void RemovePost(Post i_PostToRemove)
        {
            m_FavoritesPosts.Remove(i_PostToRemove);
        }
    }
}

