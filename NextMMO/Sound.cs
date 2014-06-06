using NAudio.Wave;
using NVorbis.NAudioSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace NextMMO
{
	public class Sound
	{
		MemoryStream mem;

		public Sound(System.IO.Stream stream)
		{
			this.mem = new MemoryStream();
			stream.CopyTo(this.mem);
			this.mem.Position = 0;
		}

		public void Play()
		{
			mem.Position = 0;
			var vorbis = new NVorbis.NAudioSupport.VorbisWaveReader(mem);
			var wave = new WaveOut();
			wave.Init(vorbis);
			wave.PlaybackStopped += (s, e) =>
				{
					//wave.Dispose();
					//vorbis.Dispose();
				};
			wave.Play();
		}
	}
}
