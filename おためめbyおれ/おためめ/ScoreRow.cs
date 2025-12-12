using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace おためめ
{
    internal class ScoreRow
    { Form1 _form1;
        public int sum;
        public string Category { get; set; }  // カテゴリ名（例: "1の目"）
        public int? Score { get; set; }// スコア（未入力なら null
                                       // ダイスの合計を計算してスコアに反映  
       public bool IsFixed { get; set; } // スコアが確定しているかどうか 

        //1~6の目のスコア計算   
        public void CalcNumberScore(int[] diceValues, int number)
        {
            // 指定したnumber（1〜6）の出目の合計を計算
            Score = diceValues.Where(v => v == number).Sum();
        }
        //3カードのスコア計算
        public void CalacThreeCardScore(int[] diceValues)
        {
            for (int i = 1; i<= 6; i++)
            {
                if(diceValues.Count(v=> v == i) >=3)
                {
                    Score = diceValues.Sum();
                    return;
                } 
            }
            Score = 0;
        }
        //4カードのスコア計算
        public void CalacFourCardScore(int[] diceValues)
        {
            for (int i = 1; i <= 6; i++)
            {
                if (diceValues.Count(v => v == i) >= 4)
                {
                    Score = diceValues.Sum();
                    return;
                }
            }
            Score = 0;
        }
        //フルハウスのスコア計算
        public void CalacFullHouseScore(int[] diceValues)
        {
            var groups = diceValues.GroupBy(v => v).Select(g => g.Count()).OrderByDescending(c => c).ToArray();
            if (groups.Length == 2 && groups[0] == 3 && groups[1] == 2)
            {
                Score = 25;
            }
            else
            {
                Score = 0;
            }
        }
        //小ストレートのスコア計算
        public void CalacSmallStraightScore(int[] diceValues)
        {
            var unique = diceValues.Distinct().OrderBy(x => x).ToArray();
            // 1-2-3-4, 2-3-4-5, 3-4-5-6 のいずれか
            if (unique.Contains(1) && unique.Contains(2) && unique.Contains(3) && unique.Contains(4) ||
                unique.Contains(2) && unique.Contains(3) && unique.Contains(4) && unique.Contains(5) ||
                unique.Contains(3) && unique.Contains(4) && unique.Contains(5) && unique.Contains(6))
            {
                Score = 30;
            }
            else
            {
                Score = 0;
            }
        }
        //大ストレートのスコア計算
        public void CalacLargeStraightScore(int[] diceValues)
        {
            var unique = diceValues.Distinct().OrderBy(x => x).ToArray();
            // 1-2-3-4-5 または 2-3-4-5-6
            if ((unique.Length == 5 && unique[0] == 1 && unique[4] == 5) ||
                (unique.Length == 5 && unique[0] == 2 && unique[4] == 6))
            {
                Score = 40;
            }
            else
            {
                Score = 0;
            }
        }
        //ヨットのスコア計算
        public void CalacYachtScore(int[] diceValues)
        {
            if (diceValues.Distinct().Count() == 1)
            {
                Score = 50;
            }
            else
            {
                Score = 0;
            }
        }




        //チョイスのスコア計算

        public void Choice(int[] diceValues)
        {
            int sum = diceValues.Sum();
            Score = sum;   // ← Score に代入
        }
        


    }
}
