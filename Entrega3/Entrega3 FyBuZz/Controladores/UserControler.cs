using Entrega3_FyBuZz.CustomArgs;
using Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Entrega3_FyBuZz.Controladores
{
    public class UserControler
    {
        List<User> userDataBase = new List<User>() { new User() };

        DataBase dataBase = new DataBase();
        static DataBase data = new DataBase();
        Modelos.Menu menu = new Modelos.Menu();
        Server server = new Server(data);
        FyBuZz fyBuZz;


        public UserControler(Form fyBuZz)
        {
            Initialize();
            this.fyBuZz = fyBuZz as FyBuZz;
            this.fyBuZz.LogInLogInButton_Clicked2 += userGetter;
            this.fyBuZz.GetProfileNames += GetUserProfiles;
            this.fyBuZz.LogInLogInButton_Clicked += OnLoginButtonClicked;
            this.fyBuZz.RegisterRegisterButton_Clicked += OnRegisterRegisterButtonClicked;
            this.fyBuZz.CreateProfileCreateProfileButton_Clicked += OnCreateProfileCreateProfileButton_Clicked;
            this.fyBuZz.ProfilesChooseProfile_Clicked2 += OnProfilesChooseProfile_Click2;
            this.fyBuZz.ProfilesChooseProfile_Clicked += OnProfilesChooseProfile_Click;
            this.fyBuZz.SearchUserButton_Clicked += OnSearchUserButton_Click;
            this.fyBuZz.SearchFollowButton_Clicked += OnSearchFollowButton_Click;
            this.fyBuZz.UserProfileChangeInfoConfirmButton_Clicked += OnUserProfileChangeInfoConfirmButton_Click;
            this.fyBuZz.AddSearchedMult_Done += AddSearchedMultimedia;
            this.fyBuZz.ReturnSearchedMult_Done += ReturnSearchMultList;
            this.fyBuZz.AdminMethods_Done += AdminMethods;
            this.fyBuZz.AddedLikedMult += AddLikedMultimedia;
            this.fyBuZz.ReturnLikedMult_Done += ReturnLikedMultList;
            this.fyBuZz.ProfileDeleted += DeleteProfile;
            this.fyBuZz.SharedMultSetter += SetSharedMult;
            this.fyBuZz.SharedMultGetter += GetSharedMult;
        }

        public void Initialize()
        {
            if (File.Exists("AllUsers.bin") != true) dataBase.Save_Users(userDataBase);
            userDataBase = dataBase.Load_Users();
        }

        private int UserIndex(ProfileEventArgs e)
        {
            int u = 0;
            foreach(User user in userDataBase)
            {
                if (user.Username == e.UsernameText && user.Password == e.PasswordText)
                {
                    break;
                }
                u++;
            }
            return u;
        }

        private User OnLoginButtonClicked(object sender, LogInEventArgs e)
        {
            
            return dataBase.LogIn(e.UsernameText, e.PasswrodText, userDataBase);
        }
        private bool OnRegisterRegisterButtonClicked(object sender, RegisterEventArgs e)
        {
            User user = new User();
            bool x;
            x = server.Register(user, userDataBase,e.UsernameText, e.EmailText, e.PasswrodText, e.SubsText, e.PrivacyText, e.GenderText, e.BirthdayText, e.ProfileTypeText);
            dataBase.Save_Users(userDataBase);
            return x;
        }
        private string OnCreateProfileCreateProfileButton_Clicked(object sender, ProfileEventArgs e)
        {
            int u = UserIndex(e);
            if (userDataBase[u].Accountype != "standard")
            {   
                int pAge = DateTime.Now.Year - e.BirthdayText.Year;
                string pPic = "...";
                string name = userDataBase[u].Username;
                Profile profile = userDataBase[u].CreateProfile(e.ProfileNameText, pPic, e.ProfileTypeText, e.EmailText, e.GenderText, pAge);
                userDataBase[u].Perfiles.Add(profile);
                dataBase.Save_Users(userDataBase);
                return profile.ProfileName;
            }
            else
            {
                return null;
            }
        }

        private List<string> userGetter(object sender, UserEventArgs e)
        {
            List<string> userGetterString = new List<string>();
            foreach(User user in userDataBase)
            {
                if(user.Username == e.UsernameText)
                {
                    userGetterString.Add(user.Username);
                    userGetterString.Add(user.Password);
                    userGetterString.Add(user.Email);
                    userGetterString.Add(user.Accountype);
                    userGetterString.Add(user.Followers.ToString());
                    userGetterString.Add(user.Following.ToString());
                    userGetterString.Add(user.Verified.ToString());
                    userGetterString.Add(user.AdsOn.ToString());
                    userGetterString.Add(user.Privacy.ToString());
                    userGetterString.Add(user.Banned.ToString());
                }
            }
            if(userGetterString.Count == 0)
            {
                return null;
            }
            return userGetterString;
        }
        private List<List<string>> userListsGetter(object sender, UserEventArgs e)
        {
            List<List<string>> userGetterList = new List<List<string>>();
            foreach (User user in userDataBase)
            {
                if (user.Username == e.UsernameText)
                {
                    userGetterList.Add(user.FollowingList);
                    userGetterList.Add(user.FollowerList);
                }
            }
            if (userGetterList.Count == 0)
            {
                return null;
            }
            return userGetterList;
        }
        private List<string> GetUserProfiles(object sender, UserEventArgs e)
        {
            List<string> userProfileList = new List<string>();
            foreach (User user in userDataBase)
            {
                string usr = user.Username;
                if (user.Username == e.UsernameText)
                {
                    foreach(Profile profile in user.Perfiles)
                    {
                        userProfileList.Add(profile.ProfileName);
                    }
                }
            }
            if (userProfileList.Count == 0)
            {
                return null;
            }
            return userProfileList;
        }

        private Profile OnProfilesChooseProfile_Click(object sender, ProfileEventArgs e)

        {
            List<string> profileGetterString = new List<string>();
            int u = UserIndex(e);
            int pAge = DateTime.Now.Year - e.BirthdayText.Year;

            Profile prof = new Profile(e.ProfileNameText,"..",e.ProfileTypeText,e.EmailText,e.GenderText, pAge);
            foreach(User user in userDataBase)
            {
                if(user.Username == e.UsernameText)
                {
                    foreach (Profile profile in user.Perfiles)
                    {
                        if (profile.ProfileName == prof.ProfileName || profile.Username == prof.ProfileName)
                        {
                            prof = profile;
                        }
                    }
                }
                
            }
            
            return prof;
        }
        private List<string> OnProfilesChooseProfile_Click2(object sender, ProfileEventArgs e)
        {
            List<string> profileGetterString = new List<string>();
            int u = UserIndex(e);
            int pAge = DateTime.Now.Year - e.BirthdayText.Year;
            Profile prof = new Profile(e.ProfileNameText, "..", e.ProfileTypeText, e.EmailText, e.GenderText, pAge);
            foreach (Profile profile in userDataBase[u].Perfiles)
            {
                if (profile.ProfileName == prof.ProfileName || profile.Username == prof.ProfileName)
                {
                    profileGetterString.Add(profile.ProfileName);
                    profileGetterString.Add(profile.ProfileType);
                    profileGetterString.Add(profile.Gender);
                    profileGetterString.Add(profile.Age.ToString());
                }
            }
            if (profileGetterString.Count() == 0)
            {
                return null;
            }
            return profileGetterString;
        }
        private List<User> OnSearchUserButton_Click(object sender, RegisterEventArgs e)
        {
            dataBase.Save_Users(userDataBase);
            return userDataBase;
        }
        private string OnSearchFollowButton_Click(object sender, UserEventArgs e)
        {
            List<string> listuser = e.UserLogIn.FollowingList;
            List<PlayList> followedPL = e.ProfileUserLogIn.FollowedPlayList;
            string tryingToFollow = e.UserSearched.Username;
            string result = null;

            if (listuser.Contains(tryingToFollow) == false)
            {
                e.UserSearched.Followers = e.UserSearched.Followers + 1;
                e.UserLogIn.Following = e.UserLogIn.Following + 1;
                if (e.UserSearched.ProfilePlaylists != null)
                {
                    foreach (PlayList Pls in e.UserSearched.ProfilePlaylists)
                    {
                        followedPL.Add(Pls);
                    }
                }
                /*else
                {
                    result = "Error, this user don't have created playlists";
                }*/

                e.UserLogIn.FollowingList.Add(e.UserSearched.Username);
                e.UserSearched.FollowerList.Add(e.UserLogIn.Username);
                result = "Followed: " + e.UserSearched.SearchedInfoUser();
                dataBase.Save_Users(userDataBase);
            }
            return result;
        }
        private string OnUserProfileChangeInfoConfirmButton_Click(object sender, UserEventArgs e)
        {
            string result = null;
            if(e.WantToChangeText == 1)
            {
                foreach(User user in userDataBase)
                {
                    if(user.Username == e.UsernameText && user.Password == e.PasswordText)
                    {
                        int cont1 = 0;
                        foreach (User usr in userDataBase)
                        {
                            if(usr.Username == e.ChangedText)
                            {
                                cont1++;
                            }
                            
                        }
                        if (cont1 == 0)
                        {
                            user.Username = e.ChangedText;
                            result = "Succesfully changed Username";
                        }

                    }
                }
            }
            else if (e.WantToChangeText == 2)
            {
                foreach (User user in userDataBase)
                {

                    if (user.Username == e.UsernameText && user.Password == e.PasswordText)
                    {
                        user.Password = e.ChangedText;
                        result = "Succesfully changed Password";
                    }
                }
            }
            else
            {
                foreach (User user in userDataBase)
                {
                    if (user.Username == e.UsernameText && user.Password == e.PasswordText)
                    {

                        if(user.Accountype == "standard" && int.Parse(e.ChangedText) >= 7999)
                        {
                            user.Accountype = "premium";
                            result = "Succesfully changed AccounType";
                        }
                        else if(user.Accountype != "standard")
                        {
                            return null;
                        }
                        else if(int.Parse(e.ChangedText) < 7999)
                        {
                            return null;
                        }
                        

                    }
                }
            }
            dataBase.Save_Users(userDataBase);
            return result;
        }
        private bool AddSearchedMultimedia(object sender, UserEventArgs e)
        {
            bool added = false;
            Profile usedProfile = null;
            foreach(User user in userDataBase)
            {
                foreach(Profile profile in user.Perfiles)
                {
                    if(profile.ProfileName == e.ProfilenameText)
                    {
                        usedProfile = profile;
                    }
                }
            }
            if(e.SongFileText != null && usedProfile.PersSongPlaylist.Contains(e.SongFileText) == false)
            {
                usedProfile.PersSongPlaylist.Add(e.SongFileText);
                added = true;
            }
            else if(e.VideoFileText != null && usedProfile.PersVideoPlaylist.Contains(e.VideoFileText) == false)
            {
                usedProfile.PersVideoPlaylist.Add(e.VideoFileText);
                added = true;
            }
            dataBase.Save_Users(userDataBase);
            return added;
        }
        private bool AddLikedMultimedia(object sender, UserEventArgs e)
        {
            bool added = false;
            Profile usedProfile = null;
            foreach (User user in userDataBase)
            {
                foreach (Profile profile in user.Perfiles)
                {
                    if (profile.ProfileName == e.ProfilenameText)
                    {
                        usedProfile = profile;
                    }
                }
            }
            if (e.SongFileText != null && usedProfile.PlaylistFavoritosSongs2.Contains(e.SongFileText) == false)
            {
                usedProfile.PlaylistFavoritosSongs2.Add(e.SongFileText);
                added = true;
            }
            else if (e.VideoFileText != null && usedProfile.PlaylistFavoritosVideos2.Contains(e.VideoFileText) == false)
            {
                usedProfile.PlaylistFavoritosVideos2.Add(e.VideoFileText);
                added = true;
            }
            dataBase.Save_Users(userDataBase);
            return added;
        }
        private List<string> ReturnLikedMultList(object sender, UserEventArgs e)
        {
            List<string> searchMultList = new List<string>();
            foreach (User user in userDataBase)
            {
                foreach (Profile profile in user.Perfiles)
                {
                    if (profile.ProfileName == e.ProfilenameText)
                    {
                        if (e.VideoFileText == "Video")
                        {
                            searchMultList = profile.PlaylistFavoritosVideos2;
                        }
                        else if (e.SongFileText == "Song")
                        {
                            searchMultList = profile.PlaylistFavoritosSongs2;
                        }
                    }
                }
            }
            return searchMultList;
        }
        private List<string> ReturnSearchMultList(object sender, UserEventArgs e)
        {
            List<string> searchMultList = new List<string>();
            foreach (User user in userDataBase)
            {
                foreach (Profile profile in user.Perfiles)
                {
                    if (profile.ProfileName == e.ProfilenameText)
                    {
                        if(e.VideoFileText == "Video")
                        {
                            searchMultList = profile.PersVideoPlaylist;
                        }
                        else if(e.SongFileText == "Song")
                        {
                            searchMultList = profile.PersSongPlaylist;
                        }
                    }
                }
            }
            return searchMultList;
        }
        private string AdminMethods(object sender, UserEventArgs e)
        {
            string result = null;
            if (e.WantToChangeText == 0)
            {
                foreach (User user in userDataBase)
                {
                    if (e.UsernameText == user.Username)
                    {
                        userDataBase.Remove(user);
                        result = "User erased";
                        break;
                    }
                }
            }
            else if (e.WantToChangeText == 1)
            {
                foreach (User user in userDataBase)
                {
                    if (e.UsernameText == user.Username)
                    {
                        user.Banned = 1;
                        result = "User banned";
                        break;
                    }
                }
            }
            else if (e.WantToChangeText == 2)
            {
                foreach (User user in userDataBase)
                {
                    if (e.UsernameText == user.Username)
                    {
                        user.Banned = 0;
                        result = "User unBanned";
                        break;
                    }
                }
            }

            dataBase.Save_Users(userDataBase);
            return result;
        }
        private bool DeleteProfile(object sender, UserEventArgs e)
        {
            foreach(User user in userDataBase)
            {
                if(user.Username == e.UsernameText)
                {
                    int cont = 0;
                    foreach(Profile profile in user.Perfiles)
                    {
                        if(profile.ProfileName == e.ProfilenameText)
                        {
                            user.Perfiles.RemoveAt(cont);
                            dataBase.Save_Users(userDataBase);
                            return true;
                        }
                        cont++;
                    }
                    break;
                }
            }
            return false;
        }

        private string SetSharedMult(object sender, UserEventArgs e)
        {
            string description = null;
            foreach(User user in userDataBase)
            {
                if(user.Username == e.UsernameText)
                {
                    user.SharedMult = e.SharedMult;             //<Nombre del usuario que te lo comportio> + "//" + <archivo>
                    description = "Shared succesfully";
                }
            }
            dataBase.Save_Users(userDataBase);
            return description;
        }

        private List<string> GetSharedMult(Object sender, UserEventArgs e)
        {
            List<string> description = new List<string>();
            foreach(User user in userDataBase)
            {
                if (user.Username == e.UsernameText)
                {
                    description = user.SharedMult;
                }
            }
            return description;
        }

        /*private List<Song> GetPrivPersPlaylist(object sender, UserEventArgs e)
        {
            List<Song> plSongs = new List<Song>();
            foreach(User user in userDataBase)
            {
                if(user.Username == e.UsernameText)
                {
                    foreach (Profile profile in user.Perfiles)
                    {
                        if(profile.ProfileName == e.ProfilenameText)
                        {
                            if (e.playlistType.Contains("Favorite"))
                            {
                                plSongs = profile.
                            }
                        }
                    }
                }
                
            }
        }*/
    }
    
}
