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
//using perspectivemind.validation;
using HROne.Lib.Entities;
using HROne.Translation;

public partial class Attendance_Formula_View : HROneWebPage
{
    private const string FUNCTION_CODE = "ATT002";

    public Binding binding;
    public DBManager db = EAttendanceFormula.db;
    public EAttendanceFormula obj;
    public int CurID = -1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUtils.CheckAccess(Response, Session, FUNCTION_CODE, WebUtils.AccessLevel.ReadWrite))
            return;
        toolBar.FunctionCode = FUNCTION_CODE;

        

        binding = new Binding(dbConn, db);
        binding.add(AttendanceFormulaID);
        binding.add(AttendanceFormulaCode);
        binding.add(AttendanceFormulaDesc);
        binding.add(new LabelVLBinder(db, AttendanceFormulaType, EAttendanceFormula.VLFormulaType));
        binding.add(new LabelVLBinder(db, AttendanceFormulaPayFormID, EPayrollProrataFormula.VLPayrollProrataFormula));
        binding.add(AttendanceFormulaWorkHourPerDay);
        binding.add(new CheckBoxBinder(db, AttendanceFormulaIsUseRosterCodeForDefaultWorkHourPerDay));
        binding.add(AttendanceFormulaFixedRate);
        binding.add(new LabelVLBinder(db, AttendanceFormulaRoundingRule, Values.VLRoundingRuleWithNoRounding));
        binding.add(new LabelVLBinder(db, AttendanceFormulaDecimalPlace, Values.VL8DecimalPlace));
        binding.init(Request, Session);

        HROne.Common.WebUtility.WebControlsLocalization(this, this.Controls);

        if (!int.TryParse(DecryptedRequest["AttendanceFormulaID"], out CurID))
            CurID = -1;

        if (!Page.IsPostBack)
        {
            if (CurID > 0)
            {
                loadObject();
            }
            else
                toolBar.DeleteButton_Visible = false;
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {


    }
    protected bool loadObject()
    {
        obj = new EAttendanceFormula();
        bool isNew = WebFormWorkers.loadKeys(db, obj, DecryptedRequest);
        if (!db.select(dbConn, obj))
            return false;

        FixOTPanel.Visible = false;
        FormulaPanel.Visible = false;
        if (obj.AttendanceFormulaType.Equals(EAttendanceFormula.FORMULATYPE_CODE_BY_FORMULA))
        {
            FormulaPanel.Visible = true;
        }
        else if (obj.AttendanceFormulaType.Equals(EAttendanceFormula.FORMULATYPE_CODE_FIX_RATE))
        {
            FixOTPanel.Visible = true;
        }

        if (string.IsNullOrEmpty(obj.AttendanceFormulaRoundingRule))
            AttendanceFormulaDecimalPlace.Visible = false;
        else
            if (obj.AttendanceFormulaRoundingRule.Equals(Values.ROUNDING_RULE_NO_ROUND))
            {
                AttendanceFormulaDecimalPlace.Visible = false;
                AttendanceFormulaDecimalPlaceDesc.Visible = false;
            }
            else
            {
                AttendanceFormulaDecimalPlace.Visible = true;
                AttendanceFormulaDecimalPlaceDesc.Visible = true;
            }


        Hashtable values = new Hashtable();
        db.populate(obj, values);
        binding.toControl(values);

        return true;
    }


    protected void Delete_Click(object sender, EventArgs e)
    {
        PageErrors errors = PageErrors.getErrors(db, Page.Master);
        errors.clear();

        EAttendanceFormula c = new EAttendanceFormula();
        c.AttendanceFormulaID = CurID;
        if (EAttendanceFormula.db.select(dbConn, c))
        {
            DBFilter attendancePlanFilter = new DBFilter();
            attendancePlanFilter.add(new Match("AttendancePlanOTFormula", c.AttendanceFormulaID));
            attendancePlanFilter.add(new Match("AttendancePlanLateFormula", c.AttendanceFormulaID));
            ArrayList attendancePlanList = EAttendancePlan.db.select(dbConn, attendancePlanFilter);
            if (attendancePlanList.Count > 0)
            {
                errors.addError(string.Format(HROne.Translation.PageErrorMessage.ERROR_CODE_USED_BY_EMPLOYEE, new string[] { HROne.Common.WebUtility.GetLocalizedString("AttendanceFormula Code"), c.AttendanceFormulaCode }));
                foreach (EAttendancePlan attendancePlan in attendancePlanList)
                {
                    errors.addError("- " + attendancePlan.AttendancePlanCode + ", " + attendancePlan.AttendancePlanDesc);
                }
                errors.addError(HROne.Translation.PageErrorMessage.ERROR_ACTION_ABORT);
                return;

            }
            else
            {
                WebUtils.StartFunction(Session, FUNCTION_CODE);
                db.delete(dbConn, c);
                WebUtils.EndFunction(dbConn);
            }
        }
        HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_Formula_List.aspx");
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_Formula_Edit.aspx?AttendanceFormulaID=" + CurID);
    }
    protected void Back_Click(object sender, EventArgs e)
    {
            HROne.Common.WebUtility.RedirectURLwithEncryptedQueryString(Response, Session, "Attendance_Formula_List.aspx");
    }

}
