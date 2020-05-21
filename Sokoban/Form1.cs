using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Sokoban
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        PictureBox box, box2, Place, Walls, player;
        int playerX, playerY, WIN = 0, theme = 1, lvl = 1, WallCount = 1, BoxCount = 1;
        bool HODI;
        string BOXimg,PLACEimg,inPLACEimg,PLAYERimg;

        private void этоМенюToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lvl1_Click(object sender, EventArgs e)
        {
            var Whichlvl = sender as ToolStripMenuItem;
            lvl = Convert.ToInt32(Whichlvl.Text);
            startGame();
            WIN = 0;
            //lvl = Convert.ToInt32(Whichlvl.Name.Substring(2, 1));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Visible = false;
            timer1.Enabled = false;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e){theme = 3;Visial(); }

        private void toolStripMenuItem3_Click(object sender, EventArgs e){theme = 2;Visial(); }

        private void toolStripMenuItem2_Click(object sender, EventArgs e){theme = 1;Visial();}

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            HODI = true;
             playerX = player.Location.X;
             playerY = player.Location.Y;
            switch (e.KeyCode)
            {
                case Keys.S: findBox(0, 50);break;
                case Keys.W: findBox(0, -50); break;
                case Keys.D: findBox(50, 0); break;
                case Keys.A: findBox(-50, 0); break;
            }
        }

        public void findBox(int ChangeX, int ChangeY)
        {
            for (int i = 1; i < BoxCount; i++)
            {
                box = Controls["Box" + i] as PictureBox;
                if (box.Location.X == playerX + ChangeX && box.Location.Y == playerY + ChangeY)
                {
                    for (int j = 1; j < BoxCount; j++)
                    {
                        box2 = Controls["Box" + j] as PictureBox;
                        if (box2.Location.X == playerX + ChangeX * 2 && box2.Location.Y == playerY + ChangeY * 2)
                        {
                            HODI = false; break;
                        }
                    }
                        for (int a = 1; a < WallCount; a++)
                        {
                            Walls = Controls["wallBox" + a] as PictureBox;
                        if (Walls.Location.X == playerX + ChangeX * 2 && Walls.Location.Y == playerY + ChangeY * 2)
                        {
                            HODI = false; break;
                        }
                        }
                        if (HODI)
                        {
                            Color1(PLACEimg, -1);
                            box.Location = new Point(playerX + ChangeX * 2, playerY + ChangeY * 2);
                            Color1(inPLACEimg, 1);
                        }
                break;
                }
            }
            for (int a = 1; a < WallCount; a++)
            {
                Walls = Controls["wallBox" + a] as PictureBox;
                if (Walls.Location.X == playerX + ChangeX && Walls.Location.Y == playerY + ChangeY)
                {
                    HODI = false; break;
                }
            }
            if (HODI) player.Location = new Point(playerX + ChangeX, playerY + ChangeY);
            if (WIN ==  BoxCount-1)
            {
                if (lvl == 5)//
                {
                    DialogResult res = MessageBox.Show("Вы прошли игру\nХотите начать новую игру", "Поздравляю", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (res == DialogResult.Yes)
                    {
                        lvl = 1;
                    }
                    else if (res == DialogResult.No) startGame();
                }
                else
                {
                    DialogResult res = MessageBox.Show("Вы прошли уровень\nхотите начать новый уровень???\nЧтобы пройти на " + (lvl+1) + " нажмите\n 'ДА'\nЧтобы перезапустить " + lvl + " уровень\n 'НЕТ'", "Congretilations", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (res == DialogResult.Yes)
                    {
                        lvl++;
                    }
                    else if (res == DialogResult.No) startGame();
                }
                WIN = 0;
                startGame();
            }
        }
        public void Color1(string color ,int ChangeWin)
        {
            for (int j = 1; j < BoxCount; j++)
            {
                Place = Controls["PLACEbox" + j] as PictureBox;
                if (box.Location == Place.Location)
                {
                    Place.Image = Image.FromFile(color);
                    WIN += ChangeWin;
                    break;
                }
            }
        }
        public void startGame()
        {
            
            for (int i = 1; i < WallCount; i++)
            {
                Controls.Remove(Controls["wallBox" + i]);
            }
                Controls.Remove(Controls["playerBox1"]);
            for (int i = 1; i < BoxCount; i++)
            {
                Controls.Remove(Controls["Box" + i]);
                Controls.Remove(Controls["PLACEbox" + i]);
            }
            Level(lvl);
            Visial();
            label1.Size = new Size(this.Width, this.Height);
            label1.Visible = true;
            label1.Text = "Уровень" + lvl;
            timer1.Enabled = true;
            label1.BringToFront();
        }
        public void Level(int lvl)
        {
            using(StreamReader sr = new StreamReader("levels/level" + lvl + "Walls.txt"))
            {
                WallCount = 1;

                while (!sr.EndOfStream)
                {
                    string[] Cord = sr.ReadLine().Split(',');
                    Walls = new PictureBox()
                    {
                        Name = "wallBox" + WallCount,
                        Size = new Size(50, 50),
                        Location = new Point(Convert.ToInt32(Cord[0]), Convert.ToInt32(Cord[1])),
                        // Image = Image.FromFile(),
                        BackColor = Color.White,
                    };
                    Controls.Add(Walls);
                    WallCount++;
                }
            }
            using(StreamReader sr = new StreamReader("levels/level" + lvl + "Box.txt"))
            {
                BoxCount = 1;
                while (!sr.EndOfStream)
                {
                    string[] BoxOrPlace = sr.ReadLine().Split(';');
                    string[] BoxCord = BoxOrPlace[0].Split(',');
                    box = new PictureBox()
                    {
                        Name = "Box" + BoxCount,
                        Size = new Size(50, 50),
                        Location = new Point(Convert.ToInt32(BoxCord[0]), Convert.ToInt32(BoxCord[1])),
                        SizeMode = PictureBoxSizeMode.StretchImage
                    };
                    Controls.Add(box);
                    box.SendToBack();
                    string[] PlaceCord = BoxOrPlace[1].Split(',');
                    Place = new PictureBox()
                    {
                        Name = "PLACEbox" + BoxCount,
                        Size = new Size(50, 50),
                        Location = new Point(Convert.ToInt32(PlaceCord[0]), Convert.ToInt32(PlaceCord[1])),
                        SizeMode = PictureBoxSizeMode.StretchImage
                    };
                    Controls.Add(Place);
                    Place.BringToFront();
                    BoxCount++;
                }
            }
            using(StreamReader sr = new StreamReader("levels/level" + lvl + "Player.txt"))
            {
                string[] cord1 = sr.ReadLine().Split(',');
                player = new PictureBox()
                {
                    Name = "playerBox1",
                    Size = new Size(50, 50),
                    Location = new Point(Convert.ToInt32(cord1[0]), Convert.ToInt32(cord1[1])),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                Controls.Add(player);
                player.BringToFront();
                cord1 = sr.ReadLine().Split(',');
                this.Size = new Size(Convert.ToInt32(cord1[0]), Convert.ToInt32(cord1[1]));
            }
        }
        public void Visial()
        {
            player = Controls["playerBox1"] as PictureBox;
            BOXimg = "Theme/Theme" + theme + "/Box.jpg";
            PLACEimg = "Theme/Theme" + theme + "/gem.jpg";
            inPLACEimg = "Theme/Theme" + theme + "/BoxOk.jpg";
            PLAYERimg = "Theme/Theme" + theme + "/player.jpg";
            for (int i = 1; i < BoxCount; i++)
            {
                Place = Controls["PLACEbox" + i] as PictureBox;
                for (int j = 1; j < BoxCount; j++)
                {
                    box = Controls["Box" + j] as PictureBox;
                    if (Place.Location == box.Location)//
                    {
                        Place.Image = Image.FromFile(inPLACEimg);
                        break;
                    }
                    else Place.Image = Image.FromFile(PLACEimg);
                }
                box = Controls["Box" + i] as PictureBox;
                box.Image = Image.FromFile(BOXimg);
            }
            player.Image = Image.FromFile(PLAYERimg);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            startGame();
            Visial();
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void играToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startGame();
        }
    }
}
