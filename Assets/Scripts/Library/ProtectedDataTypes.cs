using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Krafton.SP2.X1.Lib
{
    [Serializable]
    public struct ProtectedInt : IFormattable, IEquatable<ProtectedInt>, IEquatable<int>, IComparable<ProtectedInt>, IComparable<int>, IComparable
    {
        [SerializeField]
        private int _Key;

        [SerializeField]
        private int _Value;

        [SerializeField]
        private bool _IsExistent;		

        private ProtectedInt(int value)
        {
			//Encrypt()함수가 static이 아닌 이상, 모든 멤버변수가 할당되야 Encrypt()함수를 쓸 수 있다.
			_Key = 0;
			_Value = 0;
			_IsExistent = true;
			Encrypt(value);
        }

        private void Encrypt(int value)
        {
			_Key = CreateKey();
			_Value = value ^ _Key;
        }

        private int Decrypt()
        {
            return _Value ^ _Key;
        }

        private static int CreateKey()
        {
            return RandomNumberGenerator.GetIntRandomNumber();
        }

        private int InbuiltDecrypt()
        {
            if (!_IsExistent)
            {
                Encrypt(0);
                _IsExistent = true;
                return 0;
            }

            return Decrypt();
        }
        
        public static implicit operator ProtectedInt(int value)
        {
            return new ProtectedInt(value);
        }
        
        public static implicit operator int(ProtectedInt value)
        {
            return value.InbuiltDecrypt();
        }

        public static ProtectedInt operator ++(ProtectedInt input)
        {
            int decryptedValue = input.InbuiltDecrypt() + 1;
			input.Encrypt(decryptedValue);

            return input;
        }

        public static ProtectedInt operator --(ProtectedInt input)
        {
            int decryptedValue = input.InbuiltDecrypt() - 1;
			input.Encrypt(decryptedValue);

            return input;
        }

        public override int GetHashCode()
        {
            return InbuiltDecrypt().GetHashCode();
        }

        public override string ToString()
        {
            return InbuiltDecrypt().ToString();
        }

        public string ToString(string format)
        {
            return InbuiltDecrypt().ToString(format);
        }

        public string ToString(IFormatProvider provider)
        {
            return InbuiltDecrypt().ToString(provider);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return InbuiltDecrypt().ToString(format, provider);
        }

        public override bool Equals(object obj)
        {
            return obj is ProtectedInt && Equals((ProtectedInt)obj);
        }

        public bool Equals(ProtectedInt obj)
        {
            if (_Key == obj._Key)
                return _Value.Equals(obj._Value);

            return Decrypt().Equals(obj.Decrypt());
        }

		public bool Equals(int other)
        {
            return Decrypt().Equals(other);
        }

        public int CompareTo(ProtectedInt other)
        {
            return InbuiltDecrypt().CompareTo(other.InbuiltDecrypt());
        }

        public int CompareTo(int other)
        {
            return InbuiltDecrypt().CompareTo(other);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
				return 1;
			
			if(!(obj is int))
				throw new ArgumentException("Argument must be int.");
				
            return CompareTo((int)obj);
        }
    }

    [Serializable]
    public struct ProtectedLong : IFormattable, IEquatable<ProtectedLong>, IEquatable<long>, IComparable<ProtectedLong>, IComparable<long>, IComparable
    {
        [SerializeField]
        private long _Key;

        [SerializeField]
        private long _Value;

        [SerializeField]
        private bool _IsExistent;		

        private ProtectedLong(long value)
        {
            _Key = 0L;
			_Value = 0L;
			_IsExistent = true;
			Encrypt(value);
        }

        private void Encrypt(long value)
        {
			_Key = CreateKey();
			_Value = value ^ _Key;
        }

        private long Decrypt()
        {
            return _Value ^ _Key;
        }

        private static long CreateKey()
        {
            return RandomNumberGenerator.GetLongRandomNumber();
        }

        private long InbuiltDecrypt()
        {
            if (!_IsExistent)
            {
                Encrypt(0L);
                _IsExistent = true;
                return 0L;
            }

            return Decrypt();
        }
        
        public static implicit operator ProtectedLong(long value)
        {
            return new ProtectedLong(value);
        }
        
        public static implicit operator long(ProtectedLong value)
        {
            return value.InbuiltDecrypt();
        }

        public static ProtectedLong operator ++(ProtectedLong input)
        {
            long decryptedValue = input.InbuiltDecrypt() + 1L;
			input.Encrypt(decryptedValue);

            return input;
        }

        public static ProtectedLong operator --(ProtectedLong input)
        {
            long decryptedValue = input.InbuiltDecrypt() - 1L;
			input.Encrypt(decryptedValue);

            return input;
        }

        public override int GetHashCode()
        {
            return InbuiltDecrypt().GetHashCode();
        }

        public override string ToString()
        {
            return InbuiltDecrypt().ToString();
        }

        public string ToString(string format)
        {
            return InbuiltDecrypt().ToString(format);
        }

        public string ToString(IFormatProvider provider)
        {
            return InbuiltDecrypt().ToString(provider);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return InbuiltDecrypt().ToString(format, provider);
        }

        public override bool Equals(object obj)
        {
            return obj is ProtectedLong && Equals((ProtectedLong)obj);
        }

        public bool Equals(ProtectedLong obj)
        {
            if (_Key == obj._Key)
                return _Value.Equals(obj._Value);

            return Decrypt().Equals(obj.Decrypt());
        }

		public bool Equals(long other)
        {
            return Decrypt().Equals(other);
        }

        public int CompareTo(ProtectedLong other)
        {
            return InbuiltDecrypt().CompareTo(other.InbuiltDecrypt());
        }

        public int CompareTo(long other)
        {
            return InbuiltDecrypt().CompareTo(other);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
				return 1;

			if(!(obj is long))
				throw new ArgumentException("Argument must be long.");

            return CompareTo((long)obj);
        }
    }

    [Serializable]
	public struct ProtectedFloat : IFormattable, IEquatable<ProtectedFloat>, IEquatable<float>, IComparable<ProtectedFloat>, IComparable<float>, IComparable
	{
		[SerializeField]
		private int _Key;

		[SerializeField]
		private int _Value;

		[SerializeField]
		private bool _IsExistent;

		private ProtectedFloat(float value)
		{
			_Key = 0;
			_Value = 0;
			_IsExistent = true;
			Encrypt(value);
		}

		private void Encrypt(float value)
		{
			_Key = CreateKey();
            FloatIntUnion floatIntUnion;
            floatIntUnion.IntValue = 0;
            floatIntUnion.FloatValue = value;
            _Value = floatIntUnion.IntValue ^ _Key;
		}

		private float Decrypt()
		{
            FloatIntUnion floatIntUnion;
            floatIntUnion.FloatValue = 0.0f;
            floatIntUnion.IntValue = _Value ^ _Key;
			return floatIntUnion.FloatValue;
		}

		public static int CreateKey()
		{
			return RandomNumberGenerator.GetIntRandomNumber();
		}

		private float InbuiltDecrypt()
		{
			if (!_IsExistent)
			{
				Encrypt(0.0f);
				_IsExistent = true;
				return 0.0f;
            }

			return Decrypt();
		}

		public static implicit operator ProtectedFloat(float value)
		{
			return new ProtectedFloat(value);
		}

		public static implicit operator float(ProtectedFloat value)
		{
			return value.InbuiltDecrypt();
		}

		public static ProtectedFloat operator ++(ProtectedFloat input)
		{
			float decryptedValue = input.InbuiltDecrypt() + 1.0f;
            input.Encrypt(decryptedValue);

            return input;
		}

		public static ProtectedFloat operator --(ProtectedFloat input)
		{
			float decryptedValue = input.InbuiltDecrypt() - 1.0f;
            input.Encrypt(decryptedValue);

            return input;
		}

		public override int GetHashCode()
		{
			return InbuiltDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InbuiltDecrypt().ToString();
		}

		public string ToString(string format)
		{
			return InbuiltDecrypt().ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(format, provider);
		}

		public override bool Equals(object obj)
		{
			return obj is ProtectedFloat && Equals((ProtectedFloat)obj);
		}

		public bool Equals(ProtectedFloat obj)
		{
			return obj.InbuiltDecrypt().Equals(InbuiltDecrypt());
		}

		public bool Equals(float other)
		{
			return InbuiltDecrypt().Equals(other);
		}

		public int CompareTo(ProtectedFloat other)
		{
			return InbuiltDecrypt().CompareTo(other.InbuiltDecrypt());
		}

		public int CompareTo(float other)
		{
			return InbuiltDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			
			if(!(obj is float))
				throw new ArgumentException("Argument must be float.");
			
			return CompareTo((float)obj);
		}

		[StructLayout(LayoutKind.Explicit)]
		internal struct FloatIntUnion
		{
			[FieldOffset(0)]
			internal float FloatValue;

			[FieldOffset(0)]
			internal int IntValue;
		}
	}

    [Serializable]
	public struct ProtectedDouble : IFormattable, IEquatable<ProtectedDouble>, IEquatable<double>, IComparable<ProtectedDouble>, IComparable<double>, IComparable
	{
		[SerializeField]
		private long _Key;

		[SerializeField]
		private long _Value;

		[SerializeField]
		private bool _IsExistent;

		private ProtectedDouble(double value)
		{
			_Key = 0L;
			_Value = 0L;
			_IsExistent = true;
			Encrypt(value);
		}

		private void Encrypt(double value)
		{
			_Key = CreateKey();
            DoubleLongUnion doubleLongUnion;
            doubleLongUnion.LongValue = 0L;
            doubleLongUnion.DoubleValue = value;
            _Value = doubleLongUnion.LongValue ^ _Key;
		}

		private double Decrypt()
		{
            DoubleLongUnion doubleLongUnion;
            doubleLongUnion.DoubleValue = 0.0;
            doubleLongUnion.LongValue = _Value ^ _Key;
			return doubleLongUnion.DoubleValue;
		}

		public static long CreateKey()
		{
			return RandomNumberGenerator.GetLongRandomNumber();
		}

		private double InbuiltDecrypt()
		{
			if (!_IsExistent)
			{
				Encrypt(0.0);
				_IsExistent = true;
				return 0.0;
            }

			return Decrypt();
		}

		public static implicit operator ProtectedDouble(double value)
		{
			return new ProtectedDouble(value);
		}

		public static implicit operator double(ProtectedDouble value)
		{
			return value.InbuiltDecrypt();
		}

		public static ProtectedDouble operator ++(ProtectedDouble input)
		{
			double decryptedValue = input.InbuiltDecrypt() + 1.0;
            input.Encrypt(decryptedValue);

            return input;
		}

		public static ProtectedDouble operator --(ProtectedDouble input)
		{
			double decryptedValue = input.InbuiltDecrypt() - 1.0;
            input.Encrypt(decryptedValue);

            return input;
		}

		public override int GetHashCode()
		{
			return InbuiltDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InbuiltDecrypt().ToString();
		}

		public string ToString(string format)
		{
			return InbuiltDecrypt().ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(format, provider);
		}

		public override bool Equals(object obj)
		{
			return obj is ProtectedDouble && Equals((ProtectedDouble)obj);
		}

		public bool Equals(ProtectedDouble obj)
		{
			return obj.InbuiltDecrypt().Equals(InbuiltDecrypt());
		}

		public bool Equals(double other)
		{
			return InbuiltDecrypt().Equals(other);
		}

		public int CompareTo(ProtectedDouble other)
		{
			return InbuiltDecrypt().CompareTo(other.InbuiltDecrypt());
		}

		public int CompareTo(double other)
		{
			return InbuiltDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			
			if(!(obj is double))
				throw new ArgumentException("Argument must be double.");
			
			return CompareTo((double)obj);
		}

		[StructLayout(LayoutKind.Explicit)]
		internal struct DoubleLongUnion
		{
			[FieldOffset(0)]
			internal double DoubleValue;

			[FieldOffset(0)]
			internal long LongValue;
		}
	}

    [Serializable]
	public struct ProtectedBool : IEquatable<ProtectedBool>, IEquatable<bool>, IComparable<ProtectedBool>, IComparable<bool>, IComparable
	{
		[SerializeField]
		private int _Key;

		[SerializeField]
		private int _Value;

		[SerializeField]
		private bool _IsExistent;

		private ProtectedBool(bool value)
		{
			_Key = 0;
			_Value = 0;
			_IsExistent = true;
			Encrypt(value);
			
		}

		private void Encrypt(bool value)
		{
			_Key = CreateKey();
			_Value = (value ? 211 : 183) ^ _Key;
		}

		private bool Decrypt()
		{
			return (_Value ^ _Key) != 183;
		}

		public static int CreateKey()
		{
			return RandomNumberGenerator.GetIntRandomNumber();
		}

		private bool InbuiltDecrypt()
		{
			if (!_IsExistent)
			{
				Encrypt(false);
				_IsExistent = true;
				return false;
			}

			return Decrypt();
		}

		public static implicit operator ProtectedBool(bool value)
		{
			return new ProtectedBool(value);
		}

		public static implicit operator bool(ProtectedBool value)
		{
			return value.InbuiltDecrypt();
		}

		public override int GetHashCode()
		{
			return InbuiltDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InbuiltDecrypt().ToString();
		}

		public override bool Equals(object obj)
		{
			return obj is ProtectedBool && Equals((ProtectedBool)obj);
		}

		public bool Equals(ProtectedBool obj)
		{
			if (_Key == obj._Key)
			{
				return _Value.Equals(obj._Value);
			}

			return Decrypt().Equals(obj.Decrypt());
		}

		public bool Equals(bool other)
		{
			return Decrypt().Equals(other);
		}

		public int CompareTo(ProtectedBool other)
		{
			return InbuiltDecrypt().CompareTo(other.InbuiltDecrypt());
		}

		public int CompareTo(bool other)
		{
			return InbuiltDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			
			if(!(obj is bool))
				throw new ArgumentException("Argument must be bool.");
			
			return CompareTo((bool)obj);
		}
	}

    [Serializable]
	public struct ProtectedChar : IEquatable<ProtectedChar>, IEquatable<char>, IComparable<ProtectedChar>, IComparable<char>, IComparable
	{
		private char _Key;
		private char _Value;
		private bool _IsExistent;

		private ProtectedChar(char value)
		{
			_Key = '\0';
			_Value = '\0';
			_IsExistent = true;
			Encrypt(value);
		}

		private void Encrypt(char value)
		{
			_Key = CreateKey();
			_Value = (char)(value ^ _Key);
		}

		private char Decrypt()
		{
			return (char)(_Value ^ _Key);
		}

		public static char CreateKey()
		{
			return (char)RandomNumberGenerator.GetIntRandomNumber(10000, 65530);
		}

		private char InbuiltDecrypt()
		{
			if (!_IsExistent)
			{
				Encrypt('\0');
				_IsExistent = true;
				return '\0';
			}

			return Decrypt();
		}

		public static implicit operator ProtectedChar(char value)
		{
			return new ProtectedChar(value);
		}

		public static implicit operator char(ProtectedChar value)
		{
			return value.InbuiltDecrypt();
		}

		public static ProtectedChar operator ++(ProtectedChar input)
		{
			char decryptedValue = (char)(input.InbuiltDecrypt() + 1);
            input.Encrypt(decryptedValue);

            return input;
		}

		public static ProtectedChar operator --(ProtectedChar input)
		{
			char decryptedValue = (char)(input.InbuiltDecrypt() - 1);
            input.Encrypt(decryptedValue);
			
            return input;
		}

		public override int GetHashCode()
		{
			return InbuiltDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InbuiltDecrypt().ToString();
		}

		public string ToString(IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(provider);
		}


		public override bool Equals(object obj)
		{
			return obj is ProtectedChar && Equals((ProtectedChar)obj);
		}

		public bool Equals(ProtectedChar obj)
		{
			if (_Key == obj._Key)
			{
				return _Value.Equals(obj._Value);
			}

			return Decrypt().Equals(obj.Decrypt());
		}

		public bool Equals(char other)
		{
			return Decrypt().Equals(other);
		}

		public int CompareTo(ProtectedChar other)
		{
			return InbuiltDecrypt().CompareTo(other.InbuiltDecrypt());
		}

		public int CompareTo(char other)
		{
			return InbuiltDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;

			if(!(obj is char))
				throw new ArgumentException("Argument must be char.");
			
			return CompareTo((char)obj);
		}
	}

    [Serializable]
	public struct ProtectedShort : IFormattable, IEquatable<ProtectedShort>, IEquatable<short>, IComparable<ProtectedShort>, IComparable<short>, IComparable
	{
		[SerializeField]
		private short _Key;

		[SerializeField]
		private short _Value;

		[SerializeField]
		private bool _IsExistent;

		private ProtectedShort(short value)
		{
			_Key = 0;
			_Value = 0;
			_IsExistent = true;
			Encrypt(value);
        }

        private void Encrypt(short value)
        {
			_Key = CreateKey();
			_Value = (short)(value ^ _Key);
        }

		private short Decrypt()
		{
			return (short)(_Value ^ _Key);
		}

		public static short CreateKey()
		{
			return (short)RandomNumberGenerator.GetIntRandomNumber(10000, short.MaxValue);
		}

		private short InbuiltDecrypt()
		{
			if (!_IsExistent)
			{
				Encrypt(0);
                _IsExistent = true;
                return 0;
			}

			return Decrypt();
		}

		public static implicit operator ProtectedShort(short value)
		{
			return new ProtectedShort(value);
		}

		public static implicit operator short(ProtectedShort value)
		{
			return value.InbuiltDecrypt();
		}

		public static ProtectedShort operator ++(ProtectedShort input)
		{
			short decryptedValue = (short)(input.InbuiltDecrypt() + 1);
			input.Encrypt(decryptedValue);

            return input;
		}

		public static ProtectedShort operator --(ProtectedShort input)
		{
			short decryptedValue = (short)(input.InbuiltDecrypt() - 1);
			input.Encrypt(decryptedValue);

            return input;
		}

		public override int GetHashCode()
		{
			return InbuiltDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InbuiltDecrypt().ToString();
		}

		public string ToString(string format)
		{
			return InbuiltDecrypt().ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(format, provider);
		}

		public override bool Equals(object obj)
		{
			return obj is ProtectedShort && Equals((ProtectedShort)obj);
		}

		public bool Equals(ProtectedShort obj)
		{
			if (_Key == obj._Key)
			{
				return _Value.Equals(obj._Value);
			}

			return Decrypt().Equals(obj.Decrypt());
		}

		public bool Equals(short other)
		{
			return Decrypt().Equals(other);
		}

		public int CompareTo(ProtectedShort other)
		{
			return InbuiltDecrypt().CompareTo(other.InbuiltDecrypt());
		}

		public int CompareTo(short other)
		{
			return InbuiltDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			
			if(!(obj is short))
				throw new ArgumentException("Argument must be short.");
			
			return CompareTo((short)obj);
		}
	}

    [Serializable]
	public sealed class ProtectedString : IEquatable<ProtectedString>, IEquatable<string>, IComparable<ProtectedString>, IComparable<string>, IComparable
	{
		[SerializeField]
		private char[] _Key;
        [SerializeField]
		private static readonly int _KeyLength = 8;
		[SerializeField]
		private char[] _Value;
		[SerializeField]
		private bool _IsExistent;
        public int Length { get { return _Value == null ? 0 : _Value.Length; }}

        // Do not delete!
		private ProtectedString(){}

		private ProtectedString(string value)
		{
			_Key = new char[_KeyLength];
			CreateKey(ref _Key);
			_Value = InbuiltEncryptAndDecrypt(value.ToCharArray(), _Key);
			_IsExistent = true;
		}

		private static void CreateKey(ref char[] key)
		{
            for(int i=0; i<key.Length; i++)
            {
                key[i] = (char)(RandomNumberGenerator.GetIntRandomNumber() % 256);
            }
		}

		private char[] InbuiltEncryptAndDecrypt(char[] value, char[] key)
		{
			if (value == null || value.Length == 0 || key == null || key.Length == 0)
				return value;

            var valueLength = value.Length;

			var result = new char[valueLength];
			for (var i = 0; i < valueLength; i++)
			{
				result[i] = (char)(value[i] ^ key[i % _KeyLength]);
			}

			return result;
		}

		private string InbuiltDecryptToString()
		{
			return new string(InbuiltDecrypt());
		}

		private char[] InbuiltDecrypt()
		{
			if (!_IsExistent)
			{
				_Key = new char[_KeyLength];
				CreateKey(ref _Key);
				_Value = InbuiltEncryptAndDecrypt(new char[0], _Key);
				_IsExistent = true;

				return new char[0];
			}

			return InbuiltEncryptAndDecrypt(_Value, _Key);
		}

		public char this[int index]
		{
			get
			{
				if (index < 0 || index >= Length)
				{
					throw new IndexOutOfRangeException();
				}

				return InbuiltDecrypt()[index];
			}
		}

		public static implicit operator ProtectedString(string value)
		{
			return value == null ? null : new ProtectedString(value);
		}

		public static implicit operator string(ProtectedString value)
		{
			return (object)value == null ? null : value.InbuiltDecryptToString();
		}

		public static bool operator ==(ProtectedString a, ProtectedString b)
		{
			if (ReferenceEquals(a, b))
				return true;
			
			if ((object)a == null || (object)b == null)
				return false;

			if (a._Key == b._Key)
				return ArraysEquals(a._Value, b._Value);

			return ArraysEquals(a.InbuiltDecrypt(), b.InbuiltDecrypt());
		}

		public static bool operator !=(ProtectedString a, ProtectedString b)
		{
			return !(a == b);
		}

		public static bool operator ==(string a, ProtectedString b)
		{			
			if (a == null || (object)b == null)
				return false;

			return ArraysEquals(a.ToCharArray(), b.InbuiltDecrypt());
		}

		public static bool operator !=(string a, ProtectedString b)
		{
			return !(a == b);
		}

		public static bool operator ==(ProtectedString a, string b)
		{			
			if ((object)a == null || b == null)
				return false;

			return ArraysEquals(a.InbuiltDecrypt(), b.ToCharArray());
		}

		public static bool operator !=(ProtectedString a, string b)
		{
			return !(a == b);
		}

		public string Substring(int startIndex)
		{
			return Substring(startIndex, Length - startIndex);
		}

		public string Substring(int startIndex, int length)
		{
			return InbuiltDecryptToString().Substring(startIndex, length);
		}

		public bool StartsWith(string value, StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			return InbuiltDecryptToString().StartsWith(value, comparisonType);
		}

		public bool EndsWith(string value, StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			return InbuiltDecryptToString().EndsWith(value, comparisonType);
		}

		public override int GetHashCode()
		{
			return InbuiltDecryptToString().GetHashCode();
		}

		public override string ToString()
		{
			return new string(InbuiltDecrypt());
		}

		public override bool Equals(object obj)
		{
			var protectedString = obj as ProtectedString;
			return (object)protectedString != null && Equals(protectedString);
		}

		public bool Equals(ProtectedString value)
		{
			if ((object)value == null) return false;

			if (_Key == value._Key)
			{
				return ArraysEquals(_Value, value._Value);
			}

			return ArraysEquals(InbuiltDecrypt(), value.InbuiltDecrypt());
		}

		public bool Equals(string value)
		{
			if (value == null)
				return false;

			return ArraysEquals(InbuiltDecrypt(), value.ToCharArray());
		}

		public bool Equals(ProtectedString value, StringComparison comparisonType)
		{
			return (object)value != null && string.Equals(InbuiltDecryptToString(), value.InbuiltDecryptToString(), comparisonType);
		}

		public int CompareTo(ProtectedString other)
		{
			return InbuiltDecryptToString().CompareTo(other.InbuiltDecryptToString());
		}

		public int CompareTo(string other)
		{
			return InbuiltDecryptToString().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			
			if(!(obj is string))
				throw new ArgumentException("Argument must be string.");
	
			return CompareTo((string)obj);
		}

		private static bool ArraysEquals(char[] a1, char[] a2)
		{
			if (a1 == a2) return true;
			if (a1 == null || a2 == null) return false;
			if (a1.Length != a2.Length) return false;

			for (var i = 0; i < a1.Length; i++)
			{
				if (a1[i] != a2[i])
				{
					return false;
				}
			}
			return true;
		}
	}

	[Serializable]
	public struct ProtectedUInt : IFormattable, IEquatable<ProtectedUInt>, IEquatable<uint>, IComparable<ProtectedUInt>, IComparable<uint>, IComparable
	{
		[SerializeField]
		private uint _Key;

		[SerializeField]
		private uint _Value;

		[SerializeField]
		private bool _IsExistent;

		private ProtectedUInt(uint value)
		{
			_Key = 0U;
			_Value = 0U;
			_IsExistent = true;
			Encrypt(value);
        }

        private void Encrypt(uint value)
        {
			_Key = CreateKey();
			_Value = value ^ _Key;
        }

		private uint Decrypt()
		{
			return _Value ^ _Key;
		}

		public static uint CreateKey()
		{
			return (uint)RandomNumberGenerator.GetIntRandomNumber();
		}

		private uint InbuiltDecrypt()
		{
			if (!_IsExistent)
			{
				Encrypt(0U);
				_IsExistent = true;
				return 0U;
			}

			return Decrypt();
		}

		public static implicit operator ProtectedUInt(uint value)
		{
			return new ProtectedUInt(value);
		}

		public static implicit operator uint(ProtectedUInt value)
		{
			return value.InbuiltDecrypt();
		}

		public static explicit operator ProtectedInt(ProtectedUInt value)
		{
			return (int)value.InbuiltDecrypt();
		}

		public static ProtectedUInt operator ++(ProtectedUInt input)
		{
			uint decryptedValue = input.InbuiltDecrypt() + 1U;
			input.Encrypt(decryptedValue);

            return input;
		}

		public static ProtectedUInt operator --(ProtectedUInt input)
		{
			uint decryptedValue = input.InbuiltDecrypt() - 1U;
			input.Encrypt(decryptedValue);

            return input;
		}

		public override int GetHashCode()
		{
			return InbuiltDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InbuiltDecrypt().ToString();
		}

		public string ToString(string format)
		{
			return InbuiltDecrypt().ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(format, provider);
		}

		public override bool Equals(object obj)
		{
			return obj is ProtectedUInt && Equals((ProtectedUInt)obj);
		}

		public bool Equals(ProtectedUInt obj)
		{
			if (_Key == obj._Key)
			{
				return _Value.Equals(obj._Value);
			}

			return Decrypt().Equals(obj.Decrypt());
		}

		public bool Equals(uint other)
		{
			return Decrypt().Equals(other);
		}

		public int CompareTo(ProtectedUInt other)
		{
			return InbuiltDecrypt().CompareTo(other.InbuiltDecrypt());
		}

		public int CompareTo(uint other)
		{
			return InbuiltDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;

			if(!(obj is uint))
				throw new ArgumentException("Argument must be uint.");
			
			return CompareTo((uint)obj);
		}
	}

	[Serializable]
	public struct ProtectedUShort : IFormattable, IEquatable<ProtectedUShort>,  IEquatable<ushort>, IComparable<ProtectedUShort>, IComparable<ushort>, IComparable
	{
		[SerializeField]
		private ushort _Key;

		[SerializeField]
		private ushort _Value;

		[SerializeField]
		private bool _IsExistent;

		private ProtectedUShort(ushort value)
		{
			_Key = 0;
			_Value = 0;
			_IsExistent = true;
			Encrypt(value);
		}

		private void Encrypt(ushort value)
        {
			_Key = CreateKey();
			_Value = (ushort)(value ^ _Key);
        }

		private ushort Decrypt()
		{
			return (ushort)(_Value ^ _Key);
		}

		public static ushort CreateKey()
		{
			return (ushort)RandomNumberGenerator.GetIntRandomNumber(10000, short.MaxValue);
		}

		private ushort InbuiltDecrypt()
		{
			if (!_IsExistent)
			{
				Encrypt(0);
				_IsExistent = true;
				return 0;
			}

			return Decrypt();
		}

		public static implicit operator ProtectedUShort(ushort value)
		{
			return new ProtectedUShort(value);
		}

		public static implicit operator ushort(ProtectedUShort value)
		{
			return value.InbuiltDecrypt();
		}

		public static ProtectedUShort operator ++(ProtectedUShort input)
		{
			ushort decryptedValue = (ushort)(input.InbuiltDecrypt() + 1);
			input.Encrypt(decryptedValue);

            return input;
		}

		public static ProtectedUShort operator --(ProtectedUShort input)
		{
			ushort decryptedValue = (ushort)(input.InbuiltDecrypt() - 1);
			input.Encrypt(decryptedValue);

            return input;
		}

		public override int GetHashCode()
		{
			return InbuiltDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InbuiltDecrypt().ToString();
		}

		public string ToString(string format)
		{
			return InbuiltDecrypt().ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(format, provider);
		}

		public override bool Equals(object obj)
		{
			return obj is ProtectedUShort && Equals((ProtectedUShort)obj);
		}

		public bool Equals(ProtectedUShort obj)
		{
			if (_Key == obj._Key)
			{
				return _Value.Equals(obj._Value);
			}

			return Decrypt().Equals(obj.Decrypt());
		}

		public bool Equals(ushort other)
		{
			return Decrypt().Equals(other);
		}

		public int CompareTo(ProtectedUShort other)
		{
			return InbuiltDecrypt().CompareTo(other.InbuiltDecrypt());
		}

		public int CompareTo(ushort other)
		{
			return InbuiltDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			
			if(!(obj is ushort))
				throw new ArgumentException("Argument must be ushort.");
			
			return CompareTo((ushort)obj);
		}
	}

	[Serializable]
    public struct ProtectedULong : IFormattable, IEquatable<ProtectedULong>, IEquatable<ulong>, IComparable<ProtectedULong>, IComparable<ulong>, IComparable
    {
        [SerializeField]
        private ulong _Key;

        [SerializeField]
        private ulong _Value;

        [SerializeField]
        private bool _IsExistent;		

        private ProtectedULong(ulong value)
        {
            _Key = 0UL;
			_Value = 0UL;
			_IsExistent = true;
			Encrypt(value);
        }

        private void Encrypt(ulong value)
        {
			_Key = CreateKey();
			_Value = value ^ _Key;
        }

        private ulong Decrypt()
        {
            return _Value ^ _Key;
        }

        private static ulong CreateKey()
        {
            return (ulong)RandomNumberGenerator.GetLongRandomNumber();
        }

        private ulong InbuiltDecrypt()
        {
            if (!_IsExistent)
            {
                Encrypt(0UL);
                _IsExistent = true;
                return 0UL;
            }

            return Decrypt();
        }
        
        public static implicit operator ProtectedULong(ulong value)
        {
            return new ProtectedULong(value);
        }
        
        public static implicit operator ulong(ProtectedULong value)
        {
            return value.InbuiltDecrypt();
        }

        public static ProtectedULong operator ++(ProtectedULong input)
        {
            ulong decryptedValue = input.InbuiltDecrypt() + 1UL;
			input.Encrypt(decryptedValue);

            return input;
        }

        public static ProtectedULong operator --(ProtectedULong input)
        {
            ulong decryptedValue = input.InbuiltDecrypt() - 1UL;
			input.Encrypt(decryptedValue);

            return input;
        }

        public override int GetHashCode()
        {
            return InbuiltDecrypt().GetHashCode();
        }

        public override string ToString()
        {
            return InbuiltDecrypt().ToString();
        }

        public string ToString(string format)
        {
            return InbuiltDecrypt().ToString(format);
        }

        public string ToString(IFormatProvider provider)
        {
            return InbuiltDecrypt().ToString(provider);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return InbuiltDecrypt().ToString(format, provider);
        }

        public override bool Equals(object obj)
        {
            return obj is ProtectedULong && Equals((ProtectedULong)obj);
        }

        public bool Equals(ProtectedULong obj)
        {
            if (_Key == obj._Key)
                return _Value.Equals(obj._Value);

            return Decrypt().Equals(obj.Decrypt());
        }

		public bool Equals(ulong other)
        {
            return Decrypt().Equals(other);
        }

        public int CompareTo(ProtectedULong other)
        {
            return InbuiltDecrypt().CompareTo(other.InbuiltDecrypt());
        }

        public int CompareTo(ulong other)
        {
            return InbuiltDecrypt().CompareTo(other);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
				return 1;
            
			if(!(obj is ulong))
				throw new ArgumentException("Argument must be ulong.");

            return CompareTo((ulong)obj);
        }
    }

	[Serializable]
	public struct ProtectedByte : IFormattable, IEquatable<ProtectedByte>, IEquatable<byte>, IComparable<ProtectedByte>, IComparable<byte>, IComparable
	{
		private byte _Key;
		private byte _Value;
		private bool _IsExistent;

		private ProtectedByte(byte value)
		{
			_Key = 0;
			_Value = 0;
			_IsExistent = true;
			Encrypt(value);
		}

		private void Encrypt(byte value)
        {
			_Key = CreateKey();
			_Value = (byte)(value ^ _Key);
        }

        private byte Decrypt()
        {
            return (byte)(_Value ^ _Key);
        }

		public static byte CreateKey()
		{
			return (byte)RandomNumberGenerator.GetIntRandomNumber(128, 255);
		}

		private byte InbuiltDecrypt()
		{
			if (!_IsExistent)
			{
				Encrypt(0);
				_IsExistent = true;
				return 0;
			}

			return Decrypt();
		}

		public static implicit operator ProtectedByte(byte value)
		{
			return new ProtectedByte(value);
		}

		public static implicit operator byte(ProtectedByte value)
		{
			return value.InbuiltDecrypt();
		}

		public static ProtectedByte operator ++(ProtectedByte input)
		{
            byte decryptedValue = (byte)(input.InbuiltDecrypt() + 1);
			input.Encrypt(decryptedValue);

            return input;
		}

		public static ProtectedByte operator --(ProtectedByte input)
		{
			byte decryptedValue = (byte)(input.InbuiltDecrypt() - 1);
			input.Encrypt(decryptedValue);

            return input;
		}

		public override int GetHashCode()
		{
			return InbuiltDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InbuiltDecrypt().ToString();
		}

		public string ToString(string format)
		{
			return InbuiltDecrypt().ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return InbuiltDecrypt().ToString(format, provider);
		}

		public override bool Equals(object obj)
		{
			return obj is ProtectedByte && Equals((ProtectedByte)obj);
		}

		public bool Equals(ProtectedByte obj)
		{
			if (_Key == obj._Key)
			{
				return _Value.Equals(obj._Value);
			}

			return Decrypt().Equals(obj.Decrypt());
		}

		public bool Equals(byte other)
		{
			return Decrypt().Equals(other);
		}

		public int CompareTo(ProtectedByte other)
		{
			return InbuiltDecrypt().CompareTo(other.InbuiltDecrypt());
		}

		public int CompareTo(byte other)
		{
			return InbuiltDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;
			
			if(!(obj is byte))
				throw new ArgumentException("Argument must be byte.");
			
			return CompareTo((byte)obj);
		}
	}

    internal static class RandomNumberGenerator
    {
        private static System.Random _Random = new System.Random();

        internal static int GetIntRandomNumber(int min=1000000, int max=int.MaxValue)
        {
            return _Random.Next(min, max);
        }

        internal static long GetLongRandomNumber(long min=1000000000000, long max=long.MaxValue)
        {
			long randomNumber = (long)_Random.Next((int)(min >> 32), (int)(max >> 32));
			randomNumber <<= 32; // long의 8바이트 중 앞에 4바이트 랜덤넘버
			randomNumber |= (uint)_Random.Next((int)min, (int)max); //long의 8바이트 중 뒤의 4바이트 랜덤넘버
			return randomNumber;
        }
    }

    /*
    internal interface IProtectedDataType<in T>
    {
        T Encrypt(T value, T key);
    }
    */
}