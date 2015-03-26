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

using System.Native.IO;
using System.Native.IO.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32.SafeHandles;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Directory = System.Native.IO.FileSystem.Directory;
using DriveInfo = System.Native.IO.FileSystem.DriveInfo;
using File = System.Native.IO.FileSystem.File;
using NativeMethods = System.Native.IO.FileSystem.NativeMethods;
using Path = System.Native.IO.FileSystem.Path;

namespace AlphaFS.UnitTest
{
   /// <summary>This is a test class for Path and is intended to contain all Path Unit Tests.</summary>
   [TestClass]
   public class PathTest
   {
      #region Unit Tests

      private void Dump83Path(bool isLocal)
      {
         #region Setup

         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         string myLongPath = Path.GetTempPath("My Long Data File Or Directory");
         if (!isLocal) myLongPath = Path.LocalToUnc(myLongPath);

         Console.WriteLine("\nInput Path: [{0}]\n", myLongPath);

         #endregion // Setup

         #region File

         string short83Path;

         try
         {
            using (File.Create(myLongPath))

            UnitTestConstants.StopWatcher(true);

            short83Path = Path.GetShort83Path(myLongPath);

            Console.WriteLine("Short 8.3 file path    : [{0}]\t\t\t{1}", short83Path, UnitTestConstants.Reporter(true));

            Assert.IsTrue(!short83Path.Equals(myLongPath));

            Assert.IsTrue(short83Path.EndsWith(@"~1"));



            UnitTestConstants.StopWatcher(true);

            string longFrom83Path = Path.GetLongFrom83ShortPath(short83Path);

            Console.WriteLine("Long path from 8.3 path: [{0}]{1}", longFrom83Path, UnitTestConstants.Reporter(true));

            Assert.IsTrue(longFrom83Path.Equals(myLongPath));

            Assert.IsFalse(longFrom83Path.EndsWith(@"~1"));

         }
         catch (Exception ex)
         {
            Console.WriteLine("Caught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         finally
         {
            if (File.Exists(myLongPath))
               File.Delete(myLongPath);
         }
         Console.WriteLine();

         #endregion // File

         #region Directory

         try
         {
            Directory.CreateDirectory(myLongPath);

            UnitTestConstants.StopWatcher(true);

            short83Path = Path.GetShort83Path(myLongPath);

            Console.WriteLine("Short 8.3 directory path: [{0}]\t\t\t{1}", short83Path, UnitTestConstants.Reporter(true));

            Assert.IsFalse(short83Path.Equals(myLongPath));

            Assert.IsTrue(short83Path.EndsWith(@"~1"));



            UnitTestConstants.StopWatcher(true);

            string longFrom83Path = Path.GetLongFrom83ShortPath(short83Path);

            Console.WriteLine("Long path from 8.3 path : [{0}]{1}", longFrom83Path, UnitTestConstants.Reporter(true));

            Assert.IsTrue(longFrom83Path.Equals(myLongPath));

            Assert.IsFalse(longFrom83Path.EndsWith(@"~1"));
         }
         catch (Exception ex)
         {
            Console.WriteLine("Caught (unexpected) {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
         }
         finally
         {
            if (Directory.Exists(myLongPath))
               Directory.Delete(myLongPath);
         }
         Console.WriteLine();

         #endregion // Directory
      }

      private void DumpGetDirectoryNameWithoutRoot(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         const string neDir = "Non-Existing Directory";
         const string sys32 = "system32";

         string fullPath = Path.Combine(Environment.SystemDirectory, neDir);
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         string directoryNameWithoutRoot = Path.GetDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetDirectoryNameWithoutRoot() (Should be: [{1}]): [{2}]", fullPath, sys32, directoryNameWithoutRoot);
         Assert.IsTrue(directoryNameWithoutRoot.Equals(sys32, StringComparison.OrdinalIgnoreCase));



         fullPath = Path.Combine(fullPath, "Non-Existing file.txt");
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         directoryNameWithoutRoot = Path.GetDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetDirectoryNameWithoutRoot() (Should be: [{1}]): [{2}]", fullPath, neDir, directoryNameWithoutRoot);
         Assert.IsTrue(directoryNameWithoutRoot.Equals(neDir, StringComparison.OrdinalIgnoreCase));



         fullPath = UnitTestConstants.SysRoot;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         directoryNameWithoutRoot = Path.GetDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetDirectoryNameWithoutRoot() (Should be: [null]): [{1}]", fullPath, directoryNameWithoutRoot ?? "null");
         Assert.AreEqual(null, directoryNameWithoutRoot);

         Console.WriteLine("\n");
      }

      private void DumpGetFinalPathNameByHandle(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         string tempFile = Path.GetTempFileName();
         if (!isLocal) tempFile = Path.LocalToUnc(tempFile);

         string longTempStream = Path.LongPathPrefix + tempFile;
         bool gotFileNameNormalized;
         bool gotFileNameOpened;
         bool gotVolumeNameDos;
         bool gotVolumeNameGuid;
         bool gotVolumeNameNt;
         bool gotVolumeNameNone;
         bool gotSomething;

         using (FileStream stream = File.Create(tempFile))
         {
            // For Windows versions < Vista, the file must be > 0 bytes.
            if (!NativeMethods.IsAtLeastWindowsVista)
               stream.WriteByte(1);

            SafeFileHandle handle = stream.SafeFileHandle;

            UnitTestConstants.StopWatcher(true);
            string fileNameNormalized = Path.GetFinalPathNameByHandle(handle);
            string fileNameOpened = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.FileNameOpened);

            string volumeNameDos = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.None);
            string volumeNameGuid = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.VolumeNameGuid);
            string volumeNameNt = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.VolumeNameNT);
            string volumeNameNone = Path.GetFinalPathNameByHandle(handle, FinalPathFormats.VolumeNameNone);

            // These three output the same.
            gotFileNameNormalized = !string.IsNullOrWhiteSpace(fileNameNormalized) && longTempStream.Equals(fileNameNormalized);
            gotFileNameOpened = !string.IsNullOrWhiteSpace(fileNameOpened) && longTempStream.Equals(fileNameOpened);
            gotVolumeNameDos = !string.IsNullOrWhiteSpace(volumeNameDos) && longTempStream.Equals(volumeNameDos);

            gotVolumeNameGuid = !string.IsNullOrWhiteSpace(volumeNameGuid) && volumeNameGuid.StartsWith(Path.VolumePrefix) && volumeNameGuid.EndsWith(volumeNameNone);
            gotVolumeNameNt = !string.IsNullOrWhiteSpace(volumeNameNt) && volumeNameNt.StartsWith(Path.DevicePrefix);
            gotVolumeNameNone = !string.IsNullOrWhiteSpace(volumeNameNone) && tempFile.EndsWith(volumeNameNone);

            Console.WriteLine("\nInput Path: [{0}]", tempFile);
            Console.WriteLine("\n\tFilestream.Name  : [{0}]", stream.Name);
            Console.WriteLine("\tFilestream.Length: [{0}] (Note: For Windows versions < Vista, the file must be > 0 bytes.)\n", Utils.UnitSizeToText(stream.Length));

            Console.WriteLine("\tFinalPathFormats.None          : [{0}]", fileNameNormalized);
            Console.WriteLine("\tFinalPathFormats.FileNameOpened: [{0}]", fileNameOpened);
            Console.WriteLine("\tFinalPathFormats.VolumeNameDos : [{0}]", volumeNameDos);
            Console.WriteLine("\tFinalPathFormats.VolumeNameGuid: [{0}]", volumeNameGuid);
            Console.WriteLine("\tFinalPathFormats.VolumeNameNT  : [{0}]", volumeNameNt);
            Console.WriteLine("\tFinalPathFormats.VolumeNameNone: [{0}]", volumeNameNone);

            Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

            gotSomething = true;
         }


         bool fileExists = File.Exists(tempFile);

         File.Delete(tempFile, true);
         Assert.IsFalse(File.Exists(tempFile), "Cleanup failed: File should have been removed.");

         Assert.IsTrue(fileExists);
         Assert.IsTrue(gotFileNameNormalized);
         Assert.IsTrue(gotFileNameOpened);
         Assert.IsTrue(gotVolumeNameDos);
         Assert.IsTrue(gotVolumeNameGuid);
         Assert.IsTrue(gotVolumeNameNt);
         Assert.IsTrue(gotVolumeNameNone);
         Assert.IsTrue(gotSomething);
      }

      private void DumpGetSuffixedDirectoryName(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         string neDir = "Non-Existing Directory";
         string sys32 = Environment.SystemDirectory + Path.DirectorySeparator;

         string fullPath = Path.Combine(Environment.SystemDirectory, neDir);
         if (!isLocal)
         {
            fullPath = Path.LocalToUnc(fullPath);
            sys32 = Path.LocalToUnc(sys32);
         }

         UnitTestConstants.StopWatcher(true);
         string suffixedDirectoryName = Path.GetSuffixedDirectoryName(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [{1}]): [{2}]\n{3}", fullPath, sys32, suffixedDirectoryName, UnitTestConstants.Reporter());
         Assert.IsTrue(suffixedDirectoryName.Equals(sys32, StringComparison.OrdinalIgnoreCase));



         fullPath = Path.Combine(fullPath, "Non-Existing file.txt");
         neDir = Path.Combine(Environment.SystemDirectory, neDir) + Path.DirectorySeparator;
         if (!isLocal)
         {
            fullPath = Path.LocalToUnc(fullPath);
            neDir = Path.LocalToUnc(neDir);
         }

         UnitTestConstants.StopWatcher(true);
         suffixedDirectoryName = Path.GetSuffixedDirectoryName(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [{1}]): [{2}]\n{3}", fullPath, neDir, suffixedDirectoryName, UnitTestConstants.Reporter());
         Assert.IsTrue(suffixedDirectoryName.Equals(neDir, StringComparison.OrdinalIgnoreCase));



         fullPath = UnitTestConstants.SysRoot;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         suffixedDirectoryName = Path.GetSuffixedDirectoryName(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryName ?? "null", UnitTestConstants.Reporter());
         Assert.AreEqual(null, suffixedDirectoryName);


         fullPath = UnitTestConstants.SysDrive + Path.DirectorySeparator;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         suffixedDirectoryName = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryName ?? "null", UnitTestConstants.Reporter());
         Assert.AreEqual(null, suffixedDirectoryName);

         Console.WriteLine("\n");
      }

      private void DumpGetSuffixedDirectoryNameWithoutRoot(bool isLocal)
      {
         Console.WriteLine("\n=== TEST {0} ===", isLocal ? UnitTestConstants.Local : UnitTestConstants.Network);

         string neDir = "Non-Existing Directory";
         string sys32 = (UnitTestConstants.SysRoot + Path.DirectorySeparator + "system32" + Path.DirectorySeparator).Replace(UnitTestConstants.SysDrive + Path.DirectorySeparator, "");

         string fullPath = Path.Combine(UnitTestConstants.SysRoot + Path.DirectorySeparator + "system32" + Path.DirectorySeparator, neDir);
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         string suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [{1}]): [{2}]\n{3}", fullPath, sys32, suffixedDirectoryNameWithoutRoot, UnitTestConstants.Reporter());
         Assert.IsTrue(suffixedDirectoryNameWithoutRoot.Equals(sys32, StringComparison.OrdinalIgnoreCase), "Path mismatch.");



         fullPath = Path.Combine(fullPath, "Non-Existing file.txt");
         neDir = (Path.Combine(Environment.SystemDirectory, neDir) + Path.DirectorySeparator).Replace(UnitTestConstants.SysDrive + Path.DirectorySeparator, "");
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [{1}]): [{2}]\n{3}", fullPath, neDir, suffixedDirectoryNameWithoutRoot, UnitTestConstants.Reporter());
         Assert.IsTrue(suffixedDirectoryNameWithoutRoot.Equals(neDir, StringComparison.OrdinalIgnoreCase), "Path mismatch.");



         fullPath = UnitTestConstants.SysRoot;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryNameWithoutRoot ?? "null", UnitTestConstants.Reporter());
         Assert.AreEqual(null, suffixedDirectoryNameWithoutRoot, "Path mismatch.");



         fullPath = UnitTestConstants.SysDrive + Path.DirectorySeparator;
         if (!isLocal) fullPath = Path.LocalToUnc(fullPath);

         UnitTestConstants.StopWatcher(true);
         suffixedDirectoryNameWithoutRoot = Path.GetSuffixedDirectoryNameWithoutRoot(fullPath);
         Console.WriteLine("\nInput Path: [{0}]\n\tGetSuffixedDirectoryName() (Should be: [null]): [{1}]\n{2}", fullPath, suffixedDirectoryNameWithoutRoot ?? "null", UnitTestConstants.Reporter());
         Assert.AreEqual(null, suffixedDirectoryNameWithoutRoot, "Path mismatch.");

         Console.WriteLine("\n");
      }

      #endregion // Unit Tests

      #region Unit Test Callers

      #region .NET

      #region ChangeExtension (.NET)

      [TestMethod]
      public void NET_ChangeExtension()
      {
         Console.WriteLine("Path.ChangeExtension()");
         Console.WriteLine("\nThe .NET method is used.");
      }

      #endregion // ChangeExtension (.NET)

      #region Combine

      [TestMethod]
      public void Combine()
      {
         Console.WriteLine("Path.Combine()");

         int pathCnt = 0;
         int errorCnt = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            foreach (string path2 in UnitTestConstants.InputPaths)
            {
               string expected = null;
               string actual = null;

               Console.WriteLine("\n#{0:000}\tInput Path: [{1}]\tCombine with: [{2}]", ++pathCnt, path, path2);

               // System.IO
               try
               {
                  expected = System.IO.Path.Combine(path, path2);
               }
               catch (Exception ex)
               {
                  Console.WriteLine("\tCaught [System.IO] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               }
               Console.WriteLine("\tSystem.IO : [{0}]", expected ?? "null");


               // AlphaFS
               try
               {
                  actual = Path.Combine(path, path2);

                  Assert.AreEqual(expected, actual);
               }
               catch (Exception ex)
               {
                  errorCnt++;

                  Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               }
               Console.WriteLine("\tAlphaFS   : [{0}]", actual ?? "null");
            }
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "Encountered paths where AlphaFS != System.IO");
      }

      #endregion // Combine

      #region GetDirectoryName

      [TestMethod]
      public void GetDirectoryName()
      {
         Console.WriteLine("Path.GetDirectoryName()");

         int pathCnt = 0;
         int errorCnt = 0;
         bool skipAssert = false;

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // System.IO
            try
            {
               expected = System.IO.Path.GetDirectoryName(path);
            }
            catch (Exception ex)
            {
               skipAssert = ex is ArgumentException;

               Console.WriteLine("\tCaught [System.IO] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tSystem.IO : [{0}]", expected ?? "null");


            // AlphaFS
            try
            {
               actual = Path.GetDirectoryName(path);

               if (!skipAssert)
                  Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tAlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "Encountered paths where AlphaFS != System.IO");
      }

      #endregion // GetDirectoryName

      #region GetExtension

      [TestMethod]
      public void GetExtension()
      {
         Console.WriteLine("Path.GetExtension()");

         int pathCnt = 0;
         int errorCnt = 0;
         bool skipAssert = false;

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // System.IO
            try
            {
               expected = System.IO.Path.GetExtension(path);
            }
            catch (Exception ex)
            {
               skipAssert = ex is ArgumentException;

               Console.WriteLine("\tCaught [System.IO] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tSystem.IO : [{0}]", expected ?? "null");


            // AlphaFS
            try
            {
               actual = Path.GetExtension(path);

               if (!skipAssert)
                  Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tAlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "Encountered paths where AlphaFS != System.IO");
      }

      #endregion // GetExtension

      #region GetFileName

      [TestMethod]
      public void GetFileName()
      {
         Console.WriteLine("Path.GetFileName()");

         int pathCnt = 0;
         int errorCnt = 0;
         bool skipAssert = false;

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // System.IO
            try
            {
               expected = System.IO.Path.GetFileName(path);
            }
            catch (Exception ex)
            {
               skipAssert = ex is ArgumentException;

               Console.WriteLine("\tCaught [System.IO] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tSystem.IO : [{0}]", expected ?? "null");


            // AlphaFS
            try
            {
               actual = Path.GetFileName(path);

               if (!skipAssert)
                  Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tAlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "Encountered paths where AlphaFS != System.IO");
      }

      #endregion // GetFileName

      #region GetFileNameWithoutExtension

      [TestMethod]
      public void GetFileNameWithoutExtension()
      {
         Console.WriteLine("Path.GetFileNameWithoutExtension()");

         int pathCnt = 0;
         int errorCnt = 0;
         bool skipAssert = false;

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // System.IO
            try
            {
               expected = System.IO.Path.GetFileNameWithoutExtension(path);
            }
            catch (Exception ex)
            {
               skipAssert = ex is ArgumentException;

               Console.WriteLine("\tCaught [System.IO] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tSystem.IO : [{0}]", expected ?? "null");


            // AlphaFS
            try
            {
               actual = Path.GetFileNameWithoutExtension(path);

               if (!skipAssert)
                  Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tAlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "Encountered paths where AlphaFS != System.IO");
      }

      #endregion // GetFileNameWithoutExtension

      #region GetFullPath

      [TestMethod]
      public void GetFullPath()
      {
         Console.WriteLine("Path.GetFullPath()");

         int pathCnt = 0;
         int errorCnt = 0;
         bool skipAssert = false;

         #region Exceptions

         try
         {
            Path.GetFullPath(UnitTestConstants.SysDrive + @"\?test.txt");
            //Path.GetFullPath(UnitTestConstants.SysDrive + @"\*test.txt");
            //Path.GetFullPath(UnitTestConstants.SysDrive + @"\\test.txt");
            //Path.GetFullPath(UnitTestConstants.SysDrive + @"\/test.txt");
         }
         catch (ArgumentException)
         {
            skipAssert = true;
         }
         Assert.IsTrue(skipAssert, "ArgumentException should have been caught.");


         skipAssert = false;
         try
         {
            Path.GetFullPath(UnitTestConstants.SysDrive + @"\dev\test\aaa:aaa.txt");
         }
         catch (NotSupportedException)
         {
            skipAssert = true;
         }
         Assert.IsTrue(skipAssert, "NotSupportedException should have been caught.");


         skipAssert = false;
         try
         {
            Path.GetFullPath(@"\\\\.txt");
         }
         catch (ArgumentException)
         {
            skipAssert = true;
         }
         Assert.IsTrue(skipAssert, "ArgumentException should have been caught.");

         #endregion // Exceptions

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;
            skipAssert = false;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // System.IO
            try
            {
               expected = System.IO.Path.GetFullPath(path);
            }
            catch (Exception ex)
            {
               skipAssert = ex is ArgumentException;

               Console.WriteLine("\tCaught [System.IO] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tSystem.IO : [{0}]", expected ?? "null");


            // AlphaFS
            try
            {
               actual = Path.GetFullPath(path);

               if (!skipAssert)
                  Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tAlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "Encountered paths where AlphaFS != System.IO");
      }

      #endregion // GetFullPath

      #region GetInvalidFileNameChars (.NET)

      [TestMethod]
      public void NET_GetInvalidFileNameChars()
      {
         Console.WriteLine("Path.GetInvalidFileNameChars()");
         Console.WriteLine("\nThe .NET method is used.\n");

         UnitTestConstants.StopWatcher(true);
         foreach (char c in Path.GetInvalidFileNameChars())
            Console.WriteLine("\tChar: [{0}]", c);
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));
      }

      #endregion // GetInvalidFileNameChars (.NET)

      #region GetInvalidPathChars (.NET)

      [TestMethod]
      public void NET_GetInvalidPathChars()
      {
         Console.WriteLine("Path.GetInvalidPathChars()");
         Console.WriteLine("\nThe .NET method is used.\n");

         UnitTestConstants.StopWatcher(true);
         foreach (char c in Path.GetInvalidPathChars())
            Console.WriteLine("\tChar: [{0}]", c);
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));
      }

      #endregion // GetInvalidPathChars (.NET)

      #region GetPathRoot

      [TestMethod]
      public void GetPathRoot()
      {
         Console.WriteLine("Path.GetPathRoot()");

         int pathCnt = 0;
         int errorCnt = 0;
         bool skipAssert = false;

         #region Exceptions

         try
         {
            Path.GetPathRoot("");
         }
         catch (ArgumentException)
         {
            skipAssert = true;
         }
         Assert.IsTrue(skipAssert, "ArgumentException should have been caught.");

         #endregion // Exceptions

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            string expected = null;
            string actual = null;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // System.IO
            try
            {
               expected = System.IO.Path.GetPathRoot(path);

               skipAssert = path.StartsWith(Path.LongPathUncPrefix);
            }
            catch (Exception ex)
            {
               skipAssert = ex is ArgumentException;

               Console.WriteLine("\tCaught [System.IO] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tSystem.IO : [{0}]", expected ?? "null");


            // AlphaFS
            try
            {
               actual = Path.GetPathRoot(path);

               if (!skipAssert)
                  Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tAlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "Encountered paths where AlphaFS != System.IO");
      }

      #endregion // GetPathRoot

      #region GetRandomFileName (.NET)

      [TestMethod]
      public void NET_GetRandomFileName()
      {
         Console.WriteLine("Path.GetRandomFileName()");
         Console.WriteLine("\nThe .NET method is used.");
      }

      #endregion //GetRandomFileName (.NET)

      #region GetTempFileName (.NET)

      [TestMethod]
      public void NET_GetTempFileName()
      {
         Console.WriteLine("Path.GetTempFileName()");
         Console.WriteLine("\nThe .NET method is used.");
      }

      #endregion // GetTempFileName (.NET)

      #region GetTempPath (.NET)

      [TestMethod]
      public void NET_GetTempPath()
      {
         Console.WriteLine("Path.GetTempPath()");
         Console.WriteLine("\nThe .NET method is used.");
      }

      #endregion // GetTempPath (.NET)

      #region HasExtension (.NET)

      [TestMethod]
      public void NET_HasExtension()
      {
         Console.WriteLine("Path.HasExtension()");
         Console.WriteLine("\nThe .NET method is used.\n");

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            bool action = Path.HasExtension(path);
            Console.WriteLine("\tHasExtension: [{0}]\t\tInput Path: [{1}]", action, path);
            Assert.AreEqual(System.IO.Path.HasExtension(path), action);
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));
      }

      #endregion // HasExtension (.NET)

      #region IsPathRooted (.NET)

      [TestMethod]
      public void NET_IsPathRooted()
      {
         Console.WriteLine("Path.IsPathRooted()");
         Console.WriteLine("\nThe .NET method is used.\n");

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            bool action = Path.IsPathRooted(path);
            Console.WriteLine("\tIsPathRooted: [{0}]\t\tInput Path: [{1}]", action, path);
            Assert.AreEqual(System.IO.Path.IsPathRooted(path), action);
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));
      }

      #endregion // IsPathRooted (.NET)

      #endregion // .NET

      #region AlphaFS

      #region AddTrailingDirectorySeparator

      [TestMethod]
      public void AlphaFS_AddTrailingDirectorySeparator()
      {
         Console.WriteLine("Path.AddTrailingDirectorySeparator()\n");

         const string nonSlashedString = "SlashMe";
         Console.WriteLine("\tstring: [{0}];\n", nonSlashedString);

         // True, add DirectorySeparatorChar.
         string hasBackslash = Path.AddTrailingDirectorySeparator(nonSlashedString, false);
         bool addedBackslash = hasBackslash.EndsWith(Path.DirectorySeparatorChar.ToString()) && (nonSlashedString + Path.DirectorySeparatorChar).Equals(hasBackslash);
         Console.WriteLine("\tAddTrailingDirectorySeparator(string);\n\tAdded == [{0}]: {1}\n\tResult: [{2}]\n", UnitTestConstants.TextTrue, addedBackslash, hasBackslash);

         // True, add AltDirectorySeparatorChar.
         string hasSlash = Path.AddTrailingDirectorySeparator(nonSlashedString, true);
         bool addedSlash = hasSlash.EndsWith(Path.AltDirectorySeparatorChar.ToString(CultureInfo.CurrentCulture)) && (nonSlashedString + Path.AltDirectorySeparatorChar).Equals(hasSlash);
         Console.WriteLine("\tAddTrailingDirectorySeparator(string, true);\n\tAdded == [{0}]: {1}\n\tResult: [{2}]\n", UnitTestConstants.TextTrue, addedSlash, hasSlash);

         Assert.IsTrue(addedBackslash);
         Assert.IsTrue(addedSlash);
      }

      #endregion // AddTrailingDirectorySeparator

      #region GetDirectoryNameWithoutRoot

      [TestMethod]
      public void AlphaFS_GetDirectoryNameWithoutRoot()
      {
         Console.WriteLine("Path.GetDirectoryNameWithoutRoot()");

         // Note: since System.IO.Path does not have a similar method,
         // some more work is needs to test the validity of these result.

         DumpGetDirectoryNameWithoutRoot(true);
         DumpGetDirectoryNameWithoutRoot(false);
      }

      #endregion // GetDirectoryNameWithoutRoot

      #region GetFinalPathNameByHandle

      [TestMethod]
      public void AlphaFS_GetFinalPathNameByHandle()
      {
         Console.WriteLine("Path.GetFinalPathNameByHandle()");

         DumpGetFinalPathNameByHandle(true);
      }

      #endregion // GetFinalPathNameByHandle

      #region GetSuffixedDirectoryName

      [TestMethod]
      public void AlphaFS_GetSuffixedDirectoryName()
      {
         Console.WriteLine("Path.GetSuffixedDirectoryName()");

         // Note: since System.IO.Path does not have a similar method,
         // some more work is needs to test the validity of these result.

         DumpGetSuffixedDirectoryName(true);
         DumpGetSuffixedDirectoryName(false);
      }

      #endregion // GetSuffixedDirectoryName

      #region GetSuffixedDirectoryNameWithoutRoot

      [TestMethod]
      public void AlphaFS_GetSuffixedDirectoryNameWithoutRoot()
      {
         Console.WriteLine("Path.GetSuffixedDirectoryNameWithoutRoot()");

         // Note: since System.IO.Path does not have a similar method,
         // some more work is needed to test the validity of these result.

         DumpGetSuffixedDirectoryNameWithoutRoot(true);
         DumpGetSuffixedDirectoryNameWithoutRoot(false);
      }

      #endregion // GetSuffixedDirectoryNameWithoutRoot

      #region GetLongPath

      [TestMethod]
      public void AlphaFS_GetLongPath()
      {
         Console.WriteLine("Path.GetLongPath()");

         int pathCnt = 0;
         int errorCnt = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            string actual = null;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // AlphaFS
            try
            {
               actual = Path.GetLongPath(path);

               if (Path.IsUncPath(path))
                  Assert.IsTrue(actual.StartsWith(Path.LongPathUncPrefix), "Path should start with a long unc path prefix.");
               else
               {
                  var c = path[0];
                  if (!Path.IsPathRooted(path) && ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')))
                     Assert.IsFalse(actual.StartsWith(Path.LongPathPrefix), "Path should not start with a long path prefix.");
                  else
                  {
                     if (!Path.IsPathRooted(path) && !Utils.IsNullOrWhiteSpace(Path.GetDirectoryName(path)))
                        Assert.IsTrue(actual.StartsWith(Path.LongPathUncPrefix), "Path should start with a long path prefix.");
                  }
               }
            }
            catch (Exception ex)
            {
               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
               
               errorCnt++;
            }
            Console.WriteLine("\tAlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "No errors were expected.");
      }

      #endregion // GetLongPath

      #region GetLongFrom83ShortPath

      [TestMethod]
      public void AlphaFS_GetLongFrom83ShortPath()
      {
         Console.WriteLine("Path.GetLongFrom83ShortPath()");

         Dump83Path(true);
         Dump83Path(false);
      }

      #endregion // GetLongFrom83ShortPath

      #region GetMappedConnectionName

      [TestMethod]
      public void AlphaFS_GetMappedConnectionName()
      {
         Console.WriteLine("Path.GetMappedConnectionName()");

         int cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (string drive in Directory.GetLogicalDrives().Where(drive => new DriveInfo(drive).IsUnc))
         {
            ++cnt;

            UnitTestConstants.StopWatcher(true);
            string gmCn = Path.GetMappedConnectionName(drive);
            string gmUn = Path.GetMappedUncName(drive);
            Console.WriteLine("\n\tMapped drive: [{0}]\tGetMappedConnectionName(): [{1}]", drive, gmCn);
            Console.WriteLine("\tMapped drive: [{0}]\tGetMappedUncName()       : [{1}]", drive, gmUn);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(gmCn));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(gmUn));
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");
      }

      #endregion // GetMappedConnectionName

      #region GetRegularPath

      [TestMethod]
      public void AlphaFS_GetRegularPath()
      {
         Console.WriteLine("Path.GetRegularPath()");

         int pathCnt = 0;
         int errorCnt = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            string actual = null;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // AlphaFS
            try
            {
               actual = Path.GetRegularPath(path);

               Assert.IsFalse(actual.StartsWith(Path.LongPathPrefix, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tAlphaFS   : [{0}]", actual ?? "null");
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         Assert.AreEqual(0, errorCnt, "No errors were expected.");
      }

      #endregion // GetRegularPath

      #region IsLongPath

      [TestMethod]
      public void AlphaFS_IsLongPath()
      {
         Console.WriteLine("Path.IsLongPath()");

         int pathCnt = 0;
         int errorCnt = 0;
         int longPathCnt = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            bool actual = false;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // AlphaFS
            try
            {
               bool expected = path.StartsWith(Path.LongPathPrefix, StringComparison.OrdinalIgnoreCase);
               actual = Path.IsLongPath(path);

               Assert.AreEqual(expected, actual);

               if (actual)
                  longPathCnt++;
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tAlphaFS   : [{0}]", actual);
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         // Hand counted 33 True's.
         Assert.AreEqual(33, longPathCnt, "Number of local paths do not match.", errorCnt);

         Assert.AreEqual(0, errorCnt, "No errors were expected.");
      }

      #endregion // IsLongPath

      #region IsUncPath

      [TestMethod]
      public void AlphaFS_IsUncPath()
      {
         Console.WriteLine("Path.IsUncPath()");

         int pathCnt = 0;
         int errorCnt = 0;
         int uncPathCnt = 0;

         UnitTestConstants.StopWatcher(true);
         foreach (string path in UnitTestConstants.InputPaths)
         {
            bool actual = false;

            Console.WriteLine("\n#{0:000}\tInput Path: [{1}]", ++pathCnt, path);

            // AlphaFS
            try
            {
               bool expected = path.StartsWith(Path.UncPrefix, StringComparison.OrdinalIgnoreCase);

               actual = Path.IsUncPath(path);

               if (!(!path.StartsWith(Path.GlobalRootPrefix, StringComparison.OrdinalIgnoreCase) ||
                     !path.StartsWith(Path.VolumePrefix, StringComparison.OrdinalIgnoreCase)))
                  Assert.AreEqual(expected, actual);

               if (actual)
                  uncPathCnt++;
            }
            catch (Exception ex)
            {
               errorCnt++;

               Console.WriteLine("\tCaught [AlphaFS] {0}: [{1}]", ex.GetType().FullName, ex.Message.Replace(Environment.NewLine, "  "));
            }
            Console.WriteLine("\tAlphaFS   : [{0}]", actual);
         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         // Hand counted 32 True's.
         Assert.AreEqual(32, uncPathCnt, "Number of UNC paths do not match.", errorCnt);

         Assert.AreEqual(0, errorCnt, "No errors were expected.");
      }

      #endregion // IsUncPath

      #region LocalToUnc

      [TestMethod]
      public void AlphaFS_LocalToUnc()
      {
         Console.WriteLine("Path.LocalToUnc()");

         Console.WriteLine("\nInput Path: [{0}]\n", UnitTestConstants.SysRoot);

         int cnt = 0;
         UnitTestConstants.StopWatcher(true);
         foreach (string path2 in Directory.EnumerateFileSystemEntries(UnitTestConstants.SysRoot))
         {
            string uncPath = Path.LocalToUnc(path2);
            Console.WriteLine("\t#{0:000}\tLocal: [{1}]\t\t\tUNC: [{2}]", ++cnt, path2, uncPath);

            Assert.IsTrue(Path.IsUncPath(uncPath));

         }
         Console.WriteLine("\n{0}", UnitTestConstants.Reporter(true));

         if (cnt == 0)
            Assert.Inconclusive("Nothing was enumerated.");

         Console.WriteLine();
      }

      #endregion // LocalToUnc

      #region RemoveTrailingDirectorySeparator

      [TestMethod]
      public void AlphaFS_RemoveTrailingDirectorySeparator()
      {
         Console.WriteLine("Path.RemoveTrailingDirectorySeparator()\n");

         const string backslashedString = @"Backslashed\";
         const string slashedString = "Slashed/";
         // True, add DirectorySeparatorChar.
         string hasBackslash = Path.RemoveTrailingDirectorySeparator(backslashedString);
         bool removedBackslash = !hasBackslash.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.CurrentCulture)) && !backslashedString.Equals(hasBackslash);
         Console.WriteLine("\tstring = @[{0}];\n", backslashedString);
         Console.WriteLine("\tRemoveTrailingDirectorySeparator(string);\n\tRemoved == [{0}]: {1}\n\tResult: [{2}]\n", UnitTestConstants.TextTrue, removedBackslash, hasBackslash);

         // True, add AltDirectorySeparatorChar.
         string hasSlash = Path.RemoveTrailingDirectorySeparator(slashedString, true);
         bool removedSlash = !hasSlash.EndsWith(Path.AltDirectorySeparatorChar.ToString(CultureInfo.CurrentCulture)) && !slashedString.Equals(hasSlash);
         Console.WriteLine("\tstring: [{0}];\n", slashedString);
         Console.WriteLine("\tRemoveTrailingDirectorySeparator(string, true);\n\tRemoved == [{0}]: {1}\n\tResult: [{2}]\n", UnitTestConstants.TextTrue, removedSlash, hasSlash);

         Assert.IsTrue(removedBackslash);
         Assert.IsTrue(removedSlash);
      }

      #endregion // RemoveTrailingDirectorySeparator

      #endregion // AlphaFS

      #endregion // Unit Test Callers
   }
}
