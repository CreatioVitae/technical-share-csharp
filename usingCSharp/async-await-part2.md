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
async / await は I/O待ちの並列実行は得意領域。

### foreach + await;
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

//適当にDirectory指定
var appDirectory = new FileInfo(typeof(EnumerableExtensionLibrary).Assembly.Location).Directory.Parent.Parent.Parent.FullName;

await CopyAsync((originDirectory: $"{appDirectory}/Origin",destinationDirectory: $"{appDirectory}/Destination"));
```