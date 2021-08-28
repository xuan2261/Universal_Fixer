using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Universal_Fixer
{
	public class MetadataReader
	{
		public enum Types
		{
			Module = 0,
			TypeRef = 1,
			TypeDef = 2,
			FieldPtr = 3,
			Field = 4,
			MethodPtr = 5,
			Method = 6,
			ParamPtr = 7,
			Param = 8,
			InterfaceImpl = 9,
			MemberRef = 10,
			Constant = 11,
			CustomAttribute = 12,
			FieldMarshal = 13,
			Permission = 14,
			ClassLayout = 0xF,
			FieldLayout = 0x10,
			StandAloneSig = 17,
			EventMap = 18,
			EventPtr = 19,
			Event = 20,
			PropertyMap = 21,
			PropertyPtr = 22,
			Property = 23,
			MethodSemantics = 24,
			MethodImpl = 25,
			ModuleRef = 26,
			TypeSpec = 27,
			ImplMap = 28,
			FieldRVA = 29,
			ENCLog = 30,
			ENCMap = 0x1F,
			Assembly = 0x20,
			AssemblyProcessor = 33,
			AssemblyOS = 34,
			AssemblyRef = 35,
			AssemblyRefProcessor = 36,
			AssemblyRefOS = 37,
			File = 38,
			ExportedType = 39,
			ManifestResource = 40,
			NestedClass = 41,
			TypeTyPar = 42,
			MethodTyPar = 43,
			TypeDefOrRef = 0x40,
			HasConstant = 65,
			CustomAttributeType = 66,
			HasSemantic = 67,
			ResolutionScope = 68,
			HasFieldMarshal = 69,
			HasDeclSecurity = 70,
			MemberRefParent = 71,
			MethodDefOrRef = 72,
			MemberForwarded = 73,
			Implementation = 74,
			HasCustomAttribute = 75,
			UInt16 = 97,
			UInt32 = 99,
			String = 101,
			Blob = 102,
			Guid = 103,
			UserString = 112
		}

		public struct IMAGE_DOS_HEADER
		{
			public short e_magic;

			public short e_cblp;

			public short e_cp;

			public short e_crlc;

			public short e_cparhdr;

			public short e_minalloc;

			public short e_maxalloc;

			public short e_ss;

			public short e_sp;

			public short e_csum;

			public short e_ip;

			public short e_cs;

			public short e_lfarlc;

			public short e_ovno;

			private unsafe fixed short e_res1[4];

			public short e_oeminfo;

			private unsafe fixed short e_res2[10];

			public int e_lfanew;
		}

		public struct IMAGE_NT_HEADERS
		{
			public int Signature;

			public IMAGE_FILE_HEADER ifh;

			public IMAGE_OPTIONAL_HEADER ioh;
		}

		public struct IMAGE_DATA_DIRECTORY
		{
			public int RVA;

			public int Size;
		}

		public struct IMAGE_FILE_HEADER
		{
			public short Machine;

			public short NumberOfSections;

			public int TimeDateStamp;

			public int PointerToSymbolTable;

			public int NumberOfSymbols;

			public short SizeOfOptionalHeader;

			public short Characteristics;
		}

		public struct IMAGE_OPTIONAL_HEADER
		{
			public short Magic;

			public byte MajorLinkerVersion;

			public byte MinorLinkerVersion;

			public int SizeOfCode;

			public int SizeOfInitializedData;

			public int SizeOfUninitializedData;

			public int AddressOfEntryPoint;

			public int BaseOfCode;

			public int BaseOfData;

			public int ImageBase;

			public int SectionAlignment;

			public int FileAlignment;

			public short MajorOperatingSystemVersion;

			public short MinorOperatingSystemVersion;

			public short MajorImageVersion;

			public short MinorImageVersion;

			public short MajorSubsystemVersion;

			public short MinorSubsystemVersion;

			public int Win32VersionValue;

			public int SizeOfImage;

			public int SizeOfHeaders;

			public int CheckSum;

			public short Subsystem;

			public short DllCharacteristics;

			public int SizeOfStackReserve;

			public int SizeOfStackCommit;

			public int SizeOfHeapReserve;

			public int SizeOfHeapCommit;

			public int LoaderFlags;

			public int NumberOfRvaAndSizes;

			public IMAGE_DATA_DIRECTORY ExportDirectory;

			public IMAGE_DATA_DIRECTORY ImportDirectory;

			public IMAGE_DATA_DIRECTORY ResourceDirectory;

			public IMAGE_DATA_DIRECTORY ExceptionDirectory;

			public IMAGE_DATA_DIRECTORY SecurityDirectory;

			public IMAGE_DATA_DIRECTORY RelocationDirectory;

			public IMAGE_DATA_DIRECTORY DebugDirectory;

			public IMAGE_DATA_DIRECTORY ArchitectureDirectory;

			public IMAGE_DATA_DIRECTORY Reserved;

			public IMAGE_DATA_DIRECTORY TLSDirectory;

			public IMAGE_DATA_DIRECTORY ConfigurationDirectory;

			public IMAGE_DATA_DIRECTORY BoundImportDirectory;

			public IMAGE_DATA_DIRECTORY ImportAddressTableDirectory;

			public IMAGE_DATA_DIRECTORY DelayImportDirectory;

			public IMAGE_DATA_DIRECTORY MetaDataDirectory;
		}

		public struct image_section_header
		{
			public unsafe fixed byte name[8];

			public int virtual_size;

			public int virtual_address;

			public int size_of_raw_data;

			public int pointer_to_raw_data;

			public int pointer_to_relocations;

			public int pointer_to_linenumbers;

			public short number_of_relocations;

			public short number_of_linenumbers;

			public int characteristics;
		}

		public struct NETDirectory
		{
			public int cb;

			public short MajorRuntimeVersion;

			public short MinorRuntimeVersion;

			public int MetaDataRVA;

			public int MetaDataSize;

			public int Flags;

			public int EntryPointToken;

			public int ResourceRVA;

			public int ResourceSize;

			public int StrongNameSignatureRVA;

			public int StrongNameSignatureSize;

			public int CodeManagerTableRVA;

			public int CodeManagerTableSize;

			public int VTableFixupsRVA;

			public int VTableFixupsSize;

			public int ExportAddressTableJumpsRVA;

			public int ExportAddressTableJumpsSize;

			public int ManagedNativeHeaderRVA;

			public int ManagedNativeHeaderSize;
		}

		public struct MetaDataHeader
		{
			public int Signature;

			public short MajorVersion;

			public short MinorVersion;

			public int Reserved;

			public int VerionLenght;

			public byte[] VersionString;

			public short Flags;

			public short NumberOfStreams;
		}

		public struct TableHeader
		{
			public int Reserved_1;

			public byte MajorVersion;

			public byte MinorVersion;

			public byte HeapOffsetSizes;

			public byte Reserved_2;

			public long MaskValid;

			public long MaskSorted;
		}

		public struct MetaDataStream
		{
			public int Offset;

			public int Size;

			public string Name;

			public int headerpos;

			public int Sizeofheader;
		}

		public struct TableInfo
		{
			public string Name;

			public string[] names;

			public Types type;

			public Types[] ctypes;
		}

		public struct RefTableInfo
		{
			public Types type;

			public Types[] reftypes;

			public int[] refindex;
		}

		public struct TableSize
		{
			public int TotalSize;

			public int[] Sizes;
		}

		public struct Table
		{
			public long[][] members;
		}

		public IMAGE_DOS_HEADER idh;

		public IMAGE_NT_HEADERS inh;

		public image_section_header[] sections;

		public NETDirectory netdir;

		public MetaDataHeader mh;

		public MetaDataStream[] streams;

		public MetaDataStream MetadataRoot;

		public MetaDataStream StringsStream;

		public byte[] Strings;

		public byte[] US;

		public byte[] GUID;

		public byte[] Blob;

		public long TablesOffset;

		public TableHeader tableheader;

		public int[] TableLengths;

		public TableInfo[] tablesinfo;

		public RefTableInfo[] reftables;

		public TableSize[] tablesize;

		public int[] codedTokenBits;

		public Table[] tables;

		public long BlobOffset;

		public long BlobSize;

		public long StringOffset;

		public void InitTablesInfo()
		{
			tablesinfo = new TableInfo[45];
			tablesinfo[0].Name = "Module";
			tablesinfo[0].names = new string[5] { "Generation", "Name", "Mvid", "EncId", "EncBaseId" };
			tablesinfo[0].type = Types.Module;
			tablesinfo[0].ctypes = new Types[5]
			{
				Types.UInt16,
				Types.String,
				Types.Guid,
				Types.Guid,
				Types.Guid
			};
			tablesinfo[1].Name = "TypeRef";
			tablesinfo[1].names = new string[3] { "ResolutionScope", "Name", "Namespace" };
			tablesinfo[1].type = Types.TypeRef;
			tablesinfo[1].ctypes = new Types[3]
			{
				Types.ResolutionScope,
				Types.String,
				Types.String
			};
			tablesinfo[2].Name = "TypeDef";
			tablesinfo[2].names = new string[6] { "Flags", "Name", "Namespace", "Extends", "FieldList", "MethodList" };
			tablesinfo[2].type = Types.TypeDef;
			tablesinfo[2].ctypes = new Types[6]
			{
				Types.UInt32,
				Types.String,
				Types.String,
				Types.TypeDefOrRef,
				Types.Field,
				Types.Method
			};
			tablesinfo[3].Name = "FieldPtr";
			tablesinfo[3].names = new string[1] { "Field" };
			tablesinfo[3].type = Types.FieldPtr;
			tablesinfo[3].ctypes = new Types[1] { Types.Field };
			tablesinfo[4].Name = "Field";
			tablesinfo[4].names = new string[3] { "Flags", "Name", "Signature" };
			tablesinfo[4].type = Types.Field;
			tablesinfo[4].ctypes = new Types[3]
			{
				Types.UInt16,
				Types.String,
				Types.Blob
			};
			tablesinfo[5].Name = "MethodPtr";
			tablesinfo[5].names = new string[1] { "Method" };
			tablesinfo[5].type = Types.MethodPtr;
			tablesinfo[5].ctypes = new Types[1] { Types.Method };
			tablesinfo[6].Name = "Method";
			tablesinfo[6].names = new string[6] { "RVA", "ImplFlags", "Flags", "Name", "Signature", "ParamList" };
			tablesinfo[6].type = Types.Method;
			tablesinfo[6].ctypes = new Types[6]
			{
				Types.UInt32,
				Types.UInt16,
				Types.UInt16,
				Types.String,
				Types.Blob,
				Types.Param
			};
			tablesinfo[7].Name = "ParamPtr";
			tablesinfo[7].names = new string[1] { "Param" };
			tablesinfo[7].type = Types.ParamPtr;
			tablesinfo[7].ctypes = new Types[1] { Types.Param };
			tablesinfo[8].Name = "Param";
			tablesinfo[8].names = new string[3] { "Flags", "Sequence", "Name" };
			tablesinfo[8].type = Types.Param;
			tablesinfo[8].ctypes = new Types[3]
			{
				Types.UInt16,
				Types.UInt16,
				Types.String
			};
			tablesinfo[9].Name = "InterfaceImpl";
			tablesinfo[9].names = new string[2] { "Class", "Interface" };
			tablesinfo[9].type = Types.InterfaceImpl;
			tablesinfo[9].ctypes = new Types[2]
			{
				Types.TypeDef,
				Types.TypeDefOrRef
			};
			tablesinfo[10].Name = "MemberRef";
			tablesinfo[10].names = new string[3] { "Class", "Name", "Signature" };
			tablesinfo[10].type = Types.MemberRef;
			tablesinfo[10].ctypes = new Types[3]
			{
				Types.MemberRefParent,
				Types.String,
				Types.Blob
			};
			tablesinfo[11].Name = "Constant";
			tablesinfo[11].names = new string[3] { "Type", "Parent", "Value" };
			tablesinfo[11].type = Types.Constant;
			tablesinfo[11].ctypes = new Types[3]
			{
				Types.UInt16,
				Types.HasConstant,
				Types.Blob
			};
			tablesinfo[12].Name = "CustomAttribute";
			tablesinfo[12].names = new string[3] { "Type", "Parent", "Value" };
			tablesinfo[12].type = Types.CustomAttribute;
			tablesinfo[12].ctypes = new Types[3]
			{
				Types.HasCustomAttribute,
				Types.CustomAttributeType,
				Types.Blob
			};
			tablesinfo[13].Name = "FieldMarshal";
			tablesinfo[13].names = new string[2] { "Parent", "Native" };
			tablesinfo[13].type = Types.FieldMarshal;
			tablesinfo[13].ctypes = new Types[2]
			{
				Types.HasFieldMarshal,
				Types.Blob
			};
			tablesinfo[14].Name = "Permission";
			tablesinfo[14].names = new string[3] { "Action", "Parent", "PermissionSet" };
			tablesinfo[14].type = Types.Permission;
			tablesinfo[14].ctypes = new Types[3]
			{
				Types.UInt16,
				Types.HasDeclSecurity,
				Types.Blob
			};
			tablesinfo[15].Name = "ClassLayout";
			tablesinfo[15].names = new string[3] { "PackingSize", "ClassSize", "Parent" };
			tablesinfo[15].type = Types.ClassLayout;
			tablesinfo[15].ctypes = new Types[3]
			{
				Types.UInt16,
				Types.UInt32,
				Types.TypeDef
			};
			tablesinfo[16].Name = "FieldLayout";
			tablesinfo[16].names = new string[2] { "Offset", "Field" };
			tablesinfo[16].type = Types.FieldLayout;
			tablesinfo[16].ctypes = new Types[2]
			{
				Types.UInt32,
				Types.Field
			};
			tablesinfo[17].Name = "StandAloneSig";
			tablesinfo[17].names = new string[1] { "Signature" };
			tablesinfo[17].type = Types.StandAloneSig;
			tablesinfo[17].ctypes = new Types[1] { Types.Blob };
			tablesinfo[18].Name = "EventMap";
			tablesinfo[18].names = new string[2] { "Parent", "EventList" };
			tablesinfo[18].type = Types.EventMap;
			tablesinfo[18].ctypes = new Types[2]
			{
				Types.TypeDef,
				Types.Event
			};
			tablesinfo[19].Name = "EventPtr";
			tablesinfo[19].names = new string[1] { "Event" };
			tablesinfo[19].type = Types.EventPtr;
			tablesinfo[19].ctypes = new Types[1] { Types.Event };
			tablesinfo[20].Name = "Event";
			tablesinfo[20].names = new string[3] { "EventFlags", "Name", "EventType" };
			tablesinfo[20].type = Types.Event;
			tablesinfo[20].ctypes = new Types[3]
			{
				Types.UInt16,
				Types.String,
				Types.TypeDefOrRef
			};
			tablesinfo[21].Name = "PropertyMap";
			tablesinfo[21].names = new string[2] { "Parent", "PropertyList" };
			tablesinfo[21].type = Types.PropertyMap;
			tablesinfo[21].ctypes = new Types[2]
			{
				Types.TypeDef,
				Types.Property
			};
			tablesinfo[22].Name = "PropertyPtr";
			tablesinfo[22].names = new string[1] { "Property" };
			tablesinfo[22].type = Types.PropertyPtr;
			tablesinfo[22].ctypes = new Types[1] { Types.Property };
			tablesinfo[23].Name = "Property";
			tablesinfo[23].names = new string[3] { "PropFlags", "Name", "Type" };
			tablesinfo[23].type = Types.Property;
			tablesinfo[23].ctypes = new Types[3]
			{
				Types.UInt16,
				Types.String,
				Types.Blob
			};
			tablesinfo[24].Name = "MethodSemantics";
			tablesinfo[24].names = new string[3] { "Semantic", "Method", "Association" };
			tablesinfo[24].type = Types.MethodSemantics;
			tablesinfo[24].ctypes = new Types[3]
			{
				Types.UInt16,
				Types.Method,
				Types.HasSemantic
			};
			tablesinfo[25].Name = "MethodImpl";
			tablesinfo[25].names = new string[3] { "Class", "MethodBody", "MethodDeclaration" };
			tablesinfo[25].type = Types.MethodImpl;
			tablesinfo[25].ctypes = new Types[3]
			{
				Types.TypeDef,
				Types.MethodDefOrRef,
				Types.MethodDefOrRef
			};
			tablesinfo[26].Name = "ModuleRef";
			tablesinfo[26].names = new string[1] { "Name" };
			tablesinfo[26].type = Types.ModuleRef;
			tablesinfo[26].ctypes = new Types[1] { Types.String };
			tablesinfo[27].Name = "TypeSpec";
			tablesinfo[27].names = new string[1] { "Signature" };
			tablesinfo[27].type = Types.TypeSpec;
			tablesinfo[27].ctypes = new Types[1] { Types.Blob };
			tablesinfo[28].Name = "ImplMap";
			tablesinfo[28].names = new string[4] { "MappingFlags", "MemberForwarded", "ImportName", "ImportScope" };
			tablesinfo[28].type = Types.ImplMap;
			tablesinfo[28].ctypes = new Types[4]
			{
				Types.UInt16,
				Types.MemberForwarded,
				Types.String,
				Types.ModuleRef
			};
			tablesinfo[29].Name = "FieldRVA";
			tablesinfo[29].names = new string[2] { "RVA", "Field" };
			tablesinfo[29].type = Types.FieldRVA;
			tablesinfo[29].ctypes = new Types[2]
			{
				Types.UInt32,
				Types.Field
			};
			tablesinfo[30].Name = "ENCLog";
			tablesinfo[30].names = new string[2] { "Token", "FuncCode" };
			tablesinfo[30].type = Types.ENCLog;
			tablesinfo[30].ctypes = new Types[2]
			{
				Types.UInt32,
				Types.UInt32
			};
			tablesinfo[31].Name = "ENCMap";
			tablesinfo[31].names = new string[1] { "Token" };
			tablesinfo[31].type = Types.ENCMap;
			tablesinfo[31].ctypes = new Types[1] { Types.UInt32 };
			tablesinfo[32].Name = "Assembly";
			tablesinfo[32].names = new string[9] { "HashAlgId", "MajorVersion", "MinorVersion", "BuildNumber", "RevisionNumber", "Flags", "PublicKey", "Name", "Locale" };
			tablesinfo[32].type = Types.Assembly;
			tablesinfo[32].ctypes = new Types[9]
			{
				Types.UInt32,
				Types.UInt16,
				Types.UInt16,
				Types.UInt16,
				Types.UInt16,
				Types.UInt32,
				Types.Blob,
				Types.String,
				Types.String
			};
			tablesinfo[33].Name = "AssemblyProcessor";
			tablesinfo[33].names = new string[1] { "Processor" };
			tablesinfo[33].type = Types.AssemblyProcessor;
			tablesinfo[33].ctypes = new Types[1] { Types.UInt32 };
			tablesinfo[34].Name = "AssemblyOS";
			tablesinfo[34].names = new string[3] { "OSPlatformId", "OSMajorVersion", "OSMinorVersion" };
			tablesinfo[34].type = Types.AssemblyOS;
			tablesinfo[34].ctypes = new Types[3]
			{
				Types.UInt32,
				Types.UInt32,
				Types.UInt32
			};
			tablesinfo[35].Name = "AssemblyRef";
			tablesinfo[35].names = new string[9] { "MajorVersion", "MinorVersion", "BuildNumber", "RevisionNumber", "Flags", "PublicKeyOrToken", "Name", "Locale", "HashValue" };
			tablesinfo[35].type = Types.AssemblyRef;
			tablesinfo[35].ctypes = new Types[9]
			{
				Types.UInt16,
				Types.UInt16,
				Types.UInt16,
				Types.UInt16,
				Types.UInt32,
				Types.Blob,
				Types.String,
				Types.String,
				Types.Blob
			};
			tablesinfo[36].Name = "AssemblyRefProcessor";
			tablesinfo[36].names = new string[2] { "Processor", "AssemblyRef" };
			tablesinfo[36].type = Types.AssemblyRefProcessor;
			tablesinfo[36].ctypes = new Types[2]
			{
				Types.UInt32,
				Types.AssemblyRef
			};
			tablesinfo[37].Name = "AssemblyRefOS";
			tablesinfo[37].names = new string[4] { "OSPlatformId", "OSMajorVersion", "OSMinorVersion", "AssemblyRef" };
			tablesinfo[37].type = Types.AssemblyRefOS;
			tablesinfo[37].ctypes = new Types[4]
			{
				Types.UInt32,
				Types.UInt32,
				Types.UInt32,
				Types.AssemblyRef
			};
			tablesinfo[38].Name = "File";
			tablesinfo[38].names = new string[3] { "Flags", "Name", "HashValue" };
			tablesinfo[38].type = Types.File;
			tablesinfo[38].ctypes = new Types[3]
			{
				Types.UInt32,
				Types.String,
				Types.Blob
			};
			tablesinfo[39].Name = "ExportedType";
			tablesinfo[39].names = new string[5] { "Flags", "TypeDefId", "TypeName", "TypeNamespace", "TypeImplementation" };
			tablesinfo[39].type = Types.ExportedType;
			tablesinfo[39].ctypes = new Types[5]
			{
				Types.UInt32,
				Types.UInt32,
				Types.String,
				Types.String,
				Types.Implementation
			};
			tablesinfo[40].Name = "ManifestResource";
			tablesinfo[40].names = new string[4] { "Offset", "Flags", "Name", "Implementation" };
			tablesinfo[40].type = Types.ManifestResource;
			tablesinfo[40].ctypes = new Types[4]
			{
				Types.UInt32,
				Types.UInt32,
				Types.String,
				Types.Implementation
			};
			tablesinfo[41].Name = "NestedClass";
			tablesinfo[41].names = new string[2] { "NestedClass", "EnclosingClass" };
			tablesinfo[41].type = Types.NestedClass;
			tablesinfo[41].ctypes = new Types[2]
			{
				Types.TypeDef,
				Types.TypeDef
			};
			tablesinfo[42].Name = "TypeTyPar";
			tablesinfo[42].names = new string[4] { "Number", "Class", "Bound", "Name" };
			tablesinfo[42].type = Types.TypeTyPar;
			tablesinfo[42].ctypes = new Types[4]
			{
				Types.UInt16,
				Types.TypeDef,
				Types.TypeDefOrRef,
				Types.String
			};
			tablesinfo[43].Name = "MethodTyPar";
			tablesinfo[43].names = new string[4] { "Number", "Method", "Bound", "Name" };
			tablesinfo[43].type = Types.MethodTyPar;
			tablesinfo[43].ctypes = new Types[4]
			{
				Types.UInt16,
				Types.Method,
				Types.TypeDefOrRef,
				Types.String
			};
			tablesinfo[44].Name = "MethodTyPar";
			tablesinfo[44].names = new string[4] { "Number", "Method", "Bound", "Name" };
			tablesinfo[44].type = Types.MethodTyPar;
			tablesinfo[44].ctypes = new Types[4]
			{
				Types.UInt16,
				Types.Method,
				Types.TypeDefOrRef,
				Types.String
			};
			codedTokenBits = new int[33]
			{
				0, 1, 1, 2, 2, 3, 3, 3, 3, 4,
				4, 4, 4, 4, 4, 4, 4, 5, 5, 5,
				5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
				5, 5, 5
			};
			reftables = new RefTableInfo[12];
			reftables[0].type = Types.TypeDefOrRef;
			reftables[0].reftypes = new Types[3]
			{
				Types.TypeDef,
				Types.TypeRef,
				Types.TypeSpec
			};
			reftables[0].refindex = new int[3] { 1, 2, 27 };
			reftables[1].type = Types.HasConstant;
			reftables[1].reftypes = new Types[3]
			{
				Types.Field,
				Types.Param,
				Types.Property
			};
			reftables[1].refindex = new int[3] { 4, 8, 23 };
			reftables[2].type = Types.CustomAttributeType;
			reftables[2].reftypes = new Types[5]
			{
				Types.TypeRef,
				Types.TypeDef,
				Types.Method,
				Types.MemberRef,
				Types.UserString
			};
			reftables[2].refindex = new int[5] { 1, 2, 6, 10, 1 };
			reftables[3].type = Types.HasSemantic;
			reftables[3].reftypes = new Types[2]
			{
				Types.Event,
				Types.Property
			};
			reftables[3].refindex = new int[2] { 20, 23 };
			reftables[4].type = Types.ResolutionScope;
			reftables[4].reftypes = new Types[4]
			{
				Types.Module,
				Types.ModuleRef,
				Types.AssemblyRef,
				Types.TypeRef
			};
			reftables[4].refindex = new int[4] { 0, 26, 35, 1 };
			reftables[5].type = Types.HasFieldMarshal;
			reftables[5].reftypes = new Types[2]
			{
				Types.Field,
				Types.Param
			};
			reftables[5].refindex = new int[2] { 4, 8 };
			reftables[6].type = Types.HasDeclSecurity;
			reftables[6].reftypes = new Types[3]
			{
				Types.TypeDef,
				Types.Method,
				Types.Assembly
			};
			reftables[6].refindex = new int[3] { 2, 6, 32 };
			reftables[7].type = Types.MemberRefParent;
			reftables[7].reftypes = new Types[5]
			{
				Types.TypeDef,
				Types.TypeRef,
				Types.ModuleRef,
				Types.Method,
				Types.TypeSpec
			};
			reftables[7].refindex = new int[5] { 2, 1, 26, 6, 27 };
			reftables[8].type = Types.MethodDefOrRef;
			reftables[8].reftypes = new Types[2]
			{
				Types.Method,
				Types.MemberRef
			};
			reftables[8].refindex = new int[2] { 6, 10 };
			reftables[9].type = Types.MemberForwarded;
			reftables[9].reftypes = new Types[2]
			{
				Types.Field,
				Types.Method
			};
			reftables[9].refindex = new int[2] { 4, 6 };
			reftables[10].type = Types.Implementation;
			reftables[10].reftypes = new Types[3]
			{
				Types.File,
				Types.AssemblyRef,
				Types.ExportedType
			};
			reftables[10].refindex = new int[3] { 38, 35, 39 };
			reftables[11].type = Types.HasCustomAttribute;
			reftables[11].reftypes = new Types[19]
			{
				Types.Method,
				Types.Field,
				Types.TypeRef,
				Types.TypeDef,
				Types.Param,
				Types.InterfaceImpl,
				Types.MemberRef,
				Types.Module,
				Types.Permission,
				Types.Property,
				Types.Event,
				Types.StandAloneSig,
				Types.ModuleRef,
				Types.TypeSpec,
				Types.Assembly,
				Types.AssemblyRef,
				Types.File,
				Types.ExportedType,
				Types.ManifestResource
			};
			reftables[11].refindex = new int[19]
			{
				6, 4, 1, 2, 8, 9, 10, 0, 14, 23,
				20, 17, 26, 27, 32, 35, 38, 39, 40
			};
		}

		public int Rva2Offset(int rva)
		{
			for (int i = 0; i < sections.Length; i++)
			{
				if (sections[i].virtual_address <= rva && sections[i].virtual_address + sections[i].size_of_raw_data >= rva)
				{
					return sections[i].pointer_to_raw_data + (rva - sections[i].virtual_address);
				}
			}
			return 0;
		}

		public int Offset2Rva(int uOffset)
		{
			for (int i = 0; i < sections.Length; i++)
			{
				if (sections[i].pointer_to_raw_data <= uOffset && sections[i].pointer_to_raw_data + sections[i].size_of_raw_data >= uOffset)
				{
					return sections[i].virtual_address + (uOffset - sections[i].pointer_to_raw_data);
				}
			}
			return 0;
		}

		public int GetTypeSize(Types trans)
		{
			if (trans == Types.UInt16)
			{
				return 2;
			}
			if (trans == Types.UInt32)
			{
				return 4;
			}
			if (trans == Types.String)
			{
				return GetStringIndexSize();
			}
			if (trans == Types.Guid)
			{
				return GetGuidIndexSize();
			}
			if (trans == Types.Blob)
			{
				return GetBlobIndexSize();
			}
			if (trans < Types.TypeDefOrRef)
			{
				if (TableLengths[(int)trans] > 65535)
				{
					return 4;
				}
				return 2;
			}
			if (trans < Types.UInt16)
			{
				int num = (int)(trans - 64);
				int num2 = codedTokenBits[reftables[num].refindex.Length];
				int num3 = 65535;
				num3 >>= num2;
				for (int i = 0; i < reftables[num].refindex.Length; i++)
				{
					if (TableLengths[reftables[num].refindex[i]] > num3)
					{
						return 4;
					}
				}
				return 2;
			}
			return 0;
		}

		public int GetStringIndexSize()
		{
			return (((uint)tableheader.HeapOffsetSizes & (true ? 1u : 0u)) != 0) ? 4 : 2;
		}

		public int GetGuidIndexSize()
		{
			return ((tableheader.HeapOffsetSizes & 2u) != 0) ? 4 : 2;
		}

		public int GetBlobIndexSize()
		{
			return ((tableheader.HeapOffsetSizes & 4u) != 0) ? 4 : 2;
		}

		public static bool BytesEqual(byte[] Array1, byte[] Array2)
		{
			if (Array1.Length != Array2.Length)
			{
				return false;
			}
			for (int i = 0; i < Array1.Length; i++)
			{
				if (Array1[i] != Array2[i])
				{
					return false;
				}
			}
			return true;
		}

		public static byte[] BuildNetReloc(int RVA)
		{
			byte[] array = new byte[12];
			int value = (int)(RVA & 0xFFFFF000u);
			byte[] bytes = BitConverter.GetBytes(value);
			for (int i = 0; i < bytes.Length; i++)
			{
				array[i] = bytes[i];
			}
			int value2 = 12;
			bytes = BitConverter.GetBytes(value2);
			for (int i = 0; i < bytes.Length; i++)
			{
				array[i + 4] = bytes[i];
			}
			int value3 = (RVA & 0xFFF) + 12288;
			bytes = BitConverter.GetBytes(value3);
			for (int i = 0; i < bytes.Length; i++)
			{
				array[i + 8] = bytes[i];
			}
			return array;
		}

		public unsafe bool FixPE(string filename, BinaryReader reader, byte[] filebytes, TextBox txt, bool Fixcharacteristics, bool FixRawAllignment, bool FixSizeOfImage, bool FixOptionaHeader1, bool FixSizeOfInitData, bool FixVersionInfo, bool FixSizeOfStackHeap, bool FixDataDir, bool FixNetSpider)
		{
			txt.Text = "\r\nStage 1: Fixing PE\r\n";
			reader.BaseStream.Position = 0L;
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(filebytes));
			IntPtr ptr2;
			fixed (byte* ptr = reader.ReadBytes(sizeof(IMAGE_DOS_HEADER)))
			{
				ptr2 = (IntPtr)ptr;
			}
			idh = (IMAGE_DOS_HEADER)Marshal.PtrToStructure(ptr2, typeof(IMAGE_DOS_HEADER));
			if (idh.e_magic != 23117)
			{
				return false;
			}
			reader.BaseStream.Position = idh.e_lfanew;
			fixed (byte* ptr = reader.ReadBytes(sizeof(IMAGE_NT_HEADERS)))
			{
				ptr2 = (IntPtr)ptr;
			}
			inh = (IMAGE_NT_HEADERS)Marshal.PtrToStructure(ptr2, typeof(IMAGE_NT_HEADERS));
			if (inh.Signature != 17744)
			{
				return false;
			}
			if (Fixcharacteristics)
			{
				bool flag = false;
				if (((uint)inh.ifh.Characteristics & 0x2000u) != 0)
				{
					flag = true;
				}
				if (filename.ToLower().Contains(".dll") && !flag)
				{
					short value = (short)(inh.ifh.Characteristics + 8192);
					binaryWriter.BaseStream.Position = idh.e_lfanew + 22;
					binaryWriter.Write(value);
					txt.Text += "Set File->Characteristics->FileIsADll as true\r\n";
				}
				if (filename.ToLower().Contains(".exe") && flag)
				{
					short value = (short)(inh.ifh.Characteristics - 8192);
					binaryWriter.BaseStream.Position = idh.e_lfanew + 22;
					binaryWriter.Write(value);
					txt.Text += "Set File->Characteristics->FileIsADll as false\r\n";
				}
			}
			reader.BaseStream.Position = idh.e_lfanew + 4 + sizeof(IMAGE_FILE_HEADER) + inh.ifh.SizeOfOptionalHeader;
			sections = new image_section_header[inh.ifh.NumberOfSections];
			long position = reader.BaseStream.Position;
			fixed (byte* ptr = reader.ReadBytes(sizeof(image_section_header) * inh.ifh.NumberOfSections))
			{
				ptr2 = (IntPtr)ptr;
			}
			for (int i = 0; i < sections.Length; i++)
			{
				ref image_section_header reference = ref sections[i];
				reference = (image_section_header)Marshal.PtrToStructure(ptr2, typeof(image_section_header));
				ptr2 = (IntPtr)(ptr2.ToInt32() + Marshal.SizeOf(typeof(image_section_header)));
			}
			if (FixRawAllignment)
			{
				for (int i = 0; i < sections.Length; i++)
				{
					if (sections[i].size_of_raw_data % inh.ioh.FileAlignment == 0)
					{
						continue;
					}
					binaryWriter.BaseStream.Position = position + (long)Marshal.SizeOf(typeof(image_section_header)) * (long)i + 16;
					int num = sections[i].size_of_raw_data % inh.ioh.FileAlignment;
					if (num != 0)
					{
						num = inh.ioh.FileAlignment - num;
					}
					int num2 = sections[i].size_of_raw_data + num;
					if (i < sections.Length - 1)
					{
						int num3 = sections[i + 1].pointer_to_raw_data - sections[i].pointer_to_raw_data;
						if (num2 > num3)
						{
							num2 = num3;
						}
					}
					binaryWriter.Write(num2);
					txt.Text = txt.Text + "Set raw size of section with number " + (i + 1) + " to " + num2.ToString("X4") + "\r\n";
					sections[i].size_of_raw_data = num2;
				}
			}
			if (FixSizeOfImage && (inh.ioh.SizeOfHeaders % inh.ioh.FileAlignment != 0 || inh.ioh.SizeOfHeaders == 0 || inh.ioh.SizeOfHeaders > sections[0].pointer_to_raw_data))
			{
				int pointer_to_raw_data = sections[0].pointer_to_raw_data;
				int num4 = pointer_to_raw_data % inh.ioh.FileAlignment;
				if (num4 != 0)
				{
					num4 = inh.ioh.FileAlignment - num4;
				}
				pointer_to_raw_data += num4;
				binaryWriter.BaseStream.Position = idh.e_lfanew + 84;
				binaryWriter.Write(pointer_to_raw_data);
				inh.ioh.SizeOfHeaders = pointer_to_raw_data;
				txt.Text = txt.Text + "Set SizeOfHeaders to right value: " + pointer_to_raw_data.ToString("X8") + "\r\n";
			}
			int num5 = inh.ioh.SizeOfHeaders % inh.ioh.SectionAlignment;
			if (num5 != 0)
			{
				num5 = inh.ioh.SectionAlignment - num5;
			}
			int num6 = inh.ioh.SizeOfHeaders + num5;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			for (int i = 0; i < inh.ifh.NumberOfSections; i++)
			{
				num5 = sections[i].virtual_size % inh.ioh.SectionAlignment;
				if (num5 != 0)
				{
					num5 = inh.ioh.SectionAlignment - num5;
				}
				num6 = num6 + sections[i].virtual_size + num5;
				int num12 = 0;
				fixed (byte* ptr3 = sections[i].name)
				{
					if ((sections[i].characteristics & 0xFF000000u) == 1610612736 && ((sections[i].characteristics & 0xFF) == 32 || (sections[i].characteristics & 0xFF) == 0) && *ptr3 == 46 && ptr3[1] == 116 && ptr3[2] == 101 && ptr3[3] == 120 && ptr3[4] == 116 && ptr3[5] == 0 && ptr3[6] == 0 && ptr3[7] == 0)
					{
						num12 = 1;
					}
					if ((sections[i].characteristics & 0xFF000000u) == 1073741824 && ((sections[i].characteristics & 0xFF) == 64 || (sections[i].characteristics & 0xFF) == 0) && *ptr3 == 46 && ptr3[1] == 114 && ptr3[2] == 100 && ptr3[3] == 97 && ptr3[4] == 116 && ptr3[5] == 97 && ptr3[6] == 0 && ptr3[7] == 0)
					{
						num12 = 2;
					}
					if ((sections[i].characteristics & 0xFF000000u) == 3221225472u && ((sections[i].characteristics & 0xFF) == 64 || (sections[i].characteristics & 0xFF) == 0) && *ptr3 == 46 && ptr3[1] == 100 && ptr3[2] == 97 && ptr3[3] == 116 && ptr3[4] == 116 && ptr3[5] == 0 && ptr3[6] == 0 && ptr3[7] == 0)
					{
						num12 = 2;
					}
				}
				if ((sections[i].characteristics & 0xFF) == 32 || num12 == 1)
				{
					num7 += sections[i].size_of_raw_data;
					if (num10 == 0)
					{
						num10 = sections[i].virtual_address;
					}
				}
				if ((sections[i].characteristics & 0xFF) == 64 || num12 == 2)
				{
					num8 += sections[i].size_of_raw_data;
					if (num11 == 0)
					{
						num11 = sections[i].virtual_address;
					}
				}
				if ((sections[i].characteristics & 0xFF) == 128)
				{
					num9 += sections[i].size_of_raw_data;
					if (num11 == 0)
					{
						num11 = sections[i].virtual_address;
					}
				}
			}
			if (FixSizeOfImage && num6 != inh.ioh.SizeOfImage)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 80;
				binaryWriter.Write(num6);
				txt.Text = txt.Text + "Set SizeOfImage to right value: " + num6.ToString("X8") + "\r\n";
			}
			if (FixOptionaHeader1)
			{
				if (num7 != inh.ioh.SizeOfCode && num7 != 0)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 28;
					binaryWriter.Write(num7);
					txt.Text = txt.Text + "Set OptionalHeader.SizeOfCode to: " + num7.ToString("X8") + "\r\n";
				}
				if (num9 != inh.ioh.SizeOfUninitializedData)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 36;
					binaryWriter.Write(num9);
					txt.Text = txt.Text + "Set OptionalHeader.SizeOfUninitData to: " + num9.ToString("X8") + "\r\n";
				}
				if (Rva2Offset(inh.ioh.BaseOfCode) == 0)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 44;
					binaryWriter.Write(num10);
					txt.Text = txt.Text + "Set OptionalHeader.BaseOfCode to: " + num10.ToString("X8") + "\r\n";
				}
				if (Rva2Offset(inh.ioh.BaseOfData) == 0)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 48;
					binaryWriter.Write(num11);
					txt.Text = txt.Text + "Set OptionalHeader.BaseOfData to: " + num11.ToString("X8") + "\r\n";
				}
			}
			if (FixSizeOfInitData && num8 != inh.ioh.SizeOfInitializedData)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 32;
				binaryWriter.Write(num8);
				txt.Text = txt.Text + "Set OptionalHeader.SizeOfInitData to: " + num8.ToString("X8") + "\r\n";
			}
			if (FixVersionInfo)
			{
				if (inh.ioh.MajorLinkerVersion <= 0 || inh.ioh.MajorLinkerVersion > 100)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 26;
					binaryWriter.Write((byte)8);
					txt.Text += "Set OptionalHeader.MajorLinkerVersion to 8\r\n";
				}
				if (inh.ioh.MinorLinkerVersion > 100)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 27;
					binaryWriter.Write((byte)0);
					txt.Text += "Set OptionalHeader.MinorLinkerVersion to 0\r\n";
				}
				if (inh.ioh.MajorOperatingSystemVersion <= 0 || inh.ioh.MajorOperatingSystemVersion > 30)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 64;
					binaryWriter.Write((short)4);
					txt.Text += "Set OptionalHeader.MajorOperatingSystemVersion to 4\r\n";
				}
				if (inh.ioh.MinorOperatingSystemVersion < 0 || inh.ioh.MinorOperatingSystemVersion > 30)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 66;
					binaryWriter.Write((short)0);
					txt.Text += "Set OptionalHeader.MinorOperatingSystemVersion to 0\r\n";
				}
				if (inh.ioh.MajorImageVersion < 0)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 68;
					binaryWriter.Write((short)0);
					txt.Text += "Set OptionalHeader.MajorImageVersion to 0\r\n";
				}
				if (inh.ioh.MinorImageVersion != 0)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 70;
					binaryWriter.Write((short)0);
					txt.Text += "Set OptionalHeader.MinorImageVersion to 0\r\n";
				}
				if (inh.ioh.MajorSubsystemVersion <= 0 || inh.ioh.MajorSubsystemVersion > 30)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 72;
					binaryWriter.Write((short)4);
					txt.Text += "Set OptionalHeader.MajorSubsystemVersion to 4\r\n";
				}
				if (inh.ioh.MinorSubsystemVersion != 0)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 74;
					binaryWriter.Write((short)0);
					txt.Text += "Set OptionalHeader.MinorSubsystemVersion to 0\r\n";
				}
				if (inh.ioh.Win32VersionValue != 0)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 76;
					binaryWriter.Write(0);
					txt.Text += "Set OptionalHeader.Win32VersionValue to 0\r\n";
				}
			}
			if (FixSizeOfStackHeap)
			{
				if (inh.ioh.SizeOfStackReserve != 1048576)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 96;
					binaryWriter.Write(1048576);
					txt.Text += "Set OptionalHeader.SizeOfStackReserve to 0x0100000\r\n";
				}
				if (inh.ioh.SizeOfStackCommit != 4096)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 100;
					binaryWriter.Write(4096);
					txt.Text += "Set OptionalHeader.SizeOfStackCommit to 0x01000\r\n";
				}
				if (inh.ioh.SizeOfHeapReserve != 1048576)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 104;
					binaryWriter.Write(1048576);
					txt.Text += "Set OptionalHeader.SizeOfHeapReserve to 0x0100000\r\n";
				}
				if (inh.ioh.SizeOfHeapCommit != 4096)
				{
					binaryWriter.BaseStream.Position = idh.e_lfanew + 108;
					binaryWriter.Write(4096);
					txt.Text += "Set OptionalHeader.SizeOfHeapCommit to 0x01000\r\n";
				}
			}
			if (inh.ioh.ExportDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.ExportDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 120;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set ExportDirectory RVA/Size to 0\r\n";
			}
			if (inh.ioh.ImportDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.ImportDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 128;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set ImportDirectory RVA/Size to 0\r\n";
			}
			if (inh.ioh.ResourceDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.ResourceDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 136;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set ResourceDirectory RVA/Size to 0\r\n";
			}
			if (inh.ioh.ExceptionDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.ExceptionDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 144;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set ExceptionDirectory RVA/Size to 0\r\n";
			}
			if (inh.ioh.SecurityDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.SecurityDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 152;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set SecurityDirectory RVA/Size to 0\r\n";
			}
			if (inh.ioh.RelocationDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.RelocationDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 160;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set RelocationDirectory RVA/Size to 0\r\n";
			}
			if (inh.ioh.DebugDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.DebugDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 168;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set DebugDirectory RVA/Size to 0\r\n";
			}
			if (inh.ioh.ArchitectureDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.ArchitectureDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 176;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set ArchitectureDirectory RVA/Size to 0\r\n";
			}
			if (inh.ioh.TLSDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.TLSDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 192;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set TLSDirectory RVA/Size to 0\r\n";
			}
			if (inh.ioh.ConfigurationDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.ConfigurationDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 200;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set ConfigurationDirectory RVA/Size to 0\r\n";
			}
			if (inh.ioh.BoundImportDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.BoundImportDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 208;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set BoundImportDirectory RVA/Size to 0\r\n";
			}
			if (inh.ioh.ImportAddressTableDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.ImportAddressTableDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 216;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set ImportAddressTableDirectory RVA/Size to 0\r\n";
			}
			if (inh.ioh.DelayImportDirectory.RVA != 0 && FixDataDir && Rva2Offset(inh.ioh.DelayImportDirectory.RVA) == 0)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 224;
				binaryWriter.Write(0);
				binaryWriter.Write(0);
				txt.Text += "Set DelayImportDirectory RVA/Size to 0\r\n";
			}
			if (FixNetSpider && inh.ioh.ResourceDirectory.RVA != 0)
			{
				int num13 = Rva2Offset(inh.ioh.ResourceDirectory.RVA);
				if (num13 != 0)
				{
					reader.BaseStream.Position = num13;
					byte[] buffer = reader.ReadBytes(inh.ioh.ResourceDirectory.Size);
					ByteBuffer byteBuffer = new ByteBuffer(buffer);
					FixSpider fixSpider = new FixSpider();
					fixSpider.PatchWin32Resources(byteBuffer);
					binaryWriter.BaseStream.Position = num13;
					binaryWriter.Write(byteBuffer.buffer);
					txt.Text += fixSpider.result;
				}
			}
			binaryWriter.Close();
			return true;
		}

		public unsafe bool FixNetPE(string filename, BinaryReader reader, byte[] filebytes, TextBox txt, bool FixNumberOfRvaAndSizes, bool FixRelocations, bool FixMetadataDataDir, bool fixBSJB, bool fixnetdir, bool fiximports)
		{
			txt.Text += "\r\nStage 2: Fixing .NET PE\r\n";
			reader.BaseStream.Position = 0L;
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(filebytes));
			IntPtr ptr2;
			fixed (byte* ptr = reader.ReadBytes(sizeof(IMAGE_DOS_HEADER)))
			{
				ptr2 = (IntPtr)ptr;
			}
			idh = (IMAGE_DOS_HEADER)Marshal.PtrToStructure(ptr2, typeof(IMAGE_DOS_HEADER));
			if (idh.e_magic != 23117)
			{
				return false;
			}
			reader.BaseStream.Position = idh.e_lfanew;
			fixed (byte* ptr = reader.ReadBytes(sizeof(IMAGE_NT_HEADERS)))
			{
				ptr2 = (IntPtr)ptr;
			}
			inh = (IMAGE_NT_HEADERS)Marshal.PtrToStructure(ptr2, typeof(IMAGE_NT_HEADERS));
			if (inh.Signature != 17744)
			{
				return false;
			}
			reader.BaseStream.Position = idh.e_lfanew + 4 + sizeof(IMAGE_FILE_HEADER) + inh.ifh.SizeOfOptionalHeader;
			sections = new image_section_header[inh.ifh.NumberOfSections];
			long position = reader.BaseStream.Position;
			fixed (byte* ptr = reader.ReadBytes(sizeof(image_section_header) * inh.ifh.NumberOfSections))
			{
				ptr2 = (IntPtr)ptr;
			}
			for (int i = 0; i < sections.Length; i++)
			{
				ref image_section_header reference = ref sections[i];
				reference = (image_section_header)Marshal.PtrToStructure(ptr2, typeof(image_section_header));
				ptr2 = (IntPtr)(ptr2.ToInt32() + Marshal.SizeOf(typeof(image_section_header)));
			}
			bool flag = true;
			int num = 0;
			if (inh.ioh.MetaDataDirectory.RVA <= 0)
			{
				flag = false;
			}
			else if (Rva2Offset(inh.ioh.MetaDataDirectory.RVA) == 0)
			{
				flag = false;
			}
			if (FixMetadataDataDir && !flag)
			{
				int num2 = 0;
				int num3 = Rva2Offset(8192);
				if (num3 != 0)
				{
					txt.Text += ".NET MetaData Directory RVA invalid - fast search for right value!\r\n";
					for (int i = 0; i < 30; i++)
					{
						bool flag2 = false;
						if (filebytes[num3 + i] == 72 && filebytes[num3 + i + 1] == 0 && filebytes[num3 + i + 2] == 0 && filebytes[num3 + i + 3] == 0)
						{
							flag2 = true;
						}
						if (!flag2 && filebytes[num3 + i + 4] == 2 && filebytes[num3 + i + 5] == 0 && (filebytes[num3 + i + 6] == 0 || filebytes[num3 + i + 6] == 5) && filebytes[num3 + i + 7] == 0)
						{
							flag2 = true;
						}
						if (!flag2 && filebytes[num3 + i + 16] <= 31 && filebytes[num3 + i + 17] == 0 && (filebytes[num3 + i + 18] == 0 || filebytes[num3 + i + 18] == 1) && filebytes[num3 + i + 19] <= 0 && filebytes[num3 + i + 23] == 6)
						{
							flag2 = true;
						}
						byte b = filebytes[num3 + i + 16];
						if (flag2)
						{
							num2 = 8192 + i;
							break;
						}
					}
					if (num2 == 0)
					{
						txt.Text += "Failed to find right .NET MetaData Directory RVA\r\n";
						txt.Text += "Fixing aborted!\r\n";
						return false;
					}
					byte[] bytes = BitConverter.GetBytes(num2);
					txt.Text = txt.Text + "Set .NET MetaData Directory RVA with the value " + num2.ToString("X8") + "\r\n";
					Array.Copy(bytes, 0, filebytes, idh.e_lfanew + 232, bytes.Length);
					bytes = BitConverter.GetBytes(72);
					Array.Copy(bytes, 0, filebytes, idh.e_lfanew + 232 + 4, bytes.Length);
					inh.ioh.MetaDataDirectory.RVA = num2;
					inh.ioh.MetaDataDirectory.Size = 72;
					flag = true;
				}
			}
			if (!flag)
			{
				txt.Text += ".NET MetaData Directory RVA invalid!\r\n";
				txt.Text += "Fixing aborted!\r\n";
				return false;
			}
			bool flag3 = true;
			reader.BaseStream.Position = Rva2Offset(inh.ioh.MetaDataDirectory.RVA) + 8;
			int num4 = reader.ReadInt32();
			int num5 = 0;
			if (num4 <= 0)
			{
				flag3 = false;
			}
			else
			{
				num5 = Rva2Offset(num4);
				if (num5 == 0)
				{
					flag3 = false;
				}
				if (flag3)
				{
					reader.BaseStream.Position = num5;
					int num6 = reader.ReadInt32();
					if (num6 != 1112167234)
					{
						flag3 = false;
					}
				}
			}
			if (fixBSJB)
			{
				int num7 = 0;
				if (!flag3)
				{
					for (int j = 0; j < filebytes.Length; j++)
					{
						if (filebytes[j] == 66 && filebytes[j + 1] == 83 && filebytes[j + 2] == 74 && filebytes[j + 3] == 66)
						{
							num7 = ((num7 == 0) ? Offset2Rva(j) : (-1));
						}
					}
					txt.Text += "Fixing MetaData.RVA (BSJB)\r\n";
					if (num7 <= 0)
					{
						txt.Text += "Failed to find right value for MetaData.RVA (BSJB)\r\n";
						txt.Text += "Fixing aborted!\r\n";
						return false;
					}
					txt.Text = txt.Text + "Set MetaData.RVA (BSJB) with " + num7.ToString("X8") + "\r\n";
					netdir.MetaDataRVA = num7;
					byte[] bytes = BitConverter.GetBytes(num7);
					Array.Copy(bytes, 0, filebytes, Rva2Offset(inh.ioh.MetaDataDirectory.RVA) + 8, bytes.Length);
					int num8 = 0;
					try
					{
						reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA);
						mh = default(MetaDataHeader);
						mh.Signature = reader.ReadInt32();
						mh.MajorVersion = reader.ReadInt16();
						mh.MinorVersion = reader.ReadInt16();
						mh.Reserved = reader.ReadInt32();
						mh.VerionLenght = reader.ReadInt32();
						mh.VersionString = reader.ReadBytes(mh.VerionLenght);
						mh.Flags = reader.ReadInt16();
						mh.NumberOfStreams = reader.ReadInt16();
						streams = new MetaDataStream[mh.NumberOfStreams];
						for (int i = 0; i < mh.NumberOfStreams; i++)
						{
							streams[i].headerpos = (int)reader.BaseStream.Position;
							streams[i].Offset = reader.ReadInt32();
							streams[i].Size = reader.ReadInt32();
							char[] array = new char[32];
							int num9 = 0;
							byte b2 = 0;
							while ((b2 = reader.ReadByte()) != 0)
							{
								array[num9++] = (char)b2;
							}
							num9++;
							int count = ((num9 % 4 != 0) ? (4 - num9 % 4) : 0);
							reader.ReadBytes(count);
							if (i == mh.NumberOfStreams - 1)
							{
								num8 = streams[i].Offset + streams[i].Size;
							}
						}
					}
					catch
					{
					}
					if (num8 == 0)
					{
						txt.Text += "Failed to get MetaData.Size!\r\n";
					}
					else
					{
						txt.Text = txt.Text + "Set MetaData.Size with " + num8.ToString("X8") + "\r\n";
						bytes = BitConverter.GetBytes(num8);
						Array.Copy(bytes, 0, filebytes, Rva2Offset(inh.ioh.MetaDataDirectory.RVA) + 8 + 4, bytes.Length);
					}
					flag3 = true;
				}
			}
			if (!flag3)
			{
				txt.Text += "MetaData.RVA (BSJB) invalid!\r\n";
				txt.Text += "Fixing aborted!\r\n";
				return false;
			}
			if (FixNumberOfRvaAndSizes && inh.ioh.NumberOfRvaAndSizes != 16)
			{
				binaryWriter.BaseStream.Position = idh.e_lfanew + 116;
				binaryWriter.Write(16);
				txt.Text += "Set NumberOfRvaAndSizes to 0x10\r\n";
			}
			if (fixnetdir && flag && flag3)
			{
				reader.BaseStream.Position = Rva2Offset(inh.ioh.MetaDataDirectory.RVA);
				int num10 = reader.ReadInt32();
				if (num10 != 72)
				{
					txt.Text += "Set .NET Directory->cb to 0x48\r\n";
					byte[] bytes2 = BitConverter.GetBytes(72);
					Array.Copy(bytes2, 0, filebytes, Rva2Offset(inh.ioh.MetaDataDirectory.RVA), bytes2.Length);
				}
				short num11 = reader.ReadInt16();
				short num12 = reader.ReadInt16();
				if (num11 != 2)
				{
					txt.Text += "Set .NET Directory->MajorRuntimeVersion to 02\r\n";
					byte[] bytes3 = BitConverter.GetBytes((short)2);
					Array.Copy(bytes3, 0, filebytes, Rva2Offset(inh.ioh.MetaDataDirectory.RVA) + 4, bytes3.Length);
				}
				if (num12 != 5 && num12 != 0)
				{
					byte[] bytes4 = BitConverter.GetBytes((short)5);
					txt.Text += "Set .NET Directory->MirorRuntimeVersion to 05\r\n";
					Array.Copy(bytes4, 0, filebytes, Rva2Offset(inh.ioh.MetaDataDirectory.RVA) + 6, bytes4.Length);
				}
			}
			bool flag4 = true;
			int num13 = 0;
			bool flag5 = true;
			int num14 = 0;
			bool flag6 = true;
			int num15 = 0;
			int num16 = 0;
			int num17 = 0;
			int num18 = 0;
			int num19 = 0;
			bool flag7 = fiximports;
			if (inh.ioh.ImportDirectory.RVA <= 0)
			{
				flag5 = false;
			}
			else
			{
				num14 = Rva2Offset(inh.ioh.ImportDirectory.RVA);
				if (num14 == 0)
				{
					flag5 = false;
				}
				if (Rva2Offset(inh.ioh.ImportDirectory.RVA + 40) == 0)
				{
					flag5 = false;
				}
			}
			while (true)
			{
				num16 = 0;
				num19 = 0;
				num18 = 0;
				if (!flag5)
				{
					break;
				}
				int num20 = 0;
				int num21 = BitConverter.ToInt32(filebytes, num14 + 12 + num20);
				int num22 = Rva2Offset(num21);
				try
				{
					while (num21 != 0 && num22 != 0)
					{
						byte[] array2 = new byte[12]
						{
							109, 115, 99, 111, 114, 101, 101, 46, 100, 108,
							108, 0
						};
						byte[] array3 = new byte[array2.Length];
						Array.Copy(filebytes, num22, array3, 0, array3.Length);
						if (BytesEqual(array3, array2))
						{
							int num23 = BitConverter.ToInt32(filebytes, num14 + num20);
							if (num23 > 0)
							{
								int num24 = Rva2Offset(num23);
								if (num24 > 0)
								{
									int num25 = BitConverter.ToInt32(filebytes, num24);
									if (num25 > 0)
									{
										int num26 = Rva2Offset(num25);
										if (num26 > 0)
										{
											byte[] array4 = new byte[12]
											{
												95, 67, 111, 114, 69, 120, 101, 77, 97, 105,
												110, 0
											};
											byte[] array5 = new byte[12]
											{
												95, 67, 111, 114, 68, 108, 108, 77, 97, 105,
												110, 0
											};
											array3 = new byte[array4.Length];
											Array.Copy(filebytes, num26 + 2, array3, 0, array3.Length);
											if (BytesEqual(array3, array4) || BytesEqual(array3, array5))
											{
												num19 = num24;
												num18 = BitConverter.ToInt32(filebytes, num14 + 16 + num20);
												num16 = BitConverter.ToInt32(filebytes, num14 + num20 + 16);
												if (num16 != 0)
												{
													num17 = Rva2Offset(num16);
													if (num17 == 0)
													{
														num16 = 0;
													}
												}
												if (num17 != 0)
												{
													break;
												}
											}
										}
									}
								}
							}
						}
						if (num17 != 0)
						{
							break;
						}
						num20 += 20;
						num21 = BitConverter.ToInt32(filebytes, num14 + 12 + num20);
						num22 = Rva2Offset(num21);
					}
				}
				catch
				{
				}
				if (flag7 && (num16 == 0 || num19 == 0 || num18 == 0))
				{
					byte[] buffer = new byte[74];
					BinaryWriter binaryWriter2 = new BinaryWriter(new MemoryStream(buffer));
					binaryWriter2.BaseStream.Position = 0L;
					binaryWriter2.Write(inh.ioh.ImportDirectory.RVA + 40);
					binaryWriter2.BaseStream.Position = 12L;
					binaryWriter2.Write(inh.ioh.ImportDirectory.RVA + 62);
					binaryWriter2.Write(8192);
					binaryWriter2.BaseStream.Position = 40L;
					binaryWriter2.Write(inh.ioh.ImportDirectory.RVA + 48);
					byte[] buffer2 = new byte[12]
					{
						95, 67, 111, 114, 69, 120, 101, 77, 97, 105,
						110, 0
					};
					byte[] buffer3 = new byte[12]
					{
						109, 115, 99, 111, 114, 101, 101, 46, 100, 108,
						108, 0
					};
					binaryWriter2.BaseStream.Position = 50L;
					binaryWriter2.Write(buffer2);
					binaryWriter2.BaseStream.Position = 62L;
					binaryWriter2.Write(buffer3);
					binaryWriter2.Close();
					binaryWriter.BaseStream.Position = num14;
					binaryWriter.Write(buffer);
					flag7 = false;
					txt.Text += "Import Table rebuilded!\r\n";
					continue;
				}
				if ((num16 == 0 && (num19 == 0 || num18 == 0)) || !FixRelocations)
				{
					break;
				}
				flag4 = true;
				if (inh.ioh.AddressOfEntryPoint <= 0)
				{
					flag4 = false;
				}
				else
				{
					num13 = Rva2Offset(inh.ioh.AddressOfEntryPoint);
					if (num13 == 0)
					{
						flag4 = false;
					}
					if (Rva2Offset(inh.ioh.AddressOfEntryPoint + 5) == 0)
					{
						flag4 = false;
					}
				}
				if (!flag4 && num16 != 0)
				{
					int value = num16 + inh.ioh.ImageBase;
					byte[] bytes5 = BitConverter.GetBytes(value);
					byte[] array6 = new byte[2 + bytes5.Length];
					array6[0] = byte.MaxValue;
					array6[1] = 37;
					Array.Copy(bytes5, 0, array6, 2, 4);
					int num27 = 0;
					for (int i = num14; i < filebytes.Length; i++)
					{
						bool flag8 = true;
						for (int j = 0; j < array6.Length; j++)
						{
							if (filebytes[i + j] != array6[j])
							{
								flag8 = false;
								break;
							}
						}
						if (flag8)
						{
							num27 = i;
							break;
						}
					}
					if (num27 != 0)
					{
						int num28 = Offset2Rva(num27);
						if (num28 != 0)
						{
							binaryWriter.BaseStream.Position = idh.e_lfanew + 40;
							binaryWriter.Write(num28);
							txt.Text = txt.Text + "Set EntryPoint with: " + num28.ToString("X8") + "\r\n";
						}
					}
				}
				if (num19 != 0 && num18 != 0)
				{
					byte[] bytes5 = new byte[4];
					Array.Copy(filebytes, num19, bytes5, 0, 4);
					byte[] array7 = new byte[4];
					int num29 = Rva2Offset(num18);
					Array.Copy(filebytes, num29, array7, 0, 4);
					if (!BytesEqual(array7, bytes5))
					{
						Array.Copy(bytes5, 0, filebytes, num29, 4);
						txt.Text += "Fixing IAT FTs\r\n";
					}
				}
				if (flag4 && num16 != 0)
				{
					if (filebytes[num13] != byte.MaxValue || filebytes[num13 + 1] != 37)
					{
						filebytes[num13] = byte.MaxValue;
						filebytes[num13 + 1] = 37;
						txt.Text += "Set first 2 bytes of EntryPoint to 0x0FF25\r\n";
					}
					int value = num16 + inh.ioh.ImageBase;
					byte[] bytes5 = BitConverter.GetBytes(value);
					byte[] array8 = new byte[4];
					Array.Copy(filebytes, num13 + 2, array8, 0, 4);
					if (!BytesEqual(bytes5, array8))
					{
						Array.Copy(bytes5, 0, filebytes, num13 + 2, 4);
						txt.Text = txt.Text + "Fix jump address at EntryPoint to " + value.ToString("X8") + "\r\n";
					}
				}
				if (inh.ioh.RelocationDirectory.RVA <= 0)
				{
					flag6 = false;
				}
				else
				{
					num15 = Rva2Offset(inh.ioh.RelocationDirectory.RVA);
					if (num14 == 0)
					{
						flag6 = false;
					}
					if (Rva2Offset(inh.ioh.RelocationDirectory.RVA + 11) == 0)
					{
						flag6 = false;
					}
				}
				if (flag6 && num16 != 0)
				{
					byte[] array9 = BuildNetReloc(inh.ioh.AddressOfEntryPoint + 2);
					byte[] array10 = new byte[12];
					Array.Copy(filebytes, num15, array10, 0, array10.Length);
					if (!BytesEqual(array9, array10))
					{
						Array.Copy(array9, 0, filebytes, num15, 12);
						txt.Text += "Relocation contents fixed!\r\n";
						byte[] bytes6 = BitConverter.GetBytes(12);
						Array.Copy(bytes6, 0, filebytes, idh.e_lfanew + 164, bytes6.Length);
					}
				}
				break;
			}
			binaryWriter.Close();
			return true;
		}

		public unsafe bool RemoveInvalids(string filename, BinaryReader reader, byte[] filebytes, TextBox txt, bool RemoveStreams)
		{
			txt.Text += "\r\nStage 3: Removing invalid streams\r\n";
			reader.BaseStream.Position = 0L;
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(filebytes));
			IntPtr ptr2;
			fixed (byte* ptr = reader.ReadBytes(sizeof(IMAGE_DOS_HEADER)))
			{
				ptr2 = (IntPtr)ptr;
			}
			idh = (IMAGE_DOS_HEADER)Marshal.PtrToStructure(ptr2, typeof(IMAGE_DOS_HEADER));
			if (idh.e_magic != 23117)
			{
				return false;
			}
			reader.BaseStream.Position = idh.e_lfanew;
			fixed (byte* ptr = reader.ReadBytes(sizeof(IMAGE_NT_HEADERS)))
			{
				ptr2 = (IntPtr)ptr;
			}
			inh = (IMAGE_NT_HEADERS)Marshal.PtrToStructure(ptr2, typeof(IMAGE_NT_HEADERS));
			if (inh.Signature != 17744)
			{
				return false;
			}
			reader.BaseStream.Position = idh.e_lfanew + 4 + sizeof(IMAGE_FILE_HEADER) + inh.ifh.SizeOfOptionalHeader;
			sections = new image_section_header[inh.ifh.NumberOfSections];
			long position = reader.BaseStream.Position;
			fixed (byte* ptr = reader.ReadBytes(sizeof(image_section_header) * inh.ifh.NumberOfSections))
			{
				ptr2 = (IntPtr)ptr;
			}
			for (int i = 0; i < sections.Length; i++)
			{
				ref image_section_header reference = ref sections[i];
				reference = (image_section_header)Marshal.PtrToStructure(ptr2, typeof(image_section_header));
				ptr2 = (IntPtr)(ptr2.ToInt32() + Marshal.SizeOf(typeof(image_section_header)));
			}
			if (inh.ioh.MetaDataDirectory.RVA <= 0)
			{
				txt.Text += "Can't continue processing MetaDataDirectory!\r\n";
				txt.Text += "Probabily invalid RVA!\r\n";
				return true;
			}
			long num = Rva2Offset(inh.ioh.MetaDataDirectory.RVA);
			if (num == 0)
			{
				txt.Text += "Can't continue processing MetaDataDirectory!\r\n";
				txt.Text += "Probabily invalid RVA!\r\n";
				return true;
			}
			reader.BaseStream.Position = num;
			fixed (byte* ptr = reader.ReadBytes(sizeof(NETDirectory)))
			{
				ptr2 = (IntPtr)ptr;
			}
			netdir = (NETDirectory)Marshal.PtrToStructure(ptr2, typeof(NETDirectory));
			reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA);
			mh = default(MetaDataHeader);
			mh.Signature = reader.ReadInt32();
			mh.MajorVersion = reader.ReadInt16();
			mh.MinorVersion = reader.ReadInt16();
			mh.Reserved = reader.ReadInt32();
			mh.VerionLenght = reader.ReadInt32();
			mh.VersionString = reader.ReadBytes(mh.VerionLenght);
			mh.Flags = reader.ReadInt16();
			mh.NumberOfStreams = reader.ReadInt16();
			streams = new MetaDataStream[mh.NumberOfStreams];
			int num2 = 0;
			for (int i = 0; i < mh.NumberOfStreams; i++)
			{
				streams[i].headerpos = (int)reader.BaseStream.Position;
				streams[i].Offset = reader.ReadInt32();
				streams[i].Size = reader.ReadInt32();
				char[] array = new char[32];
				int num3 = 0;
				byte b = 0;
				while ((b = reader.ReadByte()) != 0)
				{
					array[num3++] = (char)b;
				}
				num3++;
				int count = ((num3 % 4 != 0) ? (4 - num3 % 4) : 0);
				reader.ReadBytes(count);
				streams[i].Sizeofheader = (int)reader.BaseStream.Position - streams[i].headerpos;
				num2 += streams[i].Sizeofheader;
				ref MetaDataStream reference2 = ref streams[i];
				string text = new string(array);
				char[] trimChars = new char[1];
				reference2.Name = text.Trim(trimChars);
				if (MetadataRoot.Offset == 0 && MetadataRoot.Size == 0 && (streams[i].Name == "#~" || streams[i].Name == "#-"))
				{
					MetadataRoot.Name = streams[i].Name;
					MetadataRoot.Offset = streams[i].Offset;
					MetadataRoot.Size = streams[i].Size;
				}
				if (StringsStream.Offset == 0 && StringsStream.Size == 0 && streams[i].Name == "#Strings")
				{
					StringsStream.Name = streams[i].Name;
					StringsStream.Offset = streams[i].Offset;
					StringsStream.Size = streams[i].Size;
				}
				if (BlobOffset == 0 && streams[i].Name == "#Blob")
				{
					BlobOffset = streams[i].Offset;
					BlobSize = streams[i].Size;
				}
			}
			int num4 = 0;
			bool flag = false;
			MemoryStream memoryStream = new MemoryStream();
			if (RemoveStreams)
			{
				for (int i = 0; i < streams.Length; i++)
				{
					if (!(streams[i].Name == "#~") && !(streams[i].Name == "#-") && !(streams[i].Name == "#Strings") && !(streams[i].Name == "#US") && !(streams[i].Name == "#Blob") && !(streams[i].Name == "#GUID"))
					{
						num4++;
						continue;
					}
					bool flag2 = false;
					for (int j = i + 1; j < streams.Length; j++)
					{
						if (streams[j].Name == streams[i].Name)
						{
							flag2 = true;
						}
					}
					if (flag2)
					{
						num4++;
						continue;
					}
					reader.BaseStream.Position = streams[i].headerpos;
					byte[] array2 = reader.ReadBytes(streams[i].Sizeofheader);
					if (streams[i].Name == "#GUID" && streams[i].Size % 16 != 0)
					{
						array2[4] = 16;
						array2[5] = 0;
						array2[6] = 0;
						array2[7] = 0;
						flag = true;
						txt.Text += "Set size of GUID stream to 16\r\n";
					}
					memoryStream.Write(array2, 0, array2.Length);
				}
			}
			if (RemoveStreams && (num4 > 0 || flag))
			{
				byte[] buffer = memoryStream.ToArray();
				binaryWriter.BaseStream.Position = streams[0].headerpos;
				binaryWriter.Write(buffer);
				if (num4 > 0)
				{
					binaryWriter.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + mh.VerionLenght + 18;
					short value = (short)(mh.NumberOfStreams - (short)num4);
					binaryWriter.Write(value);
					txt.Text = txt.Text + num4 + " metada streams removed\r\n";
				}
			}
			binaryWriter.Close();
			return true;
		}

		public unsafe bool RemoveMultiple(string filename, BinaryReader reader, byte[] filebytes, TextBox txt, bool removeMultiple)
		{
			txt.Text += "\r\nStage 4: Removing multiple definition\r\n";
			reader.BaseStream.Position = 0L;
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(filebytes));
			IntPtr ptr2;
			fixed (byte* ptr = reader.ReadBytes(sizeof(IMAGE_DOS_HEADER)))
			{
				ptr2 = (IntPtr)ptr;
			}
			idh = (IMAGE_DOS_HEADER)Marshal.PtrToStructure(ptr2, typeof(IMAGE_DOS_HEADER));
			if (idh.e_magic != 23117)
			{
				return false;
			}
			reader.BaseStream.Position = idh.e_lfanew;
			fixed (byte* ptr = reader.ReadBytes(sizeof(IMAGE_NT_HEADERS)))
			{
				ptr2 = (IntPtr)ptr;
			}
			inh = (IMAGE_NT_HEADERS)Marshal.PtrToStructure(ptr2, typeof(IMAGE_NT_HEADERS));
			if (inh.Signature != 17744)
			{
				return false;
			}
			reader.BaseStream.Position = idh.e_lfanew + 4 + sizeof(IMAGE_FILE_HEADER) + inh.ifh.SizeOfOptionalHeader;
			sections = new image_section_header[inh.ifh.NumberOfSections];
			long position = reader.BaseStream.Position;
			fixed (byte* ptr = reader.ReadBytes(sizeof(image_section_header) * inh.ifh.NumberOfSections))
			{
				ptr2 = (IntPtr)ptr;
			}
			for (int i = 0; i < sections.Length; i++)
			{
				ref image_section_header reference = ref sections[i];
				reference = (image_section_header)Marshal.PtrToStructure(ptr2, typeof(image_section_header));
				ptr2 = (IntPtr)(ptr2.ToInt32() + Marshal.SizeOf(typeof(image_section_header)));
			}
			if (inh.ioh.MetaDataDirectory.RVA <= 0)
			{
				txt.Text += "Can't continue processing MetaDataDirectory!\r\n";
				txt.Text += "Probabily invalid RVA!\r\n";
				return true;
			}
			long num = Rva2Offset(inh.ioh.MetaDataDirectory.RVA);
			if (num == 0)
			{
				txt.Text += "Can't continue processing MetaDataDirectory!\r\n";
				txt.Text += "Probabily invalid RVA!\r\n";
				return true;
			}
			reader.BaseStream.Position = num;
			fixed (byte* ptr = reader.ReadBytes(sizeof(NETDirectory)))
			{
				ptr2 = (IntPtr)ptr;
			}
			netdir = (NETDirectory)Marshal.PtrToStructure(ptr2, typeof(NETDirectory));
			reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA);
			mh = default(MetaDataHeader);
			mh.Signature = reader.ReadInt32();
			mh.MajorVersion = reader.ReadInt16();
			mh.MinorVersion = reader.ReadInt16();
			mh.Reserved = reader.ReadInt32();
			mh.VerionLenght = reader.ReadInt32();
			mh.VersionString = reader.ReadBytes(mh.VerionLenght);
			mh.Flags = reader.ReadInt16();
			mh.NumberOfStreams = reader.ReadInt16();
			streams = new MetaDataStream[mh.NumberOfStreams];
			int num2 = 0;
			for (int i = 0; i < mh.NumberOfStreams; i++)
			{
				streams[i].headerpos = (int)reader.BaseStream.Position;
				streams[i].Offset = reader.ReadInt32();
				streams[i].Size = reader.ReadInt32();
				char[] array = new char[32];
				int num3 = 0;
				byte b = 0;
				while ((b = reader.ReadByte()) != 0)
				{
					array[num3++] = (char)b;
				}
				num3++;
				int count = ((num3 % 4 != 0) ? (4 - num3 % 4) : 0);
				reader.ReadBytes(count);
				streams[i].Sizeofheader = (int)reader.BaseStream.Position - streams[i].headerpos;
				num2 += streams[i].Sizeofheader;
				ref MetaDataStream reference2 = ref streams[i];
				string text = new string(array);
				char[] trimChars = new char[1];
				reference2.Name = text.Trim(trimChars);
				if (MetadataRoot.Offset == 0 && MetadataRoot.Size == 0 && (streams[i].Name == "#~" || streams[i].Name == "#-"))
				{
					MetadataRoot.Name = streams[i].Name;
					MetadataRoot.Offset = streams[i].Offset;
					MetadataRoot.Size = streams[i].Size;
				}
				if (StringsStream.Offset == 0 && StringsStream.Size == 0 && streams[i].Name == "#Strings")
				{
					StringsStream.Name = streams[i].Name;
					StringsStream.Offset = streams[i].Offset;
					StringsStream.Size = streams[i].Size;
				}
				if (BlobOffset == 0 && streams[i].Name == "#Blob")
				{
					BlobOffset = streams[i].Offset;
					BlobSize = streams[i].Size;
				}
			}
			MemoryStream memoryStream = new MemoryStream();
			reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + MetadataRoot.Offset;
			fixed (byte* ptr = reader.ReadBytes(sizeof(TableHeader)))
			{
				ptr2 = (IntPtr)ptr;
			}
			tableheader = (TableHeader)Marshal.PtrToStructure(ptr2, typeof(TableHeader));
			TableLengths = new int[64];
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i < 64; i++)
			{
				if (i == 0)
				{
					num4 = (int)reader.BaseStream.Position;
				}
				if (i == 32)
				{
					num5 = (int)reader.BaseStream.Position;
				}
				int num6 = ((((tableheader.MaskValid >> i) & 1) != 0) ? reader.ReadInt32() : 0);
				TableLengths[i] = num6;
			}
			TablesOffset = reader.BaseStream.Position;
			InitTablesInfo();
			tablesize = new TableSize[45];
			tables = new Table[45];
			for (int i = 0; i < tablesize.Length; i++)
			{
				tablesize[i].Sizes = new int[tablesinfo[i].ctypes.Length];
				tablesize[i].TotalSize = 0;
				for (int j = 0; j < tablesinfo[i].ctypes.Length; j++)
				{
					tablesize[i].Sizes[j] = GetTypeSize(tablesinfo[i].ctypes[j]);
					tablesize[i].TotalSize = tablesize[i].TotalSize + tablesize[i].Sizes[j];
				}
			}
			if (removeMultiple && TableLengths[32] > 1)
			{
				long num7 = TablesOffset;
				for (int k = 0; k < 32; k++)
				{
					num7 += tablesize[k].TotalSize * TableLengths[k];
				}
				long position2 = num7 + tablesize[32].TotalSize;
				long num8 = num7 + tablesize[32].TotalSize * TableLengths[32];
				int count2 = Rva2Offset(netdir.MetaDataRVA) + MetadataRoot.Offset + MetadataRoot.Size - (int)num8;
				reader.BaseStream.Position = num8;
				byte[] buffer = reader.ReadBytes(count2);
				binaryWriter.BaseStream.Position = position2;
				binaryWriter.Write(buffer);
				binaryWriter.BaseStream.Position = num5;
				binaryWriter.Write(1);
				txt.Text = txt.Text + (TableLengths[32] - 1) + " assemblies definitions removed from tables\r\n";
			}
			if (removeMultiple && TableLengths[0] > 1)
			{
				long position2 = TablesOffset + tablesize[0].TotalSize;
				long num8 = TablesOffset + tablesize[0].TotalSize * TableLengths[0];
				int count2 = Rva2Offset(netdir.MetaDataRVA) + MetadataRoot.Offset + MetadataRoot.Size - (int)num8;
				reader.BaseStream.Position = num8;
				byte[] buffer = reader.ReadBytes(count2);
				binaryWriter.BaseStream.Position = position2;
				binaryWriter.Write(buffer);
				binaryWriter.BaseStream.Position = num4;
				binaryWriter.Write(1);
				txt.Text = txt.Text + (TableLengths[0] - 1) + " modules removed from tables\r\n";
			}
			binaryWriter.Close();
			return true;
		}

		public unsafe bool FixAssembly(string filename, BinaryReader reader, byte[] filebytes, TextBox txt, bool FixNameExced, bool Extends, bool FixMethods, bool FixNetResources, bool FixNested, bool setRet, bool FixPropertyMap, bool FixResSize, bool fixmodulemember, bool fixmethodbodysize, bool assemblyref, bool fixblob, bool ClassLayout)
		{
			txt.Text += "\r\nStage 5: Fixing .NET\r\n";
			reader.BaseStream.Position = 0L;
			BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(filebytes));
			IntPtr ptr2;
			fixed (byte* ptr = reader.ReadBytes(sizeof(IMAGE_DOS_HEADER)))
			{
				ptr2 = (IntPtr)ptr;
			}
			idh = (IMAGE_DOS_HEADER)Marshal.PtrToStructure(ptr2, typeof(IMAGE_DOS_HEADER));
			if (idh.e_magic != 23117)
			{
				return false;
			}
			reader.BaseStream.Position = idh.e_lfanew;
			fixed (byte* ptr = reader.ReadBytes(sizeof(IMAGE_NT_HEADERS)))
			{
				ptr2 = (IntPtr)ptr;
			}
			inh = (IMAGE_NT_HEADERS)Marshal.PtrToStructure(ptr2, typeof(IMAGE_NT_HEADERS));
			if (inh.Signature != 17744)
			{
				return false;
			}
			reader.BaseStream.Position = idh.e_lfanew + 4 + sizeof(IMAGE_FILE_HEADER) + inh.ifh.SizeOfOptionalHeader;
			sections = new image_section_header[inh.ifh.NumberOfSections];
			long position = reader.BaseStream.Position;
			fixed (byte* ptr = reader.ReadBytes(sizeof(image_section_header) * inh.ifh.NumberOfSections))
			{
				ptr2 = (IntPtr)ptr;
			}
			for (int i = 0; i < sections.Length; i++)
			{
				ref image_section_header reference = ref sections[i];
				reference = (image_section_header)Marshal.PtrToStructure(ptr2, typeof(image_section_header));
				ptr2 = (IntPtr)(ptr2.ToInt32() + Marshal.SizeOf(typeof(image_section_header)));
			}
			if (inh.ioh.MetaDataDirectory.RVA <= 0)
			{
				txt.Text += "Can't continue processing MetaDataDirectory!\r\n";
				txt.Text += "Probabily invalid RVA!\r\n";
				return true;
			}
			long num = Rva2Offset(inh.ioh.MetaDataDirectory.RVA);
			if (num == 0)
			{
				txt.Text += "Can't continue processing MetaDataDirectory!\r\n";
				txt.Text += "Probabily invalid RVA!\r\n";
				return true;
			}
			reader.BaseStream.Position = num;
			fixed (byte* ptr = reader.ReadBytes(sizeof(NETDirectory)))
			{
				ptr2 = (IntPtr)ptr;
			}
			netdir = (NETDirectory)Marshal.PtrToStructure(ptr2, typeof(NETDirectory));
			reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA);
			mh = default(MetaDataHeader);
			mh.Signature = reader.ReadInt32();
			mh.MajorVersion = reader.ReadInt16();
			mh.MinorVersion = reader.ReadInt16();
			mh.Reserved = reader.ReadInt32();
			mh.VerionLenght = reader.ReadInt32();
			mh.VersionString = reader.ReadBytes(mh.VerionLenght);
			mh.Flags = reader.ReadInt16();
			mh.NumberOfStreams = reader.ReadInt16();
			streams = new MetaDataStream[mh.NumberOfStreams];
			int num2 = 0;
			for (int i = 0; i < mh.NumberOfStreams; i++)
			{
				streams[i].headerpos = (int)reader.BaseStream.Position;
				streams[i].Offset = reader.ReadInt32();
				streams[i].Size = reader.ReadInt32();
				char[] array = new char[32];
				int num3 = 0;
				byte b = 0;
				while ((b = reader.ReadByte()) != 0)
				{
					array[num3++] = (char)b;
				}
				num3++;
				int count = ((num3 % 4 != 0) ? (4 - num3 % 4) : 0);
				reader.ReadBytes(count);
				streams[i].Sizeofheader = (int)reader.BaseStream.Position - streams[i].headerpos;
				num2 += streams[i].Sizeofheader;
				ref MetaDataStream reference2 = ref streams[i];
				string text = new string(array);
				char[] trimChars = new char[1];
				reference2.Name = text.Trim(trimChars);
				string name = streams[i].Name;
				if (MetadataRoot.Offset == 0 && MetadataRoot.Size == 0 && (streams[i].Name == "#~" || streams[i].Name == "#-"))
				{
					MetadataRoot.Name = streams[i].Name;
					MetadataRoot.Offset = streams[i].Offset;
					MetadataRoot.Size = streams[i].Size;
				}
				if (StringsStream.Offset == 0 && StringsStream.Size == 0 && streams[i].Name == "#Strings")
				{
					StringsStream.Name = streams[i].Name;
					StringsStream.Offset = streams[i].Offset;
					StringsStream.Size = streams[i].Size;
				}
				if (BlobOffset == 0 && streams[i].Name == "#Blob")
				{
					BlobOffset = streams[i].Offset;
					BlobSize = streams[i].Size;
				}
			}
			MemoryStream memoryStream = new MemoryStream();
			reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + MetadataRoot.Offset;
			fixed (byte* ptr = reader.ReadBytes(sizeof(TableHeader)))
			{
				ptr2 = (IntPtr)ptr;
			}
			tableheader = (TableHeader)Marshal.PtrToStructure(ptr2, typeof(TableHeader));
			TableLengths = new int[64];
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			for (int i = 0; i < 64; i++)
			{
				if (i == 41)
				{
					num4 = (int)reader.BaseStream.Position;
				}
				if (i == 21)
				{
					num5 = (int)reader.BaseStream.Position;
				}
				if (i == 15)
				{
					num6 = (int)reader.BaseStream.Position;
				}
				int num7 = ((((tableheader.MaskValid >> i) & 1) != 0) ? reader.ReadInt32() : 0);
				TableLengths[i] = num7;
			}
			TablesOffset = reader.BaseStream.Position;
			InitTablesInfo();
			tablesize = new TableSize[45];
			tables = new Table[45];
			for (int i = 0; i < tablesize.Length; i++)
			{
				tablesize[i].Sizes = new int[tablesinfo[i].ctypes.Length];
				tablesize[i].TotalSize = 0;
				for (int j = 0; j < tablesinfo[i].ctypes.Length; j++)
				{
					tablesize[i].Sizes[j] = GetTypeSize(tablesinfo[i].ctypes[j]);
					tablesize[i].TotalSize = tablesize[i].TotalSize + tablesize[i].Sizes[j];
				}
			}
			long num8 = TablesOffset;
			for (int k = 0; k < 1; k++)
			{
				num8 += tablesize[k].TotalSize * TableLengths[k];
			}
			long num9 = TablesOffset;
			for (int k = 0; k < 2; k++)
			{
				num9 += tablesize[k].TotalSize * TableLengths[k];
			}
			long num10 = TablesOffset;
			for (int k = 0; k < 4; k++)
			{
				num10 += tablesize[k].TotalSize * TableLengths[k];
			}
			long num11 = TablesOffset;
			for (int k = 0; k < 6; k++)
			{
				num11 += tablesize[k].TotalSize * TableLengths[k];
			}
			long num12 = TablesOffset;
			for (int k = 0; k < 8; k++)
			{
				num12 += tablesize[k].TotalSize * TableLengths[k];
			}
			long num13 = TablesOffset;
			for (int k = 0; k < 14; k++)
			{
				num13 += tablesize[k].TotalSize * TableLengths[k];
			}
			long num14 = TablesOffset;
			for (int k = 0; k < 15; k++)
			{
				num14 += tablesize[k].TotalSize * TableLengths[k];
			}
			if (FixNameExced)
			{
				for (int i = 0; i < TableLengths[2]; i++)
				{
					reader.BaseStream.Position = num9 + tablesize[2].TotalSize * i + 4;
					uint num15 = 0u;
					num15 = ((GetStringIndexSize() != 4) ? (reader.ReadUInt16() & 0xFFFFu) : reader.ReadUInt32());
					uint num16 = 0u;
					if (num15 >= 0 && num15 < StringsStream.Size)
					{
						reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + StringsStream.Offset + num15;
						for (byte b2 = reader.ReadByte(); b2 != 0; b2 = reader.ReadByte())
						{
							num16++;
						}
					}
					else
					{
						binaryWriter.BaseStream.Position = num9 + tablesize[2].TotalSize * i + 4;
						if (GetStringIndexSize() == 4)
						{
							binaryWriter.Write(0);
						}
						else
						{
							binaryWriter.Write((short)0);
						}
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix name of TypeDef with token " + (33554433 + i).ToString("X8") + "\r\n";
						}
					}
					if (num16 > 1000)
					{
						binaryWriter.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + StringsStream.Offset + num15 + 1000;
						binaryWriter.Write((byte)0);
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix name length exceed of TypeDef with token " + (33554433 + i).ToString("X8") + "\r\n";
						}
					}
					reader.BaseStream.Position = num9 + tablesize[2].TotalSize * i + 4 + GetStringIndexSize();
					num15 = 0u;
					num15 = ((GetStringIndexSize() != 4) ? (reader.ReadUInt16() & 0xFFFFu) : reader.ReadUInt32());
					num16 = 0u;
					if (num15 >= 0 && num15 < StringsStream.Size)
					{
						reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + StringsStream.Offset + num15;
						for (byte b2 = reader.ReadByte(); b2 != 0; b2 = reader.ReadByte())
						{
							num16++;
						}
					}
					else
					{
						binaryWriter.BaseStream.Position = num9 + tablesize[2].TotalSize * i + 4 + GetStringIndexSize();
						if (GetStringIndexSize() == 4)
						{
							binaryWriter.Write(0);
						}
						else
						{
							binaryWriter.Write((short)0);
						}
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix namespace of TypeDef with token " + (33554433 + i).ToString("X8") + "\r\n";
						}
					}
					if (num16 > 1000)
					{
						binaryWriter.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + StringsStream.Offset + num15 + 1000;
						binaryWriter.Write((byte)0);
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix namespace length exceed of TypeDef with token " + (33554433 + i).ToString("X8") + "\r\n";
						}
					}
				}
				for (int i = 0; i < TableLengths[4]; i++)
				{
					reader.BaseStream.Position = num10 + tablesize[4].TotalSize * i + 2;
					uint num15 = 0u;
					num15 = ((GetStringIndexSize() != 4) ? ((uint)reader.ReadInt16() & 0xFFFFu) : reader.ReadUInt32());
					uint num16 = 0u;
					if (num15 >= 0 && num15 < StringsStream.Size)
					{
						reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + StringsStream.Offset + num15;
						for (byte b2 = reader.ReadByte(); b2 != 0; b2 = reader.ReadByte())
						{
							num16++;
						}
					}
					else
					{
						binaryWriter.BaseStream.Position = num10 + tablesize[4].TotalSize * i + 2;
						if (GetStringIndexSize() == 4)
						{
							binaryWriter.Write(0);
						}
						else
						{
							binaryWriter.Write((short)0);
						}
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix name of Field with token " + (67108865 + i).ToString("X8") + "\r\n";
						}
					}
					if (num16 > 1000)
					{
						binaryWriter.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + StringsStream.Offset + num15 + 1000;
						binaryWriter.Write((byte)0);
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix name length exceed of Field with token " + (67108865 + i).ToString("X8") + "\r\n";
						}
					}
				}
				for (int i = 0; i < TableLengths[6]; i++)
				{
					reader.BaseStream.Position = num11 + tablesize[6].TotalSize * i + 8;
					uint num15 = 0u;
					num15 = ((GetStringIndexSize() != 4) ? (reader.ReadUInt16() & 0xFFFFu) : reader.ReadUInt32());
					uint num16 = 0u;
					if (num15 >= 0 && num15 < StringsStream.Size)
					{
						reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + StringsStream.Offset + num15;
						for (byte b2 = reader.ReadByte(); b2 != 0; b2 = reader.ReadByte())
						{
							num16++;
						}
					}
					else
					{
						binaryWriter.BaseStream.Position = num11 + tablesize[6].TotalSize * i + 8;
						if (GetStringIndexSize() == 4)
						{
							binaryWriter.Write(0);
						}
						else
						{
							binaryWriter.Write((short)0);
						}
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix name of Method with token " + (100663297 + i).ToString("X8") + "\r\n";
						}
					}
					if (num16 > 1000)
					{
						binaryWriter.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + StringsStream.Offset + num15 + 1000;
						binaryWriter.Write((byte)0);
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix name length exceed of Method with token " + (100663297 + i).ToString("X8") + "\r\n";
						}
					}
				}
				for (int i = 0; i < TableLengths[8]; i++)
				{
					reader.BaseStream.Position = num12 + tablesize[8].TotalSize * i + 4;
					uint num15 = 0u;
					num15 = ((GetStringIndexSize() != 4) ? (reader.ReadUInt16() & 0xFFFFu) : reader.ReadUInt32());
					uint num16 = 0u;
					if (num15 >= 0 && num15 < StringsStream.Size)
					{
						reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + StringsStream.Offset + num15;
						for (byte b2 = reader.ReadByte(); b2 != 0; b2 = reader.ReadByte())
						{
							num16++;
						}
					}
					else
					{
						binaryWriter.BaseStream.Position = num12 + tablesize[8].TotalSize * i + 4;
						if (GetStringIndexSize() == 4)
						{
							binaryWriter.Write(0);
						}
						else
						{
							binaryWriter.Write((short)0);
						}
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix name of Param with token " + (134217729 + i).ToString("X8") + "\r\n";
						}
					}
					if (num16 > 1000)
					{
						binaryWriter.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + StringsStream.Offset + num15 + 1000;
						binaryWriter.Write((byte)0);
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix name length exceed of Param with token " + (134217729 + i).ToString("X8") + "\r\n";
						}
					}
				}
			}
			if (Extends)
			{
				int num17 = -1;
				for (int i = 0; i < TableLengths[1]; i++)
				{
					reader.BaseStream.Position = num8 + tablesize[1].TotalSize * i + tablesize[1].Sizes[0];
					uint num18 = 0u;
					uint num19 = 0u;
					if (GetStringIndexSize() == 4)
					{
						num18 = reader.ReadUInt32();
						num19 = reader.ReadUInt32();
					}
					else
					{
						num18 = reader.ReadUInt16() & 0xFFFFu;
						num19 = reader.ReadUInt16() & 0xFFFFu;
					}
					char c = 'a';
					string text2 = "";
					reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + StringsStream.Offset + num18;
					while (c != 0)
					{
						c = (char)reader.ReadByte();
						if (c != 0)
						{
							text2 += c;
						}
					}
					if (text2 == "Object")
					{
						text2 = "";
						reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + StringsStream.Offset + num19;
						c = 'a';
						while (c != 0)
						{
							c = (char)reader.ReadByte();
							if (c != 0)
							{
								text2 += c;
							}
						}
						if (text2 == "System")
						{
							num17 = i;
						}
					}
					if (num17 != -1)
					{
						break;
					}
				}
				if (num17 != -1)
				{
					num17++;
					num17 = (num17 << 2) + 1;
					int typeSize = GetTypeSize(tablesinfo[2].ctypes[3]);
					for (int i = 0; i < TableLengths[1]; i++)
					{
						reader.BaseStream.Position = num8 + tablesize[1].TotalSize * i;
						uint num20 = 0u;
						num20 = ((tablesize[1].Sizes[0] != 4) ? (reader.ReadUInt16() & 0xFFFFu) : reader.ReadUInt32());
						uint num21 = num20 >> 2;
						uint num22 = num20 & 3u;
						if (num21 == 0 || (num22 == 0 && num21 > TableLengths[0]) || (num22 == 1 && num21 > TableLengths[26]) || (num22 == 2 && num21 > TableLengths[35]) || (num22 == 3 && num21 > TableLengths[1]))
						{
							binaryWriter.BaseStream.Position = num8 + tablesize[1].TotalSize * i;
							if (typeSize == 4)
							{
								binaryWriter.Write(num17 + 2);
							}
							else
							{
								binaryWriter.Write((short)(num17 + 2));
							}
							if (txt.Text.Length < 20000)
							{
								txt.Text = txt.Text + "Fix ResolutionScope of TypeRef with token " + (16777217 + i).ToString("X8") + "\r\n";
							}
						}
						else
						{
							if (num22 != 3)
							{
								continue;
							}
							reader.BaseStream.Position = num8 + tablesize[1].TotalSize * (num21 - 1);
							uint num23 = 0u;
							num23 = ((tablesize[1].Sizes[0] != 4) ? (reader.ReadUInt16() & 0xFFFFu) : reader.ReadUInt32());
							uint num24 = num23 & 3u;
							uint num25 = num23 >> 2;
							if (num24 == 3 && num25 == i + 1)
							{
								binaryWriter.BaseStream.Position = num8 + tablesize[1].TotalSize * i;
								if (typeSize == 4)
								{
									binaryWriter.Write(0);
								}
								else
								{
									binaryWriter.Write((short)0);
								}
								if (txt.Text.Length < 20000)
								{
									txt.Text = txt.Text + "Fix ResolutionScope of TypeRef with token " + (16777217 + i).ToString("X8") + "\r\n";
								}
								binaryWriter.BaseStream.Position = num8 + tablesize[1].TotalSize * num25;
								if (typeSize == 4)
								{
									binaryWriter.Write(0);
								}
								else
								{
									binaryWriter.Write((short)0);
								}
								if (txt.Text.Length < 20000)
								{
									txt.Text = txt.Text + "Fix ResolutionScope of TypeRef with token " + (16777217 + num25).ToString("X8") + "\r\n";
								}
							}
						}
					}
					for (int i = 0; i < TableLengths[2]; i++)
					{
						reader.BaseStream.Position = num9 + tablesize[2].TotalSize * i + 4 + 2 * GetStringIndexSize();
						uint num26 = 0u;
						num26 = ((typeSize != 4) ? (reader.ReadUInt16() & 0xFFFFu) : reader.ReadUInt32());
						if (num26 == 0)
						{
							continue;
						}
						uint num21 = num26 >> 2;
						uint num22 = num26 & 3u;
						if (num21 == 0 || (num22 == 0 && num21 > TableLengths[2]) || (num22 == 1 && num21 > TableLengths[1]) || (num22 == 2 && num21 > TableLengths[27]) || (num22 == 3 && num21 > TableLengths[0]))
						{
							binaryWriter.BaseStream.Position = num9 + tablesize[2].TotalSize * i + 4 + 2 * GetStringIndexSize();
							if (typeSize == 4)
							{
								binaryWriter.Write(num17);
							}
							else
							{
								binaryWriter.Write((short)num17);
							}
							if (txt.Text.Length < 20000)
							{
								txt.Text = txt.Text + "Fix extends of TypeDef with token " + (33554433 + i).ToString("X8") + "\r\n";
							}
						}
					}
					for (int i = 0; i < TableLengths[14]; i++)
					{
						reader.BaseStream.Position = num13 + tablesize[14].TotalSize * i;
						short num27 = (short)(reader.ReadInt16() & 0xFFFF);
						if (num27 < 0)
						{
							binaryWriter.BaseStream.Position = num13 + tablesize[14].TotalSize * i;
							binaryWriter.Write((short)0);
							if (txt.Text.Length < 20000)
							{
								txt.Text = txt.Text + "Fix Action of DeclSecurity with token " + (234881025 + i).ToString("X8") + "\r\n";
							}
						}
						uint num26 = 0u;
						num26 = ((tablesize[14].Sizes[1] != 4) ? (reader.ReadUInt16() & 0xFFFFu) : reader.ReadUInt32());
						uint num21 = num26 >> 2;
						uint num22 = num26 & 3u;
						if (num17 != -1 && (num26 == 0 || num21 == 0 || (num22 == 0 && num21 > TableLengths[2]) || (num22 == 1 && num21 > TableLengths[6]) || (num22 == 2 && num21 > TableLengths[32]) || (num22 == 3 && num21 > TableLengths[0])))
						{
							binaryWriter.BaseStream.Position = num13 + tablesize[14].TotalSize * i + 2;
							if (typeSize == 4)
							{
								binaryWriter.Write(num17);
							}
							else
							{
								binaryWriter.Write((short)num17);
							}
							if (txt.Text.Length < 20000)
							{
								txt.Text = txt.Text + "Fix Parent of DeclSecurity with token " + (234881025 + i).ToString("X8") + "\r\n";
							}
						}
						if (GetBlobIndexSize() == 4)
						{
							uint num28 = reader.ReadUInt32();
							if (num28 > (uint)BlobSize)
							{
								binaryWriter.BaseStream.Position = num13 + tablesize[14].TotalSize * i + 2 + tablesize[14].Sizes[1];
								binaryWriter.Write(0);
								if (txt.Text.Length < 20000)
								{
									txt.Text = txt.Text + "Fix PermissionSet of DeclSecurity with token " + (234881025 + i).ToString("X8") + "\r\n";
								}
							}
							continue;
						}
						ushort num29 = reader.ReadUInt16();
						if (num29 > (uint)BlobSize)
						{
							binaryWriter.BaseStream.Position = num13 + tablesize[14].TotalSize * i + 2 + tablesize[14].Sizes[1];
							binaryWriter.Write((short)0);
							if (txt.Text.Length < 20000)
							{
								txt.Text = txt.Text + "Fix PermissionSet of DeclSecurity with token " + (234881025 + i).ToString("X8") + "\r\n";
							}
						}
					}
				}
			}
			if (FixMethods || setRet || fixmethodbodysize)
			{
				for (int i = 0; i < TableLengths[6]; i++)
				{
					reader.BaseStream.Position = num11 + tablesize[6].TotalSize * i;
					int num30 = reader.ReadInt32();
					int num31 = Rva2Offset(num30);
					if (num30 == 0 || num31 == 0)
					{
						if (num30 != 0 && FixMethods)
						{
							binaryWriter.BaseStream.Position = num11 + tablesize[6].TotalSize * i;
							binaryWriter.Write(0);
							if (txt.Text.Length < 20000)
							{
								txt.Text = txt.Text + "Fix RVA of Method with token " + (100663297 + i).ToString("X8") + "\r\n";
							}
						}
						continue;
					}
					reader.BaseStream.Position = num31;
					byte b3 = (byte)(reader.ReadByte() & 0xFFu);
					if (FixMethods && (b3 & 3) == 3)
					{
						reader.BaseStream.Position = (long)num31 + 8L;
						int num32 = reader.ReadInt32();
						if (num32 != 0)
						{
							if ((num32 & 0xFF000000u) != 285212672)
							{
								binaryWriter.BaseStream.Position = (long)num31 + 8L + 3;
								binaryWriter.Write((byte)17);
								if (txt.Text.Length < 20000)
								{
									txt.Text = txt.Text + "Fix MethodSignature token of Method with token " + (100663297 + i).ToString("X8") + "\r\n";
								}
							}
							num32 &= 0xFFFFFF;
							if (num32 <= 0 || num32 > TableLengths[17])
							{
								binaryWriter.BaseStream.Position = (long)num31 + 8L;
								binaryWriter.Write(0);
								if (txt.Text.Length < 20000)
								{
									txt.Text = txt.Text + "Fix MethodSignature of Method with token " + (100663297 + i).ToString("X8") + "\r\n";
								}
							}
						}
					}
					if (!setRet && !fixmethodbodysize)
					{
						continue;
					}
					int num33;
					byte[] array2;
					bool flag;
					if ((b3 & 3) != 2)
					{
						reader.BaseStream.Position = (long)num31 + 4L;
						num33 = reader.ReadInt32();
						if (num33 < 0 || num33 + reader.BaseStream.Position >= reader.BaseStream.Length)
						{
							if (fixmethodbodysize)
							{
								int num34 = 0;
								reader.BaseStream.Position = num11 + tablesize[6].TotalSize * i + 8 + GetStringIndexSize();
								num34 = (int)((GetBlobIndexSize() != 4) ? (reader.ReadInt16() & 0xFFFF) : (reader.ReadInt32() & 0xFFFFFFFFu));
								reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + BlobOffset + num34 + 3;
								byte b4 = reader.ReadByte();
								binaryWriter.BaseStream.Position = num31;
								if (b4 != 1)
								{
									binaryWriter.Write((byte)10);
									binaryWriter.Write((short)10772);
								}
								else
								{
									binaryWriter.Write((short)10758);
								}
								if (txt.Text.Length < 20000)
								{
									txt.Text = txt.Text + "Fix body of Method with token " + (100663297 + i).ToString("X8") + "\r\n";
								}
							}
							continue;
						}
						reader.BaseStream.Position = (long)num31 + 12L;
						array2 = reader.ReadBytes(num33);
						flag = false;
						if (array2.Length > 0 && (array2[array2.Length - 1] == 42 || array2[array2.Length - 1] == 220 || array2[array2.Length - 1] == 122))
						{
							flag = true;
						}
						if (!flag && array2.Length > 8 && array2[array2.Length - 5] > 55 && array2[array2.Length - 5] < 69)
						{
							int num35 = BitConverter.ToInt32(array2, array2.Length - 4);
							if (num35 < 0)
							{
								num35 = -num35;
								if (num35 <= array2.Length)
								{
									flag = true;
								}
							}
						}
						if (!flag && array2.Length > 5 && array2[array2.Length - 1] > 127 && array2[array2.Length - 2] > 42 && array2[array2.Length - 2] < 56)
						{
							flag = true;
						}
						if (setRet && !flag)
						{
							int num34 = 0;
							reader.BaseStream.Position = num11 + tablesize[6].TotalSize * i + 8 + GetStringIndexSize();
							num34 = (int)((GetBlobIndexSize() != 4) ? (reader.ReadInt16() & 0xFFFF) : (reader.ReadInt32() & 0xFFFFFFFFu));
							reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + BlobOffset + num34 + 3;
							byte b4 = reader.ReadByte();
							binaryWriter.BaseStream.Position = num31;
							if (b4 != 1)
							{
								binaryWriter.Write((byte)10);
								binaryWriter.Write((short)10772);
							}
							else
							{
								binaryWriter.Write((short)10758);
							}
							if (txt.Text.Length < 20000)
							{
								txt.Text = txt.Text + "Fix body of Method with token " + (100663297 + i).ToString("X8") + "\r\n";
							}
						}
						continue;
					}
					num33 = b3;
					num33 >>= 2;
					if (num33 + reader.BaseStream.Position >= reader.BaseStream.Length)
					{
						if (fixmethodbodysize)
						{
							int num34 = 0;
							reader.BaseStream.Position = num11 + tablesize[6].TotalSize * i + 8 + GetStringIndexSize();
							num34 = (int)((GetBlobIndexSize() != 4) ? (reader.ReadInt16() & 0xFFFF) : (reader.ReadInt32() & 0xFFFFFFFFu));
							reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + BlobOffset + num34 + 3;
							byte b4 = reader.ReadByte();
							binaryWriter.BaseStream.Position = num31;
							if (b4 != 1)
							{
								binaryWriter.Write((byte)10);
								binaryWriter.Write((short)10772);
							}
							else
							{
								binaryWriter.Write((short)10758);
							}
							if (txt.Text.Length < 20000)
							{
								txt.Text = txt.Text + "Fix body of Method with token " + (100663297 + i).ToString("X8") + "\r\n";
							}
						}
						continue;
					}
					reader.BaseStream.Position = (long)num31 + 1L;
					array2 = reader.ReadBytes(num33);
					flag = false;
					if (array2.Length > 0 && (array2[array2.Length - 1] == 42 || array2[array2.Length - 1] == 220 || array2[array2.Length - 1] == 122))
					{
						flag = true;
					}
					if (!flag && array2.Length > 8 && array2[array2.Length - 5] > 55 && array2[array2.Length - 5] < 69)
					{
						int num35 = BitConverter.ToInt32(array2, array2.Length - 4);
						if (num35 < 0)
						{
							num35 = -num35;
							if (num35 <= array2.Length)
							{
								flag = true;
							}
						}
					}
					if (!flag && array2.Length > 5 && array2[array2.Length - 1] > 127 && array2[array2.Length - 2] > 42 && array2[array2.Length - 2] < 56)
					{
						flag = true;
					}
					if (setRet && !flag)
					{
						int num34 = 0;
						reader.BaseStream.Position = num11 + tablesize[6].TotalSize * i + 8 + GetStringIndexSize();
						num34 = (int)((GetBlobIndexSize() != 4) ? (reader.ReadInt16() & 0xFFFF) : (reader.ReadInt32() & 0xFFFFFFFFu));
						reader.BaseStream.Position = Rva2Offset(netdir.MetaDataRVA) + BlobOffset + num34 + 3;
						byte b4 = reader.ReadByte();
						binaryWriter.BaseStream.Position = num31;
						if (b4 != 1)
						{
							binaryWriter.Write((byte)10);
							binaryWriter.Write((short)10772);
						}
						else
						{
							binaryWriter.Write((short)10758);
						}
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix body of Method with token " + (100663297 + i).ToString("X8") + "\r\n";
						}
					}
				}
			}
			if ((FixNetResources || FixResSize) && Rva2Offset(netdir.ResourceRVA) != 0)
			{
				long num36 = TablesOffset;
				for (int k = 0; k < 40; k++)
				{
					num36 += tablesize[k].TotalSize * TableLengths[k];
				}
				uint num37 = 0u;
				long num38 = Rva2Offset(netdir.ResourceRVA);
				for (int i = 0; i < TableLengths[40]; i++)
				{
					reader.BaseStream.Position = num36 + tablesize[40].TotalSize * i;
					uint num39 = reader.ReadUInt32();
					if (num39 < 0 || num39 > netdir.ResourceSize)
					{
						if (FixNetResources)
						{
							binaryWriter.BaseStream.Position = num36 + tablesize[40].TotalSize * i;
							binaryWriter.Write(0);
							if (txt.Text.Length < 20000)
							{
								txt.Text = txt.Text + "Fix Offset of ManifestResource with token " + (671088641 + i).ToString("X8") + "\r\n";
							}
						}
					}
					else if (FixResSize)
					{
						try
						{
							if (num38 != 0)
							{
								reader.BaseStream.Position = num38 + num39;
								uint num40 = reader.ReadUInt32();
								if (num40 > netdir.ResourceSize)
								{
									uint num41 = 0u;
									if (i < TableLengths[40] - 1)
									{
										reader.BaseStream.Position = num36 + tablesize[40].TotalSize * (i + 1);
										uint num42 = reader.ReadUInt32();
										num41 = num42 - num39;
										num37 += num41;
									}
									else
									{
										num41 = (uint)netdir.ResourceSize - num37;
									}
									binaryWriter.BaseStream.Position = num38 + num39;
									binaryWriter.Write(num41);
									if (txt.Text.Length < 20000)
									{
										txt.Text = txt.Text + "Fix Size of ManifestResource with token " + (671088641 + i).ToString("X8") + "\r\n";
									}
								}
								else
								{
									num37 += num40;
								}
							}
						}
						catch
						{
						}
					}
					if (!FixNetResources)
					{
						continue;
					}
					reader.BaseStream.Position = num36 + tablesize[40].TotalSize * i + tablesize[40].TotalSize - tablesize[40].Sizes[3];
					uint num43 = 0u;
					num43 = ((tablesize[40].Sizes[3] != 2) ? reader.ReadUInt32() : ((uint)reader.ReadInt16() & 0xFFFFu));
					uint num21 = num43 >> 2;
					uint num22 = num43 & 3u;
					if ((num43 == 0 || num21 != 0) && (num22 != 0 || num21 <= TableLengths[38]) && (num22 != 1 || num21 <= TableLengths[35]) && (num22 != 2 || num21 <= TableLengths[0]) && (num22 != 3 || num21 <= TableLengths[0]))
					{
						continue;
					}
					if (tablesize[40].Sizes[3] == 2)
					{
						binaryWriter.BaseStream.Position = num36 + tablesize[40].TotalSize * i + tablesize[40].TotalSize - tablesize[40].Sizes[3];
						binaryWriter.Write((short)0);
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix Implementation of ManifestResource with token " + (671088641 + i).ToString("X8") + "\r\n";
						}
					}
					else
					{
						binaryWriter.BaseStream.Position = num36 + tablesize[40].TotalSize * i + tablesize[40].TotalSize - tablesize[40].Sizes[3];
						binaryWriter.Write(0);
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix Implementation of ManifestResource with token " + (671088641 + i).ToString("X8") + "\r\n";
						}
					}
				}
			}
			if (fixmodulemember && TableLengths[0] > 0)
			{
				long tablesOffset = TablesOffset;
				int guidIndexSize = GetGuidIndexSize();
				reader.BaseStream.Position = tablesOffset + tablesize[0].TotalSize - 2 * guidIndexSize;
				if (guidIndexSize == 2)
				{
					short num44 = reader.ReadInt16();
					short num45 = reader.ReadInt16();
					if (num44 < 0)
					{
						binaryWriter.BaseStream.Position = tablesOffset + tablesize[0].TotalSize - 2 * guidIndexSize;
						binaryWriter.Write((short)0);
						if (txt.Text.Length < 20000)
						{
							txt.Text += "Set EncId of Module with 0\r\n";
						}
					}
					if (num45 < 0)
					{
						binaryWriter.BaseStream.Position = tablesOffset + tablesize[0].TotalSize - guidIndexSize;
						binaryWriter.Write((short)0);
						if (txt.Text.Length < 20000)
						{
							txt.Text += "Set EncBaseId of Module with 0\r\n";
						}
					}
				}
				else
				{
					int num46 = reader.ReadInt32();
					int num47 = reader.ReadInt32();
					if (num46 < 0)
					{
						binaryWriter.BaseStream.Position = tablesOffset + tablesize[0].TotalSize - 2 * guidIndexSize;
						binaryWriter.Write(0);
						if (txt.Text.Length < 20000)
						{
							txt.Text += "Set EncId of Module with 0\r\n";
						}
					}
					if (num47 < 0)
					{
						binaryWriter.BaseStream.Position = tablesOffset + tablesize[0].TotalSize - guidIndexSize;
						binaryWriter.Write(0);
						if (txt.Text.Length < 20000)
						{
							txt.Text += "Set EncBaseId of Module with 0\r\n";
						}
					}
				}
			}
			if (assemblyref && TableLengths[35] > 0)
			{
				long num48 = TablesOffset;
				for (int k = 0; k < 35; k++)
				{
					num48 += tablesize[k].TotalSize * TableLengths[k];
				}
				for (int i = 0; i < TableLengths[35]; i++)
				{
					reader.BaseStream.Position = num48 + tablesize[35].TotalSize * i;
					ushort num49 = reader.ReadUInt16();
					if (num49 == ushort.MaxValue)
					{
						binaryWriter.BaseStream.Position = num48 + tablesize[35].TotalSize * i;
						binaryWriter.Write((ushort)0);
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix Major version of AssemblyRef number " + i + "\r\n";
						}
					}
					reader.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 2;
					ushort num50 = reader.ReadUInt16();
					if (num50 == ushort.MaxValue)
					{
						binaryWriter.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 2;
						binaryWriter.Write((ushort)0);
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix Minor version of AssemblyRef number " + i + "\r\n";
						}
					}
					reader.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 4;
					ushort num51 = reader.ReadUInt16();
					if (num51 == ushort.MaxValue)
					{
						binaryWriter.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 4;
						binaryWriter.Write((ushort)0);
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix BuildNumber of AssemblyRef number " + i + "\r\n";
						}
					}
					reader.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 6;
					ushort num52 = reader.ReadUInt16();
					if (num51 == ushort.MaxValue)
					{
						binaryWriter.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 6;
						binaryWriter.Write((ushort)0);
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix RevisionNumber of AssemblyRef number " + i + "\r\n";
						}
					}
					reader.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 8;
					uint num53 = reader.ReadUInt32();
					if (num53 == uint.MaxValue)
					{
						binaryWriter.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 8;
						binaryWriter.Write(0u);
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix Flags of AssemblyRef number " + i + "\r\n";
						}
					}
					uint num54 = 0u;
					reader.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 12;
					num54 = ((GetBlobIndexSize() != 4) ? (reader.ReadUInt16() & 0xFFFFu) : (reader.ReadUInt32() & 0xFFFFFFFFu));
					if (num54 > (uint)BlobSize)
					{
						binaryWriter.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 12;
						if (GetBlobIndexSize() == 4)
						{
							binaryWriter.Write(0);
						}
						else
						{
							binaryWriter.Write((short)0);
						}
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix PublicKeyOrToken of AssemblyRef number " + i + "\r\n";
						}
					}
					uint num55 = 0u;
					reader.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 12 + GetBlobIndexSize();
					num55 = ((GetStringIndexSize() != 4) ? (reader.ReadUInt16() & 0xFFFFu) : (reader.ReadUInt32() & 0xFFFFFFFFu));
					if (num55 > (uint)StringsStream.Size)
					{
						binaryWriter.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 12 + GetBlobIndexSize();
						if (GetStringIndexSize() == 4)
						{
							binaryWriter.Write(0);
						}
						else
						{
							binaryWriter.Write((short)0);
						}
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix Name of AssemblyRef number " + i + "\r\n";
						}
					}
					reader.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 12 + GetBlobIndexSize() + GetStringIndexSize();
					uint num56 = 0u;
					num56 = ((GetStringIndexSize() != 4) ? (reader.ReadUInt16() & 0xFFFFu) : (reader.ReadUInt32() & 0xFFFFFFFFu));
					if (num56 > (uint)StringsStream.Size)
					{
						binaryWriter.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 12 + GetBlobIndexSize() + GetStringIndexSize();
						if (GetStringIndexSize() == 4)
						{
							binaryWriter.Write(0);
						}
						else
						{
							binaryWriter.Write((short)0);
						}
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix Culture of AssemblyRef number " + i + "\r\n";
						}
					}
					uint num57 = 0u;
					reader.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 12 + GetBlobIndexSize() + 2 * GetStringIndexSize();
					num57 = ((GetBlobIndexSize() != 4) ? (reader.ReadUInt16() & 0xFFFFu) : (reader.ReadUInt32() & 0xFFFFFFFFu));
					if (num57 > (uint)BlobSize)
					{
						binaryWriter.BaseStream.Position = num48 + tablesize[35].TotalSize * i + 12 + GetBlobIndexSize() + 2 * GetStringIndexSize();
						if (GetBlobIndexSize() == 4)
						{
							binaryWriter.Write(0);
						}
						else
						{
							binaryWriter.Write((short)0);
						}
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix HashValue of AssemblyRef number " + i + "\r\n";
						}
					}
				}
			}
			if (fixblob)
			{
				for (int i = 0; i < TableLengths[6]; i++)
				{
					reader.BaseStream.Position = num11 + tablesize[6].TotalSize * i + 4 + 4 + GetStringIndexSize();
					uint num58 = 0u;
					num58 = ((GetBlobIndexSize() != 4) ? (reader.ReadUInt16() & 0xFFFFu) : (reader.ReadUInt32() & 0xFFFFFFFFu));
					if (num58 > BlobSize)
					{
						binaryWriter.BaseStream.Position = num11 + tablesize[6].TotalSize * i + 4 + 4 + GetStringIndexSize();
						if (GetBlobIndexSize() == 4)
						{
							binaryWriter.Write(0);
						}
						else
						{
							binaryWriter.Write((short)0);
						}
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix Signature of Method with token " + (100663297 + i).ToString("X8") + "\r\n";
						}
					}
				}
				for (int i = 0; i < TableLengths[4]; i++)
				{
					reader.BaseStream.Position = num10 + tablesize[4].TotalSize * i + 2 + GetStringIndexSize();
					uint num58 = 0u;
					num58 = ((GetBlobIndexSize() != 4) ? (reader.ReadUInt16() & 0xFFFFu) : (reader.ReadUInt32() & 0xFFFFFFFFu));
					if (num58 > BlobSize)
					{
						binaryWriter.BaseStream.Position = num10 + tablesize[4].TotalSize * i + 2 + GetStringIndexSize();
						if (GetBlobIndexSize() == 4)
						{
							binaryWriter.Write(0);
						}
						else
						{
							binaryWriter.Write((short)0);
						}
						if (txt.Text.Length < 20000)
						{
							txt.Text = txt.Text + "Fix Signature of Field with token " + (67108865 + i).ToString("X8") + "\r\n";
						}
					}
				}
			}
			if (FixNested && TableLengths[41] > 0)
			{
				long num59 = TablesOffset;
				for (int k = 0; k < 41; k++)
				{
					num59 += tablesize[k].TotalSize * TableLengths[k];
				}
				MemoryStream memoryStream2 = new MemoryStream();
				int num60 = 0;
				for (int i = 0; i < TableLengths[41]; i++)
				{
					bool flag2 = false;
					reader.BaseStream.Position = num59 + tablesize[41].TotalSize * i;
					uint num61 = 0u;
					num61 = ((tablesize[41].Sizes[0] != 2) ? reader.ReadUInt32() : (reader.ReadUInt16() & 0xFFFFu));
					uint num62 = 0u;
					num62 = ((tablesize[41].Sizes[1] != 2) ? reader.ReadUInt32() : (reader.ReadUInt16() & 0xFFFFu));
					if (num61 > TableLengths[2] || num62 > TableLengths[2])
					{
						flag2 = true;
					}
					if (!flag2)
					{
						for (int j = 0; j < i; j++)
						{
							reader.BaseStream.Position = num59 + tablesize[41].TotalSize * j;
							uint num63 = 0u;
							num63 = ((tablesize[41].Sizes[0] != 2) ? reader.ReadUInt32() : (reader.ReadUInt16() & 0xFFFFu));
							if (num61 == num63)
							{
								flag2 = true;
								break;
							}
						}
					}
					if (!flag2)
					{
						reader.BaseStream.Position = num59 + tablesize[41].TotalSize * i;
						byte[] array3 = reader.ReadBytes(tablesize[41].TotalSize);
						memoryStream2.Write(array3, 0, array3.Length);
					}
					else
					{
						num60++;
					}
				}
				if (num60 > 0)
				{
					byte[] buffer = memoryStream2.ToArray();
					binaryWriter.BaseStream.Position = num59;
					binaryWriter.Write(buffer);
					binaryWriter.BaseStream.Position = num4;
					binaryWriter.Write(TableLengths[41] - num60);
					if (txt.Text.Length < 22000)
					{
						txt.Text = txt.Text + num60 + " NestedClasses removed from tables\r\n";
					}
					long num64 = num59 + tablesize[41].TotalSize * TableLengths[41];
					long position2 = num59 + tablesize[41].TotalSize * (TableLengths[41] - num60);
					int count2 = (int)(Rva2Offset(netdir.MetaDataRVA) + MetadataRoot.Offset + MetadataRoot.Size - num64);
					reader.BaseStream.Position = num64;
					byte[] buffer2 = reader.ReadBytes(count2);
					binaryWriter.BaseStream.Position = position2;
					binaryWriter.Write(buffer2);
				}
			}
			if (FixPropertyMap && TableLengths[21] > 0)
			{
				long num65 = TablesOffset;
				for (int k = 0; k < 21; k++)
				{
					num65 += tablesize[k].TotalSize * TableLengths[k];
				}
				MemoryStream memoryStream2 = new MemoryStream();
				int num60 = 0;
				for (int i = 0; i < TableLengths[21]; i++)
				{
					reader.BaseStream.Position = num65 + tablesize[21].TotalSize * i;
					uint num66 = 0u;
					num66 = ((tablesize[21].Sizes[0] != 4) ? (reader.ReadUInt16() & 0xFFFFu) : reader.ReadUInt32());
					uint num67 = 0u;
					num67 = ((tablesize[21].Sizes[1] != 4) ? (reader.ReadUInt16() & 0xFFFFu) : reader.ReadUInt32());
					if (num66 == 0 || num66 > TableLengths[2] || num67 == 0 || num67 > TableLengths[23])
					{
						num60++;
						continue;
					}
					reader.BaseStream.Position = num65 + tablesize[21].TotalSize * i;
					byte[] array3 = reader.ReadBytes(tablesize[21].TotalSize);
					memoryStream2.Write(array3, 0, array3.Length);
				}
				if (num60 > 0)
				{
					byte[] buffer = memoryStream2.ToArray();
					binaryWriter.BaseStream.Position = num65;
					binaryWriter.Write(buffer);
					binaryWriter.BaseStream.Position = num5;
					binaryWriter.Write(TableLengths[21] - num60);
					if (txt.Text.Length < 22000)
					{
						txt.Text = txt.Text + num60 + " PropertyMaps removed from tables\r\n";
					}
					long num68 = num65 + tablesize[21].TotalSize * TableLengths[21];
					long position3 = num65 + tablesize[21].TotalSize * (TableLengths[21] - num60);
					int count2 = (int)(Rva2Offset(netdir.MetaDataRVA) + MetadataRoot.Offset + MetadataRoot.Size - num68);
					reader.BaseStream.Position = num68;
					byte[] buffer2 = reader.ReadBytes(count2);
					binaryWriter.BaseStream.Position = position3;
					binaryWriter.Write(buffer2);
				}
			}
			if (ClassLayout && TableLengths[15] > 0)
			{
				MemoryStream memoryStream2 = new MemoryStream();
				int num60 = 0;
				for (int i = 0; i < TableLengths[15]; i++)
				{
					reader.BaseStream.Position = num14 + tablesize[15].TotalSize * i + 2 + 4;
					uint num66 = 0u;
					num66 = ((GetTypeSize(Types.TypeDef) != 4) ? (reader.ReadUInt16() & 0xFFFFu) : (reader.ReadUInt32() & 0xFFFFFFFFu));
					if (num66 > TableLengths[2])
					{
						num60++;
						continue;
					}
					reader.BaseStream.Position = num14 + tablesize[15].TotalSize * i;
					byte[] array3 = reader.ReadBytes(tablesize[15].TotalSize);
					memoryStream2.Write(array3, 0, array3.Length);
				}
				if (num60 > 0)
				{
					byte[] buffer = memoryStream2.ToArray();
					binaryWriter.BaseStream.Position = num14;
					binaryWriter.Write(buffer);
					binaryWriter.BaseStream.Position = num6;
					binaryWriter.Write(TableLengths[15] - num60);
					if (txt.Text.Length < 22000)
					{
						txt.Text = txt.Text + num60 + " ClassLayouts removed from tables\r\n";
					}
					long num69 = num14 + tablesize[15].TotalSize * TableLengths[15];
					long position4 = num14 + tablesize[15].TotalSize * (TableLengths[15] - num60);
					int count2 = (int)(Rva2Offset(netdir.MetaDataRVA) + MetadataRoot.Offset + MetadataRoot.Size - num69);
					reader.BaseStream.Position = num69;
					byte[] buffer2 = reader.ReadBytes(count2);
					binaryWriter.BaseStream.Position = position4;
					binaryWriter.Write(buffer2);
				}
			}
			binaryWriter.Close();
			return true;
		}
	}
}
