using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using GTA.Math;
using GTA.NaturalMotion;
using GTA.UI;
using System.Windows.Forms; //used to get input
using System.Drawing;

namespace HUDExtended
{
    public class Main : Script
    {
        public Main()
        {
            Interval = 10;
            Tick += OnTick;
            KeyDown += OnKeyDown;
            Notification.Show(this.Name + "has launched.");
        }

        //event runs every tick
        void OnTick(object sender, EventArgs e)
        {

            //update current direction
            GetDirection().Draw();


            //show details of targeted NPC (if aiming)
            TargetNPC().Draw();

            //show speed if in vehicle
        }

        //event triggered when any key is pressed
        void OnKeyDown(object sender, KeyEventArgs e)
        {

        }

        TextElement GetDirection()
        {
            PointF coords = new PointF() { X = 25, Y = 500 };
            string direction = "";

            var pos = Game.Player.Character.Position;
            var forward = Game.Player.Character.ForwardVector;
            var dir = forward;

            //set to direction vector by normalizing and rounding 
            dir.Normalize(); 
            dir = dir.Round(0);

            // x = east, west. y = north, south.
            // -1 means east, 1 means west
            // -1 means south, 1 means north
            //determine which direction the vector is pointing at 

            //get Y axis first
            if(dir.Y == 1)
            {
                direction += "N";
            }
            else if(dir.Y == -1)
            {
                direction += "S";
            }

            //then x axis
            if(dir.X == 1)
            {
                direction += "W";
            }
            else if(dir.X == -1)
            {
                direction += "E";
            }

            return new TextElement("DIR: " + direction, coords , .5f);
        }

        TextElement TargetNPC()
        {
            PointF coords = new PointF() { X = 50, Y = 600 };
            Entity npc = Game.Player.TargetedEntity;

            Notification.Show("targetNPC reached DEBUG");
            return new TextElement(npc.Handle.ToString(), coords, .5f);
        }
    }
}
