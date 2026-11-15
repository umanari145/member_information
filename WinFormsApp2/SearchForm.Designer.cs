namespace WinFormsApp2
{
    partial class SearchForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            search_button = new Button();
            name = new TextBox();
            name_label = new Label();
            dataGridView1 = new DataGridView();
            age_from = new NumericUpDown();
            age_to = new NumericUpDown();
            age_label = new Label();
            range_label = new Label();
            sex_label = new Label();
            man_radio_button = new RadioButton();
            woman_radio_button = new RadioButton();
            gender_group_panel = new Panel();
            menuStrip1 = new MenuStrip();
            member_menu = new ToolStripMenuItem();
            register_menu_item = new ToolStripMenuItem();
            bulk_member_regist_ToolStripMenuItem = new ToolStripMenuItem();
            all_check = new Button();
            all_uncheck = new Button();
            delete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)age_from).BeginInit();
            ((System.ComponentModel.ISupportInitialize)age_to).BeginInit();
            gender_group_panel.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // search_button
            // 
            search_button.Location = new Point(249, 555);
            search_button.Name = "search_button";
            search_button.Size = new Size(94, 29);
            search_button.TabIndex = 0;
            search_button.Text = "検索";
            search_button.UseVisualStyleBackColor = true;
            search_button.Click += search_button_Click;
            // 
            // name
            // 
            name.Location = new Point(92, 390);
            name.Name = "name";
            name.Size = new Size(156, 27);
            name.TabIndex = 1;
            // 
            // name_label
            // 
            name_label.AutoSize = true;
            name_label.Location = new Point(36, 393);
            name_label.Name = "name_label";
            name_label.Size = new Size(39, 20);
            name_label.TabIndex = 2;
            name_label.Text = "名前";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(36, 89);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(575, 248);
            dataGridView1.TabIndex = 3;
            // 
            // age_from
            // 
            age_from.Location = new Point(92, 440);
            age_from.Maximum = new decimal(new int[] { 150, 0, 0, 0 });
            age_from.Name = "age_from";
            age_from.Size = new Size(60, 27);
            age_from.TabIndex = 4;
            // 
            // age_to
            // 
            age_to.Location = new Point(190, 440);
            age_to.Maximum = new decimal(new int[] { 150, 0, 0, 0 });
            age_to.Name = "age_to";
            age_to.Size = new Size(60, 27);
            age_to.TabIndex = 5;
            age_to.Value = new decimal(new int[] { 150, 0, 0, 0 });
            // 
            // age_label
            // 
            age_label.AutoSize = true;
            age_label.Location = new Point(36, 447);
            age_label.Name = "age_label";
            age_label.Size = new Size(39, 20);
            age_label.TabIndex = 6;
            age_label.Text = "年齢";
            // 
            // range_label
            // 
            range_label.AutoSize = true;
            range_label.Location = new Point(158, 442);
            range_label.Name = "range_label";
            range_label.Size = new Size(24, 20);
            range_label.TabIndex = 7;
            range_label.Text = "～";
            // 
            // sex_label
            // 
            sex_label.AutoSize = true;
            sex_label.Location = new Point(36, 501);
            sex_label.Name = "sex_label";
            sex_label.Size = new Size(39, 20);
            sex_label.TabIndex = 8;
            sex_label.Text = "性別";
            // 
            // man_radio_button
            // 
            man_radio_button.AutoSize = true;
            man_radio_button.Location = new Point(0, 3);
            man_radio_button.Name = "man_radio_button";
            man_radio_button.Size = new Size(60, 24);
            man_radio_button.TabIndex = 11;
            man_radio_button.TabStop = true;
            man_radio_button.Text = "男性";
            man_radio_button.UseVisualStyleBackColor = true;
            // 
            // woman_radio_button
            // 
            woman_radio_button.AutoSize = true;
            woman_radio_button.Location = new Point(98, 3);
            woman_radio_button.Name = "woman_radio_button";
            woman_radio_button.Size = new Size(60, 24);
            woman_radio_button.TabIndex = 12;
            woman_radio_button.TabStop = true;
            woman_radio_button.Text = "女性";
            woman_radio_button.UseVisualStyleBackColor = true;
            // 
            // gender_group_panel
            // 
            gender_group_panel.Controls.Add(woman_radio_button);
            gender_group_panel.Controls.Add(man_radio_button);
            gender_group_panel.Location = new Point(92, 501);
            gender_group_panel.Name = "gender_group_panel";
            gender_group_panel.Size = new Size(250, 39);
            gender_group_panel.TabIndex = 13;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { member_menu });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(915, 28);
            menuStrip1.TabIndex = 14;
            menuStrip1.Text = "menuStrip1";
            // 
            // member_menu
            // 
            member_menu.DropDownItems.AddRange(new ToolStripItem[] { register_menu_item, bulk_member_regist_ToolStripMenuItem });
            member_menu.Name = "member_menu";
            member_menu.Size = new Size(83, 24);
            member_menu.Text = "会員管理";
            // 
            // register_menu_item
            // 
            register_menu_item.Name = "register_menu_item";
            register_menu_item.Size = new Size(182, 26);
            register_menu_item.Text = "会員登録";
            register_menu_item.Click += register_menu_item_Click;
            // 
            // bulk_member_regist_ToolStripMenuItem
            // 
            bulk_member_regist_ToolStripMenuItem.Name = "bulk_member_regist_ToolStripMenuItem";
            bulk_member_regist_ToolStripMenuItem.Size = new Size(182, 26);
            bulk_member_regist_ToolStripMenuItem.Text = "一括会員登録";
            bulk_member_regist_ToolStripMenuItem.Click += bulk_member_regist_ToolStripMenuItem_Click;
            // 
            // all_check
            // 
            all_check.Location = new Point(36, 49);
            all_check.Name = "all_check";
            all_check.Size = new Size(94, 29);
            all_check.TabIndex = 15;
            all_check.Text = "全選択";
            all_check.UseVisualStyleBackColor = true;
            all_check.Click += all_check_Click;
            // 
            // all_uncheck
            // 
            all_uncheck.Location = new Point(136, 49);
            all_uncheck.Name = "all_uncheck";
            all_uncheck.Size = new Size(94, 29);
            all_uncheck.TabIndex = 16;
            all_uncheck.Text = "全解除";
            all_uncheck.UseVisualStyleBackColor = true;
            all_uncheck.Click += all_uncheck_Click;
            // 
            // delete
            // 
            delete.Location = new Point(36, 349);
            delete.Name = "delete";
            delete.Size = new Size(86, 26);
            delete.TabIndex = 17;
            delete.Text = "削除";
            delete.UseVisualStyleBackColor = true;
            delete.Click += delete_Click;
            // 
            // SearchForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(915, 622);
            Controls.Add(delete);
            Controls.Add(all_uncheck);
            Controls.Add(all_check);
            Controls.Add(gender_group_panel);
            Controls.Add(sex_label);
            Controls.Add(range_label);
            Controls.Add(age_label);
            Controls.Add(age_to);
            Controls.Add(age_from);
            Controls.Add(dataGridView1);
            Controls.Add(name_label);
            Controls.Add(name);
            Controls.Add(search_button);
            Controls.Add(menuStrip1);
            Name = "SearchForm";
            Text = "SearchForm";
            Load += SearchForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)age_from).EndInit();
            ((System.ComponentModel.ISupportInitialize)age_to).EndInit();
            gender_group_panel.ResumeLayout(false);
            gender_group_panel.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button search_button;
        private TextBox name;
        private Label name_label;
        private Label sex_label;
        private DataGridView dataGridView1;
        private NumericUpDown age_from;
        private NumericUpDown age_to;
        private Label age_label;
        private Label range_label;
        private RadioButton man_radio_button;
        private RadioButton woman_radio_button;
        private Panel gender_group_panel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem member_menu;
        private ToolStripMenuItem register_menu_item;
        private ToolStripMenuItem bulk_member_regist_ToolStripMenuItem;
        private Button all_check;
        private Button all_uncheck;
        private Button delete;
    }
}
