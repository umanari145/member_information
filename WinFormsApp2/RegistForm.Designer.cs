namespace WinFormsApp2
{
    partial class RegistForm
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
            name_label = new Label();
            age_label = new Label();
            sex_label = new Label();
            sex_group = new FlowLayoutPanel();
            man_check = new RadioButton();
            woman_check = new RadioButton();
            age_input = new NumericUpDown();
            name_input = new TextBox();
            regist_button = new Button();
            sex_group.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)age_input).BeginInit();
            SuspendLayout();
            // 
            // name_label
            // 
            name_label.AutoSize = true;
            name_label.Location = new Point(46, 75);
            name_label.Name = "name_label";
            name_label.Size = new Size(39, 20);
            name_label.TabIndex = 0;
            name_label.Text = "名前";
            // 
            // age_label
            // 
            age_label.AutoSize = true;
            age_label.Location = new Point(46, 153);
            age_label.Name = "age_label";
            age_label.Size = new Size(39, 20);
            age_label.TabIndex = 1;
            age_label.Text = "年齢";
            // 
            // sex_label
            // 
            sex_label.AutoSize = true;
            sex_label.Location = new Point(46, 237);
            sex_label.Name = "sex_label";
            sex_label.Size = new Size(39, 20);
            sex_label.TabIndex = 2;
            sex_label.Text = "性別";
            // 
            // sex_group
            // 
            sex_group.Controls.Add(man_check);
            sex_group.Controls.Add(woman_check);
            sex_group.Location = new Point(122, 237);
            sex_group.Name = "sex_group";
            sex_group.Size = new Size(355, 43);
            sex_group.TabIndex = 3;
            // 
            // man_check
            // 
            man_check.AutoSize = true;
            man_check.Location = new Point(3, 3);
            man_check.Name = "man_check";
            man_check.Size = new Size(60, 24);
            man_check.TabIndex = 0;
            man_check.TabStop = true;
            man_check.Text = "男性";
            man_check.UseVisualStyleBackColor = true;
            // 
            // woman_check
            // 
            woman_check.AutoSize = true;
            woman_check.Location = new Point(69, 3);
            woman_check.Name = "woman_check";
            woman_check.Size = new Size(60, 24);
            woman_check.TabIndex = 1;
            woman_check.TabStop = true;
            woman_check.Text = "女性";
            woman_check.UseVisualStyleBackColor = true;
            // 
            // age_input
            // 
            age_input.Location = new Point(125, 151);
            age_input.Name = "age_input";
            age_input.Size = new Size(92, 27);
            age_input.TabIndex = 4;
            // 
            // name_input
            // 
            name_input.Location = new Point(122, 72);
            name_input.Name = "name_input";
            name_input.Size = new Size(153, 27);
            name_input.TabIndex = 5;
            // 
            // regist_button
            // 
            regist_button.Location = new Point(46, 313);
            regist_button.Name = "regist_button";
            regist_button.Size = new Size(126, 33);
            regist_button.TabIndex = 6;
            regist_button.Text = "登録";
            regist_button.UseVisualStyleBackColor = true;
            regist_button.Click += regist_button_Click;
            // 
            // RegistForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 380);
            Controls.Add(regist_button);
            Controls.Add(name_input);
            Controls.Add(age_input);
            Controls.Add(sex_group);
            Controls.Add(sex_label);
            Controls.Add(age_label);
            Controls.Add(name_label);
            Name = "RegistForm";
            Text = "新規登録";
            Load += RegistForm_Load;
            sex_group.ResumeLayout(false);
            sex_group.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)age_input).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label name_label;
        private Label age_label;
        private Label sex_label;
        private FlowLayoutPanel sex_group;
        private RadioButton man_check;
        private RadioButton woman_check;
        private NumericUpDown age_input;
        private TextBox name_input;
        private Button regist_button;
    }
}