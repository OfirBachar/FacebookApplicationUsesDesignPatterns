using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    public enum eOrderStrategyType
    {
        DATE = 0,
        LEXICOGRAPHIC = 1
    }

    public class PostsOrder
    {
        private IPostsOrderStrategy m_PostsOrderStrategy;

        public void SetOrderStrategy(eOrderStrategyType i_OrderStrategyType)
        {
            switch (i_OrderStrategyType)
            {
                case eOrderStrategyType.DATE:
                    m_PostsOrderStrategy = new DateItemOrderStrategy();
                    break;
                case eOrderStrategyType.LEXICOGRAPHIC:
                    m_PostsOrderStrategy = new LexicographicItemOrderStrategy();
                    break;
            }
        }

        public IEnumerable<Post> Order(IEnumerable<Post> i_Items)
        {
            return m_PostsOrderStrategy.Order(i_Items);
        }
    }
}
