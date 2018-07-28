namespace WidgetFactory
{
    partial class WidgetFactory
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
            this.rtxt_WidgFact = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtxt_WidgFact
            // 
            this.rtxt_WidgFact.Location = new System.Drawing.Point(12, 87);
            this.rtxt_WidgFact.Name = "rtxt_WidgFact";
            this.rtxt_WidgFact.Size = new System.Drawing.Size(645, 96);
            this.rtxt_WidgFact.TabIndex = 0;
            this.rtxt_WidgFact.Text = "";
            // 
            // WidgetFactory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 195);
            this.Controls.Add(this.rtxt_WidgFact);
            this.Name = "WidgetFactory";
            this.Text = "Widget Factory 1.0";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxt_WidgFact;
    }
}

