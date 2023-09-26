using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeClimber
{
    internal class Level
    {
        public char[,] Layout;
        public int[] PlayerStartPlace;
        public int PlayerStartSideUp;
        public int PlayerStartRot;
        public int[] WinPlace;


        public Level()
        {

        }
    }
}
