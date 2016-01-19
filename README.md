# Mono.BlueZ
Wrappers &amp; Classes for interfacing with bluetooth via BlueZ 5 from Mono via DBus

Uses a patched dbus-sharp for dbus calls https://github.com/mono/dbus-sharp
NDesk.DBus Documentation: http://www.ndesk.org/DBus_Documentation (but this is very old)

If you're poking around in DBus I highly recommend a tool such as d-feet

BlueZ 5 Documentation: http://git.kernel.org/cgit/bluetooth/bluez.git/tree/doc

Information on converting from BlueZ <5 to 5+
http://www.bluez.org/bluez-5-api-introduction-and-porting-guide/

Currently most of these interfaces work, with the big exception being interfaces that pass a unix file descriptor in/out (aka Profile1.NewConnection).  Doing this requires unix sockets to support socket control messages, thankfully, this is soon to be committed to mono: https://github.com/mono/mono/pull/2097 however I may hack that into my dbus repo so that everything will still run on older mono builds (aka debian)

To use any of the gatt interfaces, you must run bluetoothd with the experimental flag turned on (-E) or they will not be available.
