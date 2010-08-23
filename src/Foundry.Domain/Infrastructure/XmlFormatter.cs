/*
 * Author: Fredrik Norén
 * Version: 1.0
 * 
 * Copyright (c) 2010 Furie AB
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml;

namespace Foundry.Domain.Infrastructure
{
    public class XmlFormatter : IFormatter
    {
        public XmlFormatter()
        {
            Context = new StreamingContext(StreamingContextStates.All);
            Binder = new XmlFormatterBinder();
            ErrorCallback = (error) => { throw new Exception(error); };
        }

        #region Serialize
        /// <summary>
        /// Serializes an object, or graph of objects with the given root to the provided stream.
        /// </summary>
        /// <param name="serializationStream">The stream where the formatter puts the serialized data. 
        /// This stream can reference a variety of backing stores (such as files, network, memory, and so on).</param>
        /// <param name="graph">The object, or root of the object graph, to serialize. 
        /// All child objects of this root object are automatically serialized.</param>
        public void Serialize(System.IO.Stream serializationStream, object graph)
        {
            XmlTextWriter writer = new XmlTextWriter(serializationStream, null);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("Data");
            Dictionary<object, bool> savedObjects = new Dictionary<object, bool>();
            if (PersistenceData == null) PersistenceData = new XmlFormatterPersistenceData();
            writer.WriteStartElement("Object");
            SerializeObject(writer, graph, savedObjects);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
        }

        void SerializeObject(XmlWriter writer, object obj, Dictionary<object, bool> savedObjects)
        {
            if (obj == null)
            {
                writer.WriteAttributeString("cast", "null");
            }
            else
            {
                Type type = obj.GetType();
                var objAsType = obj as Type;
                if (type.IsPrimitive || type == typeof(string) || type.IsEnum || objAsType != null)
                {
                    writer.WriteAttributeString("cast", "value");
                    writer.WriteAttributeString("type", type.FullName + ", " + type.Assembly.ToString());


                    if (type.IsEnum)
                        writer.WriteString(((int)obj).ToString(format));
                    else if (objAsType != null)
                        writer.WriteString(objAsType.FullName + ", " + objAsType.Assembly.ToString());
                    else
                        writer.WriteString(System.Convert.ToString(obj, format));
                }
                else
                {
                    if (savedObjects.ContainsKey(obj))
                    {
                        writer.WriteAttributeString("cast", "reference");
                        writer.WriteString(PersistenceData.ObjectIDs[obj]);
                    }
                    else
                    {
                        writer.WriteAttributeString("cast", "object");
                        writer.WriteAttributeString("type", type.FullName + ", " + type.Assembly.ToString());

                        String id;
                        if (!PersistenceData.ObjectIDs.TryGetValue(obj, out id))
                            PersistenceData.ObjectIDs[obj] = id = System.Guid.NewGuid().ToString();
                        savedObjects[obj] = true;

                        writer.WriteAttributeString("reference", id);

                        if (type.IsArray || typeof(System.Collections.IList).IsAssignableFrom(type))
                        {
                            if (type.IsArray)
                            {
                                String size = "";
                                for (int i = 0; i < type.GetArrayRank(); i++)
                                    size += type.GetMethod("GetLength").Invoke(obj, new object[] { i }) + ", ";
                                if (size.Length > 2) size = size.Substring(0, size.Length - 2);
                                writer.WriteAttributeString("size", size);
                            }
                            foreach (object o in (System.Collections.ICollection)obj)
                            {
                                writer.WriteStartElement("Element");
                                SerializeObject(writer, o, savedObjects);
                                writer.WriteEndElement();
                            }
                        }
                        else if (typeof(System.Collections.IDictionary).IsAssignableFrom(type))
                        {
                            foreach (System.Collections.DictionaryEntry o in ((System.Collections.IDictionary)obj))
                            {
                                writer.WriteStartElement("Element");
                                writer.WriteStartElement("Key");
                                SerializeObject(writer, o.Key, savedObjects);
                                writer.WriteEndElement();
                                writer.WriteStartElement("Value");
                                SerializeObject(writer, o.Value, savedObjects);
                                writer.WriteEndElement();
                                writer.WriteEndElement();
                            }
                        }
                        else
                        {
                            MemberInfo[] members = FormatterServices.GetSerializableMembers(obj.GetType(), Context);

                            object[] objs = FormatterServices.GetObjectData(obj, members);
                            for (int i = 0; i < objs.Length; ++i)
                            {
                                writer.WriteStartElement("Member");
                                writer.WriteAttributeString("name", members[i].Name);
                                SerializeObject(writer, objs[i], savedObjects);
                                writer.WriteEndElement();
                            }
                        }
                    }
                }
            }

        }
        #endregion

        #region Deserialize
        /// <summary>
        /// Deserializes the data on the provided stream and reconstitutes the graph of objects.
        /// </summary>
        /// <param name="serializationStream">The stream that contains the data to deserialize.</param>
        /// <returns>The top object of the deserialized graph.</returns>
        public object Deserialize(System.IO.Stream serializationStream)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            //First the xml document is loaded
            XmlDocument doc = new XmlDocument();
            doc.Load(serializationStream);

            //Then we read it into a structure
            Dictionary<String, INode> objects = new Dictionary<String, INode>();
            INode graph = ParseNode(doc.DocumentElement.ChildNodes[0], objects, ErrorCallback);

            //And finally we build the object graph and return it
            PersistenceData = new XmlFormatterPersistenceData();
            object o = graph.GetObject(null, objects);
            if (o == null)
                ErrorCallback("Unable to create root object");

            return o;
        }

        INode ParseNode(XmlNode node, Dictionary<String, INode> objects, ErrorHandler errorCallback)
        {
            String cast = node.Attributes["cast"].Value;
            if (cast == "null")
            {
                return new Null(this);
            }
            else
            {
                if (cast == "value")
                {
                    Type type = null;
                    if (node.Attributes["type"] != null)
                        type = ResolveType(node.Attributes["type"].Value);
                    return new ValueNode(this) { Value = node.InnerText, Type = type };
                }
                else if (cast == "reference")
                {
                    return new ReferenceNode(this) { Reference = node.InnerText };
                }
                else if (cast == "object")
                {
                    Type type = ResolveType(node.Attributes["type"].Value);

                    if (type == null)
                    {
                        errorCallback("Unrecognized type '" + node.Attributes["type"].Value + "'");
                        return new Null(this);
                    }
                    if (type.IsArray)
                        return new OArray(this, node, objects, errorCallback);
                    else if (typeof(System.Collections.IList).IsAssignableFrom(type))
                        return new OList(this, node, objects, errorCallback);
                    else if (typeof(System.Collections.IDictionary).IsAssignableFrom(type))
                        return new ODictionary(this, node, objects, errorCallback);
                    else
                        return new Object(this, node, objects, errorCallback);
                }
            }
            errorCallback("Unrecognized cast '" + cast + "'");
            return new Null(this);
        }

        abstract class INode
        {
            protected INode(XmlFormatter formatter)
            {
                this.formatter = formatter;
            }
            public virtual object GetObject(Type fieldType, Dictionary<String, INode> objects) { return null; }
            protected XmlFormatter formatter;
        }
        class Null : INode
        {
            public Null(XmlFormatter formatter) : base(formatter) { }
        }
        class ValueNode : INode
        {
            public ValueNode(XmlFormatter formatter) : base(formatter) { }
            public override object GetObject(Type fieldType, Dictionary<String, INode> objects)
            {
                Type type = Type;
                if (!fieldType.IsAssignableFrom(type)) type = fieldType;

                if (type.IsEnum)
                    return Enum.Parse(type, Value);
                else if (typeof(Type).IsAssignableFrom(type))
                    return formatter.ResolveType(Value);

                else
                {
                    if (type.IsPrimitive || type == typeof(string) || type.IsEnum)
                        return System.Convert.ChangeType(Value, type, formatter.format);
                    else
                    {
                        ConstructorInfo c = type.GetConstructor(new Type[] { typeof(String) });
                        if (c != null)
                            return c.Invoke(new object[] { Value });
                    }
                }

                formatter.ErrorCallback("Unable to convert value '" + Value + "' to type '" + fieldType + "'");
                return null;
            }
            public String Value;
            public Type Type;
        }
        class ReferenceNode : INode
        {
            public ReferenceNode(XmlFormatter formatter) : base(formatter) { }
            public override object GetObject(Type fieldType, Dictionary<String, INode> objects)
            {
                INode n;
                if (!objects.TryGetValue(Reference, out n))
                {
                    formatter.ErrorCallback("Unable to find reference '" + Reference + "'");
                    return null;
                }
                return n.GetObject(fieldType, objects);
            }
            public String Reference;
        }
        class IObject : INode
        {
            protected IObject(XmlFormatter formatter, XmlNode node, Dictionary<String, INode> objects, ErrorHandler errorCallback)
                : base(formatter)
            {
                var r = node.Attributes["reference"];
                if (r != null)
                {
                    reference = node.Attributes["reference"].Value;
                    objects.Add(reference, this);
                }
                type = formatter.ResolveType(node.Attributes["type"].Value);

                if (type == null)
                    errorCallback("Cannot find type \"" + node.Attributes["type"].Value + "\"");

            }
            protected Type type;
            protected String reference;
        }
        class Object : IObject
        {
            public Object(XmlFormatter formatter, XmlNode node, Dictionary<String, INode> objects, ErrorHandler errorCallback)
                : base(formatter, node, objects, errorCallback)
            {

                foreach (XmlNode n in node.ChildNodes)
                    if (n.Name == "Member")
                        members.Add(n.Attributes["name"].Value, formatter.ParseNode(n, objects, errorCallback));
            }

            protected Dictionary<string, INode> members = new Dictionary<string, INode>();

            object obj;

            public override object GetObject(Type fieldType, Dictionary<String, INode> objects)
            {
                if (obj == null && type != null)
                {
                    if (type.GetConstructor(new Type[] { }) != null)
                        obj = Activator.CreateInstance(type);
                    else
                        obj = FormatterServices.GetUninitializedObject(type);


                    MemberInfo[] members = FormatterServices.GetSerializableMembers(obj.GetType(), formatter.Context);

                    object[] data = new object[members.Length];
                    for (int i = 0; i < members.Length; ++i)
                    {
                        FieldInfo fi = ((FieldInfo)members[i]);
                        INode m = null;
                        //We start by trying the "fully qualified" name
                        if (!this.members.TryGetValue(fi.Name, out m))
                        {
                            //but if it doesn't exist we just use the field name and try to find it,
                            //for example if a variable has been moved from one class to another
                            String ending = fi.Name;
                            if (ending.Contains('<'))
                                ending = fi.Name.Substring(fi.Name.IndexOf('<'));
                            foreach (KeyValuePair<string, INode> e in this.members)
                                if (e.Key.EndsWith(ending))
                                {
                                    m = e.Value;
                                    break;
                                }
                        }
                        if (m != null)
                            data[i] = m.GetObject(fi.FieldType, objects);
                    }
                    FormatterServices.PopulateObjectMembers(obj, members, data);
                    if (!String.IsNullOrEmpty(reference))
                        formatter.PersistenceData.ObjectIDs[obj] = reference;

                    foreach (var m in type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
                    {
                        var a = m.GetCustomAttributes(typeof(OnDeserializedAttribute), true);
                        if (a.Length != 0)
                            m.Invoke(obj, new object[] { formatter.Context });
                    }
                }
                return obj;
            }
        }
        class OArray : IObject
        {
            public OArray(XmlFormatter formatter, XmlNode node, Dictionary<String, INode> objects, ErrorHandler errorCallback)
                : base(formatter, node, objects, errorCallback)
            {
                String size = node.Attributes["size"].Value;
                String[] ss = size.Split(new string[] { ", " }, StringSplitOptions.None);
                sizes = new object[ss.Length];
                for (int i = 0; i < sizes.Length; i++) sizes[i] = int.Parse(ss[i], formatter.format);
                foreach (XmlNode n in node.ChildNodes)
                    if (n.Name == "Element")
                        elements.Add(formatter.ParseNode(n, objects, errorCallback));
            }
            protected object[] sizes;
            protected List<INode> elements = new List<INode>();

            object obj;

            public override object GetObject(Type fieldType, Dictionary<String, INode> objects)
            {
                if (obj == null)
                {
                    if (fieldType.IsAssignableFrom(type))
                    {
                        obj = Activator.CreateInstance(type, sizes);
                        Type elemType = type.GetElementType();
                        MethodInfo setValue = type.GetMethod("SetValue", new Type[] { typeof(object), typeof(int[]) });
                        int[] index = new int[sizes.Length];
                        for (int i = 0; i < index.Length; i++) index[i] = 0;
                        foreach (INode m in elements)
                        {
                            object o = m.GetObject(elemType, objects);
                            setValue.Invoke(obj, new object[] { o, index });
                            for (int i = index.Length - 1; i >= 0; i--)
                            {
                                index[i]++;
                                if (index[i] < (int)sizes[i]) break;
                                else index[i] = 0;
                            }
                        }
                    }
                    else
                    {
                        object[] args = new object[sizes.Length];
                        for (int i = 0; i < args.Length; i++) args[i] = 0;
                        obj = Activator.CreateInstance(fieldType, args);
                    }
                    if (!String.IsNullOrEmpty(reference))
                        formatter.PersistenceData.ObjectIDs[obj] = reference;

                }
                return obj;
            }
        }
        class OList : IObject
        {
            public OList(XmlFormatter formatter, XmlNode node, Dictionary<String, INode> objects, ErrorHandler errorCallback)
                : base(formatter, node, objects, errorCallback)
            {
                foreach (XmlNode n in node.ChildNodes)
                    if (n.Name == "Element")
                        elements.Add(formatter.ParseNode(n, objects, errorCallback));
            }
            protected List<INode> elements = new List<INode>();

            object obj;

            public override object GetObject(Type fieldType, Dictionary<String, INode> objects)
            {
                if (obj == null)
                {
                    if (fieldType.IsAssignableFrom(type))
                    {
                        obj = Activator.CreateInstance(type);
                        Type elemType = obj.GetType().GetGenericArguments()[0];
                        foreach (INode m in elements)
                        {
                            object o = m.GetObject(elemType, objects);
                            ((System.Collections.IList)obj).Add(o);
                        }
                    }
                    else
                        obj = Activator.CreateInstance(fieldType);
                    if (!String.IsNullOrEmpty(reference))
                        formatter.PersistenceData.ObjectIDs[obj] = reference;

                }
                return obj;
            }
        }
        class ODictionary : IObject
        {
            public ODictionary(XmlFormatter formatter, XmlNode node, Dictionary<String, INode> objects, ErrorHandler errorCallback)
                : base(formatter, node, objects, errorCallback)
            {
                foreach (XmlNode n2 in node.ChildNodes)
                {
                    if (n2.Name == "Element")
                    {
                        INode key = null, value = null;
                        foreach (XmlNode n4 in n2.ChildNodes)
                        {
                            if (n4.Name == "Key") key = formatter.ParseNode(n4, objects, errorCallback);
                            if (n4.Name == "Value") value = formatter.ParseNode(n4, objects, errorCallback);
                        }
                        elements.Add(new KeyValuePair<INode, INode>(key, value));
                    }
                }
            }
            protected List<KeyValuePair<INode, INode>> elements = new List<KeyValuePair<INode, INode>>();

            object obj;

            public override object GetObject(Type fieldType, Dictionary<String, INode> objects)
            {
                if (obj == null)
                {
                    Type t = type;
                    if (!fieldType.IsAssignableFrom(t)) t = fieldType;

                    obj = Activator.CreateInstance(t);

                    Type keyType = obj.GetType().GetGenericArguments()[0];
                    Type valueType = obj.GetType().GetGenericArguments()[1];

                    if (type == fieldType
                        || (keyType.IsAssignableFrom(type.GetGenericArguments()[0])
                            || keyType.GetConstructor(new Type[] { typeof(string) }) != null)
                        && (valueType.IsAssignableFrom(type.GetGenericArguments()[1])
                            || valueType.GetConstructor(new Type[] { typeof(string) }) != null))
                        foreach (KeyValuePair<INode, INode> m in elements)
                        {
                            object key = m.Key.GetObject(keyType, objects);
                            object value = m.Value.GetObject(valueType, objects);
                            ((System.Collections.IDictionary)obj).Add(key, value);
                        }

                    if (!String.IsNullOrEmpty(reference))
                        formatter.PersistenceData.ObjectIDs[obj] = reference;
                }
                return obj;
            }
        }
        #endregion

        #region Type resolving
        /// <summary>
        /// First tries to load the type using ResolveType. If that fails it decomposes the type
        /// and searches the loaded assemblies for the type and then assembles it
        /// </summary>
        Type ResolveType(String typeName)
        {
            //Complex type (`1[[..]])
            if (typeName.Contains('`'))
            {
                int lastIndexTypeArgs = typeName.LastIndexOf(']');
                String type = typeName.Substring(0, lastIndexTypeArgs + 1);
                String assembly = typeName.Substring(lastIndexTypeArgs + 3);
                assembly = assembly.Split(new String[] { ", " }, StringSplitOptions.None)[0];
                int firstIndexTypeArgs = typeName.IndexOf('[');
                type = type.Substring(0, firstIndexTypeArgs);
                Type tType = Binder.BindToType(assembly, type);
                String typeArgs = typeName.Substring(firstIndexTypeArgs + 1, lastIndexTypeArgs - firstIndexTypeArgs - 1);

                List<Type> tArgs = new List<Type>();
                int depth = 1;
                int start = 1;
                for (int i = 1; i < typeArgs.Length; i++)
                {
                    if (typeArgs[i] == '[') depth++;
                    if (typeArgs[i] == ']')
                    {
                        depth--;
                        if (depth == 0)
                        {
                            tArgs.Add(ResolveType(typeArgs.Substring(start, i - start)));
                            start = i + 3;
                        }
                    }
                }
                return tType.MakeGenericType(tArgs.ToArray());
            }
            //Simple type
            else
            {
                String[] ss = typeName.Split(new String[] { ", " }, StringSplitOptions.None);
                String type = ss[0];
                String assembly = ss[1];
                return Binder.BindToType(assembly, type);
            }
        }
        #endregion

        IFormatProvider format = System.Globalization.CultureInfo.InvariantCulture;

        // The error callbacks is just started on, the idea is that deserialize almost always is able to
        // deserialize files, but it reports all the errors it encounters along the way, instead of throwing
        // exceptions, because we want the execution to continue
        public delegate void ErrorHandler(String error);
        /// <summary>
        /// The error handling callback. Default behavior is to throw an exception.
        /// </summary>
        public ErrorHandler ErrorCallback { get; set; }
        /// <summary>
        /// Used to minimize changes to the document. Store the object after deserialization and then set it when serializing again.
        /// </summary>
        public XmlFormatterPersistenceData PersistenceData { get; set; }
        public ISurrogateSelector SurrogateSelector { get; set; }
        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }
    }
    public sealed class XmlFormatterPersistenceData
    {
        public XmlFormatterPersistenceData()
        {
            ObjectIDs = new Dictionary<object, string>();
        }
        public Dictionary<object, String> ObjectIDs { get; private set; }
    }
    /// <summary>
    /// The default binder for the XmlFormatter. This binder first tries to find the type in the specified assembly,
    /// if it fails, it will look in other assemblies.
    /// </summary>
    public class XmlFormatterBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type bestMatch = null;
            foreach (Assembly a in loadedAssemblies)
            {
                Type t = a.GetType(typeName);
                if (a.FullName.StartsWith(assemblyName) && t != null)
                    return t;
                if (t != null)
                    bestMatch = t;
            }
            return bestMatch;
        }
    }
}
