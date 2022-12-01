using System;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode.Day16;

public sealed class PacketDecoder : ISolver
{
    string _hex;
    public async ValueTask ExecutePart1(string inputFilePath)
    {
        _hex = await File.ReadAllTextAsync(inputFilePath);
        long sum = Evaluate(p => p.VersionSum());
        LogResults(16, 1, sum.ToString());
        // var input = await File.ReadAllLinesAsync(inputFilePath);
        // var packages = new List<Packet>();
        // var sumVersion = 0;

        // foreach(var hexstring in input)
        // {
        //     var version = BinaryToDecimal(HexToBinary(hexstring).Substring(0,3));
        //     var type = BinaryToDecimal(HexToBinary(hexstring).Substring(3,3));
        //     switch(type)
        //     {
        //         case 4:
        //             var packet = new LitteralPacket(version, type, HexToBinary(hexstring));
        //             sumVersion += packet.Version;
        //             break;
        //         default:
        //             var operatorPacket = new OperatorPacket(version,type,HexToBinary(hexstring));
        //             sumVersion += operatorPacket.Version;
        //             sumVersion += operatorPacket.SubPackets.Sum(sum => sum.Version);
        //             break;
        //     }
        // }

        // Console.WriteLine(sumVersion);
    }

    long Evaluate(Func<BasePacket, long> valueGetter)
    {
        var packetParser = new PacketParser(_hex);
        var packet = packetParser.NextPacket();

        return valueGetter(packet);
    }

    public ValueTask ExecutePart2(string inputFilePath)
    {
        long value = Evaluate(p => p.Value());

        LogResults(16, 2, value.ToString());
        return ValueTask.CompletedTask;
    }

    public void LogResults(int day, int solution, string result)
    {
        Console.WriteLine("Day: " + day.ToString() + "\tSolution: " + solution.ToString() + "\tResult: " + result + "\n");
    }

    public bool ShouldExecute(int day)
    {
        return day==16;
    }

    // private int BinaryToDecimal(string binary)
    // {
    //     return Convert.ToInt32(binary, 2);
    // }

    // private string HexToBinary(string hexstring)
    // {
    //     return String.Join(String.Empty,
    //         hexstring.Select(
    //             c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
    //         )
    //     );
    // }

    // internal abstract class Packet
    // {
    //     protected string message;
    //     public int Version { get; protected set; }
    //     public int Type { get; protected set; }
    //     public virtual int Value { get; private set; }

    //     protected virtual void Process() 
    //     {
    //         this.Value = ProcessLitteral(this.message.Remove(0,6));
    //     }

    //     protected int BinaryToDecimal(string binary)
    //     {
    //         return Convert.ToInt32(binary, 2);
    //     }

    //     protected string HexToBinary(string hexstring)
    //     {
    //         return String.Join(String.Empty,
    //             hexstring.Select(
    //                 c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
    //             )
    //         );
    //     }

    //     protected int ProcessLitteral(string litteral)
    //     {
    //         string number = string.Empty;
    //         bool final = false;
    //         var index = 0;
    //         do
    //         {
    //             final = litteral[index] == '1' ? false : true;
    //             number += litteral.Substring(index+1,4);
    //             index += 5;
    //         } while(!final);
    //         return BinaryToDecimal(number);
    //     }
    // }

    // internal class LitteralPacket : Packet
    // {
    //     public LitteralPacket(int version, int type, string binaryMessage)
    //     {
    //         base.Version = version;
    //         base.Type = type;
    //         base.message = binaryMessage;

    //         Process();
    //     }
    // }

    // internal class OperatorPacket : Packet
    // {
    //     private IList<Packet> _subPackets = new List<Packet>();

    //     public OperatorPacket(int version, int type, string binaryMessage)
    //     {
    //         base.Version = version;
    //         base.Type = type;
    //         base.message = binaryMessage;

    //         Process();
    //     }

    //     public IList<Packet> SubPackets => _subPackets;

    //     public override int Value => 0;

    //     protected override void Process()
    //     {
    //         var message = base.message.Remove(0,6);
    //         if(message[0] == '0')
    //         {
    //             var totalLengthOfBits = base.BinaryToDecimal(message.Substring(1,15));
    //             var payload = message.Substring(16,totalLengthOfBits);
    //             foreach(var p in SplitIntoPackages(payload))
    //             {
    //                 _subPackets.Add(p);
    //             }
    //         }
    //         if(message[0] == '1')
    //         {
    //             var numberOfSubPackets = base.BinaryToDecimal(message.Substring(1,11));
    //             var payload = message.Remove(0,12);
    //             foreach(var p in SplitIntoPackages(payload))
    //             {
    //                 _subPackets.Add(p);
    //             }
    //         }
    //     }

    //     private IList<Packet> SplitIntoPackages(string payload)
    //     {
    //         var list = new List<Packet>();

    //         var packet = string.Empty;
    //         var index = 0;
    //         while(index < payload.Length)
    //         {
    //             if(payload.Length-index < 11) break;
    //             var message = string.Empty;
    //             message += payload.Substring(index,3);
    //             var version = BinaryToDecimal(payload.Substring(index,3));
    //             index += 3;
    //             message += payload.Substring(index,3);
    //             var type = BinaryToDecimal(payload.Substring(index,3));
    //             index += 3;
    //             switch(type)
    //             {
    //                 case 4:
    //                     var final = true;
    //                     do
    //                     {
    //                         final = payload[index] == '1' ? false : true;
    //                         message += payload.Substring(index,5);
    //                         index += 5;
    //                     } while(!final);
    //                     var l = new LitteralPacket(version,type,message);
    //                     list.Add(l);
    //                     break;
    //                 default:
    //                     var lengthTypeId = payload.Substring(index, 1);
    //                     if(lengthTypeId == "0")
    //                     {
    //                         var numberOfBits = BinaryToDecimal(payload.Substring(index+1,15));
    //                     }
    //                     if(lengthTypeId == "1")
    //                     {
    //                         var numberOfPackets = BinaryToDecimal(payload.Substring(index+1,11));
    //                         index += 12;
    //                         var tmp = payload.Remove(0,index);
    //                         foreach(var a in SplitIntoPackages(tmp))
    //                         {
    //                             list.Add(a);
    //                         }
    //                     }
    //                     break;
    //             }
    //         }
    //         return list;
    //     }
    // }

    public abstract class BasePacket
    {
        public int Version { get; private set; }

        public BasePacket(int version)
        {
            Version = version;
        }

        public abstract int Size();
        public abstract int VersionSum();
        public abstract long Value();
    }

    public class LiteralPacket : BasePacket
    {
        public long LiteralValue { get; private set; }

        /// <summary>
        /// Count of bits used to store the literal in the original bitstream
        /// </summary>
        public int LiteralBits { get; private set; }

        public LiteralPacket(int version, long val, int bits)
            : base(version)
        {
            LiteralValue = val;
            LiteralBits = bits;
        }

        /// <summary>
        /// Compute the length of bits that must have been needed to construct this packet
        /// </summary>
        public override int Size()
        {
            const int version = 3;
            const int typeId = 3;

            return version + typeId + LiteralBits;
        }

        public override int VersionSum() => Version;
        public override long Value() => LiteralValue;
    }

    public enum LengthType
    {
        PacketCount,
        BitCount
    }

    public class OperatorPacket : BasePacket
    {
        /// <summary>
        /// Nested packets.
        /// </summary>
        public List<BasePacket> SubPackets { get; private set; }
        public LengthType LengthType { get; private set; }
        public int OperatorType { get; private set; }

        public OperatorPacket(int version, int type, LengthType lengthType, IEnumerable<BasePacket> subs)
            : base(version)
        {
            OperatorType = type;
            LengthType = lengthType;
            SubPackets = subs.ToList();
        }

        /// <summary>
        /// Compute the length of bits that must have been needed to construct this packet
        /// </summary>
        public override int Size()
        {
            const int version = 3;
            const int typeId = 3;
            int size = version + typeId;

            size++; // length type bit

            if (LengthType == LengthType.PacketCount)
                size += 11;
            else
                size += 15;

            int subpackets = SubPackets
                .Select(p => p.Size())
                .Sum();

            size += subpackets;

            return size;
        }

        public override int VersionSum()
        {
            int subs = SubPackets
                .Select(p => p.VersionSum())
                .Sum();

            return subs + Version;
        }

        public override long Value()
        {
            return OperatorType switch
            {
                0 => SubPackets.Select(packet => packet.Value()).Sum(),
                1 => SubPackets.Select(packet => packet.Value()).Product(),
                2 => SubPackets.Min(packet => packet.Value()),
                3 => SubPackets.Max(packet => packet.Value()),
                5 => SubPackets[0].Value() > SubPackets[1].Value() ? 1 : 0,
                6 => SubPackets[0].Value() < SubPackets[1].Value() ? 1 : 0,
                7 => SubPackets[0].Value() == SubPackets[1].Value() ? 1 : 0,
                _ => throw new NotImplementedException($"OperatorPacket with OperatorType {OperatorType}"),
            };
        }
    }

    class PacketParser
    {
        IEnumerable<int> _stream;

        public PacketParser(string hex)
        {
            _stream = new BitStream(hex).Bits();
        }

        public BasePacket NextPacket()
        {
            int version = (int)GetNBitsAsLong(3);
            int type = (int)GetNBitsAsLong(3);

            // operator packet
            if (type != 4)
            {
                (var lengthType, var subs) = GetNextBit() == 0
                    ? (LengthType.BitCount, SubsByBitCount())
                    : (LengthType.PacketCount, SubsByPacketCount());

                return new OperatorPacket(version, type, lengthType, subs);
            }

            // literal value packet
            else
            {
                (long literalValue, int bitcount) = GetLiteral();
                return new LiteralPacket(version, literalValue, bitcount);
            }
        }

        /// <summary>
        /// Extract a numeric literal from the stream by taking 5 bits at a time until we
        /// have the whole number
        /// </summary>
        (long literalValue, int bitcount) GetLiteral()
        {
            bool lastGroup;
            List<int> bits = new();
            int literalbits = 0;

            do
            {
                lastGroup = GetNextBit() == 0;
                bits.AddRange(GetNextNBits(4));
                literalbits += 5;
            } while (!lastGroup);

            return (BitsToLong(bits), literalbits);
        }

        IEnumerable<BasePacket> SubsByPacketCount()
        {
            var packetCount = GetNBitsAsLong(11);

            for (long i = 1; i <= packetCount; i++)
                yield return NextPacket();
        }

        IEnumerable<BasePacket> SubsByBitCount()
        {
            var totalBits = GetNBitsAsLong(15);

            long n = 0;
            while (n < totalBits)
            {
                var next = NextPacket();
                yield return next;
                n += next.Size();
            }
        }

        /// <summary>
        /// The next three methods get one or more bits from the stream and advance the
        /// stream by the same amount
        /// </summary>
        int GetNextBit() => GetNextNBits(1).First();
        long GetNBitsAsLong(int count) => BitsToLong(GetNextNBits(count));

        IEnumerable<int> GetNextNBits(int count)
        {
            var next = _stream.Take(count);
            _stream = _stream.Skip(count);
            return next;
        }

        /// <summary>
        /// Convert a string of bits into a long, most-significant-bit first
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        static long BitsToLong(IEnumerable<int> bits)
        {
            long value = 0;
            long doubler = 1;

            foreach (var bit in bits.Reverse())
            {
                value += doubler * bit;
                doubler *= 2;
            }

            return value;
        }
    }

    /// <summary>
    /// Convert a hex string into a stream of 1s and 0s
    /// </summary>
    class BitStream
    {
        readonly string _hex;

        public BitStream(string hex)
        {
            _hex = hex;
        }

        public IEnumerable<int> Bits()
        {
            return _hex
                .ToCharArray()
                .SelectMany(HexToBits)
                .Select(c => c == '1' ? 1 : 0);
        }

        public string HexToBits(char hex) => hex switch
        {
            '0' => "0000",
            '1' => "0001",
            '2' => "0010",
            '3' => "0011",
            '4' => "0100",
            '5' => "0101",
            '6' => "0110",
            '7' => "0111",
            '8' => "1000",
            '9' => "1001",
            'A' => "1010",
            'B' => "1011",
            'C' => "1100",
            'D' => "1101",
            'E' => "1110",
            'F' => "1111",
            _ => throw new ArgumentOutOfRangeException($"hex {hex}")
        };
    }
}

public static class Extensions
{
    /// <summary>
    /// Return the product of the numbers in the passed IEnumerable&lt;int&gt;
    /// </summary>
    /// <param name="factors"></param>
    public static long Product(this IEnumerable<int> factors)
    {
        long product = 1;

        foreach (int factor in factors)
            product *= factor;

        return product;
    }

    /// <summary>
    /// Return the product of the numbers in the passed IEnumerable&lt;long&gt;
    /// </summary>
    /// <param name="factors"></param>
    public static long Product(this IEnumerable<long> factors)
    {
        long product = 1;

        foreach (long factor in factors)
            product *= factor;

        return product;
    }
}