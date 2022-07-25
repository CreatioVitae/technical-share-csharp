# Wip:interface is 何
`OOP`における`interface`は、`object`が網羅すべき規約 / 契約 / 制約 を表す。  
`object`の`design`と`implementation / using`を繋ぐもの、とも言える。

## interfaceの効能
* 疎結合（`object`自身ではなく、`interface`への依存とする。）
* 機能の保証（`object`の`member`定義の強制による。ポリモーフィズムも此処に類する。）
* `object`への操作制御（`interface`を`adaptive`に利用しながら、`object`への操作への制約を付与する。）

## Wip:疎結合（`object`自身ではなく、`interface`への依存とする。）

## 機能の保証（`object`の`member`定義の強制による。ポリモーフィズムも此処に類する。）
冒頭で書いた、  

> `OOP`における`interface`は、`object`が網羅すべき規約 / 契約 / 制約 を表す

を最も素直に表す効能にあたる。

`interface`を実装した`object`は、実装対象となる`interface`に定義された`member`が実装されていることが保証される。

### `IEnumerable<T>`の例
Docs : https://docs.microsoft.com/ja-jp/dotnet/api/system.collections.generic.ienumerable-1?view=net-6.0

#### `IEnumerable<T>`は`IEnumerator<T>`を返す`GetEnumerator()`を実装する。
Docs : https://docs.microsoft.com/ja-jp/dotnet/api/system.collections.generic.ienumerator-1?view=net-6.0

`foreach`ステートメントは、
* `GetEnumerator()`を実装している。
* `GetEnumerator()`から返される`object`が、`MoveNext` `method` 及び, `Curent` `prop`を実装している。
ケースで`Available`となる。  

つまり、`IEnumerable<T>`を実装しているobjectは`foreach`ステートメントが`Available`であることが保証されている。

#### ポリモーフィズムについての補足

`ICollection<T>`, `IList<T>`, `List<T>`等は、`IEnumerable<T>`を実装しているため、これらのobjectは`IEnumerable<T>`を通る。

## Wip: `object`への操作制御
