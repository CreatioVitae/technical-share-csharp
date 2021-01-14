# using Extension Method;
## �g�����\�b�h�Ƃ́H

�ÓI���\�b�h���C���X�^���X���\�b�h�Ɠ����`���ŌĂяo����悤�ɂł���d�g��

* C#2.0�܂� => �����N���X�̋@�\�g���́A�N���X�p���Ȃ̂ŐV�����N���X������Ă����B
* C#3.0�ȍ~ => �g�����\�b�h�ł̋@�\�ǉ����\�B

### �g�����\�b�h�̗p�r
* �������쐬���Ă��Ȃ��N���X�Ƀ��\�b�h�𑫂������ꍇ
* �ÓI���\�b�h��Fluent�ŋL�q�������ꍇ

### Code��
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

// �g�����\�b�h = �ÓI���\�b�h���C���X�^���X���\�b�h�Ɠ��������ŌĂяo����悤�ɂ���d�g�݁B
// �g�����\�b�h��ǉ��������^��������Ƃ��āAthis�L�[���[�h�t�Œ�`���s���B
internal static class ClientEntitiesExtensions {
    internal static IEnumerable<MailProp> CreateMailProps(this IEnumerable<ClientEntity> clients) {
        foreach (var client in clients) {
            yield return new(client.ClientName, client.MailAddress1);
            yield return new(client.ClientName, client.MailAddress2);
            yield return new(client.ClientName, client.MailAddress3);
        }
    }
}

// �C���X�^���X���\�b�h�̂悤�ɗ��p����
return clients.CreateMailProps();
```

### �g�����\�b�h�̎Q�Ƃ̎d��
* using �f�B���N�e�B�u �Ŏw�肵�����O��ԓ��ɑ��݂���g�����\�b�h���Q�Ƃ���B
* using���Ă��閼�O��Ԃ̊ԂŁA�g�����\�b�h���ŏՓ˂���ƃG���[�ɂȂ邱�Ƃɗ��ӂ��邱�ƁB

### �C���X�^���X���\�b�h�Ɠ����̊g�����\�b�h���쐬���Ȃ�����
�g�����\�b�h�Ɠ����̃C���X�^���X���\�b�h�����݂���ꍇ�A�C���X�^���X���\�b�h���D�悳���B

### �C���^�[�t�F�[�X�ɁA�g�����\�b�h��ǉ��ł���
�C���^�[�t�F�[�X�Ɋg�����\�b�h��ǉ�����g��������{�I�ɂ���ׂ��B
�t��C#�̑g�ݍ��݌^��A�����N���X�ɑ΂��āA���ʃN���X���C�u�����[�ɒǉ����铙�̍s�ׂ͂����߂��Ȃ��B�i���͕⊮����������������邽�߁B�j

�eDSL�iLibrary���j�̓��������Ń��X�N������݌v�ɂ���̂ł���΁A�����N���X�ɑ΂���g�����\�b�h�̒ǉ���OK