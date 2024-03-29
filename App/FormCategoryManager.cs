﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Business.Entity;
using Business.IRepository;
using Business.Repository;

namespace App
{
    public partial class FormCategoryManager : Form
    {
        private ICategoryRepository _categoryRepository;
        private List<Category> _list;
        private List<Category> _listAdd;
        private List<Category> _listUpdate;
        private List<Category> _listDelete;
        int totalRowsAdd;
        int totalRowsUpdate;
        int totalRowsDelete;


        public FormCategoryManager()
        {
            InitializeComponent();

            //contructor field
            _categoryRepository = new CategoryRepository();
            _list = new List<Category>();
            _listAdd = new List<Category>();
            _listUpdate = new List<Category>();
            _listDelete = new List<Category>();
        }

        private void FormCategoryManager_Load(object sender, EventArgs e)
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
                Utilities.ShowMessageError("Error when run!!!");
            }

        }

        private void LoadTextBox(Category category)
        {
            txtCode.Text = category.Code;
            txtName.Text = category.Name;
        }

        private void LoadDataGridView()
        {
            dataGridViewCategory.DataSource = null;
            dataGridViewCategory.AutoGenerateColumns = false;
            dataGridViewCategory.DataSource = _list;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            lblAlert.Text = string.Empty;

            var item = new Category { Code = txtCode.Text, Name = txtName.Text };

            _listAdd.Add(item);
            _list.Add(item);
            LoadDataGridView();
        }

        private void dataGridViewCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblAlert.Text = string.Empty;
            if (e.RowIndex < 0) return;
            LoadTextBox(_list.ElementAt(e.RowIndex));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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

        private void btnDelete_Click(object sender, EventArgs e)
        {


            if (dataGridViewCategory.SelectedRows.Count < 0) return;

            var item = (Category)dataGridViewCategory.SelectedRows[0].DataBoundItem;

            var anser = MessageBox.Show(string.Format("You wanna delete {0} ?", item.Name), "Warning", MessageBoxButtons.YesNo,
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
                timerNoticeLable.Tick += new EventHandler(HideLableNotice);

                ClearAllList();
            }
            catch (Exception)
            {
                Utilities.ShowMessageError("Error. Can not Save");
            }


        }
       
        private void SetLableNotice()
        {
            var msgAdd = string.Format("Added: {0} categories", totalRowsAdd);
            var msgUpdate = string.Format("Updated: {0} categories", totalRowsUpdate);
            var msgDelete = string.Format("Deleted: {0} categories", totalRowsDelete);
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
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _categoryRepository.Refresh();
            ClearAllList();
            LoadTextBox(_list.ElementAt(0));
            lblAlert.Text = string.Empty;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Content textbox is strign empty => get all items
            if (txtSearch.Text.Trim().Length < 1)
                LoadDataGridView();

        }


        private void ClearAllList()
        {
            _listAdd.Clear();
            _listUpdate.Clear();
            _listDelete.Clear();
            _list = _categoryRepository.GetAll().ToList();
            LoadDataGridView();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dataGridViewCategory.DataSource = null;
            dataGridViewCategory.DataSource = _categoryRepository.Find(txtSearch.Text.Trim());
        }


    }
}
