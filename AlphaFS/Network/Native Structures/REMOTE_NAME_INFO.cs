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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Native.IO.Network
{
   internal static partial class NativeMethods
   {
      ///<summary>Contains path and name information for a network resource.</summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal struct REMOTE_NAME_INFO
      {
         /// <summary>Identifies a network resource.</summary>
         [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")] [MarshalAs(UnmanagedType.LPWStr)] public string lpUniversalName;

         /// <summary>The name of a network connection.</summary>
         [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")] [MarshalAs(UnmanagedType.LPWStr)] public string lpConnectionName;

         /// <summary>The remaing path.</summary>
         [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")] [MarshalAs(UnmanagedType.LPWStr)] public string lpRemainingPath;
      }
   }
}
