#region Licence Statment
// Copyright (c) Zi-Yu.com - All Rights Reserved
// http://midgard.zi-yu.com/
//
// The use and distribution terms for this software are covered by the
// LGPL (http://opensource.org/licenses/lgpl-license.php).
// By using this software in any fashion, you are agreeing to be bound by
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
#endregion

using System;
using System.Collections.Generic;
using System.Collections;

namespace Loki.DataRepresentation {

    /// <summary>
    /// Persists entities of type T
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
	public interface IPersistance<T> {

        #region Create/Update

        /// <summary>
        /// Creates an instance of the type T
        /// </summary>
        /// <returns>The created instance</returns>
		T Create();

        /// <summary>
        /// Persists an object
        /// </summary>
        /// <param name="t">The object</param>
		void Update(T t);

        #endregion Create/Update

        #region Delete

        /// <summary>
        /// Deletes an object from the persistance repository
        /// </summary>
        /// <param name="t">The target object</param>
		void Delete(T t);

        /// <summary>
        /// Deletes an object from the persistance repository
        /// </summary>
        /// <param name="id">The target object's Id</param>
        /// <returns>The number of objects deleted</returns>
        int Delete(int id);

        /// <summary>
        /// Deletes objects based on a HQL
        /// </summary>
        /// <param name="hql">The HQL</param>
        /// <returns>The number of objects deleted</returns>
        int Delete(string hql);

        /// <summary>
        /// Deletes objects based on a HQL
        /// </summary>
        /// <param name="hql">The HQL</param>
        /// <param name="args">The parameters to format the HQL</param>
        /// <returns>The number of deleted objects</returns>
        int Delete(string hql, params object[] args);

        /// <summary>
        /// Deletes all objects from the persistance repository
        /// </summary>
        /// <returns>The number of objects deleted</returns>
        int DeleteAll();

        #endregion Delete

        #region Select

        /// <summary>
        /// Selects all elements
        /// </summary>
        /// <returns>The list with all elements</returns>
		List<T> Select();

        /// <summary>
        /// Selects the element with the given Id
        /// </summary>
        /// <param name="id">The target's id</param>
        /// <returns>The associated object</returns>
        T Select(int id);

        /// <summary>
        /// Selects all elements with a given order
        /// </summary>
        /// <param name="orderByProperty">Field to order by</param>
        /// <param name="ascending">Order direction</param>
        /// <returns>The list with all elements</returns>
		List<T> Select( string orderByProperty, bool ascending );

        /// <summary>
        /// Selects all elements with a given order
        /// </summary>
        /// <param name="orderByProperties">Fields to order by</param>
        /// <param name="ascending">Order directions</param>
        /// <returns>The list with all elements</returns>
		List<T> Select( string[] orderByProperties, bool[] ascending);

        /// <summary>
        /// Selects a subset of all the elements
        /// </summary>
        /// <param name="start">The first element index</param>
        /// <param name="count">The number of elements</param>
        /// <returns>The list with the elements</returns>
		List<T> Select( int start, int count );

        /// <summary>
        /// Selects a subset of all the elements ordered by the given fields
        /// </summary>
        /// <param name="start">The first element index</param>
        /// <param name="count">The number of elements</param>
        /// <param name="orderByProperty">Field to order by</param>
        /// <param name="ascending">Order direction</param>
        /// <returns>The list with the elements</returns>
		List<T> Select( int start, int count, string orderByProperty, bool ascending );

        /// <summary>
        /// Selects a subset of all the elements ordered by the given fields
        /// </summary>
        /// <param name="start">The first element index</param>
        /// <param name="count">The number of elements</param>
        /// <param name="orderByProperties">Fields to order by</param>
        /// <param name="ascending">Order directions</param>
        /// <returns>The list with the elements</returns>
		List<T> Select( int start, int count, string[] orderByProperties, bool[] ascending);

        #endregion Select

        #region HQL Query

        /// <summary>
        /// Performs a HQL query
        /// </summary>
        /// <param name="hql">The HQL</param>
        /// <returns>The list</returns>
		IList Query( string hql );

        /// <summary>
        /// Performs a HQL query
        /// </summary>
        /// <param name="hql">The HQL</param>
        /// <param name="args">Arguments to format the HQL</param>
        /// <returns>The list</returns>
		IList Query( string hql, params object[] args );

        /// <summary>
        /// Performs a HQL query
        /// </summary>
        /// <param name="start">The first index</param>
        /// <param name="count">The number of elements to return</param>
        /// <param name="hql">The HQL</param>
        /// <param name="args">Parameters to format the HQL</param>
        /// <returns>The list</returns>
        IList Query( int start, int count, string hql, params object[] args );

        #endregion HQL Query

        #region HQL TypedQuery

        /// <summary>
        /// Performs a HQL query
        /// </summary>
        /// <param name="hql">The HQL</param>
        /// <returns>The list</returns>
        List<T> TypedQuery(string hql);

        /// <summary>
        /// Performs a HQL query
        /// </summary>
        /// <param name="hql">The HQL</param>
        /// <param name="args">Arguments to format the HQL</param>
        /// <returns>The list</returns>
        List<T> TypedQuery(string hql, params object[] args);

        /// <summary>
        /// Performs a HQL query
        /// </summary>
        /// <param name="start">The first index</param>
        /// <param name="count">The number of elements to return</param>
        /// <param name="hql">The HQL</param>
        /// <param name="args">Parameters to format the HQL</param>
        /// <returns>The list</returns>
        List<T> TypedQuery(int start, int count, string hql, params object[] args);

        #endregion HQL TypedQuery

        #region ExecuteScalar

        /// <summary>
        /// Executes HQL
        /// </summary>
        /// <param name="hql">The HQL</param>
        /// <returns>The result</returns>
        int ExecuteScalar(string hql);

        /// <summary>
        /// Executes HQL
        /// </summary>
        /// <param name="hql">The HQL</param>
        /// <param name="args">The parameters to format the HQL</param>
        /// <returns>The result</returns>
        int ExecuteScalar(string hql, params object[] args);

        #endregion ExecuteScalar

        #region Utilities

        /// <summary>
        /// Updates the persistance repository with the information stilll in memory
        /// </summary>
        void Flush();

        /// <summary>
        /// Gets the number of instanced persisted
        /// </summary>
        /// <returns>The instance count</returns>
		int GetCount();

        /// <summary>
        /// Gets a random element
        /// </summary>
        /// <returns>An element</returns>
		T GetRandom();

        #endregion Utilities

        #region Transactions

        /// <summary>
        /// Starts a transaction
        /// </summary>
		void StartTransaction();

        /// <summary>
        /// Commit transaction
        /// </summary>
		void CommitTransaction();

        /// <summary>
        /// Rollback transaction
        /// </summary>
		void RollbackTransaction();

        #endregion Transactions

    };
}
