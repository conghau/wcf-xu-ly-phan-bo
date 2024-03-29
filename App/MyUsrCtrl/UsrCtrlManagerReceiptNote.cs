﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Business.Entity;
using Business.IRepository;
using Business.Repository;

namespace App.MyUsrCtrl
{
    public partial class UsrCtrlManagerReceiptNote : MyUserControl
    {
        private IReceiptNoteRepository _noteRepository;
        private IEnumerable<ReceiptNote> _list;
        private ReceiptNote _selectedItem;


        private DateTime _from;
        private DateTime _to;
        private int _status;
        public ReceiptNote SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public UsrCtrlManagerReceiptNote()
        {
            InitializeComponent();

            _noteRepository = new ReceiptNoteRepository();
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            IsRemove = true;
        }

        private void UsrCtrlManagerReceiptNote_Load(object sender, EventArgs e)
        {
            Reload();
            dtFrom.Value = DateTime.Now.AddYears(-1);
            dtTo.Value = DateTime.Now;
            _from = dtFrom.Value;
            _to = dtTo.Value;
            _status = -1;
        }

        public void Reload()
        {
            LoadCboStatus();
            //LoadLstReceiptNote();
            EmtySubItemlstDetails();
        }

        private void LoadCboStatus()
        {
            var list = new Dictionary<int, string> { { -1, "All" }, { 0, "Pending" }, { 1, "Checked" } }.ToList();
            cboStatus.DataSource = list;
        }

        private void EmtySubItemlstDetails(bool ishave = false)
        {

            foreach (var item in lstDetails.Items.Cast<ListViewItem>())
            {
                if (!ishave)
                    item.SubItems.Add(string.Empty);
                else
                    item.SubItems[1].Text = string.Empty;
            }

        }


        private void LoadLstReceiptNote(IEnumerable<ReceiptNote> list = null)
        {

            if (list == null)
            {
                _list = _noteRepository.GetAll();
                list = _list;
            }


            lstNote.Items.Clear();


            foreach (var note in list)
            {
                var item = new ListViewItem(note.StrStatus) { Tag = note.Id };
                item.SubItems.Add(String.Format("{0:dd/MM/yyyy HH:mm:ss}", note.Date));
                item.SubItems.Add(note.ReceiptNoteDetails
                                      .Count().ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(note.ReceiptNoteDetails
                                      .Sum(d => d.Unit).ToString(CultureInfo.InvariantCulture));
                lstNote.Items.Add(item);
            }
        }

        private void lstNote_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstNote.SelectedItems.Count < 1) return;

            var item = lstNote.SelectedItems[0];
            if (item == null) return;

            var noteId = (int)item.Tag;
            var note = _list.FirstOrDefault(n => n.Id == noteId);
            if (note == null) return;

            lstDetails.Tag = noteId;
            for (var i = 0; i < lstDetails.Items.Count; i++)
            {
                lstDetails.Items[i].SubItems[1].Text = item.SubItems[i].Text;
            }

            LoadLstNoteDetails(note.ReceiptNoteDetails);
        }

        private void LoadLstNoteDetails(IEnumerable<ReceiptNoteDetail> list)
        {
            if (list == null) return;
            lstNoteDetail.Items.Clear();
            foreach (var noteDetail in list)
            {
                var item = new ListViewItem(noteDetail.Product.Name);
                item.SubItems.Add(string.Format("{0:0,0}", noteDetail.Unit));
                item.SubItems.Add(string.Format("{0:0,0}", noteDetail.CostPrice));
                item.SubItems.Add(noteDetail.Repository == 1 ? "A" : "B");
                lstNoteDetail.Items.Add(item);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lstNote.Items.Clear();
            LoadLstReceiptNote();
            EmtySubItemlstDetails(true);
            lstNoteDetail.Items.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lstDetails.Tag == null)
            {
                SelectedItem = null;
                return;
            }
            var noteId = (int)lstDetails.Tag;
            var note = _list.FirstOrDefault(n => n.Id == noteId);
            if (note == null) return;

            SelectedItem = note.Status ? null : note;

        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            _status = (int)cboStatus.SelectedValue;
            FilterNote();

        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            _from = dtFrom.Value;
            FilterNote();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            _to = dtTo.Value;
            FilterNote();
        }

        private void FilterNote()
        {
            //var status = (int)cboStatus.SelectedValue;
            _list = _noteRepository.GetAll().ToList();
            switch (_status)
            {
                case 0:
                    _list = _list.Where(n => !n.Status).ToList();
                    break;
                case 1:
                    _list = _list.Where(n => n.Status).ToList();
                    break;
                default:
                    _list = _noteRepository.GetAll().ToList();
                    break;

            }

            _list = _list.Where(n =>
                                DateTime.Compare(n.Date, _from) >= 0 &&
                                DateTime.Compare(n.Date, _to) <= 0);
            LoadLstReceiptNote(_list);
        }

        private void lstDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
