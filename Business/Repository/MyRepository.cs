﻿

// File name: Repository.cs
// Solution: iShopSolution
// Project: Business


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Business.Database;
using Business.Entity;
using Business.IRepository;

namespace Business.Repository
{
    public class MyRepository : IMyRepository
    {
        private shopDataContext _data = ShopData.DataContext;

        #region Implementation of IMyRepository

        public int GetUnitProductAllRepository(int proId)
        {
            try
            {
                int temp;
                var unit = GetUnitProductByRepository(proId, 1, out temp);
                unit += GetUnitProductByRepository(proId, 2, out temp);
                return unit;
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Get unit product all repository: " + exception.Message);
                return 0;
            }


        }

        public int GetUnitProductByRepository(int proId, int repo, out int Id)
        {
            var kho = _data.Khos.Where(k => k.IDSanPham == proId && k.Kho1 == repo && (bool)k.LaPhieuNhap).OrderByDescending(k => k.ID).FirstOrDefault();

            if (kho != null && kho.SoLuongTonKho != null)
            {
                Id = kho.ID;
                return (int)kho.SoLuongTonKho;
            }

            Id = 0;
            return 0;
        }

        public MyRepo GetNewRepoProduct(int proId, int repo)
        {
            var result = _data.Khos.Where(k => k.IDSanPham == proId && k.Kho1 == repo)
                .OrderByDescending(k => k.ID).FirstOrDefault();
            return result.ConvertToRepository();
        }

        public IEnumerable<MyRepo> GetNewRepoProduct(int proId)
        {
            var list = new List<MyRepo>();
            var repo1 = GetNewRepoProduct(proId, 1);
            var repo2 = GetNewRepoProduct(proId, 2);
            if (repo1.Id < repo2.Id)
            {
                list.Add(repo1);
                list.Add(repo2);
            }
            else
            {
                list.Add(repo2);
                list.Add(repo1);
            }
            return list;
        }

        public MyRepo GetFirstItemReceipt(int proId, int repo)
        {
            var list = from k in _data.Khos
                       where (bool)k.LaPhieuNhap &&
                             k.SoLuongConLanNhap > 0 &&
                             k.IDSanPham == proId &&
                             k.Kho1 == repo
                       select k;
            var current = list.FirstOrDefault();
            return current == null ? null : current.ConvertToRepository();
        }

        public List<MyRepo> GetFirstItemReceipt(int proId)
        {
            var list = new List<MyRepo>();
            var repo1 = GetFirstItemReceipt(proId, 1);
            var repo2 = GetFirstItemReceipt(proId, 2);
            if (repo1.Id < repo2.Id)
            {
                list.Add(repo1);
                list.Add(repo2);
            }
            else
            {
                list.Add(repo2);
                list.Add(repo1);
            }
            return list;
        }

        public IEnumerable<MyRepo> GetRepoProduct(int proId)
        {
            var list = from k in _data.Khos
                       where k.IDSanPham == proId && k.SoLuongConLanNhap > 0 && (bool)k.LaPhieuNhap
                       orderby k.ID
                       select k.ConvertToRepository();

            return list;
        }


        private MyRepo GetRepoProduct(int proId, byte repo, DateTime date)
        {
            var data = new shopDataContext();
            var result = data.Khos.Where(k => k.IDSanPham == proId && k.Kho1 == repo && k.ThoiGian <= date);
            if (!result.Any()) return new MyRepo { StockUnit = 0, Repository = repo };
            var last = result.OrderByDescending(k => k.ID).FirstOrDefault();
            return last.ConvertToRepository();
        }
        public IEnumerable<MyRepo> GetRepoProduct(int proId, DateTime date)
        {
            var list = new List<MyRepo> { GetRepoProduct(proId, 1, date), GetRepoProduct(proId, 2, date) };
            return list;
        }

        public IEnumerable<MyRepo> GetRepoByDeliveryNoteDetails(int noteDetailsId)
        {
            var data = new shopDataContext();
            var list = data.Khos.Where(k => !((bool)k.LaPhieuNhap) && k.IDNhapXuat == noteDetailsId);
            return !list.Any() ? null : list.Select(k => k.ConvertToRepository());
        }


        public bool ProgressRepoWhenAddDelivery(DeliveryNoteDetail noteDetail, int noteId)
        {
            try
            {
                var unit = noteDetail.Unit;
                var binhQuan = _data.GiaBinhQuans
                    .Where(g => g.IDSanPham == noteDetail.ProductId)
                    .OrderByDescending(g => g.ID).FirstOrDefault();
                if (binhQuan == null || binhQuan.SoLuong < unit)
                {
                    // create receipt note status pending with unit 2x notedetail.Unit

                    // add receipt note with status is pending
                    var receiptRepository = new ReceiptNoteRepository();
                    var receiptNote = new ReceiptNote { Date = DateTime.Now, Status = false, DeliveryNoteId = noteId };
                    receiptRepository.AddPending(receiptNote);
                    var unitInRepo = 0;
                    if (binhQuan != null)
                        unitInRepo = binhQuan.SoLuong ?? 0;
                    // with on receipt note, add receipt note detail with unit = 2 * noteDetail.Unit
                    var receiptNoteDetailsRepository = new ReceiptNoteDetailRepository();
                    var receiptNoteDetail = new ReceiptNoteDetail
                                                {
                                                    ProductId = noteDetail.ProductId,
                                                    ReceiptNoteId = receiptNote.Id,
                                                    Unit = 2 * (noteDetail.Unit - unitInRepo),
                                                    CostPrice = decimal.Zero,
                                                    Repository = 1
                                                };
                    receiptNoteDetailsRepository.Add(receiptNoteDetail);

                    return false;
                }
                //var stockUnitProduct = binhQuan.SoLuong;

                foreach (var repo in GetRepoProduct(noteDetail.ProductId))
                {


                    // update unit
                    unit = unit - repo.ReceiptUnit;
                    // update Receipt Unit this repository

                    Update(repo.Id, ((unit >= 0) ? 0 : unit * (-1)));

                    // add this repository history
                    repo.ReceiptDeliveryId = noteDetail.Id;
                    repo.IsReceipt = false;
                    int remain;
                    var stockUnit = GetNewRepoProduct(noteDetail.ProductId, repo.Repository).StockUnit;
                    if (unit > 0)
                    {
                        remain = stockUnit - repo.ReceiptUnit;
                        repo.ReceiptUnit = repo.ReceiptUnit;
                    }
                    else
                    {

                        remain = stockUnit - (repo.ReceiptUnit - unit * (-1));
                        repo.ReceiptUnit = (repo.ReceiptUnit - unit * (-1));
                    }

                    repo.StockUnit = remain < 0 ? 0 : remain;
                    repo.Date = DateTime.Now;
                    repo.Inventory = 0;

                    //repo.ReceiptUnit = 0;
                    Add(repo);

                    if (unit <= 0)
                        break;
                }


                var gia = new GiaBinhQuan
                              {
                                  GiaBinQuan = binhQuan.GiaBinQuan,
                                  SoLuong = binhQuan.SoLuong - noteDetail.Unit,
                                  ThoiGian = DateTime.Now,
                                  IDSanPham = binhQuan.IDSanPham
                              };
                _data.GiaBinhQuans.InsertOnSubmit(gia);
                Submit();
                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Progress add repo delivery: " + exception.Message);
                return false;
            }
        }

        public void Add(MyRepo repo)
        {
            if (repo.IsReceipt)
            {
                int oldRepoId;
                var oldUnit = GetUnitProductByRepository(repo.ProductId, repo.Repository, out oldRepoId);
                if (oldUnit > 0)
                {
                    repo.Inventory = oldRepoId;
                    repo.StockUnit += oldUnit;
                }
            }


            var kho = repo.ConvertToKho();
            _data.Khos.InsertOnSubmit(kho);
            Submit();
        }

        public void Update(int id, int remainUnit)
        {
            var kho = _data.Khos.FirstOrDefault(k => k.ID == id);
            if (kho == null) return;
            kho.SoLuongConLanNhap = remainUnit;
            Submit();
        }

        public void Submit()
        {
            _data.SubmitChanges();
        }

        #endregion
    }
}