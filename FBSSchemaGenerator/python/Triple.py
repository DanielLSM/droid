# automatically generated by the FlatBuffers compiler, do not modify

# namespace: State

import flatbuffers

class Triple(object):
    __slots__ = ['_tab']

    @classmethod
    def GetRootAsTriple(cls, buf, offset):
        n = flatbuffers.encode.Get(flatbuffers.packer.uoffset, buf, offset)
        x = Triple()
        x.Init(buf, n + offset)
        return x

    # Triple
    def Init(self, buf, pos):
        self._tab = flatbuffers.table.Table(buf, pos)

    # Triple
    def Triple(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(4))
        if o != 0:
            x = o + self._tab.Pos
            from .Vector3 import Vector3
            obj = Vector3()
            obj.Init(self._tab.Bytes, x)
            return obj
        return None

    # Triple
    def XRange(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(6))
        if o != 0:
            x = o + self._tab.Pos
            from .Range import Range
            obj = Range()
            obj.Init(self._tab.Bytes, x)
            return obj
        return None

    # Triple
    def YRange(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(8))
        if o != 0:
            x = o + self._tab.Pos
            from .Range import Range
            obj = Range()
            obj.Init(self._tab.Bytes, x)
            return obj
        return None

    # Triple
    def ZRange(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(10))
        if o != 0:
            x = o + self._tab.Pos
            from .Range import Range
            obj = Range()
            obj.Init(self._tab.Bytes, x)
            return obj
        return None

def TripleStart(builder): builder.StartObject(4)
def TripleAddTriple(builder, triple): builder.PrependStructSlot(0, flatbuffers.number_types.UOffsetTFlags.py_type(triple), 0)
def TripleAddXRange(builder, xRange): builder.PrependStructSlot(1, flatbuffers.number_types.UOffsetTFlags.py_type(xRange), 0)
def TripleAddYRange(builder, yRange): builder.PrependStructSlot(2, flatbuffers.number_types.UOffsetTFlags.py_type(yRange), 0)
def TripleAddZRange(builder, zRange): builder.PrependStructSlot(3, flatbuffers.number_types.UOffsetTFlags.py_type(zRange), 0)
def TripleEnd(builder): return builder.EndObject()
