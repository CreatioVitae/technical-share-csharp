# using async / await Part1
## TL;DR
### 非同期メソッド、実は必ずしも非同期じゃない。
=>1個目のawaitより手前まではただの同期実行。（並列実行は苦手なので、注意が必要。）

=>完了済みタスクをawaitしてもコストは低い。（コールバック展開しないから。）

### async voidは使わない
=>無用なバグを積み込む原因になる。単体テスト実施時に検知できる可能性が１００％にならないため、そもそも書いてはいけない。

=>戻り値取れない、例外受け取れない。言わばイベントハンドラのasync化のためだけに用意されたものなので、関わらないようにすること。async voidではなく、必ずasync taskを使う。

### asyncメソッドの戻り値として受け取ったTaskはWaitしない。

=>`SynchronizationContext`が利用されている場合、デッドロックが発生する。`SynchronizationContext`を利用していない場合でもI/O完了ポートへのコールバック投函を非同期で待てず、Threadの無駄使いに繋がるため、使わないこと。

### Constructorにはasyncがつけられない（非同期コンストラクターが欲しくても言語制約上作れない）

=>CreateAsyncメソッドを作成してその中で初期化処理を行う。（めんどくさい。でも出来ないからこうするしかない。）

### async / await は I/O待ちの際に積極的に使う
async / await は I/O待ちの際に積極的に使うことでスレッドをロックされることを防ぎ、全体のスループットの向上が見込める。

今、自分自身の書いている処理自体はコールバック登録が行われることで若干のオーバーヘッドが積み込まれるが、I/O待ちの場合は、UIスレッドのアンロック＋スレッドプールに移譲する処理も外部からのコールバック待ちに出来る。

I/O待ちの処理では積極的にasync / awaitを利用すべし。

### async / await での制約事項
* out / ref 引数は利用できない
* async メソッドの呼び出しは asyncメソッドから行いawaitを行う

## 非同期の種類
* 同時実行(Concurrency)  
=> Share Cpu, Switch Thread  
* 並列実行(parallelism)  
=> Sharing processing On multiple CPUs  
* I/O待ち(I/O Completion)  
=> Communicate with the outside of the Cpu  

 **async / awaitの使いどころはI/O待ち** 

## .NETの非同期処理機能（ここから先はasync / awaitを知る上での基礎的なお話も含む）
### System.Threading.Thread;
* 生スレッドを表すクラス
* OS側の権限で切り替えを保証
* 但し、 **激重** 
#### 激重 is 何故
##### スレッドが消費するリソース（１スレッド辺り）
* スレッド自体の管理情報(1KB)
* スタックメモリ（1MB）

##### スレッド開始＆終了時のコスト
* OSのイベント発火

##### スレッド切り替えに伴うコスト
* OSの特権モードへの移行・復帰
* レジスターの保存・復元
* 次に実行するスレッドの決定

##### コード例
```
var t = new Thread(() =>{
    // 新しいスレッドで実行したい処理
});

t.start();
```

 **※ 勿論令和の時代に生スレッドをそのまま扱うことはありません。** 

### System.Threading.ThreadPool;
* スレッドプールを使うためのクラス

.NET 4以前はこちらを使う必要があった。
非同期処理の完了を待って違う非同期処理を開始することが出来ない。
例外や、処理結果の値を使うことが出来ない。

#### コード例
```
ThreadPool.QueueUserWorkItem(_ =>{
    // スレッドプール上で実行したい処理
});
//ここに何か書くと↑とは同時実行になる
```

#### スレッドプール Is 何
事前にいくつかスレッドを立てておいて、使いまわす仕組み
スレッドに関わる負担を軽減する。
ただし、優先度や実行順番等の細かい保障はできない。

##### I/O待ちとスレッドプールの関係
非同期I/O APIを利用してI/O待ちを行う。
* Windows=> I/O完了ポート
* Linux=>epoll（File I/Oはまた違うApiだった気もする。）
* BSD/Mac=>kqueue
I/Oが完了したらスレッドプールにコールバック処理を投函する仕組み。

### System.Threading.Tasks.Task;
非同期処理の続きが書けるクラス
新規に非同期処理を開始するときはRunメソッドを利用する。
非同期処理のあとに何か続けたいときはContinueWithメソッドを利用する。
尚、ContinueWithを利用する際、何も指定しない場合は、後続タスクはスレッドプール上で実行される。
UIスレッドを操作したい場合は、```TaskScheduler.FromCurrentSynchronizationContext()```をTaskSchedulerの引数に設定すること。

 [参考:ContinueWith](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.tasks.task.continuewith?view=net-5.0#System_Threading_Tasks_Task_ContinueWith_System_Action_System_Threading_Tasks_Task_System_Object__System_Object_System_Threading_CancellationToken_System_Threading_Tasks_TaskContinuationOptions_System_Threading_Tasks_TaskScheduler_) 

#### コード例
```
Task.Run(() =>{
    //非同期処理
    return 戻り値;
}).ContinueWith(t =>{
    var result = t.Result;
    //後続の処理
}
```

### async
asyncがついたメソッド=>メソッドが非同期であることのマーカー。

https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/keywords/async

#####  About ```return value```
```void```ではなく必ず、```Task```を指定する。（戻り値は返せない。）
```T```を返したい場合、 ```Task<T>```を指定する。

#### await Keyword（Case ```Task```, ```Task<T>```）
```await``` Keywordのついた処理を実行する場合、

* await Keywordが付与された処理をスレッドプールで実行する。
* メソッドをどこまで実行したかを記録する。
* gotoラベルをawait Keywordが付与された処理の後に挿入する。
* スレッドプールの処理が完了済の場合は、後続の処理を流す。
* スレッドプールの処理が未完了ならContinueWithで自分自身をコールバック登録（=スレッドプールに預けた処理が終わったら再帰呼び出し）
* 再帰呼び出しの時、貼っておいたラベル位置までgoto
といった処理を勝手に展開してくれる。（```a.k.a. AsyncStateMachine```） ※ ```IteratorBlock```でコンパイラ生成される```IteratorStateMachine```と近しい仕組み

つまり、async / awaitは、中を開けると、(Taskの場合）ContinueWithを利用して、I/O待ちの展開や、I/O待ち終了後の処理再開を制御するコードをキーワード一発で書ける仕組み。
そして、TaskはThreadPoolを、便利に使う仕組み。

#### async / await Bad Practices
##### async void is bad;
public async void GetHogeAsync()
とかやると、メソッド内でawaitが出来ない。そして、例外も受け取ることが出来ない。
async Taskだと、awaitで終了待ちが出来る。例外を受け取ることも出来る。

**async voidを書かざるを得ないのは、WinForm等でイベントハンドラを作る場合のみ。（async voidじゃないと非同期対応できないので仕方なく残っている。）** 

async voidメソッドの処理が完了するまでにスレッドプールに預けた非同期処理が完了しない場合、
```System.InvalidOperationException```で落ちます。
※ async voidメソッド本体の処理が遅くて、スレッドプールに預けた非同期処理が高速に動作している時は発覚しない負債です。

##### (async returned)Task.Wait is bad;
async メソッドの戻り値のTaskを```.Wait```してはいけない。  
=> UI/Web Context(.NET Framework版）において、SynchronizationContextを利用する制約上、デッドロックで死ぬ仕様であるため。  
`ASP.NET Core / Console App`の場合、`SynchronizationContext`は利用しないため、即座にデッドロックは発生しないものの、.Resultや.Waitを利用した場合、`UI/Web Contextのthreadは解放されない`ため`threadの無駄遣い`が発生する。  
threadpoolの枯渇もまたデッドロックを招くこととUIスレッドの解放が行われないことを併せて考えると、即時デッドロックが発生しないケースでも`原則 async / await`は`伝搬`することが`筋が良い`。

〇Wait is Bad...
```
var t = MailClient.SendAsync(new …省略);

t.Wait();
```
とした場合
※ MailClient.SendAsyncは以下の通り、asyncメソッドであり、メソッド内でawaitを行う。

https://github.com/CreatioVitae/BclExtensionPack/blob/master/src/BclExtensionPack.Mail/MailClient.cs#L25


１．t.WaitでUIスレッドをロックする。

２．MailClient.SendAsyncは、スレッドプールからUIスレッドに制御を戻そうとする

３．ロックされたUIスレッドに制御を戻せず、デッドロックで処理継続不可能

となる。

また類似の現象として`.Result`も同様の注意が必要になる。  
```
static HttpClient HttpClient { get; } = new HttpClient();

public ActionResult Index()
{
    _ = UseAwaitHttpClientAsAsync().Result;

    return View();
}

async Task<byte[]> UseAwaitHttpClientAsAsync()
{
    var response = await HttpClient.GetAsync("https://hoge.com");

    return await response.Content.ReadAsByteArrayAsync();
}
```

上記のようなコードを.NET Framework 版のASP.NET で書いた場合、  
`_ = UseAwaitHttpClientAsAsync().Result;`  
のところでデッドロックが発生する。  

<b>Taskは、await すべし。</b>

##### Can't Write async Constructor;
C#ではasync Constructorは書くことができない。（そもそも構文エラーになる。※ C#9現在）

が、Constructorの発火でもってそのObjectを利用可能とするという役割の原則に則ると、asyncメソッドを呼びたくなる時がある。

<b>(例):MailClientのWrapperを書く場合等であれば、MailServerへの接続や認証までをConstructorで行いたいけれど、これらは同期処理ではなく、I/O待ちで行いたい（ネットワーク越しの処理のため。）</b>

その場合、staticなCreateAsyncを生やしてあげる必要がある。

### async / await の制約事項
* async / await では、out / ref 引数は利用できない => 多値戻り値が必要な場合は```ValueTuple```構造体を利用すること。
* async メソッドの呼び出しは asyncメソッドから行いawaitを行う => awaitを書いて終了待ちが出来るのは```async Task or Task Like```メソッドのみのため。

## Appendix:WebFormsでのasync / await の使い方
* Pageディレクティブの Async 属性を "true" に設定する
* RegisterAsyncTaskメソッドで、実行したい非同期タスクを登録する

### ASP.NET WebFormsでの async / await 利用方法
https://docs.microsoft.com/ja-jp/aspnet/web-forms/overview/performance-and-caching/using-asynchronous-methods-in-aspnet-45

### ASP.NET WebFormsのライフサイクル
https://docs.microsoft.com/en-us/previous-versions/aspnet/ms178472(v=vs.100)?redirectedfrom=MSDN
