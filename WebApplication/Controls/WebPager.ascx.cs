using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public delegate void GoToPage(int PageNum);

    public partial class WebPager : System.Web.UI.UserControl
    {
        private GoToPage _GoToPage = null;

        public void InitControl(GoToPage GP)
        {
            _GoToPage = GP;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int DataCount
        {
            get { return Int32.Parse(lbl_TotalCount.Text); }
            set { lbl_TotalCount.Text = value.ToString(); }
        }

        public int CurrPageNum
        {
            get { return Int32.Parse(lbl_CurrPage.Text); }
            set { lbl_CurrPage.Text = value.ToString(); }
        }

        public int TotalPageNum
        {
            get { return Int32.Parse(lbl_TotalPage.Text); }
            set { lbl_TotalPage.Text = value.ToString(); }
        }

        public int PageSize
        {
            get { return Int32.Parse(ddl_PageSize.SelectedValue); }
        }

        protected void btn_FirstPage_Click(object sender, EventArgs e)
        {
            _GoToPage(1);
        }

        protected void btn_PrevPage_Click(object sender, EventArgs e)
        {
            if (int.Parse(lbl_CurrPage.Text) > 1)
                _GoToPage(int.Parse(lbl_CurrPage.Text) - 1);
            else
                _GoToPage(1);
        }

        protected void btn_NextPage_Click(object sender, EventArgs e)
        {
            if (int.Parse(lbl_CurrPage.Text) < int.Parse(lbl_TotalPage.Text))
                _GoToPage(int.Parse(lbl_CurrPage.Text) + 1);
            else
                _GoToPage(int.Parse(lbl_TotalPage.Text));
        }

        protected void btn_LastPage_Click(object sender, EventArgs e)
        {
            _GoToPage(int.Parse(lbl_TotalPage.Text));
        }

        public void ControlButtonClick()
        {
            if (DataCount > 0)
            {
                btn_FirstPage.Enabled = true;
                btn_PrevPage.Enabled = true;
                btn_LastPage.Enabled = true;
                btn_NextPage.Enabled = true;
            }
            else
            {
                btn_FirstPage.Enabled = false;
                btn_PrevPage.Enabled = false;
                btn_LastPage.Enabled = false;
                btn_NextPage.Enabled = false;
            }

            if (CurrPageNum == 1)
            {
                btn_FirstPage.Enabled = false;
                btn_PrevPage.Enabled = false;
            }

            if (CurrPageNum == TotalPageNum)
            {
                btn_LastPage.Enabled = false;
                btn_NextPage.Enabled = false;
            }

            if (CurrPageNum == 0)
            {
                btn_FirstPage.Enabled = false;
                btn_PrevPage.Enabled = false;
                btn_LastPage.Enabled = false;
                btn_NextPage.Enabled = false;
            }
        }

        protected void ddl_PageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            _GoToPage(1);
        }

        protected void btn_GO_Click(object sender, EventArgs e)
        {
            int pageNum;
            if (int.TryParse(txt_PageNum.Text, out pageNum))
            {
                if (pageNum > TotalPageNum)
                    _GoToPage(TotalPageNum);
                else if (pageNum < 1)
                    _GoToPage(1);
                else
                    _GoToPage(pageNum);
            }
        }
    }
}