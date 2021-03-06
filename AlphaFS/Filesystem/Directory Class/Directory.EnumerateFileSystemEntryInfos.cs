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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;

namespace System.Native.IO.FileSystem
{
   partial class Directory
   {
      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries in a specified path.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path)
      {
         return EnumerateFileSystemEntryInfosCore<T>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries in a specified path.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosCore<T>(null, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, pathFormat);
      }



      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries in a specified path.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, DirectoryEnumerationOptions options)
      {
         return EnumerateFileSystemEntryInfosCore<T>(null, path, Path.WildcardStarMatchAll, options, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries in a specified path.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosCore<T>(null, path, Path.WildcardStarMatchAll, options, pathFormat);
      }



      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern" /> in a specified path.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   The search string to match against the names of directories in <paramref name="path"/>.
      ///   This parameter can contain a combination of valid literal path and wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>) characters, but does not support regular expressions.
      /// </param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosCore<T>(null, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   The search string to match against the names of directories in <paramref name="path"/>.
      ///   This parameter can contain a combination of valid literal path and wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>) characters, but does not support regular expressions.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosCore<T>(null, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, pathFormat);
      }



      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path using <see cref="DirectoryEnumerationOptions"/>.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   The search string to match against the names of directories in <paramref name="path"/>.
      ///   This parameter can contain a combination of valid literal path and wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>) characters, but does not support regular expressions.
      /// </param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern, DirectoryEnumerationOptions options)
      {
         return EnumerateFileSystemEntryInfosCore<T>(null, path, searchPattern, options, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path using <see cref="DirectoryEnumerationOptions"/>.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   The search string to match against the names of directories in <paramref name="path"/>.
      ///   This parameter can contain a combination of valid literal path and wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>) characters, but does not support regular expressions.
      /// </param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfos<T>(string path, string searchPattern, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosCore<T>(null, path, searchPattern, options, pathFormat);
      }

      #region Transactional

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries in a specified path.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfosTransacted<T>(KernelTransaction transaction, string path)
      {
         return EnumerateFileSystemEntryInfosCore<T>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries in a specified path.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfosTransacted<T>(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosCore<T>(transaction, path, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, pathFormat);
      }



      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries in a specified path.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfosTransacted<T>(KernelTransaction transaction, string path, DirectoryEnumerationOptions options)
      {
         return EnumerateFileSystemEntryInfosCore<T>(transaction, path, Path.WildcardStarMatchAll, options, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries in a specified path.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfosTransacted<T>(KernelTransaction transaction, string path, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosCore<T>(transaction, path, Path.WildcardStarMatchAll, options, pathFormat);
      }



      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   The search string to match against the names of directories in <paramref name="path"/>.
      ///   This parameter can contain a combination of valid literal path and wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>) characters, but does not support regular expressions.
      /// </param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfosTransacted<T>(KernelTransaction transaction, string path, string searchPattern)
      {
         return EnumerateFileSystemEntryInfosCore<T>(transaction, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   The search string to match against the names of directories in <paramref name="path"/>.
      ///   This parameter can contain a combination of valid literal path and wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>) characters, but does not support regular expressions.
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfosTransacted<T>(KernelTransaction transaction, string path, string searchPattern, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosCore<T>(transaction, path, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, pathFormat);
      }



      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path using <see cref="DirectoryEnumerationOptions"/>.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   The search string to match against the names of directories in <paramref name="path"/>.
      ///   This parameter can contain a combination of valid literal path and wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>) characters, but does not support regular expressions.
      /// </param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfosTransacted<T>(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions options)
      {
         return EnumerateFileSystemEntryInfosCore<T>(transaction, path, searchPattern, options, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of file system entries that match a <paramref name="searchPattern"/> in a specified path using <see cref="DirectoryEnumerationOptions"/>.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   The search string to match against the names of directories in <paramref name="path"/>.
      ///   This parameter can contain a combination of valid literal path and wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>) characters, but does not support regular expressions.
      /// </param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public static IEnumerable<T> EnumerateFileSystemEntryInfosTransacted<T>(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         return EnumerateFileSystemEntryInfosCore<T>(transaction, path, searchPattern, options, pathFormat);
      }
      
      #endregion // Transactional

      #region Internal Methods

      /// <summary>Returns an enumerable collection of file system entries in a specified path using <see cref="DirectoryEnumerationOptions"/>.</summary>
      /// <returns>The matching file system entries. The type of the items is determined by the type <typeparamref name="T"/>.</returns>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <typeparam name="T">The type to return. This may be one of the following types:
      ///    <list type="definition">
      ///    <item>
      ///       <term><see cref="FileSystemEntryInfo"/></term>
      ///       <description>This method will return instances of <see cref="FileSystemEntryInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="FileSystemInfo"/></term>
      ///       <description>This method will return instances of <see cref="DirectoryInfo"/> and <see cref="FileInfo"/> instances.</description>
      ///    </item>
      ///    <item>
      ///       <term><see cref="string"/></term>
      ///       <description>This method will return the full path of each item.</description>
      ///    </item>
      /// </list>
      /// </typeparam>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The directory to search.</param>
      /// <param name="searchPattern">
      ///   The search string to match against the names of directories in <paramref name="path"/>.
      ///   This parameter can contain a combination of valid literal path and wildcard
      ///   (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>) characters, but does not support regular expressions.
      /// </param>
      /// <param name="options"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static IEnumerable<T> EnumerateFileSystemEntryInfosCore<T>(KernelTransaction transaction, string path, string searchPattern, DirectoryEnumerationOptions options, PathFormat pathFormat)
      {
         // Enable BasicSearch and LargeCache by default.
         options |= DirectoryEnumerationOptions.BasicSearch | DirectoryEnumerationOptions.LargeCache;

         return (new FindFileSystemEntryInfo(true, transaction, path, searchPattern, options, typeof(T), pathFormat)).Enumerate<T>();
      }

      #endregion // Internal Methods
   }
}
