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

        ContainerElement locationInfo;
        ContainerElement targetInfo;

        #region main methods

        public Main()
        {
            //determines tick rate for script
            Interval = 10;
            Tick += OnTick;
            KeyDown += OnKeyDown;
            Notification.Show(this.Name + "has launched.");
        }

        //event runs every tick interval (set as MS)
        void OnTick(object sender, EventArgs e)
        {

            //update current direction
            GetLocationInfo();
            locationInfo.Draw();


            //show details of targeted NPC
            if (Game.Player.TargetedEntity != null && Game.Player.TargetedEntity.EntityType == EntityType.Ped)
            {
                TargetNPC().Draw();
            }
            
            //show speed if in vehicle
        }

        //event triggered when any key is pressed
        void OnKeyDown(object sender, KeyEventArgs e)
        {
        }

        #endregion

        #region container methods
        void GetLocationInfo()
        {
            locationInfo.Items.Add(Direction());
            locationInfo.Items.Add(GetStreet(Game.Player.Character.Position));
        }
        #endregion

        #region helper methods

        TextElement Direction()
        {
            PointF coords = new PointF() { X = 25, Y = 500 };
            string direction = "";

            var dir = Game.Player.Character.ForwardVector;

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

            //white color, left alignment. shadow = false, outline = true
            return new TextElement("DIR: " + direction, coords , .5f, Color.White, GTA.UI.Font.Pricedown, GTA.UI.Alignment.Left, false, true);
        }

        /// <summary>
        /// Displays data about the targeted NPC
        /// --------------------------
        /// Name:
        /// Health:
        /// Relationship:
        /// -------------------------
        /// always potential for more
        /// </summary>
        /// <returns></returns>
        TextElement TargetNPC()
        {       
            PointF coords = new PointF() { X = 640, Y = 0 };
            Ped npc = (Ped)Game.Player.TargetedEntity;
            Color color = new Color();

            string str = "";
            string name = ((PedHash)npc.Model.Hash).ToString();
            string health = "";

            //get relation between player and targeted ped
            Relationship rel = Game.Player.Character.GetRelationshipWithPed(npc);
            
            //use relationship to determine text color
            switch(rel)
            {
                case Relationship.Respect:
                case Relationship.Like:
                case Relationship.Companion:
                    color = Color.Green;
                    break;
                case Relationship.Dislike:
                    color = Color.LightPink;
                    break;
                case Relationship.Hate:
                    color = Color.Red;
                    break;
                case Relationship.Neutral:
                case Relationship.Pedestrians:
                    color = Color.White;
                    break;

            }

            //get health percentage
            int healthPerc = (npc.Health / npc.MaxHealth) * 100;
            health = '\n' + healthPerc.ToString() + '%';

            str = name + health;
            return new TextElement(str, coords, .5f, color, GTA.UI.Font.Pricedown, GTA.UI.Alignment.Center, false, true);

        }

        TextElement GetStreet(Vector3 pos)
        {
            PointF coords = new PointF() { X = 25, Y = 500 };
            string str = World.GetStreetName(pos);

            return new TextElement(str, coords, .5f, Color.White, GTA.UI.Font.Pricedown, GTA.UI.Alignment.Left, false, true);
        }
        #endregion
    }
}
