namespace Laba1
{
    partial class SourceCodeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourceCodeForm));
            this.CompilerButton = new System.Windows.Forms.Button();
            this.ScannerButton = new System.Windows.Forms.Button();
            this.ParserButton = new System.Windows.Forms.Button();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ExButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CompilerButton
            // 
            this.CompilerButton.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CompilerButton.Location = new System.Drawing.Point(12, 32);
            this.CompilerButton.Name = "CompilerButton";
            this.CompilerButton.Size = new System.Drawing.Size(148, 41);
            this.CompilerButton.TabIndex = 0;
            this.CompilerButton.Text = "Comiler.cs";
            this.CompilerButton.UseVisualStyleBackColor = true;
            this.CompilerButton.Click += new System.EventHandler(this.CompilerButton_Click);
            // 
            // ScannerButton
            // 
            this.ScannerButton.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ScannerButton.Location = new System.Drawing.Point(221, 32);
            this.ScannerButton.Name = "ScannerButton";
            this.ScannerButton.Size = new System.Drawing.Size(148, 41);
            this.ScannerButton.TabIndex = 1;
            this.ScannerButton.Text = "Scanner.cs";
            this.ScannerButton.UseVisualStyleBackColor = true;
            this.ScannerButton.Click += new System.EventHandler(this.ScannerButton_Click);
            // 
            // ParserButton
            // 
            this.ParserButton.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ParserButton.Location = new System.Drawing.Point(435, 32);
            this.ParserButton.Name = "ParserButton";
            this.ParserButton.Size = new System.Drawing.Size(148, 41);
            this.ParserButton.TabIndex = 2;
            this.ParserButton.Text = "Parser.cs";
            this.ParserButton.UseVisualStyleBackColor = true;
            this.ParserButton.Click += new System.EventHandler(this.ParserButton_Click);
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox.Location = new System.Drawing.Point(0, 0);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.ReadOnly = true;
            this.richTextBox.Size = new System.Drawing.Size(776, 317);
            this.richTextBox.TabIndex = 3;
            this.richTextBox.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 92);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.richTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(776, 346);
            this.splitContainer1.SplitterDistance = 317;
            this.splitContainer1.TabIndex = 4;
            // 
            // ExButton
            // 
            this.ExButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExButton.Location = new System.Drawing.Point(640, 33);
            this.ExButton.Name = "ExButton";
            this.ExButton.Size = new System.Drawing.Size(148, 41);
            this.ExButton.TabIndex = 5;
            this.ExButton.Text = "Выход";
            this.ExButton.UseVisualStyleBackColor = true;
            this.ExButton.Click += new System.EventHandler(this.ExButton_Click);
            // 
            // SourceCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ExButton);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ParserButton);
            this.Controls.Add(this.ScannerButton);
            this.Controls.Add(this.CompilerButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SourceCodeForm";
            this.Text = "Исходный код программы";
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CompilerButton;
        private System.Windows.Forms.Button ScannerButton;
        private System.Windows.Forms.Button ParserButton;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button ExButton;
    }
}