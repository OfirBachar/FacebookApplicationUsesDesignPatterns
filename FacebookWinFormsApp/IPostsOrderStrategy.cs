using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    public interface IPostsOrderStrategy
    {
        IEnumerable<Post> Order(IEnumerable<Post> i_List);
    }
}
