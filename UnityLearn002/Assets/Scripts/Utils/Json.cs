﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class Json
{
    /// <summary>
    /// Parses the string json into a value
    /// </summary>
    /// <param name="json">A JSON string.</param>
    /// <returns>An List&lt;object&gt;, a Dictionary&lt;string, object&gt;, a double, an integer,a string, null, true, or false</returns>
    public static object Deserialize(string json)
    {
        // save the string for debug information
        if (json == null)
        {
            return null;
        }

        return Parser.Parse(json);
    }

    sealed class Parser : IDisposable
    {
        const string WordBreak = "{}[],:\"";

        private static bool IsWordBreak(char c)
        {
            return char.IsWhiteSpace(c) || WordBreak.IndexOf(c) != -1;
        }

        enum Token
        {
            None,
            CurlyOpen,
            CurlyClose,
            SquaredOpen,
            SquaredClose,
            Colon,
            Comma,
            String,
            Number,
            True,
            False,
            Null
        };

        private StringReader json;

        private Parser(string jsonString)
        {
            json = new StringReader(jsonString);
        }

        public static object Parse(string jsonString)
        {
            using (var instance = new Parser(jsonString))
            {
                return instance.ParseValue();
            }
        }

        public void Dispose()
        {
            json.Dispose();
            json = null;
        }

        Dictionary<string, object> ParseObject()
        {
            Dictionary<string, object> table = new Dictionary<string, object>();

            // ditch opening brace
            json.Read();

            // {
            while (true)
            {
                switch (NextToken)
                {
                    case Token.None:
                        return null;
                    case Token.Comma:
                        continue;
                    case Token.CurlyClose:
                        return table;
                    default:
                        // name
                        string name = ParseString();
                        if (name == null)
                        {
                            return null;
                        }

                        // :
                        if (NextToken != Token.Colon)
                        {
                            return null;
                        }
                        // ditch the colon
                        json.Read();

                        // value
                        table[name] = ParseValue();
                        break;
                }
            }
        }

        private List<object> ParseArray()
        {
            var array = new List<object>();

            // ditch opening bracket
            json.Read();

            // [
            var parsing = true;
            while (parsing)
            {
                Token nextToken = NextToken;

                switch (nextToken)
                {
                    case Token.None:
                        return null;
                    case Token.Comma:
                        continue;
                    case Token.SquaredClose:
                        parsing = false;
                        break;
                    default:
                        object value = ParseByToken(nextToken);

                        array.Add(value);
                        break;
                }
            }

            return array;
        }

        private object ParseValue()
        {
            var nextToken = NextToken;
            return ParseByToken(nextToken);
        }

        private object ParseByToken(Token token)
        {
            switch (token)
            {
                case Token.String:
                    return ParseString();
                case Token.Number:
                    return ParseNumber();
                case Token.CurlyOpen:
                    return ParseObject();
                case Token.SquaredOpen:
                    return ParseArray();
                case Token.True:
                    return true;
                case Token.False:
                    return false;
                case Token.Null:
                    return null;
                default:
                    return null;
            }
        }

        private string ParseString()
        {
            var s = new StringBuilder();
            char c;

            // ditch opening quote
            json.Read();

            bool parsing = true;
            while (parsing)
            {

                if (json.Peek() == -1)
                {
                    parsing = false;
                    break;
                }

                c = NextChar;
                switch (c)
                {
                    case '"':
                        parsing = false;
                        break;
                    case '\\':
                        if (json.Peek() == -1)
                        {
                            parsing = false;
                            break;
                        }

                        c = NextChar;
                        switch (c)
                        {
                            case '"':
                            case '\\':
                            case '/':
                                s.Append(c);
                                break;
                            case 'b':
                                s.Append('\b');
                                break;
                            case 'f':
                                s.Append('\f');
                                break;
                            case 'n':
                                s.Append('\n');
                                break;
                            case 'r':
                                s.Append('\r');
                                break;
                            case 't':
                                s.Append('\t');
                                break;
                            case 'u':
                                var hex = new char[4];

                                for (int i = 0; i < 4; i++)
                                {
                                    hex[i] = NextChar;
                                }

                                s.Append((char)Convert.ToInt32(new string(hex), 16));
                                break;
                        }
                        break;
                    default:
                        s.Append(c);
                        break;
                }
            }

            return s.ToString();
        }

        private object ParseNumber()
        {
            var number = NextWord;

            if (number.IndexOf('.') == -1)
            {
                long parsedInt;
                Int64.TryParse(number, out parsedInt);
                return parsedInt;
            }

            double.TryParse(number, out var parsedDouble);
            return parsedDouble;
        }

        private void EatWhitespace()
        {
            while (char.IsWhiteSpace(PeekChar))
            {
                json.Read();

                if (json.Peek() == -1)
                {
                    break;
                }
            }
        }

        private char PeekChar => Convert.ToChar(json.Peek());

        private char NextChar => Convert.ToChar(json.Read());

        private string NextWord
        {
            get
            {
                var word = new StringBuilder();

                while (!IsWordBreak(PeekChar))
                {
                    word.Append(NextChar);

                    if (json.Peek() == -1)
                    {
                        break;
                    }
                }

                return word.ToString();
            }
        }

        private Token NextToken
        {
            get
            {
                EatWhitespace();

                if (json.Peek() == -1)
                {
                    return Token.None;
                }

                switch (PeekChar)
                {
                    case '{':
                        return Token.CurlyOpen;
                    case '}':
                        json.Read();
                        return Token.CurlyClose;
                    case '[':
                        return Token.SquaredOpen;
                    case ']':
                        json.Read();
                        return Token.SquaredClose;
                    case ',':
                        json.Read();
                        return Token.Comma;
                    case '"':
                        return Token.String;
                    case ':':
                        return Token.Colon;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-':
                        return Token.Number;
                }

                switch (NextWord)
                {
                    case "false":
                        return Token.False;
                    case "true":
                        return Token.True;
                    case "null":
                        return Token.Null;
                }

                return Token.None;
            }
        }
    }

    /// <summary>
    /// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
    /// </summary>
    /// <param name="json">A Dictionary&lt;string, object&gt; / List&lt;object&gt;</param>
    /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
    public static string Serialize(object obj)
    {
        return Serializer.Serialize(obj);
    }

    private sealed class Serializer
    {
        readonly StringBuilder builder;

        private Serializer()
        {
            builder = new StringBuilder();
        }

        public static string Serialize(object obj)
        {
            var instance = new Serializer();

            instance.SerializeValue(obj);

            return instance.builder.ToString();
        }

        private void SerializeValue(object value)
        {
            IList asList;
            IDictionary asDict;
            string asStr;

            if (value == null)
            {
                builder.Append("null");
            }
            else if ((asStr = value as string) != null)
            {
                SerializeString(asStr);
            }
            else if (value is bool)
            {
                builder.Append((bool)value ? "true" : "false");
            }
            else if ((asList = value as IList) != null)
            {
                SerializeArray(asList);
            }
            else if ((asDict = value as IDictionary) != null)
            {
                SerializeObject(asDict);
            }
            else if (value is char)
            {
                SerializeString(new string((char)value, 1));
            }
            else
            {
                SerializeOther(value);
            }
        }

        private void SerializeObject(IDictionary obj)
        {
            var first = true;

            builder.Append('{');

            foreach (var e in obj.Keys)
            {
                if (!first)
                {
                    builder.Append(',');
                }

                SerializeString(e.ToString());
                builder.Append(':');

                SerializeValue(obj[e]);

                first = false;
            }

            builder.Append('}');
        }

        void SerializeArray(IList anArray)
        {
            builder.Append('[');

            var first = true;

            foreach (var obj in anArray)
            {
                if (!first)
                {
                    builder.Append(',');
                }

                SerializeValue(obj);

                first = false;
            }

            builder.Append(']');
        }

        private void SerializeString(string str)
        {
            builder.Append('\"');

            var charArray = str.ToCharArray();
            foreach (var c in charArray)
            {
                switch (c)
                {
                    case '"':
                        builder.Append("\\\"");
                        break;
                    case '\\':
                        builder.Append("\\\\");
                        break;
                    case '\b':
                        builder.Append("\\b");
                        break;
                    case '\f':
                        builder.Append("\\f");
                        break;
                    case '\n':
                        builder.Append("\\n");
                        break;
                    case '\r':
                        builder.Append("\\r");
                        break;
                    case '\t':
                        builder.Append("\\t");
                        break;
                    default:
                        int codepoint = Convert.ToInt32(c);
                        if ((codepoint >= 32) && (codepoint <= 126))
                        {
                            builder.Append(c);
                        }
                        else
                        {
                            builder.Append("\\u");
                            builder.Append(codepoint.ToString("x4"));
                        }
                        break;
                }
            }

            builder.Append('\"');
        }

        private void SerializeOther(object value)
        {
            // NOTE: decimals lose precision during serialization.
            // They always have, I'm just letting you know.
            // Previously floats and doubles lost precision too.
            if (value is float f)
            {
                builder.Append(f.ToString("R"));
            }
            else if (value is int
              || value is uint
              || value is long
              || value is sbyte
              || value is byte
              || value is short
              || value is ushort
              || value is ulong)
            {
                builder.Append(value);
            }
            else if (value is double || value is decimal)
            {
                builder.Append(Convert.ToDouble(value).ToString("R"));
            }
            else
            {
                SerializeString(value.ToString());
            }
        }
    }
}
