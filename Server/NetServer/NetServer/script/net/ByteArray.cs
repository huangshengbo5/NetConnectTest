using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetServer.script.net
{
    class ByteArray
    {
            private const int DEFAULT_SIZE = 1024;

    //初始化大小
    private int initSize = 0;

    public byte[] bytes;
    //可读位置
    public int readIndex = 0;
    //可写位置
    public int writeIndex = 0;

    //缓冲区的容量
    private int capacity = 0;

    //缓冲区还可容纳的字节数
    public int remain
    {
        get { return capacity - writeIndex; }
    }

    public int length
    {
        get { return writeIndex - readIndex; }
    }

    public ByteArray(int size = DEFAULT_SIZE)
    {
        bytes =  new byte[size];
        capacity = size;
        initSize = size;
        readIndex = 0;
        writeIndex = 0;
    }

    public ByteArray(byte[] defaultBytes)
    {
        bytes = defaultBytes;
        capacity = defaultBytes.Length;
        initSize = defaultBytes.Length;
        readIndex = 0;
        writeIndex = defaultBytes.Length;
    }

    //重新计算大小
    public void ReSize(int size)
    {
        if (size < length) return;
        if (size > initSize) return;

        int n = 1;
        while (n<size)
        {
            n *= 2;
        }

        capacity = n;
        byte[] newBytes = new byte[capacity];
        Array.Copy(bytes,readIndex,newBytes,0,writeIndex-readIndex);
        bytes = newBytes;
        writeIndex = length;
        readIndex = 0;
    }

    public void CheckAndMoveBytes()
    {
        if (length<8)
        {
            MoveBytes();
        }
    }

    public void MoveBytes()
    {
        if (length>0)
        {
            Array.Copy(bytes,readIndex,bytes,0,length);
        }

        writeIndex = length;
        readIndex = 0;
    }

    //消息写入
    public int Write(byte[] bs, int offset, int count)
    {
        if (remain <count)
        {
            ReSize(length+count);
        }
        Array.Copy(bs,offset,bytes,writeIndex,count);
        writeIndex += count;
        return count;
    }

    //消息读取
    public int Read(byte[] bs, int offset, int count)
    {
        count = Math.Min(count, length);
        Array.Copy(bytes,readIndex,bs,offset,count);
        readIndex += count;
        CheckAndMoveBytes();
        return count;
    }

    public Int16 ReadInt16()
    {
        if (length < 2) return 0;
        Int16 ret = (Int16)((bytes[readIndex+1]<<8)|bytes[readIndex]);
        readIndex += 2;
        CheckAndMoveBytes();
        return ret;
    }

    public Int32 ReadInt32()
    {
        if (length < 4) return 0;
        Int32 ret = (Int32) ((bytes[readIndex + 3] << 24) | (bytes[readIndex + 2] << 16) | (bytes[readIndex + 1] << 8) |
                             bytes[readIndex]);
        readIndex += 4;
        CheckAndMoveBytes();
        return ret;
    }
    }
}
