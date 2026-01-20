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
        private int firstRoll = 0;
        CheckBox[] holdCheckBox;
        private bool scoreFixedThisTurn = false;
        private bool scoreTableOpened = false;
        private Point originalScoreGridLocation;
        private Size originalScoreGridSize;
        private PictureBox[] dicePics;




        public Form1()
        {

            InitializeComponent();// ← これがないと何も表示されない
            richTextBox1.ReadOnly = true;
            this.KeyPreview = true;              // キー入力をフォームで先に受け取る
            this.KeyDown += Form1_KeyDown;       // キー押下イベントを登録

            //ホールドを配列に入れる
            holdCheckBox = new CheckBox[]
            {
              chkHold1, chkHold2, chkHold3, chkHold4, chkHold5
            };
            dicePics = new PictureBox[]
            {
              picDice1, picDice2, picDice3, picDice4, picDice5
            };

            // 初期状態ではホールド不可にする（サイコロ未表示のため）
            foreach (var chk in holdCheckBox)
            {
                chk.Checked = false;
                chk.Enabled = false;
            }


            this.Load += Form1_Load; // フォームロードイベントを紐付け

            // コンストラクタ内、holdCheckBox と dicePics を初期化した直後に追加
            for (int i = 0; i < holdCheckBox.Length; i++)
            {
                int idx = i; // クロージャ対策
                holdCheckBox[idx].AutoCheck = false; // 自動で Checked を切り替えない
                holdCheckBox[idx].MouseClick += (s, ev) =>
                {
                    if (ev.Button == MouseButtons.Left && holdCheckBox[idx].Enabled)
                    {
                        // Enabled を確認してから自前でトグルする
                        holdCheckBox[idx].Checked = !holdCheckBox[idx].Checked;
                    }
                };
            }

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

        // ダイス画像の有無・値に応じてホールドの可否を更新する共通処理
        private void UpdateHoldAvailability()
        {
            for (int i = 0; i < holdCheckBox.Length; i++)
            {
                // 表示は diceValues を基準に判定（0 = 未表示/未設定）
                bool hasDice = diceValues[i] != 0;
                holdCheckBox[i].Enabled = hasDice;
                // Checked の自動解除はここでは行わない（明示的に解除する場面で行う）
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            

            if (rollCount >= 3)
            {
                MessageBox.Show("スコアを入れろ");
                
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
            // Application.Restart();

            // ダイス表示が更新されたのでホールドの有効/無効を更新
            UpdateHoldAvailability();






            //残り回数の処理
            nokori.Text = $"残り {2- rollCount} 回";
            rollCount++;
            firstRoll++;






            //各目のスコアを更新
            for (int number = 1; number <= 6; number++)
            {
                var numberRow = scoreRows.First(r => r.Category == $"{number}の目");
                if (!numberRow.IsFixed)

                    numberRow.CalcNumberScore(diceValues, number);
            }
            // 「チョイス」行を探して更新
            var choiceRow = scoreRows.First(r => r.Category == "チョイス");
            if (!choiceRow.IsFixed)
                choiceRow.Choice(diceValues);

            // スリーカードのスコアを計算して格納
            var threeCardRow = scoreRows.First(r => r.Category == "スリーカード");
            if (!threeCardRow.IsFixed)
                threeCardRow.CalacThreeCardScore(diceValues);

            // フォーカードのスコアを計算して格納
            var fourCardRow = scoreRows.First(r => r.Category == "フォーカード");
            if (!fourCardRow.IsFixed)
                fourCardRow.CalacFourCardScore(diceValues);

            // フルハウスのスコアを計算して格納
            var fullHouseRow = scoreRows.First(r => r.Category == "フルハウス");
            if (!fullHouseRow.IsFixed)
                fullHouseRow.CalacFullHouseScore(diceValues);

            // 小ストレートのスコアを計算して格納
            var smallStraightRow = scoreRows.First(r => r.Category == "小ストレート");
            if (!smallStraightRow.IsFixed)
                smallStraightRow.CalacSmallStraightScore(diceValues);

            // 大ストレートのスコアを計算して格納
            var largeStraightRow = scoreRows.First(r => r.Category == "大ストレート");
            if (!largeStraightRow.IsFixed)
                largeStraightRow.CalacLargeStraightScore(diceValues);

            // ヨットのスコアを計算して格納
            var yachtRow = scoreRows.First(r => r.Category == "ヨット");
            if (!yachtRow.IsFixed)
                yachtRow.CalacYachtScore(diceValues);







            // 合計行のスコアを計算して格納
            var totalRow = scoreRows.First(r => r.Category == "合計");
            // 合計対象：Scoreがnullでなく、"合計"以外の行
            totalRow.Score = scoreRows
                .Where(r => r.Category != "合計" && r.Score.HasValue)
                .Sum(r => r.Score.Value);

            // DataGridView を更新
            scoreGrid.Refresh();// データの変更を反映←これがないと表に表示されない

            scoreFixedThisTurn = false; // 新しいターン開始、まだスコア未確定





          



        }

        private void scoreGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)//クリックでスコアに反映   
        {
            if (scoreFixedThisTurn)
            {
                if (!alreadyScoredMessageShown)
                {
                    MessageBox.Show("このターンではすでにスコアを入力しました");
                    alreadyScoredMessageShown = true; // 一度表示したらフラグを立てる
                }
                return;


            }
            var row = scoreRows[e.RowIndex];
            if (e.RowIndex < 0) return;// ヘッダー行は無視

            if (rollCount == 0 && firstRoll == 0)
            {
                if (!rollMessageShown)
                {
                    MessageBox.Show("まずはダイスを振ってください");
                    rollMessageShown = true; // 一度表示したらフラグを立てる
                }
                return;
            }




            if (row.IsFixed || row.Category == "合計") return; // すでに確定している行や合計行は無視


          


            //カテゴリーごとにスコアを確定
            if (row.Category.EndsWith("の目"))
            {
                int number = int.Parse(row.Category.Substring(0, 1));
                row.CalcNumberScore(diceValues, number);
            }
            else if (row.Category == "スリーカード")
            {
                row.CalacThreeCardScore(diceValues);
            }
            else if (row.Category == "フォーカード")
            {
                row.CalacFourCardScore(diceValues);
            }
            else if (row.Category == "フルハウス")
            {
                row.CalacFullHouseScore(diceValues);
            }
            else if (row.Category == "小ストレート")
            {
                row.CalacSmallStraightScore(diceValues);
            }
            else if (row.Category == "大ストレート")
            {
                row.CalacLargeStraightScore(diceValues);
            }
            else if (row.Category == "ヨット")
            {
                row.CalacYachtScore(diceValues);
            }
            else if (row.Category == "チョイス")
            {
                row.Choice(diceValues);
            }
            row.IsFixed = true; // スコアを確定
            rollCount = 0; // ロール回数をリセット
            scoreFixedThisTurn = true; // このターンはもう入力済み
            nokori.Text = $"残り 3回"; // 残り回数をリセット
            scoreGrid.Refresh();



            
           



            // ★ 振り直したらホールド解除
            foreach (var chk in holdCheckBox)
            {
                chk.Checked = false;
            }

            for (int i = 0; i < diceValues.Length; i++)
            {
                diceValues[i] = 0;
                dicePics[i].Image = null; // または初期画像
            }
            // ホールドはダイスが表示されていないので無効化して解除
            UpdateHoldAvailability();


            // すべてのスコアが確定済みか判定（合計行以外）
            bool allFixed = scoreRows
                .Where(r => r.Category != "合計")
                .All(r => r.IsFixed);

            // すべて確定済みなら合計を自動計算して中央に表示
            if (allFixed)
            {
                var totalRow = scoreRows.First(r => r.Category == "合計");
                totalRow.Score = scoreRows
                    .Where(r => r.Category != "合計" && r.Score.HasValue)
                    .Sum(r => r.Score.Value);
                scoreGrid.Refresh();

                // 合計点を画面中央に大きく表示
                using (Form resultForm = new Form())
                {
                    resultForm.StartPosition = FormStartPosition.CenterScreen;
                    resultForm.FormBorderStyle = FormBorderStyle.None;
                    resultForm.BackColor = Color.White;
                    resultForm.Width = 400;
                    resultForm.Height = 200;

                    Label lbl = new Label();
                    lbl.Text = $"合計点: {totalRow.Score}";
                    lbl.Font = new Font("Meiryo UI", 32, FontStyle.Bold);
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Dock = DockStyle.Fill;
                    resultForm.Controls.Add(lbl);

                    // 2秒後に自動で閉じる
                    Timer timer = new Timer();
                    timer.Interval = 3000;
                    timer.Tick += (s, args) => { resultForm.Close(); timer.Dispose(); };
                    timer.Start();

                    resultForm.ShowDialog();

                    var result = MessageBox.Show(
                                 "ゲームを続けますか？",
                                 "確認", 
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // 続ける処理
                        Application.Restart();
                    }
                    else
                    {
                        // やめる処理
                    }
                }
            }






        }




        private void scoreGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)//クリックし終わったセルの色を変える
        {
            var row = scoreRows[e.RowIndex];
            if (row.IsFixed)
            {
                e.CellStyle.BackColor = Color.LightGray;
            }
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
        private bool alreadyScoredMessageShown;
        private bool rollMessageShown;
      

        private void Form1_Load(object sender, EventArgs e)　　//フォームロードイベント
        {
            scoreGrid.AutoGenerateColumns = true;
            scoreGrid.RowHeadersVisible = false; // ← 左のスペースを消す

            this.ActiveControl = scoreGrid; // ← 表にフォーカスを移す

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

            scoreGrid.CellFormatting += scoreGrid_CellFormatting;

            //固定・編集不可にする
            scoreGrid.AllowUserToResizeColumns = false;
            scoreGrid.AllowUserToOrderColumns = false;
            scoreGrid.AllowUserToResizeRows = false;
            scoreGrid.AllowUserToAddRows = false;
            scoreGrid.AllowUserToDeleteRows = false;
            scoreGrid.ReadOnly = true;
            scoreGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            scoreGrid.MultiSelect = false;
            scoreGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

         





            // 元の位置を記録
            originalScoreGridLocation = scoreGrid.Location;

            originalScoreGridSize = scoreGrid.Size;

            


            scoreGrid.CellClick += scoreGrid_CellContentClick;//クリックイベントを紐付け

            // 初期状態のホールド有効/無効を更新（ダイス未表示なので無効のまま）
            UpdateHoldAvailability();



        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)// スペースキーでダイスを振る
        {
            if (e.KeyCode == Keys.Space)
            {
                button1.PerformClick(); // ダイスを振るボタンをクリックしたことにする


            }
            if (e.KeyCode == Keys.R && e.Shift)// Rキーでリセット
            {
                Application.Restart();// アプリケーションを再起動
            }
            if (e.KeyCode == Keys.Escape)// ESCキーで終了
            {
                Application.Exit();// アプリケーションを終了
            }
            if (e.KeyCode == Keys.S)//Sの入力で表の拡大
            {


                if (!scoreTableOpened && scoreGrid.CurrentCell != null)
                {
                    // 開く（中央へ移動）
                    scoreGrid.Left = (this.ClientSize.Width - scoreGrid.Width) / 2;
                    scoreGrid.Top = (this.ClientSize.Height - scoreGrid.Height) / 2;
                    scoreGrid.BringToFront();// ほかのコントロールより前面に表示
                    scoreTableOpened = true; 
                    
                    scoreGrid.BringToFront();

                }
                else if (scoreTableOpened)
                {
                    // 閉じる（元の位置へ戻す）
                    scoreGrid.Location = originalScoreGridLocation;
                    scoreTableOpened = false;

                }

            }

            switch (e.KeyCode)//1〜5キーでホールドのON/OFF切り替え   
            {
                case Keys.D1: 
                    if (chkHold1.Enabled) chkHold1.Checked = !chkHold1.Checked;
                    break;
                case Keys.D2:
                    if (chkHold2.Enabled) chkHold2.Checked = !chkHold2.Checked;
                    break;
                case Keys.D3:
                    if (chkHold3.Enabled) chkHold3.Checked = !chkHold3.Checked;
                    break;
                case Keys.D4:
                    if (chkHold4.Enabled) chkHold4.Checked = !chkHold4.Checked;
                    break;
                case Keys.D5:
                    if (chkHold5.Enabled) chkHold5.Checked = !chkHold5.Checked;
                    break;
            }


        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)//説明文
        {
           

        }
    }

}






        

      


    


