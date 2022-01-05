# SWD4CS
 ・SWD4CSは、Simple WinForms Designer for CSharp (VSCode)の略  
 ・VisualStudioは重いので、VSCodeをよく使う。がデザイナがない。  
 ・デザイナの作り方が全く分からず、思い付くままに書いてみた。  
 ・この作り方では複雑なものは無理。ちょっとしたツール程度なら使えるかも？
  
## 開発環境
 ・Windows11 Home  
 ・VisualStudio2022 C# → VSCode .net6.0
 
## 動画
 https://youtu.be/3KAt203qVsA
 
## ブログ
 https://danpapa-hry.hateblo.jp/entry/2022/01/01/211917
 
## 実装
 ・ポトペタ  
 ・一部のプロパティ変更  
 ・Button  
 ・Label  
 ・TextBox  
 ・ListBox  
 ・GroupBox  
 ・他のコントローラー等は必要になったら追加する。

## コントロール追加方法
 ・cls_button等を参考に追加したいコントロールを継承したクラスを作成  
 ・「// コントロール追加時に下記を編集すること」に追記。（cls_form.cs, cls_selectbox.cs, MainForm.cs）  
 ・追記は、コピーしてクラス名等を追加したクラスに変更するだけ。
