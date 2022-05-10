# WIP:delegate
## TL;DR
* delegateは、メソッドを参照するための型を表す。
* a.k.a. 関数ポインターみたいなやつ（みたいなやつ）
* 複数のmethod参照も可能

## C#1.1時代とC#2時代のdelegate 

```cs
using System;

// Delegate Type
delegate void DelegateHoge(int a);

internal class DelegateTest
{
    static void M()
    {
        // Delegate Typeの変数にmethodを代入
        DelegateHoge a = new DelegateHoge(FuncTest);

        a(256); // デリゲートを経由してメソッド[ FuncTest ]を呼び出し

        DelegateHoge b = FuncTest; // new DelegateHoge(FuncTest) と一緒
        b(256);
    }

    static void FuncTest(int n)
    {
        Console.WriteLine($"call ... {nameof(FuncTest)}({n})");
    }
}
```

```cs
delegate void DelegateHoge(int a);
```

は`delegate`型の定義を表す。  
`delegate`型の変数には、 `delegate`と`戻り値`,`in / out Parameters`の一致した定義を持った`method`の参照を代入する事が出来る。 

つまり、
```cs
delegate void DelegateHoge(int a);
```
には`int` の`in parameter`を持つ、戻り値なしの`method`の参照が代入可能。  

### method参照とmethod呼び出しについて
```cs
DelegateHoge a = new DelegateHoge(FuncTest);
```
にて、`a`に`FuncTest`の`参照`を`代入`する。(この時点ではまだmethodは呼び出さない。)

```cs
a(256); // デリゲートを経由してメソッド[ FuncTest ]を呼び出し
```
にて、`a`を経由して、`FuncTest`を`int`の`in parameter` 256を渡して `呼び出す`

https://sharplab.io/#v2:EYLgtghglgdgNAExAagD4AEBMBGAsAKAIHoiACAEQFMAbSgcwgBdLSAVATwAdKCEb6mLdABYK/BswASAezqUAFLEakIASgDcBAksoAnGBGqksY2hMqtKAZ0YEA3gVJPj2AGzHRAWXmrHzh/jOQaQkpgLMbFyUgHYMgJCagA6mgNYMYJSMABbSCIBJDIDHcoCmioBADH7BYeYyciqkALykMJQA7qWC5QroAKyWNhpagSUq8phtrhohZIDjDIBXDIBNDIA/DIATDFmAMr6AjK6A6gyAZgyAhwyAvQyAwwyAkwwA2sYd1soAulmAPiqAzgyAX4qrPX1NUrIswNUnnYzqo7UNL0oLXk7W+qlIgAsGQAAcoAl32KwWAAyG3V6TgAvk9nOg3B4vmdFDBlDBfKjSAFntiAJzyAAkACIAMaGIwAOjZZIMKWkADMQacumj5HYYGjVHSUUEMfg0UA=

### 匿名メソッド式(C#2～)
delegateを使う場合、
* メソッドの定義をする
* 定義したメソッドを参照する

という準備が必須だった。

#### メソッドの定義の煩わしさ
メソッドの定義 / 参照をせずにインスタントに書きたい欲求から生まれたのが匿名メソッドにあたる。  
コンパイラ生成を見てみるとわかるが、`Action / func`を生成する。  
現在の`C#`では`匿名メソッド式`ではなく、`ラムダ式`を利用するが、`メソッドの定義を作らない / メソッドの参照に伴うdeligate 型を定義しない（Action / funcを利用する）`というポイントは匿名メソッド式のタイミングでインストールされた。

https://sharplab.io/#v2:EYLgtghglgdgNAExAagD4AEBMBGAsAKAIHoiACAEQFMAbSgcwgBdLSAVATwAdKCEb6mLdABYK/BswASAezqUAFLEakIASgDcBAksoAnGBGqksY2hMqtKAZ0YEA3gVJPj2AGzHRAWXmrHzh/jOQaQkpgLMbFyUgHYMgJCagA6mgNYMYJSMABbSCIBJDIDHcoCmioBADH7BYeYyciqkALykMJQA7qWC5QroAKyWNhpagSUq8phtrhohZIDjDIBXDIBNDIA/DIATDFmAMr6AjK6A6gyAZgyAhwyAvQyAwwyAkwwA2sYd1soAulmAPiqAzgyAX4qrPX1NUrIswNUnnYzqo7UNL0oLXk7W+qlIgAsGQAAcoAl32KzyC8KCwAGQ26vRKSOcADcILpSABjT58MyCRQwZQwcF2FwATnkABIAEQEwxGAB0nNIdgMKWkADMQacugBfeQ8kWqJkjEWaDHBLFOAmo4ZyoIip7OdBuDxfM7kym+eUBZ7a+nM1nUDlcnkQPmC0FnVRiiVS9HqggioA

### ラムダ式（C#3～）
匿名メソッド式では
```cs
var c = delegate(int n) { Console.WriteLine($"call ... {nameof(FuncTest)}({n})"); };
```

と書いていたものを、

```cs
Action<int> action = (int n) =>  { Console.WriteLine($"call ... {nameof(FuncTest)}({n})"); };
```

と書けるようになった。

尚、

```cs
Action<int> actionAlt = (int n) =>  Console.WriteLine($"call ... {nameof(FuncTest)}({n})");
```

とも書けるし、C#10以降では

```cs
var actionAltCS10 = void (int n) =>  Console.WriteLine($"call ... {nameof(FuncTest)}({n})");
```

とも書けるが、コンパイラ生成後のコードはいずれも同じコードに展開される。

#### ラムダ式 is 何
ラムダ式は匿名メソッド式と同じく、`メソッドの定義を作らない / メソッドの参照に伴うdeligate 型を定義しない（Action / funcを利用する）` 使い捨ての関数をインスタントに定義するための仕組みにあたる。 ※ ラムダ式は`Expression Tree`の生成にも利用するが、`delegate`のdocが大きくなりすぎるため、詳細はラムダ式に関するdocにて解説する。

https://sharplab.io/#v2:EYLgtghglgdgNAExAagD4AEBMBGAsAKAIHoiACAEQFMAbSgcwgBdLSAVATwAdKCEb6mLdABYK/BswASAezqUAFLEakIASgDcBAksoAnGBGqksY2hMqtKAZ0YEA3gVJPj2AGzHRAWXmrHzh/jOQaQkpgLMbFyUgHYMgJCagA6mgNYMYJSMABbSCIBJDIDHcoCmioBADH7BYeYyciqkALykMJQA7qWC5QroAKyWNhpagSUq8phtrhohZIDjDIBXDIBNDIA/DIATDFmAMr6AjK6A6gyAZgyAhwyAvQyAwwyAkwwA2sYd1soAulmAPiqAzgyAX4qrPX1NUrIswNUnnYzqo7UNL0oLXk7W+qlIgAsGQAAcoAl32KzyC8KCwAGQ26vRKSOcADcILpSABjT58MyCRQwZQwcF2FwATnkABIAEQEwxGAB0nNIdgMKWkADMQacugBfeQ8kWqJkjEWaDHBLFOAmo4Zy56KlyYAA8SgAfCoCYwoNIYJ9yZTwVV9dy6YyWWzSJz2dzeZQBUKwWKJVKZWq+hr0NhtXqDUaTQBBajKGrm2qW62B+nM1nUDlcnkQPmC0FnVRemCS6V+zHyoK4/EQQ3GmCRxgAYQAytgAAyfESkWNU6oJ7BJ+2px3p13unOi8UFn3F0gip7OQPudujxid3zygLPRN2lNp50ZrMe3P5wvooIz/AioA
