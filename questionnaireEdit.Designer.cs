namespace Menu_14
{
    partial class questionnaireEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(questionnaireEdit));
            this.richTextBoxEdit = new System.Windows.Forms.RichTextBox();
            this.buttonRejection = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBoxEdit
            // 
            this.richTextBoxEdit.Location = new System.Drawing.Point(36, 42);
            this.richTextBoxEdit.Name = "richTextBoxEdit";
            this.richTextBoxEdit.Size = new System.Drawing.Size(727, 462);
            this.richTextBoxEdit.TabIndex = 0;
            this.richTextBoxEdit.Text = "";
            // 
            // buttonRejection
            // 
            this.buttonRejection.Location = new System.Drawing.Point(530, 532);
            this.buttonRejection.Name = "buttonRejection";
            this.buttonRejection.Size = new System.Drawing.Size(178, 42);
            this.buttonRejection.TabIndex = 1;
            this.buttonRejection.Text = "Отказ";
            this.buttonRejection.UseVisualStyleBackColor = true;
            this.buttonRejection.Click += new System.EventHandler(this.buttonRejection_Click_1);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(69, 533);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(264, 41);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click_1);
            // 
            // questionnaireEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 590);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonRejection);
            this.Controls.Add(this.richTextBoxEdit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "questionnaireEdit";
            this.Text = "аннотация";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxEdit;
        private System.Windows.Forms.Button buttonRejection;
        private System.Windows.Forms.Button buttonSave;
    }
}