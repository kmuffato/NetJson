/*
    +==========================================+
    +              NetJson v1.0                +
    +           by jonasfx21 @ GitHub          +
    +==========================================+
*/

using System;
using NetJson.Analysis;

namespace NetJson
{
    /// <summary>
    /// This exception is thrown when a key doesn't exist in a JSON objet.
    /// </summary>
    [Serializable] public class JsonKeyException : Exception
    {
        /// <summary>
        /// The object in question.
        /// </summary>
        public JsonObject Object;

        /// <summary>
        /// Initializes a new JsonKeyException.
        /// </summary>
        /// <param name="Object">The object in question.</param>
        public JsonKeyException(JsonObject Object)
        {
            this.Object = Object;
        }

        /// <summary>
        /// Initializes a new JsonKeyException.
        /// </summary>
        /// <param name="Object">The object in question.</param>
        /// <param name="message">The message.</param>
        public JsonKeyException(JsonObject Object, string message) : base(message)
        {
            this.Object = Object;
        }

        /// <summary>
        /// Initializes a new JsonKeyException.
        /// </summary>
        /// <param name="Object">The object in question.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public JsonKeyException(JsonObject Object, string message, Exception inner) : base(message, inner)
        {
            this.Object = Object;
        }

        /// <summary>
        /// Initializes a new JsonKeyException.
        /// </summary>
        protected JsonKeyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// This exception is thrown when an index is out of range in a JSON array.
    /// </summary>
    [Serializable] public class JsonIndexException : Exception
    {
        /// <summary>
        /// The array in question.
        /// </summary>
        public JsonArray Array;

        /// <summary>
        /// Initializes a new JsonIndexException.
        /// </summary>
        /// <param name="Array">The array in question.</param>
        public JsonIndexException(JsonArray Array)
        {
            this.Array = Array;
        }

        /// <summary>
        /// Initializes a new JsonIndexException.
        /// </summary>
        /// <param name="Array">The array in question.</param>
        /// <param name="message">The message.</param>
        public JsonIndexException(JsonArray Array, string message) : base(message)
        {
            this.Array = Array;
        }

        /// <summary>
        /// Initializes a new JsonIndexException.
        /// </summary>
        /// <param name="Array">The array in question.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public JsonIndexException(JsonArray Array, string message, Exception inner) : base(message, inner)
        {
            this.Array = Array;
        }

        /// <summary>
        /// Initializes a new JsonIndexException.
        /// </summary>
        protected JsonIndexException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// This exception is thrown when an item in a JSON array doesn't exist.
    /// </summary>
    [Serializable] public class JsonItemException : Exception
    {
        /// <summary>
        /// The array in question.
        /// </summary>
        public JsonArray Array;

        /// <summary>
        /// Initializes a new JsonItemException.
        /// </summary>
        /// <param name="Array">The array in question.</param>
        public JsonItemException(JsonArray Array)
        {
            this.Array = Array;
        }

        /// <summary>
        /// Initializes a new JsonItemException.
        /// </summary>
        /// <param name="Array">The array in question.</param>
        /// <param name="message">The message.</param>
        public JsonItemException(JsonArray Array, string message) : base(message)
        {
            this.Array = Array;
        }

        /// <summary>
        /// Initializes a new JsonItemException.
        /// </summary>
        /// <param name="Array">The array in question.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public JsonItemException(JsonArray Array, string message, Exception inner) : base(message, inner)
        {
            this.Array = Array;
        }

        /// <summary>
        /// Initializes a new JsonItemException.
        /// </summary>
        protected JsonItemException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// This exception is thrown when invalid JSON is given to the parser.
    /// </summary>
    [Serializable] public class JsonParseException : Exception
    {
        /// <summary>
        /// The source position.
        /// </summary>
        public PositionInfo Position;

        /// <summary>
        /// Initializes a new JsonParseException.
        /// </summary>
        /// <param name="Position">The source position.</param>
        public JsonParseException(PositionInfo Position)
        {
            this.Position = Position;
        }

        /// <summary>
        /// Initializes a new JsonParseException.
        /// </summary>
        /// <param name="Position">The source position.</param>
        /// <param name="message">The message.</param>
        public JsonParseException(PositionInfo Position, string message) : base(message)
        {
            this.Position = Position;
        }

        /// <summary>
        /// Initializes a new JsonParseException.
        /// </summary>
        /// <param name="Position">The source position.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public JsonParseException(PositionInfo Position, string message, Exception inner) : base(message, inner)
        {
            this.Position = Position;
        }

        /// <summary>
        /// Initializes a new JsonParseException.
        /// </summary>
        protected JsonParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    /// <summary>
    /// This exception is thrown when a non-JSON type was encountered.
    /// </summary>
    [Serializable] public class JsonTypeException : Exception
    {
        /// <summary>
        /// Initializes a new JsonTypeException.
        /// </summary>
        public JsonTypeException() {}

        /// <summary>
        /// Initializes a new JsonTypeException.
        /// </summary>
        /// <param name="message">The message.</param>
        public JsonTypeException(string message) : base(message) {}

        /// <summary>
        /// Initializes a new JsonTypeException.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public JsonTypeException(string message, Exception inner) : base(message, inner) {}

        /// <summary>
        /// Initializes a new JsonTypeException.
        /// </summary>
        protected JsonTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// This exception is thrown when a field type mismatches with a JSON type.
    /// </summary>
    [Serializable] public class FieldTypeException : Exception
    {
        /// <summary>
        /// The object key.
        /// </summary>
        public string Key;

        /// <summary>
        /// The field type.
        /// </summary>
        public Type FieldType;

        /// <summary>
        /// The JSON type.
        /// </summary>
        public Type JsonType;

        /// <summary>
        /// Initializes a new FieldTypeException.
        /// </summary>
        /// <param name="Key">The object key.</param>
        /// <param name="FieldType">The field type.</param>
        /// <param name="JsonType">The JSON type.</param>
        public FieldTypeException(string Key, Type FieldType, Type JsonType)
        {
            this.Key = Key;
            this.FieldType = FieldType;
            this.JsonType = JsonType;
        }

        /// <summary>
        /// Initializes a new FieldTypeException.
        /// </summary>
        /// <param name="Key">The object key.</param>
        /// <param name="FieldType">The field type.</param>
        /// <param name="JsonType">The JSON type.</param>
        /// <param name="message">The message.</param>
        public FieldTypeException(string Key, Type FieldType, Type JsonType, string message) : base(message)
        {
            this.Key = Key;
            this.FieldType = FieldType;
            this.JsonType = JsonType;
        }

        /// <summary>
        /// Initializes a new FieldTypeException.
        /// </summary>
        /// <param name="Key">The object key.</param>
        /// <param name="FieldType">The field type.</param>
        /// <param name="JsonType">The JSON type.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public FieldTypeException(string Key, Type FieldType, Type JsonType, string message, Exception inner) : base(message, inner)
        {
            this.Key = Key;
            this.FieldType = FieldType;
            this.JsonType = JsonType;
        }

        /// <summary>
        /// Initializes a new FieldTypeException.
        /// </summary>
        protected FieldTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}