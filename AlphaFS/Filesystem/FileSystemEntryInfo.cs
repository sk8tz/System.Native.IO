/*  Copyright (C) 2008-2015 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using System;
using System.Globalization;
using System.IO;
using System.Security;

namespace System.Native.IO.FileSystem
{
   /// <summary>Represents information about a file system entry.
   /// <para>This class cannot be inherited.</para>
   /// </summary>
   [SerializableAttribute]
   [SecurityCritical]
   public sealed class FileSystemEntryInfo
   {
      #region Constructor

      /// <summary>Initializes a new instance of the <see cref="FileSystemEntryInfo"/> class.</summary>
      /// <param name="findData">The NativeMethods.WIN32_FIND_DATA structure.</param>
      internal FileSystemEntryInfo(NativeMethods.WIN32_FIND_DATA findData)
      {
         _win32FindData = findData;
      }

      #endregion // Constructor
      
      #region Properties

      #region AlternateFileName

      /// <summary>Gets the 8.3 version of the filename.</summary>
      /// <value>the 8.3 version of the filename.</value>
      public string AlternateFileName
      {
         get { return _win32FindData.cAlternateFileName; }
      }

      #endregion // AlternateFileName

      #region Attributes

      /// <summary>Gets the attributes.</summary>
      /// <value>The attributes.</value>
      public FileAttributes Attributes
      {
         get { return _win32FindData.dwFileAttributes; }
      }

      #endregion // Attributes

      #region CreationTime

      /// <summary>Gets the time this entry was created.</summary>
      /// <value>The time this entry was created.</value>
      public DateTime CreationTime
      {
         get { return CreationTimeUtc.ToLocalTime(); }
      }

      #endregion // CreationTime

      #region CreationTimeUtc

      /// <summary>Gets the time, in coordinated universal time (UTC), this entry was created.</summary>
      /// <value>The time, in coordinated universal time (UTC), this entry was created.</value>
      public DateTime CreationTimeUtc
      {
         get { return DateTime.FromFileTimeUtc(_win32FindData.ftCreationTime); }
      }

      #endregion // CreationTimeUtc

      #region FileName

      /// <summary>Gets the name of the file.</summary>
      /// <value>The name of the file.</value>
      public string FileName
      {
         get { return _win32FindData.cFileName; }
      }

      #endregion // FileName

      #region FileSize

      /// <summary>Gets the size of the file.</summary>
      /// <value>The size of the file.</value>
      public long FileSize
      {
         get { return NativeMethods.ToLong(_win32FindData.nFileSizeHigh, _win32FindData.nFileSizeLow); }
      }

      #endregion // FileSize

      #region FullPath

      private string _fullPath;

      /// <summary>The full path of the file system object.</summary>
      public string FullPath
      {
         get { return _fullPath; }
         set
         {
            LongFullPath = value;
            _fullPath = Path.GetRegularPathCore(LongFullPath, GetFullPathOptions.None);
         }
      }

      #endregion // FullPath

      #region IsDirectory

      /// <summary>Gets a value indicating whether this instance represents a directory.</summary>
      /// <value><see langword="true"/> if this instance represents a directory; otherwise, <see langword="false"/>.</value>
      public bool IsDirectory
      {
         get { return Attributes != (FileAttributes)(-1) && (Attributes & FileAttributes.Directory) == FileAttributes.Directory; }
      }

      #endregion // IsDirectory

      #region IsMountPoint

      /// <summary>Gets a value indicating whether this instance is a mount point.</summary>
      /// <value><see langword="true"/> if this instance is a mount point; otherwise, <see langword="false"/>.</value>
      public bool IsMountPoint
      {
         get { return ReparsePointTag == ReparsePointTag.MountPoint; }
      }

      #endregion // IsMountPoint

      #region IsReparsePoint

      /// <summary>Gets a value indicating whether this instance is a reparse point.</summary>
      /// <value><see langword="true"/> if this instance is a reparse point; otherwise, <see langword="false"/>.</value>
      public bool IsReparsePoint
      {
         get { return Attributes != (FileAttributes)(-1) && (Attributes & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint; }
      }

      #endregion // IsReparsePoint

      #region IsSymbolicLink

      /// <summary>Gets a value indicating whether this instance is a symbolic link.</summary>
      /// <value><see langword="true"/> if this instance is a symbolic link; otherwise, <see langword="false"/>.</value>
      public bool IsSymbolicLink
      {
         get { return ReparsePointTag == ReparsePointTag.SymLink; }
      }

      #endregion // IsSymbolicLink

      #region LastAccessTime

      /// <summary>Gets the time this entry was last accessed.</summary>
      /// <value>The time this entry was last accessed.</value>
      public DateTime LastAccessTime
      {
         get { return LastAccessTimeUtc.ToLocalTime(); }
      }

      #endregion // LastAccessTime

      #region LastAccessTimeUtc

      /// <summary>Gets the time, in coordinated universal time (UTC), this entry was last accessed.</summary>
      /// <value>The time, in coordinated universal time (UTC), this entry was last accessed.</value>
      public DateTime LastAccessTimeUtc
      {
         get { return DateTime.FromFileTimeUtc(_win32FindData.ftLastAccessTime); }
      }

      #endregion // LastAccessTimeUtc

      #region LastWriteTime

      /// <summary>Gets the time this entry was last modified.</summary>
      /// <value>The time this entry was last modified.</value>
      public DateTime LastWriteTime
      {
         get { return LastWriteTimeUtc.ToLocalTime(); }
      }

      #endregion // LastWriteTime
      
      #region LastWriteTimeUtc

      /// <summary>Gets the time, in coordinated universal time (UTC), this entry was last modified.</summary>
      /// <value>The time, in coordinated universal time (UTC), this entry was last modified.</value>
      public DateTime LastWriteTimeUtc
      {
         get { return DateTime.FromFileTimeUtc(_win32FindData.ftLastWriteTime); }
      }

      #endregion // LastWriteTimeUtc

      #region LongFullPath

      private string _longFullPath;

      /// <summary>The full path of the file system object in Unicode (LongPath) format.</summary>
      public string LongFullPath
      {
         get { return _longFullPath; }
         private set { _longFullPath = Path.GetLongPathCore(value, GetFullPathOptions.None); }
      }

      #endregion // LongFullPath

      #region ReparsePointTag

      /// <summary>Gets the reparse point tag of this entry.</summary>
      /// <value>The reparse point tag of this entry.</value>
      public ReparsePointTag ReparsePointTag
      {
         get { return IsReparsePoint ? _win32FindData.dwReserved0 : ReparsePointTag.None; }
      }

      #endregion // ReparsePointTag

      #region WIN32_FIND_DATA
      
      private readonly NativeMethods.WIN32_FIND_DATA _win32FindData;

      /// <summary>Gets internal WIN32 FIND Data</summary>
      internal NativeMethods.WIN32_FIND_DATA Win32FindData
      {
         get { return _win32FindData; }
      }

      #endregion // WIN32_FIND_DATA

      #endregion // Properties

      #region Methods

      /// <summary>Returns the <see cref="ReparsePointTag"/> of the <see cref="FileSystemEntryInfo"/> instance.</summary>
      /// <returns>The <see cref="ReparsePointTag"/> instance as a string.</returns>
      public override string ToString()
      {
         return string.Format(CultureInfo.InvariantCulture, "{0}", ReparsePointTag);
      }

      #endregion // Methods
   }
}
