using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DALAPI.DAO;

namespace DALAPI
{
    /// <summary>
    /// Data Access Layer interface
    /// </summary>
    public interface IDAL
    {
        /// <summary>
        /// adds object to the database
        /// </summary>
        /// <param name="toAdd">the object to add</param>
        /// <returns>the added object primary key</returns>
        /// <remarks>the object must have the next properties -> IsDeleted:bool, IsRunningId:bool,Id:int</remarks>
        /// <exception cref="ItemAlreadyExistsException">there is already an item with the same primary key in the database</exception>
        int Add(object toAdd);
        /// <summary>
        /// add collection of objects to the database
        /// </summary>the collection to add
        /// <param name="toAdd"></param>
        /// <remarks>the object must have the next properties -> IsDeleted:bool, IsRunningId:bool,Id:int</remarks>
        /// <exception cref="ItemAlreadyExistsException">there is already an item with the same primary key in the database</exception>
        void AddCollection(IEnumerable<object> toAdd);
        /// <summary>
        /// removes object from the database
        /// </summary>
        /// <param name="toRemove">the object to remove (only need to include the sme primary key as the object in the database)</param>
        /// <returns>true, if the object has been removed. else, false</returns>
        /// <remarks>the object must have the next properties -> IsDeleted:bool, IsRunningId:bool,Id:int</remarks>
        bool Remove(object toRemove);
        /// <summary>
        /// update object in teh database
        /// </summary>
        /// <param name="toUpdate">the updated item (must have the same primary key as the item in the database)</param>
        /// <exception cref="ItemNotFoundException">cant find the item to update</exception>
        void Update(object toUpdate);
        /// <summary>
        /// get an object from the database according to his primary key
        /// </summary>
        /// <typeparam name="DAOType">the object type</typeparam>
        /// <param name="id">the object primary key value</param>
        /// <returns>the object with the given primary key and the given type</returns>
        DAOType GetById<DAOType>(int id) where DAOType:class,new();
        /// <summary>
        /// get all the objects with condition 
        /// </summary>
        /// <typeparam name="DAOType"></typeparam>
        /// <param name="condition"></param>
        /// <returns>all the object with that condition and the given type in the database</returns>
        /// <seealso cref="Enumerable.Where{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
        IEnumerable<DAOType> Where<DAOType>(Func<DAOType, bool> condition) where DAOType:class,new();
        /// <summary>
        /// get all the objects with the given type in the database
        /// </summary>
        /// <typeparam name="DAOType">the type to get</typeparam>
        /// <returns>the objects with the given type in the database</returns>
        IEnumerable<DAOType> All<DAOType>() where DAOType:class,new();

    }
}
