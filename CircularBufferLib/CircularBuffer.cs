using System;

namespace CircularBufferLib;

public class CircularBuffer<T>
{
    private readonly T[] _buffer;
    private readonly int _capacity;
    private readonly int _idxMask;
    private int _readIdx;
    private object _lock = new();
    private int _writeIdx;
    private AutoResetEvent _addEvent = new AutoResetEvent(false);
    private AutoResetEvent _getEvent = new AutoResetEvent(false);
    private bool _addComplete;
    private bool _getComplete;

    public CircularBuffer(int minCapacity){
        _idxMask = Helper.CalculateCapacity(minCapacity);
        _capacity = _idxMask + 1;
        _buffer = new T[_capacity];
    }

    private bool IsBufferEmpty {
        get {
            lock(_lock) {
                return _readIdx == _writeIdx;
            }
        }
    }

    private bool IsBufferFull {
        get {
            lock(_lock) {
                var _nextWriteIdx = Helper.IncrementCounterWithMask(_writeIdx, _idxMask);
                return _readIdx == _nextWriteIdx;
            }
        }
    }

    public bool IsAddComplete => _addComplete;

    public bool IsGetComplete => _getComplete;

    public void Add(T item){
        int to = 5;
        while(IsBufferFull){
            _getEvent.WaitOne(to);
            to *= 2;
        }

        lock(_lock){
            _buffer[_writeIdx] = item;
            _writeIdx = Helper.IncrementCounterWithMask(_writeIdx, _idxMask);
        }
        _addEvent.Set();
    }

    public void AddComplete(){
        _addComplete = true;
    }

    public void GetComplete(){
        _getComplete = true;
    }

    public T Get(){
        int to = 5;
        while(IsBufferEmpty && !_addComplete){
            _addEvent.WaitOne(to);
            to *= 2;
        }

        if(_addComplete){ return default;}
        
        T retVal;
        lock(_lock){
            retVal = _buffer[_readIdx];
            _readIdx = Helper.IncrementCounterWithMask(_readIdx, _idxMask);
        }
        _getEvent.Set();
        return retVal;
    }
}
