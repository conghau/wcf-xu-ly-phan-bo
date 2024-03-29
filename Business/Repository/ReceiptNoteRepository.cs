﻿
using System;
using System.Linq;
using Business.Database;
using Business.Entity;
using Business.IRepository;

namespace Business.Repository
{
    public class ReceiptNoteRepository : IReceiptNoteRepository
    {

        private shopDataContext _data = ShopData.DataContext;
        //private IReceiptNoteDetailRepository _noteDetailRepository = new ReceiptNoteDetailRepository();
        //private IWeightedAverageUnitPriceRepository _weightedAverageUnitPriceRepository = new WeightedAverageUnitPriceRepository();
        //private MyRepository _myRepository = new MyRepository();

        #region Implementation of IReceiptNoteRepository

        //public void Add(ReceiptNote receiptNote)
        //{


        //    var _noteDetailRepository = new ReceiptNoteDetailRepository();
        //    var _weightedAverageUnitPriceRepository = new WeightedAverageUnitPriceRepository();
        //    var _myRepository = new MyRepository();

        //    // save receipt note
        //    var phieuNhap = new PhieuNhapHang
        //                        {
        //                            ThoiGianNhap = DateTime.Now
        //                        };

        //    _data.PhieuNhapHangs.InsertOnSubmit(phieuNhap);
        //    Submit();

        //    // save list receipt note details
        //    foreach (var noteDetail in receiptNote.ReceiptNoteDetails)
        //    {
        //        // add one receipt note details
        //        noteDetail.ReceiptNoteId = phieuNhap.ID;
        //        _noteDetailRepository.Add(noteDetail);

        //        // add (update) new weighted average unit price of product 
        //        _weightedAverageUnitPriceRepository.Add(noteDetail);
        //        //Submit();
        //        // add (update) new repository
        //        var repo = new MyRepo
        //                       {
        //                           ReceiptDeliveryId = noteDetail.Id,
        //                           IsReceipt = true,
        //                           StockUnit = noteDetail.Unit,
        //                           ProductId = noteDetail.ProductId,
        //                           Date = DateTime.Now,
        //                           Repository = noteDetail.Repository
        //                       };
        //        _myRepository.Add(repo);
        //        //Submit();

        //    }

        //}

        public IQueryable<ReceiptNote> GetAll()
        {
            var data = new shopDataContext();
            return data.PhieuNhapHangs.Select(note => note.ConvertToReceiptNote());
        }

        public void AddPending(ReceiptNote receiptNote)
        {
            var exists = _data.PhieuNhapHangs.FirstOrDefault(p => !(bool)p.TrangThai
                                                                  && p.IDPhieuNhap == receiptNote.DeliveryNoteId);
            if (exists == null)
            {
                receiptNote.Status = false;
                Add(receiptNote);
            }
            else
            {
                receiptNote.Id = exists.ID;
            }

        }

        public void Add(ReceiptNote receiptNote)
        {
            var phieunhap = receiptNote.ConvertToPhieuNhapHang();
            _data.PhieuNhapHangs.InsertOnSubmit(phieunhap);
            Submit();
            receiptNote.Id = phieunhap.ID;
        }

        public void Update(ReceiptNote receiptNote)
        {
            var phieuNhap = _data.PhieuNhapHangs.FirstOrDefault(p => p.ID == receiptNote.Id);
            if (phieuNhap == null) return;

            phieuNhap.TrangThai = true;
            Submit();
            var _reciptDetails = new ReceiptNoteDetailRepository();

            foreach (var noteDetail in receiptNote.ReceiptNoteDetails)
            {
                _reciptDetails.Update(noteDetail);
            }

            // submit changed (save) to database
            _reciptDetails.Submit();

            var deliveryRepository = new DeliveryNoteRepository();
            var noteId = phieuNhap.IDPhieuNhap ?? 0;
            deliveryRepository.ProgressWaiting(noteId);
        }

        public void Submit()
        {
            _data.SubmitChanges();
        }

        #endregion
    }
}