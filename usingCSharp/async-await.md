# using async / await Part1
## TL;DR
### �񓯊����\�b�h�A���͕K�������񓯊�����Ȃ��B
=>1�ڂ�await����O�܂ł͂����̓������s�B�i������s�͋��Ȃ̂ŁA���ӂ��K�v�B�j

=>�����ς݃^�X�N��await���Ă��R�X�g�͒Ⴂ�B�i�R�[���o�b�N�W�J���Ȃ�����B�j

### async void�͎g��Ȃ�
=>���p�ȃo�O��ςݍ��ތ����ɂȂ�B�P�̃e�X�g���{���Ɍ��m�ł���\�����P�O�O���ɂȂ�Ȃ����߁A�������������Ă͂����Ȃ��B

=>�߂�l���Ȃ��A��O�󂯎��Ȃ��B����΃C�x���g�n���h����async���̂��߂����ɗp�ӂ��ꂽ���̂Ȃ̂ŁA�ւ��Ȃ��悤�ɂ��邱�ƁBasync void�ł͂Ȃ��A�K��async task���g���B

### async���\�b�h�̖߂�l�Ƃ��Ď󂯎����Task��Wait���Ȃ��B

=>�f�b�h���b�N����������B�g��Ȃ����ƁB

### Constructor�ɂ�async�������Ȃ��i�񓯊��R���X�g���N�^�[���~�����Ă����ꐧ�����Ȃ��j

=>CreateAsync���\�b�h���쐬���Ă��̒��ŏ������������s���B�i�߂�ǂ������B�ł��o���Ȃ����炱�����邵���Ȃ��B�j

### async / await �� I/O�҂��̍ۂɐϋɓI�Ɏg��
async / await �� I/O�҂��̍ۂɐϋɓI�Ɏg�����ƂŃX���b�h�����b�N����邱�Ƃ�h���A�S�̂̃X���[�v�b�g�̌��オ�����߂�B

���A�������g�̏����Ă��鏈�����̂̓R�[���o�b�N�o�^���s���邱�ƂŎ኱�̃I�[�o�[�w�b�h���ςݍ��܂�邪�AI/O�҂��̏ꍇ�́AUI�X���b�h�̃A�����b�N�{�X���b�h�v�[���Ɉڏ����鏈�����O������̃R�[���o�b�N�҂��ɏo����B

I/O�҂��̏����ł͐ϋɓI��async / await�𗘗p���ׂ��B

## �񓯊��̎��
* �������s(Concurrency)
* ������s(parallelism)
* I/O�҂�(I/O Completion)

 **async / await�̎g���ǂ����I/O�҂�** 

## .NET�̔񓯊������@�\�i����������async / await��m���ł̊�b�I�Ȃ��b���܂ށj
### System.Threading.Thread;
* ���X���b�h��\���N���X
* OS���̌����Ő؂�ւ���ۏ�
* �A���A **���d** 
#### ���d is ����
##### �X���b�h������郊�\�[�X�i�P�X���b�h�ӂ�j
* �X���b�h���̂̊Ǘ����(1KB)
* �X�^�b�N�������i1MB�j

##### �X���b�h�J�n���I�����̃R�X�g
* OS�̃C�x���g����

##### �X���b�h�؂�ւ��ɔ����R�X�g
* OS�̓������[�h�ւ̈ڍs�E���A
* ���W�X�^�[�̕ۑ��E����
* ���Ɏ��s����X���b�h�̌���

##### �R�[�h��
```
var t = new Thread(() =>{
    // �V�����X���b�h�Ŏ��s����������
});

t.start();
```

 **�� �ܘ_�ߘa�̎���ɐ��X���b�h�����̂܂܈������Ƃ͂���܂���B** 

### System.Threading.ThreadPool;
* �X���b�h�v�[�����g�����߂̃N���X

.NET 4�ȑO�͂�������g���K�v���������B
�񓯊������̊�����҂��ĈႤ�񓯊��������J�n���邱�Ƃ��o���Ȃ��B
��O��A�������ʂ̒l���g�����Ƃ��o���Ȃ��B

#### �R�[�h��
```
ThreadPool.QueueUserWorkItem(_ =>{
    // �X���b�h�v�[����Ŏ��s����������
});
//�����ɉ��������Ɓ��Ƃ͓������s�ɂȂ�
```

#### �X���b�h�v�[�� Is ��
���O�ɂ������X���b�h�𗧂ĂĂ����āA�g���܂킷�d�g��
�X���b�h�Ɋւ�镉�S���y������B
�������A�D��x����s���ԓ��ׂ̍����ۏ�͂ł��Ȃ��B

##### I/O�҂��ƃX���b�h�v�[���̊֌W
�񓯊�I/O API�𗘗p����I/O�҂����s���B
* Windows=> I/O�����|�[�g
* Linux=>epoll�iFile I/O�͂܂��ႤApi�������C������B�j
* BSD/Mac=>kqueue
I/O������������X���b�h�v�[���ɃR�[���o�b�N�����𓊔�����d�g�݁B

### System.Threading.Tasks.Task;
�񓯊������̑�����������N���X
�V�K�ɔ񓯊��������J�n����Ƃ���Run���\�b�h�𗘗p����B
�񓯊������̂��Ƃɉ������������Ƃ���ContinueWith���\�b�h�𗘗p����B
���AContinueWith�𗘗p����ہA�����w�肵�Ȃ��ꍇ�́A�㑱�^�X�N�̓X���b�h�v�[����Ŏ��s�����B
UI�X���b�h�𑀍삵�����ꍇ�́A```TaskScheduler.FromCurrentSynchronizationContext()```��TaskScheduler�̈����ɐݒ肷�邱�ƁB

 [�Q�l:ContinueWith](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.tasks.task.continuewith?view=net-5.0#System_Threading_Tasks_Task_ContinueWith_System_Action_System_Threading_Tasks_Task_System_Object__System_Object_System_Threading_CancellationToken_System_Threading_Tasks_TaskContinuationOptions_System_Threading_Tasks_TaskScheduler_) 

#### �R�[�h��
```
Task.Run(() =>{
    //�񓯊�����
    return �߂�l;
}).ContinueWith(t =>{
    var result = t.Result;
    //�㑱�̏���
}
```

### async
async���������\�b�h=>���\�b�h���񓯊��ł��邱�Ƃ̃}�[�J�[�B

https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/async

#####  About ```return value```
```void```�ł͂Ȃ��K���A```Task```���w�肷��B�i�߂�l�͕Ԃ��Ȃ��B�j
```T```��Ԃ������ꍇ�A ```Task<T>```���w�肷��B

#### await Keyword�iCase ```Task```, ```Task<T>```�j
```await``` Keyword�̂������������s����ꍇ�A

* await Keyword���t�^���ꂽ�������X���b�h�v�[���Ŏ��s����B
* ���\�b�h���ǂ��܂Ŏ��s���������L�^����B
* goto���x����await Keyword���t�^���ꂽ�����̌�ɑ}������B
* �X���b�h�v�[���̏����������ς̏ꍇ�́A�㑱�̏����𗬂��B
* �X���b�h�v�[���̏������������Ȃ�ContinueWith�Ŏ������g���R�[���o�b�N�o�^�i=�X���b�h�v�[���ɗa�����������I�������ċA�Ăяo���j
* �ċA�Ăяo���̎��A�\���Ă��������x���ʒu�܂�goto
�Ƃ���������������ɓW�J���Ă����B�ia.k.a. StateMachine�j

�܂�Aasync / await�́A�����J����ƁA(Task�̏ꍇ�jContinueWith�𗘗p���āAI/O�҂��̓W�J��AI/O�҂��I����̏����ĊJ�𐧌䂷��R�[�h���L�[���[�h�ꔭ�ŏ�����d�g�݁B
�����āATask��ThreadPool���A�֗��Ɏg���d�g�݁B

#### async / await Bad Practices
##### async void is bad;
public async void GetHogeAsync()
�Ƃ����ƁA���\�b�h����await���o���Ȃ��B�����āA��O���󂯎�邱�Ƃ��o���Ȃ��B
async Task���ƁAawait�ŏI���҂����o����B��O���󂯎�邱�Ƃ��o����B

**async void����������𓾂Ȃ��̂́AWinForm���ŃC�x���g�n���h�������ꍇ�̂݁B�iasync void����Ȃ��Ɣ񓯊��Ή��ł��Ȃ��̂Ŏd���Ȃ��c���Ă���B�j** 

async void���\�b�h�̏�������������܂łɃX���b�h�v�[���ɗa�����񓯊��������������Ȃ��ꍇ�A
```System.InvalidOperationException```�ŗ����܂��B
�� async void���\�b�h�{�̂̏������x���āA�X���b�h�v�[���ɗa�����񓯊������������ɓ��삵�Ă��鎞�͔��o���Ȃ����ł��B

##### (async returned)Task.Wait is bad;
async ���\�b�h�̖߂�l��Task��```.Wait```���Ă͂����Ȃ��B
=> �f�b�h���b�N�Ŏ��ʂ��߁B
```
var t = MailClient.SendAsync(new �c�ȗ�);

t.Wait();
```
�Ƃ����ꍇ

�P�Dt.Wait��UI�X���b�h�����b�N����B

�Q�DMailClient.SendAsync�́A�X���b�h�v�[������UI�X���b�h�ɐ����߂����Ƃ���

�R�D���b�N���ꂽUI�X���b�h�ɐ����߂����A�f�b�h���b�N�ŏ����p���s�\

�ƂȂ�B

<b>Task�́Aawait ���ׂ��B</b>

##### Can't Write async Constructor;
C#�ł�async Constructor�͏������Ƃ��ł��Ȃ��B�i���������\���G���[�ɂȂ�B�� C#9���݁j

���AConstructor�̔��΂ł����Ă���Object�𗘗p�\�Ƃ���Ƃ��������̌����ɑ���ƁAasync���\�b�h���Ăт����Ȃ鎞������B

<b>(��):MailClient��Wrapper�������ꍇ���ł���΁AMailServer�ւ̐ڑ���F�؂܂ł�Constructor�ōs����������ǁA�����͓��������ł͂Ȃ��AI/O�҂��ōs�������i�l�b�g���[�N�z���̏����̂��߁B�j</b>

���̏ꍇ�Astatic��CreateAsync�𐶂₵�Ă�����K�v������B
