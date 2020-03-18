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
            Tick += OnTick;
            KeyDown += OnKeyDown;
        }

        //event runs every tick
        void OnTick(object sender, EventArgs e)
        {

            //update current direction
            GetDirection().Draw();


            //show details of targeted NPC (if aiming)
            if(Game.Player.IsTargetingAnything == true && Game.Player.IsAiming == true)
            {
                TargetNPC().Draw();
            }
            
            //show speed if in vehicle
        }

        //event triggered when any key is pressed
        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.J)
            {
                // set wanted level
                Game.Player.WantedLevel = 0;
                Notification.Show("Wanted Level Reset!");
            }
        }

        TextElement GetDirection()
        {
            PointF coords = new PointF() { X = 500, Y = 500 };

            var pos = Game.Player.Character.Position;
            var forward = Game.Player.Character.ForwardVector;
            var dir = pos + forward;

            return new TextElement(dir.ToString(), coords , .25f);
        }

        TextElement TargetNPC()
        {
            PointF coords = new PointF() { X = 250, Y = 250 };
            Entity npc = Game.Player.TargetedEntity;

            return new TextElement(npc.ToString(), coords, .25f);
        }
    }
}
