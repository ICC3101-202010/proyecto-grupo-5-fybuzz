using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    [Serializable]
    public class Multimedia
    {
        //ATRIBUTOS:

        protected string name;
        protected string date;
        protected int ranking;
        protected int likes;
        protected int generalRep;
        protected int profileRep;
        protected double duration;
        protected double presentTime = 0;
        protected string format;
        //--------------------------------------------------------------------------------------------------

        //GETTERS Y SETTERS
        //--------------------------------------------------------------------------------------------------
        public double Duration { get => duration; }
        public string Name { get => name; }
        public int Likes { get => likes; set => likes = value; }
        public int GeneralRep { get => generalRep; set => generalRep = value; }
        public double PresentTime { get => presentTime; set => presentTime = value; }
        //--------------------------------------------------------------------------------------------------

        //MÉTODOS

        //METODOS CHANGE
        //--------------------------------------------------------------------------------------------------
        public string AddMult()
        {
            throw new NotImplementedException();
        }

        public string DeleteMult()
        {
            throw new NotImplementedException();
        }

        public bool VerifyMult()
        {
            throw new NotImplementedException();
        }

        //--------------------------------------------------------------------------------------------------



        /*InfoMult()
          Todavia no sabemos como colocar un archivo srt o una lyric de una cancion, entonces no puedo hacer display (lyrics esta en bool) */




    }
}
