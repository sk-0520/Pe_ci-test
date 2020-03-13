//#define ENABLED_NETCoreJSON

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Runtime.Serialization.Json;

#if ENABLED_NETCoreJSON
using System.Text.Json;
using System.Text.Json.Serialization;
#endif

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// シリアライズ・デシリアライズ処理を統括。
    /// </summary>
    public abstract class SerializerBase
    {
        #region property

        /// <summary>
        /// 文字列の場合のエンコーディング。
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        /// <summary>
        /// バッファサイズ。
        /// </summary>
        public int BufferSize { get; set; } = 4 * 1024;

        public Func<Stream>? InnserStreamCreator { get; set; }

        #endregion

        #region function

        protected Stream CreateInnerStream() => InnserStreamCreator?.Invoke() ?? new MemoryStream(BufferSize);

        protected TextReader GetReader(Stream stream) => new StreamReader(stream, Encoding, true, BufferSize, true);
        protected TextWriter GetWriter(Stream stream) => new StreamWriter(stream, Encoding, BufferSize, true);

        /// <summary>
        /// オブジェクトを複製する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">複製したいオブジェクト。</param>
        /// <returns></returns>
        public T Clone<T>(object source)
        {
            if(source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            using(var stream = CreateInnerStream()) {
                Save(source, stream);
                stream.Position = 0;
                return Load<T>(stream);
            }
        }

        public abstract TResult Load<TResult>(Stream stream);
        public abstract void Save(object value, Stream stream);

        #endregion
    }

#if ENABLED_JsonNet
    public class JsonNetSerializer : SerializerBase
    {
    #region SerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            using(var reader = GetReader(stream))
            using(var jsonReader = new Newtonsoft.Json.JsonTextReader(reader)) {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                return serializer.Deserialize<TResult>(jsonReader);
            }
        }

        public override void Save(object value, Stream stream)
        {
            using(var writer = GetWriter(stream))
            using(var jsonWriter = new Newtonsoft.Json.JsonTextWriter(writer)) {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(jsonWriter, value);
            }
        }

    #endregion
    }
#endif

#if ENABLED_NETCoreJSON

    public class JsonNetCoreSerializer : SerializerBase
    {
        #region property

        public JsonReaderOptions ReaderOptions { get; set; } = new JsonReaderOptions() {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip,
        };
        public JsonWriterOptions WriterOptions { get; set; } = new JsonWriterOptions() {
            Indented = true,
        };

        #endregion

        #region SerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            var buffer = new byte[stream.Length];
            //var span = new Span<byte>();
            stream.Read(buffer);
            var reader = new Utf8JsonReader(buffer, ReaderOptions);
            return System.Text.Json.JsonSerializer.Deserialize<TResult>(ref reader);
        }

        public override void Save(object value, Stream stream)
        {
            using(var writer = new Utf8JsonWriter(stream, WriterOptions)) {
                System.Text.Json.JsonSerializer.Serialize(writer, value);
            }
        }

        #endregion
    }

#endif

    public class JsonDataSerializer : SerializerBase
    {
        #region SerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            using(var reader = GetReader(stream)) {
                var serializer = new DataContractJsonSerializer(typeof(TResult));
                return (TResult)serializer.ReadObject(stream);
            }
        }

        public override void Save(object value, Stream stream)
        {
            using(var writer = GetWriter(stream)) {
                var serializer = new DataContractJsonSerializer(value.GetType());
                serializer.WriteObject(stream, value);
            }
        }

        #endregion
    }


    public abstract class XmlSerializerBase : SerializerBase
    {
        #region function

        protected XmlReaderSettings CreateXmlReaderSettings()
        {
            return new XmlReaderSettings() {
                CloseInput = false,
            };
        }

        protected XmlWriterSettings CreateXmlWriterSettings()
        {
            return new XmlWriterSettings() {
                CloseOutput = false,
                NewLineHandling = NewLineHandling.Entitize,
            };
        }

        #endregion
    }

    public class XmlSerializer : XmlSerializerBase
    {
        #region XmlSerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            using(var reader = XmlReader.Create(stream, CreateXmlReaderSettings())) {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TResult));
                return (TResult)serializer.Deserialize(reader);
            }
        }

        public override void Save(object value, Stream stream)
        {
            using(var writer = XmlWriter.Create(stream, CreateXmlWriterSettings())) {
                var serializer = new System.Xml.Serialization.XmlSerializer(value.GetType());
                serializer.Serialize(writer, value);
            }
        }

        #endregion
    }

    public abstract class DataContractSerializerBase : XmlSerializerBase
    { }

    public class XmlDataContractSerializer : DataContractSerializerBase
    {
        #region DataContractSerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            using(var reader = XmlReader.Create(stream, CreateXmlReaderSettings())) {
                var serializer = new DataContractSerializer(typeof(TResult));
                return (TResult)serializer.ReadObject(reader);
            }
        }

        public override void Save(object value, Stream stream)
        {
            using(var writer = XmlWriter.Create(stream, CreateXmlWriterSettings())) {
                var serializer = new DataContractSerializer(value.GetType());
                serializer.WriteObject(writer, value);
            }
        }

        #endregion
    }

    public class BinaryDataContractSerializer : DataContractSerializerBase
    {
        #region DataContractSerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            // 閉じない方法がわっからん
            var quotas = new XmlDictionaryReaderQuotas();
            using(var reader = XmlDictionaryReader.CreateBinaryReader(stream, quotas)) {
                var serializer = new DataContractSerializer(typeof(TResult));
                return (TResult)serializer.ReadObject(reader);
            }
        }

        public override void Save(object value, Stream stream)
        {
            using(var writer = XmlDictionaryWriter.CreateBinaryWriter(stream, null, null, false)) {
                var serializer = new DataContractSerializer(value.GetType());
                serializer.WriteObject(writer, value);
            }
        }

        #endregion
    }

    public static class SerializeUtility
    {
        #region function

        public static TResult Clone<TResult>(object value)
        {
            if(!(value is TResult)) {
                throw new ArgumentException($"cast error: {nameof(value)} is not ${typeof(TResult).FullName}");
            }

            var serializer = new BinaryDataContractSerializer();
            return serializer.Clone<TResult>(value);
        }

        public static TResult Clone<TResult>(TResult value)
            where TResult : new()
        {
            if(!(value is TResult)) {
                throw new ArgumentException($"cast error: {nameof(value)} is not ${typeof(TResult).FullName}");
            }

            var serializer = new BinaryDataContractSerializer();
            return serializer.Clone<TResult>(value);
        }

        #endregion
    }

#if ENABLED_MessagePack
    public class MessagePackSerializer : SerializerBase
    {
    #region SerializerBase

        public override TResult Load<TResult>(Stream stream)
        {
            return MessagePack.MessagePackSerializer.Deserialize<TResult>(stream);
        }

        public override void Save(object value, Stream stream)
        {
            MessagePack.MessagePackSerializer.Serialize(stream, value);
        }

    #endregion
    }
#endif
}
