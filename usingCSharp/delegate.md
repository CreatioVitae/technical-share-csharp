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