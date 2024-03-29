﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Business.Entity;
using Business.IRepository;
using Business.Repository;

namespace App.MyUsrCtrl
{
    public partial class UsrCtrlCategoryManager : MyUserControl
    {
        private ICategoryRepository _categoryRepository;
        private List<Category> _list;
        private List<Category> _listAdd;
        private List<Category> _listUpdate;
        private List<Category> _listDelete;
        int totalRowsAdd;
        int totalRowsUpdate;
        int totalRowsDelete;

        public UsrCtrlCategoryManager()
        {
            InitializeComponent();
            //contructor field
            _categoryRepository = new CategoryRepository();
            _list = new List<Category>();
            _listAdd = new List<Category>();
            _listUpdate = new List<Category>();
            _listDelete = new List<Category>();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            IsRemove = true;
        }

        private void UsrCtrlCategoryManager_Load(object sender, EventArgs e)
        {
            try
            {
                _list = _categoryRepository.GetAll().ToList();
                LoadDataGridView();
                lblAlert.Text = string.Empty;
                LoadTextBox(_list.ElementAt(0));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Form category: " + ex.Message);
                Utilities.ShowMessageError("Lỗi khi chạy!!!");
            }
        }

        private void LoadTextBox(Category category)
        {
            txtCode.Text = category.Code;
            txtName.Text = category.Name;
        }

        private void ResetTextBox()
        {
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
        }
        private void LoadDataGridView()
        {
            dataGridViewCategory.DataSource = null;
            dataGridViewCategory.AutoGenerateColumns = false;
            dataGridViewCategory.DataSource = _list;
        }

        private void dataGridViewCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCode.Enabled = false;
            txtName.Enabled = false;
            btn_task.Enabled = false;
            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            lblAlert.Text = string.Empty;
            if (e.RowIndex < 0) return;
            var item = (Category)dataGridViewCategory.Rows[e.RowIndex].DataBoundItem;
            LoadTextBox(item);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtCode.Enabled = true;
            txtName.Enabled = true;
            txtName.Text = "";
            txtCode.Text = "";
            btnUpdate.Enabled = false;
            btn_task.Enabled = true;
            lblAlert.Text = string.Empty;
            //var item = new Category { Code = txtCode.Text, Name = txtName.Text };
            //_listAdd.Add(item);
            //_list.Add(item);
            //LoadDataGridView();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            txtCode.Enabled = true;
            txtName.Enabled = true;
            btnAdd.Enabled = false;
            btn_task.Enabled = true;
            //if (dataGridViewCategory.SelectedRows.Count < 0) return;
            ////identify item index in list
            //var index = dataGridViewCategory.SelectedRows[0].Index;

            ////get a item in list at index
            //var itemOriginal = _list[index];

            ////set data to item
            //itemOriginal.Code = txtCode.Text;
            //itemOriginal.Name = txtName.Text;

            //var indexItemInAdd = _listAdd.FindIndex(c => c.Code.Equals(itemOriginal.Code));
            //if (indexItemInAdd > -1)
            //{
            //    var itemInAdd = _listAdd[indexItemInAdd];
            //    itemInAdd.Code = itemOriginal.Code;
            //    itemInAdd.Name = itemOriginal.Name;

            //}
            //else
            //{
            //    //add in list update
            //    _listUpdate.Add(itemOriginal);
            //}


            ////reload data grid view
            //LoadDataGridView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (dataGridViewCategory.SelectedRows.Count < 0) return;

            var item = (Category)dataGridViewCategory.SelectedRows[0].DataBoundItem;

            var anser = MessageBox.Show(string.Format("Bạn có chắc chắn xóa {0} ?", item.Name), "Cảnh báo", MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question);


            if (anser == DialogResult.No) return;

            var indexItemInAdd = _listAdd.FindIndex(c => c.Code.Equals(item.Code));
            if (indexItemInAdd > -1)
            {
                _listAdd.RemoveAt(indexItemInAdd);
            }
            else
            {
                _listDelete.Add(item);
            }

            _list.Remove(item);
            LoadDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // add category to a pending state
                totalRowsAdd = _listAdd.Count(item => _categoryRepository.Add(item));
                totalRowsUpdate = _listUpdate.Count(item => _categoryRepository.Edit(item));
                totalRowsDelete = _listDelete.Count(item => _categoryRepository.Delete(item.Id));

                //implement changes to database
                _categoryRepository.Commit();
                //set into a label
                SetLableNotice();
                timerNoticeLable.Enabled = true;
                timerNoticeLable.Tick += HideLableNotice;

                ClearAllList();
            }
            catch (Exception)
            {
                Utilities.ShowMessageError("Không thể lưu");
            }

        }
        private void SetLableNotice()
        {
            var msgAdd = string.Format("Thêm : {0} nhóm sản phẩm", totalRowsAdd);
            var msgUpdate = string.Format("Sửa: {0} nhóm sản phẩm", totalRowsUpdate);
            var msgDelete = string.Format("Xóa: {0} nhóm sản phẩm", totalRowsDelete);
            lblAlert.Text = string.Format("{0}\n{1}\n{2}", msgAdd, msgUpdate, msgDelete);

            totalRowsAdd = 0;
            totalRowsUpdate = 0;
            totalRowsDelete = 0;

        }
        private void HideLableNotice(object sender, EventArgs e)
        {
            timerNoticeLable.Start();
            lblAlert.Text = "";
            timerNoticeLable.Stop();
        }
        private void ClearAllList()
        {
            _listAdd.Clear();
            _listUpdate.Clear();
            _listDelete.Clear();
            _list = _categoryRepository.GetAll().ToList();
            LoadDataGridView();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Content textbox is strign empty => get all items
            if (txtSearch.Text.Trim().Length < 1)
            {
                LoadDataGridView();
                LoadTextBox(_list.ElementAt(0));
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dataGridViewCategory.DataSource = null;
            //var listSearch = _categoryRepository.Find(txtSearch.Text.Trim()).ToList();
            var listSearch = _list.Where(c => c.Name.Contains(txtSearch.Text.Trim())).ToList();
            dataGridViewCategory.DataSource = listSearch;
            if (listSearch.Count > 0)
                LoadTextBox(listSearch.ElementAt(0));
            else
                ResetTextBox();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _categoryRepository.Refresh();
            ClearAllList();
            LoadTextBox(_list.ElementAt(0));
            lblAlert.Text = string.Empty;
        }

        private void btn_task_Click(object sender, EventArgs e)
        {
            if (btnAdd.Enabled == true)
            {
                var item = new Category { Code = txtCode.Text, Name = txtName.Text };
                _listAdd.Add(item);
                _list.Add(item);
                LoadDataGridView();
            }

            if (btnUpdate.Enabled == true)
            {
                if (dataGridViewCategory.SelectedRows.Count < 0) return;
                //identify item index in list
                var index = dataGridViewCategory.SelectedRows[0].Index;

                //get a item in list at index
                var itemOriginal = _list[index];

                //set data to item
                itemOriginal.Code = txtCode.Text;
                itemOriginal.Name = txtName.Text;

                var indexItemInAdd = _listAdd.FindIndex(c => c.Code.Equals(itemOriginal.Code));
                if (indexItemInAdd > -1)
                {
                    var itemInAdd = _listAdd[indexItemInAdd];
                    itemInAdd.Code = itemOriginal.Code;
                    itemInAdd.Name = itemOriginal.Name;

                }
                else
                {
                    //add in list update
                    _listUpdate.Add(itemOriginal);
                }


                //reload data grid view
                LoadDataGridView();
            }
        }
    }
}
