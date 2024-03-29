﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DALXML.XMLSchemas
{
    /// <summary>
    /// schema for the files in the metadata
    /// </summary>
    internal class MetadataTypeFile
    {
        public string TypeName { get; set; }
        public string TypeAssembly { get; set; }
        public string TypeNamespace { get; set; }
        public string FileName { get; set; }
        /// <summary>
        /// returns the assembly of the file object
        /// </summary>
        /// <returns>the type of teh file object as Key and the name of the object as Value</returns>
        public new KeyValuePair<Type, string> GetType()
        {
            Assembly asm = Assembly.Load(TypeAssembly);
            Type type = asm.GetType($"{this.TypeNamespace}.{this.TypeName}");
            return new KeyValuePair<Type, string>(type, this.FileName);
        }
        /// <summary>
        /// serialize the file data into XElement
        /// </summary>
        /// <returns></returns>
        public XElement Serialize()
        {
            return Serialize(this);
        }
        /// <summary>
        /// get MetadataFileType from the file pair
        /// </summary>
        /// <param name="pair">the file key value pair</param>
        /// <returns>the MetadataFileType of that pair</returns>
        ///<see cref="GetType"/>
        public static MetadataTypeFile GetMetadataTypeFile(KeyValuePair<Type, string> pair)
        {
            MetadataTypeFile ret = new MetadataTypeFile();
            Type type = pair.Key;
            ret.TypeAssembly = type.Assembly.FullName;
            ret.TypeName = type.Name;
            ret.TypeNamespace = type.Namespace;
            ret.FileName = pair.Value;
            return ret;
        }
        /// <summary>
        /// serialize a given MetadataFileType to XElement
        /// </summary>
        /// <param name="metadata">the object to serialize</param>
        /// <returns></returns>
        public static XElement Serialize(MetadataTypeFile metadata)
        {
            XElement ret = new XElement("type-file");
            XElement fileName = new XElement("file-name");
            fileName.Value = metadata.FileName;
            XElement typeName = new XElement("type-name");
            typeName.Value = metadata.TypeName;
            XElement typeNamespace = new XElement("type-namespace");
            typeNamespace.Value = metadata.TypeNamespace;
            XElement typeAssembly = new XElement("type-assembly");
            typeAssembly.Value = metadata.TypeAssembly;
            ret.Add(fileName);
            ret.Add(typeNamespace);
            ret.Add(typeName);
            ret.Add(typeAssembly);
            return ret;

        }
        /// <summary>
        /// Deserialize XElement to MetadataFileType
        /// </summary>
        /// <param name="xml">the XElement to deserialize</param>
        /// <returns>the deserialized object</returns>
        public static MetadataTypeFile Deserialize(XElement xml)
        {
            return xml.Elements()
                     .Select(element => new MetadataTypeFile
                     {
                         FileName = xml.Elements()
                                     .First(e => e.Name.LocalName == "file-name")
                                     .Value,
                         TypeAssembly = xml.Elements()
                                     .First(e => e.Name.LocalName == "type-assembly")
                                     .Value,
                         TypeName = xml.Elements()
                                     .First(e => e.Name.LocalName == "type-name")
                                     .Value,
                         TypeNamespace = xml.Elements()
                                     .First(e => e.Name.LocalName == "type-namespace")
                                     .Value,
                     })
                     .FirstOrDefault();
        }
    }
}
