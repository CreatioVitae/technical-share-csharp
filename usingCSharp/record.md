# Using Record

## How to Record
C#9で追加された新しい参照型。

SharpLab等でC#の展開を見るとわかるが、```IEquatable<T>```を実装した```class```になる。

尚、値型のrecordを定義する方法はC# 9 GA 時点では存在していない。


値ベースの等値性が使用される仕様。
[[https://docs.microsoft.com/ja-jp/dotnet/csharp/tutorials/exploration/records#characteristics-of-records]]

## Default Immutable

### classを定義する場合

```
public class Hoge {
    public string Fuga { get; init; }

    public string Piyo { get; init; }

    public Hoge(string fuga, string piyo) {
        Fuga = fuga;
        Piyo = piyo;
    }
}
```

上記のように、メンバーとなるPropsを定義し、VS2019でのQuickActionのSupportはあるにせよ、Constructorを定義するのは少々面倒だった。


### recordで定義する場合

```
public record Hoge(string Fuga, string Piyo) {
}
```

```record```の定義の後ろにPropsの定義(Primary ctor)をすれば、```ctor```と ```get; init;``` の```Props```として展開される。

```record```は既定の定義だと変更が不可。（```get; init;```）
```
class program { 
    public void Main(string[] args) {
        //ctor、Initializerだと、書き換え可能。
        var hoge = new Hoge("fuga", "piyo") { Fuga = "fugaHoge", Piyo = "piyoHoge" };

        //ここだとビルドエラー
        hoge.Fuga = "ngFuga";
        hoge.Piyo = "ngPiyo";
    }
}
```

この辺はrecord自体というよりは、アクセサーにinitのみのsetterが設定されたコードが展開されるため。（initのみのsetterの詳細については別のDocにて解説を行う。）

ただし、通常のclassと同じく、```ctor```を書いて、```Props```を定義することは可能なので、```get; set;```の```Props```を定義すれば変更は許可される。
#### つまり↓も書ける(```Primary ctor```しか書けないわけではない。)
```
public record Hoge {
    public string Fuga { get; set; }

    public string Piyo { get; set; }

    public Hoge(string fuga, string piyo) {
        Fuga = fuga;
        Piyo = piyo;
    }
}
```

## 値ベースの等値性について

### classの場合

```
public class Hoge {
    public string Fuga { get; init; }

    public string Piyo { get; init; }

    public Hoge(string fuga, string piyo) {
        Fuga = fuga;
        Piyo = piyo;
    }
}
```
### Reference Equalのため、↓の場合、falseが出力される。
```
var class1 = new Hoge("fuga", "piyo");
var class2 = new Hoge("fuga", "piyo");

//Reference Equal
Console.WriteLine(class1.Equals(class2));　
```

### recordの場合

```
public record Hoge(string Fuga, string Piyo) {
}
```

### Memberwise Equalのため、↓の場合、trueが出力される。
```
var record1 = new Hoge("fuga", "piyo");
var record2 = new Hoge("fuga", "piyo");

//Memberwise Equal
Console.WriteLine(record1.Equals(record2));　
```

## Primary ctorを介さないProps
### Get Only
``` 
public record Hoge(string Fuga, string Piyo) {
    public bool IsHoge => Fuga == "Hoge";
}
```

### Get＆Init Set
``` 
public record Hoge(string Fuga, string Piyo) {
    public string? HogeFuga { get; init; }
}
```

### Get＆Set
``` 
public record Hoge(string Fuga, string Piyo) {
    public string? HogeFuga { get; set; }
}
```