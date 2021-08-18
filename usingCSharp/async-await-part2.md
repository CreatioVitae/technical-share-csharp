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
```
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
```
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