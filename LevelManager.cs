using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CubeClimber
{
    internal class LevelManager
    {
        public static Level A = new Level();
        public static Level B = new Level();
        public static Level C = new Level();
        public static Level D = new Level();
        public static Level[] Levels = new Level[] {A, B, C, D};

        static public void Giveth()
        {
            A.Layout = new char[5, 10];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    A.Layout[i, j] = 'A';
                }
            }
            A.Layout[1, 1] = 'C';
            A.Layout[3, 1] = 'C';
            A.Layout[0, 3] = 'V';
            A.Layout[1, 4] = 'H';
            A.Layout[2, 4] = 'H';
            A.Layout[3, 4] = 'H';
            A.Layout[4, 3] = 'V';
            A.Layout[2, 2] = 'P';
            A.Layout[2, 1] = 'H';
            A.PlayerStartPlace = new int[] {2,1};
            A.PlayerStartSideUp = 2;
            A.PlayerStartRot = 2;
            A.WinPlace = new int[] {2,2};

            B.Layout = new char[5, 8];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    B.Layout[i, j] = 'A';
                }
            }
            B.Layout[2, 4] = 'H';
            B.Layout[2, 3] = 'H';
            B.Layout[2, 2] = 'H';
            B.Layout[2, 1] = 'W';
            B.PlayerStartPlace = new int[] { 2, 4 };
            B.PlayerStartSideUp = 1;
            B.PlayerStartRot = 1;
            B.WinPlace = new int[] { 2, 1 };

            C.Layout = new char[7, 9];
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    C.Layout[i, j] = 'A';
                }
            }
            C.Layout[2, 1] = 'H';
            C.Layout[2, 2] = 'H';
            C.Layout[1, 4] = 'H';
            C.Layout[2, 4] = 'H';
            C.Layout[3, 4] = 'H';
            C.Layout[4, 4] = 'H';
            C.Layout[5, 7] = 'W';
            C.PlayerStartPlace = new int[] { 2, 1 };
            C.PlayerStartSideUp = 2;
            C.PlayerStartRot = 2;
            C.WinPlace = new int[] { 5, 7 };

            D.Layout = new char[7, 10];
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    D.Layout[i, j] = 'A';
                }
            }
            D.Layout[2, 1] = 'H';
            D.Layout[3, 1] = 'C';
            D.Layout[4, 1] = 'H';
            D.Layout[5, 1] = 'H';
            D.Layout[3, 2] = 'V';
            D.Layout[1, 3] = 'V';
            D.Layout[2, 3] = 'H';
            D.Layout[3, 3] = 'C';
            D.Layout[1, 4] = 'V';
            D.Layout[2, 4] = 'H';
            D.Layout[3, 4] = 'V';
            D.Layout[1, 5] = 'V';
            D.Layout[5, 5] = 'V';
            D.Layout[1, 6] = 'V';
            D.Layout[5, 6] = 'W';
            D.PlayerStartPlace = new int[] { 1, 6 };
            D.PlayerStartSideUp = 1;
            D.PlayerStartRot = 2;
            D.WinPlace = new int[] { 5, 6 };
        }
    }
}
