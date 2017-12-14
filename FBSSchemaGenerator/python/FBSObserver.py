# automatically generated by the FlatBuffers compiler, do not modify

# namespace: State

import flatbuffers

class FBSObserver(object):
    __slots__ = ['_tab']

    @classmethod
    def GetRootAsFBSObserver(cls, buf, offset):
        n = flatbuffers.encode.Get(flatbuffers.packer.uoffset, buf, offset)
        x = FBSObserver()
        x.Init(buf, n + offset)
        return x

    # FBSObserver
    def Init(self, buf, pos):
        self._tab = flatbuffers.table.Table(buf, pos)

    # FBSObserver
    def ObserverName(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(4))
        if o != 0:
            return self._tab.String(o + self._tab.Pos)
        return bytes()

    # FBSObserver
    def DataType(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(6))
        if o != 0:
            return self._tab.String(o + self._tab.Pos)
        return bytes()

    # FBSObserver
    def Data(self, j):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(8))
        if o != 0:
            a = self._tab.Vector(o)
            return self._tab.Get(flatbuffers.number_types.Uint8Flags, a + flatbuffers.number_types.UOffsetTFlags.py_type(j * 1))
        return 0

    # FBSObserver
    def DataAsNumpy(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(8))
        if o != 0:
            return self._tab.GetVectorAsNumpy(flatbuffers.number_types.Uint8Flags, o)
        return 0

    # FBSObserver
    def DataLength(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(8))
        if o != 0:
            return self._tab.VectorLen(o)
        return 0

    # FBSObserver
    def Transform(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(10))
        if o != 0:
            x = o + self._tab.Pos
            from .FBSTransform import FBSTransform
            obj = FBSTransform()
            obj.Init(self._tab.Bytes, x)
            return obj
        return None

    # FBSObserver
    def Body(self):
        o = flatbuffers.number_types.UOffsetTFlags.py_type(self._tab.Offset(12))
        if o != 0:
            x = o + self._tab.Pos
            from .FBSBody import FBSBody
            obj = FBSBody()
            obj.Init(self._tab.Bytes, x)
            return obj
        return None

def FBSObserverStart(builder): builder.StartObject(5)
def FBSObserverAddObserverName(builder, observerName): builder.PrependUOffsetTRelativeSlot(0, flatbuffers.number_types.UOffsetTFlags.py_type(observerName), 0)
def FBSObserverAddDataType(builder, dataType): builder.PrependUOffsetTRelativeSlot(1, flatbuffers.number_types.UOffsetTFlags.py_type(dataType), 0)
def FBSObserverAddData(builder, data): builder.PrependUOffsetTRelativeSlot(2, flatbuffers.number_types.UOffsetTFlags.py_type(data), 0)
def FBSObserverStartDataVector(builder, numElems): return builder.StartVector(1, numElems, 1)
def FBSObserverAddTransform(builder, transform): builder.PrependStructSlot(3, flatbuffers.number_types.UOffsetTFlags.py_type(transform), 0)
def FBSObserverAddBody(builder, body): builder.PrependStructSlot(4, flatbuffers.number_types.UOffsetTFlags.py_type(body), 0)
def FBSObserverEnd(builder): return builder.EndObject()