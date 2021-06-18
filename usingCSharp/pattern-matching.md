# using Pattern Matchng
https://sharplab.io/#v2:CYLg1APgAgTAjAWAFBQAwAIpwCwG5lqZwB0AMgJYB2AjvkgMSUCuANiwIYBGLApuj5S68CAZiIA2TDHQBhdAG9k6ZZjFZJFAM4AXADwAVAHzoAgpq16jACm0ALcpqIiDxzQHsmAJwDGPAJToALyGSiph7l6+6A7oFi7oLA7a6AD8CUnoIOgRPjzE+m4WVn50ocpQanCSVMlmAJKU2lZunABWPN7JOb4BwWVh6FbdfDE10QFp5JnoALRwdAMq/RX8zAC26ADinuzAfIpIi8oAygA0/WEm54dHAELXR7IPRwAiz4sAWv0Avv3LlZJtrs+JseNogXtbgBPACKTHYiW0UKsY2o8MRUICB0e5AAZoM0QjyEj0Lp0HBUFiLkcoAB2LY7PbEF4LI6/G6LHgsTQjfFWQkY6KOQyBckYdiUYCksWUhTUxZ0hnA4gyVmLdmPLk86J8gXEqFC9AimXoCVSskAVlQsuxjxUioheVuaoGGqOWt5BPR+sNxqt4sl0pg1pt8oGDsZeRMLrCbs53M9/O9JJixuD1qpHLtEeVxxjKjjAw9cqzjxzTI++eUhcLyAA9HWwit1Eq9lswY7oXCiUi4CjGug9UjMw2y/ShwbNAB3YneWwl0d25RkilBYyO5nXRdL42rs3SikYYKtvIyLeNpdG0WH02By3Wtcn4j3esXnei/2383odNH9eR4grlfS8rx/ENHw3M5gMvAB9CCAK+JBt1dOhkO+dBkH+CRMGwdAADk3G0AAFdhtG0HhPEoZo2g6ZIWlaPxbUWPFqNaQ1KEIogMA4yg8NYFhjm0TwqAAc0YsMm1pKt0ELMJoOY3ErFsNwRJGRx5HQni+LYcSkLfAYG3k10wzDLAAE4rC0/jBOEygRLIAQRLsEofj+LNmyqE9220W52B5R0rH7bQ0gnU5okaNIdmE4d0G8HZcWI9gdjWTNHmnWd5ysOL2ASkjktSy9vD8vhLP4sLmB0rI7E8Nwp3QSgeDqkxPBEpg1gEbQAHkmC63EACUJVUgBRAAPXwAAdtHINwqJc0sjiK7VSrYMKYL8LIJJURaSpg8r+PWzB6Q3StNuUbbBjGKy2BhMLLpm7SWD6g6HQ7SMu2TZErpYGF0DAer7v4p7pJrZB2VEdBPA6NxPClAAJFSeCsLBUDSAAxJgRPYMLkbSIjyChNwCuUSHdhmlgDWR9AhrWSaoQANQRJg+FFZHiGp2nSncyoMBMAA3dhyA4bgeHh1S1zDOwYnS7Q5xLECkYpVaAinWwBHQdHMfYziKpYR8Nax07Bl2rjldVyh0DxgmteSHXH0ttx3jtODj3ZpEGZYJmw2+aSsMpvmBaF3hRZ4IiavGzQSLIijzb6eblElxxpdlpi7XkdWMfYLIOJt/iZL1jPHceNP7az7Xc/Q497cLo5neMV36cZngvbob4gA==
## 前提条件
is 演算子とswitchの基礎を理解していること

## is 演算子での宣言パターン
```hoge is T fuga``` と書くことで型の判定＆キャスト＆宣言が可能。


```cs
#nullable enable
public static List<T> AsList<T>(this IEnumerable<T> source) =>
    source is List<T> list ? list : source.ToList();
```

と書くと、
```cs
public static List<T> AsList<[System.Runtime.CompilerServices.Nullable(2)] T>(IEnumerable<T> source) {
    List<T> list = source as List<T>;
    return (list != null) ? list : Enumerable.ToList(source);
}
```

にコンパイラ生成される。

尚、

```cs
#nullable enable
public static int AsInt(object? source) =>
    (source is int i) ? i : -1;
```

は

```cs
public static int AsInt(object source)
{
    int result;
    if (source is int)
    {
        int num = (int)source;
        result = num;
    }
    else
    {
        result = -1;
    }
    return result;
}
```

にコンパイラ生成される。

## switchでの比較パターン With 破棄パターンと網羅性検査
```cs
public static Grade GetGradeByQuality(int quality) {
    if (quality < 10) {
        return Grade.D;
    }
    else if (quality is >= 10 and < 100) {
        return Grade.C;
    }
    else if (quality is >= 100 and < 500) {
        return Grade.B;
    }
    else if (quality is >= 500 and < 2000) {
        return Grade.A;
    }
    else if (quality is >= 2000) {
        return Grade.S;
    }
    else {
        return Grade.Z;
    }
}
```
のように```quality```に対してif文を書いていく。

これをVisualStudioのクイックアクションでSwitch式にリファクタリングを行うと、比較パターンに書き換えることができる。

```cs
public static Grade GetGradeByQuality(int quality) {
    return quality switch {
        < 10 => Grade.D,
        >= 10 and < 100 => Grade.C,
        >= 100 and < 500 => Grade.B,
        >= 500 and < 2000 => Grade.A,
        >= 2000 => Grade.S,
        _ => Grade.Z
    };
}
```
大小比較パターンと呼ばれるパターンマッチング。

このパターンマッチングのメリットは、readable / writable ではなく、網羅性検査が入ることが挙げられる。

if文での記述の場合、Build Errorにはならない。

その一方で、大小比較パターンにコンバートした場合、上記コードはbuild Errorになる。

上記の式だと、５行までの条件で、intの範囲を網羅していることをRoslynが検査してくれる。

そのため、上記式だとbuild Errorになる。

逆に、範囲が網羅されていない場合にも警告が出る。

## Appendix:using ValueTuple
オブジェクトを定義したくないけれど、メンバーは定義したい場合に利用する。

```cs
public class HogeServiceModel {
}

public class Fuga {
    public (bool result, HogeServiceModel? availableModel) ValidateAndGetAvailableModel() {
        return (result:false, availableModel:null);
    }
}
```

のように記述が可能。

```ValidateResult``` のようなオブジェクトを定義する必要がなくなる。

## Appendix:using Deconstructions
```Deconstructions```は、パターンベースでの実装が基本になっているため、```Deconstruct```が所定のルールで実装されていれば利用可能。
尚、```ValueTuple```は```Deconstruct```を実装していないが、特別扱いで```Deconstructions```が可能となっている。

### ```Deconstructions```が```ValueTuple```でサポートされている経緯と使いどころ
ValueTupleはオブジェクトを定義したくないけどメンバーは定義したい場合に利用する。
この根本は、「束ねる役割を持つオブジェクトの命名を省きたい。」という欲求がある。
そのため、返却されたValueTupleをオブジェクトで受けるのではなく、各値に分解して、宣言、及び代入を行うことができる。
また、分解不要な値については、破棄パターンで破棄可能。

```cs
public class HogeServiceModel {
}

public class Fuga {
    public (bool result, HogeServiceModel? availableModel) ValidateAndGetAvailableModel() {
        return (result:false, availableModel:null);
    }
    
    public void Hoge() {
        // Deconstruction Declaration
        var (result, availableModel) = ValidateAndGetAvailableModel();

        // Deconstruction Assignment
        (result, availableModel) = ValidateAndGetAvailableModel();

        (_, availableModel) = ValidateAndGetAvailableModel();
    }
    
}
```

### System.Tupleに対するDeconstruction
```System.Tuple```に対しては```Deconstruct```拡張メソッドが用意されている。[ https://docs.microsoft.com/ja-jp/dotnet/api/system.tupleextensions.deconstruct?view=net-5.0 ]
が、```System.Tuple```は基本非推奨なので、

## using not With 宣言パターン
```cs
public static void NotPattern(object? obj) {
    if(obj is not string nonNullString){
        return;
    }
    
    Console.WriteLine(nonNullString.Length);
}
```

```hoge is not string nonNull``` で宣言パターンが可能。

else 以降で```assigned```となる。

ただし、```switch```では利用できないため注意すること。

## Switch式での位置パターン With 破棄パターン
```cs
#nullable enable
public record Hoge(string? Fuga, string? Piyo) {
    readonly string EmptyValue = string.Empty;

    public string AvailableHoge =>
        this switch {
            (string, _) when Fuga is not null => Fuga,
            (_, string) when Piyo is not null => Piyo,
            _ => EmptyValue
        };
}
```

## タプルswitch
```cs
public static Grade GetBaseGrade((int? quality, int? rarity) craftParam) {
    switch (craftParam) {
        case (null, null): throw new ArgumentOutOfRangeException();
        case (null, _): 
        case (_, null): return Grade.Z;
        case (int nonNullQ, int nonNullR): return GetGradeByQuality(nonNullQ + nonNullR);
    }
}
```
## プロパティパターン
```cs
#nullable enable
public record Hoge(string? Fuga, string? Piyo) {
    readonly string EmptyValue = string.Empty;

public string AvailableHogePropsPattern =>
    this switch {
        { Fuga: not null } => Fuga,
        { Piyo: not null } => Piyo,
        _ => EmptyValue
    };
```
分解が出来ない場合はプロパティパターンを利用する。

## プロパティパターンを使っての型switch
```cs
if(hoge is {} nonNull){
    //
}
```
