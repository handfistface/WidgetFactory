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
            this.btn_ObjConst = new System.Windows.Forms.Button();
            this.txt_ObjConst = new System.Windows.Forms.TextBox();
            this.txt_BuildOrder = new System.Windows.Forms.TextBox();
            this.btn_BuildOrder = new System.Windows.Forms.Button();
            this.lstv_Components = new System.Windows.Forms.ListView();
            this.lstv_BuildableObjects = new System.Windows.Forms.ListView();
            this.lstv_BuildOrder = new System.Windows.Forms.ListView();
            this.btn_BeginConstruction = new System.Windows.Forms.Button();
            this.col_Components = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_Buildable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_BuildOrder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // rtxt_WidgFact
            // 
            this.rtxt_WidgFact.HideSelection = false;
            this.rtxt_WidgFact.Location = new System.Drawing.Point(12, 272);
            this.rtxt_WidgFact.Name = "rtxt_WidgFact";
            this.rtxt_WidgFact.Size = new System.Drawing.Size(612, 108);
            this.rtxt_WidgFact.TabIndex = 0;
            this.rtxt_WidgFact.Text = "";
            // 
            // btn_ObjConst
            // 
            this.btn_ObjConst.Location = new System.Drawing.Point(254, 10);
            this.btn_ObjConst.Name = "btn_ObjConst";
            this.btn_ObjConst.Size = new System.Drawing.Size(143, 23);
            this.btn_ObjConst.TabIndex = 1;
            this.btn_ObjConst.Text = "Widget Construction File...";
            this.btn_ObjConst.UseVisualStyleBackColor = true;
            this.btn_ObjConst.Click += new System.EventHandler(this.btn_ObjConst_Click);
            // 
            // txt_ObjConst
            // 
            this.txt_ObjConst.Location = new System.Drawing.Point(12, 12);
            this.txt_ObjConst.Name = "txt_ObjConst";
            this.txt_ObjConst.Size = new System.Drawing.Size(236, 20);
            this.txt_ObjConst.TabIndex = 2;
            // 
            // txt_BuildOrder
            // 
            this.txt_BuildOrder.Location = new System.Drawing.Point(12, 38);
            this.txt_BuildOrder.Name = "txt_BuildOrder";
            this.txt_BuildOrder.Size = new System.Drawing.Size(236, 20);
            this.txt_BuildOrder.TabIndex = 3;
            // 
            // btn_BuildOrder
            // 
            this.btn_BuildOrder.Location = new System.Drawing.Point(254, 36);
            this.btn_BuildOrder.Name = "btn_BuildOrder";
            this.btn_BuildOrder.Size = new System.Drawing.Size(143, 23);
            this.btn_BuildOrder.TabIndex = 4;
            this.btn_BuildOrder.Text = "Build Order File...";
            this.btn_BuildOrder.UseVisualStyleBackColor = true;
            this.btn_BuildOrder.Click += new System.EventHandler(this.btn_BuildOrder_Click);
            // 
            // lstv_Components
            // 
            this.lstv_Components.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_Components});
            this.lstv_Components.FullRowSelect = true;
            this.lstv_Components.Location = new System.Drawing.Point(12, 64);
            this.lstv_Components.MultiSelect = false;
            this.lstv_Components.Name = "lstv_Components";
            this.lstv_Components.Size = new System.Drawing.Size(200, 200);
            this.lstv_Components.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstv_Components.TabIndex = 5;
            this.lstv_Components.UseCompatibleStateImageBehavior = false;
            this.lstv_Components.View = System.Windows.Forms.View.Details;
            // 
            // lstv_BuildableObjects
            // 
            this.lstv_BuildableObjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_Buildable});
            this.lstv_BuildableObjects.Location = new System.Drawing.Point(218, 64);
            this.lstv_BuildableObjects.MultiSelect = false;
            this.lstv_BuildableObjects.Name = "lstv_BuildableObjects";
            this.lstv_BuildableObjects.Size = new System.Drawing.Size(200, 200);
            this.lstv_BuildableObjects.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstv_BuildableObjects.TabIndex = 6;
            this.lstv_BuildableObjects.UseCompatibleStateImageBehavior = false;
            this.lstv_BuildableObjects.View = System.Windows.Forms.View.Details;
            // 
            // lstv_BuildOrder
            // 
            this.lstv_BuildOrder.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_BuildOrder});
            this.lstv_BuildOrder.FullRowSelect = true;
            this.lstv_BuildOrder.Location = new System.Drawing.Point(424, 64);
            this.lstv_BuildOrder.MultiSelect = false;
            this.lstv_BuildOrder.Name = "lstv_BuildOrder";
            this.lstv_BuildOrder.Size = new System.Drawing.Size(200, 200);
            this.lstv_BuildOrder.TabIndex = 7;
            this.lstv_BuildOrder.UseCompatibleStateImageBehavior = false;
            this.lstv_BuildOrder.View = System.Windows.Forms.View.Details;
            // 
            // btn_BeginConstruction
            // 
            this.btn_BeginConstruction.Location = new System.Drawing.Point(505, 9);
            this.btn_BeginConstruction.Name = "btn_BeginConstruction";
            this.btn_BeginConstruction.Size = new System.Drawing.Size(119, 49);
            this.btn_BeginConstruction.TabIndex = 8;
            this.btn_BeginConstruction.Text = "Begin Construction";
            this.btn_BeginConstruction.UseVisualStyleBackColor = true;
            this.btn_BeginConstruction.Click += new System.EventHandler(this.btn_BeginConstruction_Click);
            // 
            // col_Components
            // 
            this.col_Components.Text = "Components";
            this.col_Components.Width = 195;
            // 
            // col_Buildable
            // 
            this.col_Buildable.Text = "Buildable Objects";
            this.col_Buildable.Width = 195;
            // 
            // col_BuildOrder
            // 
            this.col_BuildOrder.Text = "Build Order";
            this.col_BuildOrder.Width = 195;
            // 
            // WidgetFactory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 392);
            this.Controls.Add(this.btn_BeginConstruction);
            this.Controls.Add(this.lstv_BuildOrder);
            this.Controls.Add(this.lstv_BuildableObjects);
            this.Controls.Add(this.lstv_Components);
            this.Controls.Add(this.btn_BuildOrder);
            this.Controls.Add(this.txt_BuildOrder);
            this.Controls.Add(this.txt_ObjConst);
            this.Controls.Add(this.btn_ObjConst);
            this.Controls.Add(this.rtxt_WidgFact);
            this.Name = "WidgetFactory";
            this.Text = "Widget Factory 1.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxt_WidgFact;
        private System.Windows.Forms.Button btn_ObjConst;
        private System.Windows.Forms.TextBox txt_ObjConst;
        private System.Windows.Forms.TextBox txt_BuildOrder;
        private System.Windows.Forms.Button btn_BuildOrder;
        private System.Windows.Forms.ListView lstv_Components;
        private System.Windows.Forms.ListView lstv_BuildableObjects;
        private System.Windows.Forms.ListView lstv_BuildOrder;
        private System.Windows.Forms.Button btn_BeginConstruction;
        public System.Windows.Forms.ColumnHeader col_Components;
        private System.Windows.Forms.ColumnHeader col_Buildable;
        private System.Windows.Forms.ColumnHeader col_BuildOrder;
    }
}

