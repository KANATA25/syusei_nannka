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
            this.Load += Form1_Load; // フォームロードイベントを紐付け
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
            //ダイスの表示ランダム分かんねえ
            picDice1.Image = Properties.Resources.icons8_dice_two_100;
            picDice2.Image = Properties.Resources.icons8_dice_one_100;
            picDice3.Image = Properties.Resources.icons8_dice_three_100;
            picDice4.Image = Properties.Resources.icons8_dice_one_100;
            picDice5.Image = Properties.Resources.icons8_dice_one_100;




            //残り回数の処理
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



        private void Form1_Load(object sender, EventArgs e)
        {
            scoreGrid.AutoGenerateColumns = true;
            scoreGrid.RowHeadersVisible = false; // ← 左のスペースを消す


            var scoreRows = new List<ScoreRow>
            {
                new ScoreRow { Category = "1の目", Score = null },
                new ScoreRow { Category = "2の目", Score = null },
                new ScoreRow { Category = "3の目", Score = null },
                new ScoreRow { Category = "4の目", Score = null },
                new ScoreRow { Category = "5の目", Score = null },
                new ScoreRow { Category = "6の目", Score = null },
                new ScoreRow { Category = "スリーカード", Score = null },
                new ScoreRow { Category = "フォーカード", Score = null },
                new ScoreRow { Category = "フルハウス", Score = null },
                new ScoreRow { Category = "小ストレート", Score = null },
                new ScoreRow { Category = "大ストレート", Score = null },
                new ScoreRow { Category = "ヨット", Score = null },
                new ScoreRow { Category = "チョイス", Score = null },
                new ScoreRow { Category = "合計", Score = 0 }
            };

            scoreGrid.DataSource = scoreRows;

            // 列の見出しを日本語に変更
            scoreGrid.Columns[nameof(ScoreRow.Category)].HeaderText = "カテゴリ";
            scoreGrid.Columns[nameof(ScoreRow.Score)].HeaderText = "スコア";

            scoreGrid.Columns[nameof(ScoreRow.Category)].Frozen = true;
            scoreGrid.AllowUserToResizeRows = false;


        }
    }
}

