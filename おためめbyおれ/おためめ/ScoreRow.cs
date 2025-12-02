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
        public void Choice(int[] diceValues)
        {
            int sum = diceValues.Sum();
            Score = sum;   // ← Score に代入
        }
        


    }
}
