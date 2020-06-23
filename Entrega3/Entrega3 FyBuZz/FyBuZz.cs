using Entrega3_FyBuZz.CustomArgs;
using Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using System.Media;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;

namespace Entrega3_FyBuZz
{
    public partial class FyBuZz : Form
    {
        /*
        INDICE CODIGO: (Puedes apretar Ctrl + F y copiar los nombres que estan abajo para encontrar la sección que buscas)
                       (Puedes usar el COMMAND o el CONTENIDO si quieres buscar más especifico)
        ╔═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
        ║   | NAME: WELCOME                  | COMMAND: !WW      |  CONTENIDO: !WELCOME PANEL, !REGISTER PANEL, !LOGIN PANEL                    ║ 
        ║   | NAME: PROFILE                  | COMMAND: !PRE     |  CONTENIDO: !PROFILE PANEL, !CREATE PROFILE PANEL                            ║
        ║   | NAME: MENU                     | COMMAND: !M       |  CONTENIDO: !DISPLAY START PANEL                                             ║
        ║   | NAME: SEARCH                   | COMMAND: !SCH     |  CONTENIDO: !SEARCH PANEL, !SEARCH USER PANEL                                ║
        ║   | NAME: PLAY                     | COMMAND: !PY      |  CONTENIDO: !PLAY SONG PANEL, !PLAY VIDEO PANEL, !PLAY PLAYLIST PANEL        ║
        ║   | NAME: ADD SHOW                 | COMMAND: !AS      |  CONTENIDO: !ADD SHOW PANEL                                                  ║
        ║   | NAME: ADD                      | COMMAND: !!A      |  CONTENIDO: !CREATE SONG PANEL, !CREATE VIDEO PANEL, !CREATE PLAYLIST PANEL  ║
        ║   | NAME: SHOW                     | COMMAND: !!S      |  CONTENIDO:                                                                  ║
        ║   | NAME: DISPLAY PLAYLIST         | COMMAND: !DP      |  CONTENIDO: !DISPLAY PLAYLIST PANEL                                          ║
        ║   | NAME: ACCOUNT PROFILE SETTINGS | COMMAND: !APS     |  CONTENIDO: !ACCOUNT PROFILE SETTINGS PANEL                                  ║
        ║   | NAME: USER CHANGE              | COMMAND: !UC      |  CONTENIDO: !USER PROFILE CHANGE INFO PANEL                                  ║
        ║   | NAME: ADMIN MENU               | COMMAND: !AM      |  CONTENIDO: !ADMIN MENU PANEL                                                ║
        ╚═══════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝   
         */

        //PUBLIC DELEGATES
        //--------------------------------------------------------------------------------


        SoundPlayer soundPlayer;
        private List<string> badWords = new List<string>() { "fuck", "sex", "niggas", "sexo", "ass", "nigger", "culo", "viola", "violar", "spank", "puta", "hooker", "perra", "hoe", "cocaina", "alchohol", "blunt", "weed", "marihuana", "lcd", "kush", "krippy", "penis", "dick", "cock", "shit", "percocet" };

        public delegate List<string> UserGetterListEventHandler(object source, UserEventArgs args);
        public event UserGetterListEventHandler LogInLogInButton_Clicked2;

        public delegate List<string> UserProfilesNames(object source, UserEventArgs args);
        public event UserProfilesNames GetProfileNames;

        public delegate User LogInEventHandler(object soruce, LogInEventArgs args);
        public event LogInEventHandler LogInLogInButton_Clicked;

        public delegate List<string> GetSongInfo(object source, SongEventArgs args);
        public event GetSongInfo GetSongInformation;

        public delegate List<List<string>> GetAllSongsInfo(object source, SongEventArgs args);
        public event GetAllSongsInfo GetAllSongInformation;

        public delegate List<string> GetVideoInfo(object source, VideoEventArgs args);
        public event GetVideoInfo GetVideoInformation;

        public delegate List<List<string>> GetAllVideosInfo(object source, VideoEventArgs args);
        public event GetAllVideosInfo GetAllVideosInformation;

        public delegate string ReturnChangedInfo(object source, UserEventArgs args);
        public event ReturnChangedInfo UserProfileChangeInfoConfirmButton_Clicked;

        public delegate bool RegisterEventHandler(object soruce, RegisterEventArgs args);
        public event RegisterEventHandler RegisterRegisterButton_Clicked;

        public delegate string ProfileEventHandler(object source, ProfileEventArgs args);
        public event ProfileEventHandler CreateProfileCreateProfileButton_Clicked;

        public delegate List<string> ChooseProfileEventHandlerMVC(object source, ProfileEventArgs args);
        public event ChooseProfileEventHandlerMVC ProfilesChooseProfile_Clicked2;

        public delegate Profile ChooseProfileEventHandler(object source, ProfileEventArgs args);
        public event ChooseProfileEventHandler ProfilesChooseProfile_Clicked;

        public delegate List<PlayList> ChoosePlaylistEventHandler(object source, PlaylistEventArgs args);
        public event ChoosePlaylistEventHandler DisplayPlaylistsGlobalPlaylist_Clicked;

        public delegate bool SongEventHandler(object source, SongEventArgs args);
        public event SongEventHandler CreateSongCreateSongButton_Clicked;

        public delegate List<Song> ListSongEventHandler(object source, SongEventArgs args);
        public event ListSongEventHandler SearchSongButton_Clicked;


        public delegate List<User> ListUserEventHandler(object source, RegisterEventArgs args);
        public event ListUserEventHandler SearchUserButton_Clicked;

        public delegate string SearchedUserEventHandler(object source, UserEventArgs args);
        public event SearchedUserEventHandler SearchFollowButton_Clicked;

        public delegate string PlaylistEventHandler(object source, PlaylistEventArgs args);
        public event PlaylistEventHandler CreatePlaylistCreatePlaylistButton_Clicked;

        public delegate bool CreateVideoEventHandler(object source, VideoEventArgs args);
        public event CreateVideoEventHandler CreateVideoSaveButton_Clicked;

        public delegate List<Video> ListVideoEventHandler(object source, VideoEventArgs args);
        public event ListVideoEventHandler SearchVideoButton_Clicked;

        public delegate string ChoosePLEventHanlder(object source, PlaylistEventArgs args);
        public event ChoosePLEventHanlder PlaySongChoosePlsButton_Clicked;

        public delegate string SelectPlVideoEventHandler(object source, PlaylistEventArgs args);
        public event SelectPlVideoEventHandler PlayVideoSelectPlButton_Clicked;

        public delegate string RateSongEventHandler(object source, SongEventArgs args);
        public event RateSongEventHandler PlaysSongRateButton_Clicked;

        public delegate string RateVideoEventHandler(object source, VideoEventArgs args);
        public event RateVideoEventHandler PlaysVideoRateButton_Clicked;

        public delegate Song SkipOrPreviousSongEventHandler(object source, SongEventArgs args);
        public event SkipOrPreviousSongEventHandler SkipOrPreviousSongButton_Clicked;

        public delegate Video SkipOrPreviousVideoEventHandler(object source, VideoEventArgs args);
        public event SkipOrPreviousVideoEventHandler SkipOrPreviousVideoButton_Clicked;

        public delegate bool AddSearchedMult(object sender, UserEventArgs args);
        public event AddSearchedMult AddSearchedMult_Done;

        public delegate List<string> GetSearchedMult(object sender, UserEventArgs args);
        public event GetSearchedMult ReturnSearchedMult_Done;

        public delegate string AddPlaylistMult(object sender, PlaylistEventArgs args);
        public event AddPlaylistMult AddPlaylistMult_Done;

        public delegate string AdminMenu(object sender, UserEventArgs args);
        public event AdminMenu AdminMethods_Done;

        public delegate string LikeSong(object source, SongEventArgs args);
        public event LikeSong LikedSong_Done;

        public delegate string LikeVideo(object source, VideoEventArgs args);
        public event LikeVideo LikedVideo_Done;

        public delegate bool AddLikeSong(object source, UserEventArgs args);
        public event AddLikeSong AddedLikedMult;

        public delegate List<string> GetLikedMult(object sender, UserEventArgs args);
        public event GetLikedMult ReturnLikedMult_Done;

        public delegate List<PlayList> GetPrivatePlaylists(object sender, PlaylistEventArgs args);
        public event GetPrivatePlaylists ReturnPrivatePls;

        public delegate bool DeleteProfile(object sender, UserEventArgs args);
        public event DeleteProfile ProfileDeleted;

        public delegate List<string> ReturnSongInfo_Done(object sender, SongEventArgs args);
        public event ReturnSongInfo_Done ReturnSongInfo_Did;

        public delegate string ShareMultSetter(object sender, UserEventArgs args);
        public event ShareMultSetter SharedMultSetter;

        public delegate List<string> ShareMultGetter(object sender, UserEventArgs args);
        public event ShareMultGetter SharedMultGetter;

        //ATRIBUTOS
        //--------------------------------------------------------------------------------
        private string ProfileName { get; set; }
        private List<string> queueListSongs = new List<string>();
        private int songIndex = -1;
        private int videoIndex = -1;
        private string formatProgressBar = "";
        private double durationWav = 0;
        private int ticks = 0;
        private bool isShowing;
        private bool volumeIcon = false;
        private int notification = 0;

        private bool isInPlaylist = false;

        //--------------------------------------------------------------------------------


        //CONSTRUCTOR
        //--------------------------------------------------------------------------------
        public FyBuZz()
        {
            InitializeComponent();
            Diseno_Oculto();
        }
        private void FyBuZz_Load(object sender, EventArgs e)
        {

        }
        //--------------------------------------------------------------------------------

        /*---------------------------------------------------!WW----------------------------------------------------- */


        //<<!WELCOME PANEL>>
        //-------------------------------------------------------------------------------------------

        //BUTTONS
        private void WelcomeRegisterButton_Click(object sender, EventArgs e)
        {
            UserLogInTextBox.Clear();
            PasswordLogInTextBox.Clear();
            LogInInvalidCredentialsTetxbox.Clear();
            LogInInvalidCredentialsTetxbox.Visible = false;
            RegisterPanel.BringToFront();
        }

        private void LogInLogInButton_Click_1(object sender, EventArgs e)
        {
            SideMenuPanel.Visible = false;
            LogInInvalidCredentialsTetxbox.Clear();

            //LogInPanel.BringToFront();
            LogInInvalidCredentialsTetxbox.Clear();
            List<string> userGetter = new List<string>();
            string username = UserLogInTextBox.Text;
            string pass = PasswordLogInTextBox.Text;
            userGetter = OnLogInLogInButton_Clicked2(username);
            //user = OnLoginButtonClicked(username, pass);
            if (userGetter != null && userGetter[1] == pass && userGetter[9] != "1")
            {
                ProfilesInvalidCredentialTextBox.Clear();
                ProfilePanel.BringToFront();
            }
            else
            {
                LogInInvalidCredentialsTetxbox.Clear();
                LogInInvalidCredentialsTetxbox.Text = "Incorrect Username or Password";
                Thread.Sleep(2000);
                LogInInvalidCredentialsTetxbox.Visible = true;
            }
            LogInInvalidCredentialsTetxbox.Clear();
            ProfilesWelcomeTextBox.Clear();
        }




        //GO BACK/CLOSE

        private void WelcomeCloseFyBuZz_Click_1(object sender, EventArgs e)
        {

            Close();

        }

        //-------------------------------------------------------------------------------------------




        //<<!REGISTER PANEL>>
        //-------------------------------------------------------------------------------------------

        private void RegisterRegisterButton_Click_1(object sender, EventArgs e)
        {
            string username = UsernameRegisterTextBox.Text;
            string email = EmailRegisterTextBox.Text;
            string pswd = PasswordRegisterTextBox.Text;
            string subs = SubscriptionRegisterDomainUp.Text;
            bool privacy = PrivacyRegisterCheckBox.Checked; // true = privado
            string gender = GenderRegisterDomainUp.Text;
            DateTime birthday = AgeRegisterDateTimePicker.Value;
            string profileType = ProfileTypeRegisterDomainUp.Text;
            OnRegisterRegisterButtonClicked(username, email, pswd, subs, privacy, gender, birthday, profileType);
            WelcomePanel.BringToFront();
        }



        //ONEVENT
        public void OnRegisterRegisterButtonClicked(string username, string email, string psswd, string subs, bool priv, string gender, DateTime birthday, string profileType)
        {
            if (RegisterRegisterButton_Clicked != null)
            {
                bool result = RegisterRegisterButton_Clicked(this, new RegisterEventArgs() { UsernameText = username, EmailText = email, PasswrodText = psswd, SubsText = subs, PrivacyText = priv, GenderText = gender, BirthdayText = birthday, ProfileTypeText = profileType });
                if (!result) //Resultado es falso
                {
                    RegisterInvalidCredencialsTextBox.AppendText("User already exist");
                    RegisterInvalidCredencialsTextBox.Visible = true;
                    Thread.Sleep(2000);
                    UsernameRegisterTextBox.Clear();
                    EmailRegisterTextBox.Clear();
                    PasswordRegisterTextBox.Clear();
                    RegisterInvalidCredencialsTextBox.Clear();

                }
                else
                {
                    RegisterInvalidCredencialsTextBox.AppendText("Registered Succesfull");
                    RegisterInvalidCredencialsTextBox.Visible = true;
                    UsernameRegisterTextBox.Clear();
                    EmailRegisterTextBox.Clear();
                    PasswordRegisterTextBox.Clear();
                    RegisterInvalidCredencialsTextBox.Clear();
                    Thread.Sleep(2000);
                }
            }
        }

        //TEXT BOX WATERMARK


        private void UsernameRegisterTextBox_Enter(object sender, EventArgs e)
        {
            if (UsernameRegisterTextBox.Text == "Ahoward")
            {
                UsernameRegisterTextBox.Text = "";
                UsernameRegisterTextBox.ForeColor = Color.White;
            }
        }

        private void UsernameRegisterTextBox_Leave(object sender, EventArgs e)
        {
            if (UsernameRegisterTextBox.Text == "")
            {
                UsernameRegisterTextBox.Text = "Ahoward";
                UsernameRegisterTextBox.ForeColor = Color.Gold;
            }
        }

        private void EmailRegisterTextBox_Enter(object sender, EventArgs e)
        {
            if (EmailRegisterTextBox.Text == "ahoward@uandes.cl")
            {
                EmailRegisterTextBox.Text = "";
                EmailRegisterTextBox.ForeColor = Color.White;
            }
        }

        private void EmailRegisterTextBox_Leave(object sender, EventArgs e)
        {
            if (EmailRegisterTextBox.Text == "")
            {
                EmailRegisterTextBox.Text = "ahoward@uandes.cl";
                EmailRegisterTextBox.ForeColor = Color.Gold;
            }

        }

        //GO BACK/CLOSE

        //-------------------------------------------------------------------------------------------

        private void GoBackRegisterButton_Click_1(object sender, EventArgs e)
        {
            WelcomePanel.BringToFront();
        }


        //<<!LOGIN PANEL>>
        //-------------------------------------------------------------------------------------------


        //ONEVENT

        public User OnLoginButtonClicked(string username, string password)
        //Este metodo ya no deberia servir, no es MVC
        {
            User user = new User();
            if (LogInLogInButton_Clicked != null)
            {
                user = LogInLogInButton_Clicked(this, new LogInEventArgs() { UsernameText = username, PasswrodText = password });
                if (user == null) //Resultado es falso
                {
                    LogInInvalidCredentialsTetxbox.AppendText("Incorrect Username or Password");
                    Thread.Sleep(2000);
                    LogInInvalidCredentialsTetxbox.Visible = true;
                }
                else
                {
                    LogInInvalidCredentialsTetxbox.AppendText("Log-In Succesfull");
                    Thread.Sleep(2000);
                    LogInInvalidCredentialsTetxbox.Visible = true;

                    ProfilesWelcomeTextBox.AppendText("Welcome to FyBuZz " + user.Username);
                    foreach (Profile profile in user.Perfiles)
                    {
                        ProfileDomainUp.Items.Add(profile.ProfileName);
                    }
                }
            }
            return user;
        }

        public List<string> OnLogInLogInButton_Clicked2(string username)
        {
            List<string> userGetterStringList = new List<string>();
            List<string> userProfileNames = new List<string>();
            if (LogInLogInButton_Clicked2 != null)
            {
                userGetterStringList = LogInLogInButton_Clicked2(this, new UserEventArgs() { UsernameText = username });
                if (userGetterStringList != null && userGetterStringList[1] == PasswordLogInTextBox.Text)
                {
                    LogInInvalidCredentialsTetxbox.AppendText("Log-In Succesfull");
                    Thread.Sleep(2000);
                    LogInInvalidCredentialsTetxbox.Visible = true;

                    ProfilesWelcomeTextBox.AppendText("Welcome to FyBuZz " + userGetterStringList[0]);
                    userProfileNames = GetProfileNames(this, new UserEventArgs() { UsernameText = username });
                    foreach (string profilename in userProfileNames)
                    {
                        ProfileDomainUp.Items.Add(profilename);
                    }
                    return userGetterStringList;
                }
                else
                {
                    LogInInvalidCredentialsTetxbox.AppendText("Incorrect Username or Password");
                    Thread.Sleep(2000);
                    LogInInvalidCredentialsTetxbox.Visible = true;
                    return null;
                }

            }
            return null;
        }

        //GO BACK/CLOSE

        //-------------------------------------------------------------------------------------------



        /*---------------------------------------------------!PRE----------------------------------------------------- */


        //<<!PROFILE PANEL>>
        //-------------------------------------------------------------------------------------------
        private void ProfilesChooseProfile_Click_1(object sender, EventArgs e)
        {
            AdminMenuButton.Visible = false;
            SharedMultNotificationButton.Visible = false;
            DisplayStartChooseSharedMult.Visible = false;
            DisplayStartNotificationDomainUp.Visible = false;

            ProfilesInvalidCredentialTextBox.Clear();
            soundPlayer = new SoundPlayer();
            string username = UserLogInTextBox.Text;

            string password = PasswordLogInTextBox.Text;
            string profileProfileName = ProfileDomainUp.Text;

            List<string> profileGetterString = OnProfilesChooseProfile_Click2(profileProfileName, username, password);


            List<Song> songDataBase = OnSearchSongButton_Click();
            List<Video> videoDataBase = OnSearchVideoButton_Click();

            List<string> allSharedMultInfo = new List<string>();

            if (profileGetterString != null)
            {
                List<string> userInfo = OnLogInLogInButton_Clicked2(UserLogInTextBox.Text);
                if (userInfo[3] == "admin")
                {
                    AdminMenuButton.Visible = true;
                }

                ProfileName = profileProfileName;
                //Hace el metodo de obtener la multimedia que te comparten
                allSharedMultInfo = SharedMultGet(username);

                if (allSharedMultInfo != null && allSharedMultInfo.Count != 0)
                {
                    int aux = allSharedMultInfo.Count();
                    DisplayStartChooseSharedMult.Visible = true;
                    DisplayStartNotificationDomainUp.Visible = true;

                    string multName = null;

                    string[] sharedMultInfo = allSharedMultInfo[aux - 1].Split('/');
                    if (sharedMultInfo[1].Contains(".mp3") || sharedMultInfo[1].Contains(".waV"))
                    {
                        foreach (Song song in songDataBase)
                        {
                            if (song.SongFile == sharedMultInfo[1])
                            {
                                multName = song.Name;
                                DisplayStartMultimediaInfoDomainUp.Items.Add(song.Name + ":" + song.Artist);
                                break;
                            }
                        }
                    }
                    else if (sharedMultInfo[1].Contains(".mp4") || sharedMultInfo[1].Contains(".avi") || sharedMultInfo[1].Contains(".mov"))
                    {
                        foreach (Video video in videoDataBase)
                        {
                            if (video.FileName == sharedMultInfo[1])
                            {
                                multName = video.Name;
                                DisplayStartMultimediaInfoDomainUp.Items.Add(video.Name + ":" + video.Actors + ":" + video.Directors);
                                break;
                            }
                        }
                    }
                    if (multName != null)
                    {
                        DisplayStartNotificationDomainUp.Items.Add("User: " + sharedMultInfo[0] + " Multimedia: " + multName);
                    }
                }

                //SideMenuPanel.Visible = true;
                SideMenuPanel.Width = 0;
                PlayerMultPanel.Visible = true;
                SearchGeneralTopPanel.Visible = true;
                if (userInfo[3] == "standard")
                {
                    AddsPanel1.Visible = true;
                    AddsPanel2.Visible = true;
                }
                if(DisplayStartNotificationDomainUp.Items.Count != 0)
                {
                    SharedMultNotificationButton.Visible = true;
                }

                Profile profileGetter = OnProfilesChooseProfile_Click(profileProfileName, username, password);

                if (DisplayStartNotificationDomainUp.Items.Count != 0)
                {
                    SharedMultNotificationButton.Visible = true;
                }
                if (notification != 0)
                {
                    SharedMultNotificationButton.Visible = false;
                    DisplayStartChooseSharedMult.Visible = false;
                    DisplayStartNotificationDomainUp.Visible = false;

                    notification = 0;
                }
                DisplayStartProfileInfoPanel.Visible = false;
                PlayerMultPanel.Visible = false;
                DisplayStartPanel.BringToFront();
            }
            else
            {
                ProfilesInvalidCredentialTextBox.AppendText("ERROR [!]You have to choose a profile");
            }
        }
        private void ProfileDeletePorfileButto_Click(object sender, EventArgs e)
        {
            string profileName = ProfileDomainUp.Text;
            string username = UserLogInTextBox.Text;
            DeleteProfile_Did(username, profileName);
            DeleteProfile_Did(username, profileName);
            ProfilesInvalidCredentialTextBox.AppendText("Profile Deleted, FyBuZz Restarting...");
            Thread.Sleep(2500);
            Application.Restart();
        }


        private void ProfileCreateProfileButton_Click(object sender, EventArgs e)
        {
            CreateProfilePanel.BringToFront();
        }

        //ON EVENT

        public Profile OnProfilesChooseProfile_Click(string pName, string usr, string pass)
        {
            if (ProfilesChooseProfile_Clicked != null)
            {
                Profile choosenProfile = ProfilesChooseProfile_Clicked(this, new ProfileEventArgs() { ProfileNameText = pName, UsernameText = usr, PasswordText = pass });
                ProfilesInvalidCredentialTextBox.ResetText();
                ProfilesInvalidCredentialTextBox.AppendText("Entering FyBuZz with... " + choosenProfile.ProfileName);

                Thread.Sleep(2000);

                return choosenProfile;
            }
            else
            {
                return null;
            }
        }
        public void DeleteProfile_Did(string uName, string pName)
        {
            if (ProfileDeleted != null)
            {
                bool result = ProfileDeleted(this, new UserEventArgs() { UsernameText = uName, ProfilenameText = pName });
                if (result)
                {

                }
                else
                {
                    ProfilesInvalidCredentialTextBox.AppendText("ERROR[!] Couldn't delete Profile");
                }
            }
        }
        public List<string> OnProfilesChooseProfile_Click2(string pName, string usr, string pass)
        {
            if (ProfilesChooseProfile_Clicked2 != null)
            {
                List<string> choosenProfile = ProfilesChooseProfile_Clicked2(this, new ProfileEventArgs() { ProfileNameText = pName, UsernameText = usr, PasswordText = pass });
                if (choosenProfile != null)
                {
                    ProfilesInvalidCredentialTextBox.ResetText();
                    ProfilesInvalidCredentialTextBox.AppendText("Entering FyBuZz with... " + choosenProfile[0]);

                    Thread.Sleep(2000);
                    return choosenProfile;
                }
                else
                {
                    return null;
                }
                
            }
            else
            {
                return null;
            }
        }

        //GO BACK/CLOSE
        private void ProfileGoBack_Click(object sender, EventArgs e)
        {
            int cont = 0;
            if(ProfileDomainUp.SelectedIndex != -1)
            {
                foreach (object searched in ProfileDomainUp.Items)
                {
                    cont++;
                }

                for (int i = 0; i < cont; cont--)
                {
                    ProfileDomainUp.Items.Remove(cont - 1);
                }
            }
           
            WelcomePanel.BringToFront();
            UserLogInTextBox.Clear();
            PasswordLogInTextBox.Clear();
            LogInInvalidCredentialsTetxbox.Clear();
        }
        //-------------------------------------------------------------------------------------------




       
            //<<!CREATE PROFILE PANEL>>
        //-------------------------------------------------------------------------------------------

        private void CreateProfileCreateProfileButton_Click(object sender, EventArgs e)
        {
            string pName = CreateProfileProfileNameTextBox.Text;
            string pGender = CreateProfileProfileGenderDomainUp.Text;
            DateTime pBirth = CreateProfileProfileBirthdayTimePicker.Value;
            string pType = CreateProfileProfileTypeDomainUp.Text;
            string pEmail = EmailRegisterTextBox.Text;
            string username = UserLogInTextBox.Text;
            string psswd = PasswordLogInTextBox.Text;
            Image pPic = CreateProfilePic1.Image;
            if (CreateProfilePicCheckedListBox.SelectedIndex == 0) pPic = CreateProfilePic1.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 1) pPic = CreateProfilePic2.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 2) pPic = CreateProfilePic3.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 3) pPic = CreateProfilePic4.Image;
            OnCreateProfileCreateProfileButton_Click2(username, psswd, pName, pGender, pType, pEmail, pBirth, pPic);
        }
        private void CreateProfileCreateProfileButton_Click_1(object sender, EventArgs e)
        {
            string pName = CreateProfileProfileNameTextBox.Text;
            string pGender = CreateProfileProfileGenderDomainUp.Text;
            DateTime pBirth = CreateProfileProfileBirthdayTimePicker.Value;
            string pType = CreateProfileProfileTypeDomainUp.Text;
            string pEmail = EmailRegisterTextBox.Text;
            string username = UserLogInTextBox.Text;
            string psswd = PasswordLogInTextBox.Text;
            Image pPic = CreateProfilePic1.Image;
            if (CreateProfilePicCheckedListBox.SelectedIndex == 0) pPic = CreateProfilePic1.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 1) pPic = CreateProfilePic2.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 2) pPic = CreateProfilePic3.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 3) pPic = CreateProfilePic4.Image;
            OnCreateProfileCreateProfileButton_Click2(username, psswd, pName, pGender, pType, pEmail, pBirth, pPic);
        }

        //ONEVENT

        public void OnCreateProfileCreateProfileButton_Click2(string username, string pswd, string pName, string pGender, string pType, string pEmail, DateTime pBirth, Image pPic)
        {
            if (CreateProfileCreateProfileButton_Clicked != null)
            {
                string result = CreateProfileCreateProfileButton_Clicked(this, new ProfileEventArgs() { UsernameText = username, PasswordText = pswd, ProfileNameText = pName, EmailText = pEmail, GenderText = pGender, BirthdayText = pBirth, ProfileTypeText = pType, PicImage = pPic });
                if (result != null)
                {
                    ProfileDomainUp.Items.Add(result);
                    ProfilePanel.BringToFront();
                    CreateProfileProfileNameTextBox.Clear();
                }
                else
                {
                    ProfilePanel.BringToFront();
                    ProfilesInvalidCredentialTextBox.AppendText("Only premium Users can create profiles");
                }
            }
        }

        //TEXTBOX WATERMARK
        private void CreateProfileProfileNameTextBox_Enter(object sender, EventArgs e)
        {
            if(CreateProfileProfileNameTextBox.Text == "Andres")
            {
                CreateProfileProfileNameTextBox.Text = "";
                CreateProfileProfileNameTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateProfileProfileNameTextBox_Leave(object sender, EventArgs e)
        {
            if(CreateProfileProfileNameTextBox.Text == "")
            {
                CreateProfileProfileNameTextBox.Text = "Andres";
                CreateProfileProfileNameTextBox.ForeColor = Color.Gold;
            }
        }

        //GO BACK/CLOSE
        private void CreateProfileGoBackButton_Click(object sender, EventArgs e)
        {
            ProfilePanel.BringToFront();
        }
        private void CreateProfileGoBackButton_Click_1(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------




        /*---------------------------------------------------!M----------------------------------------------------- */



        //<<!DISPLAY START PANEL>>
        //-------------------------------------------------------------------------------------------

        private void DisplayStartSearchButton_Click(object sender, EventArgs e)
        {
            SearchPlayingLabel.Clear();
            SearchPanel.BringToFront();
        }

        private void DisplayStartSettingsButton_Click(object sender, EventArgs e)
        {
            
        }


        private void DisplayStartAdminMenuButton_Click(object sender, EventArgs e)
        {
            List<string> listUser = OnLogInLogInButton_Clicked2(UserLogInTextBox.Text);
            List<User> userDatabase = OnSearchUserButton_Click();
            if(listUser[3] == "admin")
            {
                foreach(User user in userDatabase)
                {
                    if(user.Username != null)
                    {
                        AdminMenuAllUsers.Items.Add(user.Username);
                    }
                }
                AdminMenuPanel.BringToFront();
            }

        }

        //<<PANEL DISPLAY PLAYLISTS>>

        private void DisplayStartDisplayPlaylistButton_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            //<<Followed>>
            if (profile.FollowedPlayList.Count() == 1)
            {
                DisplayPlaylistsFollowedPlaylist1.Visible = true;
                DisplayPlaylistsFollowedPlaylist1.Image = CreateProfilePic1.Image;
            }
            else if (profile.FollowedPlayList.Count() == 2)
            {
                DisplayPlaylistsFollowedPlaylist1.Visible = true;
                DisplayPlaylistsFollowedPlaylist2.Visible = true;
            }
            else if (profile.FollowedPlayList.Count() == 3)
            {
                DisplayPlaylistsFollowedPlaylist1.Visible = true;
                DisplayPlaylistsFollowedPlaylist2.Visible = true;
                DisplayPlaylistsFollowedPlaylist3.Visible = true;
            }
            else if (profile.FollowedPlayList.Count() > 3)
            {
                DisplayPlaylistsFollowedPlaylist1.Visible = true;
                DisplayPlaylistsFollowedPlaylist2.Visible = true;
                DisplayPlaylistsFollowedPlaylist3.Visible = true;
                DisplayPlaylistsMoreFollowedPlaylistButton.Visible = true;
            }
            //<<Created>>
            
            DisplayPlaylistCreatedPlaylistImage1.Visible = true;
            DisplayPlaylistCreatedPlaylistImage2.Visible = true;
            
            DisplayPlaylistPanel.BringToFront();
        }

        private void DisplayStartShowAddButton_Click(object sender, EventArgs e)
        {
            AddShowPanel.BringToFront();
        }

        //ONEVENT


        //GO BACK/CLOSE
        private void DisplayStartCloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DisplayStartLogOutButton_Click(object sender, EventArgs e)
        {
            
        }

        private void DisplayStartLogOutProfileButton_Click(object sender, EventArgs e)
        {
            
        }
        private void DisplayStartProfileLogOutButton_Click(object sender, EventArgs e)
        {
            AddsPanel1.Visible = false;
            AddsPanel2.Visible = false;
            SideMenuPanel.Visible = false;
            PlayerMultPanel.Visible = false;

            AccountSettingsUsernameTextBox.Clear();
            AccountSettingsPasswordTextBox.Clear();
            AccountSettingsAccountTypeTextBox.Clear();
            AccountSettingsEmailTextBox.Clear();
            AccountSettingsFollowersTextBox.Clear();
            AccountSettingsFollowingTextBox.Clear();

            ProfileSettingsNameTextBox.Clear();
            ProfileSettingsProfileTypeTextBox.Clear();
            ProfileSettingsGenderTextBox.Clear();
            ProfileSettingsBirthdayTextBox.Clear();
            //ProfileSettingsProfilePicImageBox.Image.Dispose();

            ProfilesInvalidCredentialTextBox.Clear();
            ProfilePanel.BringToFront();
        }

        private void DisplayStartLogOutFybuzzButton_Click(object sender, EventArgs e)
        {
            int cont = 0;

            if (ProfileDomainUp.SelectedIndex != -1)
            {
                foreach (object searched in ProfileDomainUp.Items)
                {
                    cont++;
                }
                for (int i = 0; i < cont; cont--)
                {
                    ProfileDomainUp.Items.RemoveAt(cont - 1);
                }
            }
            WelcomePanel.BringToFront();
            UserLogInTextBox.ResetText();
            PasswordLogInTextBox.ResetText();
            LogInInvalidCredentialsTetxbox.Clear();
            ProfilesWelcomeTextBox.Clear();
            ProfilesInvalidCredentialTextBox.Clear();
            ProfileDomainUp.ResetText();
            ProfilesInvalidCredentialTextBox.Clear();

            AddsPanel1.Visible = false;
            AddsPanel2.Visible = false;
            SideMenuPanel.Visible = false;
            PlayerMultPanel.Visible = false;
        }

        private void DisplayPlaylistGoBackButton_Click(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------





        /*---------------------------------------------------!SCH----------------------------------------------------- */


        
            //<<!SEARCH PANEL>>
        //-------------------------------------------------------------------------------------------

        private void SearchSearchButton_Click(object sender, EventArgs e)
        {
            
        }
        private void SearchSearchButton_Click_1(object sender, EventArgs e)
        {
            
        }
        private void SearchSearchButton_Click_2(object sender, EventArgs e)
        {
            //SearchSearchResultsDomainUp.ReadOnly = true;
            SearchSearchResultsDomainUp.Text = "Searched Results:";

            SearchDisplayMoreMultimediaInfo.Clear();
            SearchDisplayMoreMultimediaInfo.Visible = false;

            string search = SearchSearchTextBox.Text; //Bad Bunny and Trap and ... and ...

            bool filtersOn = SearchFiltersOnCheckBox.Checked;
            List<List<string>> allSongInfo = ReturnAllSongsInfo();
            List<List<string>> allVideosInfo = ReturnAllVideosInfo();

            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            List<User> userDataBase = new List<User>();
            userDataBase = OnSearchUserButton_Click();
            List<Video> videoDataBase = new List<Video>();
            videoDataBase = OnSearchVideoButton_Click();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();
            List<PlayList> privatePls = new List<PlayList>();
            privatePls = GetPrivPlaylist();


            int cont1 = 0;

            if (SearchSearchResultsDomainUp.Items.Count != 0)
            {
                foreach (object searched in SearchSearchResultsDomainUp.Items)
                {
                    cont1++;
                }
                for (int i = 0; i < cont1; cont1--)
                {
                    SearchSearchResultsDomainUp.Items.Clear();
                }
            }


            if (!filtersOn)
            {
                string auxSearch = "";
                string auxS = "";
                int lengthRemove = 0;

                foreach (Song song in songDataBase)
                {
                    foreach (string s in song.InfoSong())
                    {
                        int aux = 0;
                        auxSearch = search;
                        auxS = s;
                        lengthRemove = 0;

                        if (s.Length > auxSearch.Length)
                        {
                            lengthRemove = auxS.Length - auxSearch.Length;
                            auxS = auxS.Remove(auxSearch.Length, lengthRemove);
                        }
                        else if (s.Length < auxSearch.Length)
                        {
                            lengthRemove = auxSearch.Length - auxS.Length;
                            auxSearch = auxSearch.Remove(auxS.Length, lengthRemove);
                        }

                        auxSearch = auxSearch.ToLower();
                        auxS = auxS.ToLower();

                        for (int j = auxS.Length - 1; j > 0; j--)
                        {
                            if (auxSearch == auxS)
                            {
                                SearchSearchResultsDomainUp.Visible = true;
                                if(SearchSearchResultsDomainUp.Items.Contains(song.SearchedInfoSong()) == false)
                                {
                                    SearchSearchResultsDomainUp.Items.Add(song.SearchedInfoSong());
                                    SearchSelectMultButton.Visible = true;
                                    SearchViewUserButton.Visible = true;
                                }
                                
                                break;
                            }
                            auxSearch = auxSearch.Remove(j);
                            auxS = auxS.Remove(j);
                        }
                        if (aux != 0)
                        {
                            break;
                        }
                    }
                }

                foreach (Video video in videoDataBase)
                {
                    foreach (string s in video.InfoVideo())
                    {
                        int aux = 0;
                        auxSearch = search;
                        auxS = s;
                        lengthRemove = 0;

                        if (s.Length > auxSearch.Length)
                        {
                            lengthRemove = auxS.Length - auxSearch.Length;
                            auxS = auxS.Remove(auxSearch.Length, lengthRemove);
                        }
                        else if (s.Length < auxSearch.Length)
                        {
                            lengthRemove = auxSearch.Length - auxS.Length;
                            auxSearch = auxSearch.Remove(auxS.Length, lengthRemove);
                        }

                        auxSearch = auxSearch.ToLower();
                        auxS = auxS.ToLower();

                        for (int j = auxS.Length - 1; j > 0; j--)
                        {
                            if (auxSearch == auxS)
                            {
                                SearchSearchResultsDomainUp.Visible = true;
                                if(SearchSearchResultsDomainUp.Items.Contains(video.SearchedInfoVideo())== false)
                                {
                                    SearchSearchResultsDomainUp.Items.Add(video.SearchedInfoVideo());
                                    SearchSelectMultButton.Visible = true;
                                    SearchViewUserButton.Visible = true;
                                }
                                
                                break;
                            }
                            auxSearch = auxSearch.Remove(j);
                            auxS = auxS.Remove(j);
                        }
                        if (aux != 0)
                        {
                            break;
                        }
                    }
                }

                foreach (PlayList playlist in playlistDataBase)
                {
                    foreach (string s in playlist.InfoPlayList())
                    {
                        int indicador = 0;
                        auxSearch = search;
                        auxS = s;
                        lengthRemove = 0;

                        if (s.Length > auxSearch.Length)
                        {
                            lengthRemove = auxS.Length - auxSearch.Length;
                            auxS = auxS.Remove(auxSearch.Length, lengthRemove);
                        }
                        else if (s.Length < auxSearch.Length)
                        {
                            lengthRemove = auxSearch.Length - auxS.Length;
                            auxSearch = auxSearch.Remove(auxS.Length, lengthRemove);
                        }

                        auxSearch = auxSearch.ToLower();
                        auxS = auxS.ToLower();

                        for (int j = auxS.Length - 1; j > 0; j--)
                        {

                            if (auxSearch == auxS)
                            {
                                SearchSearchResultsDomainUp.Visible = true;
                                if(SearchSearchResultsDomainUp.Items.Contains(playlist.DisplayInfoPlayList()) == false)
                                {
                                    SearchSearchResultsDomainUp.Items.Add(playlist.DisplayInfoPlayList());
                                    SearchSelectMultButton.Visible = true;
                                    SearchViewUserButton.Visible = true;
                                }                                
                                indicador++;
                                break;
                            }
                            auxSearch = auxSearch.Remove(j);
                            auxS = auxS.Remove(j);
                        }
                        if (indicador != 0)
                        {
                            break;
                        }

                    }
                }
                foreach (PlayList privatePl in privatePls)
                {
                    if (privatePl.NamePlayList != "")
                    {
                        if (ProfileDomainUp.Text.Contains(privatePl.ProfileCreator) && UserLogInTextBox.Text.Contains(privatePl.Creator))
                        {
                            SearchSearchResultsDomainUp.Visible = true;
                            if (SearchSearchResultsDomainUp.Items.Contains(privatePl.DisplayInfoPlayList()) == false)
                            {
                                SearchSearchResultsDomainUp.Items.Add(privatePl.DisplayInfoPlayList() + " (private)");
                                SearchSelectMultButton.Visible = true;
                                SearchViewUserButton.Visible = true;
                            }
                        }
                    }
                }
                foreach (User user in userDataBase)
                {
                    if (user.Username != null)
                    {
                        foreach (string s in user.InfoUser())
                        {
                            int aux = 0;
                            auxSearch = search;
                            auxS = s;
                            lengthRemove = 0;

                            if (s.Length > auxSearch.Length)
                            {
                                lengthRemove = auxS.Length - auxSearch.Length;
                                auxS = auxS.Remove(auxSearch.Length, lengthRemove);
                            }
                            else if (s.Length < auxSearch.Length)
                            {
                                lengthRemove = auxSearch.Length - auxS.Length;
                                auxSearch = auxSearch.Remove(auxS.Length, lengthRemove);
                            }

                            auxSearch = auxSearch.ToLower();
                            auxS = auxS.ToLower();

                            for (int j = auxS.Length - 1; j > 0; j--)
                            {
                                if (auxSearch == auxS)
                                {
                                    SearchSearchResultsDomainUp.Visible = true;
                                    if (SearchSearchResultsDomainUp.Items.Contains(user.SearchedInfoUser()) == false)
                                    {
                                        SearchSearchResultsDomainUp.Items.Add("User: " + user.SearchedInfoUser());
                                        SearchSelectMultButton.Visible = true;
                                        SearchViewUserButton.Visible = true;
                                    }
                                    break;
                                }
                                auxSearch = auxSearch.Remove(j);
                                auxS = auxS.Remove(j);
                            }
                            if (aux != 0)
                            {
                                break;
                            }
                        }
                    }


                }

            }
           
            if (SearchSearchResultsDomainUp.Items.Count == 0)
            {
                SearchInvalidCredentialsTextBox.AppendText("ERROR[!] Nothing found.");
                Thread.Sleep(1000);
                SearchInvalidCredentialsTextBox.Clear();
            }

            SearchFiltersOnCheckBox.CheckState = CheckState.Unchecked;
            SearchAndOrCheckBox.ClearSelected();
            AllFiltersCheckbox.ClearSelected();
            //SearchAndOrCheckBox.Visible = false;
            //AllFiltersCheckbox.Visible = false;

            PlaySongChoosePlsDomainUp.Visible = false;
            PlaySongChoosePlsDomainUp.ResetText();
            PlaySongChoosePlsDomainUp.ReadOnly = true;
            PlaySongMessageTextBox.Clear();
        }

        private void SearchSelectMultButton_Click(object sender, EventArgs e)
        {
            

        }
        private void SearchSelectMultButton_Click_1(object sender, EventArgs e)
        {
            wmpVideo.Ctlcontrols.stop();
            windowsMediaPlayer.Ctlcontrols.stop();
            soundPlayer.Stop();
            PlayerMultPanel.Visible = true;
            TimerWav.Stop();
            ticks = 0;
            PlayerPlayingLabel.Clear();
            SearchPlayingLabel.Clear();
            PlaySongSongPlaying.Text = String.Empty;
            PlaySongSongPlaying.Clear();
            PlayVideoVideoPlaying.Clear();
            PlaySongChoosePlsButton.Visible = false;
            SearchDisplayMoreMultimediaInfo.Clear();
            SearchDisplayMoreMultimediaInfo.Visible = false;
            soundPlayer = new SoundPlayer();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            List<PlayList> playListsDataBase = new List<PlayList>();
            playListsDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();
            List<Video> videoDataBase = OnSearchVideoButton_Click();
            List<PlayList> privatePls = new List<PlayList>();
            privatePls = GetPrivPlaylist();
            List<string> infoProfile = OnProfilesChooseProfile_Click2(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);


            string multimediaType = SearchSearchResultsDomainUp.Text;

            if (multimediaType.Contains("Song:") == true && multimediaType.Contains("Artist:") == true)
            {
                
                string[] MultimediaType = SearchSearchResultsDomainUp.Text.Split(':');
                List<string> songMVCInfo = GetSongButton(MultimediaType[1], MultimediaType[3]);
                soundPlayer.Stop();
                windowsMediaPlayer.Ctlcontrols.pause();
                foreach (Song song in songDataBase)
                {
                    int badWordsCount = 0;
                    if (songMVCInfo[5] != null)
                    {
                        string songLyrics = File.ReadAllText(songMVCInfo[5]);
                        foreach (string badWord in badWords)
                        {
                            if (songLyrics.Contains(badWord) == true)
                            {
                                badWordsCount++;
                            }
                        }
                    }
                    if (int.Parse(infoProfile[3]) < 16 && badWordsCount != 0)
                    {
                        SearchInvalidCredentialsTextBox.AppendText("Song with explicit content only 16+");
                        Thread.Sleep(1000);
                        SearchInvalidCredentialsTextBox.Clear();
                    }
                    else
                    {
                        isInPlaylist = false;

                        if (songMVCInfo[6].Contains(".mp3") == true)
                        {
                            string result = SearchSearchResultsDomainUp.Text;
                            string songInfo = song.SearchedInfoSong();
                            if (result == song.SearchedInfoSong())
                            {
                                if (song.ImageFile != null)
                                {
                                    PlaySongImageBoxImage.Image = System.Drawing.Image.FromFile(song.ImageFile);
                                }
                                else
                                {
                                    PlaySongImageBoxImage.Image = System.Drawing.Image.FromFile("Logo (1).jpg");
                                }
                                PlaySongSongPlaying.AppendText(song.Name + ":" + song.Artist + ":" + song.Format);
                                AddingSearchedMult(ProfileDomainUp.Text, song.SongFile, null);
                                Thread.Sleep(2000);
                                PlayerPlayingLabel.Clear();
                                SearchPlayingLabel.Clear();
                                PlaySongProgressBar.Value = 0;
                                PlaySongTimerTextBox.ResetText();

                                windowsMediaPlayer.URL = song.SongFile;
                                formatProgressBar = song.Format;
                                windowsMediaPlayer.Ctlcontrols.play();

                                DurationTimer.Interval = 1000;
                                PlaySongProgressBar.Maximum = (int)(song.Duration * 60);
                                SearchProgressBar.Maximum = (int)(song.Duration * 60);
                                PlayPlaylistProgressBarBox.Maximum = (int)(song.Duration * 60);
                                PlaySongPanel.BringToFront();
                                PlayerPlayingLabel.AppendText("Song playing: " + song.Name + "." + song.Format);
                                SearchPlayingLabel.AppendText("Song playing: " + song.Name + "." + song.Format);
                                DurationTimer.Start();
                                break;
                            }
                        }
                        else if (song.Format == ".wav")
                        {
                            string result = SearchSearchResultsDomainUp.Text;
                            if (result == song.SearchedInfoSong())
                            {
                                if (song.ImageFile != null)
                                {
                                    PlaySongImageBoxImage.Image = System.Drawing.Image.FromFile(song.ImageFile);
                                }
                                else
                                {
                                    PlaySongImageBoxImage.Image = System.Drawing.Image.FromFile("Logo (1).jpg");
                                }
                                PlaySongSongPlaying.AppendText(song.Name + ":" + song.Artist + ":" + song.Format);
                                AddingSearchedMult(ProfileDomainUp.Text, song.SongFile, null);
                                Thread.Sleep(2000);
                                PlayerPlayingLabel.Clear();
                                SearchPlayingLabel.Clear();
                                PlaySongProgressBar.Value = 0;
                                PlaySongTimerTextBox.ResetText();
                                formatProgressBar = song.Format;
                                durationWav = (song.Duration * 60);
                                soundPlayer.SoundLocation = song.SongFile;
                                soundPlayer.Play();
                                TimerWav.Start();
                                PlayerMultPanelMtrackPB.Maximum = (int)durationWav;
                                PlaySongPanel.BringToFront();
                                PlayerPlayingLabel.AppendText("Song playing:" + song.Name + ":" + song.Artist + ":" + song.Format);
                                SearchPlayingLabel.AppendText("Song playing:" + song.Name + ":" + song.Artist + ":" + song.Format);
                                DurationTimer.Start();
                                break;
                            }
                        }
                    }
                }
            }
            else if (multimediaType.Contains("PlayList Name:") == true)
            {
                
                isInPlaylist = true;
                string result = SearchSearchResultsDomainUp.Text;
                string plName = "";
                foreach (PlayList playList in playListsDataBase)
                {

                    string ex = playList.DisplayInfoPlayList();
                    if (result == ex)
                    {
                        if (playList.Image != null)
                        {
                            PlayPlaylistImageBox.Image = System.Drawing.Image.FromFile(playList.Image);
                        }
                        else
                        {
                            PlayPlaylistImageBox.Image = System.Drawing.Image.FromFile("Logo (1).jpg");
                        }

                        plName = playList.NamePlayList;
                        if (playList.Format == ".mp3" || playList.Format == ".wav")
                        {
                            foreach (Song song in playList.Songs)
                            {
                                PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                                PlayPlaylistIsPrivate.Clear();
                            }
                        }
                        else if (playList.Format == ".mp4" || playList.Format == ".mov" || playList.Format == ".avi")
                        {
                            foreach (Video video in playList.Videos)
                            {
                                PlayPlaylistShowMultimedia.Items.Add(video.SearchedInfoVideo());
                                PlayPlaylistIsPrivate.Clear();
                            }
                        }
                    }
                }
                if (PlayPlaylistShowMultimedia.Items.Count == 0)
                {
                    foreach (PlayList privatePl in privatePls)
                    {

                        string ex = privatePl.DisplayInfoPlayList();
                        if (privatePl.NamePlayList != "" && result.Contains(ex))
                        {
                            if (privatePl.Image != null)
                            {
                                PlayPlaylistImageBox.Image = System.Drawing.Image.FromFile(privatePl.Image);
                            }
                            else
                            {
                                PlayPlaylistImageBox.Image = System.Drawing.Image.FromFile("Logo (1).jpg");
                            }

                            plName = privatePl.NamePlayList;
                            if (privatePl.Format == ".mp3" || privatePl.Format == ".wav")
                            {
                                foreach (Song song in privatePl.Songs)
                                {
                                    PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                                    PlayPlaylistIsPrivate.AppendText("private");
                                }
                            }
                            else if (privatePl.Format == ".mp4" || privatePl.Format == ".mov" || privatePl.Format == ".avi")
                            {
                                foreach (Video video in privatePl.Videos)
                                {
                                    PlayPlaylistShowMultimedia.Items.Add(video.SearchedInfoVideo());
                                    PlayPlaylistIsPrivate.AppendText("private");
                                }
                            }
                        }
                    }
                }
                PlayPlaylistLabel.Text = "Playlist: " + plName;
                PlayPlaylistPanel.BringToFront();
            }

            else if (multimediaType.Contains("Video:") == true)
            {
                PlayerMultPanel.Visible = false;
                PlayVideoVideoPlaying.Clear();
                string[] MultimediaType = SearchSearchResultsDomainUp.Text.Split(':');
                List<string> videoMVCInfo = GetVideoButton(MultimediaType[1], MultimediaType[3], MultimediaType[5]);
                if (int.Parse(videoMVCInfo[4]) > int.Parse(infoProfile[3]))
                {
                    SearchInvalidCredentialsTextBox.AppendText("Video has age restriction");
                    Thread.Sleep(1000);
                    SearchInvalidCredentialsTextBox.Clear();

                }
                else
                {
                    foreach (Video video in videoDataBase)
                    {
                        string result = SearchSearchResultsDomainUp.Text;
                        if (result == video.SearchedInfoVideo())
                        {
                            if (video.Image != null)
                            {
                                PlayVideoVideoImageBox.Image = System.Drawing.Image.FromFile(video.Image);
                            }
                            else
                            {
                                PlayVideoVideoImageBox.Image = System.Drawing.Image.FromFile("Logo (1).jpg");
                            }
                            PlayVideoVideoPlaying.AppendText(video.Name + ":" + video.Actors + ":" + video.Directors + ":" + video.Format);
                            AddingSearchedMult(ProfileDomainUp.Text, null, video.FileName);
                            PlayVideoPanel.BringToFront();
                            wmpVideo.URL = video.FileName;
                            wmpVideo.Ctlcontrols.play();
                        }
                    }
                }
                int cont1 = 0;
                if (PlayVideoSelectPlDomainUp.SelectedIndex != -1)
                {
                    foreach (object searched in PlayVideoSelectPlDomainUp.Items)
                    {
                        cont1++;
                    }
                    for (int i = 0; i < cont1; cont1--)
                    {
                        PlayVideoSelectPlDomainUp.Items.RemoveAt(cont1 - 1);
                    }
                }
                PlayVideoSelectPlDomainUp.Visible = false;
                PlayVideoSelectPlButton.Visible = false;
            }
            PlaySongRateNumDomainUp.Refresh();
            PlaySongRateMessageTextBox.Clear();
            SearchPlayingLabel.Clear();
        }
        private void AllFilterSearch_Click(object sender, EventArgs e)
        {
            //SearchSearchResultsDomainUp. = true;
            SearchSearchResultsDomainUp.Text = "Searched Results:";

            SearchDisplayMoreMultimediaInfo.Clear();
            SearchDisplayMoreMultimediaInfo.Visible = false;

            string search = SearchSearchTextBox.Text; //Bad Bunny and Trap and ... and ...

            bool filtersOn = SearchFiltersOnCheckBox.Checked;
            List<List<string>> allSongInfo = ReturnAllSongsInfo();
            List<List<string>> allVideosInfo = ReturnAllVideosInfo();


            int cont1 = 0;
            if (SearchSearchResultsDomainUp.Items.Count != 0)
            {
                foreach (object searched in SearchSearchResultsDomainUp.Items)
                {
                    cont1++;
                }
                for (int i = 0; i < cont1; cont1--)
                {
                    SearchSearchResultsDomainUp.Items.Clear();
                }
            }

            List<int> allChosenFilters = new List<int>();

            foreach (object filter in AllFiltersCheckbox.CheckedIndices)
            {
                allChosenFilters.Add((int)filter);
            }

            foreach (List<string> songInfo in allSongInfo)
            {
                int contS = 0;
                for (int n = 0; n < allChosenFilters.Count(); n++)
                {
                    if (allChosenFilters[n] <= 7)
                    {
                        int newIndex = allChosenFilters[n];
                        if (songInfo[newIndex].Contains(search) == true)
                        {
                            contS++;
                        }

                    }

                }
                if (contS >= allChosenFilters.Count())
                {
                    SearchSearchResultsDomainUp.Visible = true;
                    if (SearchSearchResultsDomainUp.Items.Contains("Song: " + songInfo[0] + ": Artist: " + songInfo[1]) == false)
                    {
                        SearchSearchResultsDomainUp.Items.Add("Song: " + songInfo[0] + ": Artist: " + songInfo[1]);
                        SearchSelectMultButton.Visible = true;
                        SearchViewUserButton.Visible = true;
                    }
                }

            }
            foreach (List<string> videoInfo in allVideosInfo)
            {
                int contS = 0;
                for (int n = 0; n < allChosenFilters.Count(); n++)
                {
                    if (allChosenFilters[n] >= 7)
                    {
                        int newIndex = allChosenFilters[n] - 7;
                        if (videoInfo[newIndex].Contains(search) == true)
                        {
                            contS++;
                        }
                    }

                }
                if (contS >= allChosenFilters.Count())
                {
                    SearchSearchResultsDomainUp.Visible = true;
                    if (SearchSearchResultsDomainUp.Items.Contains("Video: " + videoInfo[0] + ": Actors: " + videoInfo[1] + ": Directors:" + videoInfo[3]) == false)
                    {
                        SearchSearchResultsDomainUp.Items.Add("Video: " + videoInfo[0] + ": Actors: " + videoInfo[1] + ": Directors:" + videoInfo[3]);
                        SearchSelectMultButton.Visible = true;
                        SearchViewUserButton.Visible = true;
                    }
                }

            }
            if (SearchSearchResultsDomainUp.Items.Count == 0)
            {
                SearchInvalidCredentialsTextBox.AppendText("ERROR[!] Nothing found.");
                Thread.Sleep(1000);
                SearchInvalidCredentialsTextBox.Clear();
            }
            SearchFiltersOnCheckBox.CheckState = CheckState.Unchecked;
            SearchAndOrCheckBox.ClearSelected();
            AllFiltersCheckbox.ClearSelected();
            //SearchAndOrCheckBox.Visible = false;
            //AllFiltersCheckbox.Visible = false;

            PlaySongChoosePlsDomainUp.Visible = false;
            PlaySongChoosePlsDomainUp.ResetText();
            PlaySongChoosePlsDomainUp.ReadOnly = true;
            PlaySongMessageTextBox.Clear();
            

        }

        private void OrFiltersSearch_Click(object sender, EventArgs e)
        {
            SearchSearchResultsDomainUp.Text = "Searched Results:";

            SearchDisplayMoreMultimediaInfo.Clear();
            SearchDisplayMoreMultimediaInfo.Visible = false;

            string search = SearchSearchTextBox.Text; //Bad Bunny and Trap and ... and ...

            bool filtersOn = SearchFiltersOnCheckBox.Checked;
            List<List<string>> allSongInfo = ReturnAllSongsInfo();
            List<List<string>> allVideosInfo = ReturnAllVideosInfo();

            int cont1 = 0;
            if (SearchSearchResultsDomainUp.Items.Count != 0)
            {
                foreach (object searched in SearchSearchResultsDomainUp.Items)
                {
                    cont1++;
                }
                for (int i = 0; i < cont1; cont1--)
                {
                    SearchSearchResultsDomainUp.Items.Clear();
                }
            }
            //SearchAndOrCheckBox.Visible = true;
            //SearchFiltersCheBox.Visible = true;
            string logic = null;
            List<int> allChosenFilters = new List<int>();
            foreach (object andOr in SearchAndOrCheckBox.CheckedItems)
            {
                logic = andOr.ToString();
            }
            foreach (object filter in AllFiltersCheckbox.CheckedIndices)
            {
                allChosenFilters.Add((int)filter);
            }
            foreach (List<string> songInfo in allSongInfo)
            {
                int contS = 0;
                for (int n = 0; n < allChosenFilters.Count(); n++)
                {
                    if (allChosenFilters[n] <= 7)
                    {
                        int newIndex = allChosenFilters[n];
                        if (songInfo[newIndex].Contains(search) == true)
                        {
                            contS++;
                        }

                    }

                }
                if (contS > 0 )
                {
                    SearchSearchResultsDomainUp.Visible = true;
                    SearchSearchResultsDomainUp.Items.Add("Song: " + songInfo[0] + ": Artist: " + songInfo[1]);
                    SearchSelectMultButton.Visible = true;
                    SearchViewUserButton.Visible = true;
                }
            }

            foreach (List<string> videoInfo in allVideosInfo)
            {
                int contS = 0;
                for (int n = 0; n < allChosenFilters.Count(); n++)
                {
                    if (allChosenFilters[n] >= 7)
                    {
                        int newIndex = allChosenFilters[n] - 7;
                        if (videoInfo[newIndex].Contains(search) == true)
                        {
                            contS++;
                        }
                    }

                }
                if (contS != 0)
                {
                    SearchSearchResultsDomainUp.Visible = true;
                    SearchSearchResultsDomainUp.Items.Add("Video: " + videoInfo[0] + ": Actors: " + videoInfo[1] + ": Directors:" + videoInfo[3]);
                    SearchSelectMultButton.Visible = true;
                    SearchViewUserButton.Visible = true;
                }

            }
            if (SearchSearchResultsDomainUp.Items.Count == 0)
            {
                SearchInvalidCredentialsTextBox.AppendText("ERROR[!] Nothing found.");
                Thread.Sleep(1000);
                SearchInvalidCredentialsTextBox.Clear();
            }
            SearchFiltersOnCheckBox.CheckState = CheckState.Unchecked;
            SearchAndOrCheckBox.ClearSelected();
            AllFiltersCheckbox.ClearSelected();
            //SearchAndOrCheckBox.Visible = false;
            //AllFiltersCheckbox.Visible = false;

            PlaySongChoosePlsDomainUp.Visible = false;
            PlaySongChoosePlsDomainUp.ResetText();
            PlaySongChoosePlsDomainUp.ReadOnly = true;
            PlaySongMessageTextBox.Clear();

        }


        private void SearchFiltersOnCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SearchAndOrCheckBox.ClearSelected();
            SearchFiltersCheBox.ClearSelected();
            SearchFiltersCheBox.Visible = true;
            SearchAndOrCheckBox.Visible = true;
        }

        private void SearchViewUserButton_Click(object sender, EventArgs e)
        {
            

        }
        private void SearchViewUserButton_Click_1(object sender, EventArgs e)
        {
            PlayerMultPanel.Visible = false;
            string[] searcheduser = SearchSearchResultsDomainUp.Text.Split(':');
            List<string> userGetter = OnLogInLogInButton_Clicked2(searcheduser[2]);
            if (SearchSearchResultsDomainUp.Text.Contains("User: "))
            {
                if (userGetter[8] != "True")
                {
                    SearUserName.Visible = true;
                    SearcUserEmailTextBox.Visible = true;
                    SearchUserFollowers.Visible = true;
                    SearchUserFollowing.Visible = true;

                    SearUserName.AppendText(userGetter[0]);
                    SearcUserEmailTextBox.AppendText(userGetter[2]);
                    SearchUserFollowers.AppendText(userGetter[4]);
                    SearchUserFollowing.AppendText(userGetter[5]);
                    SearcUserPanel.BringToFront();
                }
                else
                {
                    SearUserName.Visible = true;
                    SearcUserEmailTextBox.Visible = true;
                    SearUserName.AppendText(userGetter[0]);
                    SearcUserEmailTextBox.AppendText("This user is private");
                    SearcUserPanel.BringToFront();
                }
                SearcUserPanel.BringToFront();
            }
            else
            {
                SearchInvalidCredentialsTextBox.AppendText("ERROR [!] That is not a User");
                Thread.Sleep(2000);
                SearchInvalidCredentialsTextBox.Clear();
            }
            if (SearchSearchResultsDomainUp.Items.Count == 0)
            {
                SearchInvalidCredentialsTextBox.AppendText("ERROR[!] Nothing found.");
                Thread.Sleep(1000);
                SearchInvalidCredentialsTextBox.Clear();
            }
            SearchFiltersOnCheckBox.CheckState = CheckState.Unchecked;
            SearchAndOrCheckBox.ClearSelected();
            AllFiltersCheckbox.ClearSelected();
            //SearchAndOrCheckBox.Visible = false;
            //AllFiltersCheckbox.Visible = false;

            PlaySongChoosePlsDomainUp.Visible = false;
            PlaySongChoosePlsDomainUp.ResetText();
            PlaySongChoosePlsDomainUp.ReadOnly = true;
            PlaySongMessageTextBox.Clear();
        }

        //!SEARCH PLAY PANEL

        private void SearchPlayButton_Click(object sender, EventArgs e)
        {
            soundPlayer.Play();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            foreach (Song song in songDataBase)
            {
                if (song.Format == ".mp3")
                {
                    windowsMediaPlayer.Ctlcontrols.play();
                    DurationTimer.Start();
                    break;
                }
                else if (song.Format == ".wav")
                {

                    DurationTimer.Start();
                }
            }
        }


        private void SearchPauseBotton_Click(object sender, EventArgs e)
        {
            soundPlayer.Stop();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            foreach (Song song in songDataBase)
            {
                if (song.Format == ".mp3")
                {
                    windowsMediaPlayer.Ctlcontrols.pause();
                    DurationTimer.Stop();
                    break;
                }
                else if (song.Format == ".wav")
                {
                    soundPlayer.Stop();
                    DurationTimer.Stop();
                    break;
                }
            }
        }

        private void SearchPlayerToMultButton_Click(object sender, EventArgs e)
        {
            string mult = SearchPlayingLabel.Text;
            if (mult.Contains("Song") == true)
            {
                PlaySongPanel.BringToFront();
            }
            else if ((mult.Contains("Playlist") == true && mult.Contains(".mp3") == true) || (mult.Contains("Playlist") == true && mult.Contains(".wav") == true))
            {
                PlayPlaylistPanel.BringToFront();
            }
        }


        private void DurationTimer_Tick(object sender, EventArgs e)
        {
            PlaySongProgressBar.Increment(1);
            PlayPlaylistProgressBarBox.Increment(1);
            SearchProgressBar.Increment(1);
            PlaySongTimerTextBox.Text = PlaySongProgressBar.Value.ToString();
            PlayPlaylistTimerBox.Text = PlayPlaylistProgressBarBox.Value.ToString();
            SearchTimerTextBox.Text = PlaySongProgressBar.Value.ToString();
            if (SearchProgressBar.Value == SearchProgressBar.Maximum)
            {

            }

        }


        //TEXTBOX WATERMARK

        private void SearchSearchTextBox_Enter(object sender, EventArgs e)
        {
            if(SearchSearchTextBox.Text == "Search Songs,Video, Playlists or Users")
            {
                SearchSearchTextBox.Text = "";
                SearchSearchTextBox.ForeColor = Color.Gold;
            }
        }

        private void SearchSearchTextBox_Leave(object sender, EventArgs e)
        {
            if (SearchSearchTextBox.Text == "")
            {
                SearchSearchTextBox.Text = "Search Songs,Video, Playlists or Users";
                SearchSearchTextBox.ForeColor = Color.DimGray;
            }
        }

        //GO BACK/CLOSE

        private void SearchGoBackButton_Click(object sender, EventArgs e)
        {
            
        }
        private void SearchGoBackButton_Click_1(object sender, EventArgs e)
        {
            windowsMediaPlayer.Ctlcontrols.stop();
            soundPlayer.Stop();

            SearchDisplayMoreMultimediaInfo.Clear();
            SearchDisplayMoreMultimediaInfo.Visible = false;
            SearchSearchTextBox.Text = "Search Songs,Video, Playlists or Users";
            SearchSearchResultsDomainUp.Visible = false;
            //SearchSearchResultsDomainUp.Items.Clear();
            SearchSearchResultsDomainUp.Text = "Searched Results:";
            DisplayStartPanel.BringToFront();
            if (windowsMediaPlayer.URL != null)
            {
                PlayerPanel.BringToFront();
                PlayerPanel.Dock = DockStyle.Bottom;
            }
            int cont = 0;
            if (SearchSearchResultsDomainUp.SelectedIndex != -1)
            {
                foreach (object searched in SearchSearchResultsDomainUp.Items)
                {
                    cont++;
                }
                for (int i = 0; i < cont; cont--)
                {
                    SearchSearchResultsDomainUp.Items.RemoveAt(cont - 1);
                }
            }
        }
        private void PlayVideoLyrics_Click_1(object sender, EventArgs e)
        {
            PlaySongDisplayLyrics.Visible = false;
            PlaySongDisplayLyrics.Clear();
            string[] searchedVideo = PlayVideoVideoPlaying.Text.Split(':');
            List<string> infoVideo = GetVideoButton(searchedVideo[0], searchedVideo[1], searchedVideo[2]);

            if (infoVideo[9] != null && infoVideo[9].Contains(".srt"))
            {
                string strRegex = @"^.*([a-zA-Z]).*$";
                Regex myRegex = new Regex(strRegex, RegexOptions.Multiline);

                string lyricsFile = File.ReadAllText(infoVideo[9]);

                foreach (Match myMatch in myRegex.Matches(lyricsFile))
                {
                    if (myMatch.Success)
                    {
                        PlayVideoShowLyrics.Visible = true;
                        PlayVideoShowLyrics.AppendText(myMatch.Value + "\n");
                    }
                }
            }
            else
            {
                PlayVideoMessageAlertTextBox.AppendText("ERROR[!] Couldn't find subtitles");
                Thread.Sleep(2000);
                PlayVideoMessageAlertTextBox.Clear();
            }
        }
        //-------------------------------------------------------------------------------------------




        //<<!SEARCH USER PANEL>>
        //-------------------------------------------------------------------------------------------

        private void SearchUserFollowButton_Click(object sender, EventArgs e)
        {
            
        }
        private void SearchUserFollowButton_Click_1(object sender, EventArgs e)
        {
            List<User> userDataBase = new List<User>();
            userDataBase = OnSearchUserButton_Click();
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            User logInUser = OnLoginButtonClicked(UserLogInTextBox.Text, PasswordLogInTextBox.Text);

            if (SearchSearchResultsDomainUp.Text.Contains("User:"))
            {
                foreach (User searchedUser in userDataBase)
                {
                    string result = SearchSearchResultsDomainUp.Text;
                    List<string> listuser = searchedUser.FollowingList;
                    if (result == "User: " + searchedUser.SearchedInfoUser())
                    {
                        OnSearchUserFollowButton_Click(logInUser, searchedUser, profile);
                    }
                }
            }
            SearUserName.Clear();
            SearcUserEmailTextBox.Clear();
            SearchUserFollowers.Clear();
            SearchUserFollowing.Clear();
        }
        //ONEVENT

        public void OnSearchUserFollowButton_Click(User userLogIn, User userSearched, Profile profilesearched)
        {
            if (SearchFollowButton_Clicked != null)
            {
                string result = SearchFollowButton_Clicked(this, new UserEventArgs() { UserLogIn = userLogIn, UserSearched = userSearched, ProfileUserLogIn = profilesearched });
                if (result != null)
                {
                    //Un label que appende el result...
                    SearchInvalidCredentialsTextBox.AppendText(result);
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                    SearchInvalidCredentialsTextBox.Clear();
                }
                else
                {
                    //Un label que appende un error...
                    SearchInvalidCredentialsTextBox.AppendText("Error... couldn't follow " + userSearched.Username);
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                    SearchInvalidCredentialsTextBox.Clear();
                }
            }
            else
            {
                SearchInvalidCredentialsTextBox.AppendText("Error... couldn't follow " + userSearched.Username);
                Thread.Sleep(2000);
                DisplayStartPanel.BringToFront();
                SearchInvalidCredentialsTextBox.Clear();
            }
        }

        //GO BACK/CLOSE

        private void SearchUserGoBack_Click(object sender, EventArgs e)
        {
            

        }
        private void SearchUserGoBack_Click_1(object sender, EventArgs e)
        {
            if (SearchSearchResultsDomainUp.SelectedIndex != -1)
            {
                int cont = 0;
                foreach (object searched in SearchSearchResultsDomainUp.Items)
                {
                    cont++;
                }
                for (int i = 0; i < cont; cont--)
                {
                    SearchSearchResultsDomainUp.Items.RemoveAt(cont - 1);
                }
            }

            SearchPanel.BringToFront();
            SearUserName.Clear();
            SearcUserEmailTextBox.Clear();
            SearchUserFollowers.Clear();
            SearchUserFollowing.Clear();
        }
        //-------------------------------------------------------------------------------------------




        /*---------------------------------------------------!PY----------------------------------------------------- */

        //<<!PLAY SONG PANEL>>
        //-------------------------------------------------------------------------------------------
        private void PlaySongStopButton_Click(object sender, EventArgs e)
        {

            if (isInPlaylist)
            {

                List<Song> songDataBase = new List<Song>();
                songDataBase = OnSearchSongButton_Click();
                string ex = PlayPlaylistShowMultimedia.Text;
                foreach (Song song in songDataBase)
                {
                    if (ex.Contains("Song:") && song.Format == ".mp3")
                    {
                        formatProgressBar = song.Format;
                        windowsMediaPlayer.Ctlcontrols.pause();
                        ProgressTimer.Stop();
                        break;
                    }
                    else if (ex.Contains("Song:") && song.Format == ".wav")
                    {
                        formatProgressBar = song.Format;
                        soundPlayer.Stop();
                        ProgressTimer.Stop();
                    }
                }
            }

            else
            {
                if (PlaySongSongPlaying.Text.Length > 0)
                {
                    string[] infoMult = PlaySongSongPlaying.Text.Split(':');
                    string songName = infoMult[0];
                    string artistName = infoMult[1];

                    List<Song> songDataBase = new List<Song>();
                    songDataBase = OnSearchSongButton_Click();
                    foreach (Song s in songDataBase)
                    {
                        if (s.Name == songName && s.Artist == artistName)
                        {
                            if (s.Format == ".mp3")
                            {
                                windowsMediaPlayer.Ctlcontrols.pause();
                                ProgressTimer.Stop();
                                break;
                            }
                            else if (s.Format == ".wav")
                            {
                                TimerWav.Stop();
                                soundPlayer.Stop();
                                break;
                            }
                            s.PresentTime = (double)PlayerMultPanelMtrackPB.Value;
                        }
                    }
                }
            }
            

        }
        private void PlaySongLikeButton_Click(object sender, EventArgs e)
        {
            PlaySongMessageTextBox.Clear();
            string[] searchedMult = SearchSearchResultsDomainUp.Text.Split(':');
            List<string> infoSong = new List<string>();
            if(SearchSearchResultsDomainUp.Text.Contains("Song: ") == false)
            {
                searchedMult = PlayPlaylistShowMultimedia.Text.Split(':');
            }
            if (searchedMult[1] != "")
            {
                infoSong = GetSongButton(searchedMult[1], searchedMult[3]);
                LikeSong_Did(searchedMult[1], searchedMult[3]);
            }
            else
            {
                searchedMult = PlaySongSongPlaying.Text.Split(':');
                infoSong = GetSongButton(searchedMult[0], searchedMult[1]);
                LikeSong_Did(searchedMult[0], searchedMult[1]);
            }

            AddLikedMult(ProfileDomainUp.Text, infoSong[6], null);

        }

        private void PlaySongPlayButton_Click_1(object sender, EventArgs e)
        {
            if (isInPlaylist)
            {
                List<Song> songDataBase = new List<Song>();
                songDataBase = OnSearchSongButton_Click();
                string ex = PlayPlaylistShowMultimedia.Text;
                foreach (Song song in songDataBase)
                {
                    if (ex.Contains("Song:") && song.Format == ".mp3")
                    {
                        formatProgressBar = song.Format;
                        windowsMediaPlayer.Ctlcontrols.play();
                        ProgressTimer.Start();
                        break;
                    }
                    else if (ex.Contains("Song:") && song.Format == ".wav")
                    {
                        formatProgressBar = song.Format;
                        soundPlayer.Play();
                        ProgressTimer.Start();
                    }
                }
            }
            else
            {
                if (PlaySongSongPlaying.Text.Length > 0)
                {
                    string[] infoMult = PlaySongSongPlaying.Text.Split(':');
                    string songName = infoMult[0];
                    string artistName = infoMult[1];

                    List<Song> songDataBase = new List<Song>();
                    songDataBase = OnSearchSongButton_Click();
                    foreach (Song s in songDataBase)
                    {
                        if (s.Name == songName && s.Artist == artistName)
                        {
                            if (s.Format == ".mp3")
                            {
                                formatProgressBar = s.Format;
                                windowsMediaPlayer.Ctlcontrols.play();
                                break;
                            }
                            else if (s.Format == ".wav")
                            {
                                formatProgressBar = s.Format;
                                durationWav = (s.Duration * 60);
                                soundPlayer.Play();
                                TimerWav.Start();
                                PlayerMultPanelMtrackPB.Maximum = (int)durationWav;
                            }
                        }
                    }
                }
            }
            
            
            
            
        }

        private void PlaySongAddQueueButton_Click(object sender, EventArgs e)
        {
            

        }
        private void PlaySongAddQueueButton_Click_1(object sender, EventArgs e)
        {
            string[] searchedMult = PlaySongSongPlaying.Text.Split(':');
            if (SearchSearchResultsDomainUp.Text.Contains("Song: ") == false)
            {
                searchedMult = PlayPlaylistShowMultimedia.Text.Split(':');
            }
            if (searchedMult[2] == ".mp3" || searchedMult[2] == ".wav")
            {
                List<string> songInfo = GetSongButton(searchedMult[0], searchedMult[1]);
                queueListSongs.Add(songInfo[6]);
                PlaySongMessageTextBox.AppendText("Song added to Queue");
                Thread.Sleep(1000);
                PlaySongMessageTextBox.Clear();
            }
        }

        private void PlaySongAddToPlaylistButton_Click(object sender, EventArgs e)
        {
            
        }
        private void PlaySongAddToPlaylistButton_Click_1(object sender, EventArgs e)
        {
            PlaySongMessageTextBox.Clear();
            PlaySongChoosePlsDomainUp.ResetText();
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            if (profile.CreatedPlaylist.Count() != 0)
            {
                foreach (PlayList playList in profile.CreatedPlaylist)
                {
                    PlaySongChoosePlsDomainUp.Items.Add(playList.NamePlayList);
                }
                PlaySongChoosePlsButton.Visible = true;
                PlaySongChoosePlsDomainUp.Visible = true;
            }
            else
            {
                PlaySongMessageTextBox.AppendText("ERROR[!] You don´t have created Playlists");
            }
        }
        private void PlaySongChoosePlsButton_Click(object sender, EventArgs e)
        {
            
        }
        private void PlaySongChoosePlsButton_Click_1(object sender, EventArgs e)
        {
            PlaySongMessageTextBox.Clear();
            List<Song> songDataBase = new List<Song>();
            string result = PlaySongSongPlaying.Text;
            string searchedPlaylistName = PlaySongChoosePlsDomainUp.Text;
            int choosenPl = PlaySongChoosePlsDomainUp.SelectedIndex;
            songDataBase = OnSearchSongButton_Click();
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            PlaySongChoosePlsButton_Click(songDataBase, profile, result, choosenPl, searchedPlaylistName);

            int cont = 0;
            if (PlaySongChoosePlsDomainUp.SelectedIndex != -1)
            {
                foreach (object searched in PlaySongChoosePlsDomainUp.Items)
                {
                    cont++;
                }
                for (int i = 0; i < cont; cont--)
                {
                    PlaySongChoosePlsDomainUp.Items.RemoveAt(cont - 1);
                }
            }
        }


        private void PlaySongDownloadSongButton_Click(object sender, EventArgs e)
        {
            PlaySongMessageTextBox.Clear();
            List<string> listUser = OnLogInLogInButton_Clicked2(UserLogInTextBox.Text);
            if (listUser[3] != "standard")
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string destDirectory = desktopPath + "\\Downloaded-Songs-FyBuZz";
                string songFile;
                if (System.IO.Directory.Exists(destDirectory) == false)
                {
                    System.IO.Directory.CreateDirectory(destDirectory);
                    File.Create(destDirectory + "\\ FyBuZz.txt");
                    if (PlayerPlayingLabel.Text.Contains(".mp3") == true)
                    {
                        songFile = windowsMediaPlayer.URL.Split('\\')[windowsMediaPlayer.URL.Split('\\').Length - 1];
                        string destFile = destDirectory + "\\" + songFile;
                        File.Copy(windowsMediaPlayer.URL, destFile);
                    }
                    else
                    {
                        songFile = soundPlayer.SoundLocation.Split('\\')[soundPlayer.SoundLocation.Split('\\').Length - 1];
                        string destFile = destDirectory + "\\" + songFile;
                        File.Copy(soundPlayer.SoundLocation, destFile);
                    }
                }
                else
                {
                    if (PlayerPlayingLabel.Text.Contains(".mp3") == true)
                    {
                        songFile = windowsMediaPlayer.URL.Split('\\')[windowsMediaPlayer.URL.Split('\\').Length - 1];
                        string destFile = destDirectory + "\\" + songFile;
                        File.Copy(windowsMediaPlayer.URL, destFile);
                    }
                    else
                    {
                        songFile = soundPlayer.SoundLocation.Split('\\')[soundPlayer.SoundLocation.Split('\\').Length - 1];
                        string destFile = destDirectory + "\\" + songFile;
                        File.Copy(soundPlayer.SoundLocation, destFile);
                    }
                }
                PlaySongMessageTextBox.AppendText("Song downloaded succesfully.");
            }
            else
            {
                PlaySongMessageTextBox.Visible = true;
                PlaySongMessageTextBox.AppendText("Standard users can't download songs.");
            }

        }

        private void PlaySongChoosePlsButton_Click(List<Song> songDataBase, Profile profile, string result, int choosenPl, string searchedPL)
        {
            if (PlaySongChoosePlsButton_Clicked != null)
            {
                string final = PlaySongChoosePlsButton_Clicked(this, new PlaylistEventArgs() { RestultText = result, ChoosenIndex = choosenPl, SongDataBaseText = songDataBase, ProfileCreatorText = profile, SearchedPlaylistNameText = searchedPL });
                if (final == null)
                {
                    PlaySongMessageTextBox.AppendText("Song added succesfully.");
                    OnSearchUserButton_Click();

                }
                else
                {
                    PlaySongMessageTextBox.AppendText("ERROR[!] couldn't add song.");
                    Thread.Sleep(1000);
                    PlaySongMessageTextBox.Clear();

                }

            }
        }
        
        private void PlaySongPreviousSongButton_Click(object sender, EventArgs e)
        {
            if (isInPlaylist)
            {
                PlayPlaylistPreviousButton.PerformClick();
            }

            else
            {
                TimerWav.Stop();
                ticks = 0;
                PlaySongDisplayLyrics.Visible = false;
                PlaySongDisplayLyrics.Clear();
                List<List<string>> songInfoMVC = ReturnAllSongsInfo();
                string[] infoSong = PlaySongSongPlaying.Text.Split(':');

                string nameSong = infoSong[0];
                string artistSong = infoSong[1];

                if (songIndex == -1)
                {
                    int cont = 0;
                    foreach (List<string> infoSongEsp in songInfoMVC)
                    {
                        if (nameSong.Contains(infoSongEsp[0]) && artistSong.Contains(infoSongEsp[1]))
                        {
                            break;
                        }
                        cont++;
                    }
                    songIndex = cont;
                }


                int previousSong = 1;


                Song songP = OnSkipOrPreviousSongButton_Clicked(nameSong, artistSong, previousSong, null, queueListSongs, songIndex);
                if (songP != null)
                {
                    PlaySongSongPlaying.Clear();
                    PlaySongSongPlaying.AppendText(songP.Name + ":" + songP.Artist);
                    PlayerPlayingLabel.Clear();
                    SearchPlayingLabel.Clear();
                    if (songP.Format == ".mp3")
                    {
                        windowsMediaPlayer.Ctlcontrols.stop();
                        soundPlayer.Stop();
                        formatProgressBar = songP.Format;
                        windowsMediaPlayer.URL = songP.SongFile;
                        windowsMediaPlayer.Ctlcontrols.play();
                    }
                    else if (songP.Format == ".wav")
                    {
                        windowsMediaPlayer.Ctlcontrols.stop();
                        soundPlayer.Stop();
                        formatProgressBar = songP.Format;
                        durationWav = (songP.Duration * 60);
                        soundPlayer.SoundLocation = songP.SongFile;
                        soundPlayer.Play();
                        TimerWav.Start();
                        PlayerMultPanelMtrackPB.Maximum = (int)durationWav;

                    }
                    PlayerPlayingLabel.AppendText("Song playing: " + songP.Name + songP.Format);
                    SearchPlayingLabel.AppendText("Song playing: " + songP.Name + songP.Format);

                }
                else
                {
                    PlayerPlayingLabel.Clear();
                    PlayerPlayingLabel.AppendText("ERROR[!] ~Song wasn't previoused!");
                }
                if (songIndex == 0) songIndex = songInfoMVC.Count() - 1;
                else songIndex--;
            }
            

        }



        private void PlaySongSkipSongButton_Click(object sender, EventArgs e)
        {
            if (isInPlaylist)
            {
                PlayPlaylistSkipButton.PerformClick();
            }

            else
            {
                TimerWav.Stop();
                ticks = 0;
                PlaySongDisplayLyrics.Visible = false;
                PlaySongDisplayLyrics.Clear();
                List<List<string>> songInfoMVC = ReturnAllSongsInfo();
                string[] infoSong = PlaySongSongPlaying.Text.Split(':');

                string nameSong = infoSong[0];
                string artistSong = infoSong[1];

                if (songIndex == -1)
                {
                    int cont = 0;
                    foreach (List<string> infoSongEsp in songInfoMVC)
                    {
                        if (nameSong.Contains(infoSongEsp[0]) && artistSong.Contains(infoSongEsp[1]))
                        {
                            break;
                        }
                        cont++;
                    }
                    songIndex = cont;
                }

                int previousSong = 0;
                if (songIndex == songInfoMVC.Count() - 1) songIndex = -1;

                Song songS = OnSkipOrPreviousSongButton_Clicked(nameSong, artistSong, previousSong, null, queueListSongs, songIndex);

                if (songS != null)
                {
                    PlaySongSongPlaying.Clear();
                    PlaySongSongPlaying.AppendText(songS.Name + ":" + songS.Artist);
                    PlayerPlayingLabel.Clear();
                    SearchPlayingLabel.Clear();
                    if (songS.Format == ".mp3")
                    {
                        windowsMediaPlayer.Ctlcontrols.stop();
                        soundPlayer.Stop();
                        formatProgressBar = songS.Format;
                        windowsMediaPlayer.URL = songS.SongFile;
                        windowsMediaPlayer.Ctlcontrols.play();
                    }
                    else if (songS.Format == ".wav")
                    {
                        windowsMediaPlayer.Ctlcontrols.stop();
                        soundPlayer.Stop();
                        formatProgressBar = songS.Format;
                        durationWav = (songS.Duration * 60);
                        soundPlayer.SoundLocation = songS.SongFile;
                        soundPlayer.Play();
                        TimerWav.Start();
                        PlayerMultPanelMtrackPB.Maximum = (int)durationWav;
                    }
                    PlayerPlayingLabel.AppendText("Song playing: " + songS.Name + songS.Format);
                    SearchPlayingLabel.AppendText("Song playing: " + songS.Name + songS.Format);
                }
                else
                {
                    PlayerPlayingLabel.Clear();
                    PlayerPlayingLabel.AppendText("ERROR[!] ~Song wasn't skipped!");
                }
                songIndex++;
            }
            
        }


        //!SHOW SONG LYRICS
        private void PlaySongShowLyrics_Click(object sender, EventArgs e)
        {
            

        }
        private void PlaySongShowLyrics_Click_1(object sender, EventArgs e)
        {
            PlaySongDisplayLyrics.Visible = false;
            PlaySongDisplayLyrics.Clear();
            string[] searchedSong = PlaySongSongPlaying.Text.Split(':');
            List<string> infoSong = GetSongButton(searchedSong[0], searchedSong[1]);

            if (infoSong[5] != null && infoSong[5].Contains(".srt"))
            {
                string strRegex = @"^.*([a-zA-Z]).*$";
                Regex myRegex = new Regex(strRegex, RegexOptions.Multiline);

                string lyricsFile = File.ReadAllText(infoSong[5]);

                foreach (Match myMatch in myRegex.Matches(lyricsFile))
                {
                    if (myMatch.Success)
                    {
                        PlaySongDisplayLyrics.Visible = true;
                        PlaySongDisplayLyrics.AppendText(myMatch.Value + "\n");
                    }
                }
            }
            else
            {
                PlaySongMessageTextBox.AppendText("ERROR[!] Couldn't find lyrics");
                Thread.Sleep(2000);
                PlaySongMessageTextBox.Clear();
            }
        }
        //Rate Song
        private void PlaysSongRateButton_Click(object sender, EventArgs e)
        {
            PlaySongRateMessageTextBox.Clear();
            PlaySongRateNumDomainUp.Visible = true;
            int userRate = (int)PlaySongRateNumDomainUp.Value;
            string[] infoSong = SearchSearchResultsDomainUp.Text.Split(':');
            List<string> infoSongList = new List<string>();
            if (SearchSearchResultsDomainUp.Text.Contains("Song: ") == false)
            {
                infoSong = PlayPlaylistShowMultimedia.Text.Split(':');
            }
            if (infoSong[1] != "")
            {
                infoSongList = GetSongButton(infoSong[1], infoSong[3]);
                PlaysSongRateButton_Click(userRate, infoSong[1], infoSong[3]);
            }
            else
            {
                infoSong = PlaySongSongPlaying.Text.Split(':');
                infoSongList = GetSongButton(infoSong[0], infoSong[1]);
                PlaysSongRateButton_Click(userRate, infoSong[0], infoSong[1]);
            }

            PlaySongRateMessageTextBox.AppendText(infoSongList[7]);
        }
        //ONEVENT

        public void LikeSong_Did(string sName, string sArtist)
        {
            if (LikedSong_Done != null)
            {
                string result = LikedSong_Done(this, new SongEventArgs() { NameText = sName, ArtistText = sArtist });
                if (result != null)
                {
                    PlaySongMessageTextBox.AppendText(result);
                }
                else
                {
                    PlaySongMessageTextBox.AppendText("ERROR[!] couldn't like song");
                }
            }

        }
        public Song OnSkipOrPreviousSongButton_Clicked(string nameSong, string ArtistSong, int skipOrPreviousSong, PlayList playlist, List<string> onQueue, int num)
        {
            if (SkipOrPreviousSongButton_Clicked != null)
            {
                Song song = SkipOrPreviousSongButton_Clicked(this, new SongEventArgs() { NameText = nameSong, ArtistText = ArtistSong, SkipOrPrevious = skipOrPreviousSong, playlistSong = playlist, OnQueueText = onQueue, NumberText = num});
                PlayerPlayingLabel.Clear();
                if (song != null)
                {
                    if (skipOrPreviousSong == 0)
                    {
                        PlayerPlayingLabel.AppendText("song has been Skipped!");
                        return song;
                    }
                    else
                    {
                        PlayerPlayingLabel.AppendText("song has been previoused!");
                        return song;
                    }
                }
                else
                {
                    if (skipOrPreviousSong == 0)
                    {
                        PlayerPlayingLabel.AppendText("ERROR[!] ~couldn't skip song!");

                    }
                    else
                    {
                        PlayerPlayingLabel.AppendText("ERROR[!] ~couldn't previous song!");

                    }
                    return null;
                }

            }
            else
            {
                PlayerPlayingLabel.Clear();
                PlayerPlayingLabel.AppendText("ERROR[!] ~Something went horribly wrong!");
            }
            return null;
        }

        public void PlaysSongRateButton_Click(int rated, string sName, string sArtist)
        {
            if (PlaysSongRateButton_Clicked != null)
            {
                string result = PlaysSongRateButton_Clicked(this, new SongEventArgs() { RankingText = rated, NameText = sName, ArtistText = sArtist });
                if (result != null)
                {
                    PlaySongMessageTextBox.Clear();
                    PlaySongMessageTextBox.AppendText(result);
                }
                else
                {
                    PlaySongMessageTextBox.Clear();
                    PlaySongMessageTextBox.AppendText("ERROR[!] couldn't rate song");
                }
            }
        }

        //GO BACK/CLOSE

        private void PlaySongGoBackButton_Click(object sender, EventArgs e)
        {
            
        }
        private void PlaySongGoBackButton_Click_1(object sender, EventArgs e)
        {
            PlayerMultPanel.Visible = true;
            PlaySongChooseUserButton.Visible = false;
            PlaySongChooseUserDomainUp.Visible = false;
            PlaySongSkipSongButton.Visible = true;
            PlaySongPreviousSongButton.Visible = true;
            PlaySongSongPlaying.Text = String.Empty;
            SearchPlayingLabel.Clear();
            PlaySongRateMessageTextBox.Clear();
            SearchPlayingLabel.Text = PlayerPlayingLabel.Text;
            SearchSearchTextBox.Text = "Search Songs,Video, Playlists or Users";
            SearchSearchResultsDomainUp.Visible = false;
            SearchPlayingPanel.Visible = true;
            //SearchSearchResultsDomainUp.Items.Clear();
            SearchSearchResultsDomainUp.Text = "Searched Results:";
            SearchPanel.BringToFront();
            SearchSearchResultsDomainUp.ResetText();
            int cont = 0;
            if (SearchSearchResultsDomainUp.Items.Count == 0)
            {
                foreach (object searched in SearchSearchResultsDomainUp.Items)
                {
                    cont++;
                }
                for (int i = 0; i < cont; cont--)
                {
                    SearchSearchResultsDomainUp.Items.RemoveAt(cont - 1);
                }
            }
            int cont1 = 0;
            if (PlaySongChoosePlsDomainUp.SelectedIndex != -1)
            {
                foreach (object searched in PlaySongChoosePlsDomainUp.Items)
                {
                    cont1++;
                }
                for (int i = 0; i < cont; cont1--)
                {
                    PlaySongChoosePlsDomainUp.Items.RemoveAt(cont - 1);
                }
            }
            PlaySongDisplayLyrics.Clear();
            PlaySongDisplayLyrics.Visible = false;
        }
        //-------------------------------------------------------------------------------------------




        //<<!PLAY VIDEO PANEL>>
        //-------------------------------------------------------------------------------------------
        private void PlayVideoAddToPlaylistButton_Click(object sender, EventArgs e)
        {
            
        }
        private void PlayVideoAddToPlaylistButton_Click_1(object sender, EventArgs e)
        {
            PlayVideoSelectPlDomainUp.ResetText();

            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);

            if (profile.CreatedPlaylist.Count() != 0)
            {
                foreach (PlayList playlist in profile.CreatedPlaylist)
                {
                    PlayVideoSelectPlDomainUp.Items.Add(playlist.NamePlayList);
                }
                PlayVideoSelectPlDomainUp.Visible = true;
                PlayVideoSelectPlButton.Visible = true;
            }
            else
            {
                PlayVideoMessageAlertTextBox.AppendText("ERROR[!] you don't have created playlist");
                Thread.Sleep(1000);
                PlayVideoMessageAlertTextBox.Clear();
            }
        }
        private void PlayVideoLikeButton_Click(object sender, EventArgs e)
        {
            
        }
        private void PlayVideoLikeButton_Click_1(object sender, EventArgs e)
        {
            PlayVideoMessageAlertTextBox.Clear();
            string[] searchedMult = SearchSearchResultsDomainUp.Text.Split(':');
            List<string> infoVideo = new List<string>();

            if (SearchSearchResultsDomainUp.Text.Contains("Video:") == false)
            {
                searchedMult = PlayPlaylistShowMultimedia.Text.Split(':');
            }
            if (searchedMult[1] != "")
            {
                infoVideo = GetVideoButton(searchedMult[1], searchedMult[3], searchedMult[5]);
                LikeVideo_Did(searchedMult[1], searchedMult[3], searchedMult[5]);
            }
            else
            {
                searchedMult = PlayVideoVideoPlaying.Text.Split(':');
                infoVideo = GetVideoButton(searchedMult[0], searchedMult[1], searchedMult[2]);
                LikeVideo_Did(searchedMult[0], searchedMult[1], searchedMult[2]);
            }

            AddLikedMult(ProfileDomainUp.Text, null, infoVideo[8]);
        }
        private void PlayVideoLyrics_Click(object sender, EventArgs e)
        {
            PlaySongDisplayLyrics.Visible = false;
            PlaySongDisplayLyrics.Clear();
            string[] searchedVideo = PlayVideoVideoPlaying.Text.Split(':');
            List<string> infoVideo = GetVideoButton(searchedVideo[0], searchedVideo[1], searchedVideo[2]);

            if (infoVideo[9] != null && infoVideo[9].Contains(".srt"))
            {
                string strRegex = @"^.*([a-zA-Z]).*$";
                Regex myRegex = new Regex(strRegex, RegexOptions.Multiline);

                string lyricsFile = File.ReadAllText(infoVideo[9]);

                foreach (Match myMatch in myRegex.Matches(lyricsFile))
                {
                    if (myMatch.Success)
                    {
                        PlayVideoShowLyrics.Visible = true;
                        PlayVideoShowLyrics.AppendText(myMatch.Value + "\n");
                    }
                }
            }
            else
            {
                PlayVideoMessageAlertTextBox.AppendText("ERROR[!] Couldn't find subtitles");
                Thread.Sleep(2000);
                PlayVideoMessageAlertTextBox.Clear();
            }
        }

        private void PlayVideoFullScreenButton_Click(object sender, EventArgs e)
        {
            
        }
        private void PlayVideoFullScreenButton_Click_1(object sender, EventArgs e)
        {
            if (wmpVideo.URL.Length > 0)
            {
                wmpVideo.fullScreen = true;
            }
        }

        private void PlayVideoSelectPlButton_Click(object sender, EventArgs e)
        {

        }
        private void PlayVideoSelectPlButton_Click_1(object sender, EventArgs e)
        {
            List<Video> videoDataBase = new List<Video>();
            string result = PlayVideoVideoPlaying.Text;
            string searchedPlaylistName = PlayVideoSelectPlDomainUp.Text;
            videoDataBase = OnSearchVideoButton_Click();
            OnPlayVideoSelectPlButton_Clicked(result, videoDataBase, searchedPlaylistName);
            SearchSearchResultsDomainUp.ResetText();
        }


        private void PlayVideoRateVideoButton_Click(object sender, EventArgs e)
        {
            
        }
        private void PlayVideoRateVideoButton_Click_1(object sender, EventArgs e)
        {
            VideoRate.Clear();
            PlayVideoRateDomainUp.Visible = true;
            int userRate = (int)PlayVideoRateDomainUp.Value;

            string[] infoVideo = SearchSearchResultsDomainUp.Text.Split(':');
            List<string> infoVideoList = new List<string>();
            if (infoVideo[1] != "")
            {
                PlaysVideoRateButton_Click(userRate, infoVideo[1], infoVideo[3], infoVideo[5]);
                infoVideoList = GetVideoButton(infoVideo[1], infoVideo[3], infoVideo[5]);
            }
            else
            {
                infoVideo = PlayVideoVideoPlaying.Text.Split(':');
                PlaysVideoRateButton_Click(userRate, infoVideo[0], infoVideo[1], infoVideo[2]);
                infoVideoList = GetVideoButton(infoVideo[0], infoVideo[1], infoVideo[2]);
            }

            VideoRate.AppendText(infoVideoList[6]);
        }

        private void PlayVideoQueue_Click(object sender, EventArgs e)
        {

        }
        private void PlayVideoQueue_Click_1(object sender, EventArgs e)
        {
            string[] searchedMult = PlayVideoVideoPlaying.Text.Split(':');
            if (searchedMult[3] == ".mov" || searchedMult[3] == ".avi" || searchedMult[3] == ".mp4")
            {
                List<string> videoInfo = GetVideoButton(searchedMult[0], searchedMult[1], searchedMult[2]);
                queueListSongs.Add(videoInfo[8]);
                PlayVideoMessageAlertTextBox.AppendText("Video added to Queue");

                Thread.Sleep(1000);
                PlayVideoMessageAlertTextBox.Clear();
            }
        }

        private void PlayVideoPreviousButton_Click(object sender, EventArgs e)
        {
            PlaySongDisplayLyrics.Visible = false;
            PlaySongDisplayLyrics.Clear();
            List<List<string>> videoInfoMVC = ReturnAllVideosInfo();
            string[] infoVideo = SearchSearchResultsDomainUp.Text.Split(':');
            string nameVideo = infoVideo[1];
            string nameActor = infoVideo[3];
            string nameDirector = infoVideo[5];


            if (videoIndex == -1)
            {
                int cont = 0;
                foreach (List<string> infoVideoEsp in videoInfoMVC)
                {
                    if (nameVideo.Contains(infoVideoEsp[0]) && nameActor.Contains(infoVideoEsp[1]) && nameDirector.Contains(infoVideoEsp[2]))
                    {
                        break;
                    }
                    cont++;
                }
                videoIndex = cont;
            }


            int previous = 1;

            Video video = OnSkipOrPreviousVideoButton_Click(nameVideo, nameActor, previous, null, queueListSongs, videoIndex);

            if (video != null)
            {
                PlayVideoVideoPlaying.Clear();
                PlayVideoVideoPlaying.AppendText(video.Name + ":" + video.Actors + ":" + video.Directors + ":" + video.Format);
                PlayVideoMessageAlertTextBox.Clear();

                wmpVideo.Ctlcontrols.stop();
                wmpVideo.URL = video.FileName;
                wmpVideo.Ctlcontrols.play();

                PlayVideoMessageAlertTextBox.AppendText("Video Playing: " + video.Name + video.Format);
            }
            else
            {
                PlayVideoMessageAlertTextBox.Clear();
                PlayVideoMessageAlertTextBox.AppendText("Video wasn't previoused!");
            }

            if (videoIndex == 0) videoIndex = videoInfoMVC.Count() - 1;
            else videoIndex--;
        }


        private void PlayVideoSkipButton_Click(object sender, EventArgs e)
        {
            PlaySongDisplayLyrics.Visible = false;
            PlaySongDisplayLyrics.Clear();
            List<List<string>> videoInfoMVC = ReturnAllVideosInfo();
            string[] infoVideo = SearchSearchResultsDomainUp.Text.Split(':');
            string nameVideo = infoVideo[1];
            string nameActor = infoVideo[3];
            string nameDirector = infoVideo[5];

            if (videoIndex == -1)
            {
                int cont = 0;
                foreach (List<string> infoVideoEsp in videoInfoMVC)
                {
                    if ((nameVideo.Contains(infoVideoEsp[0])) && (nameActor.Contains(infoVideoEsp[1]) && nameDirector.Contains(infoVideoEsp[2])))
                    {
                        break;
                    }
                    cont++;
                }
                videoIndex = cont;
            }
            int previous = 0;
            if (videoIndex == videoInfoMVC.Count() - 1) videoIndex = -1;

            Video video = OnSkipOrPreviousVideoButton_Click(nameVideo, nameActor, previous, null, queueListSongs, videoIndex);

            if (video != null)
            {
                PlayVideoVideoPlaying.Clear();
                PlayVideoVideoPlaying.AppendText(video.Name + ":" + video.Actors + ":" + video.Directors + ":" + video.Format);
                PlayVideoMessageAlertTextBox.Clear();

                wmpVideo.Ctlcontrols.stop();
                wmpVideo.URL = video.FileName;
                wmpVideo.Ctlcontrols.play();

                PlayVideoMessageAlertTextBox.AppendText("Video Playing: " + video.Name + video.Format);
            }
            else
            {
                PlayVideoMessageAlertTextBox.Clear();
                PlayVideoMessageAlertTextBox.AppendText("Video wasn't Skipped!");
            }
            videoIndex++;
        }
        //ONEVENT

        public void OnPlayVideoSelectPlButton_Clicked(string result, List<Video> videoDataBase, string searchedPl)
        {
            if (PlayVideoSelectPlButton_Clicked != null)
            {
                string description = PlayVideoSelectPlButton_Clicked(this, new PlaylistEventArgs() { RestultText = result, videoDataBaseText = videoDataBase, SearchedPlaylistNameText = searchedPl });
                if (description == null)
                {
                    PlayVideoMessageAlertTextBox.AppendText("Video added successfully!");
                    OnSearchUserButton_Click();
                    Thread.Sleep(500);
                    PlayVideoMessageAlertTextBox.Clear();
                }
                else
                {
                    PlayVideoMessageAlertTextBox.AppendText("ERROR[!] ~ Couldn't add video");
                    Thread.Sleep(500);
                    PlayVideoMessageAlertTextBox.Clear();
                }
            }
        }
        
        public void LikeVideo_Did(string vName, string vActor, string vDirector)
        {
            if (LikedVideo_Done != null)
            {
                string result = LikedVideo_Done(this, new VideoEventArgs() { NameText = vName, ActorsText = vActor, DirectorsText = vDirector });
                if (result != null)
                {
                    PlayVideoMessageAlertTextBox.AppendText(result);
                }
                else
                {
                    PlayVideoMessageAlertTextBox.AppendText("ERROR[!] couldn't like song");
                }
            }

        }


        public Video OnSkipOrPreviousVideoButton_Click(string nameVideo, string nameActor, int skipOrPrevious, PlayList playlist, List<string> onQueue, int num)
        {
            if (SkipOrPreviousVideoButton_Clicked != null)
            {
                Video video = SkipOrPreviousVideoButton_Clicked(this, new VideoEventArgs() { NameText = nameVideo, ActorsText = nameActor, previousOrSkip = skipOrPrevious, playlistVideo = playlist, OnQueue = onQueue, NumberText = num });
                PlayVideoMessageAlertTextBox.Clear();
                if (video != null)
                {
                    if (skipOrPrevious == 0)
                    {
                        PlayVideoMessageAlertTextBox.AppendText("Video has been skipped!");
                    }
                    else
                    {
                        PlayVideoMessageAlertTextBox.AppendText("Video has been previoused!");
                    }
                    return video;
                }
                else
                {
                    if (skipOrPrevious == 0)
                    {
                        PlayVideoMessageAlertTextBox.AppendText("ERROR[!] ~ Video couldn't be skipped!");
                    }
                    else
                    {
                        PlayVideoMessageAlertTextBox.AppendText("ERROR[!] ~ Video couldn't be previoused!");
                    }
                    return null;
                }
            }
            else
            {
                PlayVideoMessageAlertTextBox.Clear();
                PlayVideoMessageAlertTextBox.AppendText("ERROR[!] ~Something went horribly wrong!");
                return null;
            }
        }

        public void PlaysVideoRateButton_Click(int rated, string vName, string vActors, string vDirectors)
        {
            VideoRate.Clear();
            if (PlaysVideoRateButton_Clicked != null)
            {
                string result = PlaysVideoRateButton_Clicked(this, new VideoEventArgs() { RankingText = rated, NameText = vName, ActorsText = vActors, DirectorsText = vDirectors });
                if (result != null)
                {
                    PlayVideoMessageLabel.Clear();
                    PlayVideoMessageLabel.AppendText(result);
                }
                else
                {
                    PlayVideoMessageLabel.Clear();
                    PlayVideoMessageLabel.AppendText("ERROR[!] couldn't rate song");
                }
            }
        }

        //GO BACK/CLOSE

        private void PlayVideoGoBackButton_Click(object sender, EventArgs e)
        {
            PlayVideoVideoPlaying.Text = String.Empty;
            PlayVideoVideoPlaying.ResetText();
            SearchPlayingLabel.Clear();
            if (PlayPlaylistShowMultimedia.SelectedIndex != -1)
            {
                int cont = 0;
                foreach (object searched in PlayPlaylistShowMultimedia.Items)
                {
                    cont++;
                }
                for (int i = 0; i < cont; cont--)
                {
                    PlayPlaylistShowMultimedia.Items.RemoveAt(cont - 1);
                }
            }
            int cont1 = 0;
            if (SearchSearchResultsDomainUp.SelectedIndex != -1)
            {
                foreach (object searched in SearchSearchResultsDomainUp.Items)
                {
                    cont1++;
                }
                for (int i = 0; i < cont1; cont1--)
                {
                    SearchSearchResultsDomainUp.Items.RemoveAt(cont1 - 1);
                }
            }
            SearchSearchResultsDomainUp.ResetText();
            SearchSearchResultsDomainUp.Text = "Searched Results:";
            SearchSearchResultsDomainUp.Visible = false;
            if (PlayVideoSelectPlDomainUp.SelectedIndex != -1)
            {
                foreach (object searched in PlayVideoSelectPlDomainUp.Items)
                {
                    cont1++;
                }
                for (int i = 0; i < cont1; cont1--)
                {
                    PlayVideoSelectPlDomainUp.Items.RemoveAt(cont1 - 1);
                }
            }
            PlayVideoSelectPlDomainUp.Visible = true;
            PlayVideoSelectPlButton.Visible = true;

            SearchSearchTextBox.Clear();
            wmpVideo.Ctlcontrols.stop();
            SearchPanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------



        //<<!PLAY PLAYLIST PANEL>>
        //-------------------------------------------------------------------------------------------


        private void PlayPlaylistChooseMultimediaButton_Click(object sender, EventArgs e)
        {
            
        }
        private void PlayVideoGoBackButton_Click_1(object sender, EventArgs e)
        {
            windowsMediaPlayer.Ctlcontrols.stop();
            soundPlayer.Stop();

            SearchDisplayMoreMultimediaInfo.Clear();
            SearchDisplayMoreMultimediaInfo.Visible = false;
            SearchSearchTextBox.Text = "Search Songs,Video, Playlists or Users";
            SearchSearchResultsDomainUp.Visible = false;
            //SearchSearchResultsDomainUp.Items.Clear();
            SearchSearchResultsDomainUp.Text = "Searched Results:";
            DisplayStartPanel.BringToFront();
            if (windowsMediaPlayer.URL != null)
            {
                PlayerPanel.BringToFront();
                PlayerPanel.Dock = DockStyle.Bottom;
            }
            int cont = 0;
            if (SearchSearchResultsDomainUp.SelectedIndex != -1)
            {
                foreach (object searched in SearchSearchResultsDomainUp.Items)
                {
                    cont++;
                }
                for (int i = 0; i < cont; cont--)
                {
                    SearchSearchResultsDomainUp.Items.RemoveAt(cont - 1);
                }
            }
        }

        private void PlayVideoInfoVideo_Click(object sender, EventArgs e)
        {
            SearchDisplayMoreMultimediaInfo.Clear();
            SearchDisplayMoreMultimediaInfo.Visible = true;
            string[] infoMult = SearchSearchResultsDomainUp.Text.Split(':');
            List<string> infoMultimedia = new List<string>();

            int n = 0;
            infoMultimedia = GetVideoButton(infoMult[1], infoMult[3], infoMult[5]);
            List<string> information = new List<string>() { "Name: ", "Actor: ", "Director: ", "Quality: ", "Category: ",
                                                               "Rated: ", "Ranking: ", "Description: ","Video File: ", "Subtitiles File: "};
            foreach (string info in infoMultimedia)
            {
                SearchDisplayMoreMultimediaInfo.AppendText(information[n] + info + "\r\n");
                n++;
            }
        }

        private void InfoMediaButton_Click(object sender, EventArgs e)
        {
            PlaySongDisplayLyrics.Clear();
            SearchDisplayMoreMultimediaInfo.Clear();
            SearchDisplayMoreMultimediaInfo.Visible = true;
            string[] infoMult = SearchSearchResultsDomainUp.Text.Split(':');
            List<string> infoMultimedia = new List<string>();

            int n = 0;
            infoMultimedia = ReturnInfoSong2(infoMult[1], infoMult[3]);
            List<string> information = new List<string>() { "Album: ", "Artists: ", "Discography: ", "Gender: ", "Studio: ",
                                                               "Lyrics File: ", "Song File: ", "Ranking: ","Name: "};
            foreach (string info in infoMultimedia)
            {
                PlaySongDisplayLyrics.AppendText(information[n] + info + "\r\n");
                n++;
            }
        }

        private void PlayPlaylistChooseMultimediaButton_Click_1(object sender, EventArgs e)
        {
            int cont1 = 0;
            if (PlayVideoSelectPlDomainUp.SelectedIndex != -1)
            {
                foreach (object x in PlayVideoSelectPlDomainUp.Items)
                {
                    cont1++;
                }
                for (int i = 0; i < cont1; cont1--)
                {
                    PlayVideoSelectPlDomainUp.Items.RemoveAt(cont1 - 1);
                }
            }
            PlayVideoSelectPlDomainUp.Visible = false;
            PlayVideoSelectPlButton.Visible = false;

            soundPlayer.Stop();
            windowsMediaPlayer.Ctlcontrols.stop();
            PlayPlaylistProgressBarBox.Value = 0;
            PlayPlaylistTimerBox.Clear();
            soundPlayer.Stop();
            windowsMediaPlayer.Ctlcontrols.pause();
            PlayPlaylistMessageBox.Clear();
            PlayPlaylistPlayerPanel.Visible = true;

            List<Song> songDataBase = new List<Song>();
            List<PlayList> playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();
            List<PlayList> privPls = GetPrivPlaylist();
            PlayList choosenPL = null;
            List<Video> videoDataBase = OnSearchVideoButton_Click();
            songDataBase = OnSearchSongButton_Click();

            string searched = SearchSearchResultsDomainUp.Text;
            string multimediaType = PlayPlaylistShowMultimedia.Text;

            List<string> infoProfile = OnProfilesChooseProfile_Click2(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            List<string> userInfo = OnLogInLogInButton_Clicked2(UserLogInTextBox.Text);

            if (userInfo[3] != "standard")
            {
                foreach (PlayList playList in playlistDataBase)
                {
                    if (searched.Contains(playList.NamePlayList) == true)
                    {
                        choosenPL = playList;
                        break;
                    }
                }
                if (choosenPL == null)
                {
                    foreach (PlayList playList in privPls)
                    {
                        if (searched.Contains(playList.NamePlayList) == true)
                        {
                            choosenPL = playList;
                            break;
                        }
                    }
                }

                if (multimediaType.Contains("Song:") == true && multimediaType.Contains("Artist:") == true)
                {

                    List<string> choosenPLPers = ReturnSearchedMult(ProfileDomainUp.Text, "Song", null);
                    int playlistIndex = PlayPlaylistShowMultimedia.SelectedIndex;

                    int cont = 0;
                    if (choosenPL.Songs.Count() == 0)
                    {
                        cont++;
                    }
                    if (choosenPL != null && cont == 0)

                    {
                        while (playlistIndex < choosenPL.Songs.Count())
                        {
                            if (choosenPL.Songs[playlistIndex].Format == ".mp3")
                            {
                                int cantBadWords = 0;
                                foreach (string badWord in badWords)
                                {
                                    if (choosenPL.Songs[playlistIndex].Lyrics.Contains(badWord))
                                    {
                                        cantBadWords++;
                                        break;
                                    }
                                }
                                if (cantBadWords != 0 && int.Parse(infoProfile[3]) < 16)
                                {
                                    PlayPlaylistMessageBox.AppendText("ERROR[!] Age restriction");
                                    break;
                                }
                                else
                                {
                                    if (multimediaType == choosenPL.Songs[playlistIndex].SearchedInfoSong())
                                    {
                                        PlayPlaylistMessageBox.Clear();
                                        PlaySongProgressBar.Value = 0;
                                        PlaySongTimerTextBox.Clear();
                                        windowsMediaPlayer.URL = choosenPL.Songs[playlistIndex].SongFile;
                                        formatProgressBar = choosenPL.Songs[playlistIndex].Format;

                                        PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                                        SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                                        ProgressTimer.Start();
                                        windowsMediaPlayer.Ctlcontrols.play();
                                    }
                                    if (playlistIndex == choosenPL.Songs.Count())
                                    {
                                        if (PlayPlaylistLoopCheckBox.Checked == true)
                                        {
                                            playlistIndex = 0;
                                        }
                                    }
                                    playlistIndex++;
                                }
                            }
                            else if (choosenPL.Songs[playlistIndex].Format == ".wav")
                            {
                                int cantBadWords = 0;
                                foreach (string badWord in badWords)
                                {
                                    if (choosenPL.Songs[playlistIndex].Lyrics.Contains(badWord))
                                    {
                                        cantBadWords++;
                                        break;
                                    }
                                }
                                if (cantBadWords != 0 && int.Parse(infoProfile[3]) < 16)
                                {
                                    PlayPlaylistMessageBox.AppendText("ERROR[!] Age restriction");
                                    break;
                                }
                                else
                                {
                                    if (multimediaType == choosenPL.Songs[playlistIndex].SearchedInfoSong())
                                    {

                                        PlayPlaylistMessageBox.Clear();
                                        PlaySongProgressBar.Value = 0;
                                        PlaySongTimerTextBox.ResetText();
                                        soundPlayer.SoundLocation = choosenPL.Songs[playlistIndex].SongFile;
                                        formatProgressBar = choosenPL.Songs[playlistIndex].Format;
                                        soundPlayer.Play();
                                        durationWav = (choosenPL.Songs[playlistIndex].Duration*60);
                                        ticks = 0;
                                        TimerWav.Start();

                                        PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                                        SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                                        DurationTimer.Start();

                                    }
                                    if (playlistIndex == choosenPL.Songs.Count())
                                    {
                                        if (PlayPlaylistLoopCheckBox.Checked == true)
                                        {
                                            playlistIndex = 0;

                                        }
                                    }
                                }
                                playlistIndex++;
                            }
                        }
                    }
                    else //Cuando entro de displayPlaylist
                    {
                        foreach (Song song in songDataBase)
                        {
                            int cantBadWords = 0;
                            foreach (string badWord in badWords)
                            {
                                if (song.Lyrics.Contains(badWord))
                                {
                                    cantBadWords++;
                                    break;
                                }
                            }
                            if (cantBadWords != 0 && int.Parse(infoProfile[3]) < 16)
                            {
                                PlayPlaylistMessageBox.AppendText("ERROR[!] Age restriction");
                                break;
                            }
                            else
                            {
                                if (choosenPLPers[playlistIndex].Contains(song.SongFile) == true)
                                {
                                    if (song.SongFile.Contains(".mp3"))
                                    {
                                        PlayPlaylistMessageBox.Clear();
                                        PlaySongProgressBar.Value = 0;
                                        PlaySongTimerTextBox.Clear();

                                        windowsMediaPlayer.URL = song.SongFile;
                                        formatProgressBar = song.Format;

                                        PlayPlaylistMessageBox.AppendText("Playlist playing: " + song.Name + song.Format);
                                        SearchPlayingLabel.AppendText("Playlist playing: " + song.Name + song.Format);
                                        ProgressTimer.Start();
                                        windowsMediaPlayer.Ctlcontrols.play();
                                        break;
                                    }
                                    else if (song.SongFile.Contains(".wav"))
                                    {
                                        PlayPlaylistMessageBox.Clear();
                                        PlaySongProgressBar.Value = 0;
                                        PlaySongTimerTextBox.ResetText();
                                        string file = song.SongFile;
                                        formatProgressBar = song.Format;
                                        soundPlayer.SoundLocation = file;
                                        soundPlayer.Play();
                                        durationWav = (song.Duration*60);
                                        ticks = 0;
                                        TimerWav.Start();

                                        PlayPlaylistMessageBox.AppendText("Playlist playing: " + song.Name + song.Format);
                                        SearchPlayingLabel.AppendText("Playlist playing: " + song.Name + song.Format);
                                        DurationTimer.Start();
                                        break;
                                    }
                                    playlistIndex++;
                                }
                            }

                        }

                    }
                }
                else if (multimediaType.Contains("Video:") == true && multimediaType.Contains("Actors:") == true)
                {
                    List<string> choosenPLPers = ReturnSearchedMult(ProfileDomainUp.Text, null, "Video");
                    int playlistIndex = PlayPlaylistShowMultimedia.SelectedIndex;
                    int cont = 0;
                    if (PlayPlaylistShowMultimedia.Items.Count == 0)
                    {
                        PlayPlaylistMessageBox.AppendText("no videos");
                        cont++;
                    }
                    if (choosenPL != null && cont == 0)
                    {
                        while (playlistIndex < choosenPL.Videos.Count())
                        {
                            if (choosenPL.Videos[playlistIndex].Format == ".mp4")
                            {
                                if (int.Parse(infoProfile[3]) < int.Parse(choosenPL.Videos[playlistIndex].Category))
                                {
                                    PlayPlaylistMessageBox.AppendText("ERROR[!] Age restriction");
                                    break;
                                }
                                else
                                {

                                    if (multimediaType == choosenPL.Videos[playlistIndex].SearchedInfoVideo())
                                    {
                                        wmpVideo.Ctlcontrols.stop();
                                        PlayPlaylistMessageBox.Clear();
                                        PlaySongTimerTextBox.Clear();
                                        wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;
                                        PlayVideoPanel.BringToFront();
                                        wmpVideo.Ctlcontrols.play();
                                        break;
                                    }
                                    if (playlistIndex == choosenPL.Videos.Count())
                                    {
                                        if (PlayPlaylistLoopCheckBox.Checked == true)
                                        {
                                            playlistIndex = 0;
                                        }
                                    }
                                    playlistIndex++;
                                }
                            }
                            else if (choosenPL.Videos[playlistIndex].Format == ".mov")
                            {
                                if (int.Parse(infoProfile[3]) < int.Parse(choosenPL.Videos[playlistIndex].Category))
                                {
                                    PlayPlaylistMessageBox.AppendText("ERROR[!] Age restriction");
                                    break;
                                }
                                else
                                {
                                    if (multimediaType == choosenPL.Videos[playlistIndex].SearchedInfoVideo())
                                    {

                                        wmpVideo.Ctlcontrols.stop();
                                        PlayPlaylistMessageBox.Clear();
                                        PlaySongTimerTextBox.Clear();
                                        wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;

                                        PlayVideoPanel.BringToFront();
                                        wmpVideo.Ctlcontrols.play();
                                        break;


                                    }
                                    if (playlistIndex == choosenPL.Videos.Count())
                                    {
                                        if (PlayPlaylistLoopCheckBox.Checked == true)
                                        {
                                            playlistIndex = 0;

                                        }
                                    }
                                    playlistIndex++;
                                }
                            }
                            else if (choosenPL.Videos[playlistIndex].Format == ".avi")
                            {
                                if (int.Parse(infoProfile[3]) < int.Parse(choosenPL.Videos[playlistIndex].Category))
                                {
                                    PlayPlaylistMessageBox.AppendText("ERROR[!] Age restriction");
                                    break;
                                }
                                else
                                {
                                    if (multimediaType == choosenPL.Videos[playlistIndex].SearchedInfoVideo())
                                    {
                                        wmpVideo.Ctlcontrols.stop();
                                        PlayPlaylistMessageBox.Clear();
                                        PlaySongTimerTextBox.Clear();
                                        wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;

                                        PlayVideoPanel.BringToFront();
                                        wmpVideo.Ctlcontrols.play();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (choosenPL == null && cont == 0)
                    {
                        foreach (Video video in videoDataBase)
                        {
                            if (int.Parse(infoProfile[3]) < int.Parse(video.Category))
                            {
                                PlayPlaylistMessageBox.AppendText("ERROR[!] Age restriction");
                                break;
                            }
                            else
                            {
                                if (choosenPLPers[playlistIndex].Contains(video.FileName) == true)
                                {
                                    if (video.FileName.Contains(".mp4"))
                                    {
                                        wmpVideo.Ctlcontrols.stop();
                                        PlayPlaylistMessageBox.Clear();
                                        PlaySongTimerTextBox.Clear();
                                        wmpVideo.URL = video.FileName;

                                        PlayVideoPanel.BringToFront();
                                        wmpVideo.Ctlcontrols.play();
                                        break;
                                    }
                                    else if (video.FileName.Contains(".mov"))
                                    {
                                        wmpVideo.Ctlcontrols.stop();
                                        PlayPlaylistMessageBox.Clear();
                                        PlaySongTimerTextBox.Clear();
                                        wmpVideo.URL = video.FileName;

                                        PlayVideoPanel.BringToFront();
                                        wmpVideo.Ctlcontrols.play();
                                        break;
                                    }
                                    else if (video.FileName.Contains(".avi"))
                                    {
                                        wmpVideo.Ctlcontrols.stop();
                                        PlayPlaylistMessageBox.Clear();
                                        PlaySongTimerTextBox.Clear();
                                        wmpVideo.URL = video.FileName;

                                        PlayVideoPanel.BringToFront();
                                        wmpVideo.Ctlcontrols.play();
                                        break;
                                    }
                                    playlistIndex++;
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                PlayPlaylistMessageBox.AppendText("Standard users can't choose multimedia from a Playlist.");
            }
        }
        private void PlayPlaylistRandomButton_Click(object sender, EventArgs e)
        {
            
        }
        private void PlayPlaylistRandomButton_Click_1(object sender, EventArgs e)
        {
            int cont1 = 0;
            if (PlayVideoSelectPlDomainUp.SelectedIndex != -1)
            {
                foreach (object x in PlayVideoSelectPlDomainUp.Items)
                {
                    cont1++;
                }
                for (int i = 0; i < cont1; cont1--)
                {
                    PlayVideoSelectPlDomainUp.Items.RemoveAt(cont1 - 1);
                }
            }
            PlayVideoSelectPlDomainUp.Visible = true;
            PlayVideoSelectPlButton.Visible = true;

            PlayPlaylistProgressBarBox.Value = 0;
            PlayPlaylistTimerBox.Clear();
            soundPlayer.Stop();
            windowsMediaPlayer.Ctlcontrols.stop();


            PlayPlaylistPlayerPanel.Visible = true;
            List<Song> songDataBase = new List<Song>();
            List<PlayList> playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();
            PlayList choosenPL = null;
            songDataBase = OnSearchSongButton_Click();
            List<Video> videoDataBase = OnSearchVideoButton_Click();
            string searched = SearchSearchResultsDomainUp.Text;
            string multimediaType = PlayPlaylistShowMultimedia.Text;
            List<string> infoProfile = OnProfilesChooseProfile_Click2(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            Random random = new Random();
            foreach (PlayList playList in playlistDataBase)
            {
                if (searched.Contains(playList.NamePlayList) == true)
                {
                    choosenPL = playList;
                }
            }
            if (choosenPL != null && choosenPL.Songs.Count() != 0)
            {
                int playlistIndex = random.Next(choosenPL.Songs.Count());

                int cantBadWords = 0;
                foreach (string badWord in badWords)
                {
                    if (choosenPL.Songs[playlistIndex].Lyrics.Contains(badWord))
                    {
                        cantBadWords++;
                    }
                }
                if (cantBadWords != 0 && int.Parse(infoProfile[3]) < 16) PlayPlaylistMessageBox.AppendText("ERROR[!] Age restriction");
                else
                {
                    if (choosenPL.Songs[playlistIndex].Format == ".mp3")
                    {
                        PlayPlaylistMessageBox.Clear();
                        PlaySongProgressBar.Value = 0;
                        PlaySongTimerTextBox.Clear();
                        PlayPlaylistProgressBarBox.Value = 0;

                        windowsMediaPlayer.URL = choosenPL.Songs[playlistIndex].SongFile;
                        formatProgressBar = choosenPL.Songs[playlistIndex].Format;
                        PlayPlaylistProgressBarBox.Maximum = (int)(choosenPL.Songs[playlistIndex].Duration * 60);

                        PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                        SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                        ProgressTimer.Start();
                        windowsMediaPlayer.Ctlcontrols.play();
                    }
                    else if (choosenPL.Songs[playlistIndex].Format == ".wav")
                    {
                        PlayPlaylistMessageBox.Clear();
                        PlaySongProgressBar.Value = 0;
                        PlaySongTimerTextBox.ResetText();
                        soundPlayer.SoundLocation = choosenPL.Songs[playlistIndex].SongFile;
                        soundPlayer.Play();
                        durationWav = (choosenPL.Songs[playlistIndex].Duration*60);
                        ticks = 0;
                        TimerWav.Start();

                        PlayPlaylistMessageBox.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);
                        SearchPlayingLabel.AppendText("Playlist playing: " + choosenPL.Songs[playlistIndex].Name + choosenPL.Songs[playlistIndex].Format);

                    }
                }
            }
            else if (choosenPL == null)
            {
                soundPlayer.Stop();
                windowsMediaPlayer.Ctlcontrols.stop();

                if (PlayPlaylistMultTypeTextBox.Text.Contains("Song"))
                {
                    List<string> choosenPLPers = ReturnSearchedMult(ProfileDomainUp.Text, "Song", null);
                    int playlistIndex = random.Next(choosenPLPers.Count());
                    if (PlayPlaylistMultTypeTextBox.Text.Contains("Favorite"))
                    {
                        choosenPLPers = ReturnLikeMult(ProfileDomainUp.Text, "Song", null);
                        playlistIndex = random.Next(choosenPLPers.Count());
                    }
                    foreach (Song song in songDataBase)
                    {
                        int cantBadWords = 0;
                        foreach (string badWord in badWords)
                        {
                            if (song.Lyrics.Contains(badWord))
                            {
                                cantBadWords++;
                                break;
                            }
                        }
                        if (cantBadWords != 0 && int.Parse(infoProfile[3]) < 16)
                        {
                            PlayPlaylistMessageBox.AppendText("ERROR[!] Age restriction");
                            break;
                        }
                        else
                        {
                            if (choosenPLPers[playlistIndex].Contains(song.SongFile) == true)
                            {
                                if (song.SongFile.Contains(".mp3"))
                                {
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongProgressBar.Value = 0;
                                    PlaySongTimerTextBox.Clear();

                                    windowsMediaPlayer.URL = song.SongFile;
                                    formatProgressBar = song.Format;


                                    PlayPlaylistMessageBox.AppendText("Playlist playing: " + song.Name + song.Format);
                                    SearchPlayingLabel.AppendText("Playlist playing: " + song.Name + song.Format);
                                    ProgressTimer.Start();
                                    windowsMediaPlayer.Ctlcontrols.play();
                                    break;
                                }
                                else if (song.SongFile.Contains(".wav"))
                                {
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongProgressBar.Value = 0;
                                    PlaySongTimerTextBox.ResetText();
                                    string file = song.SongFile;
                                    soundPlayer.SoundLocation = file;
                                    soundPlayer.Play();
                                    durationWav = (song.Duration*60);
                                    ticks = 0;
                                    TimerWav.Start();

                                   

                                    PlayPlaylistMessageBox.AppendText("Playlist playing: " + song.Name + song.Format);
                                    SearchPlayingLabel.AppendText("Playlist playing: " + song.Name + song.Format);
                                    break;
                                }
                                playlistIndex++;
                            }

                        }

                    }
                }
                if (PlayPlaylistMultTypeTextBox.Text.Contains("Video"))
                {
                    foreach (Video video in videoDataBase)
                    {
                        List<string> choosenPLPers = ReturnSearchedMult(ProfileDomainUp.Text, null, "Video");
                        int playlistIndex = random.Next(choosenPLPers.Count());
                        if (int.Parse(infoProfile[3]) < int.Parse(choosenPL.Videos[playlistIndex].Category))
                        {
                            PlayPlaylistMessageBox.AppendText("ERROR[!] Age restriction");
                            break;
                        }
                        else
                        {
                            if (PlayPlaylistMultTypeTextBox.Text.Contains("Favorite"))
                            {
                                choosenPLPers = ReturnLikeMult(ProfileDomainUp.Text, null, "Video");
                                playlistIndex = random.Next(choosenPLPers.Count());
                            }
                            if (choosenPLPers[playlistIndex].Contains(video.FileName) == true)
                            {
                                if (video.FileName.Contains(".mp4"))
                                {
                                    wmpVideo.Ctlcontrols.stop();
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongTimerTextBox.Clear();
                                    wmpVideo.URL = video.FileName;

                                    PlayVideoPanel.BringToFront();
                                    wmpVideo.Ctlcontrols.play();
                                    break;
                                }
                                else if (video.FileName.Contains(".mov"))
                                {
                                    wmpVideo.Ctlcontrols.stop();
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongTimerTextBox.Clear();
                                    wmpVideo.URL = video.FileName;

                                    PlayVideoPanel.BringToFront();
                                    wmpVideo.Ctlcontrols.play();
                                    break;
                                }
                                else if (video.FileName.Contains(".avi"))
                                {
                                    wmpVideo.Ctlcontrols.stop();
                                    PlayPlaylistMessageBox.Clear();
                                    PlaySongTimerTextBox.Clear();
                                    wmpVideo.URL = video.FileName;

                                    PlayVideoPanel.BringToFront();
                                    wmpVideo.Ctlcontrols.play();
                                    break;
                                }
                                playlistIndex++;
                            }
                        }
                    }
                }
            }
            else if (choosenPL != null && choosenPL.Videos.Count() != 0)
            {
                int playlistIndex = random.Next(choosenPL.Videos.Count());
                int aux = 0;
                if (int.Parse(infoProfile[3]) < choosenPL.Videos[playlistIndex].Ranking)
                {
                    aux++;
                }
                if (aux != 0) PlayPlaylistMessageBox.AppendText("ERROR[!] Age restriction");
                else
                {
                    if (choosenPL.Videos[playlistIndex].Format == ".mp4")
                    {
                        wmpVideo.Ctlcontrols.stop();
                        wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;
                        PlayVideoPanel.BringToFront();
                        wmpVideo.Ctlcontrols.play();

                    }
                    else if (choosenPL.Videos[playlistIndex].Format == ".mov")
                    {

                        wmpVideo.Ctlcontrols.stop();
                        wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;
                        PlayVideoPanel.BringToFront();
                        wmpVideo.Ctlcontrols.play();

                    }
                    else if (choosenPL.Videos[playlistIndex].Format == ".avi")
                    {
                        wmpVideo.Ctlcontrols.stop();
                        wmpVideo.URL = choosenPL.Videos[playlistIndex].FileName;
                        PlayVideoPanel.BringToFront();
                        wmpVideo.Ctlcontrols.play();
                    }
                }

            }
            PlayPlaylistMessageBox.Clear();
        }
        private void PlayPlaylistPlayButton_Click(object sender, EventArgs e)
        {
            soundPlayer.Play();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            string ex = PlayPlaylistShowMultimedia.Text;
            foreach (Song song in songDataBase)
            {
                if (PlayPlaylistShowMultimedia.Text.Contains("Song:") && song.Format == ".mp3")
                {
                    windowsMediaPlayer.Ctlcontrols.play();
                    formatProgressBar = song.Format;
                    ProgressTimer.Start();
                    break;
                }
                else if (PlayPlaylistShowMultimedia.Text.Contains("Song:") && song.Format == ".wav")
                {
                    formatProgressBar = song.Format;
                    durationWav = (song.Duration*60);
                    ticks = 0;
                    TimerWav.Start();
                }
            }
        }

        private void PlayPlaylistPauseButton_Click(object sender, EventArgs e)
        {
            soundPlayer.Stop();
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            string ex = PlayPlaylistShowMultimedia.Text;
            foreach (Song song in songDataBase)
            {
                if (PlayPlaylistShowMultimedia.Text.Contains("Song:") && song.Format == ".mp3")
                {
                    windowsMediaPlayer.Ctlcontrols.pause();
                    DurationTimer.Stop();
                    break;
                }
                else if (PlayPlaylistShowMultimedia.Text.Contains("Song:") && song.Format == ".wav")
                {
                    DurationTimer.Stop();
                }
            }
        }

        private void PlayPlaylistPreviousButton_Click(object sender, EventArgs e)
        {
            string[] infoPlName = PlayPlaylistLabel.Text.Split(':');
            string plName = infoPlName[1];
            int indexPl = 0;
            List<PlayList> allPl;
            if (PlayPlaylistLabel.Text.Contains("X") == false)
            {


                if (PlayPlaylistIsPrivate.Text.Contains("private"))
                {
                    allPl = GetPrivPlaylist();
                }
                else
                {
                    allPl = OnDisplayPlaylistsGlobalPlaylist_Click();
                }

                for (int i = 0; i < allPl.Count(); i++)
                {
                    if (allPl[i].NamePlayList != "" && plName.Contains(allPl[i].NamePlayList))
                    {
                        indexPl = i;
                        break;
                    }
                }

                if (allPl[indexPl].Format == ".mp3" || allPl[indexPl].Format == ".wav")
                {
                    string[] infoSong = PlayPlaylistShowMultimedia.Text.Split(':');
                    string nameSong = infoSong[1];
                    string artistSong = infoSong[3];
                    int previousSong = 1;

                    Song songP = OnSkipOrPreviousSongButton_Clicked(nameSong, artistSong, previousSong, allPl[indexPl], queueListSongs, 0);

                    if (songP != null)
                    {

                        PlayPlaylistMessageBox.Clear();
                        if (songP.Format == ".mp3")
                        {
                            windowsMediaPlayer.Ctlcontrols.stop();
                            soundPlayer.Stop();
                            windowsMediaPlayer.URL = songP.SongFile;
                            windowsMediaPlayer.Ctlcontrols.play();
                        }
                        else if (songP.Format == ".wav")
                        {
                            windowsMediaPlayer.Ctlcontrols.stop();
                            soundPlayer.Stop();
                            soundPlayer.SoundLocation = songP.SongFile;
                            soundPlayer.Play();

                        }

                        PlayPlaylistMessageBox.AppendText("Song playing: " + songP.Name + songP.Format);
                        PlayPlaylistShowMultimedia.UpButton();
                    }
                    else
                    {
                        PlayPlaylistMessageBox.Clear();
                        PlayPlaylistMessageBox.AppendText("ERROR[!] ~Song wasn't previoused!");
                    }
                }
                else if (allPl[indexPl].Format == ".mp4" || allPl[indexPl].Format == ".avi" || allPl[indexPl].Format == ".mov")
                {
                    string[] infoVideo = SearchSearchResultsDomainUp.Text.Split(':');
                    string nameVideo = infoVideo[1];
                    string nameActor = infoVideo[3];
                    int previous = 1;

                    Video video = OnSkipOrPreviousVideoButton_Click(nameVideo, nameActor, previous, allPl[indexPl], queueListSongs, 0);

                    if (video != null)
                    {
                        PlayVideoMessageAlertTextBox.Clear();

                        wmpVideo.Ctlcontrols.stop();
                        wmpVideo.URL = video.FileName;
                        wmpVideo.Ctlcontrols.play();

                        PlayPlaylistMessageBox.AppendText("Video Playing: " + video.Name + video.Format);
                        PlayPlaylistShowMultimedia.UpButton();
                    }
                    else
                    {
                        PlayPlaylistMessageBox.Clear();
                        PlayPlaylistMessageBox.AppendText("Video wasn't previoused!");
                    }
                }
            }
            else if (PlayPlaylistLabel.Text.Contains("X"))
            {
                int index = PlayPlaylistShowMultimedia.SelectedIndex;
                Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
                List<string> favSongPls = new List<string>();
                List<string> favVideoPls = new List<string>();
                List<string> persSongList = new List<string>();
                List<string> persVideoList = new List<string>();
                List<Song> songDataBase = OnSearchSongButton_Click();
                List<Video> videoDataBase = OnSearchVideoButton_Click();
                string contains = PlayPlaylistMultTypeTextBox.Text;

                if (contains.Contains("Song Favorite"))
                {
                    favSongPls = ReturnLikeMult(profile.ProfileName, "Song", null);

                    foreach (Song song in songDataBase)
                    {
                        if (index == 0)
                        {
                            index = favSongPls.Count();
                        }


                        if (song.SongFile == favSongPls[index - 1])
                        {
                            PlayPlaylistMessageBox.Clear();
                            if (song.Format == ".mp3")
                            {
                                windowsMediaPlayer.Ctlcontrols.stop();
                                soundPlayer.Stop();
                                windowsMediaPlayer.URL = song.SongFile;
                                windowsMediaPlayer.Ctlcontrols.play();
                            }
                            else if (song.Format == ".wav")
                            {
                                windowsMediaPlayer.Ctlcontrols.stop();
                                soundPlayer.Stop();
                                soundPlayer.SoundLocation = song.SongFile;
                                soundPlayer.Play();
                            }

                            PlayPlaylistMessageBox.AppendText("Song playing: " + song.Name + song.Format);
                            PlayPlaylistShowMultimedia.DownButton();
                        }

                    }
                }

                else if (PlayPlaylistMultTypeTextBox.Text.Contains("Video Favorite"))
                {
                    favVideoPls = ReturnLikeMult(profile.ProfileName, null, "Video");
                    foreach (Video video in videoDataBase)
                    {
                        if (index == 0)
                        {
                            index = favVideoPls.Count();
                        }

                        if (video.FileName == favVideoPls[index - 1])
                        {
                            PlayPlaylistMessageBox.Clear();

                            wmpVideo.Ctlcontrols.stop();
                            wmpVideo.URL = video.FileName;
                            wmpVideo.Ctlcontrols.play();

                            PlayPlaylistMessageBox.AppendText("Video Playing: " + video.Name + video.Format);
                            PlayPlaylistShowMultimedia.DownButton();
                        }
                        else
                        {
                            PlayPlaylistMessageBox.Clear();
                            PlayPlaylistMessageBox.AppendText("Video wasn't Skipped!");
                        }

                    }
                }
                else if (PlayPlaylistMultTypeTextBox.Text.Contains("Song Preference"))
                {
                    persSongList = ReturnSearchedMult(ProfileDomainUp.Text, "Song", null);
                    foreach (Song song in songDataBase)
                    {
                        if (index == 0)
                        {
                            index = persSongList.Count();
                        }

                        if (song.SongFile == persSongList[index - 1])
                        {
                            PlayPlaylistMessageBox.Clear();
                            if (song.Format == ".mp3")
                            {
                                windowsMediaPlayer.Ctlcontrols.stop();
                                soundPlayer.Stop();
                                windowsMediaPlayer.URL = song.SongFile;
                                windowsMediaPlayer.Ctlcontrols.play();
                            }
                            else if (song.Format == ".wav")
                            {
                                windowsMediaPlayer.Ctlcontrols.stop();
                                soundPlayer.Stop();
                                soundPlayer.SoundLocation = song.SongFile;
                                soundPlayer.Play();
                            }

                            PlayPlaylistMessageBox.AppendText("Song playing: " + song.Name + song.Format);
                            PlayPlaylistShowMultimedia.DownButton();
                        }

                    }
                }
                else if (PlayPlaylistMultTypeTextBox.Text.Contains("Video Preference"))
                {
                    persVideoList = ReturnSearchedMult(ProfileDomainUp.Text, "Video", null);
                    foreach (Video video in videoDataBase)
                    {
                        if (index == 0)
                        {
                            index = persVideoList.Count();
                        }


                        if (video.FileName == persVideoList[index - 1])
                        {
                            PlayPlaylistMessageBox.Clear();

                            wmpVideo.Ctlcontrols.stop();
                            wmpVideo.URL = video.FileName;
                            wmpVideo.Ctlcontrols.play();

                            PlayPlaylistMessageBox.AppendText("Video Playing: " + video.Name + video.Format);
                            PlayPlaylistShowMultimedia.DownButton();
                        }
                        else
                        {
                            PlayPlaylistMessageBox.Clear();
                            PlayPlaylistMessageBox.AppendText("Video wasn't Skipped!");
                        }

                    }
                }

            }
        }

        private void PlayPlaylistSkipButton_Click(object sender, EventArgs e)
        {

            string[] infoPlName = PlayPlaylistLabel.Text.Split(':');
            if (PlayPlaylistLabel.Text.Contains("X") == false)
            {
                string plName = infoPlName[1];
                int indexPl = 0;
                List<PlayList> allPl;
                if (PlayPlaylistIsPrivate.Text.Contains("private"))
                {
                    allPl = GetPrivPlaylist();
                }
                else
                {
                    allPl = OnDisplayPlaylistsGlobalPlaylist_Click();
                }

                for (int i = 0; i < allPl.Count(); i++)
                {
                    if (allPl[i].NamePlayList != "" && plName.Contains(allPl[i].NamePlayList))
                    {
                        indexPl = i;
                        break;
                    }
                }

                if (allPl[indexPl].Format == ".mp3" || allPl[indexPl].Format == ".wav")
                {
                    string[] infoSong = PlayPlaylistShowMultimedia.Text.Split(':');
                    string nameSong = infoSong[1];
                    string artistSong = infoSong[3];
                    int skipSong = 0;

                    Song songS = OnSkipOrPreviousSongButton_Clicked(nameSong, artistSong, skipSong, allPl[indexPl], queueListSongs, 0);

                    if (songS != null)
                    {

                        PlayPlaylistMessageBox.Clear();
                        if (songS.Format == ".mp3")
                        {
                            windowsMediaPlayer.Ctlcontrols.stop();
                            soundPlayer.Stop();
                            windowsMediaPlayer.URL = songS.SongFile;
                            windowsMediaPlayer.Ctlcontrols.play();
                        }
                        else if (songS.Format == ".wav")
                        {
                            windowsMediaPlayer.Ctlcontrols.stop();
                            soundPlayer.Stop();
                            soundPlayer.SoundLocation = songS.SongFile;
                            soundPlayer.Play();
                        }

                        PlayPlaylistMessageBox.AppendText("Song playing: " + songS.Name + songS.Format);
                        PlayPlaylistShowMultimedia.DownButton();
                    }
                    else
                    {
                        PlayPlaylistMessageBox.Clear();
                        PlayPlaylistMessageBox.AppendText("ERROR[!] ~Song wasn't skipped!");
                    }
                }
                else if (allPl[indexPl].Format == ".mp4" || allPl[indexPl].Format == ".avi" || allPl[indexPl].Format == ".mov")
                {
                    string[] infoVideo = SearchSearchResultsDomainUp.Text.Split(':');
                    string nameVideo = infoVideo[1];
                    string nameActor = infoVideo[3];
                    int previous = 0;

                    Video video = OnSkipOrPreviousVideoButton_Click(nameVideo, nameActor, previous, allPl[indexPl], queueListSongs, 0);

                    if (video != null)
                    {
                        PlayPlaylistMessageBox.Clear();

                        wmpVideo.Ctlcontrols.stop();
                        wmpVideo.URL = video.FileName;
                        wmpVideo.Ctlcontrols.play();

                        PlayPlaylistMessageBox.AppendText("Video Playing: " + video.Name + video.Format);
                        PlayPlaylistShowMultimedia.DownButton();
                    }
                    else
                    {
                        PlayPlaylistMessageBox.Clear();
                        PlayPlaylistMessageBox.AppendText("Video wasn't Skipped!");
                    }
                }
            }
            else if(PlayPlaylistLabel.Text.Contains("X"))
            {
                int index = PlayPlaylistShowMultimedia.SelectedIndex;
                Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
                List<string> favSongPls = new List<string>();
                List<string> favVideoPls = new List<string>();
                List<string> persSongList = new List<string>();
                List<string> persVideoList = new List<string>();
                List<Song> songDataBase = OnSearchSongButton_Click();
                List<Video> videoDataBase = OnSearchVideoButton_Click();
                string contains = PlayPlaylistMultTypeTextBox.Text;

                if (contains.Contains("Song Favorite"))
                {
                    favSongPls = ReturnLikeMult(profile.ProfileName, "Song", null);

                    foreach (Song song in songDataBase)
                    {
                        if (index >= favSongPls.Count() - 1)
                        {
                            index = -1;
                        }


                        if (song.SongFile == favSongPls[index + 1])
                        {
                            PlayPlaylistMessageBox.Clear();
                            if (song.Format == ".mp3")
                            {
                                windowsMediaPlayer.Ctlcontrols.stop();
                                soundPlayer.Stop();
                                windowsMediaPlayer.URL = song.SongFile;
                                windowsMediaPlayer.Ctlcontrols.play();
                            }
                            else if (song.Format == ".wav")
                            {
                                windowsMediaPlayer.Ctlcontrols.stop();
                                soundPlayer.Stop();
                                soundPlayer.SoundLocation = song.SongFile;
                                soundPlayer.Play();
                            }

                            PlayPlaylistMessageBox.AppendText("Song playing: " + song.Name + song.Format);
                            PlayPlaylistShowMultimedia.DownButton();
                        }

                    }
                }

                else if (PlayPlaylistMultTypeTextBox.Text.Contains("Video Favorite"))
                {
                    favVideoPls = ReturnLikeMult(profile.ProfileName, null, "Video");
                    foreach (Video video in videoDataBase)
                    {
                        if (index >= favVideoPls.Count() - 1)
                        {
                            index = -1;
                        }

                        if (video.FileName == favVideoPls[index + 1])
                        {
                            PlayPlaylistMessageBox.Clear();

                            wmpVideo.Ctlcontrols.stop();
                            wmpVideo.URL = video.FileName;
                            wmpVideo.Ctlcontrols.play();

                            PlayPlaylistMessageBox.AppendText("Video Playing: " + video.Name + video.Format);
                            PlayPlaylistShowMultimedia.DownButton();
                        }
                        else
                        {
                            PlayPlaylistMessageBox.Clear();
                            PlayPlaylistMessageBox.AppendText("Video wasn't Skipped!");
                        }

                    }
                }
                else if (PlayPlaylistMultTypeTextBox.Text.Contains("Song Preference"))
                {
                    persSongList = ReturnSearchedMult(ProfileDomainUp.Text, "Song", null);
                    foreach (Song song in songDataBase)
                    {
                        if (index >= persSongList.Count() - 1)
                        {
                            index = -1;
                        }

                        if (song.SongFile == persSongList[index + 1])
                        {
                            PlayPlaylistMessageBox.Clear();
                            if (song.Format == ".mp3")
                            {
                                windowsMediaPlayer.Ctlcontrols.stop();
                                soundPlayer.Stop();
                                windowsMediaPlayer.URL = song.SongFile;
                                windowsMediaPlayer.Ctlcontrols.play();
                            }
                            else if (song.Format == ".wav")
                            {
                                windowsMediaPlayer.Ctlcontrols.stop();
                                soundPlayer.Stop();
                                soundPlayer.SoundLocation = song.SongFile;
                                soundPlayer.Play();
                            }

                            PlayPlaylistMessageBox.AppendText("Song playing: " + song.Name + song.Format);
                            PlayPlaylistShowMultimedia.DownButton();
                        }

                    }
                }
                else if (PlayPlaylistMultTypeTextBox.Text.Contains("Video Preference"))
                {
                    persVideoList = ReturnSearchedMult(ProfileDomainUp.Text, "Video", null);
                    foreach (Video video in videoDataBase)
                    {
                        if(index >= persVideoList.Count() - 1)
                        {
                            index = -1;
                        }


                        if (video.FileName == persVideoList[index + 1])
                        {
                            PlayPlaylistMessageBox.Clear();

                            wmpVideo.Ctlcontrols.stop();
                            wmpVideo.URL = video.FileName;
                            wmpVideo.Ctlcontrols.play();

                            PlayPlaylistMessageBox.AppendText("Video Playing: " + video.Name + video.Format);
                            PlayPlaylistShowMultimedia.DownButton();
                        }
                        else
                        {
                            PlayPlaylistMessageBox.Clear();
                            PlayPlaylistMessageBox.AppendText("Video wasn't Skipped!");
                        }

                    }
                }
                
            }
        }
        //ONEVENT

        //GO BACK/CLOSE
        private void PlayPlaylistGoBackButton_Click(object sender, EventArgs e)
        {
            
        }
        private void PlayPlaylistGoBackButton_Click_1(object sender, EventArgs e)
        {
            PlayerMultPanel.Visible = true;
            PlayPlaylistShowMultimedia.ResetText();
            SearchPlayingLabel.Clear();
            windowsMediaPlayer.Ctlcontrols.stop();
            soundPlayer.Stop();
            PlayPlaylistLabel.Text = "Playlist";
            SearchSearchTextBox.Text = "Search Songs,Video, Playlists or Users";
            SearchSearchResultsDomainUp.Visible = false;
            SearchPlayingPanel.Visible = true;
            SearchSearchResultsDomainUp.Text = "Searched Results:";
            SearchPanel.BringToFront();
            SearchSearchResultsDomainUp.ResetText();
            //Borra el domain up de play playlist
            int cont1 = 0;
            foreach (object searched in PlayPlaylistShowMultimedia.Items)
            {
                cont1++;
            }
            for (int i = 0; i < cont1; cont1--)
            {
                PlayPlaylistShowMultimedia.Items.RemoveAt(cont1 - 1);
            }
            //Borra el domain up del search
            int cont = 0;
            foreach (object searched in SearchSearchResultsDomainUp.Items)
            {
                cont++;
            }
            for (int i = 0; i < cont; cont--)
            {
                SearchSearchResultsDomainUp.Items.RemoveAt(cont - 1);
            }
        }
        //-------------------------------------------------------------------------------------------




        /*---------------------------------------------------!AS----------------------------------------------------- */


        //<<!ADD SHOW PANEL>>
        //-------------------------------------------------------------------------------------------

        private void AddShowAddSongButton_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            if (profile.ProfileType != "viewer")
            {
                CreateSongPanel.BringToFront();
            }
            else
            {
                AddShowInvalidCredentialsLabel.Text = "You don´t have permission to create multimedia";
                Thread.Sleep(1000);
                AddShowInvalidCredentialsLabel.ResetText();
            }

        }

        private void AddShowAddVideoButton_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            if (profile.ProfileType != "viewer")
            {
                CreateVideoPanel.BringToFront();
            }
            else
            {
                AddShowInvalidCredentialsLabel.Text = "You don´t have permission to create multimedia";
                Thread.Sleep(1000);
                AddShowInvalidCredentialsLabel.ResetText();
            }
        }

        private void AddShowAddPlaylistButton_Click(object sender, EventArgs e)
        {
            CreatePlaylistNameTextBox.Clear();
            CreatePlaylistImageTextBox.Clear();
            CreatePlaylistPrivacyCheckBox.CheckState = CheckState.Unchecked;
            CreatePlaylistImageTextBox.Clear();

            CreatePlaylistInvalidCredentialstextBox.Clear();
            if (CreatePlaylistFormatDomainUp.SelectedIndex != -1)
            {
                int cont = 0;
                if (SearchSearchResultsDomainUp.SelectedIndex != -1)
                {
                    foreach (object searched in CreatePlaylistFormatDomainUp.Items)
                    {
                        cont++;
                    }
                    for (int i = 0; i < cont; cont--)
                    {
                        CreatePlaylistFormatDomainUp.Items.RemoveAt(cont - 1);
                    }
                }
            }

            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            if (profile.ProfileType != "viewer")
            {
                CreatePlaylistPanel.BringToFront();
            }
            else
            {
                AddShowInvalidCredentialsLabel.Text = "You don´t have permission to create multimedia";
                Thread.Sleep(1000);
                AddShowInvalidCredentialsLabel.ResetText();
            }
        }
        //ONEVENT

        //GO BACK/CLOSE

        private void AddShowGoBackButton_Click(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------




        /*--------------------------------------------------!!A----------------------------------------------------- */

        //<<!CREATE SONG PANEL>>
        //-------------------------------------------------------------------------------------------
        private void CreateSongCreateSongButton_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileName, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            if (profile.ProfileType != "viewer")
            {
                string songName = CreateSongNameTextBox.Text;
                string songArtist = CreateSongArtistTextBox.Text;
                string songAlbum = CreateSongAlbumTextBox.Text;
                string songDiscography = CreateSongDiscographyTextBox.Text;
                string songGender = CreateSongGenderTextBox.Text;
                DateTime songPublishDate = CreateSongPublishDateTime.Value;
                string songStudio = CreateSongStudioTextBox.Text;
                double songDuration = double.Parse(CreateSongDurationTextBox.Text);
                string songFormat = CreateSongFormatTextBox.Text;
                string songLyricsSource = CreateSongLyricsTextBox.Text;
                string songLyrics = songLyricsSource.Split('\\')[songLyricsSource.Split('\\').Length - 1];
                string songFileSource = CreateSongSongFileTextBox.Text;
                string songFile = songFileSource.Split('\\')[songFileSource.Split('\\').Length - 1];
                string songPicSource = CreateSongImageTextBox.Text;
                string songPicFile = songPicSource.Split('\\')[songPicSource.Split('\\').Length - 1];

                if (File.Exists(songFile) == false)
                {
                    OnCreateSongCreateSongButton_Click(songName, songArtist, songAlbum, songDiscography, songGender, songPublishDate, songStudio, songDuration, songFormat, songLyrics, songLyricsSource, songFileSource, songFile, songPicSource, songPicFile);
                    DisplayStartPanel.BringToFront();
                    List<Song> songDataBase = OnSearchSongButton_Click();
                    AddPlaylistMult_Did(songDataBase[songDataBase.Count - 1], null);

                }
                else
                {
                    CreateSongInvalidCredentialTextBox.AppendText("An Error has ocurred please try again");
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                    CreateSongNameTextBox.Clear();
                    CreateSongArtistTextBox.Clear();
                    CreateSongAlbumTextBox.Clear();
                    CreateSongDiscographyTextBox.Clear();
                    CreateSongGenderTextBox.Clear();
                    CreateSongStudioTextBox.Clear();
                    CreateSongDurationTextBox.Clear();
                    CreateSongFormatTextBox.Clear();
                    CreateSongLyricsTextBox.Clear();
                    CreateSongSongFileTextBox.Clear();
                }

            }
        }
        private void CreateSongSongFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                if (filename.Contains(".mp3") == true || filename.Contains(".wav") == true)
                {
                    CreateSongSongFileTextBox.Text = filename;
                }
                else
                {
                    CreateSongInvalidCredentialTextBox.AppendText("ERROR[!] wrong file format");
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                if (filename.Contains(".srt") == true)
                {
                    CreateSongLyricsTextBox.Text = filename;
                }
                else
                {
                    CreateSongInvalidCredentialTextBox.AppendText("ERROR[!] wrong file format");
                }

            }
        }
        private void CreateSongImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media Files|*.jpg;*.png;*.jpeg";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                CreateSongImageTextBox.Text = filename;
            }
        }
        private void CreateSongCreateSongButton_Click_1(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileName, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            if (profile.ProfileType != "viewer")
            {
                string songName = CreateSongNameTextBox.Text;
                string songArtist = CreateSongArtistTextBox.Text;
                string songAlbum = CreateSongAlbumTextBox.Text;
                string songDiscography = CreateSongDiscographyTextBox.Text;
                string songGender = CreateSongGenderTextBox.Text;
                DateTime songPublishDate = CreateSongPublishDateTime.Value;
                string songStudio = CreateSongStudioTextBox.Text;
                double songDuration = double.Parse(CreateSongDurationTextBox.Text);
                string songFormat = CreateSongFormatTextBox.Text;
                string songLyricsSource = CreateSongLyricsTextBox.Text;
                string songLyrics = songLyricsSource.Split('\\')[songLyricsSource.Split('\\').Length - 1];
                string songFileSource = CreateSongSongFileTextBox.Text;
                string songFile = songFileSource.Split('\\')[songFileSource.Split('\\').Length - 1];
                string songPicSource = CreateSongImageTextBox.Text;
                string songPicFile = songPicSource.Split('\\')[songPicSource.Split('\\').Length - 1];

                if (File.Exists(songFile) == false)
                {
                    OnCreateSongCreateSongButton_Click(songName, songArtist, songAlbum, songDiscography, songGender, songPublishDate, songStudio, songDuration, songFormat, songLyrics, songLyricsSource, songFileSource, songFile, songPicSource, songPicFile);
                    DisplayStartPanel.BringToFront();
                    List<Song> songDataBase = OnSearchSongButton_Click();
                    AddPlaylistMult_Did(songDataBase[songDataBase.Count - 1], null);

                }
                else
                {
                    CreateSongInvalidCredentialTextBox.AppendText("An Error has ocurred please try again");
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                    CreateSongNameTextBox.Clear();
                    CreateSongArtistTextBox.Clear();
                    CreateSongAlbumTextBox.Clear();
                    CreateSongDiscographyTextBox.Clear();
                    CreateSongGenderTextBox.Clear();
                    CreateSongStudioTextBox.Clear();
                    CreateSongDurationTextBox.Clear();
                    CreateSongFormatTextBox.Clear();
                    CreateSongLyricsTextBox.Clear();
                    CreateSongSongFileTextBox.Clear();
                }

            }
        }

        //ONEVENT

        public void OnCreateSongCreateSongButton_Click(string sName, string sArtist, string sAlbum, string sDiscography, string sGender, DateTime sPublishDate, string sStudio, double sDuration, string sFormat, string sLyrics, string sLyricsSource, string sSource, string songFile, string picSource, string picFile)
        {
            if (CreateSongCreateSongButton_Clicked != null)
            {
                bool result = CreateSongCreateSongButton_Clicked(this, new SongEventArgs() { NameText = sName, AlbumText = sAlbum, ArtistText = sArtist, DateText = sPublishDate, DiscographyText = sDiscography, DurationText = sDuration, FormatText = sFormat, GenderText = sGender, LyricsText = sLyrics, StudioText = sStudio, FileDestName = sSource, FileNameText = songFile, FileLyricsSource = sLyricsSource, PicSource = picSource, PicFile = picFile });
                if (!result)
                {
                    CreateSongInvalidCredentialTextBox.AppendText("An Error has ocurred please try again");
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                    CreateSongNameTextBox.Clear();
                    CreateSongArtistTextBox.Clear();
                    CreateSongAlbumTextBox.Clear();
                    CreateSongDiscographyTextBox.Clear();
                    CreateSongGenderTextBox.Clear();
                    CreateSongStudioTextBox.Clear();
                    CreateSongDurationTextBox.Clear();
                    CreateSongFormatTextBox.Clear();
                    CreateSongLyricsTextBox.Clear();
                    CreateSongSongFileTextBox.Clear();

                }
                else
                {
                    CreateSongInvalidCredentialTextBox.AppendText("Song Added to the system");
                    Thread.Sleep(2000);
                    CreateSongNameTextBox.Clear();
                    CreateSongArtistTextBox.Clear();
                    CreateSongAlbumTextBox.Clear();
                    CreateSongDiscographyTextBox.Clear();
                    CreateSongGenderTextBox.Clear();
                    CreateSongStudioTextBox.Clear();
                    CreateSongDurationTextBox.Clear();
                    CreateSongFormatTextBox.Clear();
                    CreateSongLyricsTextBox.Clear();
                    CreateSongSongFileTextBox.Clear();
                }
            }
        }

        //TEXT BOX WATERMARK


        private void CreateSongNameTextBox_Enter(object sender, EventArgs e)
        {
            if(CreateSongNameTextBox.Text == "Safaera")
            {
                CreateSongNameTextBox.Text = "";
                CreateSongNameTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateSongNameTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateSongNameTextBox.Text == "")
            {
                CreateSongNameTextBox.Text = "Safaera";
                CreateSongNameTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateSongArtistTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateSongArtistTextBox.Text == "Bad Bunny")
            {
                CreateSongArtistTextBox.Text = "";
                CreateSongArtistTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateSongArtistTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateSongArtistTextBox.Text == "")
            {
                CreateSongArtistTextBox.Text = "Bad Bunny";
                CreateSongArtistTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateSongAlbumTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateSongAlbumTextBox.Text == "YHLQMDLG")
            {
                CreateSongAlbumTextBox.Text = "";
                CreateSongAlbumTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateSongAlbumTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateSongAlbumTextBox.Text == "")
            {
                CreateSongAlbumTextBox.Text = "YHLQMDLG";
                CreateSongAlbumTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateSongDiscographyTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateSongDiscographyTextBox.Text == "Rimas entertainment LLC")
            {
                CreateSongDiscographyTextBox.Text = "";
                CreateSongDiscographyTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateSongDiscographyTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateSongDiscographyTextBox.Text == "")
            {
                CreateSongDiscographyTextBox.Text = "Rimas entertainment LLC";
                CreateSongDiscographyTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateSongGenderTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateSongGenderTextBox.Text == "Trap")
            {
                CreateSongGenderTextBox.Text = "";
                CreateSongGenderTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateSongGenderTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateSongGenderTextBox.Text == "")
            {
                CreateSongGenderTextBox.Text = "Trap";
                CreateSongGenderTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateSongStudioTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateSongStudioTextBox.Text == "BB Rcds")
            {
                CreateSongStudioTextBox.Text = "";
                CreateSongStudioTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateSongStudioTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateSongStudioTextBox.Text == "")
            {
                CreateSongStudioTextBox.Text = "BB Rcds";
                CreateSongStudioTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateSongDurationTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateSongDurationTextBox.Text == "4,9 (Colocar comma)")
            {
                CreateSongDurationTextBox.Text = "";
                CreateSongDurationTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateSongDurationTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateSongDurationTextBox.Text == "")
            {
                CreateSongDurationTextBox.Text = "4,9 (Colocar comma)";
                CreateSongDurationTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateSongFormatTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateSongFormatTextBox.Text == "(.mp3 || .wav)")
            {
                CreateSongFormatTextBox.Text = "";
                CreateSongFormatTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateSongFormatTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateSongFormatTextBox.Text == "")
            {
                CreateSongFormatTextBox.Text = "(.mp3 || .wav)";
                CreateSongFormatTextBox.ForeColor = Color.DimGray;
            }
        }

        //GO BACK/CLOSE
        private void CreateSongGoBackButton_Click(object sender, EventArgs e)
        {
            
        }
        private void CreateSongGoBackButton_Click_1(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
        }

        //-------------------------------------------------------------------------------------------




        //<<!CREATE VIDEO PANEL>>
        //-------------------------------------------------------------------------------------------
        private void CreateVideoSearcheSub_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                if (filename.Contains(".srt") == true)
                {
                    CreateVideoSubtitlesTextBox.Text = filename;
                }
                else
                {
                    CreateVideoMessageTextBox.AppendText("ERROR[!] wrong file format");
                }
            }
        }
        private void CreateVideoSaveButton_Click(object sender, EventArgs e)
        {
            string videoName = CreateVideoNameTextBox.Text;
            string actors = CreateVideoActorsTextBox.Text;
            string directors = CreateVideoDirectorsTextBox.Text;
            string releaseDate = CreateVideoReleaseDateDateTimePicker.Value.ToShortDateString();
            string videoDimension = CreateVideoDimensionTextBox.Text;
            string videoQuality = CreateVideoQualityTextBox.Text;
            string videoCategory = CreateVideoCategoryTextBox.Text;
            string videoDescription = CreateVideoDescriptionTextBox.Text;
            string videoDuration = CreateVideoDurationTextBox.Text;
            string videoFormat = CreateVideoFormatTextBox.Text;
            string videoFileSource = CreateVideoLoadVideoTextBox.Text;
            string videoFileName = videoFileSource.Split('\\')[videoFileSource.Split('\\').Length - 1];

            string videoSubSource = CreateVideoSubtitlesTextBox.Text;
            string videoSubFile = videoSubSource.Split('\\')[videoSubSource.Split('\\').Length - 1];

            string videoPicSource = CreateVideoImageTextBox.Text;
            string videoPicFile = videoPicSource.Split('\\')[videoPicSource.Split('\\').Length - 1];

            if (File.Exists(videoFileName) == false)
            {
                OnCreateVideoSaveButton_Clicked(videoName, actors, directors, releaseDate, videoDimension, videoQuality, videoCategory, videoDescription, videoDuration, videoFormat, videoSubFile, videoSubSource, videoFileSource, videoFileName, videoPicSource, videoPicFile);
                List<Video> videoDataBase = OnSearchVideoButton_Click();
                AddPlaylistMult_Did(null, videoDataBase[videoDataBase.Count - 1]);
            }
            else
            {
                CreateVideoMessageTextBox.AppendText("ERROR[!] Your file already exists");
                CreateVideoNameTextBox.Clear();
                CreateVideoActorsTextBox.Clear();
                CreateVideoDirectorsTextBox.Clear();
                CreateVideoDimensionTextBox.Clear();
                CreateVideoQualityTextBox.Clear();
                CreateVideoCategoryTextBox.Clear();
                CreateVideoDescriptionTextBox.Clear();
                CreateVideoDurationTextBox.Clear();
                CreateVideoFormatTextBox.Clear();
                CreateVideoSubtitlesTextBox.Clear();
                CreateVideoLoadVideoTextBox.Clear();
                Thread.Sleep(1500);
                CreateVideoMessageTextBox.Clear();
            }

        }
        private void CreateVideoSaveButton_Click_1(object sender, EventArgs e)
        {
            string videoName = CreateVideoNameTextBox.Text;
            string actors = CreateVideoActorsTextBox.Text;
            string directors = CreateVideoDirectorsTextBox.Text;
            string releaseDate = CreateVideoReleaseDateDateTimePicker.Value.ToShortDateString();
            string videoDimension = CreateVideoDimensionTextBox.Text;
            string videoQuality = CreateVideoQualityTextBox.Text;
            string videoCategory = CreateVideoCategoryTextBox.Text;
            string videoDescription = CreateVideoDescriptionTextBox.Text;
            string videoDuration = CreateVideoDurationTextBox.Text;
            string videoFormat = CreateVideoFormatTextBox.Text;
            string videoFileSource = CreateVideoLoadVideoTextBox.Text;
            string videoFileName = videoFileSource.Split('\\')[videoFileSource.Split('\\').Length - 1];

            string videoSubSource = CreateVideoSubtitlesTextBox.Text;
            string videoSubFile = videoSubSource.Split('\\')[videoSubSource.Split('\\').Length - 1];

            string videoPicSource = CreateVideoImageTextBox.Text;
            string videoPicFile = videoPicSource.Split('\\')[videoPicSource.Split('\\').Length - 1];

            if (File.Exists(videoFileName) == false)
            {
                OnCreateVideoSaveButton_Clicked(videoName, actors, directors, releaseDate, videoDimension, videoQuality, videoCategory, videoDescription, videoDuration, videoFormat, videoSubFile, videoSubSource, videoFileSource, videoFileName, videoPicSource, videoPicFile);
                List<Video> videoDataBase = OnSearchVideoButton_Click();
                AddPlaylistMult_Did(null, videoDataBase[videoDataBase.Count - 1]);
            }
            else
            {
                CreateVideoMessageTextBox.AppendText("ERROR[!] Your file already exists");
                CreateVideoNameTextBox.Clear();
                CreateVideoActorsTextBox.Clear();
                CreateVideoDirectorsTextBox.Clear();
                CreateVideoDimensionTextBox.Clear();
                CreateVideoQualityTextBox.Clear();
                CreateVideoCategoryTextBox.Clear();
                CreateVideoDescriptionTextBox.Clear();
                CreateVideoDurationTextBox.Clear();
                CreateVideoFormatTextBox.Clear();
                CreateVideoSubtitlesTextBox.Clear();
                CreateVideoLoadVideoTextBox.Clear();
                Thread.Sleep(1500);
                CreateVideoMessageTextBox.Clear();
            }
        }

        private void CreateVideoLoadVideoButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media Files|*.mp4;*.avi;*.mov";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                CreateVideoLoadVideoTextBox.Text = filename;
            }
        }
        private void PlayVideoPicButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media Files|*.jpg;*.png;*.jpeg*";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                CreateVideoImageTextBox.Text = filename;
            }
        }
        
        //ONEVENT

        public void OnCreateVideoSaveButton_Clicked(string name, string actors, string directors, string releaseDate, string dimension, string quality, string category, string description, string duration, string format, string subtitles, string subSource, string fileDest, string fileName, string imageDest, string imageFile)
        {
            if (CreateVideoSaveButton_Clicked != null)
            {
                bool createVideo = CreateVideoSaveButton_Clicked(this, new VideoEventArgs() { NameText = name, ActorsText = actors, DirectorsText = directors, ReleaseDateText = releaseDate, DimensionText = dimension, Categorytext = category, DescriptionText = description, DurationText = duration, FormatText = format, SubtitlesText = subtitles, VideoSubSource = subSource, FileDestText = fileDest, FileNameText = fileName, QualityText = quality, VideoImageFile = imageFile, VideoImageDest = imageDest });
                if (createVideo)
                {
                    CreateVideoMessageTextBox.AppendText("Video Created succesfully!");
                    CreateVideoNameTextBox.Clear();
                    CreateVideoActorsTextBox.Clear();
                    CreateVideoDirectorsTextBox.Clear();
                    CreateVideoDimensionTextBox.Clear();
                    CreateVideoQualityTextBox.Clear();
                    CreateVideoCategoryTextBox.Clear();
                    CreateVideoDescriptionTextBox.Clear();
                    CreateVideoDurationTextBox.Clear();
                    CreateVideoFormatTextBox.Clear();
                    CreateVideoSubtitlesTextBox.Clear();
                    CreateVideoLoadVideoTextBox.Clear();
                    CreateVideoImageTextBox.Clear();
                    Thread.Sleep(2000);
                    CreateVideoMessageTextBox.Clear();
                    DisplayStartPanel.BringToFront();
                }
                else
                {
                    CreateVideoMessageTextBox.AppendText("ERROR[!] could not create video!");
                    CreateVideoNameTextBox.Clear();
                    CreateVideoActorsTextBox.Clear();
                    CreateVideoDirectorsTextBox.Clear();
                    CreateVideoDimensionTextBox.Clear();
                    CreateVideoQualityTextBox.Clear();
                    CreateVideoCategoryTextBox.Clear();
                    CreateVideoDescriptionTextBox.Clear();
                    CreateVideoDurationTextBox.Clear();
                    CreateVideoFormatTextBox.Clear();
                    CreateVideoSubtitlesTextBox.Clear();
                    CreateVideoLoadVideoTextBox.Clear();
                    CreateVideoImageTextBox.Clear();
                    Thread.Sleep(1500);
                    CreateVideoMessageTextBox.Clear();
                }
            }
        }

        //TEXTBOX WATERMARK
        private void CreateVideoNameTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateVideoNameTextBox.Text == "Cats fighting with swords")
            {
                CreateVideoNameTextBox.Text = "";
                CreateVideoNameTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateVideoNameTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateVideoNameTextBox.Text == "")
            {
                CreateVideoNameTextBox.Text = "Cats fighting with swords";
                CreateVideoNameTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateVideoActorsTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateVideoActorsTextBox.Text == "Cats")
            {
                CreateVideoActorsTextBox.Text = "";
                CreateVideoActorsTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateVideoActorsTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateVideoActorsTextBox.Text == "")
            {
                CreateVideoActorsTextBox.Text = "Cats";
                CreateVideoActorsTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateVideoDirectorsTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateVideoDirectorsTextBox.Text == "Human")
            {
                CreateVideoDirectorsTextBox.Text = "";
                CreateVideoDirectorsTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateVideoDirectorsTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateVideoDirectorsTextBox.Text == "")
            {
                CreateVideoDirectorsTextBox.Text = "Human";
                CreateVideoDirectorsTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateVideoDimensionTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateVideoDimensionTextBox.Text == "1024X768")
            {
                CreateVideoDimensionTextBox.Text = "";
                CreateVideoDimensionTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateVideoDimensionTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateVideoDimensionTextBox.Text == "")
            {
                CreateVideoDimensionTextBox.Text = "1024X768";
                CreateVideoDimensionTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateVideoQualityTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateVideoQualityTextBox.Text == "480")
            {
                CreateVideoQualityTextBox.Text = "";
                CreateVideoQualityTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateVideoQualityTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateVideoQualityTextBox.Text == "")
            {
                CreateVideoQualityTextBox.Text = "480";
                CreateVideoQualityTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateVideoCategoryTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateVideoCategoryTextBox.Text == "0")
            {
                CreateVideoCategoryTextBox.Text = "";
                CreateVideoCategoryTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateVideoCategoryTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateVideoCategoryTextBox.Text == "")
            {
                CreateVideoCategoryTextBox.Text = "0";
                CreateVideoCategoryTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateVideoDescriptionTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateVideoDescriptionTextBox.Text == "My cats are fighting")
            {
                CreateVideoDescriptionTextBox.Text = "";
                CreateVideoDescriptionTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateVideoDescriptionTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateVideoDescriptionTextBox.Text == "")
            {
                CreateVideoDescriptionTextBox.Text = "My cats are fighting";
                CreateVideoDescriptionTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateVideoDurationTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateVideoDurationTextBox.Text == "2,45 (Con comma)")
            {
                CreateVideoDurationTextBox.Text = "";
                CreateVideoDurationTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateVideoDurationTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateVideoDurationTextBox.Text == "")
            {
                CreateVideoDurationTextBox.Text = "2,45 (Con comma)";
                CreateVideoDurationTextBox.ForeColor = Color.DimGray;
            }
        }

        private void CreateVideoFormatTextBox_Enter(object sender, EventArgs e)
        {
            if (CreateVideoFormatTextBox.Text == "(.mp4 || .avi || .mov)")
            {
                CreateVideoFormatTextBox.Text = "";
                CreateVideoFormatTextBox.ForeColor = Color.Black;
            }
        }

        private void CreateVideoFormatTextBox_Leave(object sender, EventArgs e)
        {
            if (CreateVideoFormatTextBox.Text == "")
            {
                CreateVideoFormatTextBox.Text = "(.mp4 || .avi || .mov)";
                CreateVideoFormatTextBox.ForeColor = Color.DimGray;
            }
        }

        //GO BACK/CLOSE
        private void CreateVideoGoBackButton_Click(object sender, EventArgs e)
        {
            AddShowPanel.BringToFront();
        }
        private void CreateVideoGoBackButton_Click_1(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------




        //<<!CREATE PLAYLIST PANEL>>
        //-------------------------------------------------------------------------------------------


        private void CreatePlaylistCreatePlaylistButton_Click(object sender, EventArgs e)
        {
            string playlistName = CreatePlaylistNameTextBox.Text;
            string playlistFormat = CreatePlaylistFormatDomainUp.Text;

            string plPicSource = CreatePlaylistImageTextBox.Text;
            string plPicFile = plPicSource.Split('\\')[plPicSource.Split('\\').Length - 1];

            User playlistCreator = OnLoginButtonClicked(UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            Profile playlistProfileCreator = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            bool playlistPrivacy = CreatePlaylistPrivacyCheckBox.Checked; //True si esta checked
            OnCreatePlaylistCreatePlaylistButton_Click(playlistName, playlistFormat, playlistPrivacy, playlistCreator, playlistProfileCreator, plPicSource, plPicFile);
            OnSearchUserButton_Click();

        }
        private void CreatePlaylistCreatePlaylistButton_Click_1(object sender, EventArgs e)
        {
            string playlistName = CreatePlaylistNameTextBox.Text;
            string playlistFormat = CreatePlaylistFormatDomainUp.Text;

            string plPicSource = CreatePlaylistImageTextBox.Text;
            string plPicFile = plPicSource.Split('\\')[plPicSource.Split('\\').Length - 1];

            User playlistCreator = OnLoginButtonClicked(UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            Profile playlistProfileCreator = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            bool playlistPrivacy = CreatePlaylistPrivacyCheckBox.Checked; //True si esta checked
            OnCreatePlaylistCreatePlaylistButton_Click(playlistName, playlistFormat, playlistPrivacy, playlistCreator, playlistProfileCreator, plPicSource, plPicFile);
            OnSearchUserButton_Click();
        }

        
        private void CreatePlaylistChooseImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media Files|*.jpg;*.png;*.jpeg*";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                CreatePlaylistImageTextBox.Text = filename;
            }
        }
        //ONEVENT

        public void OnCreatePlaylistCreatePlaylistButton_Click(string plName, string plFormat, bool plPrivacy, User plCreator, Profile plProfileCreator, string FileName, string FileSource)
        {
            if (CreatePlaylistCreatePlaylistButton_Clicked != null)
            {
                string result = CreatePlaylistCreatePlaylistButton_Clicked(this, new PlaylistEventArgs() { NameText = plName, CreatorText = plCreator, FormatText = plFormat, PrivacyText = plPrivacy, ProfileCreatorText = plProfileCreator, PicFile = FileName, PicSource = FileSource });

                if (result == null)
                {
                    CreatePlaylistInvalidCredentialstextBox.AppendText("Playlist created succesfully!!");
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                }
                else
                {
                    CreatePlaylistInvalidCredentialstextBox.AppendText("Error[!] " + result);
                    Thread.Sleep(2000);
                    DisplayStartPanel.BringToFront();
                }
            }
        }

        //TEXTBOX WATERMARK
        private void CreatePlaylistNameTextBox_Enter(object sender, EventArgs e)
        {
            if (CreatePlaylistNameTextBox.Text == "Ahoward's Playlist")
            {
                CreatePlaylistNameTextBox.Text = "";
                CreatePlaylistNameTextBox.ForeColor = Color.Black;
            }
        }

        private void CreatePlaylistNameTextBox_Leave(object sender, EventArgs e)
        {
            if (CreatePlaylistNameTextBox.Text == "")
            {
                CreatePlaylistNameTextBox.Text = "Ahoward's Playlist";
                CreatePlaylistNameTextBox.ForeColor = Color.DimGray;
            }
        }

        //GO BACK/CLOSE

        private void CreatePlaylistGoBack_Click(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
            SearchSearchResultsDomainUp.ResetText();
        }
        private void CreatePlaylistGoBack_Click_1(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
            SearchSearchResultsDomainUp.ResetText();
        }
        //-------------------------------------------------------------------------------------------



        /*--------------------------------------------------!!S----------------------------------------------------- */
        /*
          
          
         */



        /*---------------------------------------------------!DP----------------------------------------------------- */


        //<<!DISPLAY PLAYLIST PANEL>>
        //-------------------------------------------------------------------------------------------
        private void DisplayPlaylistsGlobalPlaylist1_Click(object sender, EventArgs e)
        {
            PlayPlaylistMultTypeTextBox.Clear();
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = playlistDataBase[0].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();

                if (result == ex)
                {
                    SearchSearchResultsDomainUp.Text = ex;
                    PlayPlaylistLabel.Text = ("Playlist: " + playList.NamePlayList);
                    if (playList.Format == ".mp3" || playList.Format == ".wav")
                    {
                        foreach (Song song in playList.Songs)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                        }
                    }
                }
            }
            PlayPlaylistMultTypeTextBox.AppendText("Song");
            PlayPlaylistPanel.BringToFront();

        }

        private void DisplayPlaylistsGlobalPlaylist2_Click(object sender, EventArgs e)
        {
           
        }
        private void DisplayPlaylistsGlobalPlaylist3_Click(object sender, EventArgs e)
        {

        }

        private void DisplayPlaylistsGlobalPlaylistSong_Click(object sender, EventArgs e)
        {
            isInPlaylist = true;
            PlayPlaylistMultTypeTextBox.Clear();
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = playlistDataBase[1].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();

                if (result == ex)
                {
                    SearchSearchResultsDomainUp.Text = ex;
                    PlayPlaylistLabel.Text = ("Playlist: " + playList.NamePlayList);
                    if (playList.Format == ".mp3" || playList.Format == ".wav")
                    {
                        foreach (Song song in playList.Songs)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                        }
                    }
                }
            }
            PlayPlaylistMultTypeTextBox.AppendText("Song");
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistsGlobalPlaylistVideo_Click(object sender, EventArgs e)
        {
            PlayPlaylistMultTypeTextBox.Clear();
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = playlistDataBase[2].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();

                if (result == ex)
                {
                    SearchSearchResultsDomainUp.Text = ex;
                    PlayPlaylistLabel.Text = ("Playlist: " + playList.NamePlayList);
                    if (playList.Format == ".mp4" || playList.Format == ".mov" || playList.Format == ".avi")
                    {
                        foreach (Video video in playList.Videos)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(video.SearchedInfoVideo());
                        }
                    }
                }
            }
            PlayPlaylistMultTypeTextBox.AppendText("Video");
            PlayPlaylistPanel.BringToFront();
        }
        
        private void DisplayPlaylistsMoreGlobalPlaylistButton_Click(object sender, EventArgs e)
        {

            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();
            if (playlistDataBase.Count > 3)
            {
                //Traer un panel que muestre las otras Pls Globales...
            }
        }
        private void DisplayPlaylistsFollowedPlaylist1_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = profile.FollowedPlayList[0].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();
                if (result == ex)
                {
                    if (playList.Format == ".mp3" || playList.Format == ".wav")
                    {
                        foreach (Song song in playList.Songs)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                        }
                    }
                }
            }
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistsFollowedPlaylist2_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = profile.FollowedPlayList[1].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();
                if (result == ex)
                {
                    if (playList.Format == ".mp3" || playList.Format == ".wav")
                    {
                        foreach (Song song in playList.Songs)
                        {
                            PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                        }
                    }
                }
            }
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistsFollowedPlaylist3_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            string result = profile.FollowedPlayList[2].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();

                if (playList.Format == ".mp3" || playList.Format == ".wav")
                {
                    foreach (Song song in playList.Songs)
                    {
                        PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                    }
                }
                
            }
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistsMoreFollowedPlaylistButton_Click(object sender, EventArgs e)
        {

        }
        private void DisplayPlaylistCreatedPlaylistImage1_Click(object sender, EventArgs e)
        {
            
        }

        private void DisplayPlaylistCreatedPlaylistImage2_Click(object sender, EventArgs e)
        {
            
        }
        private void DisplayPlaylistsFavoritePlaylistSongs_Click(object sender, EventArgs e)
        {
            isInPlaylist = true;
            PlayPlaylistMultTypeTextBox.Clear();
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            soundPlayer = new SoundPlayer();

            List<Song> songdatabase = OnSearchSongButton_Click();

            List<string> favPls = ReturnLikeMult(profile.ProfileName, "Song", null);
            //string result = profile.CreatedPlaylist[0].DisplayInfoPlayList();

            foreach (Song song in songdatabase)
            {
                if (favPls.Contains(song.SongFile))
                {
                    PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                }

            }
            PlayPlaylistLabel.Text = "Play Playlist: X";
            PlayPlaylistMultTypeTextBox.AppendText("Song Favorite");
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistsFavoritePlaylistVideos_Click(object sender, EventArgs e)
        {
            PlayPlaylistMultTypeTextBox.Clear();
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            soundPlayer = new SoundPlayer();
            List<Video> videodatabase = OnSearchVideoButton_Click();

            List<string> favPls = ReturnLikeMult(profile.ProfileName, null, "Video");
            //string result = profile.CreatedPlaylist[0].DisplayInfoPlayList();

            foreach (Video video in videodatabase)
            {
                if (favPls.Contains(video.FileName))
                {
                    PlayPlaylistShowMultimedia.Items.Add(video.SearchedInfoVideo());
                }

            }
            PlayPlaylistLabel.Text = "Play Playlist: X";
            PlayPlaylistMultTypeTextBox.AppendText("Video Favorite");
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistCreatedPlaylistImage3_Click(object sender, EventArgs e)
        {
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            soundPlayer = new SoundPlayer();
            List<PlayList> playlistDataBase = new List<PlayList>();
            playlistDataBase = OnDisplayPlaylistsGlobalPlaylist_Click();

            //string result = profile.CreatedPlaylist[2].DisplayInfoPlayList();
            foreach (PlayList playList in playlistDataBase)
            {
                string ex = playList.DisplayInfoPlayList();

                if (playList.Format == ".mp3" || playList.Format == ".wav")
                {
                    foreach (Song song in playList.Songs)
                    {
                        PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                    }
                }
                
            }
            PlayPlaylistPanel.BringToFront();
        }
        private void DisplayPlaylistsFavoritePlaylist1_Click(object sender, EventArgs e)
        {
            
        }

        private void DisplayPlaylistsFavoritePlaylist2_Click(object sender, EventArgs e)
        {
            
        }
        private void DisplayPlaylistPrefPlaylistSong_Click(object sender, EventArgs e)
        {
            PlayerMultPanel.Visible = true;
            isInPlaylist = true;
            PlayPlaylistMultTypeTextBox.Clear();
            PlayPlaylistMultTypeTextBox.Clear();
            List<string> persSongList = new List<string>();
            persSongList = ReturnSearchedMult(ProfileDomainUp.Text, "Song", null);
            List<Song> songDataBase = new List<Song>();
            songDataBase = OnSearchSongButton_Click();
            foreach (Song song in songDataBase)
            {
                if (persSongList.Contains(song.SongFile) == true)
                {
                    PlayPlaylistShowMultimedia.Items.Add(song.SearchedInfoSong());
                }
            }
            PlayPlaylistMultTypeTextBox.AppendText("Song Preference");
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistPrefPlaylistVideo_Click(object sender, EventArgs e)
        {
            PlayPlaylistMultTypeTextBox.Clear();
            List<string> persVideoList = new List<string>();
            persVideoList = ReturnSearchedMult(ProfileDomainUp.Text, null, "Video");
            List<Video> videoDataBase = new List<Video>();
            videoDataBase = OnSearchVideoButton_Click();
            foreach (Video video in videoDataBase)
            {
                if (persVideoList.Contains(video.FileName) == true)
                {
                    PlayPlaylistShowMultimedia.Items.Add(video.SearchedInfoVideo());
                }
            }
            PlayPlaylistMultTypeTextBox.AppendText("Video Preference");
            PlayPlaylistPanel.BringToFront();
        }

        private void DisplayPlaylistCreatedPlaylistButton_Click(object sender, EventArgs e)
        {

        }

        //ON EVENT
        public List<PlayList> OnDisplayPlaylistsGlobalPlaylist_Click()
        {
            if (DisplayPlaylistsGlobalPlaylist_Clicked != null)
            {
                List<PlayList> listPlaylist = DisplayPlaylistsGlobalPlaylist_Clicked(this, new PlaylistEventArgs()); //Nose si es necesario darle parametros
                return listPlaylist;
            }
            return null;
        }
        //-------------------------------------------------------------------------------------------


        /*---------------------------------------------------!APS----------------------------------------------------- */


        //<<!ACCOUNT PROFILE SETTINGS PANEL>>
        //-------------------------------------------------------------------------------------------
        private void UserSettinChangeUsernameButton_Click(object sender, EventArgs e)
        {
            
        }

        private void UserSettinChangePasswordButton_Click(object sender, EventArgs e)
        {
            
        }

        private void ProfileSettingsChangeProfileNameButton_Click(object sender, EventArgs e)
        {
            
        }


        private void UserSettinChangeUsernameButton_Click_1(object sender, EventArgs e)
        {
            UserProfilChangeInfoMessageBox.Clear();
            UserProfileChangeInfoInvalidBox.Clear();
            UserProfileChangeInfoPanel.BringToFront();
            UserProfilChangeInfoMessageBox.AppendText("Change Username.");
            UserProfileChangeInfoNewUsernameTextBox.Visible = true;
            UserProfileChangeInfoNewPasswordTextBox.Visible = false;
            UserProfileChangeInfoNewProfilenameTextBox.Visible = false;
            label12.Visible = true;
            label10.Visible = false;
            label11.Visible = false;
        }

        private void UserSettinChangePasswordButton_Click_1(object sender, EventArgs e)
        {
            UserProfilChangeInfoMessageBox.Clear();
            UserProfileChangeInfoInvalidBox.Clear();
            UserProfileChangeInfoPanel.BringToFront();
            UserProfilChangeInfoMessageBox.AppendText("Change Password.");
            UserProfileChangeInfoNewPasswordTextBox.Visible = true;
            UserProfileChangeInfoNewUsernameTextBox.Visible = false;
            UserProfileChangeInfoNewProfilenameTextBox.Visible = false;
            label12.Visible = false;
            label10.Visible = false;
            label11.Visible = true;
        }

        private void AccountSettingAccounTypeChangeButton_Click(object sender, EventArgs e)
        {
            UserProfilChangeInfoMessageBox.Clear();
            UserProfileChangeInfoInvalidBox.Clear();
            UserProfileChangeInfoPanel.BringToFront();
            UserProfilChangeInfoMessageBox.AppendText("Change Accountype.");
            UserProfileChangeInfoNewProfilenameTextBox.Visible = true;
            UserProfileChangeInfoNewUsernameTextBox.Visible = false;
            UserProfileChangeInfoNewPasswordTextBox.Visible = false;
            label10.Visible = true;
            label12.Visible = false;
            label11.Visible = false;
        }

        //ONEVENT

        //GO BACK/CLOSE
        private void UserProfileGoBack_Click(object sender, EventArgs e)
        {
            
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (AccountSettingsFollowerListDomainUp.SelectedIndex != -1)
            {
                int cont = 0;
                foreach (object searched in AccountSettingsFollowerListDomainUp.Items)
                {
                    cont++;
                }
                for (int i = 0; i < cont; cont--)
                {
                    AccountSettingsFollowerListDomainUp.Items.RemoveAt(cont - 1);
                }
            }
            if (AccountSettingsFollowingListDomaiUp.SelectedIndex != -1)
            {
                int cont = 0;
                foreach (object searched in AccountSettingsFollowingListDomaiUp.Items)
                {
                    cont++;
                }
                for (int i = 0; i < cont; cont--)
                {
                    AccountSettingsFollowingListDomaiUp.Items.RemoveAt(cont - 1);
                }
            }
            DisplayStartPanel.BringToFront();
        }

        private void AccountProfileSettingsGoBackButton_Click(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------

        /*---------------------------------------------------!UC----------------------------------------------------- */

        //<<!USER PROFILE INFO CHANGE PANEL>>
        //-------------------------------------------------------------------------------------------


        private void UserProfileChangeInfoConfirmButton_Click(object sender, EventArgs e)
        {
            
        }
        private void UserProfileChangeInfoConfirmButton_Click_1(object sender, EventArgs e)
        {
            UserProfilChangeInfoMessageBox.Clear();
            UserProfileChangeInfoInvalidBox.Clear();
            int wantToChange = 0;
            string changed = null;
            if (UserProfileChangeInfoNewUsernameTextBox.Visible == true)
            {
                wantToChange = 1;
                changed = UserProfileChangeInfoNewUsernameTextBox.Text;
            }
            else if (UserProfileChangeInfoNewPasswordTextBox.Visible == true)
            {
                wantToChange = 2;
                changed = UserProfileChangeInfoNewPasswordTextBox.Text;
            }
            else if (UserProfileChangeInfoNewProfilenameTextBox.Visible == true)
            {
                wantToChange = 3;
                changed = UserProfileChangeInfoNewProfilenameTextBox.Text;
            }
            if (UserProfileChangeInfoUsernameTextBox.Text == UserLogInTextBox.Text)
            {
                UserProfileChangeInfoConfirmButton_Click(UserProfileChangeInfoUsernameTextBox.Text, UserProfileChangeInfoPasswordTextBox.Text, UserProfileChangeInfoProfileNameTextBox.Text, changed, wantToChange);
            }
            else
            {
                UserProfileChangeInfoInvalidBox.Clear();
                UserProfileChangeInfoInvalidBox.AppendText("ERRROR[!] Not your username.");
            }

            Thread.Sleep(2000);

            WelcomePanel.BringToFront();
            UserLogInTextBox.Clear();
            PasswordLogInTextBox.Clear();

            UserProfileChangeInfoPasswordTextBox.Clear();
            UserProfileChangeInfoUsernameTextBox.Clear();
            UserProfileChangeInfoProfileNameTextBox.Clear();

            UserProfileChangeInfoNewPasswordTextBox.Clear();
            UserProfileChangeInfoNewUsernameTextBox.Clear();
            UserProfileChangeInfoNewProfilenameTextBox.Clear();

            UserProfileChangeInfoNewUsernameTextBox.Visible = false;
            UserProfileChangeInfoNewPasswordTextBox.Visible = false;
            UserProfileChangeInfoNewProfilenameTextBox.Visible = false;

            label11.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
        }

        //ON EVENT

        public void UserProfileChangeInfoConfirmButton_Click(string username, string password, string profile, string changed, int want)
        {
            if (UserProfileChangeInfoConfirmButton_Clicked != null)
            {
                string result = UserProfileChangeInfoConfirmButton_Clicked(this, new UserEventArgs() { UsernameText = username, PasswordText = password, ProfilenameText = profile, WantToChangeText = want, ChangedText = changed });
                if (result == null)
                {
                    UserProfileChangeInfoInvalidBox.AppendText("ERROR[!] couldn't change settings");
                    Thread.Sleep(2000);

                }
                else
                {
                    UserProfileChangeInfoInvalidBox.AppendText(result);
                    Thread.Sleep(2000);
                }
            }
        }

        //TEXTBOX WATERMARK
        private void UserProfileChangeInfoNewProfilenameTextBox_Enter(object sender, EventArgs e)
        {
            if(UserProfileChangeInfoNewProfilenameTextBox.Text == "7999 or above")
            {
                UserProfileChangeInfoNewProfilenameTextBox.Text = "";
                UserProfileChangeInfoNewProfilenameTextBox.ForeColor = Color.Black;
            }
        }

        private void UserProfileChangeInfoNewProfilenameTextBox_Leave(object sender, EventArgs e)
        {
            if (UserProfileChangeInfoNewProfilenameTextBox.Text == "")
            {
                UserProfileChangeInfoNewProfilenameTextBox.Text = "7999 or above";
                UserProfileChangeInfoNewProfilenameTextBox.ForeColor = Color.DimGray;
            }
        }

        //GO BACK/CLOSE

        private void UserProfileChangeInfoGoBackButton_Click(object sender, EventArgs e)
        {
            
        }
        private void UserProfileChangeInfoGoBackButton_Click_1(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
            UserProfileChangeInfoPasswordTextBox.Clear();
            UserProfileChangeInfoUsernameTextBox.Clear();
            UserProfileChangeInfoProfileNameTextBox.Clear();
            UserProfilChangeInfoMessageBox.Clear();
            UserProfileChangeInfoInvalidBox.Clear();
        }
        //-------------------------------------------------------------------------------------------


        /*---------------------------------------------------!AM----------------------------------------------------- */
        //<<!ADMIN MENU PANEL>>
        private void AdminMenuEraseUserButton_Click(object sender, EventArgs e)
        {
            

        }

        private void AdminMenuBanUserButton_Click(object sender, EventArgs e)
        {
            
        }

        private void AdminMenuBanUser_Click(object sender, EventArgs e) //Unbanea
        {
            
        }
        private void AdminMenuEraseUserButton_Click_1(object sender, EventArgs e)
        {
            AdminMenuMessageBox.Clear();
            if (AdminMenuAllUsers.SelectedIndex != -1)
            {
                string username = AdminMenuAllUsers.Text;
                int index = AdminMenuAllUsers.SelectedIndex;
                AdminMethods(username, 0);
                if (AdminMenuAllUsers.SelectedIndex != -1)

                    if (AdminMenuAllUsers.SelectedIndex != -1)
                    {
                        AdminMenuAllUsers.Items.RemoveAt(index);
                    }
            }
            else
            {
                AdminMenuMessageBox.AppendText("Please select a valid User");
            }
        }

        private void AdminMenuBanUserButton_Click_1(object sender, EventArgs e)
        {
            AdminMenuMessageBox.Clear();
            if (AdminMenuAllUsers.SelectedIndex != -1)
            {
                string username = AdminMenuAllUsers.Text;
                AdminMethods(username, 1);

            }
            else
            {
                AdminMenuMessageBox.AppendText("Please select a valid User");
            }
        }

        private void AdminMenuBanUser_Click_1(object sender, EventArgs e)
        {
            AdminMenuMessageBox.Clear();
            if (AdminMenuAllUsers.SelectedIndex != -1)
            {
                string username = AdminMenuAllUsers.Text;
                AdminMethods(username, 2);

            }
            else
            {
                AdminMenuMessageBox.AppendText("Please select a valid User");
            }
        }
        //-------------------------------------------------------------------------------------------

        //ONEVENT
        public void AdminMethods(string username, int choice)
        {
            if (AdminMethods_Done != null)
            {
                string result = AdminMethods_Done(this, new UserEventArgs() { UsernameText = username, WantToChangeText = choice });
                if (result != null)
                {
                    AdminMenuMessageBox.AppendText(result);
                }
                else
                {
                    AdminMenuMessageBox.AppendText("ERROR[!]");
                }
            }

        }

        //GO BACK/CLOSE

        private void AdminMenuGoBackButton_Click(object sender, EventArgs e)
        {
            
        }
        private void AdminMenuGoBackButton_Click_1(object sender, EventArgs e)
        {
            AdminMenuMessageBox.Clear();
            if (AdminMenuAllUsers.SelectedIndex != -1)
            {
                int cont = 0;
                foreach (object searched in AdminMenuAllUsers.Items)
                {
                    cont++;
                }
                for (int i = 0; i < cont; cont--)
                {
                    AdminMenuAllUsers.Items.RemoveAt(cont - 1);
                }
            }

            DisplayStartPanel.BringToFront();
        }
        //-------------------------------------------------------------------------------------------





        /*---------------------------------------------------!ON EVENT GENERALES----------------------------------------------------- */

        public List<string> GetSongButton(string sName, string sArtist)
        {
            if (GetSongInformation != null)
            {
                List<string> songInfo = GetSongInformation(this, new SongEventArgs() { NameText = sName, ArtistText = sArtist });
                return songInfo;
            }
            return null;
        }
        public List<List<string>> ReturnAllSongsInfo()
        {
            if (GetAllSongInformation != null)
            {
                List<List<string>> allSongsInfo = GetAllSongInformation(this, new SongEventArgs());
                return allSongsInfo;
            }
            else return null;
        }
        public List<string> GetVideoButton(string vName, string vActors, string vDirectors)
        {
            if (GetVideoInformation != null)
            {
                List<string> videoInfo = GetVideoInformation(this, new VideoEventArgs() { NameText = vName, ActorsText = vActors, DirectorsText = vDirectors });
                return videoInfo;
            }
            return null;
        }
        public List<List<string>> ReturnAllVideosInfo()
        {
            if (GetAllVideosInformation != null)
            {
                List<List<string>> allVideosInfo = GetAllVideosInformation(this, new VideoEventArgs());
                return allVideosInfo;
            }
            else return null;
        }

        public void AddingSearchedMult(string pName, string sFile, string vFile)
        {
            if (AddSearchedMult_Done != null)
            {
                bool result = AddSearchedMult_Done(this, new UserEventArgs() { ProfilenameText = pName, SongFileText = sFile, VideoFileText = vFile });
                if (result)
                {
                    SearchOkMultAddedLabel.ResetText();
                    SearchOkMultAddedLabel.Text = "OK";
                }
            }
        }
        public List<string> ReturnSearchedMult(string pName, string sFile, string vFile)
        {
            List<string> persMultList = new List<string>();
            if (ReturnSearchedMult_Done != null)
            {
                persMultList = ReturnSearchedMult_Done(this, new UserEventArgs() { ProfilenameText = pName, SongFileText = sFile, VideoFileText = vFile });
            }
            return persMultList;
        }
        public List<string> ReturnLikeMult(string pName, string sFile, string vFile)
        {
            List<string> persMultList = new List<string>();
            if (ReturnLikedMult_Done != null)
            {
                persMultList = ReturnLikedMult_Done(this, new UserEventArgs() { ProfilenameText = pName, SongFileText = sFile, VideoFileText = vFile });
            }
            return persMultList;
        }

        public void AddPlaylistMult_Did(Song song, Video video)
        {
            string result = null;

            if (AddPlaylistMult_Done != null)
            {
                result = AddPlaylistMult_Done(this, new PlaylistEventArgs() { SongText = song, VideoText = video });
            }
        }

        public List<Song> OnSearchSongButton_Click()
        {
            if (SearchSongButton_Clicked != null)
            {
                List<Song> songDataBase = SearchSongButton_Clicked(this, new SongEventArgs());
                return songDataBase;
            }
            return null;
        }

        public List<Video> OnSearchVideoButton_Click()
        {
            if (SearchVideoButton_Clicked != null)
            {
                List<Video> videoDataBase = SearchVideoButton_Clicked(this, new VideoEventArgs());
                return videoDataBase;
            }
            return null;
        }

        public List<User> OnSearchUserButton_Click()
        {
            if (SearchUserButton_Clicked != null)
            {
                List<User> userDataBase = SearchUserButton_Clicked(this, new RegisterEventArgs());
                return userDataBase;
            }
            return null;

        }

        public void AddLikedMult(string pName, string sFile, string vFile)
        {
            if (AddedLikedMult != null)
            {
                bool result = AddedLikedMult(this, new UserEventArgs() { ProfilenameText = pName, SongFileText = sFile, VideoFileText = vFile });
                if (result)
                {
                    PlaySongMessageTextBox.Text = "Added to liked multimedia";
                    Thread.Sleep(1000);
                    PlaySongMessageTextBox.ResetText();
                }
            }
        }
        public List<PlayList> GetPrivPlaylist()
        {
            List<PlayList> privatePls = new List<PlayList>();
            if (ReturnPrivatePls != null)
            {
                privatePls = ReturnPrivatePls(this, new PlaylistEventArgs());
            }
            return privatePls;
        }

        private void ProfilesWelcomeTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void SearchInvalidCredentialsTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        public List<string> ReturnInfoSong2(string sName, string sArtist)
        {
            List<string> result = new List<string>();
            if (ReturnSongInfo_Did != null)
            {
                result = ReturnSongInfo_Did(this, new SongEventArgs() { NameText = sName, ArtistText = sArtist });
            }
            return result;
        }

        private void SearchMoreInfoButton_Click(object sender, EventArgs e)
        {
            SearchDisplayMoreMultimediaInfo.Clear();
            SearchDisplayMoreMultimediaInfo.Visible = true;
            string[] infoMult = SearchSearchResultsDomainUp.Text.Split(':');
            List<string> infoMultimedia = new List<string>();

            if(infoMult.Contains("Song"))
            {
                int n = 0; 
                infoMultimedia = ReturnInfoSong2(infoMult[1], infoMult[3]);
                List<string> information = new List<string>() { "Album: ", "Artists: ", "Discography: ", "Gender: ", "Studio: ", 
                                                               "Lyrics File: ", "Song File: ", "Ranking: ","Name: "};
                foreach(string info in infoMultimedia)
                {
                    SearchDisplayMoreMultimediaInfo.AppendText(information[n] + info + "\r\n");
                    n++;
                }
            }
            else if(infoMult.Contains("Video"))
            {
                
            }
        }
        //---------------------------------------------//
        private void PlaySongShareButton_Click(object sender, EventArgs e)
        {
            
        }
        private void PlaySongShareButton_Click_1(object sender, EventArgs e)
        {

        }

        private void PlaySongChooseUserButton_Click(object sender, EventArgs e)
        {
            int cont = 0;
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, PasswordLogInTextBox.Text, UserLogInTextBox.Text);
            if (PlaySongChooseUserDomainUp.SelectedIndex != -1)
            {
                foreach (object searched in PlaySongChooseUserDomainUp.Items)
                {
                    cont++;
                }
                for (int i = 0; i < cont; cont--)
                {
                    PlaySongChooseUserDomainUp.Items.RemoveAt(cont - 1);
                }
                profile.SharedMult = null;
            }
            PlaySongChooseUserDomainUp.Visible = true;
            PlaySongChooseUserButton.Visible = true;

            User user = OnLoginButtonClicked(UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            foreach (String followedUser in user.FollowingList)
            {
                PlaySongChooseUserDomainUp.Items.Add(followedUser);
            }

        }
        private void PlaySongChooseUserButton_Click_1(object sender, EventArgs e)
        {
            string[] songPlaying = PlaySongSongPlaying.Text.Split(':');

            List<string> songInfo = ReturnInfoSong2(songPlaying[0], songPlaying[1]);
            string songFile = songInfo[7];
            List<string> sharedMult = new List<string>() { UserLogInTextBox.Text + "/" + songFile };
            string result = SharedMultSet(PlaySongChooseUserDomainUp.Text, sharedMult);
            if (result != null)
            {
                PlaySongMessageTextBox.AppendText(result);
            }
            Thread.Sleep(1000);
            PlaySongMessageTextBox.Clear();
        }
        //--------Video-----------
        private void PlayVideoShareButton_Click(object sender, EventArgs e)
        {
            
        }
        private void PlayVideoShareButton_Click_1(object sender, EventArgs e)
        {
            int cont = 0;
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, PasswordLogInTextBox.Text, UserLogInTextBox.Text);
            if (PlayVideoChooseUserDomainUp.SelectedIndex != -1)
            {
                foreach (object searched in PlayVideoChooseUserDomainUp.Items)
                {
                    cont++;
                }
                for (int i = 0; i < cont; cont--)
                {
                    PlayVideoChooseUserDomainUp.Items.RemoveAt(cont - 1);
                }
                profile.SharedMult = null;
            }
            PlayVideoChooseUserDomainUp.Visible = true;
            PlayVideoChooseUserButtonFeo.Visible = true;

            User user = OnLoginButtonClicked(UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            foreach (String followedUser in user.FollowingList)
            {
                PlayVideoChooseUserDomainUp.Items.Add(followedUser);
            }
        }

        private void PlayVideoChooseUserButton_Click(object sender, EventArgs e)
        {
            string[] videoPlaying = PlayVideoVideoPlaying.Text.Split(':');

            List<string> videoInfo = GetVideoButton(videoPlaying[0], videoPlaying[1], videoPlaying[2]);
            string videoFile = videoInfo[8];
            List<string> sharedMult = new List<string>() { UserLogInTextBox.Text + "/" + videoFile };
            string result = SharedMultSet(PlayVideoChooseUserDomainUp.Text, sharedMult);
            if (result != null)
            {
                PlayVideoMessageAlertTextBox.AppendText(result);
            }
            Thread.Sleep(1000);
            PlayVideoMessageAlertTextBox.Clear();
        }

        public string SharedMultSet(string userName, List<string> sharedMult)
        {
            string result = null;
            if (SharedMultSetter != null)
            {
                result = SharedMultSetter(this, new UserEventArgs() {UsernameText = userName, SharedMult = sharedMult  });
                
            }
            return result;
            
        }

        public List<string> SharedMultGet(string username)
        {
            List<string> result = new List<string>();
            if(SharedMultGetter != null)
            {
                result = SharedMultGetter(this, new UserEventArgs() {UsernameText = username });
            }
            if(result.Count != 0)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        private void DisplayStartChooseSharedMult_Click(object sender, EventArgs e)
        {
            
        }
        private void DisplayStartChooseSharedMult_Click_1(object sender, EventArgs e)
        {
            int index = DisplayStartNotificationDomainUp.SelectedIndex;

            string mult = DisplayStartMultimediaInfoDomainUp.Items[index].ToString();

            string[] multInfo = mult.Split(':');

            List<Song> songDataBase = OnSearchSongButton_Click();

            List<Video> videoDataBase = OnSearchVideoButton_Click();

            List<string> infoProfile = OnProfilesChooseProfile_Click2(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);

            if (multInfo.Count() == 3) //es video
            {
                PlayVideoSkipButton.Visible = false;
                PlayVideoPreviousButton.Visible = false;
                List<string> videoMVCInfo = GetVideoButton(multInfo[0], multInfo[1], multInfo[2]);
                if (int.Parse(videoMVCInfo[4]) > int.Parse(infoProfile[3]))
                {
                    SearchInvalidCredentialsTextBox.AppendText("Video has age restriction");
                    Thread.Sleep(1000);
                    SearchInvalidCredentialsTextBox.Clear();

                }
                else
                {
                    foreach (Video video in videoDataBase)
                    {
                        string result = SearchSearchResultsDomainUp.Text;
                        if (multInfo[0].Contains(video.Name) && multInfo[1].Contains(video.Actors) && multInfo[2].Contains(video.Directors))
                        {
                            if (video.Image != null)
                            {
                                PlayVideoVideoImageBox.Image = System.Drawing.Image.FromFile(video.Image);
                            }
                            else
                            {
                                PlayVideoVideoImageBox.Image = System.Drawing.Image.FromFile("Logo (1).jpg");
                            }
                            PlayVideoVideoPlaying.AppendText(mult);
                            AddingSearchedMult(ProfileDomainUp.Text, null, video.FileName);
                            PlayVideoPanel.BringToFront();
                            wmpVideo.URL = video.FileName;
                            wmpVideo.Ctlcontrols.play();
                            notification = 1;
                        }
                    }
                }
            }

            else //Es cancion
            {
                List<string> songMVCInfo = GetSongButton(multInfo[0], multInfo[1]);
                PlaySongSkipSongButton.Visible = false;
                PlaySongPreviousSongButton.Visible = false;
                foreach (Song song in songDataBase)
                {
                    int badWordsCount = 0;
                    if (songMVCInfo[5] != null)
                    {
                        string songLyrics = File.ReadAllText(songMVCInfo[5]);
                        foreach (string badWord in badWords)
                        {
                            if (songLyrics.Contains(badWord) == true)
                            {
                                badWordsCount++;
                                break;
                            }
                        }
                    }
                    if (int.Parse(infoProfile[3]) < 16 && badWordsCount != 0)
                    {
                        SearchInvalidCredentialsTextBox.AppendText("Song with explicit content only 16+");
                        Thread.Sleep(1000);
                        SearchInvalidCredentialsTextBox.Clear();
                    }
                    else
                    {

                        if (songMVCInfo[6].Contains(".mp3") == true)
                        {
                            string result = SearchSearchResultsDomainUp.Text;
                            string songInfo = song.SearchedInfoSong();
                            if (multInfo[0].Contains(song.Name) && multInfo[1].Contains(song.Artist))
                            {
                                if (song.ImageFile != null)
                                {
                                    PlaySongImageBoxImage.Image = System.Drawing.Image.FromFile(song.ImageFile);
                                }
                                else
                                {
                                    PlaySongImageBoxImage.Image = System.Drawing.Image.FromFile("Logo (1).jpg");
                                }
                                PlaySongSongPlaying.AppendText(mult);
                                AddingSearchedMult(ProfileDomainUp.Text, song.SongFile, null);
                                Thread.Sleep(2000);
                                PlayerPlayingLabel.Clear();
                                SearchPlayingLabel.Clear();
                                PlaySongProgressBar.Value = 0;
                                PlaySongTimerTextBox.ResetText();

                                windowsMediaPlayer.URL = song.SongFile;

                                DurationTimer.Interval = 1000;
                                PlaySongProgressBar.Maximum = (int)(song.Duration * 60);
                                SearchProgressBar.Maximum = (int)(song.Duration * 60);
                                PlayPlaylistProgressBarBox.Maximum = (int)(song.Duration * 60);
                                PlaySongPanel.BringToFront();
                                PlayerPlayingLabel.AppendText("Song playing: " + song.Name + "." + song.Format);
                                SearchPlayingLabel.AppendText("Song playing: " + song.Name + "." + song.Format);
                                DurationTimer.Start();
                                notification = 1;
                                break;
                            }
                        }
                        else if (song.Format == ".wav")
                        {
                            string result = SearchSearchResultsDomainUp.Text;
                            if (result == song.SearchedInfoSong())
                            {
                                if (song.ImageFile != null)
                                {
                                    PlaySongImageBoxImage.Image = System.Drawing.Image.FromFile(song.ImageFile);
                                }
                                else
                                {
                                    PlaySongImageBoxImage.Image = System.Drawing.Image.FromFile("Logo (1).jpg");
                                }
                                PlaySongSongPlaying.AppendText(mult);
                                AddingSearchedMult(ProfileDomainUp.Text, song.SongFile, null);
                                Thread.Sleep(2000);
                                PlayerPlayingLabel.Clear();
                                SearchPlayingLabel.Clear();
                                PlaySongProgressBar.Value = 0;
                                PlaySongTimerTextBox.ResetText();
                                soundPlayer.SoundLocation = song.SongFile;
                                soundPlayer.Play();
                                DurationTimer.Interval = 1000;
                                PlaySongProgressBar.Maximum = (int)(song.Duration * 60);
                                SearchProgressBar.Maximum = (int)(song.Duration * 60);
                                PlayPlaylistProgressBarBox.Maximum = (int)(song.Duration * 60);
                                PlaySongPanel.BringToFront();
                                PlayerPlayingLabel.AppendText("Song playing:" + song.Name + ":" + song.Artist + ":" + song.Format);
                                SearchPlayingLabel.AppendText("Song playing:" + song.Name + ":" + song.Artist + ":" + song.Format);
                                DurationTimer.Start();
                                notification = 1;
                                break;
                            }
                        }
                    }
                }
            }
            DisplayStartNotificationDomainUp.Visible = false;
            DisplayStartChooseSharedMult.Visible = false;
        }
        // --------- Panels JACO -------------- //
        private void WelcomePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Diseno_Oculto()
        {
            PlayListsOptionsPanel.Visible = false;
            CreateOptionsPanel.Visible = false;
            AboutUsInfoTextPanel.Visible = false;
        }
        private void OcultarSubMenus()
        {
            if (PlayListsOptionsPanel.Visible == true)
                PlayListsOptionsPanel.Visible = false;
            if (CreateOptionsPanel.Visible == true)
                CreateOptionsPanel.Visible = false;          
            if (AboutUsInfoTextPanel.Visible == true)
                AboutUsInfoTextPanel.Visible = false;
        }

        private void MostrarSubMenus(Panel SubMenu)
        {
            if (SubMenu.Visible == false)
            {
                OcultarSubMenus();
                SubMenu.Visible = true;
            }
            else
                SubMenu.Visible = false;
        }
        private void MultimediaButton_Click_1(object sender, EventArgs e)
        {
            SideMenuPanel.Width = 0;
            SideMenuPanel.Hide();
            PlayerMultPanel.Visible = false;
            SearchPanel.BringToFront();
        }

        private void PlayListsButton_Click_1(object sender, EventArgs e)
        {
            DisplayStartPanel.BringToFront();
            MostrarSubMenus(PlayListsOptionsPanel);
        }

        private void CreateButton_Click_1(object sender, EventArgs e)
        {
            MostrarSubMenus(CreateOptionsPanel);
        }

        private void AccountSettingsButton_Click(object sender, EventArgs e)
        {
            SideMenuPanel.Width = 0;
            SideMenuPanel.Hide();
            //MostrarSubMenus(AccountSettingsPanel);
            AccountSettingsUsernameTextBox.Clear();
            AccountSettingsPasswordTextBox.Clear();
            AccountSettingsAccountTypeTextBox.Clear();
            AccountSettingsEmailTextBox.Clear();
            AccountSettingsFollowersTextBox.Clear();
            AccountSettingsFollowingTextBox.Clear();

            ProfileSettingsNameTextBox.Clear();
            ProfileSettingsProfileTypeTextBox.Clear();
            ProfileSettingsGenderTextBox.Clear();
            ProfileSettingsBirthdayTextBox.Clear();
            //ProfileSettingsProfilePicImageBox.Image.Dispose();

            ProfilesInvalidCredentialTextBox.Clear();
            string username = UserLogInTextBox.Text;
            string password = PasswordLogInTextBox.Text;
            User user = new User();
            user = OnLoginButtonClicked(username, password);
            List<string> userGetterString = new List<string>();
            userGetterString = OnLogInLogInButton_Clicked2(username);
            string profileProfileName = ProfileDomainUp.Text;
            List<string> profileGetterString = OnProfilesChooseProfile_Click2(profileProfileName, username, password);

            AccountSettingsUsernameTextBox.AppendText(userGetterString[0]);
            AccountSettingsPasswordTextBox.AppendText(userGetterString[1]);
            AccountSettingsEmailTextBox.AppendText(userGetterString[2]);
            AccountSettingsAccountTypeTextBox.AppendText(userGetterString[3]);
            AccountSettingsFollowersTextBox.AppendText(userGetterString[4]);
            AccountSettingsFollowingTextBox.AppendText(userGetterString[5]);

            foreach (string seguidor in user.FollowingList)
            {
                AccountSettingsFollowingListDomaiUp.Items.Add(seguidor);
            }


            foreach (string followers in user.FollowerList)
            {
                AccountSettingsFollowerListDomainUp.Items.Add(followers);
            }

            ProfileSettingsNameTextBox.AppendText(profileGetterString[0]);
            ProfileSettingsProfileTypeTextBox.AppendText(profileGetterString[1]);
            ProfileSettingsGenderTextBox.AppendText(profileGetterString[2]);
            ProfileSettingsBirthdayTextBox.AppendText(profileGetterString[3]);

            Image pPic = CreateProfilePic1.Image;
            if (CreateProfilePicCheckedListBox.SelectedIndex == 0) pPic = CreateProfilePic1.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 1) pPic = CreateProfilePic2.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 2) pPic = CreateProfilePic3.Image;
            else if (CreateProfilePicCheckedListBox.SelectedIndex == 3) pPic = CreateProfilePic4.Image;

            ProfileSettingsProfilePicImageBox.Image = pPic;

            AccountProfileSettingsPanel.BringToFront();
        }
        private void AdminMenuButton_Click(object sender, EventArgs e)
        {
            SideMenuPanel.Width = 0;
            SideMenuPanel.Hide();
            AdminMenuPanel.BringToFront();
        }

        private void AboutFyBuZzButton_Click(object sender, EventArgs e)
        {
            
        }
        private void SideButtonAboutUs_Click(object sender, EventArgs e)
        {
            MostrarSubMenus(AboutUsInfoTextPanel);
        }

        private void PrivatePlsButton_Click(object sender, EventArgs e)
        {
            DisplayStartErrorPanel.Visible = false;
            DisplayStartErrorMessage.Visible = false;
            OcultarSubMenus();
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            
            if (profile.PersSongPlaylist.Count() != 0)
            {
                DisplayStartPersPlaylistPanel.Visible = true;
                PersPlaylisLabel.Visible = true;
                DisplayPlaylistPrefPlaylistSong.Visible = true;
                PersPlaylistSongLabel.Visible = true;
            }
            if (profile.PersVideoPlaylist.Count() != 0)
            {
                DisplayStartPersPlaylistPanel.Visible = true;
                PersPlaylisLabel.Visible = true;
                DisplayPlaylistPrefPlaylistVideo.Visible = true;
                PersPlaylistVideoLabel.Visible = true;
            }
            else if(DisplayStartPersPlaylistPanel.Visible == false)
            {
                DisplayStartErrorPanel.Visible = true;
                DisplayStartErrorMessage.Visible = true;

            }
        }

        private void SongsButton_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
        }

        private void VideosButton_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
        }

        private void GlobalPlsButton_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            DisplayStartGlobalPlaylistPanel.Visible = true;
            DisplayPlaylistsGlobalPlaylistSong.Visible = true;
            DisplayPlaylistsGlobalPlaylistVideo.Visible = true;
            GlobalPlaylistLabel.Visible = true;
            GlobalPlaylistSongLabel.Visible = true;
            GlobalPlaylistVideoLabel.Visible = true;
        }

        private void FavoritePlsButton_Click(object sender, EventArgs e)
        {
            DisplayStartErrorPanel.Visible = false;
            DisplayStartErrorMessage.Visible = false;

            OcultarSubMenus();
            Profile profile = OnProfilesChooseProfile_Click(ProfileDomainUp.Text, UserLogInTextBox.Text, PasswordLogInTextBox.Text);
            
            if (profile.PlaylistFavoritosSongs2.Count() != 0)
            {
                DisplayStartFavPlaylistPanel.Visible = true;
                FavPlaylistLabel.Visible = true;
                DisplayPlaylistsFavoritePlaylistSongs.Visible = true;
                FavPlaylistSongLabel.Visible = true;
            }
            if (profile.PlaylistFavoritosVideos2.Count() != 0)
            {
                DisplayStartFavPlaylistPanel.Visible = true;
                FavPlaylistLabel.Visible = true;
                DisplayPlaylistsFavoritePlaylistVideos.Visible = true;
                FavPlaylistVideoLabel.Visible = true;
            }
            else if(DisplayStartFavPlaylistPanel.Visible == false)
            {
                DisplayStartErrorPanel.Visible = true;
                DisplayStartErrorMessage.Visible = true;

            }
        }

        private void CreateSongsButton_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            CreateSongPanel.BringToFront();
        }

        private void CreateVideosButton_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            CreateVideoPanel.BringToFront();
        }

        private void CreatePlayListsButton_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            CreatePlaylistPanel.BringToFront();
        }

        private void CreateProfilesButton_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
            CreateProfilePanel.BringToFront();
        }

        private void UserInfoButton_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
        }

        private void ProfilesInfoButton_Click(object sender, EventArgs e)
        {
            OcultarSubMenus();
        }
        private void AboutUsInfoTextPanel_Paint(object sender, PaintEventArgs e)
        {
            
        }





        private void CreatePlaylistPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void PlayPlaylistMultTypeTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void PlayVideoPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AccountProfileSettingsSplitContainer_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void ProfilePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DisplayStartErrorMessage_Click(object sender, EventArgs e)
        {
           
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            PlayerMultPanelMtrackPB.Value = (int)windowsMediaPlayer.Ctlcontrols.currentPosition;
            PlayerMultPanelMtrackVB.Value = windowsMediaPlayer.settings.volume;
        }

        private void windowsMediaPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (windowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                PlayerMultPanelMtrackPB.Maximum = (int)windowsMediaPlayer.Ctlcontrols.currentItem.duration;
                ProgressTimer.Start();
            }
            else if (windowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                ProgressTimer.Stop();
            }
            else if (windowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                ProgressTimer.Stop();
                PlayerMultPanelMtrackPB.Value = 0;
            }
            else if (windowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                PlayerMultPanelMtrackPB.Value = 0;
                PlaySongSkipSongButton.PerformClick();
            }
            
        }

        private void PlayerMultPanelMtrackVB_ValueChanged(object sender, decimal value)
        {
            windowsMediaPlayer.settings.volume = PlayerMultPanelMtrackVB.Value;
        }

        private void PlayerMultPanelMtrackPB_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (formatProgressBar == ".mp3")
                {
                    windowsMediaPlayer.Ctlcontrols.currentPosition = PlayerMultPanelMtrackPB.Value;
                }

            }
        }

        private void TimerWav_Tick(object sender, EventArgs e)
        {
            PlayerMultPanelMtrackPB.Value = ticks;
            ticks++;
            if (ticks == durationWav)
            {
                TimerWav.Stop();
                ticks = 0;
                PlaySongSkipSongButton.PerformClick();
            }
        }

        private void DisplayStartProfileInfo_Click(object sender, EventArgs e)
        {
            DisplayStartProfNameTextBox.Clear();
            DisplayStartProfGenderTextBox.Clear();
            DisplayStartProfTypeTextBox.Clear();
            DisplayStartProfAgeTextBox.Clear();

            DisplayStartNotificationDomainUp.Visible = false;
            DisplayStartChooseSharedMult.Visible = false;

            string username = UserLogInTextBox.Text;

            string password = PasswordLogInTextBox.Text;
            string profileProfileName = ProfileDomainUp.Text;

            List<string> profileGetterString = OnProfilesChooseProfile_Click2(profileProfileName, username, password);

            DisplayStartProfNameTextBox.AppendText(profileGetterString[0]);
            DisplayStartProfTypeTextBox.AppendText(profileGetterString[1]);
            DisplayStartProfGenderTextBox.AppendText(profileGetterString[2]);
            DisplayStartProfAgeTextBox.AppendText(profileGetterString[3]);


            if (DisplayStartNotificationDomainUp.Items.Count != 0)
            {
                DisplayStartNotificationDomainUp.Visible = true;
                DisplayStartChooseSharedMult.Visible = true;
            }
            if(AddsPanel2.Visible == false)
            {
                DisplayStartProfileInfoPanel.Visible = true;
            }
            else
            {
                AddsPanel2.Visible = false;
                DisplayStartProfileInfoPanel.Visible = true;
            }
            
        }

        private void ProfileInfoAccountSettingsButton_Click(object sender, EventArgs e)
        {
            SideMenuPanel.Width = 0;
            SideMenuPanel.Hide();
            List<string> userGetter = OnLogInLogInButton_Clicked2(UserLogInTextBox.Text);
            DisplayStartProfileInfoPanel.Visible = false;
            if (userGetter[3] == "standard")
            {
                AddsPanel2.Visible = true;
            }
            AccountSettingsUsernameTextBox.AppendText(userGetter[0]);
            AccountSettingsPasswordTextBox.AppendText(userGetter[1]);
            AccountSettingsEmailTextBox.AppendText(userGetter[2]);
            AccountSettingsAccountTypeTextBox.AppendText(userGetter[3]);
            AccountSettingsFollowersTextBox.AppendText(userGetter[4]);
            AccountSettingsFollowingTextBox.AppendText(userGetter[5]);

            AccountProfileSettingsPanel.BringToFront();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            List<string> userGetter = OnLogInLogInButton_Clicked2(UserLogInTextBox.Text);
            DisplayStartProfileInfoPanel.Visible = false;
            if (userGetter[3] == "standard")
            {
                AddsPanel2.Visible = true;
            }
        }
        private void SideMenuShowHideIconButton_Click(object sender, EventArgs e)
        {
            if (SideMenuPanel.Visible)
            {
                isShowing = false;
                TimerSidePanel.Start();
            }
            else
            {
                isShowing = true;
                SideMenuPanel.Show();
                TimerSidePanel.Start();
            }
        }

        private void TimerSidePanel_Tick(object sender, EventArgs e)
        {
            if (isShowing)
            {
                if(SideMenuPanel.Width >= 246)
                {
                    TimerSidePanel.Stop();
                    SideMenuPanel.Width = 246;
                }
                else
                {
                    SideMenuPanel.Width += 35;
                }
            }
            else
            {
                if (SideMenuPanel.Width <= 0)
                {
                    SideMenuPanel.Hide();
                    TimerSidePanel.Stop();
                    SideMenuPanel.Width = 0;
                }
                else
                {
                    SideMenuPanel.Width -= 35;
                }
            }
        }

        private void VolumeIconButton_Click(object sender, EventArgs e)
        {
            switch (volumeIcon)
            {
                case true:
                    PlayerMultPanelMtrackVB.Value = 25;
                    VolumeIconButton.IconChar = FontAwesome.Sharp.IconChar.VolumeUp;
                    volumeIcon = false;
                    break;

                case false:
                    PlayerMultPanelMtrackVB.Value = 0;
                    VolumeIconButton.IconChar = FontAwesome.Sharp.IconChar.VolumeMute;
                    volumeIcon = true;
                    break;
            }
        }



        
    

        private void SearchFilterButton_Click(object sender, EventArgs e)
        {
            if(AllFiltersCheckbox.Visible == false)
            {
                AllFiltersCheckbox.Visible = true;
            }
            else
            {
                AllFiltersCheckbox.Visible = false;
            }
            
        }

        

        private void SharedMultNotificationButton_Click(object sender, EventArgs e)
        {
            
        }


        private void CreateSongPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CreateVideoLoadVideoTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
