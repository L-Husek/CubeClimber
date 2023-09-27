using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;

namespace CubeClimber
{
    internal class CubeManager
    {
        public static int SideUp = 0; // From 0 to 5, the side currently facing up, view textures for reference/explanation
        public static int Rot = 0; // From 0 to 3, the current rotation of the upward facing side, in 90 degree increments clockwise
        public static char PanelDown = 'N'; // Char representing what panels currently touch the wall, if any. 'N' = none, 'H' = horizontal, 'V' = vertical

        static int[] LUT0 = { 1, 5, 4, 2 };
        static int[] LUT1 = { 2, 3, 5, 0 };
        static int[] LUT2 = { 3, 1, 0, 4 };
        static int[] LUT3 = { 4, 5, 1, 2 };
        static int[] LUT4 = { 5, 3, 2, 0 };
        static int[] LUT5 = { 0, 1, 3, 4 };

        static int[] ROTS = { 3, 3, 1, 3 };

        /// <summary>
        /// Rotate the cube in a direction represented by an int (0-3)
        /// </summary>
        /// <param name="dir"></param>
        public static void Rotate(int dir)
        {
            // Determine new SideUp based on direction and rot. I am aware this could be simplified with an array of arrays, but this is easier to read and comprehend.
            int PrevSideUp = SideUp; // Save for next step.
            switch (SideUp)
            {
                case 0:
                    SideUp = LUT0[(dir - Rot + 4) % 4];
                    break;
                case 1:
                    SideUp = LUT1[(dir - Rot + 4) % 4];
                    break;
                case 2:
                    SideUp = LUT2[(dir - Rot + 4) % 4];
                    break;
                case 3:
                    SideUp = LUT3[(dir - Rot + 4) % 4];
                    break;
                case 4:
                    SideUp = LUT4[(dir - Rot + 4) % 4];
                    break;
                case 5:
                    SideUp = LUT5[(dir - Rot + 4) % 4];
                    break;
            }

            // Determine new Rot.
            Rot = (Rot + ROTS[(dir - Rot + 4) % 4] + (PrevSideUp % 2 * 2)) % 4;

            // Determine new PanelDown.
            switch (SideUp)
            {
                case 0:
                    PanelDown = 'N';
                    break;
                case 1:
                    if (Rot % 2 == 0) {
                        PanelDown = 'V';
                    } else {
                        PanelDown = 'H';
                    }
                    break;
                case 2:
                    if (Rot % 2 == 0) {
                        PanelDown = 'H';
                    } else {
                        PanelDown = 'V';
                    }
                    break;
                case 3:
                    if (Rot % 2 == 0) {
                        PanelDown = 'H';
                    } else {
                        PanelDown = 'V';
                    }
                    break;
                case 4:
                    if (Rot % 2 == 0) {
                        PanelDown = 'V';
                    } else {
                        PanelDown = 'H';
                    }
                    break;
                case 5:
                    if (Rot % 2 == 0) {
                        PanelDown = 'H';
                    } else {
                        PanelDown = 'V';
                    }
                    break;
            }
        }
    }
}
