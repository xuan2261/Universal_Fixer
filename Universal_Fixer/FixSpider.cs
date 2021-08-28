namespace Universal_Fixer
{
	public class FixSpider
	{
		public string result = "";

		public void PatchWin32Resources(ByteBuffer resources)
		{
			PatchResourceDirectoryTable(resources);
		}

		private void PatchResourceDirectoryTable(ByteBuffer resources)
		{
			resources.Advance(12);
			int position = resources.position;
			int num = resources.ReadUInt16() + resources.ReadUInt16();
			int i = 0;
			try
			{
				for (i = 0; i < num; i++)
				{
					PatchResourceDirectoryEntry(resources);
				}
			}
			catch
			{
				resources.position = position + 2;
				result += ".Net Spider fix:";
				result = result + "\r\nIn .rsrc at position 0x0" + resources.position.ToString("X") + "set Nrofentries from " + num.ToString("X") + " to " + i.ToString("X") + "\r\n";
				resources.WriteUInt16((ushort)i);
			}
		}

		private void PatchResourceDirectoryEntry(ByteBuffer resources)
		{
			resources.Advance(4);
			uint num = resources.ReadUInt32();
			int position = resources.position;
			resources.position = (int)(num & 0x7FFFFFFF);
			if ((num & 0x80000000u) != 0)
			{
				PatchResourceDirectoryTable(resources);
			}
			else
			{
				PatchResourceDataEntry(resources);
			}
			resources.position = position;
		}

		private void PatchResourceDataEntry(ByteBuffer resources)
		{
			uint num = resources.ReadUInt32();
		}
	}
}
