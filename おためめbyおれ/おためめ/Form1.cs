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

       
        private int rollCount = 0;
        CheckBox[] holdCheckBox;

        public Form1()
        {
            InitializeComponent();// ← これがないと何も表示されない

            holdCheckBox = new CheckBox[]
       {
        chkHold1, chkHold2, chkHold3, chkHold4, chkHold5
       };

        }

        int[] diceValues = new int[5];
        Random rnd = new Random();

        private void RollDice(CheckBox chk, int index)
        {
            if (!chk.Checked)
            {
                diceValues[index] = rnd.Next(1, 7); // 1〜6 の値を入れる
            }
        }

        private void UpdateDiceImage(int index)
        {
            PictureBox[] pics = { picDice1, picDice2, picDice3, picDice4, picDice5 };

            switch (diceValues[index])
            {
                case 1: pics[index].Image = Properties.Resources.icons8_dice_one_100; break;
                case 2: pics[index].Image = Properties.Resources.icons8_dice_two_100; break;
                case 3: pics[index].Image = Properties.Resources.icons8_dice_three_100; break;
                case 4: pics[index].Image = Properties.Resources.icons8_dice_four_100; break;
                case 5: pics[index].Image = Properties.Resources.icons8_dice_five_100; break;
                case 6: pics[index].Image = Properties.Resources.icons8_dice_six_100; break;
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (rollCount >= 100)
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
            //ダイスの表示ランダム分かんねえ

            for (int i = 0; i < 5; i++)
            {
                RollDice(holdCheckBox[i], i);
                UpdateDiceImage(i);
            }





            //残り回数の処理
            nokori.Text = $"残り {2 - rollCount} 回";
            rollCount++;
        }

       

        private void PicDice_Load(object sender, EventArgs e)
        {
            //picDice1.Image = Properties.Resources.icons8_dice_two_100;
            //picDice2.Image = Properties.Resources.icons8_dice_one_100;
            //picDice3.Image = Pro/perties.Resources.icons8_dice_three_100;
            //picDice4.Image = Properties.Resources.icons8_dice_one_100;
            //picDice5.Image = Properties.Resources.icons8_dice_one_100;
        }

     
    }
}

