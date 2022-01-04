namespace SWD4CS
{
    partial class cls_button
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent(int index, int X, int Y)
        {
            components = new System.ComponentModel.Container();

            this.Name = "Button" + index.ToString();
            this.Text = "Button" + index.ToString();
            this.Size = new System.Drawing.Size(80, 24);
            this.Location = new System.Drawing.Point(X, Y);
            this.TabIndex = index;
        }

        #endregion
    }
}
