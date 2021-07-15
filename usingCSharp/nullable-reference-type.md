# Introduction:Null安全
## Null Is 何
null = Invalidなことを保証できるポインター

=> 不定動作と比べれば即死の方がマシ（無効な参照であることが自明、且つ null参照例外を起こせる）

よくわからない値よりは、良く知ったInvalidな値が好ましい。

### 不定動作 Is 何故
メモリーを確保した時点ではどういう状態になっているか分からないので、適切に初期化しなければ不定動作を招く。

尚、C#では、.NETランタイムやC#コンパイラーが初期化作業を適切にしてくれているため、気にする場面は少ない。

## Null Is billion dollar mistake
https://ja.wikipedia.org/wiki/%E3%82%A2%E3%83%B3%E3%83%88%E3%83%8B%E3%83%BC%E3%83%BB%E3%83%9B%E3%83%BC%E3%82%A2

## Null安全 Is 何
Null検査をコンパイラが強制する仕組み。
最近の言語には大体入っている（TypeScriptとか、Kotlinとか。）

C#でのNull許容参照型＆Nullability解析はNull安全に相当する機能。

但し、Null許容参照型については、Null安全に関する解析結果は既定ではErrorではなくWarning扱いとなっている。（2021年7月現在）

### Null安全のための.editorconfig
Null安全に関連する解析をerrorとして扱うように.editorconfigを設定することで、VisualStudio 2019上はビルドエラーとして検知可能。

あくまでEditorのconfigのため、Compilerでの直接buildでは警告出力の上、buildが通ってしまう（はず。）
```
# CS8600: Null リテラルまたは Null の可能性がある値を Null 非許容型に変換しています。
dotnet_diagnostic.CS8600.severity = error

# CS8602: null 参照の可能性があるものの逆参照です。
dotnet_diagnostic.CS8602.severity = error

# CS8604: Null 参照引数の可能性があります。
dotnet_diagnostic.CS8604.severity = error
```

# using Nullable Reference Type

https://sharplab.io/#v2:CYLg1APgxAdgrgGwQQwEYIKYAIMzZgWAChiABAJgEYyAGLUygFgG5b7KA6AEQEtkBzGAHsAzgBceAYxEcAwkOAYAgngQBPETxGsSRUgGZ65LLKwBvYliv1DPGGKwBJYOaz8MY5lgAOAJx4Abshi2CIeXgC+ltbRVgZYdmIA/FgAEkLuzq7unj7+QSFYYblRutY27HQAcsgAtthmbuF5gcGhzaXlsRUMdABicPzI2c3FzBFYALxYAEQAZoPIMzpdROUA2kpIQgDuVYgIALrd8b1pGRg19ebd5TlTAHxYAPoAFhdXGCvl1sWPL+93J8plgggg4NgkikZnghABrHjLbqdGJraxnN4fOrYaazWEIpFoqzdda8ETIbZ7A7HIk9Sg0FIDIbAiy0u4ef7PBbM7HfH5FDmTJ5cxbA6ZgiFYKFYMSvXy7LAwDA7LBKXz8OD1ez7JAAUQAHpIMN4JEIYAAKACUfKwKOJtN6KRFPPqNpJAFkMLVUBhfFUhGIdQgAOqvXDm0gAdgAahSISAZb4IQAaLD1b2+hN4epCObm3UARzgPG8WrElstNPK8VQQiECDSyBEheLpdwDkaOUixBOhgY+gAPL0HikWyWyyMSt1eyYrf9btZHDAeGIAOK4X0UgAK8u8IitbodhlkEcog+HOCL4/bltZ/KsS5X66Vvm3u/31oXVjHbfsIIwV6/p4X6Ns2gETtMAGthOWiKgGioHDadpYDOpCMFg7qngyWCAhgqZnNyyD4fSKTeDwahCJaNy0shHpej6foBkG5rZhgubmp8FZVui6GPmuG6vggO5CHuc5CiB5rOKmnypukQLYlR0zmpQqYzGIyBwk2ryIqm8BIJ+RClGQfbGFw1HVn2lAAGxYLW9ZYAAKr4ahbsgvhhOapiSKm6z+oGByhuGYhJhglZYEIcAOFwKTAFRd73rK8oqkqKp+Y4tTeJgZYYMABpGiaPBmgeyLEBEQA

## Microsoft Docs
https://docs.microsoft.com/ja-jp/dotnet/csharp/nullable-references

## Null許容参照型の効能
```nullable / non null``` な参照型を意図して使うことができるようになる。（C#8以前は、参照型 = nullable だった。）

参照型が ```nullable / non null``` であることをコントロールできるようになったので、```Object / Member```　に制約を付けることが可能になった。

今までは参照型は必ず ```nullable``` だったため、 ```method``` の``` In Parameter``` に参照型がある場合、```null``` の検査が必要だった。

これによって、防御的プログラミングにありがちな過剰なNullチェックから解放され、Nullabilityに関して、契約プログラミングが可能になった。

```
public bool Vaidate(Hoge hoge) {
    // hoge はnullable
    if(hoge is null){
        return false;
    }

    /// ...
}
```

C#8以降は ```non null``` な型制約で ```In Parameter``` を定義することにより ```is null``` の検査が不要になった。
```
public bool Vaidate(Hoge hoge){
    // hoge はnon null
    /// ...
}
```

## Null許容参照型の有効化
### Project単位での有効化
#### 前提条件
**Project単位で有効化したい場合、SDKスタイルのcsprojに書き直す必要がある。**

```*.csproj``` の```<PropertyGroup>```内に、以下のsettingを差すことで有効にできる
```
<Nullable>enable</Nullable>
```

### #nullable での有効化
ファイル単位で、Null許容参照型の有効 / 無効を設定する。（SDKスタイルではなくともC#8↑が利用可能であれば使える。）

指定方法：

```#nullable enable|disable|restore [warnings|annotations]```


* ```#nullable enable``` でNull許容参照型を有効にする。
* ```#nullable disable``` でNull許容参照型を無効にする。
* ```#nullable restore``` でNull許容参照型をプロジェクトの設定に戻す。
* ```[warnings|annotations]``` は指定しない場合、両方指定される。

## null許容参照型の?と、null許容値型の?の違いについて
null許容値型と違い、コンパイラ生成によって属性で表現(後述)された型になる。

そのため、例えば```string```と、```string?```は本質的には同じ型になる。

逆にnull許容値型の例えば```int?```は、```nullable<int>```なので```int```とは別の型になる。

### 属性での表現 について

* ```NullableContext```
* ```Nullable```

でそれぞれ表現される。

```NullableContext``` で ```class / method``` に全体適用しつつ、```Nullable``` でオーバーライドする仕組み。

属性の引数は右記の通り。 ```[ 0:oblivious, 1:non null, 2:nullable ]``` 

#### ```NullableContext``` 及び、```Nullable```にコンパイラ生成の例
```
public void M(string? hoge, string fuga, string? piyo) {
}
```
と書くと、
```
[System.Runtime.CompilerServices.NullableContext(2)]
public void M(string hoge, [System.Runtime.CompilerServices.Nullable(1)] string fuga, string piyo)
{
}
```
にコンパイラ生成される。

* メソッド```M```の引数は、```T?```が多いため、メソッドには```NullableContext(2)```が付与される。 ※ 2 は ```nullable```
* 引数 ```fuga``` は ```T```なので、```Nullable(1)```が付与される。 ※ 1 は ```non null```

実際にはbuild時に展開される

## フロー解析(flow analysis)
コードのフローを追跡し、利用箇所より前の時点で代入や検査が行われるかをコンパイラーが調べる仕組み。

null検査の実施以降は、non nullとして扱われる。

method等で検査を行う場合、フロー解析へのヒントを属性付与という形で行う必要がある。

## Objectの初期化
non NullなPropを定義した場合、確実に```ctor```もしくは初期化子で初期化を行う必要がある。

```ctor```での初期化
```
    public C(IEnumerable<string> equipment){
        InitGeneralProps();
        Equipment = equipment;
        HasEquipment = equipment is not null;
    }
```

初期化子での初期化
```
public string Fuga { get; set;} = "fuga";
```

## Nullability解析 : MemberNotNull(C#9～)
```
public C() => 
    InitGeneralProps();

public C(IEnumerable<string> equipment){
    InitGeneralProps();
    Equipment = equipment;
    HasEquipment = equipment is not null;
}

public void M(string? hoge, string fuga, string? piyo) {
}

[MemberNotNull(nameof(Name))]
void InitGeneralProps() =>
    (Id, Name) = (1, "takashi");
```
```ctor```でPropの初期化を行わない場合、警告が発生する。

一方で、任意の単位で初期化処理を定義し、```ctor```を複数定義して呼び分けたいケースもある。

```MemberNotNull```を付与することで、指定した、Propがnon Nullでの初期化を保証する。


##  Nullability解析 : MemberNotNullWhen
```returnValue``` が 所定の値の場合（When）、指定した```member```がnon Nullであることを保証する。
```
[MemberNotNullWhen(returnValue: true, member: nameof(Equipment))]
public bool HasEquipment { get; }
```

## AllowNull
```?```が付いていなくても、```Null```を受け入れることを保証する。```Null```を受け入れ、```non Null```を返すケースで利用する。
```
[AllowNull]
public string HogeName {
    get => _hogeName;
    set => _hogeName = value ?? "naoki";
}

string _hogeName =  "naoki";
```

## DisallowNull
```?```が付いていても```Null```を受け入れないことを保証する。
```
[DisallowNull]
public string? FugaName {
    get => _fugaName;
    set => _fugaName = value ?? throw new ArgumentNullException();
}

string? _fugaName;
```
### その他利用ケース
```DisallowNull```は、エンドポイントへのモデルバインドを行う際に設定することがある。

JsonをPostするケースで、non Nullな参照型だと、Jsonのパースエラー（500エラー）が発生してしまう。

その場合は、```[Required, DisallowNull]```のような属性付与で400エラーになるよう処置が必要。

尚、構造体は、そもそも型が違うためか、パースエラーは起こらない。

## NotNullWhen
戻り値に応じて ```non Null``` であることを保証する。
```
public class D {
    public static bool TryParse(C c, [NotNullWhen(true)] out D? d) {
        ///
    }
}
```
上記メソッドの場合、```TryParse```の結果がtrueの場合、```d```は```non Null```であることが保証される。

## Todo : 型引数に関する説明。MaybeNull, NotNull,　MaybeNullWhen