namespace Menu_14
{
    partial class FormLinks
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
            this.labelNameRu = new System.Windows.Forms.Label();
            this.labelNameRuShow = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nameLat = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.triadElements = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelNameRu
            // 
            this.labelNameRu.AutoSize = true;
            this.labelNameRu.Location = new System.Drawing.Point(26, 9);
            this.labelNameRu.Name = "labelNameRu";
            this.labelNameRu.Size = new System.Drawing.Size(158, 13);
            this.labelNameRu.TabIndex = 0;
            this.labelNameRu.Text = "название растения по русски";
            // 
            // labelNameRuShow
            // 
            this.labelNameRuShow.AutoSize = true;
            this.labelNameRuShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.labelNameRuShow.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelNameRuShow.Location = new System.Drawing.Point(26, 22);
            this.labelNameRuShow.Name = "labelNameRuShow";
            this.labelNameRuShow.Size = new System.Drawing.Size(150, 17);
            this.labelNameRuShow.TabIndex = 1;
            this.labelNameRuShow.Text = "покажем по русски";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(405, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "и то же на латани";
            // 
            // nameLat
            // 
            this.nameLat.AutoSize = true;
            this.nameLat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.nameLat.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.nameLat.Location = new System.Drawing.Point(380, 22);
            this.nameLat.Name = "nameLat";
            this.nameLat.Size = new System.Drawing.Size(57, 17);
            this.nameLat.TabIndex = 3;
            this.nameLat.Text = "latText";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(701, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(216, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "и некоторые рассуждения, комментарии";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(29, 54);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(613, 487);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Arial", 7.714285F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(678, 41);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Horizontal;
            this.richTextBox1.Size = new System.Drawing.Size(291, 99);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            this.richTextBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseDoubleClick_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(757, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "заголовки и ссылки";
            // 
            // triadElements
            // 
            this.triadElements.FormattingEnabled = true;
            this.triadElements.HorizontalScrollbar = true;
            this.triadElements.Location = new System.Drawing.Point(678, 186);
            this.triadElements.Name = "triadElements";
            this.triadElements.ScrollAlwaysVisible = true;
            this.triadElements.Size = new System.Drawing.Size(291, 355);
            this.triadElements.TabIndex = 10;
            this.triadElements.SelectedIndexChanged += new System.EventHandler(this.triadElements_SelectedIndexChanged);
            // 
            // FormLinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 570);
            this.Controls.Add(this.triadElements);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nameLat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelNameRuShow);
            this.Controls.Add(this.labelNameRu);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.071428F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.ForeColor = System.Drawing.SystemColors.GrayText;
            this.Name = "FormLinks";
            this.Text = "список статей, сайтов";
            this.Load += new System.EventHandler(this.FormLinks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelNameRu;
        public System.Windows.Forms.Label labelNameRuShow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label nameLat;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox triadElements;
    }
}