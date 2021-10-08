# using async / await Part2
## async Task Main
### CSharp 7.1 New Feature
#### 非同期Main[ async Task Main ]が追加
* static async Task<int> Main()
* static async Task<int> Main(string[] args)
* static async Task Main()
* static async Task Main(string[] args)

### 非同期Mainの注意事項
void Main / int Main が既にいる場合、async Task Mainはエントリーポイントとして扱われない。

## async / awaitを利用してのI/O待ちの並列処理
```async / await``` は I/O待ちの並列実行は得意領域。

### ```foreach + await;```

<b>I/O待ちを都度行うコード例</b>
```cs
// Use ValueTuple(C# 7.0)
static async Task CopyAsync((string originDirectory, string destinationDirectory) copySettings){
    // Deconstruction(C# 7.0)
    (var originDirectory, var destinationDirectory) = copySettings;

    foreach (var fileName in Directory.EnumerateFiles(originDirectory)) {
        // using 変数宣言(C# 8.0)
        using var originStream = File.Open(fileName, FileMode.Open);
        using var destinationStream = File.Create($"{destinationDirectory}{fileName.LastIndexOf('\\'))}");
        // 都度awaitなのでイマイチ
        await originStream.CopyToAsync(destinationStream);
    }
}

var appDirectory = new FileInfo(typeof(EnumerableExtensionLibrary).Assembly.Location).Directory.Parent.Parent.Parent.FullName;

await CopyAsync((originDirectory: $"{appDirectory}/Origin",destinationDirectory: $"{appDirectory}/Destination"));
```

マシン全体にとっては無意味ではないものの、折角のI/O待ちなのだから、都度待機はしたくない。

<b>Linq + await;</b>を利用する
```cs
// Use ValueTuple(C# 7.0)
static async Task CopyAsync((string originDirectory, string destinationDirectory) copySettings) {
    (var originDirectory, var destinationDirectory) = copySettings;
    // Lamdaの中でasyncを書く =>　IEnumerable<Task> が返る
    // Task.WhenAllで I/O待ち
    await Task.WhenAll(Directory.EnumerateFiles(originDirectory).Select(async filename => {
        using var originStream = File.Open(fileName, FileMode.Open);
        using var destinationStream = File.Create($"{destinationDirectory}{fileName.LastIndexOf('\\'))}");
        await originStream.CopyToAsync(destinationStream);
    }));
}

var appDirectory = new FileInfo(typeof(EnumerableExtensionLibrary).Assembly.Location).Directory.Parent.Parent.Parent.FullName;

await CopyAsync((originDirectory: $"{appDirectory}/Origin",destinationDirectory: $"{appDirectory}/Destination"));
```

```Enumerable.Select```

https://docs.microsoft.com/ja-jp/dotnet/api/system.linq.enumerable.select?view=net-5.0

```Linq``` の　```Select``` メソッドは見ての通り、戻り値が必要になるので、```void```のLamdaは通らない。

一方で、```async``` Lamdaは```Task```が返るため、```Select```メソッドを通る。

#### 拡張メソッドを作って後置き```WhenAll```を書く

https://github.com/CreatioVitae/BclExtensionPack/blob/master/src/BclExtensionPack.CoreLib/TaskEnumerableExtensions.cs

```cs
static async Task CopyAsync((string originDirectory, string destinationDirectory) copySettings) {
    (var originDirectory, var destinationDirectory) = copySettings;
    // Lamdaの中でasyncを書く =>　IEnumerable<Task> が返る
    // Task.WhenAllで I/O待ち
    await Directory.EnumerateFiles(originDirectory).Select(async filename => {
        using var originStream = File.Open(fileName, FileMode.Open);
        using var destinationStream = File.Create($"{destinationDirectory}{fileName.LastIndexOf('\\'))}");
        await originStream.CopyToAsync(destinationStream);
    }).WhenAll();
}
```

## System.Threading.Tasks.ValueTask
同期処理のみで完結している処理フローの場合は値をそのまま保持する。 => 非同期じゃないのに```Task, Task<T>```が作られるのはパフォーマンスペナルティがあるため避けたい

非同期が必要な場合のみ、```Task``` or ```IValueTaskSource```を生成する。 <b>※　```IValueTaskSource```は```.NET Core 2.1```から</b>

```Task / IValueTaskSource```は都度生成ではなく、キャッシュを持っている。（.NET Core 2.1以降のIValueTaskSource誕生後からの仕組みと思われる。）

```cs
public async ValueTask DisposeAsync() {
    if (DbTransaction.IsInvalid()) {
        return;
    }

    if (ScopeIsComplete) {
        await DbTransaction.CommitAsync();
        return;
    }

    await DbTransaction.RollbackAsync();
}
```

必ず非同期処理を行う場合 => ```Task,Task<T>```

Validation等の同期処理後、Early Return等がありうる場合 => ```ValueTask, ValueTask<T>```

というのが、C#7＆.NET Core 2.0時代での筋の良い判断だった。

が、.NET Core 2.1での```IValueTaskSource```の登場を鑑みると、最早使える場面では```ValueTask, ValueTask<T>```が筋が良い選択に思える。

### ```WhenAll / WhenAny / Lazy```が使えない問題
```ValueTask```には、```WhenAll / WhenAny / Lazy``` 等のメソッドが存在していない。
そのため```AsTask```を利用してのキャストが必要（インスタンス生成よりはペナルティは小さいもののゼロではない。）

しかも、毎回```AsTask```を挟まないといけないので少々面倒。

```Task``` => https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.tasks.task?view=net-5.0

```ValueTask``` => https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.tasks.valuetask?view=net-5.0

#### ```ValueTaskSupplement``` を使うことで上記が利用可能になる
https://www.nuget.org/packages/ValueTaskSupplement/

## await using
using を非同期で宣言可能になった。（C#8～）
    
using 宣言した変数のライフタイム終了時にDisposeAsyncが発火し、解体処理が行われる仕組み。

```cs
// https://github.com/CreatioVitae/ORMIntegrator/blob/main/src/ORMIntegrator/ScopedTransaction.cs#L27
await using var scopedTransaction = await BbsScopedTransactionBuilder.BeginScopedTransactionAsync();
```
### using との違い
```using```は```IDisposable``` インターフェースを実装していることで利用可能になる構文。
    
一方、```await using```は```IAsyncDisposable``` インターフェースを実装していなくても、```DisposeAsync```メソッドが```Member```にいれば利用可能。（いつものパターンベース実装）

### ```IAsyncDisposable``` の存在意義
DI Containerは通常、```IDisposable```インターフェースが実装されている場合、インスタンスのライフタイム終了後```Dispose```メソッドを発火してくれる。

一方、（少なくともASP.NET Core 標準のDI Containerは）パターンベース実装には対応しておらず、```DisposeAsync```メソッドを記述しただけでは発火してくれない。

あくまで、DI Container での利用を想定するのであれば```IAsyncDisposable```の実装が必要。

## 非同期ストリーム（C#8～）
### `IAsyncEnumerable<T>`を利用することで　`await foreach` が記述可能

```
static Hoge SetAndGetEntity(Hoge hoge, HogeEditParameter editParameter, DateTime requestedDatetime) {
    (hoge.Name, hoge.DatetimeUpd) = (editParameter.Name, requestedDatetime);
    return hoge;
}

var updateTargetIds = editParameters.Select(e => e.Id);

//Use AsAsyncEnumerable...
await foreach (var updateTarget in SqlManager.DbContext.Hoges.Where(e => updateTargetIds.Contains(e.Id)).AsAsyncEnumerable()) {
    if (editParameters.SingleOrDefault(e => e.Id == updateTarget.Id) is not HogeEditParameter editParameter) {
        throw new NullReferenceException($"更新対象が見つかりませんでした。{nameof(updateTarget.Id)}:{updateTarget.Id}");
    }

    SqlManager.DbContext.Hoges.Update(SetAndGetEntity(updateTarget, editParameter, DateTime.Now));
}
```

### await using との関わり
`foreach`では、`IDisposable`を実装している`Object`の場合、`using`ステートメント / 宣言を兼ねる。
`await foreach`の場合も同様にusingを兼ねる仕様のため、同じタイミングで`await using`が実装された。
