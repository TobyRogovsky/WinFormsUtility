using System.Data;
using System.DirectoryServices.ActiveDirectory;

namespace CPUWinFormsFramework
{
    public class WinFormsUtility
    {
        public static void SetListBinding(ComboBox lst, DataTable sourcedt, DataTable targetdt, string tablename)
        {
            lst.DataSource = sourcedt;
            lst.ValueMember = tablename + "ID";
            lst.DisplayMember = lst.Name.Substring(3);
            lst.DataBindings.Add("SelectedValue", targetdt, lst.ValueMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public static void SetControlBinding(Control ctrl, BindingSource bindsource)
        {
            string propertyname = "";
            string controlname = ctrl.Name.ToLower();
            string controltype = controlname.Substring(0, 3);
            string columname = controlname.Substring(3);

            switch (controltype)
            {
                case "txt":
                case "lbl":
                    propertyname = "Text";
                    break;
                case "dtp":
                    propertyname = "Value";
                    break;
            }



            if (propertyname != "" && columname != "")
            {
                ctrl.DataBindings.Add(propertyname, bindsource, columname, true, DataSourceUpdateMode.OnPropertyChanged);
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
            DoFormatGrid(grid, tablename);            

        }

        private static void DoFormatGrid(DataGridView grid, string tablename)
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.RowHeadersWidth = 25;
            string pkname = tablename + "ID";
            if (grid.Columns.Contains(pkname))
            {
                grid.Columns[pkname]!.Visible = false;
            }
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

    }
}
