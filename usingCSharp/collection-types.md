# using Collection Types

## 大まかなcollection型とその特徴
###  ```IEnumerable<T>```
* 列挙可能な型を表す。（ = ```foreach``` が利用可能）
* Countプロパティが存在**しない**
* 要素の追加・削除が**できない**
* 遅延評価が特徴のため十分留意すること。（遅延評価についてわからない場合はIterator Blockの項を参照推奨。）

一度だけ、foreachやLinq To Objectでの処理を行う場合は積極的に利用する。

逆に、複数回Collectionを利用する場合は```.ToList()```等で先行評価してしまうか、```Lazy<T>```クラスを定義すべき。

### ```ICollection<T>```
※ ```ICollection<T>```は```IEnumerable<T>```を実装するため、```foreach``` が利用可能
* Countプロパティが存在**する**
* 要素の追加・削除が**できる**
* 順序関係を持たない（ = ```foreach``` での取得時の順序が保証されない。）

要素数を参照したい場合や、要素の追加・削除が必要な場合、かつ、順序関係を維持する必要がない場合は積極的に利用する。

### ```IList<T>```
※ ```IList<T>```は```ICollection<T>```、および、```IEnumerable<T>```を実装するため、```foreach```や ```Count```プロパティが利用可能
* 要素の追加・削除が**できる**
* 順序関係を持つ（ = ```foreach``` での取得時の順序が保証される。）
* インデクサが利用可能

要素数を参照したい場合や、要素の追加・削除が必要な場合、かつ、順序関係を維持する必要がある場合、もしくはインデクサを利用したい場合は積極的に利用する。

### ```List<T>```
※ ```List<T>```は
```IList<T>```,
```ICollection<T>```,
```IEnumerable<T>```,
```IReadOnlyList<T>```,
```IReadOnlyCollection<T>```,
```IList```
を実装する。

```List<T>```の持っている便利メソッド（順序変更や探索処理を行うようなメソッド等）を使いたい場合は```List<T>```を利用する。

#### ```List<T>``` (MS Docs)
https://docs.microsoft.com/ja-jp/dotnet/api/system.collections.generic.list-1?view=net-5.0

### Todo:```IReadOnlyCollection```
### Todo:```IReadOnlyList<T>```
### Todo:```ReadOnlyCollection<T>```