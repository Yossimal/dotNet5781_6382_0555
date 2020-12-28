using System;
using System.Collections.Generic;
using DALAPI.DAO;

namespace DS
{
    public class Data
    {
        public static Dictionary<Type, List<DAOBasic>> data;

        public static List<DAOType> GetConvertedList<DAOType>() where DAOType:DAOBasic
        {
            return ConvertDAOBasic<DAOType>(data[typeof(DAOType)]);
        }

        public static List<DAOBasic> GetList<DAOType>() where DAOType : DAOBasic
        {
            return data[typeof(DAOType)];
        }
        public static void SaveList<DAOType>(List<DAOBasic> toSave) where DAOType:DAOBasic
        {
            data[typeof(DAOType)] = toSave;
        }
        

        private static List<DAOType> ConvertDAOBasic<DAOType>(List<DAOBasic> convertFrom) where DAOType : DAOBasic
        {
            DAOType Converter(DAOBasic d) => d as DAOType;
            return convertFrom.ConvertAll(Converter);
        }
    }
}
