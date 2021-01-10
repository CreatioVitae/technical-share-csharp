# using Iterator Block;

## ���FClientEntity�̃R���N�V��������MailProp�̃R���N�V�����ɁAMailAddress1,2,3�����ꂼ��Item�Ƃ��Ēǉ�����B

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

## IteratorBlock�̓���
* �ȃ������[
* �x���]��

## C#�C���^���N�e�B�u�𗘗p���Ă̎��s����
```
> var hoges = new Hoge().GetMailProps(new List<ClientEntity>() {
.     new ("hoge", "hoge1@example.com", "hoge2@example.com", "hoge3@example.com"),
.     new ("fuga", "fuga1@example.com", "fuga2@example.com", "fuga3@example.com")
. });
> //var hoges �̎��_�ł͕]������Ȃ��B
. foreach (var item in hoges) ;
GetMailProps
> // �L���b�V���@�\�������Ă��Ȃ����߁A�����񗘗p����ƁA�y�i���e�B���󂯂����iGetMailProps�̗Ⴞ��Sleep��Console.WriteLine(nameof(GetMailProps))�����񔭉΂���B�j
> foreach (var item in hoges) ;
GetMailProps
> //������Collection�𗘗p����ꍇ��.ToList()���Ő�s�]�����Ă��܂����ALazy<T>�N���X���`���ׂ��B
. var hogeList = hoges.ToList();
GetMailProps
> foreach (var item in hogeList) ;
> 
```
