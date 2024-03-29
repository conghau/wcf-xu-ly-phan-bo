﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Manager.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="Website.admin._default" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.30930.28736, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .center-main-cont
        {
            margin: 0px auto;
            padding: 20px;
            background: white;
            border-radius: 5px;
            box-shadow: 0px 0px 5px #aaa;
            -moz-box-shadow: 0 0 5px #aaa;
            -webkit-box-shadow: 0 0 5px #aaa;
            width: 1000px;
            height: 100%;
            display: block;
            min-height: 450px;
        }
        .sidebar-cont
        {
            width: 200px;
            float: left;
        }
        .content-cont
        {
            padding: 10px;
            width: 760px;
            float: right;
            border-radius: 5px;
            border: 1px solid #CCC;
            filter: progid:DXImageTransform.Microsoft.Gradient(GradientType=0,StartColorStr=#fff0f0f0,EndColorStr=#ffe6e6e6);
            background-image: -moz-linear-gradient(top,#F0F0F0 0,#E6E6E6 100%);
            background-image: -ms-linear-gradient(top,#F0F0F0 0,#E6E6E6 100%);
            background-image: -o-linear-gradient(top,#F0F0F0 0,#E6E6E6 100%);
            background-image: -webkit-gradient(linear,left top,left bottom,color-stop(0,#F0F0F0),color-stop(100%,#E6E6E6));
            background-image: -webkit-linear-gradient(top,white 0%,#EEE 100%);
            background-image: linear-gradient(to top,white 0,#EEE 100%);
        }
        .clear
        {
            clear: both;
        }
        thead tr td
        {
            font-weight: bold;
        }
        .menuleft li
        {
            line-height: 25px;
        }
        .menuleft li a, .menuleft li a.visited
        {
            color: #007ED3;
            text-decoration: none;
        }
        .menuleft li a:hover, .menuleft li a.current138
        {
            color: Orange;
            text-decoration: underline;
        }
        div#thietlap
        {
            display: block;
        }
        #btn-cont
        {
            margin-top: 10px;
            padding: 5px 20px;
            text-align: right;
        }
        .content-cont input
        {
            color: #444;
            font-weight: bold;
            text-decoration: none;
            padding: 5px 6px;
        }
        div.popup-detail-cont
        {
            top: 150px;
            left: 200px;
            padding: 30px;
            background: white;
            border-radius: 10px;
            box-shadow: 0px 0px 20px #FFF;
            -moz-box-shadow: 0 0 20px #FFF;
            -webkit-box-shadow: 0 0 20px #FFF;
            width: 800px;
            background: white;
            position: fixed;
            z-index: 34;
            display: none;
        }
        fieldset
        {
            border-radius: 5px;
            padding: 10px;
            border: 1px solid #AAA;
            margin-bottom: 20px;
        }
        legend
        {
            font-weight: bold;
        }
        .style-first-td
        {
            padding: 2px 10px;
        }
        #show-msg-cont
        {
            margin: 10px 0px;
            color: white;
            font-weight: bold;
            line-height: 35px;
            padding: 0px 10px;
            border-radius: 5px;
            background-color: #4D7730;
            filter: progid:DXImageTransform.Microsoft.Gradient(GradientType=0,StartColorStr=#74a446,EndColorStr=#4d7730);
            background-image: -moz-linear-gradient(top,#74A446 0,#4D7730 45px);
            background-image: -ms-linear-gradient(top,#74A446 0,#4D7730 45px);
            background-image: -o-linear-gradient(top,#74A446 0,#4D7730 45px);
            background-image: -webkit-gradient(linear,left top,left bottom,color-stop(0,#74A446),color-stop(45px,#4D7730));
            background-image: -webkit-linear-gradient(top,#74A446 0,#4D7730 45px);
            background-image: linear-gradient(to bottom,#74A446 0,#4D7730 45px);
        }
        #show-msg-cont a, #show-msg-cont a.visited
        {
            color: #FFF;
            float: right;
            text-decoration: none;
        }
        #show-msg-cont a:hover
        {
            color: Orange;
        }
        #popup-closebtn
        {
            opacity:1;
            cursor: pointer;
            right: 160px;
            top: 100px;
            position: absolute;
            display: block;
            width: 50px;
            height: 50px;
            background: url('../images/button_delete_blue.png');
        }
        #popup-closebtn:hover {
        opacity:0.9;
        }
    </style>
    <script type="text/javascript">        
        function RefreshPage() {
            window.location.reload();
        }
        function showPopup() {
            $("#overlay-popup").show();
        }

        function checkedAll() {
            $("tbody INPUT[type='checkbox']").attr('checked', $('#selectall').is(':checked'));
            
            $("tbody INPUT[type='checkbox']").click(function () {
                var flag = true;
                $("tbody INPUT[type='checkbox']").each(function () {
                    if (this.checked == false)
                        flag = false;
                });
                $("#selectall").attr('checked', flag);

            });

        }

        jQuery(document).ready(function ($) {
            $("#overlay-popup").click(function () {
                $("#overlay-popup").fadeOut();
                $("#ContentPlaceHolder1_popup1").fadeOut();
                return false;
            });
            $("#popup-closebtn").click(function () {
                $("#overlay-popup").fadeOut();
                $("#ContentPlaceHolder1_popup1").fadeOut();        
            });           
            // select all checkbox
//            $('#selectall').click(function () {
//                $("tbody INPUT[type='checkbox']").attr('checked', $('#selectall').is(':checked'));
//            });
//            $("tbody INPUT[type='checkbox']").click(function () {
//                var flag = true;
//                $("tbody INPUT[type='checkbox']").each(function () {
//                    if (this.checked == false)
//                        flag = false;
//                });
//                $("#selectall").attr('checked', flag);

//            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrap">
        <div class="center-main-cont">
            <div class="sidebar-cont">
                <div>
                    <div class="tille">
                        Menu</div>
                    <div class="menuleft">
                        <ul>
                            <li>
                                <asp:LinkButton ID="lbtnManager" runat="server" OnClick="lbtnManager_Click">Quản lý đơn hàng</asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="lbtnChangeProfit" runat="server" OnClick="lbtnChangeProfit_Click">Thiết lập lợi nhuận</asp:LinkButton>
                            </li>
                            <li>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx">Về trang chủ</asp:HyperLink></li>
                            <li>
                                <asp:LinkButton ID="lbtnLogout" runat="server" OnClick="lbtnLogout_Click">Đăng xuất</asp:LinkButton></li>
                        </ul>
                    </div>
                    <div class="butleft">
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                        <asp:View ID="viewManager" runat="server">
                            <div class="content-cont" id="quanly">
                                <div class="main-cont">
                                    <div runat="server" id="popup1" class="popup-detail-cont">
                                        <!-- details customer -->
                                        <fieldset>
                                            <legend>Thông tin khách hàng</legend>
                                            <div class="ListCart">
                                                <asp:GridView ID="gvCustomer" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    GridLines="None">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="CMND">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCateName" runat="server" Text='<%# Bind("CodeId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Họ tên">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Địa chỉ">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("Address") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Số điện thoại">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("Phone") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Email">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <EmptyDataTemplate>
                                                        Chưa xuất hàng
                                                    </EmptyDataTemplate>
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                        <!-- details order -->
                                        <fieldset>
                                            <legend>Chi tiết đơn hàng</legend>
                                            <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                                GridLines="None" OnRowDataBound="gvDetails_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Loại sản phẩm">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCateName" runat="server" Text='<%# Bind("CateName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sản phẩm">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Giá">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("ProductPrice","{0:0,0 VND}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Số lượng">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Unit") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Kho A">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRepo1" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Kho B">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRepo2" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <EmptyDataTemplate>
                                                    Chưa xuất hàng
                                                </EmptyDataTemplate>
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#999999" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            </asp:GridView>
                                        </fieldset>
                                        
                                        
                                    </div>
                                    <!-- list order -->
                                    <div id="show-msg-cont">
                                        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                        <asp:LinkButton ID="lblclose" runat="server" OnClick="hideMsg"></asp:LinkButton>
                                    </div>
                                </div>
                                <div class="ListCart">
                                    <asp:ListView ID="lstvCart" runat="server" OnItemCommand="lstvCart_ItemCommand" OnItemDataBound="lstvCart_ItemDataBound">
                                        <LayoutTemplate>
                                            <table style="border-spacing: 0px; width: 100%">
                                                <thead>
                                                    <tr>
                                                        <td>
                                                            <input type="checkbox" id="selectall" name="selectall" onchange="checkedAll()" />
                                                        </td>
                                                        <td>
                                                            Khách Hàng
                                                        </td>
                                                        <td>
                                                            Thời gian
                                                        </td>
                                                        <td>
                                                            Tổng sản phẩm
                                                        </td>
                                                        <td>
                                                            Tổng số lượng
                                                        </td>
                                                        <td>
                                                            Trạng thái
                                                        </td>
                                                        <td>
                                                            Chi tiết
                                                        </td>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chbSelected" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("Customer.Name") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date","{0:dd/MM/yyyy HH:mm:ss}") %>'></asp:Label>
                                                </td>
                                                <td style="text-align: right;padding-right: 35px;">
                                                    <asp:Label ID="lblTotalProduct" runat="server" Text='<%# Eval("TotalProduct") %>'></asp:Label>
                                                </td>
                                                <td style="text-align: right;padding-right: 35px;">
                                                    <asp:Label ID="lblTotalUnit" runat="server" Text='<%# Eval("TotalUnit") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <div class="btnDetail-cont" style="text-align: center">
                                                        <asp:ImageButton ID="imgBtnDetails" runat="server" OnClientClick="showPopup();" ImageUrl="~/images/detail.png"
                                                            CommandArgument='<%# Eval("Id") %>' CommandName="details" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            Chưa có đơn đặt hàng.
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </div>
                                <div id="btn-cont">
                                    <input type="button" value="Làm mới" id="btnRefresh" name="btnRefresh" onclick="RefreshPage()" />
                                    <asp:Button ID="btnSendRequest" runat="server" Text="Gởi phiếu đặt hàng" OnClientClick="showMsg()"
                                        OnClick="btnSendRequest_Click" />
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            </div>
                            <div class="clear">
                            </div>
                        </asp:View>
                        <asp:View ID="viewChangeProfit" runat="server">
                            <div class="content-cont" id="thietlap">
                                <div class="main-cont">
                                    <table style="margin: 0 auto; width: 50%;">
                                        <tr>
                                            <td>
                                                Tỉ lệ lãi hiện tại:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCurProfit" runat="server" ReadOnly="False" Enabled="False"></asp:TextBox>
                                                %
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Tỉ lệ lãi mới:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNewProfit" runat="server"></asp:TextBox>
                                                %
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtNewProfit"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Button ID="bntChangeProfit" runat="server" Text="Thay đổi" OnClick="bntChangeProfit_Click" />
                                                <br />
                                                <asp:Label ID="lblMsgChange" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="overlay-popup" style="display: none; background-color: rgb(0, 0, 0); opacity: 0.6;
        position: fixed; top: 0px; left: 0px; z-index: 33; width: 100%; height: 1667px;"><span id="popup-closebtn"></span>
    </div>
</asp:Content>
