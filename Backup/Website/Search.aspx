﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MaterPage.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Website.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <div class="tille_content">
                    Tìm kiếm sản phẩm</div>
                <div class="clear">
                </div>
                <div class="table_center">
                    <div>
                        <table class="tableCSS"  style="border-spacing: 0px;">
                            <tr>
                                <td class="t tborder_c">
                                    Từ khóa
                                </td>
                                <td class="i tborder_c">
                                    <asp:TextBox ID="txtKeySearch" CssClass="select_class l" runat="server"></asp:TextBox>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="t tborder_c">
                                    Loại sản phẩm
                                </td>
                                <td class="i tborder_c">
                                    <asp:DropDownList ID="ddlCatePros" DataTextField="Name" DataValueField="Id" CssClass="select_class n" runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="t tborder_c">
                                </td>
                                <td class="i tborder_c">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSumit" CssClass="quicksearch_button_link" Text="Tìm kiếm" runat="server"
                                                    ValidationGroup="check" OnClick="btnSumit_Click" />
                                            </td>
                                            <td style="padding-left: 10px;">
                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                                    <ProgressTemplate>
                                                        <asp:Image ID="imgLoad" ImageUrl="~/images/load.gif" runat="server" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="clear">
                </div>
                <div class="but_center">
                </div>
            </div>
            <!-- KẾT QUẢ TÌM KIẾM -->
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="ViewResultSearch" runat="server">
                    <div style="padding-top: 5px">
                        <div class="tille_content">
                            Kết quả
                            <asp:Label ID="lblResult" runat="server" Text=""></asp:Label></div>
                        <div class="clear">
                        </div>
                        <div class="table_center">
                            <div>
                                <div style="border-bottom: 1px solid #BFBFBF; margin: 5px; padding-bottom: 5px;">
                                    <div class="ConlectionPage">
                                        <asp:DataPager ID="DataPager2" PagedControlID="lvResultSearch" runat="server" OnPreRender="DataPager1_PreRender"
                                            PageSize="3">
                                            <Fields>
                                                <asp:NumericPagerField />
                                            </Fields>
                                        </asp:DataPager>
                                    </div>
                                </div>
                                <asp:ListView ID="lvResultSearch" runat="server" ItemPlaceholderID="ResultSearch">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="5" class="table_result" width="100%">
                                            <tr>
                                                <td style="width: 100px" valign="top">
                                                    <div class="image_laptop">
                                                        <a href='<%# Eval("Id","ProductDetails.aspx?id={0}") %>'>
                                                            <img alt='<%# Eval("Name") %>' src='<%# Eval("ImageHost","{0}") %>' /></a>
                                                    </div>
                                                </td>
                                                <td valign="top">
                                                    <div class="laptop_detail_text">
                                                        <div style="padding: 5px; margin: 5px" class="mytextarea">
                                                            <asp:HyperLink ID="HyperLink1" Text='<%# Eval("Name") %>' NavigateUrl='<%# Eval("Id","~/ProductDetails.aspx?id={0}") %>'
                                                                runat="server"></asp:HyperLink>
                                                        </div>
                                                        <table class="details" id="info_product" width="100%" cellpadding="0" cellspacing="0"
                                                            style="margin: 5px;">
                                                            <tr>
                                                                <td class="description">
                                                                    <b>Loại sản phẩm</b>
                                                                </td>
                                                                <td class="info">
                                                                    <asp:Label ID="lblCateName" runat="server" Text='<%# Eval("Category.Name") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="description">
                                                                    <b>Bảo hành</b>
                                                                </td>
                                                                <td class="info">
                                                                    <asp:Label ID="lblWarranty" runat="server" Text='<%# Eval("Warranty") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="description">
                                                                    <b>Trong kho</b>
                                                                </td>
                                                                <td class="info">
                                                                    <div class="quantity">
                                                                        <asp:Label ID="lblSoTon" runat="server" Text='<%# Eval("Inventory") %>'></asp:Label>
                                                                        sản phẩm
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="description">
                                                                    <b>Giá</b>
                                                                </td>
                                                                <td class="info">
                                                                    <div class="price">
                                                                        <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("StrPrice") %>'></asp:Label>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <div class="Sale">
                                                            <div class="buynow" onclick="location='<%# Eval("Id","ShoppingCart.aspx?ProductID={0}") %>'">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <div style="text-align: center; color: Red; font-weight: bold">
                                            Không tìm thấy sản phẩm cần tìm.</div>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <asp:PlaceHolder runat="server" ID="ResultSearch"></asp:PlaceHolder>
                                    </LayoutTemplate>
                                </asp:ListView>
                                <div class="ConlectionPage">
                                    <asp:DataPager ID="DataPager1" PagedControlID="lvResultSearch" runat="server" OnPreRender="DataPager1_PreRender"
                                        PageSize="3">
                                        <Fields>
                                            <asp:NumericPagerField />
                                        </Fields>
                                    </asp:DataPager>
                                </div>
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                        <div class="but_center">
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
