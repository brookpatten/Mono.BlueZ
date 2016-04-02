# Mono.BlueZ

[![Build Status](https://travis-ci.org/brookpatten/Mono.BlueZ.svg?branch=master)](https://travis-ci.org/brookpatten/Mono.BlueZ)

Wrappers &amp; Classes for interfacing with bluetooth via BlueZ 5 from Mono via DBus

Uses a patched dbus-sharp for dbus calls that fixes properties, as well as allowing receive for unix file descriptors via socket control messages.  Support for socket control messages come from using a patched mono build.....  so yeah, this is a pain to set up right now, hopefully once these changes get packaged up for mono and dbus-sharp this will be much easier to use.

NDesk.DBus Documentation: http://www.ndesk.org/DBus_Documentation (but this is very old)

Mono patch to support socket control messages: https://github.com/mono/mono/pull/2097

If you're poking around in DBus I highly recommend a tool such as d-feet, and watch syslog filtered by "bluetooth" to see what kind of errors bluez is spitting out.

BlueZ 5 Documentation: http://git.kernel.org/cgit/bluetooth/bluez.git/tree/doc

Information on converting from BlueZ <5 to 5+
http://www.bluez.org/bluez-5-api-introduction-and-porting-guide/

To use any of the gatt interfaces, you must run bluetoothd with the experimental flag turned on (-E) or they will not be available.
