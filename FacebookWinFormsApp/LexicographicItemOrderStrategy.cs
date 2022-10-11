using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public class LexicographicItemOrderStrategy : IPostsOrderStrategy
    {
        public IEnumerable<Post> Order(IEnumerable<Post> i_List)
        {
            return i_List.OrderBy(item => item.Message);
        }
    }
}
