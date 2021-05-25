# WIP : using Nullable Reference Type

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
前提条件として、SDKスタイルのcsprojでしか、Project単位でのNull許容参照型の有効化は出来ない。

Project単位で有効化したい場合、SDKスタイルのcsprojに書き直す必要がある。

### #nullable での有効化
ファイル単位で、Null許容参照型の有効 / 無効を設定する。（SDKスタイルではなくともC#8↑が利用可能であれば使える。）

指定方法：

```#nullable enable|disable|restore [warnings|annotations]```


* ```#nullable enable``` でNull許容参照型を有効にできる。
* ```#nullable disable``` でNull許容参照型を無効にできる。
* ```#nullable restore``` でNull許容参照型を1つ前の設定に戻せる。
* ```[warnings|annotations]``` は指定しない場合、両方指定される。



## null許容参照型の?と、null許容値型の?の違いについて
null許容値型と違い、属性で表現(後述)された型になる。ため、例えば```string```と、```string?```は本質的には同じ型になる。

逆にnull許容値型の例えば```int?```は、```nullable<int>```なので```int```とは別の型になる。

### 属性での表現 について
https://sharplab.io/#v2:CYLg1APgxAdgrgGwQQwEYIKYAIMzZgWAChiABAZi1ICYsBhLAb2K1astIBYsBZAClIBGAAwB+LAAsA9gHMMAGioisAMzgzkioWKwAHAJYBPKQEomLNgF9iloA===

* ```NullableContext```
* ```Nullable```

でそれぞれ表現される。

```NullableContext``` で ```class / method``` に全体適用しつつ、```Nullable``` でオーバーライドする仕組み。

属性の引数は右記の通り。 ```[ 0:oblivious, 1:non null, 2:nullable ]``` 

## フロー解析(flow analysis)
コードのフローを追跡し、利用箇所より前の時点で代入や検査が行われるかをコンパイラーが調べる仕組み。

null検査の実施以降は、non nullとして扱われる。

method等で検査を行う場合、フロー解析へのヒントを属性付与という形で行う必要がある。