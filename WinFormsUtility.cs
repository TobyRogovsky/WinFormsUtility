using System.Data;


namespace CPUWinFormsFramework
{
    public class WinFormsUtility
    {
        public static void SetListBinding(ComboBox lst, DataTable sourcedt, DataTable? targetdt, string tablename)
        {
            lst.DataBindings.Clear();
            lst.DataSource = sourcedt;
            lst.ValueMember = tablename + "ID";
            lst.DisplayMember = lst.Name.Substring(3);

            if (targetdt != null)
            {
                lst.DataBindings.Add("SelectedValue", targetdt, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        public static void SetControlBinding(Control ctrl, BindingSource bindsource)
        {
            ctrl.DataBindings.Clear();

            string propertyname = "";
            string controlname = ctrl.Name.ToLower();
            string controltype = controlname.Substring(0, 3);
            string columnname = controlname.Substring(3);

            switch (controltype)
            {
                case "txt":
                case "lbl":
                    propertyname = "Text";
                    break;
                case "dtp":
                    propertyname = "Value";
                    break;
                case "chk":
                    propertyname = "Checked";
                    break;
            }

            if (propertyname != "" && columnname != "")
            {
                ctrl.DataBindings.Add(propertyname, bindsource, columnname, true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }
        public static void FormatGridForSearchResult(DataGridView grid, string tablename)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;            
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DoFormatGrid(grid, tablename);
        }
        

        public static void FormatGridForEdit(DataGridView grid, string tablename)
        {
            grid.EditMode = DataGridViewEditMode.EditOnEnter;
            DoFormatGrid(grid, tablename);           
        }

        private static void DoFormatGrid(DataGridView grid, string tablename)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.RowHeadersWidth = 25;
            foreach(DataGridViewColumn col in grid.Columns)
            {
                if (col.Name.EndsWith("ID") || col.Name.EndsWith("Id"))
                {
                    col.Visible = false;
                }
            }
            string pkname = tablename + "ID";
            if (grid.Columns.Contains(pkname))
            {
                grid.Columns[pkname]!.Visible = false;
            }
        }

        public static int GetIdFromGrid(DataGridView grid, int rowindex, string colname)
        {
            int id = 0;
            if(rowindex < grid.Rows.Count && grid.Columns.Contains(colname) && grid.Rows[rowindex].Cells[colname].Value != DBNull.Value)
            {
                if (grid.Rows[rowindex].Cells[colname].Value is int)
                {
                    id = (int)grid.Rows[rowindex].Cells[colname].Value!;
                }
            }
            
            return id;
        }

        public static int GetIDFromComboBox(ComboBox lst)
        {
            int value = 0;
            if(lst.SelectedValue != null && lst.SelectedValue is int)
            {
                value = (int)lst.SelectedValue;
            }
            return value;
        }

        public static void AddComboBoxToGrid(DataGridView grid, DataTable dataSource, string tableName, string displayMember, int columnIndex = 0)
        {
            DataGridViewComboBoxColumn column = new();
            column.Name = tableName;
            column.DataSource = dataSource;
            column.DisplayMember = displayMember;
            column.ValueMember = tableName + "ID";
            column.DataPropertyName = column.ValueMember;
            column.HeaderText = tableName;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns.Insert(columnIndex, column);
        }

        public static void AddDeleteButtonToGrid(DataGridView grid, string deleteColumnName)
        {
            grid.Columns.Add(new DataGridViewButtonColumn()
            {
                Text = "X", HeaderText = "Delete",Name = deleteColumnName,UseColumnTextForButtonValue = true, Width = 60
            });
        }

        public static bool IsFormOpen(Type formtype, int pkvalue = 0)
        {
            bool exists = false;
            foreach (Form frm in Application.OpenForms)
            {
                int frmpkvalue = 0;
                if(frm.Tag != null && frm.Tag is int)
                {
                    frmpkvalue = (int)frm.Tag;
                }
                if (frm.GetType() == formtype && frmpkvalue == pkvalue)
                {
                    frm.Activate();
                    exists = true;
                    break;
                }
            }
            return exists;
        }

        public static void SetupNav(ToolStrip ts)
        {
            ts.Items.Clear();
            foreach (Form f in Application.OpenForms)
            {
                if (f.IsMdiContainer == false)
                {
                    ToolStripButton btn = new();
                    btn.Text = f.Text;
                    btn.Tag = f;
                    btn.Click += Btn_Click;
                    ts.Items.Add(btn);
                    ts.Items.Add(new ToolStripSeparator());
                }
            }
        }

        public static bool IsInteger(TextBox txt, string fieldname)
        {
            if (txt.Text != "" && !int.TryParse(txt.Text, out _))
            {
                MessageBox.Show(fieldname + " must be a number. Invalid entry.","Invalid Entry", MessageBoxButtons.OK,MessageBoxIcon.Warning);

                txt.Focus();
                return false;
            }

            return true;
        }

        public static bool IsDecimal(TextBox txt, string fieldname)
        {
            if (txt.Text != "" && !decimal.TryParse(txt.Text, out _))
            {
                MessageBox.Show(fieldname + " must be a number. Invalid entry.","Invalid Entry", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txt.Focus();
                return false;            }

            return true;
        }

        private static void Btn_Click(object? sender, EventArgs e)
        {
            if (sender != null && sender is ToolStripButton)
            {
                ToolStripButton btn = (ToolStripButton)sender;
                if (btn.Tag != null && btn.Tag is Form)
                {
                    ((Form)btn.Tag).Activate();
                }
            }
        }

        public static void HideColumns(DataGridView grid, params string[] colnames)
        {
            foreach(string colname in colnames)
            {
                if (grid.Columns.Contains(colname))
                {
                    grid.Columns[colname]!.Visible = false;
                }
            }
        }

    }
}
