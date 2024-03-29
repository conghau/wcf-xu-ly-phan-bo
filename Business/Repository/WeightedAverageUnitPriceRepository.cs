﻿

// File name: WeightedAverageUnitPriceRepository.cs
// Solution: iShopSolution
// Project: Business


using System;
using System.Diagnostics;
using System.Linq;
using Business.Database;
using Business.Entity;
using Business.IRepository;

namespace Business.Repository
{
    public class WeightedAverageUnitPriceRepository : IWeightedAverageUnitPriceRepository
    {

        private shopDataContext _data = ShopData.DataContext;


        //private IMyRepository _myRepository = new MyRepository();
        //private IWeightedAverageUnitPriceRepository _wAUPRepository = new WeightedAverageUnitPriceRepository();
        #region Implementation of IWeightedAverageUnitPriceRepository

        public decimal GetPrice(int proId)
        {
            try
            {
                var price = _data.GiaBinhQuans
                                        .Where(p => p.IDSanPham == proId && p.ThoiGian <= DateTime.Now)
                                        .OrderByDescending(p => p.ID)
                                        .FirstOrDefault();
                if (price != null)
                {
                    if (price.GiaBinQuan != null) return (decimal)price.GiaBinQuan;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Get Price: " + exception.Message);
            }
            return 0;

        }

        public decimal GetPriceByDate(int proId, DateTime date)
        {
            try
            {
                var price = _data.GiaBinhQuans
                                        .Where(p => p.IDSanPham == proId && p.ThoiGian <= date)
                                        .OrderByDescending(p => p.ID)
                                        .FirstOrDefault();
                if (price != null)
                {
                    if (price.GiaBinQuan != null) return (decimal)price.GiaBinQuan;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Get price by date: " + exception.Message);

            }
            return decimal.Zero;
        }

        /// <summary>
        /// Get info price of product at date
        /// </summary>
        /// <param name="proId">ID product</param>
        /// <param name="date">filter at date time</param>
        /// <returns>weight average unit price (lite) </returns>
        public WeightedAverageUnitPrice GetByDate(int proId, DateTime date)
        {
            var data = new shopDataContext();
            var price = data.GiaBinhQuans
                .Where(p => p.IDSanPham == proId && p.ThoiGian <= date)
                .OrderByDescending(p => p.ID)
                .FirstOrDefault();
            return price == null ? null : price.ConvertToLiteAverUnitPrice();
        }

        public void Add(ReceiptNoteDetail noteDetail)
        {

            var _myRepository = new MyRepository();
            var _wAUPRepository = new WeightedAverageUnitPriceRepository();

            // update weigthed average unit price 
            var unitInRepo = _myRepository.GetUnitProductAllRepository(noteDetail.ProductId);
            var averPrice = _wAUPRepository.GetPrice(noteDetail.ProductId);

            decimal newprice;
            if (unitInRepo == 0 || averPrice == 0)
            {
                newprice = noteDetail.CostPrice;
            }
            else
            {
                newprice = ((unitInRepo * averPrice) + (noteDetail.Unit * noteDetail.CostPrice)) /
                           (unitInRepo + noteDetail.Unit);
            }

            var giabinhquan = new GiaBinhQuan
                                  {
                                      GiaBinQuan = newprice,
                                      ThoiGian = DateTime.Now,
                                      IDSanPham = noteDetail.ProductId
                                  };
            _data.GiaBinhQuans.InsertOnSubmit(giabinhquan);
            Submit();
        }

        public void Add(WeightedAverageUnitPrice averageUnitPrice)
        {
            var gia = averageUnitPrice.ConvertToGiaBinhQuan();
            _data.GiaBinhQuans.InsertOnSubmit(gia);
        }

        public WeightedAverageUnitPrice Find(int proId, DateTime date)
        {
            var result = _data.GiaBinhQuans
                .Where(g => g.IDSanPham == proId && g.ThoiGian <= date)
                .OrderByDescending(g => g.ID)
                .FirstOrDefault();
            return result.ConvertToAverUnitPrice();
        }

        public void Submit()
        {
            _data.SubmitChanges();
        }

        #endregion
    }
}