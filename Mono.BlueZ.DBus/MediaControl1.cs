using System;
using System.Collections.Generic;
using DBus;

namespace Mono.BlueZ.DBus
{
	// on /org/bluez/hciX/dev_XX_XX_XX_XX_XX_XX
	[Interface("org.bluez.MediaControl1")]
	public interface MediaControl1
	{
		void FastForward();
		void Next();
		void Pause();
		void Play();
		void Previous();
		void Rewind();
		void Stop();
		void VolumeDown();
		void VolumeUp();

		bool Connected{get;}
	}
}
