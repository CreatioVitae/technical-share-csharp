# using Extension Method;
## 拡張メソッドとは？

静的メソッドをインスタンスメソッドと同じ形式で呼び出せるようにできる仕組み

* C#2.0まで => 既存クラスの機能拡張は、クラス継承なので新しいクラスを作っていた。
* C#3.0以降 => 拡張メソッドでの機能追加が可能。

### 拡張メソッドの用途
* 自分が作成していないクラスにメソッドを足したい場合
* 静的メソッドをFluentで記述したい場合

### Code例
```
public class ClientEntity {
    public string ClientName { get; }

    public string MailAddress1 { get; }

    public string MailAddress2 { get; }

    public string MailAddress3 { get; }

    public ClientEntity(string clientName, string mailAddress1, string mailAddress2, string mailAddress3) =>
    (ClientName, MailAddress1, MailAddress2, MailAddress3) = (clientName, mailAddress1, mailAddress2, mailAddress3);
}

public class MailProp {
    public string ClientName { get; }

    public string MailAddress { get; }

    public MailProp(string clientName, string mailAddress) =>
        (ClientName, MailAddress) = (clientName, mailAddress);
}

// 拡張メソッド = 静的メソッドをインスタンスメソッドと同じ書式で呼び出せるようにする仕組み。
// 拡張メソッドを追加したい型を第一引数として、thisキーワード付で定義を行う。
internal static class ClientEntitiesExtensions {
    internal static IEnumerable<MailProp> CreateMailProps(this IEnumerable<ClientEntity> clients) {
        foreach (var client in clients) {
            yield return new(client.ClientName, client.MailAddress1);
            yield return new(client.ClientName, client.MailAddress2);
            yield return new(client.ClientName, client.MailAddress3);
        }
    }
}

// インスタンスメソッドのように利用する
return clients.CreateMailProps();
```

### 拡張メソッドの参照の仕方
* using ディレクティブ で指定した名前空間内に存在する拡張メソッドを参照する。
* usingしている名前空間の間で、拡張メソッド名で衝突するとエラーになることに留意すること。

### インスタンスメソッドと同名の拡張メソッドを作成しないこと
拡張メソッドと同名のインスタンスメソッドが存在する場合、インスタンスメソッドが優先される。

### インターフェースに、拡張メソッドを追加できる
インターフェースに拡張メソッドを追加する使い方を基本的にするべき。
逆にC#の組み込み型や、実装クラスに対して、共通クラスライブラリーに追加する等の行為はお勧めしない。（入力補完が著しく汚染されるため。）

各DSL（Library等）の内部だけでリスクが閉じる設計にするのであれば、実装クラスに対する拡張メソッドの追加もOK