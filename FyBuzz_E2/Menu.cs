﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FyBuzz_E2
{
    public class Menu
    {
        protected List<string> filters;
        public List<Song> searchedSongs;
        public List<Video> searchedVideos;
        User user = new User();
        DataBase database = new DataBase();
        Player player = new Player();

        public bool DisplayLogin()
        {
            Server server = new Server(database);

            bool x = false;
            while (x == false)
            {
                Console.WriteLine("------------Welcome to FyBuZz--------------");
                Console.WriteLine("I) Log-In with a existing account.");
                Console.WriteLine("II) Register.");
                Console.WriteLine("III) Close App.");
                string dec = Console.ReadLine();
                switch (dec)
                {
                    case "I":
                        Console.Write("Username: ");
                        string username = Console.ReadLine();
                        Console.Write("Password: ");
                        string password = Console.ReadLine();
                        if (database.LogIn(username, password) == null) //tengo que obtener mediante un get el nombre de usuario y password
                        {
                            user.Username = username;
                            user.Password = password;
                            Console.WriteLine("Login Succesfull.");
                            x = true;
                        }
                        else
                        {
                            Console.WriteLine("ERROR[!]");
                        }
                        break;


                    case "II":
                        server.Register(); //Agregue el metodo de server register.
                        x = false;
                        //Dar todas las caracteriscas del usaurio aca.
                        break;
                    case "III":
                        return false;
                        
                }
            }
            return x;
        }
        //if DisplayLogin == true:
        public Profile DisplayProfiles()
        {
            Dictionary<int, Profile> dicprofile = new Dictionary<int, Profile>();
            List<Profile> profilelist = new List<Profile>();
            Profile profile_n = new Profile("", "", "", "", "", 0);

            Console.WriteLine("---------Profiles----------");
            bool x = true;
            int pcont = 01;
            while (x == true)
            {
                Console.WriteLine("Choose a profile(0) or Create Profile(1) or Add Mult (2) or Display Mult (3)");
                string dec = Console.ReadLine();
                if (dec == "0")
                {
                    Console.WriteLine("List of profiles:");
                    dicprofile = user.Perfiles;
                    foreach (Profile profile in dicprofile.Values)
                    {
                        Console.WriteLine(profile.ProfileName);
                        profilelist.Add(profile);
                    }
                    Console.WriteLine("Choose a profile:");
                    string perfil = Console.ReadLine();
                    for (int i = 0; i < profilelist.Count(); i++)
                    {
                        if (perfil == profilelist[i].ProfileName)
                        {
                            profile_n = profilelist[i]; // tengo que devolver algun perfil
                            x = false;
                        }
                    }
                }
                else if(dec == "1")
                {
                    Console.WriteLine("Create a profile:");
                    Console.Write("Profile name: ");
                    string pname = Console.ReadLine();
                    Console.Write("Profile pic: ");
                    string ppic = Console.ReadLine();
                    Console.Write("Profile type(public/private): ");
                    string ptype = Console.ReadLine();
                    //string pmail = user.Email; Esto deberia ser....
                    string pmail = "diego@gmail.com";
                    Console.Write("Profile gender (M/F): ");
                    string pgender = Console.ReadLine();
                    Console.Write("Profile age: ");
                    int page = int.Parse(Console.ReadLine());
                    user.CreateProfile(pname, ppic, ptype, pmail, pgender, page, pcont);
                    pcont++;
                    
                }
                else if(dec == "2")
                {
                    Console.Write("Que desea agregar? (cancion(0), video(1), Playlist(2)): ");
                    int opc = int.Parse(Console.ReadLine());
                    List<string> infoMult = AskInfoMult(opc);
                    string description = database.AddMult(opc, infoMult);
                    if (description == null) Console.WriteLine("Se ha ingresado la multimedia!");
                    else Console.WriteLine("ERROR[!] ~{0}", description);
                }
                else if(dec == "3")
                {
                    Console.WriteLine("Que lista desea observar? (cancion(0), video(1), Playlist(2)): ");
                    int opc = int.Parse(Console.ReadLine());
                    DisplayGlobalMult(opc, database);
                }
                else
                {
                    x = false;
                }
                    
            }
            return profile_n;
        }

        //Se necesita el perfil con el que quiere acceder
        public void DisplayStart(Profile profile) // solo funciona si DisplayLogIn() retorna true se ve en program.
        {
            //database.createFiles(); //crea los archivos necesarios.

            List<PlayList> listPlayListGlobal = new List<PlayList>();
            listPlayListGlobal = database.Load_PLs();
            if(listPlayListGlobal != null)
            {
                DisplayPlaylists(listPlayListGlobal);
            }
            
            List<Video> listVideosGlobal = new List<Video>();
            listVideosGlobal = database.Load_Videos();
            
            List<Song> listSongsGlobal = new List<Song>();
            listSongsGlobal = database.Load_Songs();
            


            // mostrará todas las playlist del usuario, si es primera vez que ingresa estara la playlist general y la favorita(esta sin nada)
            // es la lista global de playlist que viene de database, pero hay que conectarla

            PlayList favSongs = new PlayList(".mp3", "FavoriteSongs");
            Dictionary<string, List<Song>> playlistFavSongs = favSongs.DicCanciones; //Playlist de favoritos que su nombre es el de arriba.

            PlayList favVideos = new PlayList(".mp3", "FavoriteVideos");
            Dictionary<string, List<Video>> playlistFavVideos = favVideos.DicVideos; //Playlist de favoritos que su nombre es el de arriba.

            List<PlayList> followedPL = profile.FollowedPlayList; //una lista de todas las playlist, discos, usuarios, etc.
            //Si seguimos la usuario seguiremos todas sus playlist (REVISAR ESTO)

            Console.WriteLine("------------Welcome to FyBuZz--------------"); //Se inicia el menu en si.
            bool x = true;
            while (x == true)
            {
                Console.WriteLine("I) Search Songs or Videos."); //Faltaria la bsuqueda de gente.
                Console.WriteLine("II) Display all Playlists.");
                Console.WriteLine("III) Account Settings.");
                Console.WriteLine("IV) Play a Playlist.");
                Console.WriteLine("V) LogOut.");
                Console.WriteLine("VI) CloseApp.");
                string dec = Console.ReadLine();
                switch (dec)
                { //(REVISAR DESPUES)Mejorar el metodo de busqueda para que busque canciones que se parezca
                    case "I":
                        //Método de buscar, una vez buscada la canción y elegida.
                        
                        Console.WriteLine("What would you like to search? (Songs/Videos)");
                        string type = Console.ReadLine();
                        if (type == "Songs")
                        {
                            Console.WriteLine("Type what you want to search...");
                            string search = Console.ReadLine();
                            List<string> searchEngine = SearchEngine(search, type);
                            List<int> indexglobal = new List<int>();
                            Console.WriteLine("Searched Songs, choose one...");
                            for(int i = 0; i < searchEngine.Count(); i++)
                            {
                                searchEngine[i].Split('>'); // [[Bad Bunny Safaera etc],[12]]
                                indexglobal.Add(searchEngine[i][1]);

                                Console.WriteLine((i+1) + ") " + searchEngine[i][0]);
                            }
                            int indice = int.Parse(Console.ReadLine())-1;
                            Song song = listSongsGlobal[indexglobal[indice]]; //La cancion a la que querria escuchar

                            Reproduction(1, type,indexglobal[indice] , false); //Falta arreglar el método de reproduccion
                        }
                        else
                        {
                            Console.WriteLine("Type what you want to search...");
                            string search = Console.ReadLine();
                            List<string> searchEngine = SearchEngine(search, type);
                            List<int> indexglobal = new List<int>();
                            Console.WriteLine("Searched Videos, choose one...");
                            for (int i = 0; i < searchEngine.Count(); i++)
                            {
                                searchEngine[i].Split('>'); // [[Bad Bunny Safaera etc],[12]]
                                indexglobal.Add(searchEngine[i][1]);

                                Console.WriteLine((i + 1) + ") " + searchEngine[i][0]);
                            }
                            int indice = int.Parse(Console.ReadLine()) - 1;
                            Video video = listVideosGlobal[indexglobal[indice]]; //La video a la que querria escuchar

                            Reproduction(1,type, indexglobal[indice], false); //Falta arreglar el método de reproduccion
                        }
                        break;
                    case "II":
                        if (playlistFavSongs.Count() != 0)
                        {
                            Console.WriteLine(favSongs.InfoPlayList());
                        }
                        if (playlistFavVideos.Count() != 0)
                        {
                            Console.WriteLine(favSongs.InfoPlayList());
                        }
                        if (followedPL.Count() != 0)
                        {
                            DisplayPlaylists(followedPL);
                        }
                        DisplayPlaylists(listPlayListGlobal);

                        break;
                    case "III":
                        AccountSettings(user); // incorporar el usuario.

                        break;

                    case "IV":
                        Console.WriteLine("What playlist do you want to play?(GlobalPlayLists, FollowedPlaylists or FavoritePlayList)");
                        string play = Console.ReadLine();
                        if (play == "FavoritePlayList")
                        {
                            Console.WriteLine("Random or select a song/video?");
                            string rand = Console.ReadLine();
                            if (rand == "Random")
                            {
                                //Reproduction(0, true);
                                Console.WriteLine("Aca se esta reproduciendo la playlist favorita.");
                            }
                            else
                            {
                                Console.WriteLine("What would you like to play?");
                                string mult = Console.ReadLine();
                                if (mult == "Songs")
                                {
                                    //SongsSearchEngine(searchedSongs);
                                    //Reproduction(1, mult, indexofmultimedia, false);
                                    Console.WriteLine("Aca se esta reproduciendo la cancion de playlist favorita.");
                                }
                                else
                                {
                                    //VideosSearchEngine(searchedVideos);
                                    //Reproduction(1, 1, false);
                                    Console.WriteLine("Aca se esta reproduciendo la Video de playlist favorita.");
                                }
                            }
                        }
                        else if (play == "GlobalPlayList")
                        {
                            Console.WriteLine("Please select the number...");
                            string num = Console.ReadLine();
                            //Elegir la playlist que te dan y según eso lo siguiente.
                            Console.WriteLine("Random or select a song?");
                            string rand = Console.ReadLine();
                            if (rand == "Random")
                            {
                                //Reproduction(4, true);
                                Console.WriteLine("Aca se esta reproduciendo la playlist " + num + " global.");
                            }
                            else
                            {
                                Console.WriteLine("What would you like to play?");
                                string mult = Console.ReadLine();
                                if (mult == "Songs")
                                {
                                    //ongsSearchEngine(searchedSongs);
                                    //Reproduction(1, 0, false);
                                    Console.WriteLine("Aca se esta reproduciendo la cancion de playlist " + num + " global.");
                                }
                                else
                                {
                                    //VideosSearchEngine(searchedVideos);
                                    //Reproduction(1, 1, false);
                                    Console.WriteLine("Aca se esta reproduciendo la video de playlist " + num + " global.");
                                }
                            }
                        }
                        else if (play == "FollowedPlaylist")
                        {
                            Console.WriteLine("Please select the number...");
                            string num = Console.ReadLine();
                            //Elegir la playlist que te dan y según eso lo siguiente.
                            Console.WriteLine("Random or select multimedia?");
                            string rand = Console.ReadLine();
                            if (rand == "Random")
                            {
                                //Reproduction(4, true);
                                Console.WriteLine("Aca se esta reproduciendo la playlist " + num + " followed.");
                            }
                            else
                            {
                                Console.WriteLine("What would you like to play?");
                                string mult = Console.ReadLine();
                                if (mult == "Songs")
                                {
                                    //SongsSearchEngine(searchedSongs);
                                    //Reproduction(1, 0, false);
                                    Console.WriteLine("Aca se esta reproduciendo la cancion de playlist " + num + " followed.");
                                }
                                else
                                {
                                    //VideosSearchEngine(searchedVideos);
                                    //Reproduction(1, 1, false);
                                    Console.WriteLine("Aca se esta reproduciendo la video de playlist " + num + " followed.");
                                }
                            }
                        }
                        break;
                    case "V":
                        //termina el método y llamaria al metodo de inicio en program.
                        Console.WriteLine("LoggedOut");
                        x = false;
                        break;
                    case "VI":
                        x = false;
                        break;
                }
            }
        }

        //tenemos que decidir si esta clase sera de inputs y outputs, o la que hace de reproductor.
        public void DisplayGlobalMult(int typeMult, DataBase database)
        {
            
            
            
            if (typeMult == 0)
            {
                List<Song> ListSongsGlobal = database.Load_Songs();
                for (int i = 0; i < ListSongsGlobal.Count(); i++)
                {
                    Console.WriteLine("Cancion {0}", i);
                    Console.WriteLine(ListSongsGlobal[i].DisplayInfoSong());
                }
            }
            else if(typeMult == 1)
            {
                List<Video> ListVideosGlobal = database.Load_Videos();
                for (int i = 0; i < ListVideosGlobal.Count(); i++)
                {
                    Console.WriteLine("Video {0}", i);
                    Console.WriteLine(ListVideosGlobal[i].DisplayInfoVideo());
                }
            }
            else if(typeMult == 2)
            {
                List<PlayList> ListPLsGlobal = database.Load_PLs();
                for (int i = 0; i < ListPLsGlobal.Count(); i++)
                {
                    Console.WriteLine("Playlist {0}", i);
                    Console.WriteLine(i + ") " + ListPLsGlobal[i].DisplayInfoPlayList());
                }
            }

        }

        public void DisplayPlaylists(List<PlayList> playlist)
        {
            for (int i = 0; i < playlist.Count(); i++)
            {
                Console.WriteLine(i + ") " + playlist[i].InfoPlayList());
            }
        }
        public void AccountSettings(User user)
        {
            for (int i = 0; i < user.AccountSettings().Count(); i++)
            {
                Console.WriteLine("Username: " + user.AccountSettings()[0] + "\n");
                Console.WriteLine("Password: " + user.AccountSettings()[1] + "\n");
                Console.WriteLine("Email: " + user.AccountSettings()[2] + "\n");
                Console.WriteLine("Account type: " + user.AccountSettings()[3] + "\n");
            }
        }

        public void Reproduction(int inplaylist, string type, int indexofmultimedia, bool ver) // Si viene de una playlist y se decide poner aleatorio verif sera 4, si se elige una canción sera 1.
        {
            List<Video> listVideosGlobal = new List<Video>();
            listVideosGlobal = database.Load_Videos();

            List<Song> listSongsGlobal = new List<Song>();
            listSongsGlobal = database.Load_Songs();

            if (inplaylist == 1)
            {
                int x = 0;
                int cont = 0;
                if (type == "Song") {
                    while (cont != -1)
                    {
                        cont = player.PlaySong(cont, listSongsGlobal[indexofmultimedia], ver); //Devuelve el tiempo en el que se para la canción
                        if (cont != -1) cont = player.Stop(cont); // devuelve el contador cuando se detiene para empezar de nuevo.
                    }
                }
                else
                {
                    while (cont != -1)
                    {
                        cont = player.PlayVideo(cont, listVideosGlobal[indexofmultimedia], ver); //Devuelve el tiempo en el que se para la canción
                        if (cont != -1) cont = player.Stop(cont); // devuelve el contador cuando se detiene para empezar de nuevo.
                    }
                }
                
            }
            else
            {
                //player.Random(playlist);
                Console.WriteLine("Reproduce cancion random de la playlist");
                
            }
        }

        public List<string> SearchEngine(string searching, string type)
        {
            List<Video> listVideosGlobal = new List<Video>();
            listVideosGlobal = database.Load_Videos();

            List<Song> listSongsGlobal = new List<Song>();
            listSongsGlobal = database.Load_Songs();

            List<string> searchEngine = new List<string>();
            int num_s = 0;
            int num_v = 0;


            if (type == "Songs")
            {
                foreach (Song song in listSongsGlobal)
                {
                    for (int i = 0; i < song.InfoSong().Count(); i++)
                    {
                        if (song.InfoSong()[i] == searching)
                        {
                            searchEngine.Add(song.SearchedInfoSong() + ">" + num_s); //num_s es un int y no me patalea, si tira error es aca. Usar remove leguer
                        }
                    }
                    num_s++;
                }
            }
            else
            {
                foreach (Video video in listVideosGlobal)
                {
                    for (int i = 0; i < video.InfoVideo().Count(); i++)
                    {
                        if (video.InfoVideo()[i] == searching)
                        {
                            searchEngine.Add(video.SearchedInfoVideo() + ">" + num_v);
                        }
                        
                    }
                    num_v++;
                }
            }
            if (searchEngine.Count() == 0) Console.WriteLine("No match found...");
            return searchEngine;
        }

        public void DisplayHistory(List<Song> searchStorySongs, List<Video> searchStoryVideos)
        {
            if (searchStorySongs.Count() < 5 || searchStoryVideos.Count() < 5)
            {
                for (int i = 0; i < searchStorySongs.Count(); i++)
                {
                    Console.WriteLine(searchStorySongs[i]); //Recordar que cada eleemento de estas listas van a ser la información de cada archivo multimedia.
                }
                for (int i = 0; i < searchStoryVideos.Count(); i++)
                {
                    Console.WriteLine(searchStoryVideos[i]);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(searchStorySongs[i]);
                }
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(searchStoryVideos[i]);
                }
            }
        }

        public List<string> AskInfoMult(int type)
        {
            List<string> infoMult = new List<string>();
            if(type == 0)
            {
                Console.Write("Escriba el nombre de la cancion: ");                                    string n = Console.ReadLine();
                Console.Write("Escriba su nombre de artista: ");                                       string art = Console.ReadLine();
                Console.Write("Escriba el album en donde se encontrará la cancion: ");                 string alb = Console.ReadLine();
                Console.Write("Escriba la discografia: ");                                             string disc = Console.ReadLine();
                Console.Write("Escriba el genero de la cancion: ");                                    string gen = Console.ReadLine();
                Console.Write("Escriba la fecha de publicación (fecha actual en formato dd/mm/aa): "); string date = Console.ReadLine();
                Console.Write("Escriba el studio: ");                                                  string std = Console.ReadLine();
                Console.Write("Escriba la duración de la cancion (formato: min.seg): ");               string dur = Console.ReadLine();
                Console.Write("Escriba el formato de la cancion (.mp3 || .wav): ");                    string format = Console.ReadLine();
                Console.Write("su cancion tiene lyrics?: ");
                
                string lyr;
                if ((Console.ReadLine() == "y") || (Console.ReadLine() == "Y"))
                    lyr = "true";
                else 
                    lyr = "false";

                infoMult = new List<string>() { n, art, alb, disc, gen, date, std, dur, lyr, format };
            }
            else if(type == 1)
            {
                Console.Write("Escriba el nombre del video: ");                                        string n = Console.ReadLine();
                Console.Write("Escriba el nombre de el/los actores: ");                                string act = Console.ReadLine();
                Console.Write("Escriba el nombre de el/los directores: ");                             string dir = Console.ReadLine();
                Console.Write("Escriba la fecha de publicación (fecha actual en formato dd/mm/aa): "); string date = Console.ReadLine();
                Console.Write("Escriba la dimension del video (numero): ");                            string dim = Console.ReadLine();
                Console.Write("Escriba la calidad del video: ");                                       string cal = Console.ReadLine();
                Console.Write("Escriba la categoria del video: ");                                     string cat = Console.ReadLine();
                Console.Write("Escriba la descripción del video: ");                                   string des = Console.ReadLine();
                Console.Write("Escriba la duración de la cancion (formato: min.seg)(double): ");       string dur = Console.ReadLine();
                Console.Write("Escriba el formato de la video (.mp4 || .mov): ");                      string format = Console.ReadLine();
                Console.Write("Confirme si es que tiene una imagen para agregar (y/n): ");             string im = Console.ReadLine();
                Console.Write("Confirme si es que tiene subtitulos para su video: ");                  string sub = Console.ReadLine();
                if ((im == "y") || (im == "Y"))
                    im = "true";
                else
                    im = "false";

                if ((sub== "y") || (sub == "Y"))
                    sub = "true";
                else
                    sub = "false";

                infoMult = new List<string>() { n, act, dir, date, dim, cal, cat, des, im, dur, sub, format };
            }
            else if(type == 2)
            {
                Console.Write("Escriba el nombre de la playlist: ");                     string n = Console.ReadLine();
                Console.Write("Quiere que su playlist sea de cancion o video? (c/v): "); string format = null;

                if (Console.ReadLine() == "c" || Console.ReadLine() == "C")
                {
                    Console.Write("Escriba el formato de la playlist de cancion (.mp3|.wav): ");
                    format = Console.ReadLine();
                }
                else if (Console.ReadLine() == "v" || Console.ReadLine() == "V")
                {
                    Console.Write("Escriba el formato de la playlist de video (.mp4|.mov): ");
                    format = Console.ReadLine();
                }

                    infoMult = new List<string>() { n, format};
            }
            return infoMult;
            
        }
    }
}

