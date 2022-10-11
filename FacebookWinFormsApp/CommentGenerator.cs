using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using System.Speech.Synthesis;

namespace BasicFacebookFeatures
{   
    public class CommentGenerator
    {
        public string AddedToFavorites { get; }

        public string PostIsAlreadyInFavorites { get; }

        public string PhotoIsAlreadyInFavorites { get; }

        public string PleaseSelectOnePhotoToRemove { get; }

        public string PleaseSelectOnePostToRemove { get; }

        public string PleaseSelectOnePhotoToAdd { get; }

        public string PleaseSelectOnePostToAdd { get; }

        public string NoPostsToRetrieve { get; }

        public string NoPhotosToRetrieve { get; }

        public string NoLikedPageToRetrieve { get; }

        public string LoginFailed { get; }

        public string StatusPosted { get; }

        public string PostLiked { get; }

        public string CommentPosted { get; }

        public string PleaseSelectOneItemToLike { get; }

        public CommentGenerator()
        {
            AddedToFavorites = "Added to favorites!";
            PostIsAlreadyInFavorites = "Post is already in favorites!";
            PhotoIsAlreadyInFavorites = "Photo is already in favorites!";
            PleaseSelectOnePhotoToRemove = "Please select one photo to remove";
            PleaseSelectOnePostToRemove = "Please select one post to remove";
            PleaseSelectOnePhotoToAdd = "Please select one photo to add";
            PleaseSelectOnePostToAdd = "Please select one post to add";
            NoPostsToRetrieve = "No Posts to retrieve :(";
            NoPostsToRetrieve = "No Photos to retrieve :(";
            NoLikedPageToRetrieve = "No liked pages to retrieve :(";
            LoginFailed = "Login Failed";
            StatusPosted = "Status Posted!";
            PostLiked = "Post Liked!";
            CommentPosted = "Comment Posted!";
            PleaseSelectOneItemToLike = "Please select one item to like";
        }
    }
}