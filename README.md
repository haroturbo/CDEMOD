https://web.archive.org/web/20210226034927/http://ijiro.daiwa-hotcom.com/data/CDE.html

📄 プロジェクト概要
CDEMOD は、PSP 用チートツール「CWCheat」のデータベースを編集・管理するための Windows アプリケーションです。Visual Basic .NET で開発されており、GUI ベースで操作できます。日本語形態素解析エンジン MeCab を利用していることから、日本語データの処理にも対応していると考えられます。

📁 主なディレクトリとファイル構成
Classes/, Forms/, Icons/: アプリケーションの主要なクラス、フォーム、アイコンなどのリソースが含まれています。

CWcheat Database Editor.sln, CWcheat Database Editor.vbproj: Visual Studio 用のソリューションおよびプロジェクトファイルです。

MecabDotNet.dll, libmecab.dll: MeCab を .NET から利用するためのラッパーおよびネイティブライブラリです。

updater/: アップデート関連の処理を行うモジュールが含まれている可能性があります。

bin/Release/: ビルド済みの実行ファイルが出力されるディレクトリです。

🛠️ ビルド手順
Visual Studio の準備: Visual Studio（推奨バージョンは 2010 以降）をインストールします。

リポジトリのクローン: 以下のコマンドでリポジトリをクローンします。

bash
コピーする
編集する
git clone https://github.com/haroturbo/CDEMOD.git
プロジェクトの読み込み: CWcheat Database Editor.sln を Visual Studio で開きます。

依存関係の確認: MecabDotNet.dll および libmecab.dll がプロジェクトに正しく参照されていることを確認します。

ビルドの実行: 「ビルド」メニューから「ソリューションのビルド」を選択し、ビルドを実行します。
GitHub

🧩 主な機能（推定）
CWCheat 用チートコードの追加・編集・削除

チートデータベースのインポートおよびエクスポート

日本語データの処理（MeCab を利用）

GUI ベースの直感的な操作

