using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Krafton.SP2.X1.Lib
{
    public class ProtectedDataTypesTest : MonoBehaviour
    {
        void Start()
        {
            //먼가 Generic을 쓰면 편할 것 같은데...
            //랜덤넘버 생성할 때 범위를 예를 들어 int.MinValue ~ int.MaxValue 사이에서 주도록 하는데 이부분이 T.MinValue ~ T.MaxValue 이렇게 안되어서 적용을 못시켜서 답답해요...ㅠ.ㅠ
            if(TestProtectedInt())
                Debug.LogWarning("[Test] ProtectedInt is verified!");
            if(TestProtectedShort())
                Debug.LogWarning("[Test] ProtectedShort is verified!");
            if(TestProtectedLong())
                Debug.LogWarning("[Test] ProtectedLong is verified!");
            if(TestProtectedFloat())
                Debug.LogWarning("[Test] ProtectedFloat is verified!");
            if(TestProtectedDouble())
                Debug.LogWarning("[Test] ProtectedDouble is verified!");
            if(TestProtectedUInt())
                Debug.LogWarning("[Test] ProtectedUInt is verified!");
            if(TestProtectedUShort())
                Debug.LogWarning("[Test] ProtectedUShort is verified!");
            if(TestProtectedULong())
                Debug.LogWarning("[Test] ProtectedULong is verified!");
            if(TestProtectedBool())
                Debug.LogWarning("[Test] ProtectedBool is verified!");
            if(TestProtectedByte())
                Debug.LogWarning("[Test] ProtectedByte is verified!");
            if(TestProtectedString())
                Debug.LogWarning("[Test] ProtectedString is verified!");
        }

        private bool TestProtectedIntListSorting()
        {
            List<int> list = new List<int>();
            List<ProtectedInt> protectedList = new List<ProtectedInt>();

            for(int i=0; i<10; i++)
            {
                int randomNumber = Random.Range(int.MinValue, int.MaxValue);
                ProtectedInt protectedRandomNumber = randomNumber;
                list.Add(randomNumber);
                protectedList.Add(protectedRandomNumber);
            }
            
            list.Sort();
            protectedList.Sort();
            for(int i=0; i<10; i++)
            {
                if(list[i] != protectedList[i])
                    return false; 
            }

            return true;
        }

        private bool TestProtectedInt(int repeatCount=500)
        {
            int errorCount = 0;

            for(int i=0; i<repeatCount; i++)
            {
                int number1 = Random.Range(int.MinValue, int.MaxValue);
                if(i == 0)
                    number1 = int.MaxValue;
                else if(i == 1)
                    number1 = int.MinValue;
                int number2 = Random.Range(int.MinValue, int.MaxValue);

                ProtectedInt protectedNumber1 = number1;
                ProtectedInt protectedNumber2 = number2;
                ProtectedInt protectedNumber3 = protectedNumber1;

                if(protectedNumber1 != protectedNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedInt] == operator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals((object)protectedNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedInt] Equals(object) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedInt] Equals(int) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(protectedNumber3))
                {
                    Debug.LogWarning($"[Error][ProtectedInt] Equals(ProtectedInt) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 + number2 != protectedNumber1 + protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedInt] + opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 - number2 != protectedNumber1 - protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedInt] - opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 * number2 != protectedNumber1 * protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedInt] * opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number2 != 0 && number1 / number2 != protectedNumber1 / protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedInt] / opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                int number3 = protectedNumber1;
                if(number3 != protectedNumber1 || protectedNumber1 != number3)
                {
                    Debug.LogWarning($"[Error][ProtectedInt] ProtectedInt -> int Transfer Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1.GetHashCode() != protectedNumber1.GetHashCode())
                {
                    Debug.LogWarning($"[Error][ProtectedInt] GetHashCode() Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1.ToString() != protectedNumber1.ToString())
                {
                    Debug.LogWarning($"[Error][ProtectedInt] ToString() Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(number1) != number1.CompareTo(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedInt] CompareTo(int) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(protectedNumber1) != number1.CompareTo(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedInt] CompareTo(ProtectedInt) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo((object)number1) != number1.CompareTo((object)number1))
                {
                    Debug.LogWarning($"[Error][ProtectedInt] CompareTo(object) Error : {number1}");
                    errorCount++;
                    continue;
                }

                number1++;
                protectedNumber1++;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedInt] ++ opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                ++number1;
                ++protectedNumber1;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedInt] ++ prefix opperator Error : {number1}");
                    errorCount++;
                    continue;
                }
                
                number1--;
                protectedNumber1--;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedInt] -- opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                --number1;
                --protectedNumber1;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedInt] -- prefix opperator Error : {number1}");
                    errorCount++;
                    continue;
                }
            }

            if(!TestProtectedIntListSorting())
            {
                Debug.LogWarning($"[Error][ProtectedInt] List Sorting Error");
                errorCount++;
            }

            if(errorCount == 0)
                return true;
            else
                return false;
        }

        private bool TestProtectedShortListSorting()
        {
            List<short> list = new List<short>();
            List<ProtectedShort> protectedList = new List<ProtectedShort>();

            for(int i=0; i<10; i++)
            {
                short randomNumber = (short)Random.Range(short.MinValue, short.MaxValue);
                ProtectedShort protectedRandomNumber = randomNumber;
                list.Add(randomNumber);
                protectedList.Add(protectedRandomNumber);
            }
            
            list.Sort();
            protectedList.Sort();
            for(int i=0; i<10; i++)
            {
                if(list[i] != protectedList[i])
                    return false; 
            }

            return true;
        }

        private bool TestProtectedShort(int repeatCount=500)
        {
            int errorCount = 0;

            for(int i=0; i<repeatCount; i++)
            {
                short number1 = (short)Random.Range(short.MinValue, short.MaxValue);
                if(i == 0)
                    number1 = short.MaxValue;
                else if(i == 1)
                    number1 = short.MinValue;
                short number2 = (short)Random.Range(short.MinValue, short.MaxValue);

                ProtectedShort protectedNumber1 = number1;
                ProtectedShort protectedNumber2 = number2;
                ProtectedShort protectedNumber3 = protectedNumber1;

                if(protectedNumber1 != protectedNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedShort] == operator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals((object)protectedNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedShort] Equals(object) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedShort] Equals(short) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(protectedNumber3))
                {
                    Debug.LogWarning($"[Error][ProtectedShort] Equals(ProtectedShort) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 + number2 != protectedNumber1 + protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedShort] + opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 - number2 != protectedNumber1 - protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedShort] - opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 * number2 != protectedNumber1 * protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedShort] * opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number2 != 0 && number1 / number2 != protectedNumber1 / protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedShort] / opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                short number3 = protectedNumber1;
                if(number3 != protectedNumber1 || protectedNumber1 != number3)
                {
                    Debug.LogWarning($"[Error][ProtectedShort] ProtectedShort -> short Transfer Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1.GetHashCode() != protectedNumber1.GetHashCode())
                {
                    Debug.LogWarning($"[Error][ProtectedShort] GetHashCode() Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1.ToString() != protectedNumber1.ToString())
                {
                    Debug.LogWarning($"[Error][ProtectedShort] ToString() Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(number1) != number1.CompareTo(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedShort] CompareTo(short) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(protectedNumber1) != number1.CompareTo(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedShort] CompareTo(ProtectedShort) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo((object)number1) != number1.CompareTo((object)number1))
                {
                    Debug.LogWarning($"[Error][ProtectedShort] CompareTo(object) Error : {number1}");
                    errorCount++;
                    continue;
                }

                number1++;
                protectedNumber1++;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedShort] ++ opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                ++number1;
                ++protectedNumber1;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedShort] ++ prefix opperator Error : {number1}");
                    errorCount++;
                    continue;
                }
                
                number1--;
                protectedNumber1--;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedShort] -- opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                --number1;
                --protectedNumber1;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedShort] -- prefix opperator Error : {number1}");
                    errorCount++;
                    continue;
                }
            }

            if(!TestProtectedShortListSorting())
            {
                Debug.LogWarning($"[Error][ProtectedShort] List Sorting Error");
                errorCount++;
            }

            if(errorCount == 0)
                return true;
            else
                return false;
        }

        private bool TestProtectedLongListSorting()
        {
            List<long> list = new List<long>();
            List<ProtectedLong> protectedList = new List<ProtectedLong>();

            for(int i=0; i<10; i++)
            {
                long randomNumber = (long)Random.Range(int.MinValue, int.MaxValue);
                ProtectedLong protectedRandomNumber = randomNumber;
                list.Add(randomNumber);
                protectedList.Add(protectedRandomNumber);
            }
            
            list.Sort();
            protectedList.Sort();
            for(int i=0; i<10; i++)
            {
                if(list[i] != protectedList[i])
                    return false; 
            }

            return true;
        }

        private long GetLongRandomNumber()
        {
            long number = (long)Random.Range((int)(long.MinValue >> 32), (int)(long.MinValue >> 32));
            number <<= 32;
            number |= (long)Random.Range(int.MinValue, int.MinValue);
            return number;
        }

        private bool TestProtectedLong(int repeatCount=500)
        {
            int errorCount = 0;

            for(int i=0; i<repeatCount; i++)
            {
                long number1 = GetLongRandomNumber();
                if(i == 0)
                    number1 = long.MaxValue;
                else if(i == 1)
                    number1 = long.MinValue;
                long number2 = GetLongRandomNumber();

                ProtectedLong protectedNumber1 = number1;
                ProtectedLong protectedNumber2 = number2;
                ProtectedLong protectedNumber3 = protectedNumber1;

                if(protectedNumber1 != protectedNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedLong] == operator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals((object)protectedNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedLong] Equals(object) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedLong] Equals(long) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(protectedNumber3))
                {
                    Debug.LogWarning($"[Error][ProtectedLong] Equals(ProtectedLong) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 + number2 != protectedNumber1 + protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedLong] + opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 - number2 != protectedNumber1 - protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedLong] - opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 * number2 != protectedNumber1 * protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedLong] * opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number2 != 0 && number1 / number2 != protectedNumber1 / protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedLong] / opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                long number3 = protectedNumber1;
                if(number3 != protectedNumber1 || protectedNumber1 != number3)
                {
                    Debug.LogWarning($"[Error][ProtectedLong] ProtectedLong -> long Transfer Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1.GetHashCode() != protectedNumber1.GetHashCode())
                {
                    Debug.LogWarning($"[Error][ProtectedLong] GetHashCode() Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1.ToString() != protectedNumber1.ToString())
                {
                    Debug.LogWarning($"[Error][ProtectedLong] ToString() Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(number1) != number1.CompareTo(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedLong] CompareTo(long) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(protectedNumber1) != number1.CompareTo(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedLong] CompareTo(ProtectedLong) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo((object)number1) != number1.CompareTo((object)number1))
                {
                    Debug.LogWarning($"[Error][ProtectedLong] CompareTo(object) Error : {number1}");
                    errorCount++;
                    continue;
                }

                number1++;
                protectedNumber1++;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedLong] ++ opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                ++number1;
                ++protectedNumber1;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedLong] ++ prefix opperator Error : {number1}");
                    errorCount++;
                    continue;
                }
                
                number1--;
                protectedNumber1--;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedLong] -- opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                --number1;
                --protectedNumber1;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedLong] -- prefix opperator Error : {number1}");
                    errorCount++;
                    continue;
                }
            }

            if(!TestProtectedLongListSorting())
            {
                Debug.LogWarning($"[Error][ProtectedLong] List Sorting Error");
                errorCount++;
            }

            if(errorCount == 0)
                return true;
            else
                return false;
        }

        private bool TestProtectedUIntListSorting()
        {
            List<uint> list = new List<uint>();
            List<ProtectedUInt> protectedList = new List<ProtectedUInt>();

            for(int i=0; i<10; i++)
            {
                uint randomNumber = (uint)Random.Range(int.MinValue, int.MaxValue);
                ProtectedUInt protectedRandomNumber = randomNumber;
                list.Add(randomNumber);
                protectedList.Add(protectedRandomNumber);
            }
            
            list.Sort();
            protectedList.Sort();
            for(int i=0; i<10; i++)
            {
                if(list[i] != protectedList[i])
                    return false; 
            }

            return true;
        }

        private bool TestProtectedUInt(int repeatCount=500)
        {
            int errorCount = 0;

            for(int i=0; i<repeatCount; i++)
            {
                uint uintNumber1 = (uint)Random.Range(int.MinValue, int.MaxValue);
                if(i == 0)
                    uintNumber1 = uint.MaxValue;
                else if(i == 1)
                    uintNumber1 = uint.MinValue;
                uint uintNumber2 = (uint)Random.Range(int.MinValue, int.MaxValue);

                ProtectedUInt protectedNumber1 = uintNumber1;
                ProtectedUInt protectedNumber2 = uintNumber2;
                ProtectedUInt protectedNumber3 = protectedNumber1;

                if(protectedNumber1 != protectedNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] == operator Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals((object)protectedNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] Equals(object) Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(uintNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] Equals(uint) Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(protectedNumber3))
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] Equals(ProtectedUInt) Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                if(uintNumber1 + uintNumber2 != protectedNumber1 + protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] + opperator Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                if(uintNumber1 - uintNumber2 != protectedNumber1 - protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] - opperator Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                if(uintNumber1 * uintNumber2 != protectedNumber1 * protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] * opperator Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                if(uintNumber2 != 0 && uintNumber1 / uintNumber2 != protectedNumber1 / protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] / opperator Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                uint uintNumber3 = protectedNumber1;
                if(uintNumber3 != protectedNumber1 || protectedNumber1 != uintNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] ProtectedUInt -> uint Transfer Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                if(uintNumber1.GetHashCode() != protectedNumber1.GetHashCode())
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] GetHashCode() Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                if(uintNumber1.ToString() != protectedNumber1.ToString())
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] ToString() Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(uintNumber1) != uintNumber1.CompareTo(uintNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] CompareTo(uint) Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(protectedNumber1) != uintNumber1.CompareTo(uintNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] CompareTo(ProtectedUInt) Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo((object)uintNumber1) != uintNumber1.CompareTo((object)uintNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] CompareTo(object) Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                uintNumber1++;
                protectedNumber1++;
                if(uintNumber1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] ++ opperator Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                ++uintNumber1;
                ++protectedNumber1;
                if(uintNumber1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] ++ prefix opperator Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }
                
                uintNumber1--;
                protectedNumber1--;
                if(uintNumber1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] -- opperator Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }

                --uintNumber1;
                --protectedNumber1;
                if(uintNumber1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedUInt] -- prefix opperator Error : {uintNumber1}");
                    errorCount++;
                    continue;
                }
            }

            if(!TestProtectedUIntListSorting())
            {
                Debug.LogWarning($"[Error][ProtectedUInt] List Sorting Error");
                errorCount++;
            }

            if(errorCount == 0)
                return true;
            else
                return false;
        }

        private bool TestProtectedUShortListSorting()
        {
            List<ushort> list = new List<ushort>();
            List<ProtectedUShort> protectedList = new List<ProtectedUShort>();

            for(int i=0; i<10; i++)
            {
                ushort randomNumber = (ushort)Random.Range(int.MinValue, int.MaxValue);
                ProtectedUShort protectedRandomNumber = randomNumber;
                list.Add(randomNumber);
                protectedList.Add(protectedRandomNumber);
            }
            
            list.Sort();
            protectedList.Sort();
            for(int i=0; i<10; i++)
            {
                if(list[i] != protectedList[i])
                    return false; 
            }

            return true;
        }

        private bool TestProtectedUShort(int repeatCount=500)
        {
            int errorCount = 0;

            for(int i=0; i<repeatCount; i++)
            {
                ushort ushortNumber1 = (ushort)Random.Range(short.MinValue, short.MaxValue);
                if(i == 0)
                    ushortNumber1 = ushort.MaxValue;
                else if(i == 1)
                    ushortNumber1 = ushort.MinValue;
                ushort ushortNumber2 = (ushort)Random.Range(short.MinValue, short.MaxValue);

                ProtectedUShort protectedNumber1 = ushortNumber1;
                ProtectedUShort protectedNumber2 = ushortNumber2;
                ProtectedUShort protectedNumber3 = protectedNumber1;

                if(protectedNumber1 != protectedNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] == operator Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals((object)protectedNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] Equals(object) Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(ushortNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] Equals(ushort) Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(protectedNumber3))
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] Equals(ProtectedUShort) Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                if(ushortNumber1 + ushortNumber2 != protectedNumber1 + protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] + opperator Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                if(ushortNumber1 - ushortNumber2 != protectedNumber1 - protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] - opperator Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                if(ushortNumber1 * ushortNumber2 != protectedNumber1 * protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] * opperator Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                if(ushortNumber2 != 0 && ushortNumber1 / ushortNumber2 != protectedNumber1 / protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] / opperator Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                ushort ushortNumber3 = protectedNumber1;
                if(ushortNumber3 != protectedNumber1 || protectedNumber1 != ushortNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] ProtectedUShort -> ushort Transfer Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                if(ushortNumber1.GetHashCode() != protectedNumber1.GetHashCode())
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] GetHashCode() Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                if(ushortNumber1.ToString() != protectedNumber1.ToString())
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] ToString() Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(ushortNumber1) != ushortNumber1.CompareTo(ushortNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] CompareTo(ushort) Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(protectedNumber1) != ushortNumber1.CompareTo(ushortNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] CompareTo(ProtectedUShort) Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo((object)ushortNumber1) != ushortNumber1.CompareTo((object)ushortNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] CompareTo(object) Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                ushortNumber1++;
                protectedNumber1++;
                if(ushortNumber1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] ++ opperator Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                ++ushortNumber1;
                ++protectedNumber1;
                if(ushortNumber1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] ++ prefix opperator Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }
                
                ushortNumber1--;
                protectedNumber1--;
                if(ushortNumber1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] -- opperator Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }

                --ushortNumber1;
                --protectedNumber1;
                if(ushortNumber1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedUShort] -- prefix opperator Error : {ushortNumber1}");
                    errorCount++;
                    continue;
                }
            }

            if(!TestProtectedUShortListSorting())
            {
                Debug.LogWarning($"[Error][ProtectedUShort] List Sorting Error");
                errorCount++;
            }

            if(errorCount == 0)
                return true;
            else
                return false;
        }

        private bool TestProtectedULongListSorting()
        {
            List<ulong> list = new List<ulong>();
            List<ProtectedULong> protectedList = new List<ProtectedULong>();

            for(int i=0; i<10; i++)
            {
                ulong randomNumber = (ulong)GetLongRandomNumber();
                ProtectedULong protectedRandomNumber = randomNumber;
                list.Add(randomNumber);
                protectedList.Add(protectedRandomNumber);
            }
            
            list.Sort();
            protectedList.Sort();
            for(int i=0; i<10; i++)
            {
                if(list[i] != protectedList[i])
                    return false; 
            }

            return true;
        }

        private bool TestProtectedULong(int repeatCount=500)
        {
            int errorCount = 0;

            for(int i=0; i<repeatCount; i++)
            {
                ulong ulongNumber1 = (ulong)GetLongRandomNumber();
                if(i == 0)
                    ulongNumber1 = ulong.MaxValue;
                else if(i == 1)
                    ulongNumber1 = ulong.MinValue;
                ulong ulongNumber2 = (ulong)GetLongRandomNumber();

                ProtectedULong protectedNumber1 = ulongNumber1;
                ProtectedULong protectedNumber2 = ulongNumber2;
                ProtectedULong protectedNumber3 = protectedNumber1;

                if(protectedNumber1 != protectedNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedULong] == operator Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals((object)protectedNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedULong] Equals(object) Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(ulongNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedULong] Equals(ulong) Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(protectedNumber3))
                {
                    Debug.LogWarning($"[Error][ProtectedULong] Equals(ProtectedULong) Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                if(ulongNumber1 + ulongNumber2 != protectedNumber1 + protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedULong] + opperator Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                if(ulongNumber1 - ulongNumber2 != protectedNumber1 - protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedULong] - opperator Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                if(ulongNumber1 * ulongNumber2 != protectedNumber1 * protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedULong] * opperator Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                if(ulongNumber2 != 0 && ulongNumber1 / ulongNumber2 != protectedNumber1 / protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedULong] / opperator Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                ulong ulongNumber3 = protectedNumber1;
                if(ulongNumber3 != protectedNumber1 || protectedNumber1 != ulongNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedULong] ProtectedULong -> ulong Transfer Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                if(ulongNumber1.GetHashCode() != protectedNumber1.GetHashCode())
                {
                    Debug.LogWarning($"[Error][ProtectedULong] GetHashCode() Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                if(ulongNumber1.ToString() != protectedNumber1.ToString())
                {
                    Debug.LogWarning($"[Error][ProtectedULong] ToString() Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(ulongNumber1) != ulongNumber1.CompareTo(ulongNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedULong] CompareTo(ulong) Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(protectedNumber1) != ulongNumber1.CompareTo(ulongNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedULong] CompareTo(ProtectedULong) Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo((object)ulongNumber1) != ulongNumber1.CompareTo((object)ulongNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedULong] CompareTo(object) Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                ulongNumber1++;
                protectedNumber1++;
                if(ulongNumber1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedULong] ++ opperator Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                ++ulongNumber1;
                ++protectedNumber1;
                if(ulongNumber1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedULong] ++ prefix opperator Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }
                
                ulongNumber1--;
                protectedNumber1--;
                if(ulongNumber1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedULong] -- opperator Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }

                --ulongNumber1;
                --protectedNumber1;
                if(ulongNumber1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedULong] -- prefix opperator Error : {ulongNumber1}");
                    errorCount++;
                    continue;
                }
            }

            if(!TestProtectedULongListSorting())
            {
                Debug.LogWarning($"[Error][ProtectedULong] List Sorting Error");
                errorCount++;
            }

            if(errorCount == 0)
                return true;
            else
                return false;
        }

        private bool TestProtectedFloatListSorting()
        {
            List<float> list = new List<float>();
            List<ProtectedFloat> protectedList = new List<ProtectedFloat>();

            for(int i=0; i<10; i++)
            {
                float randomNumber = Random.Range(float.MinValue, float.MaxValue);
                ProtectedFloat protectedRandomNumber = randomNumber;
                list.Add(randomNumber);
                protectedList.Add(protectedRandomNumber);
            }
            
            list.Sort();
            protectedList.Sort();
            for(int i=0; i<10; i++)
            {
                if(list[i] != protectedList[i])
                    return false; 
            }

            return true;
        }

        private bool TestProtectedFloat(int repeatCount=500)
        {
            int errorCount = 0;

            for(int i=0; i<repeatCount; i++)
            {
                float number1 = Random.Range(float.MinValue, float.MaxValue);
                if(i == 0)
                    number1 = float.MaxValue;
                else if(i == 1)
                    number1 = float.MinValue;
                float number2 = Random.Range(float.MinValue, float.MaxValue);

                ProtectedFloat protectedNumber1 = number1;
                ProtectedFloat protectedNumber2 = number2;
                ProtectedFloat protectedNumber3 = protectedNumber1;

                if(protectedNumber1 != protectedNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] == operator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals((object)protectedNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] Equals(object) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] Equals(float) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(protectedNumber3))
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] Equals(ProtectedFloat) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 + number2 != protectedNumber1 + protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] + opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 - number2 != protectedNumber1 - protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] - opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 * number2 != protectedNumber1 * protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] * opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number2 != 0 && number1 / number2 != protectedNumber1 / protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] / opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                float intNumber3 = protectedNumber1;
                if(intNumber3 != protectedNumber1 || protectedNumber1 != intNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] ProtectedFloat -> float Transfer Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1.GetHashCode() != protectedNumber1.GetHashCode())
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] GetHashCode() Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1.ToString() != protectedNumber1.ToString())
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] ToString() Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(number1) != number1.CompareTo(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] CompareTo(float) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(protectedNumber1) != number1.CompareTo(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] CompareTo(ProtectedFloat) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo((object)number1) != number1.CompareTo((object)number1))
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] CompareTo(object) Error : {number1}");
                    errorCount++;
                    continue;
                }

                number1++;
                protectedNumber1++;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] ++ opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                ++number1;
                ++protectedNumber1;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] ++ prefix opperator Error : {number1}");
                    errorCount++;
                    continue;
                }
                
                number1--;
                protectedNumber1--;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] -- opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                --number1;
                --protectedNumber1;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedFloat] -- prefix opperator Error : {number1}");
                    errorCount++;
                    continue;
                }
            }

            if(!TestProtectedFloatListSorting())
            {
                Debug.LogWarning($"[Error][ProtectedFloat] List Sorting Error");
                errorCount++;
            }

            if(errorCount == 0)
                return true;
            else
                return false;
        }

        private bool TestProtectedDoubleListSorting()
        {
            List<double> list = new List<double>();
            List<ProtectedDouble> protectedList = new List<ProtectedDouble>();

            for(int i=0; i<10; i++)
            {
                double randomNumber = Random.Range(float.MinValue, float.MaxValue);
                ProtectedDouble protectedRandomNumber = randomNumber;
                list.Add(randomNumber);
                protectedList.Add(protectedRandomNumber);
            }
            
            list.Sort();
            protectedList.Sort();
            for(int i=0; i<10; i++)
            {
                if(list[i] != protectedList[i])
                    return false; 
            }

            return true;
        }

        private bool TestProtectedDouble(int repeatCount=500)
        {
            int errorCount = 0;

            for(int i=0; i<repeatCount; i++)
            {
                double number1 = (double)Random.Range(float.MinValue, float.MaxValue);
                if(i == 0)
                    number1 = double.MaxValue;
                else if(i == 1)
                    number1 = double.MinValue;
                double number2 = (double)Random.Range(float.MinValue, float.MaxValue);

                ProtectedDouble protectedNumber1 = number1;
                ProtectedDouble protectedNumber2 = number2;
                ProtectedDouble protectedNumber3 = protectedNumber1;

                if(protectedNumber1 != protectedNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] == operator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals((object)protectedNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] Equals(object) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] Equals(double) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(protectedNumber3))
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] Equals(ProtectedDouble) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 + number2 != protectedNumber1 + protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] + opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 - number2 != protectedNumber1 - protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] - opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 * number2 != protectedNumber1 * protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] * opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number2 != 0 && number1 / number2 != protectedNumber1 / protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] / opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                double intNumber3 = protectedNumber1;
                if(intNumber3 != protectedNumber1 || protectedNumber1 != intNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] ProtectedDouble -> double Transfer Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1.GetHashCode() != protectedNumber1.GetHashCode())
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] GetHashCode() Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1.ToString() != protectedNumber1.ToString())
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] ToString() Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(number1) != number1.CompareTo(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] CompareTo(double) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(protectedNumber1) != number1.CompareTo(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] CompareTo(ProtectedDouble) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo((object)number1) != number1.CompareTo((object)number1))
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] CompareTo(object) Error : {number1}");
                    errorCount++;
                    continue;
                }

                number1++;
                protectedNumber1++;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] ++ opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                ++number1;
                ++protectedNumber1;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] ++ prefix opperator Error : {number1}");
                    errorCount++;
                    continue;
                }
                
                number1--;
                protectedNumber1--;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] -- opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                --number1;
                --protectedNumber1;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedDouble] -- prefix opperator Error : {number1}");
                    errorCount++;
                    continue;
                }
            }

            if(!TestProtectedDoubleListSorting())
            {
                Debug.LogWarning($"[Error][ProtectedDouble] List Sorting Error");
                errorCount++;
            }

            if(errorCount == 0)
                return true;
            else
                return false;
        }

        private bool TestProtectedBool(int repeatCount=50)
        {
            int errorCount = 0;

            for(int i=0; i<repeatCount; i++)
            {
                bool boolean1 = Random.Range(0.0f, 1.0f) < 0.5 ? false : true;
                bool boolean2 = Random.Range(0.0f, 1.0f) < 0.5 ? false : true;

                ProtectedBool protectedBoolean1 = boolean1;
                ProtectedBool protectedBoolean2 = boolean2;
                ProtectedBool protectedBoolean3 = protectedBoolean1;

                if(protectedBoolean1 != protectedBoolean3)
                {
                    Debug.LogWarning($"[Error][ProtectedInt] == operator Error : {boolean1}");
                    errorCount++;
                    continue;
                }

                if(!protectedBoolean1.Equals((object)protectedBoolean1))
                {
                    Debug.LogWarning($"[Error][ProtectedInt] Equals(object) Error : {boolean1}");
                    errorCount++;
                    continue;
                }

                if(!protectedBoolean1.Equals(boolean1))
                {
                    Debug.LogWarning($"[Error][ProtectedInt] Equals(int) Error : {boolean1}");
                    errorCount++;
                    continue;
                }

                if(!protectedBoolean1.Equals(protectedBoolean3))
                {
                    Debug.LogWarning($"[Error][ProtectedInt] Equals(ProtectedInt) Error : {boolean1}");
                    errorCount++;
                    continue;
                }

                bool boolean3 = protectedBoolean1;
                if(boolean3 != protectedBoolean1 || protectedBoolean1 != boolean3)
                {
                    Debug.LogWarning($"[Error][ProtectedInt] ProtectedInt -> int Transfer Error : {boolean1}");
                    errorCount++;
                    continue;
                }

                if(boolean1.GetHashCode() != protectedBoolean1.GetHashCode())
                {
                    Debug.LogWarning($"[Error][ProtectedInt] GetHashCode() Error : {boolean1}");
                    errorCount++;
                    continue;
                }

                if(boolean1.ToString() != protectedBoolean1.ToString())
                {
                    Debug.LogWarning($"[Error][ProtectedInt] ToString() Error : {boolean1}");
                    errorCount++;
                    continue;
                }

                if(protectedBoolean1.CompareTo(boolean1) != boolean1.CompareTo(boolean1))
                {
                    Debug.LogWarning($"[Error][ProtectedInt] CompareTo(int) Error : {boolean1}");
                    errorCount++;
                    continue;
                }

                if(protectedBoolean1.CompareTo(protectedBoolean1) != boolean1.CompareTo(boolean1))
                {
                    Debug.LogWarning($"[Error][ProtectedInt] CompareTo(ProtectedInt) Error : {boolean1}");
                    errorCount++;
                    continue;
                }

                if(protectedBoolean1.CompareTo((object)boolean1) != boolean1.CompareTo((object)boolean1))
                {
                    Debug.LogWarning($"[Error][ProtectedInt] CompareTo(object) Error : {boolean1}");
                    errorCount++;
                    continue;
                }
            }

            if(errorCount == 0)
                return true;
            else
                return false;
        }

        private bool TestProtectedByteListSorting()
        {
            List<byte> list = new List<byte>();
            List<ProtectedByte> protectedList = new List<ProtectedByte>();

            for(int i=0; i<10; i++)
            {
                byte randomNumber = (byte)Random.Range(0, 255);
                ProtectedByte protectedRandomNumber = randomNumber;
                list.Add(randomNumber);
                protectedList.Add(protectedRandomNumber);
            }
            
            list.Sort();
            protectedList.Sort();
            for(int i=0; i<10; i++)
            {
                if(list[i] != protectedList[i])
                    return false; 
            }

            return true;
        }

        private bool TestProtectedByte(int repeatCount=250)
        {
            int errorCount = 0;

            for(int i=0; i<repeatCount; i++)
            {
                byte number1 = (byte)Random.Range(0, 255);
                if(i == 0)
                    number1 = byte.MaxValue;
                else if(i == 1)
                    number1 = byte.MinValue;
                byte number2 = (byte)Random.Range(0, 255);

                ProtectedByte protectedNumber1 = number1;
                ProtectedByte protectedNumber2 = number2;
                ProtectedByte protectedNumber3 = protectedNumber1;

                if(protectedNumber1 != protectedNumber3)
                {
                    Debug.LogWarning($"[Error][ProtectedByte] == operator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals((object)protectedNumber1))
                {
                    Debug.LogWarning($"[Error][ProtectedByte] Equals(object) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedByte] Equals(byte) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(!protectedNumber1.Equals(protectedNumber3))
                {
                    Debug.LogWarning($"[Error][ProtectedByte] Equals(ProtectedByte) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 + number2 != protectedNumber1 + protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedByte] + opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 - number2 != protectedNumber1 - protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedByte] - opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1 * number2 != protectedNumber1 * protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedByte] * opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number2 != 0 && number1 / number2 != protectedNumber1 / protectedNumber2)
                {
                    Debug.LogWarning($"[Error][ProtectedByte] / opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                byte number3 = protectedNumber1;
                if(number3 != protectedNumber1 || protectedNumber1 != number3)
                {
                    Debug.LogWarning($"[Error][ProtectedByte] ProtectedByte -> byte Transfer Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1.GetHashCode() != protectedNumber1.GetHashCode())
                {
                    Debug.LogWarning($"[Error][ProtectedByte] GetHashCode() Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(number1.ToString() != protectedNumber1.ToString())
                {
                    Debug.LogWarning($"[Error][ProtectedByte] ToString() Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(number1) != number1.CompareTo(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedByte] CompareTo(byte) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo(protectedNumber1) != number1.CompareTo(number1))
                {
                    Debug.LogWarning($"[Error][ProtectedByte] CompareTo(ProtectedByte) Error : {number1}");
                    errorCount++;
                    continue;
                }

                if(protectedNumber1.CompareTo((object)number1) != number1.CompareTo((object)number1))
                {
                    Debug.LogWarning($"[Error][ProtectedByte] CompareTo(object) Error : {number1}");
                    errorCount++;
                    continue;
                }

                number1++;
                protectedNumber1++;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedByte] ++ opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                ++number1;
                ++protectedNumber1;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedByte] ++ prefix opperator Error : {number1}");
                    errorCount++;
                    continue;
                }
                
                number1--;
                protectedNumber1--;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedByte] -- opperator Error : {number1}");
                    errorCount++;
                    continue;
                }

                --number1;
                --protectedNumber1;
                if(number1 != protectedNumber1)
                {
                    Debug.LogWarning($"[Error][ProtectedByte] -- prefix opperator Error : {number1}");
                    errorCount++;
                    continue;
                }
            }

            if(!TestProtectedByteListSorting())
            {
                Debug.LogWarning($"[Error][ProtectedByte] List Sorting Error");
                errorCount++;
            }

            if(errorCount == 0)
                return true;
            else
                return false;
        }

        private bool TestProtectedString(int repeatCount=2)
        {
            int errorCount = 0;

            for(int i=0; i<repeatCount; i++)
            {
                string str1 = "abcdefg";
                if(i == 0)
                    str1 = string.Empty;

                string str2 = "ABCDEFG";

                ProtectedString protectedStr1 = str1;
                ProtectedString protectedStr2 = str2;
                ProtectedString protectedStr3 = protectedStr1;

                if(protectedStr1 != protectedStr3)
                {
                    Debug.LogWarning($"[Error][ProtectedString] == operator Error : {str1}");
                    errorCount++;
                    continue;
                }

                if(!protectedStr1.Equals((object)protectedStr1))
                {
                    Debug.LogWarning($"[Error][ProtectedString] Equals(object) Error : {str1}");
                    errorCount++;
                    continue;
                }

                if(!protectedStr1.Equals(str1))
                {
                    Debug.LogWarning($"[Error][ProtectedString] Equals(byte) Error : {str1}");
                    errorCount++;
                    continue;
                }

                if(!protectedStr1.Equals(protectedStr3))
                {
                    Debug.LogWarning($"[Error][ProtectedString] Equals(ProtectedByte) Error : {str1}");
                    errorCount++;
                    continue;
                }

                if(str1 + str2 != protectedStr1 + protectedStr2)
                {
                    Debug.LogWarning($"[Error][ProtectedString] + opperator Error : {str1}");
                    errorCount++;
                    continue;
                }

                string str3 = protectedStr1;
                if(str3 != protectedStr1 || protectedStr1 != str3)
                {
                    Debug.LogWarning($"[Error][ProtectedString] ProtectedString -> string Transfer Error : {str1}");
                    errorCount++;
                    continue;
                }

                if(str1.GetHashCode() != protectedStr1.GetHashCode())
                {
                    Debug.LogWarning($"[Error][ProtectedString] GetHashCode() Error : {str1}");
                    errorCount++;
                    continue;
                }

                if(str1.ToString() != protectedStr1.ToString())
                {
                    Debug.LogWarning($"[Error][ProtectedString] ToString() Error : {str1}");
                    errorCount++;
                    continue;
                }

                if(protectedStr1.CompareTo(str1) != str1.CompareTo(str1))
                {
                    Debug.LogWarning($"[Error][ProtectedString] CompareTo(byte) Error : {str1}");
                    errorCount++;
                    continue;
                }

                if(protectedStr1.CompareTo(protectedStr1) != str1.CompareTo(str1))
                {
                    Debug.LogWarning($"[Error][ProtectedString] CompareTo(ProtectedByte) Error : {str1}");
                    errorCount++;
                    continue;
                }

                if(protectedStr1.CompareTo((object)str1) != str1.CompareTo((object)str1))
                {
                    Debug.LogWarning($"[Error][ProtectedString] CompareTo(object) Error : {str1}");
                    errorCount++;
                    continue;
                }

                if(protectedStr1.Length > 1)
                {
                    if(protectedStr1.Substring(1) != str1.Substring(1))
                    {
                        Debug.LogWarning($"[Error][ProtectedString] Substring(index) Error : {str1}");
                        errorCount++;
                        continue;
                    }
                }
                
                
                if(protectedStr1.StartsWith("a") != str1.StartsWith("a"))
                {
                    Debug.LogWarning($"[Error][ProtectedString] StartsWith() Error : {str1}");
                    errorCount++;
                    continue;
                }

                if(protectedStr1.EndsWith("g") != str1.EndsWith("g"))
                {
                    Debug.LogWarning($"[Error][ProtectedString] EndsWith() Error : {str1}");
                    errorCount++;
                    continue;
                }
            }

            if(errorCount == 0)
                return true;
            else
                return false;
        }
    }
}
