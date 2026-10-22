# BackToA1

<div align="center" markdown="1">

日本語 | [简体中文](README_zh_cn.md)

Excelファイルを共有するときも、見やすい状態に。

ブックを閉じるだけで、各ワークシートの選択セルを自動的に A1 に戻します。

[![](https://img.shields.io/github/v/release/boris1993/BackToA1)](https://github.com/boris1993/BackToA1)
[![.NET](https://img.shields.io/badge/.NET-4.7.2-512BD4.svg)](https://dotnet.microsoft.com/)
[![Office](https://img.shields.io/badge/Office-2013+-D83B01.svg)](https://www.microsoft.com/microsoft-365)
[![License](https://img.shields.io/github/license/boris1993/BackToA1)](LICENSE.txt)
![Downloads](https://img.shields.io/github/downloads/boris1993/BackToA1/total)

<a href="https://www.buymeacoffee.com/boris1993" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me a Coffee" style="height: 60px !important;width: 217px !important;" ></a>

</div>

---

## 動作デモ

![](./docs/BackToA1Demo.gif)

## システム要件

- Windows 7 以降
- Microsoft Office 2013 以降
- .NET Framework 4.7.2 以降

## インストール

- [Releases](https://github.com/boris1993/BackToA1/releases/latest) から最新版をダウンロードします。
- ZIP ファイルの内容を任意のフォルダーに展開します。
- コード署名証明書は自分で作成した自己署名証明書のため、以下の手順でインストールし、信頼する必要があります。
    - 展開したフォルダーで PowerShell を起動し、以下のコマンドを実行して、`BackToA1_Signing.cer` の拇印が `B68CBB359417EAB50C2133E9D6AB51277267D7A5` と一致することを確認します。
    
    ```powershell
    $cert = [System.Security.Cryptography.X509Certificates.X509Certificate2]::new(
        (Resolve-Path ".\BackToA1_Signing.cer")
    )

    $cert.Thumbprint
    ```

    - 出力された拇印を上記の値と比較してください（大文字・小文字と空白の違いは無視できます）。拇印が一致しない場合は、インストールしないでください。
    - スクリプトが Windows によってブロックされている場合は、以下のコマンドを実行してブロックを解除します。
    
    ```powershell
    Unblock-File -LiteralPath .\install-and-trust-code-signing-cert.ps1
    ```

    - 同じ PowerShell ウィンドウで `.\install-and-trust-code-signing-cert.ps1` を実行し、表示される指示に従って証明書をインストールします。
- `setup.exe` を実行してアドインをインストールします。

## アンインストール

Windows の設定で「インストールされているアプリ」を開き、一覧から BackToA1 を見つけて「アンインストール」をクリックしてください。

## ビルド方法

- Visual Studio 2026 と Microsoft 365 開発ツールをインストールします。
- 本リポジトリをクローンします。
- Visual Studio 2026 で、本プロジェクトのソリューションファイル `BackToA1.slnx` を開きます。

## ライセンス

本プロジェクトは MIT ライセンスの下でオープンソースとして公開されています。ライセンスの全文は [LICENSE](LICENSE.txt) をご覧ください。
