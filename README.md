# 木漏れ日の迷宮

## 作品概要
![Title](https://raw.githubusercontent.com/Wata0912/Labyrinth_of_Sunlight/refs/heads/main/images/Tiitle.png)

### ゲームについて
- プレイヤーをスタート地点からゴール地点まで導くスライドパズルゲーム
- 4×4のスライドパズルを操作し、道タイルをつなげてゴールまでの道を作成します。
- パズルを完成させた後、プレイヤーを操作してゴールまでたどり着くことができればステージクリアとなります。
- ステージをクリアするとアイテムを獲得でき、コレクションとして収集状況を確認できます。

---

## ゲーム画面

### タイトル画面
![Title](https://raw.githubusercontent.com/Wata0912/Labyrinth_of_Sunlight/refs/heads/main/images/Tiitle.png)

ゲームの開始や終了を選択する画面です。

---

### パズル画面
![Puzzle](https://raw.githubusercontent.com/Wata0912/Labyrinth_of_Sunlight/refs/heads/main/images/SlidePanel.png)

- 4×4のスライドパズルを操作して道をつなげます。
- スタートからゴールまで道が完成するようにタイルを並べ替えます。
- 空いているマスを利用してタイルをスライドさせることができます。

---

### プレイヤー移動
![Player](https://raw.githubusercontent.com/Wata0912/Labyrinth_of_Sunlight/refs/heads/main/images/SlidePanel1.png)

- パズル完成後、プレイヤーを操作できます。
- 作成した道を通ってゴールに到達するとステージクリアとなります。

---

### アイテム画面
![Items](https://raw.githubusercontent.com/Wata0912/Labyrinth_of_Sunlight/refs/heads/main/images/Items.png)

- ステージクリア時に獲得したアイテムを確認できます。
- 収集したアイテムはコレクションとして一覧表示されます。

---

## 制作で工夫した点

- パズルを解くだけでなく、完成した道を実際にプレイヤーが歩くことで達成感を得られるゲーム性を目指しました。
- アイテム収集要素を追加し、クリア後も繰り返し遊べるようにしました。
- MySQLを利用してユーザーデータやアイテムの取得状況を保存し、プレイヤーの進行状況を管理できるようにしました。
- PHPを用いて管理ページを作成し、ユーザー情報やゲームデータをブラウザ上から確認・管理できるようにしました。
- https://github.com/Wata0912/puzzle-app
---

## 開発環境

### クライアント
- Unity
- C#

### サーバー・Web
- PHP

### データベース
- MySQL

## 開発人数

1人
