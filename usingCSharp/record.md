# Using Record

## How to Record
C#9で追加された新しい参照型。
SharpLab等でC#の展開を見るとわかるが、```IEquatable<T>```を実装した```class```になる。

```IEquatable<T>```を実装していることからわかる通り、```Equals```メソッドを実装している。

値ベースの等値性が使用される仕様。
[[https://docs.microsoft.com/ja-jp/dotnet/csharp/tutorials/exploration/records#characteristics-of-records]]

## Immutable

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

```record```の定義の後ろにPropsの定義をすれば、constructorと get＆init setterのPropsとして展開される。