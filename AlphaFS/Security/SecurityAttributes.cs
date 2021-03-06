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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace System.Native.IO.Security
{
   internal static partial class NativeMethods
   {
      /// <summary>Class used to represent the SECURITY_ATTRIBUES native Win32 structure. It provides initialization function from an <see cref="ObjectSecurity"/> object.</summary>
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      internal sealed class SecurityAttributes : IDisposable
      {
         [MarshalAs(UnmanagedType.U4)]
         private int nLength;

         private readonly SafeGlobalMemoryBufferHandle lpSecurityDescriptor;

         public SecurityAttributes(ObjectSecurity securityDescriptor)
         {
            var safeBuffer = ToUnmanagedSecurityAttributes(securityDescriptor);
            nLength = safeBuffer.Capacity;
            lpSecurityDescriptor = safeBuffer;
         }

         /// <summary>
         /// Marshals an ObjectSecurity instance to unmanaged memory.
         /// </summary>
         /// <param name="securityDescriptor">The security descriptor.</param>
         /// <returns>A safe handle containing the marshalled security descriptor.</returns>
         [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
         private static SafeGlobalMemoryBufferHandle ToUnmanagedSecurityAttributes(ObjectSecurity securityDescriptor)
         {
            if (securityDescriptor == null)
               return new SafeGlobalMemoryBufferHandle();
            
            
            byte[] src = securityDescriptor.GetSecurityDescriptorBinaryForm();
            var safeBuffer = new SafeGlobalMemoryBufferHandle(src.Length);
            try
            {
               safeBuffer.CopyFrom(src, 0, src.Length);
               return safeBuffer;
            }
            catch
            {
               safeBuffer.Close();
               throw;
            }
         }

         public void Dispose()
         {
            if (lpSecurityDescriptor != null)
               lpSecurityDescriptor.Close();
         }
      }
   }
}