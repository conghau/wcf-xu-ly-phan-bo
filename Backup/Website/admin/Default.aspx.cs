﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Website.Code;
using Website.Code.Entity;

namespace Website.admin
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var author = new Authentication();
            if (!author.IsLogin)
                Response.Redirect("~/admin/login.aspx");
            if (!IsPostBack)
            {
                LoadlstvCart();
                txtCurProfit.Text = string.Format("{0:0,0}", Repository.XPercent * 100);
            }
            popup1.Attributes.CssStyle.Add("display", "none");
        }

        private void LoadlstvCart()
        {
            var list = Code.ShoppingCart.GetAllOrder().ToList();
            foreach (var cart in list)
            {
                Utils.UpdateStatusOrder(cart.Id, cart.DeliveryNoteId);
            }
            list = Code.ShoppingCart.GetAllOrder().ToList();
            lstvCart.DataSource = list;
            lstvCart.DataBind();

            if (!list.Any()) return;
            ViewState["list"] = list.ToList();

        }



        protected void btnSendRequest_Click(object sender, EventArgs e)
        {
            var list = ViewState["list"] as List<Cart>;
            if (list == null)
            {
                return;
            }

            var success = 0;
            var send = 0;
            var error = string.Empty;
            foreach (var item in lstvCart.Items
                                     .Where(i => i.ItemType == ListViewItemType.DataItem))
            {
                var chb = item.FindControl("chbSelected") as CheckBox;
                if (chb == null || !chb.Checked) continue;

                var cart = list[item.DisplayIndex];
                if (cart.Status > 1)
                {
                    error = "Có phiếu đặt hàng đã gởi yêu cầu. ";
                    chb.Checked = false;
                    continue;
                }
                send++;
                if (Repository.SendShoppingCart(list[item.DisplayIndex]))
                    success++;
            }

            var msg = string.Format("Gởi <b>{0}</b> yêu cầu, thành công <b>{1}</b>", send, success);
            if (!string.IsNullOrEmpty(error))
                msg = string.Format("<b>{0}</b>{1}", error, msg);
            lblMsg.Text = msg;
            // Saliproit           
            lblclose.Text = "[X]";
            if (success > 0)
                LoadlstvCart();

        }

        protected void lstvCart_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (!e.CommandName.Equals("details")) return;
            var id = Convert.ToInt32(e.CommandArgument);
            var list = ViewState["list"] as List<Cart> ?? Code.ShoppingCart.GetAllOrder().ToList();
            var details = list.Find(c => c.Id == id);
            var cus = new List<Customer> { details.Customer };
            gvCustomer.DataSource = cus;
            gvCustomer.DataBind();
            if (details.Status == 3)
                details = Repository.GetDelivery(details.DeliveryNoteId);
            gvDetails.DataSource = details.OrdersDetails;
            gvDetails.DataBind();
            popup1.Attributes.CssStyle.Add("display", "block");
            
        }


      

        protected void lstDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem) return;

            var odrDetail = e.Item.DataItem as OrderDetail;
            if (odrDetail == null) return;
            
            var list = odrDetail.Stocks.Count > 0 ? odrDetail.Stocks : null;

            var lbl1 = e.Item.FindControl("lblRepo1") as Label;
            var lbl2 = e.Item.FindControl("lblRepo2") as Label;
            if (lbl1 == null || lbl2 == null) return;
            if(list==null) return;
            foreach (var stock in list)
            {
                if (stock.Id == 1)
                    lbl1.Text = string.Format("{0:0,0}", stock.Unit);
                else
                    lbl2.Text = string.Format("{0:0,0}", stock.Unit);
            }

        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType!=DataControlRowType.DataRow) return;
            var odrDetail = e.Row.DataItem as OrderDetail;
            if (odrDetail == null) return;

            var list = odrDetail.Stocks.Count > 0 ? odrDetail.Stocks : null;

            var lbl1 = e.Row.FindControl("lblRepo1") as Label;
            var lbl2 = e.Row.FindControl("lblRepo2") as Label;
            if (lbl1 == null || lbl2 == null) return;
            if (list == null) return;
            foreach (var stock in list)
            {
                if (stock.Id == 1)
                    lbl1.Text = string.Format("{0:0,0}", stock.Unit);
                else
                    lbl2.Text = string.Format("{0:0,0}", stock.Unit);
            }

        }


        protected void lstvCart_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem) return;

            var lbl = e.Item.FindControl("lblStatus") as Label;
            if (lbl == null) return;

            int status;
            if (!int.TryParse(lbl.Text, out status)) return;
            string str;
            switch (status)
            {
                case 1:
                    str = "Mới tạo";
                    break;
                case 2:
                    str = "Chờ xuất hàng";
                    break;
                case 3:
                    str = "Đã xuất hàng";
                    break;
                default:
                    str = "Không xác định";
                    break;
            }
            lbl.Text = str;
        }


        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadlstvCart();
            gvDetails.DataSource = null;
            gvDetails.DataBind();
        }

        protected void lbtnLogout_Click(object sender, EventArgs e)
        {
            var author = new Authentication();
            author.Logout();
            Response.Redirect("~/admin/login.aspx");
        }

        protected void bntChangeProfit_Click(object sender, EventArgs e)
        {
            double percent;
            if (double.TryParse(txtNewProfit.Text, out percent))
            {
                percent = percent / 100;
                Repository.XPercent = percent;
                Utils.SetPercent();
                lblMsgChange.Text = "Thay đổi lợi nhuận thành công";
                txtCurProfit.Text = string.Format("{0:0,0}", Repository.XPercent * 100);
                txtNewProfit.Text = string.Empty;
            }
            else
            {
                lblMsgChange.Text = "Có lỗi xảy ra";
            }

        }

        protected void lbtnManager_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(viewManager);
        }

        protected void lbtnChangeProfit_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(viewChangeProfit);
        }

        protected void hideMsg(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            // Saliproit            
            lblclose.Text = "";
        }

      


    }
}