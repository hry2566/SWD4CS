# SWD4CS
 * SWD4CSは、Simple WinForms Designer for CSharp (VSCode)の略  
 * VisualStudioは重いので、VSCodeをよく使う。がデザイナがない。  
 * デザイナの作り方が全く分からず、思い付くままに書いてみた。  
 * この作り方では複雑なものは無理。ちょっとしたツール程度なら使えるかも？
  
## 開発環境
 * Windows11 Home  
 * VisualStudio2022 C# → VSCode .net6.0
 
## スクリーンショット  
![SWD4CS](https://user-images.githubusercontent.com/86605611/152679486-e8f7bbed-69b4-4186-b402-35d7bd2fec8f.png)
![SWD4CS](https://user-images.githubusercontent.com/86605611/152784518-c135ec3a-e156-4163-8f8d-90cc023d8448.png)
※ControlTreeは表示のみ  


## 動画
 https://youtu.be/BJAhuU2W3uM  
 https://youtu.be/3LyjAvXLpYg  
 https://youtu.be/82qa0vOP_qk  
 https://youtu.be/FkDaMW4hGyk
 
## ブログ
 https://danpapa-hry.hateblo.jp/entry/2022/02/23/210416
 
## 実装
 ・ポトペタ  
 ・一部のプロパティ変更  
 ・Button  
 ・CheckBox  
 ・CheckedListBox  
 ・ComboBox  
 ・DataGridView  
 ・DateTimePicker  
 ・DomainUpDown  
 ・FlowLayoutPanel  
 ・GroupBox  
 ・HScrollBar  
 ・Label  
 ・LinkLabel  
 ・ListBox  
 ・ListView  
 ・MaskedTextBox  
 ・MonthCalender  
 ・Panel  
 ・PictureBox  
 ・ProgressBar  
 ・PropertyGrid  
 ・RadioButton  
 ・RichTextBox  
 ・SplitContainer  
 ・Splitter  
 ・StatusStrip  
 ・TabControl  
 ・TableLayoutPanel  
 ・TabPage  
 ・TextBox  
 ・TrackBar  
 ・TreeView  
 ・VScrollBar  
 ・ColorDialog  
 ・FolderBrowserDialog  
 ・FontDialog  
 ・imageList  
 ・OpenFiledialog  
 ・SaveFiledialog  
 ・Timer  
 ・Designer.csファイルのRead/Write　~~（ただし、SWD4CS以外で編集したものは開けない）~~  
 ・他のコントローラー等は必要になったら追加する。

## 対応プロパティ（Type）
 ・System.Drawing.Point  
 ・System.Drawing.Size  
 ・System.String  
 ・System.Boolean  
 ・System.Int32  
 ・System.Windows.Forms.AnchorStyles  
 ・System.Windows.Forms.DockStyle  
 ・System.Drawing.ContentAlignment  
 ・System.Windows.Forms.ScrollBars  
 ・System.Windows.Forms.HorizontalAlignment  
 ・System.Drawing.Color  
 ・System.Windows.Forms.FormStartPosition  
 ・System.Windows.Forms.FormWindowState  
 ・System.Windows.Forms.FixedPanel  
 ・System.Windows.Forms.PictureBoxSizeMode  
 ・System.Windows.Forms.View  
 ・System.Windows.Forms.Orientation  
 ・System.Windows.Forms.FormBorderStyle  
 ・System.Windows.Forms.AutoScaleMode  
 ・System.Drawing.Font  
 ・System.Windows.Forms.TableLayoutPanelCellBorderStyle  

## コントロール追加方法  
 * 「// コントロール追加時に下記を編集すること」に追記  
・cls_control.cs  
