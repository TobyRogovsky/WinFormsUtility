using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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

        public static void SetControlBinding(Control ctrl, DataTable dt)
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
                ctrl.DataBindings.Add(propertyname, dt, columname, true, DataSourceUpdateMode.OnPropertyChanged);
            }

        }
        public static void FormatGridForSearchResult(DataGridView grid)
        {
            grid.AllowUserToAddRows = false;
            grid.ReadOnly = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
    }
}
