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

       ScoreRow scoreRow;
        private int rollCount = 0;
        CheckBox[] holdCheckBox;

        public Form1()
        {

            InitializeComponent();// ← これがないと何も表示されない
            //ホールドを配列に入れる
            holdCheckBox = new CheckBox[]
            {
              chkHold1, chkHold2, chkHold3, chkHold4, chkHold5
            };


            this.Load += Form1_Load; // フォームロードイベントを紐付け

        }

       public int[] diceValues = new int[5];
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
            //picDiceを配列に
            PictureBox[] pics = { picDice1, picDice2, picDice3, picDice4, picDice5 };
            //各picsに画像を割り当てる
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


            //ダイスのランダム処理
            for (int i = 0; i < 5; i++)
            {
                RollDice(holdCheckBox[i], i);
                UpdateDiceImage(i);
            }





            //残り回数の処理
            nokori.Text = $"残り {2 - rollCount} 回";
            rollCount++;

            //各目のスコアを更新
            for (int number = 1; number <= 6; number++)
            {
                var numberRow = scoreRows.First(r => r.Category == $"{number}の目");
                numberRow.CalcNumberScore(diceValues, number);
            }
            // 「チョイス」行を探して更新
            var choiceRow = scoreRows.First(r => r.Category == "チョイス");
            choiceRow.Choice(diceValues);

            // DataGridView に反映
            scoreGrid.Refresh();


            //各目のスコアを更新
            for (int number = 1; number <= 6; number++)
            {
                var numberRow = scoreRows.First(r => r.Category == $"{number}の目");
                numberRow.CalcNumberScore(diceValues, number);
            }

            // スリーカードのスコアを計算して格納
            var threeCardRow = scoreRows.First(r => r.Category == "スリーカード");
            threeCardRow.CalacThreeCardScore(diceValues);

            // フォーカードのスコアを計算して格納
            var fourCardRow = scoreRows.First(r => r.Category == "フォーカード");
            fourCardRow.CalacFourCardScore(diceValues);

            // フルハウスのスコアを計算して格納
            var fullHouseRow = scoreRows.First(r => r.Category == "フルハウス");
            fullHouseRow.CalacFullHouseScore(diceValues);

            // 小ストレートのスコアを計算して格納
            var smallStraightRow = scoreRows.First(r => r.Category == "小ストレート");
            smallStraightRow.CalacSmallStraightScore(diceValues);

            // 大ストレートのスコアを計算して格納
            var largeStraightRow = scoreRows.First(r => r.Category == "大ストレート");
            largeStraightRow.CalacLargeStraightScore(diceValues);

            // ヨットのスコアを計算して格納
            var yachtRow = scoreRows.First(r => r.Category == "ヨット");
            yachtRow.CalacYachtScore(diceValues);





            // DataGridView に反映
            scoreGrid.Refresh();

            // 合計行のスコアを計算して格納
            var totalRow = scoreRows.First(r => r.Category == "合計");
            // 合計対象：Scoreがnullでなく、"合計"以外の行
            totalRow.Score = scoreRows
                .Where(r => r.Category != "合計" && r.Score.HasValue)
                .Sum(r => r.Score.Value);

            // DataGridView を更新
            scoreGrid.Refresh();





        }







        private void PicDice_Load(object sender, EventArgs e)
        {
            //picDice1.Image = Properties.Resources.icons8_dice_two_100;
            //picDice2.Image = Properties.Resources.icons8_dice_one_100;
            //picDice3.Image = Pro/perties.Resources.icons8_dice_three_100;
            //picDice4.Image = Properties.Resources.icons8_dice_one_100;
            //picDice5.Image = Properties.Resources.icons8_dice_one_100;
        }

        // フィールドに保持しておく
        private List<ScoreRow> scoreRows;


        private void Form1_Load(object sender, EventArgs e)
        {
            scoreGrid.AutoGenerateColumns = true;
            scoreGrid.RowHeadersVisible = false; // ← 左のスペースを消す


             scoreRows = new List<ScoreRow>
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

            scoreGrid.ReadOnly = true;




        }
    }
}

