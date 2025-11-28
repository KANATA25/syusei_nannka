using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace おためめ
{
    public partial class Form1 : Form
    {

        private int[] icons8_dice_one_100 = new int[5];
        private int rollCount = 0;
        private Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();   // ← これがないと何も表示されない
        }

        


        private void RollDice(CheckBox chk, int index)
        {
            if (!chk.Checked)
            {
                icons8_dice_one_100[index] = rnd.Next(1, 7);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (rollCount >= 3)
            {
                MessageBox.Show("三回まで");
                return;
            }
            //ホールドの処理
            RollDice(chkHold1, 0);
            RollDice(chkHold2, 1);
            RollDice(chkHold3, 2);
            RollDice(chkHold4, 3);
            RollDice(chkHold5, 4);

            picDice1.Image = Properties.Resources.icons8_dice_two_100;
            picDice2.Image = Properties.Resources.icons8_dice_one_100;
            picDice3.Image = Properties.Resources.icons8_dice_three_100;
            picDice4.Image = Properties.Resources.icons8_dice_one_100;
            picDice5.Image = Properties.Resources.icons8_dice_one_100;





            nokori.Text = $"残り {2 - rollCount} 回";
            rollCount++;
        }

    
        private void PicDice_Load(object sender, EventArgs e)
        {
            picDice1.Image = Properties.Resources.icons8_dice_two_100;
            picDice2.Image = Properties.Resources.icons8_dice_one_100;
            picDice3.Image = Properties.Resources.icons8_dice_three_100;
            picDice4.Image = Properties.Resources.icons8_dice_one_100;
            picDice5.Image = Properties.Resources.icons8_dice_one_100;
        }

     
    }
    }

