# automatically generated by the FlatBuffers compiler, do not modify

# namespace: FBS

import flatbuffers

class FBSBody(object):
    __slots__ = ['_tab']

    # FBSBody
    def Init(self, buf, pos):
        self._tab = flatbuffers.table.Table(buf, pos)

    # FBSBody
    def Velocity(self, obj):
        obj.Init(self._tab.Bytes, self._tab.Pos + 0)
        return obj

    # FBSBody
    def AngularVelocity(self, obj):
        obj.Init(self._tab.Bytes, self._tab.Pos + 24)
        return obj


def CreateFBSBody(builder, velocity_x, velocity_y, velocity_z, angular_velocity_x, angular_velocity_y, angular_velocity_z):
    builder.Prep(8, 48)
    builder.Prep(8, 24)
    builder.PrependFloat64(angular_velocity_z)
    builder.PrependFloat64(angular_velocity_y)
    builder.PrependFloat64(angular_velocity_x)
    builder.Prep(8, 24)
    builder.PrependFloat64(velocity_z)
    builder.PrependFloat64(velocity_y)
    builder.PrependFloat64(velocity_x)
    return builder.Offset()
