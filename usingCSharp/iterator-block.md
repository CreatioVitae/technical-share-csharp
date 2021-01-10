# using Iterator Block;

## 例題：ClientEntityのコレクションからMailPropのコレクションに、MailAddress1,2,3をそれぞれItemとして追加する。

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

public class Hoge {
    public IEnumerable<MailProp> GetMailProps(IEnumerable<ClientEntity> clients) {
        System.Threading.Thread.Sleep(1000);
        Console.WriteLine(nameof(GetMailProps));

        foreach (var client in clients) {
            yield return new(client.ClientName, client.MailAddress1);
            yield return new(client.ClientName, client.MailAddress2);
            yield return new(client.ClientName, client.MailAddress3);
        }
    }
}
```

## IteratorBlockの特徴
* 省メモリー
* 遅延評価

## C#インタラクティブを利用しての試行結果
```
> var hoges = new Hoge().GetMailProps(new List<ClientEntity>() {
.     new ("hoge", "hoge1@example.com", "hoge2@example.com", "hoge3@example.com"),
.     new ("fuga", "fuga1@example.com", "fuga2@example.com", "fuga3@example.com")
. });
> //var hoges の時点では評価されない。
. foreach (var item in hoges) ;
GetMailProps
> // キャッシュ機構を持っていないため、複数回利用すると、ペナルティを受けがち（GetMailPropsの例だとSleep＆Console.WriteLine(nameof(GetMailProps))が毎回発火する。）
> foreach (var item in hoges) ;
GetMailProps
> //複数回Collectionを利用する場合は.ToList()等で先行評価してしまうか、Lazy<T>クラスを定義すべき。
. var hogeList = hoges.ToList();
GetMailProps
> foreach (var item in hogeList) ;
> 
```
