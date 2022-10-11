using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form
    {
        User m_LoggedInUser;
        FacebookWrapper.LoginResult m_LoginResult;
        CommentGenerator commentGenerator;
        private TimerUpdate m_TimerUpdate;
        private const int k_UpdateTime = 6000000;
        FavoritesSingleton m_FavoritesSingleton;
        private readonly PostsOrder r_PostOrder;

        public FormMain()
        {
            InitializeComponent();
            InitializeUpdateTimer();
            commentGenerator = new CommentGenerator();
            FacebookWrapper.FacebookService.s_CollectionLimit = 200;
            r_PostOrder = new PostsOrder();
            m_PostsSortByComboBox.SelectedIndex = 0;
        }

        private void InitializeUpdateTimer()
        {
            this.m_TimerUpdate = new TimerUpdate(k_UpdateTime);
            this.m_TimerUpdate.DoWhenTimePassed += fetchUserInfo;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            loginAndInit();
        }

        private void loginAndInit()
        {
            m_LoginResult = FacebookService.Login(
                    "1042527796654559",
                    "email",
                    "public_profile",
                    "user_likes",
                    "user_photos",
                    "user_posts",
                    "user_age_range",
                    "user_birthday",
                    "user_events",
                    "user_friends",
                    "user_gender",
                    "user_hometown",
                    "user_link",
                    "user_location",
                    "user_videos"
                    );

            if (!string.IsNullOrEmpty(m_LoginResult.AccessToken))
            {
                m_LoggedInUser = m_LoginResult.LoggedInUser;
                buttonLogin.Text = $"Logged in as {m_LoginResult.LoggedInUser.Name}";
                fetchUserInfo();
            }
            else
            {
                MessageBox.Show(m_LoginResult.ErrorMessage, commentGenerator.LoginFailed);
            }
        }

        private void fetchUserInfo()
        {
            ProfilePictureBox.LoadAsync(m_LoggedInUser.PictureNormalURL);
            new Thread(fetchPosts).Start();
            new Thread(fetchPhotos).Start();
            new Thread(fetchPages).Start();
        }

        private void fetchPosts()
        {
            m_PostsListBox.Invoke(new Action(() => m_PostsListBox.Items.Clear()));
            IEnumerable<Post> orderPosts = r_PostOrder.Order(m_LoggedInUser.Posts).Cast<Post>();

            foreach (Post post in orderPosts)
            {
                if (post != null && post.Message != null)
                {
                    m_PostsListBox.Invoke(new Action(() => m_PostsListBox.Items.Add(post)));
                }
            }

            if (m_PostsListBox.Items.Count == 0)
            {
                MessageBox.Show(commentGenerator.NoPostsToRetrieve);
            }
        }

        private void fetchPhotos()
        {
            m_PhotosListBox.Invoke(new Action(() => m_PhotosListBox.Items.Clear()));

            m_PhotosListBox.DisplayMember = "From";

            foreach (Photo photo in m_LoggedInUser.PhotosTaggedIn)
            {
                m_PhotosListBox.Invoke(new Action(() =>
                m_PhotosListBox.Items.Add(new PhotoProxy { m_Photo = photo, m_PhotoDescription = photo.Name })));
            }

            if (m_PhotosListBox.Items.Count == 0)
            {
                MessageBox.Show(commentGenerator.NoPhotosToRetrieve);
            }
        }

        private void listBoxPhotos_SelectedIndexChanged(object sender, EventArgs e)
        {
            displaySelectedPhoto(sender);
        }

        private void displaySelectedPhoto(object i_Sender)
        {
            string senderName = ((ListBox)i_Sender).Name;

            if (senderName == "m_PhotosListBox" && m_PhotosListBox.SelectedItems.Count == 1)
            {
                PhotoProxy selectedPhoto = m_PhotosListBox.SelectedItem as PhotoProxy;

                m_PhotosTextBoxPhotoDescription.Text = selectedPhoto.m_PhotoDescription;

                if (selectedPhoto.m_Photo.PictureNormalURL != null)
                {
                    m_PhotoPictureBox.LoadAsync(selectedPhoto.m_Photo.PictureNormalURL);
                }
                else
                {
                    m_PhotoPictureBox.Image = m_PhotoPictureBox.ErrorImage;
                }

                m_PhotosCreatedTimeLable.Text = selectedPhoto.CreatedTimeAndAge();
            }
            else if (senderName == "m_PhotosFromThisDayListBox" && m_PhotosFromThisDayListBox.SelectedItems.Count == 1)
            {
                PhotoProxy selectedPhoto = m_PhotosFromThisDayListBox.SelectedItem as PhotoProxy;

                if (selectedPhoto.m_Photo.PictureNormalURL != null)
                {
                    m_PhotosFromThisDayPictureBox.LoadAsync(selectedPhoto.m_Photo.PictureNormalURL);
                }
                else
                {
                    m_PhotosFromThisDayPictureBox.Image = m_PhotoPictureBox.ErrorImage;
                }

                m_PhotosFromThisDayCreatedTimeLabel.Text = selectedPhoto.CreatedTimeAndAge();
            }
            else if (senderName == "m_FavoritesPhotosListBox" && m_FavoritesPhotosListBox.SelectedItems.Count == 1)
            {
                PhotoProxy selectedPhoto = m_FavoritesPhotosListBox.SelectedItem as PhotoProxy;

                if (selectedPhoto.m_Photo.PictureNormalURL != null)
                {
                    m_FavoritePhotoPictureBox.LoadAsync(selectedPhoto.m_Photo.PictureNormalURL);
                }
                else
                {
                    m_FavoritePhotoPictureBox.Image = m_FavoritePhotoPictureBox.ErrorImage;
                }
            }
        }

        private void m_PhotosFromThisDayShowButton_Click(object sender, EventArgs e)
        {
            decimal selectedDay = m_PhotosFromThisDayDayNumericUpDown.Value;
            decimal selectedMonth = m_PhotosFromThisDayMonthNumericUpDown.Value;

            m_PhotosFromThisDayListBox.Items.Clear();
            m_PhotosFromThisDayListBox.DisplayMember = "From";

            foreach (Photo photo in m_LoggedInUser.PhotosTaggedIn)
            {
                if (photo.CreatedTime.Value.Day == selectedDay && photo.CreatedTime.Value.Month == selectedMonth)
                {
                    m_PhotosFromThisDayListBox.Items.Add(new PhotoProxy { m_Photo = photo });
                }
            }

            if (m_PhotosFromThisDayListBox.Items.Count == 0)
            {
                MessageBox.Show(commentGenerator.NoPhotosToRetrieve);
            }
        }

        private void fetchPages()
        {
            m_PagesListBox.Invoke(new Action(() => m_PagesListBox.Items.Clear()));

            m_PagesListBox.DisplayMember = "Name";

            try
            {
                foreach (Page page in m_LoggedInUser.LikedPages)
                {
                    m_PagesListBox.Invoke(new Action(() => m_PagesListBox.Items.Add(page)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (m_PagesListBox.Items.Count == 0)
            {
                MessageBox.Show(commentGenerator.NoLikedPageToRetrieve);
            }
        }

        private void listBoxPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            displaySelectedPage();
        }

        private void displaySelectedPage()
        {
            if (m_PagesListBox.SelectedItems.Count == 1)
            {
                Page selectedPage = (Page)m_PagesListBox.SelectedItem;

                if (selectedPage.PictureNormalURL != null)
                {
                    m_SelectedPagePictureBox.LoadAsync(selectedPage.PictureNormalURL);
                }
                else
                {
                    m_SelectedPagePictureBox.Image = m_SelectedPagePictureBox.ErrorImage;
                }

                m_SelectedPageNameLabel.Text = selectedPage.Name;
            }
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            FacebookService.LogoutWithUI();
            buttonLogin.Text = "Login";
        }

        private void m_SerachPostButton_Click(object sender, EventArgs e)
        {
            bool isFound = false;
            m_SearchPostListBox.Items.Clear();
            string searchVal = m_SearchPostTextBox.Text;

            foreach (Post post in m_LoggedInUser.Posts)
            {
                if (post.ToString().Contains(searchVal) && post != null)
                {
                    m_SearchPostListBox.Items.Add(post);
                    isFound = true;
                }
            }

            if (!isFound)
            {
                MessageBox.Show(commentGenerator.NoPostsToRetrieve);
            }
        }

        private void listBoxSearchPost_SelectedIndexChanged(object sender, EventArgs e)
        {
            displaySelectedPost(sender);
        }

        private void displaySelectedPost(object i_Sender)
        {
            string senderName = ((ListBox)i_Sender).Name;
            m_PostsPostCommentsListBox.Items.Clear();
            m_PostsPostCommentsListBox.DisplayMember = "Name";

            if (senderName == "m_PostsListBox" && m_PostsListBox.SelectedItems.Count == 1)
            {
                Post selectedPost = m_PostsListBox.SelectedItem as Post;
                m_PostsShowFullPostLabel.Text = selectedPost.Message;

                foreach (Comment comment in selectedPost.Comments)
                {
                    m_PostsPostCommentsListBox.Items.Add(comment);
                }

                m_PostsCreatedTimePostLabel.Text = String.Format("Created Time: {0}", selectedPost.CreatedTime);
            }
            else if (senderName == "m_SearchPostListBox" && m_SearchPostListBox.SelectedItems.Count == 1)
            {
                Post selectedPost = m_SearchPostListBox.SelectedItem as Post;
                m_SearchPostFullPostLabel.Text = selectedPost.Message;
                m_SearchPostCreatedTimeLabel.Text = String.Format("Created Time: {0}", selectedPost.CreatedTime);
            }
            else if (senderName == "m_FavoritesPostsListBox" && m_FavoritesPostsListBox.SelectedItems.Count == 1)
            {
                Post selectedPost = m_FavoritesPostsListBox.SelectedItem as Post;
                m_FavoriteFullPostLabel.Text = selectedPost.Message;
            }
        }

        private void listBoxPosts_SelectedIndexChanged(object sender, EventArgs e)
        {
            displaySelectedPost(sender);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        private void m_WriteNewPostButton_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(commentGenerator.StatusPosted);
                m_WriteNewPostTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void m_PostsLikeButton_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(commentGenerator.PostLiked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void m_PostsCommentButton_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(commentGenerator.CommentPosted);
                m_PostsCommentTextBox.Clear();
                m_SearchPostCommentTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void m_PostsAddToFavoriteButton_Click(object sender, EventArgs e)
        {
            string senderName = ((Button)sender).Name;
            m_FavoritesSingleton = FavoritesSingleton.Instance;

            if (senderName == "m_PostsAddToFavoriteButton" && m_PostsListBox.SelectedItems.Count == 1)
            {
                if (!checkIfPostIsExistsInFavorites(m_PostsListBox.SelectedItem))
                {
                    m_FavoritesPostsListBox.Items.Add(m_PostsListBox.SelectedItem);
                    m_FavoritesSingleton.AddPost((m_PostsListBox.SelectedItem) as Post);
                    MessageBox.Show(commentGenerator.AddedToFavorites);
                }
                else
                {
                    MessageBox.Show(commentGenerator.PostIsAlreadyInFavorites);
                }
            }
            else if (senderName == "m_SearchPostAddToFavoriteButton" && m_SearchPostListBox.SelectedItems.Count == 1)
            {
                if (!checkIfPostIsExistsInFavorites(m_SearchPostListBox.SelectedItem))
                {
                    m_FavoritesPostsListBox.Items.Add(m_SearchPostListBox.SelectedItem);
                    m_FavoritesSingleton.AddPost((m_SearchPostListBox.SelectedItem) as Post);
                    MessageBox.Show(commentGenerator.AddedToFavorites);
                }
                else
                {
                    MessageBox.Show(commentGenerator.PostIsAlreadyInFavorites);
                }
            }
            else
            {
                MessageBox.Show(commentGenerator.PleaseSelectOnePostToAdd);
            }
        }

        private bool checkIfPostIsExistsInFavorites(object i_Post)
        {
            foreach (Post post in m_FavoritesSingleton.m_FavoritesPosts)
            {
                if (i_Post == post)
                {
                    return true;
                }
            }

            return false;
        }

        private bool checkIfPhotoIsExistsInFavorites(object i_Photo)
        {
            foreach (PhotoProxy photo in m_FavoritesSingleton.m_FavoritesPhotos)
            {
                if (i_Photo == photo)
                {
                    return true;
                }
            }

            return false;
        }

        private void m_PhotosAddToFavoriteButton_Click(object sender, EventArgs e)
        {
            string senderName = ((Button)sender).Name;
            m_FavoritesSingleton = FavoritesSingleton.Instance;

            if (senderName == "m_PhotosAddToFavoriteButton" && m_PhotosListBox.SelectedItems.Count == 1)
            {
                if (!checkIfPhotoIsExistsInFavorites(m_PhotosListBox.SelectedItem))
                {
                    m_FavoritesPhotosListBox.Items.Add(m_PhotosListBox.SelectedItem);
                    m_FavoritesSingleton.AddPhoto((m_PhotosListBox.SelectedItem) as PhotoProxy);
                    MessageBox.Show(commentGenerator.AddedToFavorites);
                }
                else
                {
                    MessageBox.Show(commentGenerator.PhotoIsAlreadyInFavorites);
                }
            }
            else if (senderName == "m_PhotosFromThisDayAddToFavoriteButton" && m_PhotosFromThisDayListBox.SelectedItems.Count == 1)
            {
                if (!checkIfPhotoIsExistsInFavorites(m_PhotosFromThisDayListBox.SelectedItem))
                {
                    m_FavoritesPhotosListBox.Items.Add(m_PhotosFromThisDayListBox.SelectedItem);
                    m_FavoritesSingleton.AddPhoto((m_PhotosFromThisDayListBox.SelectedItem) as PhotoProxy);
                    MessageBox.Show(commentGenerator.AddedToFavorites);
                }
                else
                {
                    MessageBox.Show(commentGenerator.PhotoIsAlreadyInFavorites);
                }
            }
            else
            {
                MessageBox.Show(commentGenerator.PleaseSelectOnePhotoToAdd);
            }
        }

        private void m_FavoriteRemovePostButton_Click(object sender, EventArgs e)
        {
            if (m_FavoritesPostsListBox.SelectedItems.Count == 1)
            {
                m_FavoritesSingleton.RemovePost((m_FavoritesPostsListBox.SelectedItem) as Post);
                m_FavoritesPostsListBox.Items.Remove(m_FavoritesPostsListBox.SelectedItem);
                m_FavoriteFullPostLabel.Text = " ";
            }
            else
            {
                MessageBox.Show(commentGenerator.PleaseSelectOnePostToRemove);
            }
        }

        private void m_FavoriteRemovePhotoButton_Click(object sender, EventArgs e)
        {
            if (m_FavoritesPhotosListBox.SelectedItems.Count == 1)
            {
                m_FavoritesSingleton.RemovePhoto((m_FavoritesPhotosListBox.SelectedItem) as PhotoProxy);
                m_FavoritesPhotosListBox.Items.Remove(m_FavoritesPhotosListBox.SelectedItem);
                m_FavoritePhotoPictureBox.Image = null;
            }
            else
            {
                MessageBox.Show(commentGenerator.PleaseSelectOnePhotoToRemove);
            }
        }

        private void listBoxFavoritePosts_SelectedIndexChanged(object sender, EventArgs e)
        {
            displaySelectedPost(sender);
        }

        private void listBoxFavoritePhotos_SelectedIndexChanged(object sender, EventArgs e)
        {
            displaySelectedPhoto(sender);
        }

        private void m_PhotosTextBoxPhotoDescription_Validating(object sender, CancelEventArgs e)
        {
            PhotoProxy selectedPhoto = m_PhotosListBox.SelectedItem as PhotoProxy;

            if (selectedPhoto != null)
            {
                selectedPhoto.m_PhotoDescription = m_PhotosTextBoxPhotoDescription.Text;
            }
        }

        private void m_PhotosSpeakButton_Click(object sender, EventArgs e)
        {
            if (m_PhotosListBox.SelectedItems.Count == 1)
            {
                ISpeakable speakableItem = new SpeakItem() { Adoptee = (m_PhotosListBox.SelectedItem) as PhotoProxy };
                speakableItem.Speak();
            }
        }

        private void m_PostsSortByComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            r_PostOrder.SetOrderStrategy((eOrderStrategyType)m_PostsSortByComboBox.SelectedIndex);

            if (m_LoginResult != null)
            {
                fetchPosts();
            }
        }
    } 
}
