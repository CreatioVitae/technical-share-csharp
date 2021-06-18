# using Pattern Matching

## 今回のコードサンプル On SharpLab
https://sharplab.io/#v2:CYLg1APgAgTAjAWAFBQAwAIpwCwG5lqZwB0AMgJYB2AjvkgMSUCuANiwIYBGLApuj5S68CAZiIA2TDHQBhdAG9k6ZZjFZJFAM4AXADwAVAHzoAgpq16jACm0ALcpqIiDxzQHsmAJwDGPAJToALyGSiph7l6+6A7oFi7oLA7a6AD8CUnoIOgRPjzE+m4WVn50ocpQanCSVMlmAJKU2lZunABWPN7aaTm+AcFlYehWPXwxNdEBaeSZ6AC0cHSDKgMV/MwAtugA4p7swHyKSEvKAMoANANhJhdHxwBCN8eyj8cAIi9LAFoDAL4DK5VJDs9nwtjxtMD9ncAJ4ARSY7ES2mhVnG1ARSOhAUOT3IADMhujEeRkehdOg4KhsZdjlAAOzbXb7YivRbHP63JY8FiaUYEqxEzHRRyGQIUjDsSjAMniqkKGlLemMkHEGRspYcp7c3nRfmCknQ4XoUWy9CS6XkgCsqDlOKeKiVkLyd3Vg01x21fMJGINRpN1olUplMBttoVg0dTLyJldYXdXJ5XoFPtJMRNIZt1M59sjKpOsZU8cGnvl2aeueZnwLyiLReQAHp62FVuplfttuCnTD4cTkXBUY10PrkVnG+WGcPDZoAO4k7y2Utj+3KcmUoLGJ0sm5L5cmtfmmWUjDBNt5GTbpvL41io9moNWm3r0/EB4Ny+7sUBu8W9AZ48bqNiGuN8r2vX9QyfTdzhAq8AH1IMA74kB3N06BQn50GQAEJEwbB0AAOTcbQAAV2G0bQeE8ShmjaDounQFpWj8O0lnxGjWiNSgiKIDAuMofDWBYE5tE8KgAHNmPDZs6WrdAizCGDWLxKxbDcMTRkceQML4gS2Ek5D30GRtFLdcNwywABOKwdME4TRMoMSyAEMS7BKX5/mzFsqlPDttDudheSdKwB3oyczmiRo0l2USR3QbxdjxEj2F2dYsyeGc5wXKx4vYRLSJStKr28AK+GswTwuYPSsjsTw3GndBKB4eqTE8MSmHWARtAAeSYbq8QAJUldSAFEAA9fAAB20cg3Gotyy2OYqdTKthwtgvwsiklQltK2CKsEjbMAZTcqy25QdqGcYbLYWFwqu2bdJYfrDsdTso27FMUWulhYXQMAGoewTntk2tkA5UR0E8Do3E8aUAAk1J4KwsFQNIADEmDE9hwpRtJiPIaE3EK5Qob2WaWENFH0GG9YpuhAA1REmD4MUUeIGm6dKTzKgwEwADd2HIDhuB4BH1PXcM7BiDLtHnUtQORyk1oCadbAEdAMaxzjuMqlgn017GzqGPaeJVtXKHQfHCe15JdafK23A+e14JPDnkUZlhmfDH5ZOwqn+cF4XeDFnhiNqibNFI8jKIt/oFuUKXHBluWWPteQNcx9gsi423BLk/XM6dp504d7OdbzjCTwdovjhd4w3YZpmeG9uhwZQNRpBDk5KL58hfAAWTcfY9cONuW2kA35YdMQrE4Nw3D1qHNFYbRwq7nu+54Qfh7SdgBaFoQt6H7kAg98hgDIngTClMFtADg+Re37liinxUGSsJeV5APFEV5cK98DofJ+LAQC63mnGDyzY1B4RDi/VOYQxyvGhpQHQngmCdBmhbJB3gOC7GmrNcMAtPBDE/iwVeZp95ByPsPPo6Az4XwotfYAt975UOAcUX28d0CIOQag9B+CLZmE0OQMSlAOqNHDB/Hgy8yH/0oUA4+LBaH0Mvkwlh8jH6KI4WZLhVgTYAIfrwYByjiQMKvjfcErCFHD20dmOsSAfhAA==

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
コンパイラ生成を見てもらえばわかるが、明示的な型宣言パターンで生成された場合、

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

その一方で、大小比較パターンにコンバートした場合、上記コードはBuild Errorになる。

上記の式だと、５行までの条件で、intの範囲を網羅していることをRoslynが検査してくれる。

そのため、上記式だと破棄パターンの行が網羅性検査がNGのため、Build Errorになる。

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
尚、```ValueTuple```は```Deconstruct```を実装していないが、特別扱いで```Deconstruction```が可能となっている。

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

### Deconstructions For System.Tuple
```System.Tuple```に対しては```Deconstruct```拡張メソッドが用意されている。[ https://docs.microsoft.com/ja-jp/dotnet/api/system.tupleextensions.deconstruct?view=net-5.0 ]

### Deconstructions For class
自分で```Deconstruct```methodを書くか、拡張メソッドをはやす必要がある。

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
switch ステートメントにてTupleでの分岐が可能になっている。

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
if(hoge is {} nonNullValue){
    //
}
```
上記例は、
```hoge```がNull許容値型の場合、nonNullな値型へのキャストが行われ、```nonNullValue```にはnonNullな値型が入る。

```hoge```がNull許容参照型の場合、フロー解析上```nonNullValue```は、```nonNullな参照型```であることを知らせることができるが、型定義としては```T```ではなく```T?```が入る。
