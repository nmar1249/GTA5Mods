using GTA;
using GTA.Math;
using GTA.UI;
using System;
using System.Drawing;
using System.Windows.Forms; //used to get input

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

            //init location info
            locationInfo = new ContainerElement();
            targetInfo = new ContainerElement();
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
                GetTargetInfo();
                targetInfo.Draw();
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
            locationInfo.Items.Clear();

            locationInfo.Items.Insert(0, Direction());
            locationInfo.Items.Insert(1, GetStreet(Game.Player.Character.Position));
        }

        void GetTargetInfo()
        {
            targetInfo.Items.Clear();

            targetInfo.Items.Insert(0, GetTargetName());
            targetInfo.Items.Insert(1, GetHealth());
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
        TextElement GetTargetName()
        {       
            PointF coords = new PointF() { X = 640, Y = 0 };
            Ped npc = (Ped)Game.Player.TargetedEntity;
            Color color = new Color();

            string name = ((PedHash)npc.Model.Hash).ToString();

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

            return new TextElement(name, coords, .5f, color, GTA.UI.Font.Pricedown, GTA.UI.Alignment.Center, false, true);

        }

        TextElement GetHealth()
        {
            PointF coords = new PointF() { X = 640, Y = 10 };
            Ped npc = (Ped)Game.Player.TargetedEntity;
            Color color = new Color();

            string health = "";

            //get health percentage
            float healthPerc = (npc.HealthFloat / npc.MaxHealthFloat) * 100;
            healthPerc = (float)Math.Round(healthPerc, 0);

            if (healthPerc == 0)
            {
                health = "\nDEAD";
                color = Color.Red;
            }
            else
            {
                health = '\n' + healthPerc.ToString() + '%';
            }

            return new TextElement(health, coords, .5f, color, GTA.UI.Font.Pricedown, GTA.UI.Alignment.Center, false, true);
        }

        TextElement GetStreet(Vector3 pos)
        {
            PointF coords = new PointF() { X = 25, Y = 550 };
            string str = World.GetStreetName(pos);

            return new TextElement(str, coords, .5f, Color.White, GTA.UI.Font.Pricedown, GTA.UI.Alignment.Left, false, true);
        }
        #endregion
    }
}
