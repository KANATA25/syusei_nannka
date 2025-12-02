using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace おためめ
{
    internal class ScoreRow
    {
        public string Category { get; set; }  // カテゴリ名（例: "1の目"）
        public int? Score { get; set; }       // スコア（未入力なら null
    }
}
