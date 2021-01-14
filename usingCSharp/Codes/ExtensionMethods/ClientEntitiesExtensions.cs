
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

// 静的メソッドをインスタンスメソッドと同じ書式で呼び出せるようにする仕組み。
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

//Like Instance Method...
return clients.CreateMailProps();
