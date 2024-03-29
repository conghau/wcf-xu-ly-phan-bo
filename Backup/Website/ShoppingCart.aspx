﻿<%@ Page Title="Giỏ hàng của bạn - iShop Bán hàng trực tuyến" Language="C#" MasterPageFile="~/MaterPage.Master" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs"
 Inherits="Website.ShoppingCart" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHGioHang" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Content" runat="server">
     <div>
        <div class="tille_content">
            Giỏ hàng của bạn</div>
        <div class="clear">
        </div>
        <div class="table_center">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="ListCart">
                        <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" Width="100%"
                            OnRowDeleting="gvCart_RowDeleting" DataKeyNames="ProductId" CellPadding="4" ForeColor="#333333"
                            GridLines="None">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField HeaderText="Loại">
                                    <ItemTemplate>
                                        <div class="CartLabel">
                                            <asp:HyperLink ID="HyperLink" runat="server" NavigateUrl='<%# Eval("CateId","~/Products.aspx?CateID={0}") %>'
                                                Text='<%# Bind("CateName") %>'></asp:HyperLink>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sản phẩm">
                                    <ItemTemplate>
                                        <div class="CartLabel mytextarea">
                                            <asp:Image ID="Image1" Width="60px" Height="80px" ImageUrl='<%# Bind("ProductImage") %>' runat="server"
                                                ImageAlign="Middle" />
                                            <asp:HyperLink ID="HyperLink1" NavigateUrl='<%# Eval("ProductId","~/ProductDetails.aspx?id={0}") %>'
                                                Text='<%# Bind("ProductName") %>' Width="160px" runat="server">HyperLink</asp:HyperLink>
                                            <asp:LinkButton ID="lbtDelete" runat="server" OnClientClick="return confirm('Bạn có chắc chắn muốn xóa sản phẩm này không?');"
                                                CausesValidation="false" CommandName="Delete"><img 
            src="images/icon_delete.gif"  alt=''  class="ImgBtnDeleteCartItem"  /></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Giá">
                                    <ItemTemplate>
                                        <div class="CartLabel Price">
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("StrPrice") %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Số Lượng">
                                    <ItemTemplate>
                                        <div class="CartLabel Number">
                                            <asp:TextBox ID="txtQuantity" Columns="5" runat="server" Text='<%# Eval("Unit") %>'
                                                MaxLength="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="RequiredFieldValidator"
                                                Text="*" ControlToValidate="txtQuantity"></asp:RequiredFieldValidator>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" ValidChars="0123456789"
                                                TargetControlID="txtQuantity">
                                            </cc1:FilteredTextBoxExtender>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Thành tiền">
                                    <ItemTemplate>
                                        <div class="Price price">
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("StrTotalPrice") %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        </asp:GridView>
                        <div class="Cart_Command">
                            <div class="btn">
                                <asp:Button ID="btnDelAll" runat="server" Text="Xóa hết" OnClick="btnDelAll_Click"
                                    OnClientClick="return confirm('Bạn có chắc chắn muốn xóa hết giỏ hàng không?');" />
                                <asp:Button ID="btnBuyMore" runat="server" Text="Mua thêm" OnClientClick="window.location='Default.aspx';return false;" />
                                <asp:Button ID="btnUpdateCart" runat="server" Text="Cập nhật" OnClick="btnUpdateCart_Click" />
                                <asp:Button ID="btnPayment" runat="server" Text="Thanh toán" OnClick="btnPayment_Click" />
                            </div>
                            <div class="txt">
                                <div class="to_txt">
                                    Tổng tiền:
                                </div>
                                <div class="Price price">
                                    <asp:Label ID="lblTotal" runat="server" Text="Label"></asp:Label></div>
                            </div>
                        </div>
                    </div>
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="ViewDangKyThongTin" runat="server">
                            <table class="tableCSS" style="border-spacing: 0px;">
                                <caption>
                                    <div class="mytextarea">
                                        Đăng ký thông tin khách hàng</div>
                                </caption>
                                <%--<tr>
                                    <td class="t tborder_c">
                                        Mã khách hàng
                                    </td>
                                    <td class="tborder_c text">
                                        <asp:Label ID="lblMaKhachHang" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>--%>
                                 <tr>
                                    <td class="t tborder_c">
                                        CMND
                                    </td>
                                    <td class="i tborder_c">
                                        <asp:TextBox ID="txtCMND" CssClass="n" runat="server"></asp:TextBox>&nbsp;
                                       <%-- <asp:RequiredFieldValidator ValidationGroup="check" ControlToValidate="txtCMND" SetFocusOnError="true"
                                            ID="RequiredFieldValidator5" runat="server">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="RegularExpressionValidator"
                                            ValidationExpression="(\d){9}" Text="CMND chưa xác thực" ControlToValidate="txtCMND"
                                            SetFocusOnError="True" ValidationGroup="check"></asp:RegularExpressionValidator>--%>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtCMND"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="t tborder_c">
                                        Họ Tên
                                    </td>
                                    <td class="i tborder_c">
                                        <asp:DropDownList ID="ddlGioiTinh" Height="22px" runat="server">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtHoTen" CssClass="n" runat="server"></asp:TextBox>                                        
                                        &nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator"
                                            ControlToValidate="txtHoTen" SetFocusOnError="True" ValidationGroup="check">Bạn chưa nhập tên</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="t tborder_c">
                                        Địa chỉ
                                    </td>
                                    <td class="i tborder_c">
                                        <asp:TextBox ID="txtDiaChi" CssClass="l" runat="server"></asp:TextBox>&nbsp;
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="RequiredFieldValidator"
                                            SetFocusOnError="True" ControlToValidate="txtDiaChi" ValidationGroup="check">*</asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="t tborder_c">
                                        Số điện thoại
                                    </td>
                                    <td class="i tborder_c">
                                        <asp:TextBox ID="txtSoDienThoai" CssClass="n" runat="server"></asp:TextBox>&nbsp;
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="check"
                                            runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtSoDienThoai"
                                            SetFocusOnError="True" Text="*"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="RegularExpressionValidator"
                                            ValidationExpression="(\d){7,11}" Text="Số điện thoại chưa đúng" ControlToValidate="txtSoDienThoai"
                                            SetFocusOnError="True" ValidationGroup="check"></asp:RegularExpressionValidator>--%>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtSoDienThoai"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="t tborder_c">
                                        Email
                                    </td>
                                    <td class="i tborder_c">
                                        <asp:TextBox ID="txtEmail" CssClass="n" runat="server"></asp:TextBox>&nbsp;
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="check"
                                            runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtEmail"
                                            SetFocusOnError="True">*</asp:RequiredFieldValidator>--%>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail"
                                            ErrorMessage="RegularExpressionValidator" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                            SetFocusOnError="True" ValidationGroup="check">Email chưa đúng</asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td class="t tborder_c">
                                    </td>
                                    <td class="i tborder_c">
                                        <asp:Button ID="btnSumit" CssClass="send_button_link" Text="Hoàn thành" runat="server"
                                            OnClick="btnSumit_Click" ValidationGroup="check" />
                                        <input id="btnReset" type="reset" value="Nhập lại" class="reset_button_link" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="ViewDangKyThanhCong" runat="server">
                            <div class="mytextarea TextCenter">
                                Chúc mừng bạn đã đăng ký thành công<br />
                                Trong vòng 12 giờ tới, nhân viên chúng tôi sẽ điện thoại cho bạn để hoàn tất xác
                                nhận đơn hàng.<br />
                            </div>
                            <div class="mytextarea" style="text-align: right;">
                                <a href="Default.aspx" style="color: Red; margin-right: 20px;">Về trang chủ</a></div>
                        </asp:View>
                        <asp:View ID="ViewDangKyThatBai" runat="server">
                            <div class="mytextarea TextCenter" style="color: Red;">
                                Lỗi khi đăng ký
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="clear">
        </div>
        <div class="but_center">
        </div>
    </div>
</asp:Content>