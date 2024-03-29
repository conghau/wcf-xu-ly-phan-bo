﻿

// File name: ICategoryRepository.cs
// Solution: iShopSolution
// Project: Business
// Time: 1:14 PM 22/04/2012

using System.Collections.Generic;
using System.Linq;
using Business.Entity;

namespace Business.IRepository
{
    public interface ICategoryRepository
    {

        /// <summary>
        /// Get all items category
        /// </summary>
        /// <returns>List items category</returns>
        IQueryable<Category> GetAll();

        /// <summary>
        /// Find all category by name cate name
        /// </summary>
        /// <param name="cateName">Category Name of find cate</param>
        /// <returns>List items category</returns>
        IQueryable<Category> Find(string cateName);

        /// <summary>
        /// Adds an entity in a pending insert state to this
        /// </summary>
        /// <param name="category">The category to be added.</param>
        /// <returns>TRUE if success</returns>
        bool Add(Category category);

        /// <summary>
        /// Edit category, just save database when call method Commit
        /// </summary>
        /// <param name="category">Category edit</param>
        /// <returns>TRUE if success</returns>
        bool Edit(Category category);

        /// <summary>
        /// Puts an entity from this table into a pending delete state 
        /// </summary>
        /// <param name="cateId">The category to be deleted</param>
        /// <returns>TRUE if success</returns>
        bool Delete(int cateId);

        /// <summary>
        /// Computes the set of modified objects to be inserted, updated,
        /// or deleted, and executes the appropriate commands to implement the changes to the database
        /// </summary>
        void Commit();

        void Refresh();
    }
}