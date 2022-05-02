# 値渡しと参照渡し Part1
C#での値の受け渡しは、基本値渡しだが、`ref`, `out`, `in` キーワードを利用することで参照渡しにすることが可能。

## 値渡し is 何
 呼び出し先のメソッド内で `in parameter`の値を書きかえても、呼び出し元の`変数`に書き換え結果は伝搬されない

## `ref` is 何
refキーワードは、呼び出し先の`method`内で`参照可能＆書き換え可能`の制約を表す。 
呼び出し先のメソッド内で `in parameter`の値を書きかえると、呼び出し元の`変数`に書き換え結果が伝搬される。  

## `out` is 何
`Out`キーワードは、呼び出し先の`method`内で`参照不可＆書き換え可能`の制約を表す。  
戻り値以外に値を返却したいケースで利用する。  
特に多値戻り値実装以前には、`out`キーワードを利用したコードが書かれることも多かった。

## 値型＆参照型×値渡し＆参照渡しの変数書き換え可否に関する注意
値型と参照型についてのおさらいになるが、

* 変数は`スタック`領域に格納される。
* 値型の変数には`直接`値が格納される。
* 参照型の変数には`参照情報` が格納される。実体は`ヒープ`領域に確保する。

となる。

参照渡しは、`変数の書き換え結果が伝搬されるか否か` という観点であるため、

|  |  | プロパティ書き換え | インスタンス書き換え |
| --- | --- | --- | --- |
| 値型 | 値渡し | × | × |
| 値型 | 参照渡し | 〇 | 〇 |
| 参照型 | 値渡し | 〇 | × |
| 参照型 | 参照渡し | 〇 | 〇 |

となる。  
### 際の注意事項
値型の値渡しは、変数に値を直接格納するため、プロパティ書き換えも含めて書き換え結果が呼び出し元に伝搬されない。  
参照型の値渡しは、変数に参照情報のみを格納するため、インスタンスについての書き換え結果が呼び出し元に伝搬されない。  

ここで、参照型の実体は変数に格納されておらず、ヒープ領域に存在していることを思い出してほしい。  
参照型の値渡しの際、呼び出し先でプロパティ書き換えを行った場合、参照情報のみが格納された変数ではなく、ヒープに確保された実体を書き換えるため、値は書き換えられる。  

### 参照型のプロパティ書き換えの例
```
public class Person { 
    public int Id { get; set; }
}

static bool UpdatePersonId(Person person) {
    person.Id = 999;
    return true;
}

static bool DoesNotUpdatePerson(Person person) {
    person = new Person() { Id = 999 };
    return false;
}
```

上記、`DoesNotUpdatePerson`では`in parameter`のインスタンス書き換えを行うが、`ref`キーワードが付与されていないため、インスタンス書き換え結果は呼び出し元に伝搬されない。  
一方で、`UpdatePersonId`のほうは、プロパティの書き換えに当たるため、変数ではなく、実体側の書き換えにあたるため、`ref`キーワードなしで書き換えが可能となっている。