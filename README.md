# WinFormsApp2


## .Netのパッケージに関して
パッケージ一覧の表示(.csprojに反映される)
`dotnet list package`


## migrationに関して

`Data/AppDbContextFactory`<br>
AppDbContextの作り方を、マイグレーションのツールに教えるクラス<br>
これがあることでAppDbContextは接続先を知らないままでいられる


`Data/AppDbContext `<br>
実際にDBとやり取りする窓口(モデルの定義も持つ)<br>
実際にSQLを発行してDBに触る<br>

`Data/Memebers`<br>
実際のテーブルに相当する部分

### 実際のコマンド例
コマンドはVisualStudioのツール→Nugetパッケージマネージャー→パッケージマネージャーコンソールから<br>
<br>
マイグレーションファイル作成コマンド<br>
`Add-Migration 変更内容の名称`

マイグレーションの反映<br>
`Update-Database`