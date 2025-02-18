using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HROne.DataAccess;
using HROne.Lib.Entities;
using System.Globalization;

public partial class EmployeeSearchControl : HROneWebControl
{

    private SearchBinding binding;
    private DBManager db = EEmpPersonalInfo.db;
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        binding = new SearchBinding(dbConn, db);
        //binding.add(new LikeSearchBinder(EmpNo, "EmpNo"));
        //binding.add(new LikeSearchBinder(EmpEngSurname, "EmpEngSurname"));
        //binding.add(new LikeSearchBinder(EmpEngOtherName, "EmpEngOtherName"));
        //binding.add(new LikeSearchBinder(EmpChiFullName, "EmpChiFullName"));
        //binding.add(new LikeSearchBinder(EmpAlias, "EmpAlias"));
        //binding.add(new DropDownVLSearchBinder(EmpGender, "EmpGender", Values.VLGender).setLocale(ci));
        //binding.add(new FieldDateRangeSearchBinder(JoinDateFrom, JoinDateTo, "EmpDateOfJoin").setUseCurDate(false));
        //binding.add(new DropDownVLSearchBinder(EmpStatus, "EmpStatus", EEmpPersonalInfo.VLEmpStatus));
        //binding.initValues("EmpStatus", null, EEmpPersonalInfo.VLEmpStatus, ci);
        binding.init(DecryptedRequest, null);


        if (!Page.IsPostBack)
        {

            PositionList.LoadListControl(dbConn, EPosition.VLPosition, true);
            RankList.LoadListControl(dbConn, ERank.VLRank, false);
            EmploymentTypeList.LoadListControl(dbConn, EEmploymentType.VLEmploymentType, true);
            StaffTypeList.LoadListControl(dbConn, EStaffType.VLStaffType, true);
            PayrollGroupList.LoadListControl(dbConn, EPayrollGroup.VLPayrollGroup, true);


            //selected = PayGroupID.SelectedValue;
            //WebFormUtils.loadValues(PayGroupID, EPayrollGroup.VLPayrollGroup, new DBFilter(), null, (string)selected, (string)"combobox.notselected");

            string selected = EmpGender.SelectedValue;
            WebFormUtils.loadValues(dbConn, EmpGender, Values.VLGender, new DBFilter(), ci, (string)selected, (string)"combobox.notselected");

        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //Page page = (Page)HttpContext.Current.Handler;
        //if (page != null)
        //{

        //}
        Page page = (Page)HttpContext.Current.Handler;
        if (page != null)
        {
            string toggleAdvancedOptionScript =
"        function toggleAdvancedOption()" +
"        {" +

"            var showAdvancedCtrl = document.getElementById('" + txtShowAdvanced.ClientID + "');" +
"            if (showAdvancedCtrl.value=='true')" +
"            {" +
"                showAdvancedCtrl.value='false';" +
"            }" +
"            else" +
"            {" +
"                showAdvancedCtrl.value='true';" +
"            }" +
"            toggleLayer('SearchEmployeeControl',showAdvancedCtrl.value=='true'); " +
"            toggleLayer('AdvancedSearchButton',showAdvancedCtrl.value!='true'); " +
"            toggleLayer('HideAdvancedSearchButton',showAdvancedCtrl.value=='true');" +
"        }";

            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "toggleAdvancedOption", toggleAdvancedOptionScript, true);
            string showAdvancedScript =
                        "          {" +
                        @"            toggleLayer('SearchEmployeeControl',document.getElementById('" + txtShowAdvanced.ClientID + "').value=='true'); " +
                        @"            toggleLayer('AdvancedSearchButton',document.getElementById('" + txtShowAdvanced.ClientID + "').value!='true'); " +
                        @"            toggleLayer('HideAdvancedSearchButton',document.getElementById('" + txtShowAdvanced.ClientID + "').value=='true');" +
                        @"        }";

            ScriptManager.RegisterStartupScript(page, page.GetType(), "showAdvance", showAdvancedScript, true);
        }
    }

    public string EmpStatusValue
    {
        set { EmpStatus.SelectedValue = value; }
    }


    protected void HierarchyLevel_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        EHierarchyLevel level = (EHierarchyLevel)e.Item.DataItem;
        DBFilter filter = new DBFilter();
        filter.add(new Match("HLevelID", level.HLevelID));
       
        DropDownList c = (DropDownList)e.Item.FindControl("HElementID");
        string selected = c.SelectedValue;
        WebFormUtils.loadValues(dbConn, c, EHierarchyElement.VLHierarchyElement, filter, ci, selected, "combobox.notselected");
        c.Attributes["HLevelID"] = level.HLevelID.ToString();


    }

    public DBFilter GetEmpInfoFilter()
    {
        //  Dummy Function. Will be removed
        return GetEmpInfoFilter(AppUtils.ServerDateTime().Date, AppUtils.ServerDateTime().Date);
    }
    public DBFilter GetEmpInfoFilter(DateTime FromDate, DateTime ToDate)
    {
        DBFilter filter = binding.createFilter();

        DateTime dtPeriodFr, dtPeriodTo;
        if (DateTime.TryParse(JoinDateFrom.Value, out dtPeriodFr))
            filter.add(new Match("ee.EmpDateOfJoin", ">=", dtPeriodFr));
        if (DateTime.TryParse(JoinDateTo.Value, out dtPeriodTo))
            filter.add(new Match("ee.EmpDateOfJoin", "<=", dtPeriodTo));

        //if (EmpStatus.SelectedValue.Equals("A") || EmpStatus.SelectedValue.Equals("T") || EmpStatus.SelectedValue.Equals("TERMINATED"))
        //    filter.add(new Match("EmpDateOfJoin", "<=", ToDate.Date));

        //filter.add(WebUtils.AddRankFilter(Session, "ee.EmpID", true));


        DateTime dtLastDateFr, dtLastDateTo;
        DBFilter empTerminationFilter = new DBFilter();
        if (DateTime.TryParse(LastDateFrom.Value, out dtLastDateFr))
            empTerminationFilter.add(new Match("searchet.EmpTermLastDate", ">=", dtLastDateFr));
        if (DateTime.TryParse(LastDateTo.Value, out dtLastDateTo))
            empTerminationFilter.add(new Match("searchet.EmpTermLastDate", "<=", dtLastDateTo));

        if (EmpStatus.SelectedValue.Equals("TERMINATED"))
            empTerminationFilter.add(new Match("searchet.EmpTermLastDate", "<", FromDate.Date));
        if (empTerminationFilter.terms().Count > 0 || EmpStatus.SelectedValue.Equals("T"))
        {
            empTerminationFilter.add(new MatchField("ee.EmpID", "searchet.EmpID"));
            filter.add(new Exists(EEmpTermination.db.dbclass.tableName + " searchet ", empTerminationFilter));
        }
        if (EmpStatus.SelectedValue.Equals("AT"))
        {
            DBFilter notExistsEmpTerminationFilter = new DBFilter();
            notExistsEmpTerminationFilter.add(new Match("searchnoet.EmpTermLastDate", "<", FromDate.Date));
            notExistsEmpTerminationFilter.add(new MatchField("ee.EmpID", "searchnoet.EmpID"));
            filter.add(new Exists(EEmpTermination.db.dbclass.tableName + " searchnoet ", notExistsEmpTerminationFilter, true));
        }
        if (EmpStatus.SelectedValue.Equals("A"))
        {
            DBFilter notExistsEmpTerminationFilter = new DBFilter();
            notExistsEmpTerminationFilter.add(new MatchField("ee.EmpID", "searchnoet.EmpID"));
            filter.add(new Exists(EEmpTermination.db.dbclass.tableName + " searchnoet ", notExistsEmpTerminationFilter, true));
        }
        DBFilter empPosFilter = new DBFilter();

        //{
        //    OR orPositionTerm = null;
        //    foreach (ListItem item in PositionList.Items)
        //        if (item.Selected)
        //        {
        //            if (orPositionTerm == null)
        //                orPositionTerm = new OR();
        //            orPositionTerm.add(new Match("searchepi.PositionID", item.Value));
        //        }
        //    if (orPositionTerm != null)
        //        empPosFilter.add(orPositionTerm);
        //}

        DBTerm positionDBTerm = CreateDBTermsFromListControl("searchepi.PositionID", EPosition.db.dbclass.tableName + " pos", "pos.PositionID", PositionList.ListControl);
        if (positionDBTerm != null)
            empPosFilter.add(positionDBTerm);

        DBTerm rankDBTerms = CreateDBTermsFromListControl("searchepi.RankID", ERank.db.dbclass.tableName + " rank", "rank.RankID", RankList.ListControl);
        if (rankDBTerms != null)
            empPosFilter.add(rankDBTerms);

        DBTerm employmentTypeDBTerms = CreateDBTermsFromListControl("searchepi.EmploymentTypeID", EEmploymentType.db.dbclass.tableName + " empType", "empType.EmploymentTypeID", EmploymentTypeList.ListControl);
        if (employmentTypeDBTerms != null)
            empPosFilter.add(employmentTypeDBTerms);

        DBTerm staffTypeDBTerms = CreateDBTermsFromListControl("searchepi.StaffTypeID", EStaffType.db.dbclass.tableName + " staffType", "staffType.StaffTypeID", StaffTypeList.ListControl);
        if (staffTypeDBTerms != null)
            empPosFilter.add(staffTypeDBTerms);

        DBTerm payrollGroupDBTerms = CreateDBTermsFromListControl("searchepi.PayGroupID", EPayrollGroup.db.dbclass.tableName + " payGroup", "payGroup.PayGroupID", PayrollGroupList.ListControl);
        if (payrollGroupDBTerms != null)
            empPosFilter.add(payrollGroupDBTerms);

        DBTerm companyAndHierarchyTerm = HierarchyCheckBoxList1.GetFilters("searchepi", "EmpPosID");
        if (companyAndHierarchyTerm != null)
            empPosFilter.add(companyAndHierarchyTerm);
        if (empPosFilter.terms().Count > 0)
        {
            empPosFilter.add(new MatchField("ee.EmpID", "searchepi.EmpID"));

            EEmpPositionInfo.DBFilterAddDateRange(empPosFilter, "searchepi", FromDate, ToDate);

            OR orEmpPosTerm = new OR();
            orEmpPosTerm.add(new Exists(EEmpPositionInfo.db.dbclass.tableName + " searchepi ", empPosFilter));
            filter.add(orEmpPosTerm);

        }


        //DBFilter empPosNotExistsFilter = new DBFilter();
        //empPosNotExistsFilter.add(new MatchField("ee.EmpID", "np.EmpID"));
        //orEmpPosTerm.add(new Exists(EEmpPositionInfo.db.dbclass.tableName + " np", empPosNotExistsFilter, true));


        return filter;
    }

    //public DataTable FilterEncryptedEmpInfoField(DataTable table)
    //{
    //    return FilterEncryptedEmpInfoField(table, null);
    //}

    public DataTable FilterEncryptedEmpInfoField(DataTable table, ListInfo info)
    {
        //binding.add(new LikeSearchBinder(EmpNo, "EmpNo"));
        //binding.add(new LikeSearchBinder(EmpEngSurname, "EmpEngSurname"));
        //binding.add(new LikeSearchBinder(EmpEngOtherName, "EmpEngOtherName"));
        //binding.add(new LikeSearchBinder(EmpChiFullName, "EmpChiFullName"));
        //binding.add(new LikeSearchBinder(EmpAlias, "EmpAlias"));
        //binding.add(new DropDownVLSearchBinder(EmpGender, "EmpGender", Values.VLGender).setLocale(ci));
        if (!string.IsNullOrEmpty(EmpNo.Text))
        {
            DBAESEncryptStringFieldAttribute.decode(table, "EmpNo");
            DataView view = new DataView(table);
            view.RowFilter = "EmpNo like '%" + EmpNo.Text.Trim().Replace("'", "''") + "%' ";
            table = view.ToTable();
        }
        if (!string.IsNullOrEmpty(EmpEngSurname.Text))
        {
            DBAESEncryptStringFieldAttribute.decode(table, "EmpEngSurname");
            DataView view = new DataView(table);
            view.RowFilter = "EmpEngSurname like '%" + EmpEngSurname.Text.Trim().Replace("'", "''") + "%' ";
            table = view.ToTable();
        }
        if (!string.IsNullOrEmpty(EmpEngOtherName.Text))
        {
            DBAESEncryptStringFieldAttribute.decode(table, "EmpEngOtherName");
            DataView view = new DataView(table);
            view.RowFilter = "EmpEngOtherName like '%" + EmpEngOtherName.Text.Trim().Replace("'", "''") + "%' ";
            table = view.ToTable();
        }
        if (!string.IsNullOrEmpty(EmpChiFullName.Text))
        {
            DBAESEncryptStringFieldAttribute.decode(table, "EmpChiFullName");
            DataView view = new DataView(table);
            view.RowFilter = "EmpChiFullName like '%" + EmpChiFullName.Text.Trim().Replace("'", "''") + "%' ";
            table = view.ToTable();
        }
        if (!string.IsNullOrEmpty(EmpAlias.Text))
        {
            DBAESEncryptStringFieldAttribute.decode(table, "EmpAlias");
            DataView view = new DataView(table);
            view.RowFilter = "EmpAlias like '%" + EmpAlias.Text.Trim().Replace("'", "''") + "%' ";
            table = view.ToTable();
        }
        if (!string.IsNullOrEmpty(EmpGender.SelectedValue))
        {
            DBAESEncryptStringFieldAttribute.decode(table, "EmpGender");
            DataView view = new DataView(table);
            view.RowFilter = "EmpGender = '" + EmpGender.SelectedValue.Replace("'", "''") + "' ";
            table = view.ToTable();
        }

        if (!string.IsNullOrEmpty(EmpHKID.Text))
        {
            DBAESEncryptStringFieldAttribute.decode(table, "EmpHKID");
            DataView view = new DataView(table);
            view.RowFilter = "EmpHKID like '%" + EmpHKID.Text.Trim().Replace("'", "''") + "%' ";
            table = view.ToTable();
        }
        if (!string.IsNullOrEmpty(EmpPassportNo.Text))
        {
            DBAESEncryptStringFieldAttribute.decode(table, "EmpPassportNo");
            DataView view = new DataView(table);
            view.RowFilter = "EmpPassportNo like '%" + EmpPassportNo.Text.Trim().Replace("'", "''") + "%' ";
            table = view.ToTable();
        }
        if (!string.IsNullOrEmpty(info.orderby))
            if (info.orderby.Equals("EmpEngFullName", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!table.Columns.Contains("EmpEngFullName"))
                {
                    table.Columns.Add("EmpEngFullName", typeof(string));
                    foreach (System.Data.DataRow row in table.Rows)
                    {
                        EEmpPersonalInfo empInfo = new EEmpPersonalInfo();
                        empInfo.EmpID = (int)row["EmpID"];
                        if (EEmpPersonalInfo.db.select(dbConn, empInfo))
                            row["EmpEngFullName"] = empInfo.EmpEngFullName;
                    }
                }
            }
        return WebUtils.DataTableSortingAndPaging(table, info);
    }


    public void Reset()
    {
        binding.clear();
        foreach (System.Web.UI.Control ctrl in Controls)
        {

            if (ctrl is TextBox)
                ((TextBox)ctrl).Text = string.Empty;
            else if (ctrl is HtmlInputText)
                ((HtmlInputText)ctrl).Value = string.Empty;
            else if (ctrl is WebDatePicker)
                ((WebDatePicker)ctrl).Value = string.Empty;
            else if (ctrl is DropDownList)
                ((DropDownList)ctrl).SelectedIndex = 0;
            else if (ctrl is ListControl)
                foreach (ListItem listItem in ((ListControl)ctrl).Items)
                    listItem.Selected = false;
            else if (ctrl is AdvancedCheckBoxList)
                ((AdvancedCheckBoxList)ctrl).Reset();
            else if (ctrl is HierarchyCheckBoxList)
                ((HierarchyCheckBoxList)ctrl).Reset();

        }
        if (AdditionElementControl != null)
            foreach (System.Web.UI.Control ctrl in AdditionElementControl.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                else if (ctrl is HtmlInputText)
                    ((HtmlInputText)ctrl).Value = string.Empty;
                else if (ctrl is WebDatePicker)
                    ((WebDatePicker)ctrl).Value = string.Empty;
                else if (ctrl is DropDownList)
                    ((DropDownList)ctrl).SelectedIndex = 0;
                else if (ctrl is ListBox)
                    foreach (ListItem positionItem in ((ListBox)ctrl).Items)
                        positionItem.Selected = false;
            }
    }

    public void Page_Init(object sender, EventArgs e)
    {
        EnsureChildControls();
    }

    public class MyTemplateContainer : Control, INamingContainer
    {
        public MyTemplateContainer(string ID)
        {
            this.ID = ID;
        }
    }


    private ITemplate _templates;
    [TemplateContainer(typeof(MyTemplateContainer))]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public ITemplate AdditionElements
    {
        get { return _templates; }
        set { _templates = value; }
    }

    protected override void CreateChildControls()
    {
        base.CreateChildControls();
        this.Placeholder1.Controls.Clear();
        if (this.AdditionElements != null)
        {
            MyTemplateContainer c = new MyTemplateContainer("content1");
            this.AdditionElements.InstantiateIn(c);
            Placeholder1.Controls.Add(c);
            //while (c.Controls.Count>0)
            //    this.Placeholder1.Controls.Add(c.Controls[0]);
        }
    }
    public Control AdditionElementControl
    {
        get { return Placeholder1.FindControl("content1"); }
    }

    protected DBTerm CreateDBTermsFromListControl(string FieldName, string ParentTableName, string ParentFieldName, ListControl listControl)
    {
        OR orTerm = null;
        string selectedValueList = string.Empty;
        foreach (ListItem item in listControl.Items)
            if (item.Selected)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    //if (orTerm == null)
                    //    orTerm = new OR();
                    //orTerm.add(new Match(FieldName, item.Value));
                    if (!string.IsNullOrEmpty(selectedValueList))
                        selectedValueList += ",";
                    selectedValueList += item.Value;
                }
                else if (listControl is CheckBoxList)
                {
                    if (orTerm == null)
                        orTerm = new OR();
                    DBFilter dbFilter = new DBFilter();
                    dbFilter.add(new MatchField(FieldName, ParentFieldName));
                    orTerm.add(new Exists(ParentTableName, dbFilter, true));
                }
            }
        if (!string.IsNullOrEmpty(selectedValueList))
        {
            if (orTerm == null)
                orTerm = new OR();
            orTerm.add(new IN(FieldName, selectedValueList, null));
        }
        return orTerm;
    }

    protected void LoadListControl(ListControl listControl, WFValueList ValueList, bool hasNotSelected)
    {
        WebFormUtils.loadValues(dbConn, listControl, ValueList, new DBFilter());
        if (!hasNotSelected)
            if (listControl.Items[0].Value.Equals(string.Empty))
                listControl.Items.RemoveAt(0);

    }
    //public Panel AdditionElements
    //{
    //    get
    //    {
    //        return this.Placeholder1;
    //    }
    //    set
    //    {
    //        Placeholder1. = value;
    //    }
    //}



}
