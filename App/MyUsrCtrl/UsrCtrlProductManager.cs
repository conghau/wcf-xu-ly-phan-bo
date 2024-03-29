﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Business.Entity;
using Business.IRepository;
using Business.Repository;

namespace App.MyUsrCtrl
{
    public partial class UsrCtrlProductManager : MyUserControl
    {
        private List<Product> _list;
        private IProductRepository _productRepository;

        private Product _selectedItem;
        public Product SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value == null) return;
                _selectedItem = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        private bool _addItem;
        public bool AddItem
        {
            get { return _addItem; }
            set
            {
                _addItem = value;
                OnPropertyChanged("AddProduct");
            }
        }

        public UsrCtrlProductManager()
        {
            InitializeComponent();

            _list = new List<Product>();
            _productRepository = new ProductRepository();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            IsRemove = true;
        }

        private void UsrCtrlProductManager_Load(object sender, EventArgs e)
        {
            try
            {
                _list = _productRepository.GetAll().ToList();
                LoadDataGridView();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Form product manager: " + ex.Message);
            }
        }

       

        private void LoadDataGridView(IList<Product> list = null)
        {
            if (list == null)
                list = _list;
            dataGridViewProduct.DataSource = null;
            dataGridViewProduct.AutoGenerateColumns = false;
            dataGridViewProduct.DataSource = list;

            //statistics 
            var totalProducts = list.Count;
            var totalQuantities = list.Sum(p => p.Inventory);
            lblAlert.Text = string.Format("Có {0} sản phẩm trong kho", totalProducts,
                                          totalQuantities);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        public void RefreshList()
        {
            txtSearch.Text = string.Empty;
            _list = _productRepository.GetAll().ToList();
            LoadDataGridView();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddItem = true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim().Length < 1)
                LoadDataGridView();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //dataGridViewProduct.DataSource = null;
            //dataGridViewProduct.DataSource = _productRepository.Find(txtSearch.Text.Trim()).ToList();
            var listResult = _productRepository.Find(txtSearch.Text.Trim()).ToList();
            LoadDataGridView(listResult);
        }

        private void dataGridViewProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            //get a product selected
            var item = (Product)dataGridViewProduct.Rows[e.RowIndex].DataBoundItem;
            switch (e.ColumnIndex)
            {
                case 3:
                    SelectedItem = item;
                    break;
                case 4:
                    //confirm delete product
                    if (Utilities.ShowQuestionMessage("Do you want to delete " + item.Name + " ?") == DialogResult.Yes)
                    {
                        try
                        {
                            var success = _productRepository.Delete(item.Id);
                            if (success)
                            {
                                _productRepository.Commit();
                                Utilities.ShowMessage("Delete success");
                                _list = _productRepository.GetAll().ToList();
                                LoadDataGridView();
                            }
                            else
                            {
                                Utilities.ShowMessageError("Delete failure");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Delete product: " + ex.Message);
                            Utilities.ShowMessageError("Error");
                        }
                    }
                    break;

            }
        }

        private void dataGridViewProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
