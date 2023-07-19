namespace IhsanTssBilgiCekme
{
    partial class Form1
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
            this.webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.addressBar = new System.Windows.Forms.TextBox();
            this.sayfa1 = new System.Windows.Forms.Button();
            this.sayfa2 = new System.Windows.Forms.Button();
            this.sayfa3 = new System.Windows.Forms.Button();
            this.kontrol = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).BeginInit();
            this.SuspendLayout();
            // 
            // webView21
            // 
            this.webView21.AllowExternalDrop = true;
            this.webView21.CreationProperties = null;
            this.webView21.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView21.Location = new System.Drawing.Point(12, 41);
            this.webView21.Name = "webView21";
            this.webView21.Size = new System.Drawing.Size(1443, 602);
            this.webView21.Source = new System.Uri("https://www.tamamlayicisaglik.com/anlasmali-saglik-kurumlari", System.UriKind.Absolute);
            this.webView21.TabIndex = 0;
            this.webView21.ZoomFactor = 1D;
            // 
            // addressBar
            // 
            this.addressBar.Location = new System.Drawing.Point(12, 12);
            this.addressBar.Name = "addressBar";
            this.addressBar.Size = new System.Drawing.Size(466, 20);
            this.addressBar.TabIndex = 1;
            // 
            // sayfa1
            // 
            this.sayfa1.Location = new System.Drawing.Point(489, 12);
            this.sayfa1.Name = "sayfa1";
            this.sayfa1.Size = new System.Drawing.Size(221, 23);
            this.sayfa1.TabIndex = 2;
            this.sayfa1.Text = "Şehir Değiştir";
            this.sayfa1.UseVisualStyleBackColor = true;
            this.sayfa1.Click += new System.EventHandler(this.sayfa1_Click);
            // 
            // sayfa2
            // 
            this.sayfa2.Location = new System.Drawing.Point(716, 12);
            this.sayfa2.Name = "sayfa2";
            this.sayfa2.Size = new System.Drawing.Size(221, 23);
            this.sayfa2.TabIndex = 3;
            this.sayfa2.Text = "Kurum Değiştir";
            this.sayfa2.UseVisualStyleBackColor = true;
            this.sayfa2.Click += new System.EventHandler(this.sayfa2_Click);
            // 
            // sayfa3
            // 
            this.sayfa3.Location = new System.Drawing.Point(943, 12);
            this.sayfa3.Name = "sayfa3";
            this.sayfa3.Size = new System.Drawing.Size(221, 23);
            this.sayfa3.TabIndex = 4;
            this.sayfa3.Text = "Bilgi Çek";
            this.sayfa3.UseVisualStyleBackColor = true;
            this.sayfa3.Click += new System.EventHandler(this.sayfa3_Click);
            // 
            // kontrol
            // 
            this.kontrol.Location = new System.Drawing.Point(1170, 12);
            this.kontrol.Name = "kontrol";
            this.kontrol.Size = new System.Drawing.Size(221, 23);
            this.kontrol.TabIndex = 5;
            this.kontrol.Text = "Kontrol";
            this.kontrol.UseVisualStyleBackColor = true;
            this.kontrol.Click += new System.EventHandler(this.kontrol_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1467, 667);
            this.Controls.Add(this.kontrol);
            this.Controls.Add(this.sayfa3);
            this.Controls.Add(this.sayfa2);
            this.Controls.Add(this.sayfa1);
            this.Controls.Add(this.addressBar);
            this.Controls.Add(this.webView21);
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.webView21)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private System.Windows.Forms.TextBox addressBar;
        private System.Windows.Forms.Button sayfa1;
        private System.Windows.Forms.Button sayfa2;
        private System.Windows.Forms.Button sayfa3;
        private System.Windows.Forms.Button kontrol;
    }
}

