# using Top Level Statements

```namespace```,```class```の記述を省略し、エントリーポイントを直接記述出来るようになる仕組み。

## SharpLab
https://sharplab.io/#v2:EYLgtghglgdgNAExAagD4Hp0AEDMACAJwFMBjAewITwAkyBzIgCiwEYAGPAMQFc6I48rDgAUoATzIBKPAG8AvgFgAUMtYA6VgE5GAcSIAXWg0aTJAbmWZ1WxhAJ0AzmoAyRGHX0ALc5eybBLACsGgBsagAiRAA2EGKMLD4qSlYA7HhsFkmsIQEceob0TNIAvAB8eABEnoUVZnj1eMq+2bl4+UZMQnhkAG5EBARQCEQAyvqD7iXlvf2Dw2MTdGZAA

## 制約事項
* トップレベルステートメントは、```1Project```に```1ファイル```しか存在できない。
* クラスや名前空間の定義よりも上にしか書けない。
* トップレベルステートメント上で記述するメソッドはローカルメソッド扱いとなるため、ローカルメソッドの制約事項が適用される。
* 名前空間はなしでコンパイラ生成される。
* ```await``` キーワードがトップレベルステートメント上にあれば ```async Main``` として、なければ ```Main``` としてコンパイラ生成される。
* ```return``` は```Main```と同じく```int```が利用可能。書くことで、```int Main``` もしくは ```Task<int> Main``` にコンパイラ生成される。