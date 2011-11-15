#define SHOWCHANNEL
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;

// type to print text to console and a log file
public static class PrintHelper
{
    private static Object s_syncLock = new Object(); // object to lock on
    private static Int32 counter = 0; // the line number

    static PrintHelper() {

        // Open a FileStream to write output to
        FileStream fs = new FileStream("DelegatorChannelOutput.txt", FileMode.Create);
        // point a trace listener to the file
        TraceListener listener = new TextWriterTraceListener(fs);        
        Trace.Listeners.Add(listener);
        Trace.AutoFlush = true;
    }

    public static void Print(String typeName, String memberName)
    {
        // Print the text as an atomic unit
        lock (s_syncLock) {
            counter++;
            String output = String.Format("{3}. {0}.{1}, Thread:{2}", typeName, memberName, Thread.CurrentThread.ManagedThreadId, counter);
            PrintInternal(output);
        }
    }

    public static void Print(String message) {
        // Print the text as an atomic unit
        lock (s_syncLock) {
            counter++;
            String output = String.Format("{0}. {1}, Thread:{2}", counter, message, Thread.CurrentThread.ManagedThreadId, counter);
            PrintInternal(output);
        }
    }

    public static void PrintNewLine() {
        lock (s_syncLock) {
            PrintInternal(Environment.NewLine);
        }
    }

    public static void PrintNoThread(String message) {
        lock (s_syncLock) {
            counter++;
            PrintInternal(message);
            
        }
    }

    private static void PrintInternal(String output) {
        // the lock is taken by one of the public Print methods so no lock is necessary
        Console.WriteLine(output);
        Trace.WriteLine(output);
    }
}

