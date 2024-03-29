﻿

// File name: IDeliveryNoteRepository.cs
// Solution: iShopSolution
// Project: Business
// Time: 10:52 PM 16/05/2012

using System.Collections.Generic;
using Business.Entity;

namespace Business.IRepository
{
    public interface IDeliveryNoteRepository
    {
        IEnumerable<DeliveryNote> GetAll();
        void Add(DeliveryNote note);
        DeliveryNote Find(int noteId);
        void UpdateStatus(int noteId);
        byte GetStatusDelivery(int noteId);
        void Submit();
    }
}