# using init Setter;

## init setterの効能
### ```ASP.NET Core``` のオプション パターンにて効能がある
https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/configuration/options?view=aspnetcore-5.0

```ASP.NET Core``` にて、構成設定を Option Classにバインドさせる際、Prop の ```set``` アクセサーは ```init```で駆動可能。

上記URLにて、Default Constructorが必要になるという記述があるので、```record```の```Primary ctor```は利用できないが、Object初期化子を利用して値を設定するらしく、```init```での対応は可能だった。