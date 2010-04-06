namespace Nibbler
{
    partial class LoaderConfig
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
            this.tb_sc1dir = new System.Windows.Forms.TextBox();
            this.tb_sc2dir = new System.Windows.Forms.TextBox();
            this.tb_wc3dir = new System.Windows.Forms.TextBox();
            this.lbl_sc1dir = new System.Windows.Forms.Label();
            this.lbl_sc2dir = new System.Windows.Forms.Label();
            this.lbl_wc3dir = new System.Windows.Forms.Label();
            this.btn_sc1dir = new System.Windows.Forms.Button();
            this.btn_sc2dir = new System.Windows.Forms.Button();
            this.btn_wc3dir = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // tb_sc1dir
            // 
            this.tb_sc1dir.Location = new System.Drawing.Point(12, 29);
            this.tb_sc1dir.Name = "tb_sc1dir";
            this.tb_sc1dir.ReadOnly = true;
            this.tb_sc1dir.Size = new System.Drawing.Size(262, 20);
            this.tb_sc1dir.TabIndex = 0;
            // 
            // tb_sc2dir
            // 
            this.tb_sc2dir.BackColor = System.Drawing.SystemColors.Control;
            this.tb_sc2dir.Location = new System.Drawing.Point(12, 72);
            this.tb_sc2dir.Name = "tb_sc2dir";
            this.tb_sc2dir.ReadOnly = true;
            this.tb_sc2dir.Size = new System.Drawing.Size(260, 20);
            this.tb_sc2dir.TabIndex = 1;
            // 
            // tb_wc3dir
            // 
            this.tb_wc3dir.Location = new System.Drawing.Point(12, 116);
            this.tb_wc3dir.Name = "tb_wc3dir";
            this.tb_wc3dir.ReadOnly = true;
            this.tb_wc3dir.Size = new System.Drawing.Size(260, 20);
            this.tb_wc3dir.TabIndex = 2;
            // 
            // lbl_sc1dir
            // 
            this.lbl_sc1dir.AutoSize = true;
            this.lbl_sc1dir.Location = new System.Drawing.Point(12, 13);
            this.lbl_sc1dir.Name = "lbl_sc1dir";
            this.lbl_sc1dir.Size = new System.Drawing.Size(96, 13);
            this.lbl_sc1dir.TabIndex = 3;
            this.lbl_sc1dir.Text = "StarCraft Directory:";
            // 
            // lbl_sc2dir
            // 
            this.lbl_sc2dir.AutoSize = true;
            this.lbl_sc2dir.Location = new System.Drawing.Point(12, 56);
            this.lbl_sc2dir.Name = "lbl_sc2dir";
            this.lbl_sc2dir.Size = new System.Drawing.Size(105, 13);
            this.lbl_sc2dir.TabIndex = 4;
            this.lbl_sc2dir.Text = "StarCraft II Directory:";
            // 
            // lbl_wc3dir
            // 
            this.lbl_wc3dir.AutoSize = true;
            this.lbl_wc3dir.Location = new System.Drawing.Point(12, 100);
            this.lbl_wc3dir.Name = "lbl_wc3dir";
            this.lbl_wc3dir.Size = new System.Drawing.Size(109, 13);
            this.lbl_wc3dir.TabIndex = 5;
            this.lbl_wc3dir.Text = "WarCraft III Directory:";
            // 
            // btn_sc1dir
            // 
            this.btn_sc1dir.Location = new System.Drawing.Point(280, 29);
            this.btn_sc1dir.Name = "btn_sc1dir";
            this.btn_sc1dir.Size = new System.Drawing.Size(75, 23);
            this.btn_sc1dir.TabIndex = 6;
            this.btn_sc1dir.Text = "Change";
            this.btn_sc1dir.UseVisualStyleBackColor = true;
            this.btn_sc1dir.Click += new System.EventHandler(this.BtnSc1DirClick);
            // 
            // btn_sc2dir
            // 
            this.btn_sc2dir.Location = new System.Drawing.Point(280, 72);
            this.btn_sc2dir.Name = "btn_sc2dir";
            this.btn_sc2dir.Size = new System.Drawing.Size(75, 23);
            this.btn_sc2dir.TabIndex = 7;
            this.btn_sc2dir.Text = "Change";
            this.btn_sc2dir.UseVisualStyleBackColor = true;
            this.btn_sc2dir.Click += new System.EventHandler(this.BtnSc2DirClick);
            // 
            // btn_wc3dir
            // 
            this.btn_wc3dir.Location = new System.Drawing.Point(280, 114);
            this.btn_wc3dir.Name = "btn_wc3dir";
            this.btn_wc3dir.Size = new System.Drawing.Size(75, 23);
            this.btn_wc3dir.TabIndex = 8;
            this.btn_wc3dir.Text = "Change";
            this.btn_wc3dir.UseVisualStyleBackColor = true;
            this.btn_wc3dir.Click += new System.EventHandler(this.BtnWc3DirClick);
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(140, 146);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 9;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // LoaderConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(362, 181);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.btn_wc3dir);
            this.Controls.Add(this.btn_sc2dir);
            this.Controls.Add(this.btn_sc1dir);
            this.Controls.Add(this.lbl_wc3dir);
            this.Controls.Add(this.lbl_sc2dir);
            this.Controls.Add(this.lbl_sc1dir);
            this.Controls.Add(this.tb_wc3dir);
            this.Controls.Add(this.tb_sc2dir);
            this.Controls.Add(this.tb_sc1dir);
            this.Name = "LoaderConfig";
            this.Text = "Nibbler Setup";
            this.Load += new System.EventHandler(this.Formsetup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_sc1dir;
        private System.Windows.Forms.TextBox tb_sc2dir;
        private System.Windows.Forms.TextBox tb_wc3dir;
        private System.Windows.Forms.Label lbl_sc1dir;
        private System.Windows.Forms.Label lbl_sc2dir;
        private System.Windows.Forms.Label lbl_wc3dir;
        private System.Windows.Forms.Button btn_sc1dir;
        private System.Windows.Forms.Button btn_sc2dir;
        private System.Windows.Forms.Button btn_wc3dir;
        private System.Windows.Forms.Button btn_ok;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}