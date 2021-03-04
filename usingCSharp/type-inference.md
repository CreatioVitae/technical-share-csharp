# Using Type Inference

## 型推論を積極的に使おう。
## 利用意図
 => コードの冗長性の排除

## var 編
### var のアレコレ
* C#のvarは型推論。バリアント型じゃない。
* 型が合ってなければビルド時やコード解析時にエラーが出る。
* 変数にマウスカーソルを合わせれば型名が表示される。
* 型セーフ
* varは右辺を見て型推論を行う。
* Varはローカル変数にのみ利用できる。

### varだとインターフェースの変数で受けられない。
そもそもvarはローカル変数でしか使えないので、メソッド内でリスクが閉じられている。

本当に抽象型で扱いたい欲があるのかはよく考える必要がある。

## ラムダ式編
ラムダ式は左辺を見て型推論を行う。

## switch式編
switch式は左辺を見て型推論を行う。

## Target Typed New
Target Typed Newは左辺を見て型推論を行う。

### Hoge recordを定義した場合
```
public record Hoge(string Fuga, string Piyo) {
}
```

### ```new ()``` でインスタンス生成が可能（```Hoge```を返却することが自明であるため左辺を見ての推論が可能。）
```
public static Hoge GetHoge() =>
    new ("fuga", "piyo");
```

### メソッド呼び出しの際にも引き渡す型が自明な場合、推論が可能。
```　
public static bool InspectAboutHoge(Hoge hoge) =>
    true;

public static bool InspectAboutHogeTest() =>
    InspectAboutHoge(new ("fuga", "piyo"));
```